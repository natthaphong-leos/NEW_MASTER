Public Class clsTest_IO
    Private tmpCtrl As TAT_MQTT_CTRL.ctrlTAT_
    Private cnDB As clsDB
    Private UserName As String = ""

    ' Constructor
    Public Sub New(ctrl As TAT_MQTT_CTRL.ctrlTAT_, DB As clsDB, cUser As String)
        tmpCtrl = ctrl
        cnDB = DB
        UserName = cUser
        AddHandler tmpCtrl.OnStatusChanged, AddressOf Ctrl_StatusChangedAsync
        'Ctrl_StatusChanged() ' เช็คสถานะทันทีที่สร้าง
        CallProcedure_StartTest()
        tmpCtrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_SUCCESS
    End Sub

    ' Event Handler เมื่อสถานะเปลี่ยน
    Private Async Function Ctrl_StatusChangedAsync() As Task
        If tmpCtrl Is Nothing Then
            Exit Function
        End If
        ' ถ้า Status = Output ไม่ต้องทำอะไร
        If Replace(tmpCtrl.Status.ToString, "_", "") = "out" Then

            Exit Function
        End If

        ' ถ้า Status = run อัปเดตผลการทดสอบ
        If Replace(tmpCtrl.Status.ToString, "_", "") = "run" Then
            If Opt_Confirm_TestIO = True Then
                Dim frmConfirm As New frmConfirmTest(tmpCtrl, cnDB, UserName)
                frmConfirm.ShowDialog(MDI_FRM)
                Application.DoEvents()
                frmConfirm.BringToFront()

                'If MsgBox("Is this device test successfull ?", MsgBoxStyle.YesNo, "Confirm Result") = MsgBoxResult.Yes Then
                '    Await CallProcedure_UpdateTestResult()
                'Else
                '    '====== Input comment and update test fail
                '    Dim Comment As String
                '    Comment = InputBox("Please input your comment", "Input comment")
                '    Await CallProcedure_UpdateTestComment(Comment)
                'End If
            Else
                Await CallProcedure_UpdateTestResult()
                tmpCtrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.TEST_SUCCESS
            End If
            DisposeClass()
        Else
            DisposeClass()
        End If
    End Function

    ' ฟังก์ชันอัปเดตผลคอมมเนต์เมื่อเทสต้องการให้เทสไม่ผ่าน
    Private Async Function CallProcedure_UpdateTestComment(ByVal strComment As String) As Task
        Dim strSQL As String = "Exec dbo.FD_Update_Comment_TestIO "
        strSQL += "@Pm_type = '" & tmpCtrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & tmpCtrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & tmpCtrl.PLC_Station_No & "', "
        strSQL += "@Pm_status = 0, "
        strSQL += "@Pm_User = '" & UserName & "', "
        strSQL += "@Pm_Comment = N'" & strComment & "'"
        cnDB.ExecuteNoneQuery(strSQL)

    End Function

    ' ฟังก์ชันอัปเดตผลการทดสอบ
    Private Async Function CallProcedure_UpdateTestResult() As Task
        ' ใส่โค้ดที่ต้องการให้ทำเมื่อทดสอบสำเร็จ
        Dim strSQL As String = "Exec dbo.FD_Update_TestIO "
        strSQL += "@Pm_type = '" & tmpCtrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & tmpCtrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & tmpCtrl.PLC_Station_No & "', "
        strSQL += "@Pm_User = '" & UserName & "'"
        cnDB.ExecuteNoneQuery(strSQL)

        'MsgBox("TEST SUCCESS")
    End Function

    Private Async Function CallProcedure_StartTest() As Task
        Dim strSQL As String = "Exec dbo.FD_Start_TestIO "
        strSQL += "@Pm_type = '" & tmpCtrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & tmpCtrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & tmpCtrl.PLC_Station_No & "', "
        strSQL += "@Pm_User = '" & UserName & "'"
        cnDB.ExecuteNoneQuery(strSQL)
    End Function

    ' ฟังก์ชันทำลายคลาส
    Private Sub DisposeClass()
        ' ลบ Event Handler
        RemoveHandler tmpCtrl.OnStatusChanged, AddressOf Ctrl_StatusChangedAsync

        ' ลบ instance ออกจาก Dictionary
        mdlTest_IO.RemoveInstance(tmpCtrl)

        ' เคลียร์ตัวแปร
        tmpCtrl = Nothing
        Me.Finalize()
    End Sub

End Class
