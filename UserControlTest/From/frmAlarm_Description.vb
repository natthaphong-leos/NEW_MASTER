Imports System.Globalization
Imports System.IO

Public Class frmAlarm_Description
    Private frmScada() As Form
    Private dtAlarm As New DataTable("dtAlarm")
    Private tempFile As String = Application.StartupPath + "/tempAlarm_Description.txt"
    Private blNewAlarm As Boolean = False
    Private cnBatching As clsDB
    Dim CheckRow As Integer = 0

    Public Sub New(ByRef Form_() As Form, ByRef cnDB As clsDB)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        frmScada = Form_
        Initial_DataTable()
        blNewAlarm = True
        cnBatching = cnDB
    End Sub

    Private Sub frmAlarm_Description_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblAppName.Text = Application.ProductName
        Call LoadDataTableFromTextFile(dtAlarm, tempFile)
        Changed_language_in_DT(structApp_Language.strLang_Main, cnBatching) 'For Multi Language
        verify_ctrl_In_frm()
        'NotifyIcon1.Icon = My.Resources.alert
        Timer1.Enabled = True
        txtFilter.Text = ""
        lblLang.Text = structApp_Language.strLang_Main
        '============ For multi language
        AddHandler Langauge_Change, AddressOf Changed_language_in_DT
    End Sub

    Private Sub verify_ctrl_In_frm()
        Dim rowsToRemove As New List(Of DataRow)
        Dim ctrlFound As Boolean = False
        For Each row As DataRow In dtAlarm.Rows
            For Each frm As Form In frmScada
                If frm Is Nothing Then
                    Exit For
                End If
                Dim foundControls() As Control = frm.Controls.Find(row("ctrlName").ToString, True)
                If foundControls.Length > 0 Then
                    ctrlFound = True
                    Exit For
                End If
            Next

            ' ถ้าไม่พบ Control ให้เตรียมลบ DataRow นั้น
            If Not ctrlFound Then
                rowsToRemove.Add(row)
            End If
        Next
        ' ลบแถวที่ไม่พบ Control ในฟอร์มหลังจากวนลูปเสร็จ
        For Each rowToRemove As DataRow In rowsToRemove
            dtAlarm.Rows.Remove(rowToRemove)
        Next
    End Sub

    Private Sub Initial_DataTable()
        Dim cMotor_Name As New DataColumn("cMotor_Name", GetType(String))
        Dim cStatus As New DataColumn("cStatus", GetType(String))
        Dim cDescription As New DataColumn("cDescription", GetType(String))
        'Dim cTime As New DataColumn("cTime", GetType(DateTime))
        Dim cTime As New DataColumn("cTime", GetType(String))
        Dim strType As New DataColumn("strType", GetType(String))
        Dim ctrlName As New DataColumn("ctrlName", GetType(String))

        dtAlarm.Columns.AddRange(New DataColumn() {cMotor_Name, cStatus, cDescription, cTime, strType, ctrlName})

        dtAlarm.PrimaryKey = New DataColumn() {cMotor_Name, cDescription, ctrlName}
        dgvAlarm.ColumnHeadersDefaultCellStyle.Font = New Font("Tahoma", 10, FontStyle.Bold)
    End Sub

#Region "Changed Language In Data table"
    Private Sub Changed_language_in_DT(ByVal strLangCode As String, ByRef cnDB As clsDB)
        For Each row As DataRow In dtAlarm.Rows
            Dim tmpMsg As String = row("cDescription")
            row("cDescription") = UCase(Query_Text_By_LanguageCode(tmpMsg, strLangCode, cnDB)) 'For Multi Language
        Next
    End Sub
#End Region

