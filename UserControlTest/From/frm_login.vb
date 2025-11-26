Option Strict On
Option Explicit On
Imports System.Drawing
Imports System.Globalization
Imports System.Configuration

Public Class frm_login
    Public Declare Function GetSystemDefaultLCID Lib "kernel32" () As Int32
    Public Const LOCALE_SLANGUAGE As Long = &H2
    Public Const LOCALE_SSHORTDATE As Long = &H1F
    Public Const LOCALE_SLONGDATE As Long = &H20
    Public Const LOCALE_STIMEFORMAT = &H1003
    Public Const LOCALE_ICALENDARTYPE = &H1009
    Public Const DATE_LONGDATE As Long = &H2
    Public Const DATE_SHORTDATE As Long = &H1
    Public Const LOCALE_USE_CP_ACP = &H40000000
    Public Const HWND_BROADCAST As Long = &HFFFF&
    Public Const WM_SETTINGCHANGE As Long = &H1A

    Private blAuto_Production As String
    Private dtProductionDate_Act As String

    Private ReadOnly CnBatching As New clsDB(
        Batching_Conf.Name,
        Batching_Conf.User,
        Batching_Conf.Password,
        Batching_Conf.IPAddress,
        Batching_Conf.Connection_Type
    )

    Private ReadOnly CnDevCenter As New clsDB(
        Dev_Center_Conf.Name,
        Dev_Center_Conf.User,
        Dev_Center_Conf.Password,
        Dev_Center_Conf.IPAddress,
        Dev_Center_Conf.Connection_Type
    )

    Private dev_operator_code As String
    Private dev_operator_name As String
    Private dev_employee As String
    Private _showingPassword As Boolean = False

    '========================= LOGIN PERMISSION =========================
    'UPDATED BY: NATTHAPHONG (refactor to FD_Login_Permission) 22/10/2025

    Private Sub frm_login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' ===== Show/Hide password: PictureBox instead of Button =====
            ' ใส่ picShowHide ลงในฟอร์มผ่าน Designer แล้วตั้ง Cursor=Hand, BorderStyle=None
            picShowHide.Image = Project.My.Resources.Resources.UI_EyeClose_32x32
            picShowHide.SizeMode = PictureBoxSizeMode.Zoom
            picShowHide.Visible = (txtPassword.TextLength > 0)
            picShowHide.BringToFront()

            ' ให้พื้นหลังของไอคอนกลืนไปกับ Parent (กันแถบสีเพี้ยนเวลา Hover)
            Dim parentColor As Color = If(picShowHide.Parent IsNot Nothing, picShowHide.Parent.BackColor, SystemColors.Control)
            picShowHide.BackColor = parentColor
            If picShowHide.Parent IsNot Nothing Then
                AddHandler picShowHide.Parent.BackColorChanged, AddressOf SyncShowHideBackColor
            Else
                AddHandler picShowHide.HandleCreated,
                    Sub()
                        If picShowHide.Parent IsNot Nothing Then
                            SyncShowHideBackColor(picShowHide.Parent, EventArgs.Empty)
                            AddHandler picShowHide.Parent.BackColorChanged, AddressOf SyncShowHideBackColor
                        End If
                    End Sub
            End If

            ' ===== Password box defaults =====
            txtPassword.UseSystemPasswordChar = True
            _showingPassword = False

            Try
                Me.Icon = My.Resources.SmartFeedmill
            Catch
            End Try

            dtp_date.Enabled = False

            Dim sqlStrs As String =
                "SELECT c_autoupdate, c_production_date " &
                "FROM thaisia.current_status WHERE c_location LIKE 'BATCHING%'"

            Dim rss As DataTable = Nothing
            Try
                rss = CnBatching.ExecuteDataTable(sqlStrs)
            Catch
                ShowMessage("Cannot load current status from database.", Color.Red)
            End Try

            If rss IsNot Nothing AndAlso rss.Rows.Count > 0 Then
                Dim autoUpdate As String = If(IsDBNull(rss.Rows(0)("c_autoupdate")), "", rss.Rows(0)("c_autoupdate").ToString())
                Dim productionDate As String = If(IsDBNull(rss.Rows(0)("c_production_date")), "", rss.Rows(0)("c_production_date").ToString())

                cmdChange.Enabled = (autoUpdate = "M")

                Dim d As DateTime
                If DateTime.TryParse(productionDate, d) Then
                    dtp_date.Text = d.ToString("dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
                Else
                    dtp_date.Text = Date.Today.ToString("dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
                End If
            Else
                cmdChange.Enabled = False
                dtp_date.Text = Date.Today.ToString("dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
            End If

            txtUserName.Select()

        Catch ex As Exception
            ShowMessage("Unexpected error while initializing: " & ex.Message, Color.Red)
        End Try
    End Sub

    ' READ FLAG LoginFailed_Count FROM App.config
    Private Function ReadLoginFailedLimit() As Integer?
        Dim raw As String = ConfigurationManager.AppSettings("LoginFailed_Count")
        If String.IsNullOrWhiteSpace(raw) Then Return Nothing
        raw = raw.Trim()
        If String.Equals(raw, "N", StringComparison.OrdinalIgnoreCase) Then Return Nothing
        Dim n As Integer
        If Integer.TryParse(raw, n) AndAlso n > 0 Then Return n
        Return Nothing
    End Function

    Private Sub cmdOK_Core()
        ' connection Dev Center
        If CnDevCenter Is Nothing AndAlso Not String.IsNullOrWhiteSpace(Dev_Center_Conf.Name) AndAlso Dev_Center_Conf.Name.ToLower() <> "error" Then
        End If

        Try
            dtProductionDate_Act = Nothing

            ' 1) VALIDATE INPUT
            If String.IsNullOrWhiteSpace(txtUserName.Text) OrElse String.IsNullOrWhiteSpace(txtPassword.Text) Then
                ShowMessage("Please enter both username and password.", Color.Red)
                txtUserName.Focus()
                Exit Sub
            End If

            ' 2) STORED PROCEDURE: thaisia.FD_Login_Permission
            Dim rs As DataTable = Nothing
            Dim limit As Integer? = ReadLoginFailedLimit()

            Try
                CnBatching.ClearPara()
                CnBatching.AddPara("@UserName", txtUserName.Text.Trim())
                CnBatching.AddPara("@Password", txtPassword.Text.Trim())
                If limit.HasValue AndAlso limit.Value > 0 Then
                    CnBatching.AddPara("@MaxFailedAttempts", limit.Value)
                Else
                    CnBatching.AddPara("@MaxFailedAttempts", DBNull.Value)
                End If

                ' DYNAMIC NAME dev_center.dbo.web_users
                If Not String.IsNullOrWhiteSpace(Dev_Center_Conf.Name) AndAlso Dev_Center_Conf.Name.ToLower() <> "error" Then
                    CnBatching.AddPara("@ExtDb", Dev_Center_Conf.Name)
                Else
                    CnBatching.AddPara("@ExtDb", DBNull.Value)
                End If

                rs = CnBatching.SelectTableProcedure("thaisia.FD_Login_Permission")
            Catch ex As Exception
                ShowMessage("Cannot validate login (SP error): " & ex.Message, Color.Red)
                Exit Sub
            End Try

            If rs Is Nothing OrElse rs.Rows.Count = 0 Then
                ShowMessage("Unexpected login response.", Color.Red)
                Exit Sub
            End If

            ' 3) RESULT OF STORED PROCEDURE
            Dim r As DataRow = rs.Rows(0)
            Dim ok As Boolean = ToBool(r("success"))
            Dim code As String = If(IsDBNull(r("code")), "", r("code").ToString())
            Dim msg As String = If(IsDBNull(r("message")), "", r("message").ToString())

            Select Case code
                Case "USER_NOT_FOUND"
                    ShowMessage(msg, Color.Red)
                    txtUserName.Focus()
                    txtUserName.SelectAll()
                    Exit Sub

                Case "LOCKED"
                    ShowMessage(msg, Color.Red)
                    Exit Sub

                Case "TOO_MANY_FAILS"
                    ShowMessage(msg, Color.Red)
                    txtPassword.SelectAll() : txtPassword.Focus()
                    Exit Sub

                Case "WRONG_PASS"
                    Dim leftTry As Integer
                    If Not IsDBNull(r("attempts_left")) AndAlso Integer.TryParse(r("attempts_left").ToString(), leftTry) Then
                        ShowMessage(msg & If(leftTry >= 0, " " & leftTry.ToString() & " attempts remaining.", ""), Color.Red)
                        ClearTextInputs()
                    Else
                        ShowMessage(msg, Color.Red)
                    End If
                    txtPassword.SelectAll() : txtPassword.Focus()
                    Exit Sub

                Case "EXPIRED"
                    Dim expStr As String = ""
                    If Not IsDBNull(r("d_date_exp")) Then
                        Dim dx As DateTime
                        If DateTime.TryParse(r("d_date_exp").ToString(), dx) Then
                            expStr = dx.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                        End If
                    End If
                    ShowMessage("User account has expired on " & expStr & ". Please contact administrator.", Color.Red)
                    Exit Sub

                Case "OK", "OK_EXP_SOON"

                Case Else
                    If Not ok Then
                        ShowMessage(If(String.IsNullOrEmpty(msg), "Login failed.", msg), Color.Red)
                        Exit Sub
                    End If
            End Select

            ' 4) SUCCESS AND WARNING EXPIRING SOON
            dev_operator_code = txtUserName.Text.Trim()
            dev_operator_name = If(IsDBNull(r("c_name")), "", r("c_name").ToString())
            dev_employee = If(IsDBNull(r("c_employee_id")), "", r("c_employee_id").ToString())

            If code = "OK_EXP_SOON" Then
                Dim daysLeft As Integer
                Dim expDateStr As String = ""

                If Not IsDBNull(r("d_date_exp")) Then
                    Dim expDate As DateTime
                    If DateTime.TryParse(r("d_date_exp").ToString(), expDate) Then
                        expDateStr = expDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)
                    End If
                End If

                If Not IsDBNull(r("days_to_expiry")) AndAlso Integer.TryParse(r("days_to_expiry").ToString(), daysLeft) Then

                    Dim warningMsg As String = "Warning: Your account will expire in " & daysLeft.ToString() & " day" & If(daysLeft > 1, "s", "")
                    If Not String.IsNullOrEmpty(expDateStr) Then
                        warningMsg &= " (Expiry Date: " & expDateStr & ")"
                    End If
                    warningMsg &= "."

                    ShowMessage(warningMsg, Color.Orange)

                    Dim ackMsg As String = "Your account will expire soon"
                    If Not String.IsNullOrEmpty(expDateStr) Then
                        ackMsg &= " on " & expDateStr
                    End If
                    ackMsg &= ". Please contact administrator to renew."

                    If Not RequireAcknowledge(ackMsg, "Account Expiring Soon") Then
                        Exit Sub
                    End If
                End If
            Else
                ShowMessage("Login successful. Welcome, " & dev_operator_name & ".", Color.Green)
            End If

            ' 5) HARDLOCK
            Try
                If StrMqtt_Config_.HardLock Then
                    If MDI_FRM.CheckHasp.Hardlock() <> 0 Then
                        ShowMessage("Hardlock Not Found," & Environment.NewLine &
                                "Please check and make sure it is installed properly on the USB port.", Color.Red)
                        Exit Sub
                    End If
                End If
            Catch ex As Exception
                ShowMessage("Cannot verify hardlock: " & ex.Message, Color.Red)
                Exit Sub
            End Try

            ' 6) PERMISSION OF MDI_FRM
            Try
                With UserLogon_
                    .UserCode = dev_operator_code
                    .UserName = dev_operator_name
                    .EmployeeId = dev_employee
                End With

                Dim mdi = MDI_FRM.Current()
                If mdi IsNot Nothing Then
                    mdi.ApplyPermissions(
                        ToBool(r("can_production_time")),
                        ToBool(r("can_job_assignment")),
                        ToBool(r("can_main_scada")),
                        ToBool(r("can_alarm")),
                        ToBool(r("can_route")),
                        ToBool(r("can_start_batch"))
                    )
                    mdi.IsAuthenticated = True
                    mdi.Is_Logon()
                End If

                CompleteLogin()
            Catch ex As Exception
                ShowMessage("Error while applying permissions: " & ex.Message, Color.Red)
                Exit Sub
            End Try

        Catch ex As Exception
            ShowMessage("Unexpected error while logging in: " & ex.Message, Color.Red)
        End Try
    End Sub

    ' ===== Button/Event Handlers =====
    Private Sub cmdOK_Click()
        cmdOK_Core()
    End Sub

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        cmdOK_Core()
    End Sub

    Private Sub txt_KeyDown(sender As Object, e As KeyEventArgs) Handles txtUserName.KeyDown, txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            e.SuppressKeyPress = True
            cmdOK_Core()
        End If
    End Sub

    ' เปลี่ยนจาก Button → PictureBox: แก้ handler ตรงนี้
    Private Sub picShowHide_Click(sender As Object, e As EventArgs) Handles picShowHide.Click
        Dim selStart As Integer = txtPassword.SelectionStart

        _showingPassword = Not _showingPassword
        txtPassword.UseSystemPasswordChar = Not _showingPassword

        If _showingPassword Then
            picShowHide.Image = Project.My.Resources.Resources.UI_EyeOpen_32x32
        Else
            picShowHide.Image = Project.My.Resources.Resources.UI_EyeClose_32x32
        End If
        picShowHide.SizeMode = PictureBoxSizeMode.Zoom

        txtPassword.SelectionStart = Math.Min(selStart, txtPassword.TextLength)
        txtPassword.SelectionLength = 0
    End Sub

    Private Sub txtPassword_TextChanged(sender As Object, e As EventArgs) Handles txtPassword.TextChanged
        picShowHide.Visible = (txtPassword.TextLength > 0)
    End Sub

    ' ===== Utils =====
    Private Sub ClearTextInputs()
        If txtUserName.InvokeRequired Then
            txtUserName.Invoke(New Action(AddressOf ClearTextInputs))
        Else
            txtUserName.Text = ""
            txtPassword.Text = ""
        End If
    End Sub

    Private Sub ShowMessage(message As String, color As Color)
        If LabelMessage.InvokeRequired Then
            LabelMessage.Invoke(New Action(Of String, Color)(AddressOf ShowMessage), message, color)
        Else
            LabelMessage.Visible = True
            LabelMessage.ForeColor = color
            LabelMessage.Text = message
        End If
    End Sub

    Private Sub CompleteLogin()
        Try
            Me.Enabled = False
            Application.DoEvents()

            With UserLogon_
                .UserCode = dev_operator_code
                .UserName = dev_operator_name
                .EmployeeId = dev_employee
            End With

            If Me.Modal Then
                Me.DialogResult = DialogResult.OK
            Else
                Me.Hide()
            End If

            If Not Me.IsDisposed AndAlso Me.Visible Then
                Me.Close()
            End If

        Catch ex As Exception
            Me.Enabled = True
            ShowMessage("Error completing login: " & ex.Message, Color.Red)
        End Try
    End Sub

    Private Function RequireAcknowledge(message As String, Optional title As String = "Login") As Boolean
        Try
            Dim f As New Form With {
                .Text = title,
                .StartPosition = FormStartPosition.CenterParent,
                .FormBorderStyle = FormBorderStyle.FixedDialog,
                .MinimizeBox = False, .MaximizeBox = False,
                .ShowInTaskbar = False, .ClientSize = New Size(420, 180)
            }

            Dim fontCustom As New Font("Segoe UI", 10.0F, FontStyle.Regular)

            Dim lbl As New Label With {
                .AutoSize = False, .Text = message,
                .Location = New Point(12, 12), .Size = New Size(396, 90),
                .Font = fontCustom
            }

            Dim chk As New CheckBox With {.Text = "I accept", .Location = New Point(12, 110), .Font = fontCustom}

            Dim btnOk As New Button With {
                .Text = "Continue", .DialogResult = DialogResult.OK, .Enabled = False,
                .Size = New Size(110, 28),
                .Location = New Point(f.ClientSize.Width - 122, f.ClientSize.Height - 36),
                .Anchor = AnchorStyles.Bottom Or AnchorStyles.Right, .Font = fontCustom
            }

            AddHandler chk.CheckedChanged, Sub() btnOk.Enabled = chk.Checked
            f.Controls.AddRange({lbl, chk, btnOk})
            f.AcceptButton = btnOk

            Return f.ShowDialog(Me) = DialogResult.OK

        Catch
            Return False
        End Try
    End Function

    Private Function ToBool(v As Object) As Boolean
        If v Is Nothing OrElse IsDBNull(v) Then Return False
        If TypeOf v Is Boolean Then Return CBool(v)
        Dim s As String = v.ToString().Trim()
        Select Case s.ToUpperInvariant()
            Case "1", "T", "Y", "TRUE", "YES", "ON" : Return True
            Case "0", "F", "N", "FALSE", "NO", "OFF", "" : Return False
        End Select
        Dim n As Integer
        If Integer.TryParse(s, n) Then Return n <> 0
        Return False
    End Function

    Private Sub SyncShowHideBackColor(sender As Object, e As EventArgs)
        If picShowHide Is Nothing OrElse picShowHide.Parent Is Nothing Then Exit Sub
        Dim parentColor As Color = picShowHide.Parent.BackColor
        picShowHide.BackColor = parentColor
    End Sub

    ' ===== UI niceties =====
    Private Sub cmdChange_Click(sender As Object, e As EventArgs) Handles cmdChange.Click
        dtp_date.Enabled = True
    End Sub

    Private Sub btClose_Click(sender As Object, e As EventArgs) Handles btClose.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        If Me.DialogResult = DialogResult.None Then
            Me.DialogResult = DialogResult.Cancel
        End If
        MyBase.OnFormClosing(e)
    End Sub

    Private Sub txtUserName_Click(sender As Object, e As EventArgs) Handles txtUserName.Click
        txtUserName.BackColor = Color.White
        Panel3.BackColor = Color.White
        Panel4.BackColor = SystemColors.Control
        txtPassword.BackColor = SystemColors.Control
    End Sub

    Private Sub txtPassword_Click(sender As Object, e As EventArgs) Handles txtPassword.Click, dtp_date.Click
        txtPassword.BackColor = Color.White
        Panel4.BackColor = Color.White
        txtUserName.BackColor = SystemColors.Control
        Panel3.BackColor = SystemColors.Control
    End Sub
End Class