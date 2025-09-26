Public Class frmConfirmTest
    Private tmpCtrl As TAT_MQTT_CTRL.ctrlTAT_
    Private cnDB As clsDB
    Private UserName As String = ""
    '702, 307
    '702, 182

    Public Sub New(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_, DB As clsDB, cUser As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        tmpCtrl = ctrl
        cnDB = DB
        UserName = cUser

    End Sub

    Private Sub frmConfirmTest_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Size = New Size(702, 182)
        Me.Icon = SystemIcons.Question
        lblM_Code.Text = tmpCtrl.M_Code
        'Me.BringToFront()
        'Me.TopMost = True
        'btnYes.Focus()
        'Me.KeyPreview = True
        Me.Activate()
    End Sub

    ' ฟังก์ชันอัปเดตผลการทดสอบ
    Private Sub CallProcedure_UpdateTestResult()
        ' ใส่โค้ดที่ต้องการให้ทำเมื่อทดสอบสำเร็จ
        Dim strSQL As String = "Exec dbo.FD_Update_TestIO "
        strSQL += "@Pm_type = '" & tmpCtrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & tmpCtrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & tmpCtrl.PLC_Station_No & "', "
        strSQL += "@Pm_User = '" & UserName & "'"
        cnDB.ExecuteNoneQuery(strSQL)

        'MsgBox("TEST SUCCESS")
    End Sub

    ' ฟังก์ชันอัปเดตผลคอมมเนต์เมื่อเทสต้องการให้เทสไม่ผ่าน
    Private Sub CallProcedure_UpdateTestComment(ByVal strComment As String)
        Dim strSQL As String = "Exec dbo.FD_Update_Comment_TestIO "
        strSQL += "@Pm_type = '" & tmpCtrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & tmpCtrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & tmpCtrl.PLC_Station_No & "', "
        strSQL += "@Pm_status = 0, "
        strSQL += "@Pm_User = '" & UserName & "', "
        strSQL += "@Pm_Comment = N'" & strComment & "'"
        cnDB.ExecuteNoneQuery(strSQL)

    End Sub

    Private Sub frmConfirmTest_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Y Then
            'MessageBox.Show("คุณกด Y", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information)
            btnYes_Click(btnYes, EventArgs.Empty)
        ElseIf e.KeyCode = Keys.N Then
            'MessageBox.Show("คุณกด N", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            btnNo_Click(btnNo, EventArgs.Empty)
        End If
    End Sub

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        CallProcedure_UpdateTestResult()
        tmpCtrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.TEST_SUCCESS
        Application.DoEvents()
        Me.Close()
    End Sub

    Private Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        Me.Size = New Size(702, 307)
        Me.KeyPreview = False
        txtComment.Focus()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        CallProcedure_UpdateTestComment(txtComment.Text.Trim)
        tmpCtrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_SUCCESS
        Me.Close()
    End Sub



    Private Sub txtComment_KeyDown(sender As Object, e As KeyEventArgs) Handles txtComment.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnOK_Click(btnOK, EventArgs.Empty)
        End If
    End Sub
End Class