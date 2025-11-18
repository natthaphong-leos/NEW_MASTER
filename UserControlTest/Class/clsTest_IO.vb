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
        CallProcedure_StartTest()
        tmpCtrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_SUCCESS
    End Sub

    ' Event Handler เมื่อสถานะเปลี่ยน
    Private Async Sub Ctrl_StatusChangedAsync()
        Try
            If tmpCtrl Is Nothing Then Exit Sub

            Dim statusText As String = Replace(tmpCtrl.Status.ToString(), "_", "").ToLowerInvariant()

            ' ถ้า Status = Output ไม่ต้องทำอะไร
            If statusText = "out" Then
                Exit Sub
            End If

            ' ถ้า Status = run อัปเดตผลการทดสอบ
            If statusText = "run" Then
                If Opt_Confirm_TestIO = True Then
                    ShowConfirmDialogSafe()
                Else
                    Await CallProcedure_UpdateTestResult()
                    tmpCtrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.TEST_SUCCESS
                End If

                DisposeClass()
            Else
                DisposeClass()
            End If
        Catch ex As Exception : End Try
    End Sub

    ' ===== เพิ่ม Method ใหม่สำหรับแสดง Dialog =====
    Private Sub ShowConfirmDialogSafe()
        Try
            ' ตรวจสอบว่าต้อง Invoke หรือไม่
            If Application.OpenForms.Count > 0 Then
                Dim mainForm As Form = Application.OpenForms(0)
                If mainForm IsNot Nothing AndAlso mainForm.InvokeRequired Then
                    mainForm.Invoke(New Action(AddressOf ShowDialogCore))
                Else
                    ShowDialogCore()
                End If
            Else
                ShowDialogCore()
            End If

        Catch ex As Exception
            ' Log error ถ้ามี
            Debug.WriteLine($"ShowConfirmDialogSafe Error: {ex.Message}")
        End Try
    End Sub

    ' ===== Core Method สำหรับแสดง Dialog =====
    Private Sub ShowDialogCore()
        Try
            Using frmConfirm As New frmConfirmTest(tmpCtrl, cnDB, UserName)
                ' ตั้งค่าให้ Dialog เป็นอิสระจาก MDI
                With frmConfirm
                    .StartPosition = FormStartPosition.CenterScreen
                    .TopMost = True
                    .ShowInTaskbar = True
                    .FormBorderStyle = FormBorderStyle.FixedDialog
                    .MaximizeBox = False
                    .MinimizeBox = False
                End With

                ' แสดง Dialog โดยไม่ส่ง Owner
                frmConfirm.ShowDialog()

                Application.DoEvents()
            End Using

        Catch ex As Exception
            ' Log error ถ้ามี
            Debug.WriteLine($"ShowDialogCore Error: {ex.Message}")
        End Try
    End Sub

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
        Dim strSQL As String = "Exec dbo.FD_Update_TestIO "
        strSQL += "@Pm_type = '" & tmpCtrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & tmpCtrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & tmpCtrl.PLC_Station_No & "', "
        strSQL += "@Pm_User = '" & UserName & "'"
        cnDB.ExecuteNoneQuery(strSQL)
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
        RemoveHandler tmpCtrl.OnStatusChanged, AddressOf Ctrl_StatusChangedAsync
        mdlTest_IO.RemoveInstance(tmpCtrl)
        tmpCtrl = Nothing
        Me.Finalize()
    End Sub

End Class