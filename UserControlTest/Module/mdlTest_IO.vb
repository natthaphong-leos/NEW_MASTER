Imports System.Reflection

Module mdlTest_IO
    ' Dictionary เก็บ ctrlTAT_ ที่มี clsTest_IO
    Private activeInstances As New Dictionary(Of TAT_MQTT_CTRL.ctrlTAT_, clsTest_IO)
    Public Mode_Test_IO As Boolean
    Public Opt_Confirm_TestIO As Boolean


    Public Function Get_ObjectList_DB(AppName As String, formName As String, cnDB As clsDB) As DataTable
        Dim dtResult As New DataTable
        Dim strSQL As String = ""
        strSQL = "SELECT * From dbo.object_list_inform WHERE c_app_name = '" & AppName & "' "
        strSQL += "And c_parent_form = '" & formName & "' "
        dtResult = cnDB.ExecuteDataTable(strSQL)
        Return dtResult.Copy
    End Function

    Public Sub Set_Obj_Mode_TestIO(ByRef controlCollection As IEnumerable)
        For Each control As Object In controlCollection
            If TypeOf control Is GroupBox Or TypeOf control Is Panel Or TypeOf control Is TableLayoutPanel Then
                Set_Obj_Mode_TestIO(control.Controls)
            Else
                If TypeOf control Is TAT_MQTT_CTRL.ctrlTAT_ Then
                    Dim ctrl As TAT_MQTT_CTRL.ctrlTAT_ = DirectCast(control, TAT_MQTT_CTRL.ctrlTAT_)
                    ctrl.Mode_Test_IO = True
                End If
            End If
        Next
    End Sub

    Public Sub Auto_Verify_Object(ByRef frm As Form, cnDB As clsDB)
        Dim AppName As String = Assembly.GetExecutingAssembly().GetName().Name
        Dim dt As DataTable = Get_ObjectList_DB(AppName, frm.Name, cnDB)

        Set_statusIO_Inform(frm.Controls, dt)


    End Sub

    Private Sub Set_statusIO_Inform(ByRef controlCollection As IEnumerable, dtObject As DataTable)
        For Each control As Object In controlCollection
            If TypeOf control Is GroupBox Or TypeOf control Is Panel Or TypeOf control Is TableLayoutPanel Then
                Set_statusIO_Inform(control.Controls, dtObject)
            Else
                If TypeOf control Is TAT_MQTT_CTRL.ctrlTAT_ Then
                    Dim ctrl As TAT_MQTT_CTRL.ctrlTAT_ = DirectCast(control, TAT_MQTT_CTRL.ctrlTAT_)
                    ctrl.Mode_Test_IO = True
                    Dim ctrlName As String = ctrl.Name
                    Dim rows As DataRow() = dtObject.Select("c_obj_name = '" & ctrlName & "'")

                    ' เช็คว่ามีข้อมูลใน rows หรือไม่
                    If rows IsNot Nothing AndAlso rows.Length > 0 Then
                        Dim row As DataRow = rows(0)

                        ' ใช้ IsDBNull() เพื่อป้องกันข้อผิดพลาด
                        Dim fCompleteTest As Integer = If(IsDBNull(row("f_complete_test")), 0, Convert.ToInt32(row("f_complete_test")))
                        Dim dtTestDateIsNull As Boolean = IsDBNull(row("dt_test_date"))

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
                        ' ถ้าไม่มีข้อมูลใน dtObject ให้กำหนดสถานะ NOT_TEST
                        ctrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_TEST
                    End If
                End If
            End If
        Next
    End Sub

    Public Sub func_get_mode_testIO(cnDB As clsDB)
        'Dim dt As DataTable
        'Dim strSQL As String
        'strSQL = "SELECT c_flag from thaisia.batch_option "
        'strSQL += "Where c_location = 'Main_Scada' "
        'strSQL += "And c_code = 'MODE_TEST_IO'"
        'dt = cnDB.ExecuteDataTable(strSQL)
        'If dt.Rows.Count > 0 Then
        '    Dim intMode As Integer = dt.Rows(0)("c_flag").ToString
        '    Mode_Test_IO = IIf(intMode = 1, True, False)
        'Else
        '    Mode_Test_IO = False
        'End If


        Dim reader As New System.Configuration.AppSettingsReader
        Mode_Test_IO = reader.GetValue("Mode_TestIO", GetType(Boolean))

    End Sub

    Public Sub func_get_Option_ConfirmtestIO(cnDB As clsDB)
        'Dim dt As DataTable
        'Dim strSQL As String
        'strSQL = "SELECT c_flag from thaisia.batch_option "
        'strSQL += "Where c_location = 'Main_Scada' "
        'strSQL += "And c_code = 'CONFIRM_TEST_IO'"
        'dt = cnDB.ExecuteDataTable(strSQL)
        'If dt.Rows.Count > 0 Then
        '    Dim intMode As Integer = dt.Rows(0)("c_flag").ToString
        '    Opt_Confirm_TestIO = IIf(intMode = 1, True, False)
        'Else
        '    Opt_Confirm_TestIO = False
        'End If

        Dim reader As New System.Configuration.AppSettingsReader
        Opt_Confirm_TestIO = reader.GetValue("Confirm_TestIO", GetType(Boolean))

    End Sub

    ' ฟังก์ชันเริ่มต้นทดสอบ
    Public Sub func_start_test(ctrl As TAT_MQTT_CTRL.ctrlTAT_, cnDB As clsDB, sUser As String)
        ' ตรวจสอบว่ามี instance แล้วหรือยัง
        If activeInstances.ContainsKey(ctrl) Then
            Exit Sub ' มีแล้ว ไม่ต้องสร้างใหม่
        ElseIf ctrl.Status.ToString = "_run" Then
            Exit Sub
        End If

        ' สร้าง instance ใหม่และบันทึกลง Dictionary
        Dim newInstance As New clsTest_IO(ctrl, cnDB, sUser)
        activeInstances(ctrl) = newInstance
    End Sub

    ' ฟังก์ชันลบ instance ออกจาก Dictionary
    Public Sub RemoveInstance(ctrl As TAT_MQTT_CTRL.ctrlTAT_)
        If activeInstances.ContainsKey(ctrl) Then
            activeInstances.Remove(ctrl)
            'activeInstances(ctrl) = Nothing
        End If
    End Sub

    Public Function Check_Already_Test(ctrl As TAT_MQTT_CTRL.ctrlTAT_, cnDB As clsDB) As Boolean
        Dim objType As String = ctrl.ControlType.ToString
        Dim objIndex As Integer = ctrl.Index
        Dim objStation As String = ctrl.PLC_Station_No
        Dim dt As New DataTable
        Dim strSQL As String = ""
        strSQL = "SELECT c_obj_name FROM dbo.object_list_inform "
        strSQL += "WHERE c_type = '" & objType & "' "
        strSQL += "AND n_index = " & objIndex & " "
        strSQL += "AND n_plc_station = " & objStation & " "
        strSQL += "AND f_complete_test = 1"
        dt = cnDB.ExecuteDataTable(strSQL)
        If dt.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
End Module
