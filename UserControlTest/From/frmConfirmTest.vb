Public Class frmConfirmTest
    Private tmpCtrl As TAT_MQTT_CTRL.ctrlTAT_
    Private cnDB As clsDB
    Private UserName As String = ""

    Public Sub New(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_, DB As clsDB, cUser As String)
        InitializeComponent()
        tmpCtrl = ctrl
        cnDB = DB
        UserName = cUser
    End Sub

    Private Sub frmConfirmTest_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.Size = New Size(702, 182)
        Me.Icon = SystemIcons.Question
        lblM_Code.Text = tmpCtrl.M_Code

        ' ===== ตั้งค่าให้เป็น Independent Form =====
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ShowInTaskbar = True
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False

        Me.KeyPreview = True
        Me.BringToFront()
        Me.Activate()
        btnYes.Focus()
    End Sub

    Private Sub frmConfirmTest_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ' ===== Force แสดงหน้าต่างอีกครั้ง =====
        Me.TopMost = True
        Me.Activate()
        Me.BringToFront()

        ' ===== Flash Window เพื่อดึงความสนใจ =====
        Try
            FlashWindow(Me.Handle, True)
            System.Threading.Thread.Sleep(100)
            FlashWindow(Me.Handle, False)
        Catch
        End Try

        btnYes.Focus()
        btnYes.Select()
    End Sub

    ' ===== Windows API สำหรับ Flash Window =====
    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function FlashWindow(hwnd As IntPtr, bInvert As Boolean) As Boolean
    End Function

    Private Sub CallProcedure_UpdateTestResult()
        Dim strSQL As String = "Exec dbo.FD_Update_TestIO "
        strSQL += "@Pm_type = '" & tmpCtrl.ControlType.ToString & "', "
        strSQL += "@Pm_index = '" & tmpCtrl.Index & "', "
        strSQL += "@Pm_plc_station = '" & tmpCtrl.PLC_Station_No & "', "
        strSQL += "@Pm_User = '" & UserName & "'"
        cnDB.ExecuteNoneQuery(strSQL)
    End Sub

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
            btnYes_Click(btnYes, EventArgs.Empty)
        ElseIf e.KeyCode = Keys.N Then
            btnNo_Click(btnNo, EventArgs.Empty)
        ElseIf e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click
        CallProcedure_UpdateTestResult()
        tmpCtrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.TEST_SUCCESS
        Application.DoEvents()
        Me.DialogResult = DialogResult.Yes
        Me.Close()
    End Sub

    Private Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        Me.Size = New Size(702, 307)
        Me.KeyPreview = False
        txtComment.Focus()
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        If String.IsNullOrWhiteSpace(txtComment.Text) Then
            MessageBox.Show("Please enter a comment", "Warning",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtComment.Focus()
            Return
        End If

        CallProcedure_UpdateTestComment(txtComment.Text.Trim)
        tmpCtrl.Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_SUCCESS
        Me.DialogResult = DialogResult.No
        Me.Close()
    End Sub

    Private Sub txtComment_KeyDown(sender As Object, e As KeyEventArgs) Handles txtComment.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnOK_Click(btnOK, EventArgs.Empty)
        End If
    End Sub
End Class