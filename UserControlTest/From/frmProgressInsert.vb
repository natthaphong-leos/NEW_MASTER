
Public Class frmProgressInsert
    Public ControlsToProcess As List(Of Control)
    Public MainVerifier As clsVerifyObject
    Public dtObj_List As DataTable
    Dim checking As obj_state
    Private UpdatedCount As Integer = 0

    Private Sub ProgressForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = SystemIcons.Information
        If ControlsToProcess IsNot Nothing AndAlso ControlsToProcess.Count > 0 Then
            ProgressBar1.Maximum = ControlsToProcess.Count
            ProgressBar1.Value = 0
            'Process_Insert()
            BackgroundWorker1.RunWorkerAsync()
        Else
            Me.Close()
        End If
    End Sub

    Private Sub Set_Status_TestIO()
        For Each ctrl As Control In ControlsToProcess
            If TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
                If dtObj_List.Rows.Count > 0 Then
                    'Check Have In Datatable
                    Dim Dr As DataRow = Get_Row_FromDT(DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).Name)
                    If Dr IsNot Nothing Then 'Have Data In DB
                        Check_Status_TestIO(ctrl, Dr)
                    Else
                        MainVerifier.Check_ObjStatus_In_OtherApp(CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_))
                    End If
                Else
                    MainVerifier.Check_ObjStatus_In_OtherApp(CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_))
                End If
            End If
            DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).Mode_Test_IO = True
        Next
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

        Dim processed As Integer = 0
        Dim total As Integer = ControlsToProcess.Count
        checking.obj_Max = ControlsToProcess.Count

        For Each ctrl As Control In ControlsToProcess

            If TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
                checking.obj_name = DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).Name
                If dtObj_List.Rows.Count > 0 Then
                    'Check Have In Datatable
                    Dim Dr As DataRow = Get_Row_FromDT(DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).Name)
                    If Dr IsNot Nothing Then 'Have Data In DB
                        If Check_Detail_Changed(ctrl, Dr) = True Then
                            UpdatedCount += 1
                        End If
                        'Check_Status_TestIO(ctrl, Dr)
                        dtObj_List.Rows.Remove(Dr)
                    Else 'New Object In Form
                        MainVerifier.Insert_CtrlTAT_ToDB(CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_))
                        UpdatedCount += 1
                        'DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_TEST '======= NEW OBJECT
                        'MainVerifier.Check_ObjStatus_In_OtherApp(CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_))
                    End If
                Else
                    MainVerifier.Insert_CtrlTAT_ToDB(CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_))
                    UpdatedCount += 1
                    'DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_TEST '======= NEW OBJECT
                    'MainVerifier.Check_ObjStatus_In_OtherApp(CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_))
                End If
            End If

            ' อัปเดต Progress Bar
            processed += 1
            checking.obj_current = processed
            Dim percent As Integer = CInt((processed / total) * 100)
            'BackgroundWorker1.ReportProgress(percent, processed)
            BackgroundWorker1.ReportProgress(percent, checking)
        Next

        If dtObj_List.Rows.Count > 0 Then
            UpdatedCount += MainVerifier.Delete_Data_From_DB(dtObj_List)
        End If

    End Sub

    Private Function Check_Detail_Changed(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_, row As DataRow) As Boolean
        If ctrl.PLC_Station_No <> row("n_plc_station").ToString OrElse
                ctrl.Index <> row("n_index") OrElse
                ctrl.ControlType.ToString <> row("c_type") Then 'แก้ไข Station PLC
            MainVerifier.Update_Ctrl_New_Device(ctrl)
            Return True
            'ElseIf ctrl.Index <> row("n_index") Then 'แก้ไข Index
            '    MainVerifier.Update_Ctrl_New_Device(ctrl)
            '    Return True
            'ElseIf ctrl.ControlType.ToString <> row("c_type") Then 'แก้ไข Type(MOTOR,SLIDE,FLAP BOX)
            '    MainVerifier.Update_Ctrl_New_Device(ctrl)
            '    Return True
        ElseIf ctrl.Parent.Name <> row("c_parent_obj") OrElse
            $"{ctrl.Location.X},{ctrl.Location.Y}" <> row("c_location") Then
            MainVerifier.Update_Ctrl_Detail_Changed(ctrl)
            Return True
        End If

        Return False
    End Function

    Private Sub Check_Status_TestIO(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_, row As DataRow)
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
        'ctrl.Mode_Test_IO = True
    End Sub

    Private Function Get_Row_FromDT(ObjName As String) As DataRow
        ' ✅ ค้นหา Control ใน DataTable 
        For Each row As DataRow In dtObj_List.Rows
            If row("c_obj_name").ToString() = ObjName Then
                Return row
            End If
        Next
        Return Nothing
    End Function

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = DirectCast(e.UserState, obj_state).obj_current
        'lblStatus.Text = $"กำลังเพิ่มข้อมูล... {e.UserState}/{ProgressBar1.Maximum} รายการ ({e.ProgressPercentage}%)"
        lblStatus.Text = $"กำลังตรวจสอบ... {DirectCast(e.UserState, obj_state).obj_name  } : {DirectCast(e.UserState, obj_state).obj_current }/{ProgressBar1.Maximum} รายการ ({e.ProgressPercentage}%)"
        lblPercent.Text = e.ProgressPercentage & "%"
        lblCurrentState.Text = $"อัพเดตข้อมูลแล้ว... {UpdatedCount} รายการ"
    End Sub

    Private Structure obj_state
        Dim obj_Max As Integer
        Dim obj_current As Integer
        Dim obj_name As String
    End Structure

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        'MessageBox.Show("Insert Completed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        lblStatus.Text = $"ตรวจสอบเสร็จสมบูรณ์... ({checking.obj_current} รายการ)"
        Set_Status_TestIO()
        btnClose.Visible = True
        'Me.Close()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
        Dim frmResult As New frmTestResult
        frmResult.AppName = MainVerifier.AppName
        frmResult.frmName = MainVerifier.frmName
        frmResult.cnDB = MainVerifier.cnDB
        frmResult.ControlsList = ControlsToProcess
        frmResult.Show()
        frmResult.BringToFront()
        Me.Close()
    End Sub
End Class