#Region "Alarm Message"
    Private Sub Obj_ErrMessage_Changed(sender As Object)
        Dim ctrl As TAT_MQTT_CTRL.ctrlTAT_ = CType(sender, TAT_MQTT_CTRL.ctrlTAT_)
        If ctrl.strAlarmMessage <> "" Then
            Dim arrMsg() As String = ctrl.strAlarmMessage.Split("|"c)
            Remove_Old_ErrMessage(ctrl, arrMsg)
            Add_ErrMessage(ctrl, arrMsg)
        Else
            Clear_All_ErrMessage(ctrl)
        End If
        dgvAlarm.DataSource = dtAlarm
    End Sub

    Private Sub Add_ErrMessage(ctrl As TAT_MQTT_CTRL.ctrlTAT_, arrMSG() As String)

        For Each tmpMSG As String In arrMSG
            Dim cTime As DateTime = DateTime.ParseExact(ctrl.time_error, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            Dim tmpLangMsg As String = Query_Text_By_LanguageCode(tmpMSG, structApp_Language.strLang_Main, cnBatching) 'For Multi Language
            If IsDataExists(ctrl, tmpLangMsg) = False Then
                Dim row As DataRow = dtAlarm.NewRow()
                row("cMotor_Name") = ctrl.M_Code
                row("cStatus") = UCase(Replace(ctrl.Status.ToString, "_", ""))
                'row("cDescription") = UCase(tmpMSG)
                row("cDescription") = UCase(Query_Text_By_LanguageCode(tmpMSG, structApp_Language.strLang_Main, cnBatching)) 'For Multi Language
                row("cTime") = ctrl.time_error
                row("strType") = "MSG"
                row("ctrlName") = ctrl.Name

                dtAlarm.Rows.Add(row)
                blNewAlarm = True
                dgvAlarm.ClearSelection()
            End If
        Next
    End Sub

    Function IsDataExists(ctrl As TAT_MQTT_CTRL.ctrlTAT_, ByVal cDescription As String) As Boolean
        Dim strM_Code As String = ctrl.M_Code
        Dim strCtrlName As String = ctrl.Name
        Dim strCond As String = "cMotor_Name = '" + strM_Code + "' AND cDescription = '" + UCase(cDescription) + "' AND ctrlName = '" & strCtrlName & "'"
        Dim rows() As DataRow = dtAlarm.Select(strCond)
        If rows.Length > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub Clear_All_ErrMessage(ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        Dim strCond As String = "cMotor_Name  = '" + ctrl.M_Code + "' AND strType = 'MSG'"
        Dim rows() As DataRow = dtAlarm.Select(strCond)
        For Each row As DataRow In rows
            dtAlarm.Rows.Remove(row)
        Next
    End Sub

    Private Sub Remove_Old_ErrMessage(ctrl As TAT_MQTT_CTRL.ctrlTAT_, currentMSG() As String)
        If dtAlarm.Rows.Count = 0 Then
            Exit Sub
        End If
        '===== Check have message not active

        Dim strCond As String = "cMotor_Name  = '" + ctrl.M_Code + "'AND strType = 'MSG'"
        Dim rows() As DataRow = dtAlarm.Select(strCond)

        For Each row As DataRow In rows
            If Check_HaveCurrent_Msg(row("cDescription"), currentMSG) = False Then
                dtAlarm.Rows.Remove(row)
            End If
        Next

    End Sub

    Private Function Check_HaveCurrent_Msg(strMSG As String, arrMSG() As String) As Boolean
        For Each tmpMSG As String In arrMSG
            Dim tmpText As String = Query_Text_By_LanguageCode(tmpMSG, structApp_Language.strLang_Main, cnBatching)
            If UCase(strMSG) = UCase(tmpText) Then
                Return True
            End If
        Next
        Return False
    End Function
#End Region

#Region "Alarm Status"
    Private Sub Get_AlarmStatus_Motor(ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        If ctrl.status_err1 Or ctrl.status_err2 Or ctrl.status_coverlock Then
            'If not Exist then add new
            Add_AlarmStatus_Motor(ctrl)
        Else
            'Check and remove alarm status if it's exist
            Remove_Old_AlarmStatus_Motor(ctrl)
        End If
    End Sub
    Private Sub Add_AlarmStatus_Motor(ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        'Dim strDesc As String = UCase(ctrl.current_status)
        Dim strDesc As String = UCase(Query_Text_By_LanguageCode(ctrl.current_status, structApp_Language.strLang_Main, cnBatching)) 'For multi language
        'Dim cTime As DateTime = DateTime.ParseExact(ctrl.time_error, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
        If ctrl.M_Code = "SL-SPARE" Then
            Console.WriteLine(ctrl.M_Code)
        End If
        If IsDataExists(ctrl, strDesc) = False Then
            Dim row As DataRow = dtAlarm.NewRow()
            row("cMotor_Name") = ctrl.M_Code
            row("cStatus") = UCase(Replace(ctrl.Status.ToString, "_", ""))
            row("cDescription") = strDesc
            row("cTime") = ctrl.time_error
            row("strType") = "STA"
            row("ctrlName") = ctrl.Name

            dtAlarm.Rows.Add(row)
            blNewAlarm = True
            dgvAlarm.ClearSelection()
        End If
    End Sub
    Private Sub Remove_Old_AlarmStatus_Motor(ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        If dtAlarm.Rows.Count = 0 Then
            Exit Sub
        End If
        '===== Check have message not active

        Dim strCond As String = "cMotor_Name  = '" + ctrl.M_Code + "'AND strType = 'STA' AND ctrlName = '" & ctrl.Name & "'"
        Dim rows() As DataRow = dtAlarm.Select(strCond)

        For Each row As DataRow In rows
            dtAlarm.Rows.Remove(row)
        Next

    End Sub


    '============================= Control Bin
    Private Sub Get_AlarmBin(ctrl As TAT_CtrlBin.ctrlBin_)
        'If ctrl.mqtt_bin_parameter_config_.BIN_CODE = "PK01" Then
        '    Debug.Print("TEST")
        'End If
        '============== Self Bin
        If ctrl.mqtt_bin_parameter_status_.STA_HI Then
            Add_AlarmBin(ctrl.mqtt_bin_parameter_config_.BIN_CODE, "HI", ctrl.mqtt_bin_parameter_status_.LAST_UPDATE)
        Else
            Remove_AlarmBin(ctrl.mqtt_bin_parameter_config_.BIN_CODE, "HI")
        End If
        If ctrl.mqtt_bin_parameter_status_.STA_LO Then
            Add_AlarmBin(ctrl.mqtt_bin_parameter_config_.BIN_CODE, "LO", ctrl.mqtt_bin_parameter_status_.LAST_UPDATE)
        Else
            Remove_AlarmBin(ctrl.mqtt_bin_parameter_config_.BIN_CODE, "LO")
        End If

        '============= Ref Bin
        If ctrl.Bin_Ref_Status <> "" Then
            If ctrl.mqtt_bin_ref_status_.STA_HI Then
                Add_AlarmBin(ctrl.mqtt_bin_ref_config_.BIN_CODE, "HI", ctrl.mqtt_bin_ref_status_.LAST_UPDATE)
            Else
                Remove_AlarmBin(ctrl.mqtt_bin_ref_config_.BIN_CODE, "HI")
            End If
            If ctrl.mqtt_bin_ref_status_.STA_LO Then
                Add_AlarmBin(ctrl.mqtt_bin_ref_config_.BIN_CODE, "LO", ctrl.mqtt_bin_ref_status_.LAST_UPDATE)
            Else
                Remove_AlarmBin(ctrl.mqtt_bin_ref_config_.BIN_CODE, "LO")
            End If
        End If

    End Sub

    Private Sub Add_AlarmBin(strBin_Name As String, strStatus As String, strEventTime As String)
        Dim strDescription As String = ""
        If UCase(strStatus) = "HI" Then
            strDescription = "BIN HIGH"
        ElseIf UCase(strStatus) = "LO" Then
            strDescription = "BIN LOW"
        End If
        strDescription = UCase(Query_Text_By_LanguageCode(strDescription, structApp_Language.strLang_Main, cnBatching)) 'For multi language
        If IsBinExists(strBin_Name, strDescription) = False Then
            Dim row As DataRow = dtAlarm.NewRow()
            row("cMotor_Name") = strBin_Name
            row("cStatus") = strStatus
            row("cDescription") = strDescription
            row("cTime") = strEventTime
            row("strType") = "BIN"
            row("ctrlName") = strBin_Name

            dtAlarm.Rows.Add(row)
            blNewAlarm = True
            dgvAlarm.ClearSelection()
        End If
    End Sub
    Function IsBinExists(strBin_Name As String, ByVal cDescription As String) As Boolean
        Dim strCond As String = "cMotor_Name = '" + strBin_Name + "' AND cDescription = '" + UCase(cDescription) + "' "
        Dim rows() As DataRow = dtAlarm.Select(strCond)
        If rows.Length > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Private Sub Remove_AlarmBin(strBin_Name As String, strStatus As String)

        Dim strCond As String = "cMotor_Name  = '" + strBin_Name + "' AND cStatus = '" + strStatus + "' AND strType = 'BIN'"
        Dim rows() As DataRow = dtAlarm.Select(strCond)

        For Each row As DataRow In rows
            dtAlarm.Rows.Remove(row)
        Next
    End Sub
#End Region

#Region "Alarm PM"
    Private Sub Get_PM_Alarm(ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        If ctrl.PM_Alarm = True Then
            'Show alarm
            Add_Alarm_PM(ctrl)
        Else
            'Delete Old alarm
            Remove_Old_Alarm_PM(ctrl)
        End If
    End Sub
    Private Sub Add_Alarm_PM(ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        Dim strDesc As String = UCase(CheckNull(ctrl.objStatus.PM_MESSAGE))
        If strDesc = "" Then
            Exit Sub
        End If
        strDesc = UCase(Query_Text_By_LanguageCode(strDesc, structApp_Language.strLang_Main, cnBatching)) 'For multi language
        'Dim cTime As DateTime = DateTime.ParseExact(ctrl.time_error, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
        If Is_PM_Already(ctrl.M_Code) = False Then
            Dim row As DataRow = dtAlarm.NewRow()
            row("cMotor_Name") = ctrl.M_Code
            row("cStatus") = UCase(Replace(ctrl.Status.ToString, "_", ""))
            row("cDescription") = strDesc
            row("cTime") = ctrl.time_error
            row("strType") = "PM"
            row("ctrlName") = ctrl.Name

            dtAlarm.Rows.Add(row)
            blNewAlarm = True
            dgvAlarm.ClearSelection()
        End If
    End Sub
    Private Sub Remove_Old_Alarm_PM(ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        If dtAlarm.Rows.Count = 0 Then
            Exit Sub
        End If
        '===== Check have message not active

        Dim strCond As String = "cMotor_Name  = '" + ctrl.M_Code + "'AND strType = 'PM'"
        Dim rows() As DataRow = dtAlarm.Select(strCond)

        For Each row As DataRow In rows
            dtAlarm.Rows.Remove(row)
        Next

    End Sub
    Private Function Is_PM_Already(ByVal cMotor_Name As String) As Boolean
        Dim strCond As String = "cMotor_Name = '" + cMotor_Name + "' AND strType = 'PM'"
        Dim rows() As DataRow = dtAlarm.Select(strCond)
        If rows.Length > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

    Private Function CheckNull(ByVal value As Object) As String
        If value Is Nothing OrElse IsDBNull(value) Then
            Return ""
        Else
            Return value.ToString()
        End If
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For Each frm As Form In frmScada
            If frm Is Nothing Then
                Exit For
            End If
            For Each ctrl As Control In frm.Controls
                If TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
                    Obj_ErrMessage_Changed(ctrl)
                    Get_AlarmStatus_Motor(ctrl)
                    Get_PM_Alarm(ctrl)
                ElseIf TypeOf ctrl Is TAT_CtrlBin.ctrlBin_ Then
                    Get_AlarmBin(ctrl)
                End If
            Next
        Next
        Set_DGV_RowColor()
        If dtAlarm.Rows.Count = 0 Then
            blNewAlarm = False
        End If
        If blNewAlarm Then
            If Me.Visible = False Then
                Me.Visible = True
            End If
            Me.WindowState = FormWindowState.Normal
            'blNewAlarm = False
            dgvAlarm.ClearSelection()
        Else
            Me.Visible = False
        End If

        If CheckRow < dgvAlarm.Rows.Count Then

        End If

        PictureBox1.Visible = Not PictureBox1.Visible
    End Sub

    Private Sub Set_DGV_RowColor()
        'dgvAlarm.DefaultCellStyle.Font = New Font("Tahoma", 8, FontStyle.Bold)
        For Each row As DataGridViewRow In dgvAlarm.Rows
            If UCase(row.Cells("cStatus").Value.ToString()) = "ERR1" Or UCase(row.Cells("cStatus").Value.ToString()) = "HI" Then
                row.DefaultCellStyle.ForeColor = Color.Red
            ElseIf UCase(row.Cells("cStatus").Value.ToString()) = "ERR2" Or UCase(row.Cells("cStatus").Value.ToString()) = "LO" Then
                row.DefaultCellStyle.ForeColor = Color.Yellow
            ElseIf UCase(row.Cells("strType").Value.ToString()) = "MSG" Then
                row.DefaultCellStyle.ForeColor = Color.Magenta
            ElseIf UCase(row.Cells("strType").Value.ToString()) = "PM" Then
                row.DefaultCellStyle.ForeColor = Color.Orange
            End If
            row.DefaultCellStyle.Font = New Font("Tahoma", 8, FontStyle.Bold)
        Next
    End Sub

    Private Sub btnHide_Click(sender As Object, e As EventArgs) Handles btnHide.Click
        blNewAlarm = False
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub btnAcknow_Click(sender As Object, e As EventArgs) Handles btnAcknow.Click
        blNewAlarm = False
        Me.Hide()
        NotifyIcon1.BalloonTipTitle = "ALARM DESCRIPTION"
        NotifyIcon1.BalloonTipText = "Double-click the icon to open again."
        Me.NotifyIcon1.Visible = True
        Me.NotifyIcon1.ShowBalloonTip(0)
    End Sub
    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        blNewAlarm = True
        Me.Show()
    End Sub
    Private Sub frmAlarm_Description_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        dgvAlarm.ClearSelection()
    End Sub
    Private Sub dgvAlarm_DataBindingComplete(sender As Object, e As DataGridViewBindingCompleteEventArgs) Handles dgvAlarm.DataBindingComplete
        dgvAlarm.ClearSelection()
    End Sub
    Private Sub txtFilter_TextChanged(sender As Object, e As EventArgs) Handles txtFilter.TextChanged
        If dtAlarm.Rows.Count = 0 Then
            Exit Sub
        End If
        Dim filterText As String = txtFilter.Text
        dtAlarm.DefaultView.RowFilter = String.Format("cMotor_Name LIKE '%{0}%' OR cStatus LIKE '%{0}%' OR cDescription LIKE '%{0}%'", filterText)
    End Sub

#Region "Temp File for Reload"
    Sub SaveDataTableToTextFile(ByVal dt As DataTable, ByVal filePath As String)
        ' Delete the file if it exists
        If File.Exists(filePath) Then
            File.Delete(filePath)
        End If

        ' Write to a new file
        Using sw As New StreamWriter(filePath)
            ' Write column headers
            Dim columnHeaders = dt.Columns.Cast(Of DataColumn)().Select(Function(column) column.ColumnName)
            sw.WriteLine(String.Join(",", columnHeaders))

            ' Write rows
            For Each row As DataRow In dt.Rows
                Dim fields = row.ItemArray.Select(Function(field) field.ToString())
                sw.WriteLine(String.Join(",", fields))
            Next
        End Using
    End Sub

    Sub LoadDataTableFromTextFile(ByVal dt As DataTable, ByVal filePath As String)
        If File.Exists(filePath) Then
            Using sr As New StreamReader(filePath)
                ' Skip the first line (column headers)
                sr.ReadLine()

                ' Read rows
                While Not sr.EndOfStream
                    Dim fields = sr.ReadLine().Split(","c)
                    dt.Rows.Add(fields)
                End While
            End Using
        Else
            Console.WriteLine("File does not exist: " & filePath)
        End If
    End Sub

    Private Sub frmAlarm_Description_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Call SaveDataTableToTextFile(dtAlarm, tempFile)
    End Sub
#End Region
End Class