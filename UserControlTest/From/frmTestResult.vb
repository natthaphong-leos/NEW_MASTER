Public Class frmTestResult
    Public AppName As String
    Public frmName As String
    Public cnDB As clsDB
    Private dtListAll As DataTable
    Private dtMotor As DataTable
    Private dtSlide As DataTable
    Private dtFlap As DataTable
    Public ControlsList As List(Of Control)
    Private selectedCtrl As Control
    Private flagStatusChanged As Boolean = False


    Private Sub frmTestResult_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = SystemIcons.Information
        Get_ObjList_Data()
        txtM_Comment.Enabled = False
        If dtListAll.Rows.Count > 0 Then
            Dim totalCount As Integer = dtListAll.Rows.Count
            Dim completeCount As Integer = dtListAll.Select("f_complete_test = 1").Length
            Dim totalPercent As Single = Math.Round(completeCount / totalCount * 100, 2)
            lblQty.Text = completeCount & " / " & totalCount
            lblPercent.Text = totalPercent & " %"
            lblAppName.Text = AppName
            lblFrmName.Text = frmName

            '================================ ข้อมูล MOTOR
            If dtMotor.Rows.Count > 0 Then
                dgvMotor.DataSource = dtMotor
                Verify_DGV(dgvMotor)
                lblMotor_Qty.Text = dtMotor.Select("f_complete_test = 1").Length & " / " & dtMotor.Rows.Count
                lblMotor_Percent.Text = Math.Round(dtMotor.Select("f_complete_test = 1").Length / dtMotor.Rows.Count * 100, 2) & " %"
            End If

            '================================ ข้อมูล SLIDE
            If dtSlide.Rows.Count > 0 Then
                dgvSlide.DataSource = dtSlide
                Verify_DGV(dgvSlide)
                lblSlide_Qty.Text = dtSlide.Select("f_complete_test = 1").Length & " / " & dtSlide.Rows.Count
                lblSlide_Percent.Text = Math.Round(dtSlide.Select("f_complete_test = 1").Length / dtSlide.Rows.Count * 100, 2) & " %"
            End If

            '================================ ข้อมูล WAYER
            If dtFlap.Rows.Count > 0 Then
                dgvFlap.DataSource = dtFlap
                Verify_DGV(dgvFlap)
                lblWayer_Qty.Text = dtFlap.Select("f_complete_test = 1").Length & " / " & dtFlap.Rows.Count
                lblWayer_Percent.Text = Math.Round(dtFlap.Select("f_complete_test = 1").Length / dtFlap.Rows.Count * 100, 2) & " %"
            End If

            Me.BringToFront()
            Me.Activate()
        Else
            Me.Close()
        End If
    End Sub

    Private Function GetFormByName(frmName As String) As Form
        For Each frm As Form In Application.OpenForms
            If frm.Name = frmName Then
                Return frm ' คืนค่า Form ที่ตรงกับชื่อ
            End If
        Next
        Return Nothing ' ถ้าไม่พบ Form คืนค่า Nothing
    End Function

    Private Sub Reload_data()
        dtMotor.Clear()
        dtSlide.Clear()
        dtFlap.Clear()
        dtMotor.Rows.Clear()
        dtSlide.Rows.Clear()
        dtFlap.Rows.Clear()

        Get_ObjList_Data()
        If dtListAll.Rows.Count > 0 Then
            Dim totalCount As Integer = dtListAll.Rows.Count
            Dim completeCount As Integer = dtListAll.Select("f_complete_test = 1").Length
            Dim totalPercent As Single = Math.Round(completeCount / totalCount * 100, 2)
            lblQty.Text = completeCount & " / " & totalCount
            lblPercent.Text = totalPercent & " %"
            lblAppName.Text = AppName
            lblFrmName.Text = frmName

            '================================ ข้อมูล MOTOR
            If dtMotor.Rows.Count > 0 Then
                dgvMotor.DataSource = dtMotor
                Verify_DGV(dgvMotor)
                lblMotor_Qty.Text = dtMotor.Select("f_complete_test = 1").Length & " / " & dtMotor.Rows.Count
                lblMotor_Percent.Text = Math.Round(dtMotor.Select("f_complete_test = 1").Length / dtMotor.Rows.Count * 100, 2) & " %"
            End If

            '================================ ข้อมูล SLIDE
            If dtSlide.Rows.Count > 0 Then
                dgvSlide.DataSource = dtSlide
                Verify_DGV(dgvSlide)
                lblSlide_Qty.Text = dtSlide.Select("f_complete_test = 1").Length & " / " & dtSlide.Rows.Count
                lblSlide_Percent.Text = Math.Round(dtSlide.Select("f_complete_test = 1").Length / dtSlide.Rows.Count * 100, 2) & " %"
            End If

            '================================ ข้อมูล WAYER
            If dtFlap.Rows.Count > 0 Then
                dgvFlap.DataSource = dtFlap
                Verify_DGV(dgvFlap)
                lblWayer_Qty.Text = dtFlap.Select("f_complete_test = 1").Length & " / " & dtFlap.Rows.Count
                lblWayer_Percent.Text = Math.Round(dtFlap.Select("f_complete_test = 1").Length / dtFlap.Rows.Count * 100, 2) & " %"
            End If
        End If
    End Sub

    Private Sub Get_ObjList_Data()
        Dim strSQL As String = ""

        Try
            strSQL = "Exec dbo.FD_Query_Object_List "
            strSQL &= "@Pm_app_name = '" & AppName & "', "
            strSQL &= "@Pm_parent_form = '" & frmName & "' "

            dtListAll = cnDB.ExecuteDataTable(strSQL)

            If dtListAll.Rows.Count > 0 Then
                dtMotor = fn_filter_data("c_type = 'MOTOR'", dtListAll)
                dtSlide = fn_filter_data("c_type = 'SLIDE'", dtListAll)
                dtFlap = fn_filter_data("c_type = 'FLAP'", dtListAll)
            End If

        Catch ex As Exception
            MessageBox.Show("Error in Get_ObjList_Data:" & Environment.NewLine &
                        ex.Message & Environment.NewLine &
                        "SQL: " & strSQL, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Verify_DGV(ByRef dgv As DataGridView)
        ' ไฮไลต์รายการที่ c_type และ n_index เหมือนกันด้วยสีเหลือง
        For i As Integer = 0 To dgv.Rows.Count - 1
            For j As Integer = i + 1 To dgv.Rows.Count - 1
                If dgv.Rows(i).Cells(0).Value.ToString() = dgv.Rows(j).Cells(0).Value.ToString() AndAlso
                   dgv.Rows(i).Cells(1).Value.ToString() = dgv.Rows(j).Cells(1).Value.ToString() Then
                    dgv.Rows(i).DefaultCellStyle.BackColor = Color.Yellow
                    dgv.Rows(j).DefaultCellStyle.BackColor = Color.Yellow
                End If
            Next
        Next

        ' ไฮไลต์รายการที่ f_complete_test = 1 ด้วยสีเขียว
        For Each row As DataGridViewRow In dgv.Rows
            If row.Cells(7).Value.ToString() = "True" Then
                row.DefaultCellStyle.BackColor = Color.LimeGreen
            ElseIf row.Cells(6).Value.ToString() <> "" AndAlso row.Cells(7).Value.ToString() = "False" Then
                row.DefaultCellStyle.BackColor = Color.Orange
            End If
        Next

        AddHandler dgv.SelectionChanged, AddressOf DGV_SelectionChanged

        dgv.ClearSelection()
    End Sub

    Private Sub DGV_SelectionChanged(sender As Object, e As EventArgs)
        Dim dgv As DataGridView = CType(sender, DataGridView)

        If dgv.SelectedRows.Count = 0 Then
            Clear_DGV_Selection()
            Exit Sub
        End If

        Dim strType As String = dgv.SelectedRows(0).Cells(0).Value.ToString
        Dim blStatus_Test As Boolean = dgv.SelectedRows(0).Cells(7).Value
        Dim strUser As String = dgv.SelectedRows(0).Cells(8).Value.ToString

        lblM_Name.Text = dgv.SelectedRows(0).Cells(0).Value.ToString
        lblM_Index.Text = dgv.SelectedRows(0).Cells(1).Value.ToString
        lblM_Code.Text = dgv.SelectedRows(0).Cells(2).Value.ToString
        lblM_Obj.Text = dgv.SelectedRows(0).Cells(3).Value.ToString
        lblM_PLC.Text = dgv.SelectedRows(0).Cells(4).Value.ToString
        lblM_User.Text = IIf(strUser <> "", strUser, "-")
        txtM_Comment.Text = dgv.SelectedRows(0).Cells(9).Value.ToString
        lblM_Status.Tag = IIf(blStatus_Test = True, 1, 0)
        Show_Detail_StatusTest(blStatus_Test, lblM_Status)
        flagStatusChanged = False
        'Select Case strType
        '    Case "MOTOR"
        '        lblM_Name.Text = dgv.SelectedRows(0).Cells(0).Value.ToString
        '        lblM_Index.Text = dgv.SelectedRows(0).Cells(1).Value.ToString
        '        lblM_Code.Text = dgv.SelectedRows(0).Cells(2).Value.ToString
        '        lblM_Obj.Text = dgv.SelectedRows(0).Cells(3).Value.ToString
        '        lblM_PLC.Text = dgv.SelectedRows(0).Cells(4).Value.ToString
        '        lblM_User.Text = IIf(strUser <> "", strUser, "-")
        '        txtM_Comment.Text = dgv.SelectedRows(0).Cells(9).Value.ToString
        '        lblM_Status.Tag = IIf(blStatus_Test = True, 1, 0)
        '        Show_Detail_StatusTest(blStatus_Test, lblM_Status)
        '    Case "SLIDE"

        '    Case "FLAP"

        'End Select

    End Sub

    Private Sub Show_Detail_StatusTest(blStatus As Boolean, ByRef lblStatus As Label)
        If blStatus = True Then
            lblStatus.Text = "PASSED"
            lblStatus.BackColor = Color.Lime
            lblM_Status.Tag = 1
        Else
            lblStatus.Text = "NOT PASS"
            lblStatus.BackColor = Color.Red
            lblM_Status.Tag = 0
        End If
    End Sub

    Private Sub Clear_DGV_Selection()
        '============================= MOTOR
        lblM_Name.Text = "????"
        lblM_Index.Text = "XXX"
        lblM_Code.Text = "M.XXXX"
        lblM_Obj.Text = "-"
        lblM_PLC.Text = "-"
        lblM_User.Text = "-"
        txtM_Comment.Text = ""
        lblM_Status.Tag = ""
        Show_Detail_StatusTest(False, lblM_Status)
        btnUpdate.Enabled = False
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TabControl1.SelectedIndexChanged
        Dim selectedTab As TabPage = TabControl1.SelectedTab
        Dim tabName As String = selectedTab.Name

        Select Case tabName
            Case "tpMotor"
                Verify_DGV(dgvMotor)
            Case "tpSlide"
                Verify_DGV(dgvSlide)
            Case "tpFlap"
                Verify_DGV(dgvFlap)
        End Select

    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Reload_data()
    End Sub


    Private Sub lblM_Status_MouseDown(sender As Object, e As MouseEventArgs) Handles lblM_Status.MouseDown
        'If lblM_Status.Tag = 0 Or MDI_FRM.Main_Scada = False Then
        '    Exit Sub
        'End If

        If lblM_Status.Tag = 0 Then
            Exit Sub
        End If

        If e.Button = Windows.Forms.MouseButtons.Right Then
            ctxMenu.Show(sender, New Point(e.X, e.Y))
        End If

    End Sub

    Private Sub tsMenu_Reset_Click(sender As Object, e As EventArgs) Handles tsMenu_Reset.Click
        Show_Detail_StatusTest(False, lblM_Status)
        If btnUpdate.Enabled = False Then btnUpdate.Enabled = True
        flagStatusChanged = True
    End Sub

    Private Sub txtM_Comment_TextChanged(sender As Object, e As EventArgs) Handles txtM_Comment.TextChanged
        If btnUpdate.Enabled = False Then btnUpdate.Enabled = True
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If UserLogon_.UserName = "" Then
            Exit Sub
        End If

        Call_Procedure_UpdateComment()
        btnUpdate.Enabled = False

        'Dim frm As Form = GetFormByName(frmName)
        If flagStatusChanged = True Then
            Dim ctrl As Control = GetControlByName(lblM_Obj.Text)
            If lblM_Status.Tag = 0 Then
                DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.NOT_SUCCESS
            Else
                DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).Status_Test_IO = TAT_MQTT_CTRL.ctrlTAT_.Enum_Status_TestIO.TEST_SUCCESS
            End If
            flagStatusChanged = False
        End If
        'RaiseEvent_Reload_StatusIO(frm, cnDB)
    End Sub

    Private Sub Call_Procedure_UpdateComment()
        Dim strSQL As String = "Exec dbo.FD_Update_Comment_TestIO "
        strSQL += "@Pm_type = '" & lblM_Name.Text & "', "
        strSQL += "@Pm_index = '" & lblM_Index.Text & "', "
        strSQL += "@Pm_plc_station = '" & lblM_PLC.Text & "', "
        strSQL += "@Pm_status = " & lblM_Status.Tag & ", "
        strSQL += "@Pm_User = '" & UserLogon_.UserName & "', "
        strSQL += "@Pm_Comment = N'" & txtM_Comment.Text.Trim & "' "
        cnDB.ExecuteNoneQuery(strSQL)
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        txtM_Comment.Enabled = MDI_FRM.Main_Scada
    End Sub

    Private Function GetControlByName(ctrlName As String) As Control
        Return ControlsList.FirstOrDefault(Function(ctrl) ctrl.Name = ctrlName)
    End Function
End Class