Imports System.Drawing
Imports System.Globalization
Imports System.Runtime.Remoting.Lifetime
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
    Dim blAuto_Production As String
    Public Const HWND_BROADCAST As Long = &HFFFF&
    Public Const WM_SETTINGCHANGE As Long = &H1A
    Dim dtProductionDate_Act As String

    Dim Cn As New clsDB(Batching_Conf.Name, Batching_Conf.User, Batching_Conf.Password, Batching_Conf.IPAddress, Batching_Conf.Connection_Type)
    Dim dt As DataTable
    Dim dev_operator_code As String
    Dim dev_operator_name As String
    Dim dev_employee As String

    '========================= LOGIN PERMISSION ========================
    'UPDATED BY: NATTHAPHONG 23/09/2025

    Private Sub frm_login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.Icon = My.Resources.SmartFeedmill
            dtp_date.Enabled = False

            Dim sqlStrs As String =
            "SELECT thaisia.current_status.c_autoupdate, thaisia.current_status.c_production_date " &
            "FROM thaisia.current_status WHERE thaisia.current_status.c_location LIKE 'BATCHING%'"

            Dim rss As DataTable = Nothing
            Try
                rss = Cn.ExecuteDataTable(sqlStrs)
            Catch ex As Exception
                LabelMessage.Visible = True
                LabelMessage.ForeColor = Color.Red
                LabelMessage.Text = "Cannot load current status from database."
            End Try

            If rss IsNot Nothing AndAlso rss.Rows.Count > 0 Then
                Dim autoUpdate As String = (rss.Rows(0)("c_autoupdate") & "").ToString()
                Dim productionDate As String = (rss.Rows(0)("c_production_date") & "").ToString()

                cmdChange.Enabled = (autoUpdate = "M")

                Dim d As DateTime
                If DateTime.TryParse(productionDate, d) Then
                    dtp_date.Text = d
                Else
                    dtp_date.Text = Date.Today
                End If
            Else
                cmdChange.Enabled = False
                dtp_date.Text = Date.Today
            End If

            txtUserName.Select()

        Catch ex As Exception
            LabelMessage.Visible = True
            LabelMessage.ForeColor = Color.Red
            LabelMessage.Text = "Unexpected error while initializing."
        End Try
    End Sub

    Private Sub cmdOK_Click()
        Try
            Dim rs As DataTable = Nothing
            Dim dev_expiredate As String = Nothing
            dtProductionDate_Act = Nothing

            ' 1) Validate input
            If String.IsNullOrWhiteSpace(txtUserName.Text) OrElse String.IsNullOrWhiteSpace(txtPassword.Text) Then
                ShowMessage("Please enter both username and password.", Color.Red)
                txtUserName.Focus()
                Exit Sub
            End If

            ' 2) Find user in DB
            Dim dtUser As DataTable = Nothing
            Try
                Cn.ClearPara()
                Cn.AddPara("@UserName", Trim(txtUserName.Text))
                dtUser = Cn.ExecuteDataTable("
                    SELECT c_user_code, c_login, c_password, c_active, ISNULL(n_login_pass_failed,0) AS n_login_pass_failed
                    FROM thaisia.s_user
                    WHERE c_login = @UserName")
            Catch ex As Exception
                ShowMessage("Cannot query user info: " & ex.Message, Color.Red)
                Exit Sub
            End Try

            If dtUser Is Nothing OrElse dtUser.Rows.Count = 0 Then
                ShowMessage("User not found. Please try another account.", Color.Red)
                txtUserName.Focus()
                txtUserName.SelectAll()
                Exit Sub
            End If

            Dim u = dtUser.Rows(0)
            Dim dbActive As String = If(IsDBNull(u("c_active")), "", u("c_active").ToString().Trim())
            Dim failCount As Integer = 0
            Try : failCount = Convert.ToInt32(u("n_login_pass_failed")) : Catch : failCount = 0 : End Try

            If String.Equals(dbActive, "N", StringComparison.OrdinalIgnoreCase) Then
                ShowMessage("This account is locked. Please contact administrator.", Color.Red)
                Exit Sub
            End If

            ' 3) Validate password via SP
            Try
                Cn.ClearPara()
                Cn.AddPara("@UserName", Trim(txtUserName.Text))
                Cn.AddPara("@Password", Trim(txtPassword.Text))
                rs = Cn.SelectTableProcedure("thaisia.SCADA_Login_Permission")
            Catch ex As Exception
                ShowMessage("Cannot validate login (SP error): " & ex.Message, Color.Red)
                Exit Sub
            End Try

            Dim isSuccess As Boolean =
            (rs IsNot Nothing AndAlso rs.Rows.Count > 0 AndAlso
             Not IsDBNull(rs.Rows(0)("c_user_code")) AndAlso
             Not String.IsNullOrWhiteSpace(rs.Rows(0)("c_user_code").ToString()))

            If Not isSuccess Then
                ' (FAIL PATH)
                failCount += 1
                Try
                    Cn.ClearPara()
                    Cn.AddPara("@UserName", Trim(txtUserName.Text))
                    Cn.AddPara("@Fail", failCount)
                    Cn.ExecuteDataTable("UPDATE thaisia.s_user SET n_login_pass_failed=@Fail WHERE c_login=@UserName")
                Catch ex As Exception
                    MsgBox("Cannot update fail count: " & ex.Message)
                End Try

                If failCount >= 3 Then
                    Try
                        Cn.ClearPara()
                        Cn.AddPara("@UserName", Trim(txtUserName.Text))
                        Cn.ExecuteDataTable("UPDATE thaisia.s_user SET c_active='N' WHERE c_login=@UserName")
                    Catch ex As Exception
                        MsgBox("Cannot lock account: " & ex.Message)
                    End Try

                    ShowMessage("Your account has been locked due to 3 failed login attempts." &
                            Environment.NewLine & "Please contact administrator.", Color.Red)
                Else
                    ShowMessage("Invalid password. " & (3 - failCount).ToString() & " attempts remaining.", Color.Red)
                End If

                txtPassword.Focus()
                txtPassword.SelectAll()
                Exit Sub
            End If

            ' 4) Success: reset fail count
            Try
                Cn.ClearPara()
                Cn.AddPara("@UserName", Trim(txtUserName.Text))
                Cn.ExecuteDataTable("UPDATE thaisia.s_user SET n_login_pass_failed=0 WHERE c_login=@UserName")
            Catch ex As Exception
                MsgBox("Cannot reset fail count: " & ex.Message)
            End Try

            ' 5) Read fields from SP
            Dim r = rs.Rows(0)
            dev_operator_code = Trim(txtUserName.Text)
            dev_operator_name = If(IsDBNull(r("c_name")), "", r("c_name").ToString())
            dev_expiredate = If(IsDBNull(r("d_date_exp")), "", r("d_date_exp").ToString())
            dev_employee = If(IsDBNull(r("c_employee_id")), "", r("c_employee_id").ToString())

            ' 6) Hardlock validation
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

            ' 7) Check expiration date
            Try
                If Not String.IsNullOrWhiteSpace(dev_expiredate) Then
                    Dim expireDate As DateTime
                    If DateTime.TryParse(dev_expiredate, expireDate) Then
                        If expireDate < DateTime.Now.Date Then
                            ShowMessage("User account has expired on " & expireDate.ToString("dd/MM/yyyy") & "." &
                                    Environment.NewLine & "Please contact administrator.", Color.Red)
                            Exit Sub
                        ElseIf expireDate <= DateTime.Now.Date.AddDays(14) Then
                            ShowMessage("Warning: Your account will expire on " & expireDate.ToString("dd/MM/yyyy") &
                                    " (" & (expireDate - DateTime.Now.Date).Days & " days remaining). Please contact administrator to renew your account.", Color.Orange)

                            Dim msg As String =
                            "WARNING !" & Environment.NewLine &
                            "Your account will expire on " & expireDate.ToString("dd/MM/yyyy") &
                            " (" & (expireDate - DateTime.Now.Date).Days & " days remaining)." & Environment.NewLine &
                            "Please contact administrator to renew your account."

                            If Not RequireAcknowledge(msg, "Account Expiring Soon") Then
                                Exit Sub
                            End If
                        Else
                            ShowMessage("Login successful. Welcome, " & dev_operator_name & ".", Color.Green)
                        End If
                    Else
                        ShowMessage("Invalid expiration date format." & Environment.NewLine & "Please contact administrator.", Color.Red)
                        Exit Sub
                    End If
                Else
                    ShowMessage("Login successful. Welcome, " & dev_operator_name & ".", Color.Green)
                End If
            Catch ex As Exception
                ShowMessage("Error while validating expiration date: " & ex.Message, Color.Red)
                Exit Sub
            End Try

            ' 8) Send the permission to MDI.
            Try
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
            Exit Sub
        End Try
    End Sub

    Private Sub ShowMessage(message As String, color As Color)
        LabelMessage.Visible = True
        LabelMessage.ForeColor = color
        LabelMessage.Text = message
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

            If Me.Visible Then
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

        Catch ex As Exception
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

    Private Sub cmdChange_Click(sender As Object, e As EventArgs) Handles cmdChange.Click
        dtp_date.Enabled = True
    End Sub

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        cmdOK_Click()
    End Sub

    Private Sub txtUserName_KeyDown(sender As Object, e As KeyEventArgs) Handles txtUserName.KeyDown, txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            cmdOK_Click()
        End If
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

    Private Sub PictureBox3_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox3.MouseDown
        txtPassword.UseSystemPasswordChar = False
    End Sub

    Private Sub PictureBox3_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox3.MouseUp
        txtPassword.UseSystemPasswordChar = True
    End Sub
End Class