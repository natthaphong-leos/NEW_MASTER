Imports System.Reflection

Public Class clsVerifyObject
    Private frmMain As Form
    Public cnDB As clsDB
    Public AppName As String
    Public frmName As String
    Public Sub New(ByRef frm As Form, DB As clsDB)
        frmMain = frm
        cnDB = DB
        AppName = Assembly.GetExecutingAssembly().GetName().Name
        frmName = frmMain.Name
    End Sub

    Public Sub Verify_Object()
        'Dim dt As DataTable = Check_ObjectList_DB()
        Dim dt As DataTable = Get_ObjectList_DB(AppName, frmName, cnDB)
        ' ดึง Control ทั้งหมดที่ต้อง Insert
        Dim allControls As New List(Of Control)
        CollectControls(frmMain.Controls, allControls)

        ' เปิด ProgressForm
        Dim progressForm As New frmProgressInsert()
        progressForm.ControlsToProcess = allControls
        progressForm.MainVerifier = Me
        progressForm.dtObj_List = dt
        progressForm.ShowDialog()

        'If dt.Rows.Count > 0 Then

        'Else
        '    'Insert_New_Data(frmMain.Controls)

        '    ' เปิด ProgressForm
        '    Dim progressForm As New frmProgressInsert()
        '    progressForm.ControlsToProcess = allControls
        '    progressForm.MainVerifier = Me
        '    progressForm.dtObj_List = dt
        '    progressForm.ShowDialog()
        'End If
    End Sub

    ' ฟังก์ชันรวบรวม Control ทั้งหมดที่ต้อง Insert
    Private Sub CollectControls(ByRef controlCollection As IEnumerable, ByRef controlList As List(Of Control))
        For Each control As Object In controlCollection
            If TypeOf control Is GroupBox Or TypeOf control Is Panel Or TypeOf control Is TableLayoutPanel Then
                CollectControls(control.Controls, controlList)
            Else
                If TypeOf control Is TAT_MQTT_CTRL.ctrlTAT_ Then
                    controlList.Add(control)
                End If
            End If
        Next
    End Sub

    Private Function Check_ObjectList_DB() As DataTable
        Dim dtResult As New DataTable
        Dim strSQL As String = ""
        strSQL = "SELECT * From dbo.object_list_inform WHERE c_app_name = '" & AppName & "' "
        strSQL += "And c_parent_form = '" & frmMain.Name & "' "
        dtResult = cnDB.ExecuteDataTable(strSQL)
        Return dtResult.Copy
    End Function

    Public Sub Check_ObjStatus_In_OtherApp(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        Dim dt As DataTable
        Dim strSQL As String
        strSQL = "SELECT TOP(1) dt_test_date,f_complete_test FROM dbo.object_list_inform "
        strSQL += "WHERE c_type = '" & ctrl.ControlType.ToString & "' "
        strSQL += "AND n_index = " & ctrl.Index & " "
        strSQL += "AND n_plc_station = " & ctrl.PLC_Station_No & " "
        dt = cnDB.ExecuteDataTable(strSQL)
        If dt.Rows.Count > 0 Then
            Dim fCompleteTest As Integer = If(IsDBNull(dt.Rows(0)("f_complete_test")), 0, Convert.ToInt32(dt.Rows(0)("f_complete_test")))
            Dim dtTestDateIsNull As Boolean = IsDBNull(dt.Rows(0)("dt_test_date"))
            If fCompleteTest = 0 And dtTestDateIsNull Then
                ' กรณีที่ยังไม่ทดสอบ
                ctrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_TEST
            ElseIf fCompleteTest = 0 And Not dtTestDateIsNull Then
                ' กรณีที่เริ่มทดสอบแล้ว แต่ยังไม่เสร็จ
                ctrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_SUCCESS
            ElseIf fCompleteTest = 1 Then
                ' กรณีทดสอบเสร็จสมบูรณ์
                ctrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.TEST_SUCCESS
            End If
        Else
            ctrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_TEST
        End If
        'ctrl.Mode_Test_IO = True
    End Sub

    Public Sub Insert_New_Data(ByRef ctrl As Control)
        If TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
            Insert_CtrlTAT_ToDB(CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_))
        End If
    End Sub

    Public Sub Insert_CtrlTAT_ToDB(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        Dim strSQL As String = "Exec dbo.FD_Insert_New_Motor_ToList "
        strSQL += "@Pm_app_name = '" & AppName & "', "
        strSQL += "@Pm_obj_name = '" & ctrl.Name & "', "
        strSQL += "@Pm_type = '" & ctrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & ctrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & ctrl.PLC_Station_No & "', "
        strSQL += "@Pm_parent_form = '" & frmMain.Name & "', "
        strSQL += "@Pm_parent_obj = '" & ctrl.Parent.Name & "', "
        strSQL += "@Pm_location = '" & $"{ctrl.Location.X},{ctrl.Location.Y}" & "' "
        cnDB.ExecuteNoneQuery(strSQL)

    End Sub

    Public Sub Update_Ctrl_New_Device(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        Dim strSQL As String = "Exec dbo.FD_Update_ObjectList_NEW_Device "
        strSQL += "@Pm_app_name = '" & AppName & "', "
        strSQL += "@Pm_obj_name = '" & ctrl.Name & "', "
        strSQL += "@Pm_type = '" & ctrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & ctrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & ctrl.PLC_Station_No & "', "
        strSQL += "@Pm_parent_form = '" & frmMain.Name & "', "
        strSQL += "@Pm_parent_obj = '" & ctrl.Parent.Name & "', "
        strSQL += "@Pm_location = '" & $"{ctrl.Location.X},{ctrl.Location.Y}" & "' "
        cnDB.ExecuteNoneQuery(strSQL)
    End Sub

    Public Sub Update_Ctrl_Detail_Changed(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        Dim strSQL As String = "Exec dbo.FD_Update_ObjectList_Detail "
        strSQL += "@Pm_app_name = '" & AppName & "', "
        strSQL += "@Pm_obj_name = '" & ctrl.Name & "', "
        strSQL += "@Pm_type = '" & ctrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & ctrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & ctrl.PLC_Station_No & "', "
        strSQL += "@Pm_parent_form = '" & frmMain.Name & "', "
        strSQL += "@Pm_parent_obj = '" & ctrl.Parent.Name & "', "
        strSQL += "@Pm_location = '" & $"{ctrl.Location.X},{ctrl.Location.Y}" & "' "
        cnDB.ExecuteNoneQuery(strSQL)
    End Sub

    Public Function Delete_Data_From_DB(dt As DataTable) As Integer
        Dim count As Integer = 0
        Dim strSQL As String = "Delete dbo.object_list_inform "
        For Each row As DataRow In dt.Rows
            strSQL &= "WHERE "
            strSQL &= "c_app_name = '" & row("c_app_name") & "' "
            strSQL &= "AND c_obj_name = '" & row("c_obj_name") & "' "
            strSQL &= "AND c_parent_form = '" & row("c_parent_form") & "' "
            cnDB.ExecuteNoneQuery(strSQL)
            count += 1
        Next
        Return count
    End Function

End Class
