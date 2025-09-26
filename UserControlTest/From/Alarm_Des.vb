Public Class Alarm_Des
    Dim Scada(5) As Form
    Dim MX As New cls_MXCompronent
    Dim LogError As New clsIO
    Public Sub New(From_() As Form, MX_ As cls_MXCompronent)
        ' This call is required by the designer.
        InitializeComponent()
        Scada = From_
        MX = MX_
    End Sub
    Private Structure Alarm_motor
        Dim n_no As String
        Dim c_name As String
        Dim c_dis As String
        Dim c_time As String
        Dim c_address As String
        Dim c_plc_no As String
    End Structure
    Dim Dt_temp As DataTable
    Dim CheckRow As Integer
    Dim Dr As DataRow
    Dim J As Integer
    Dim M_reset As String
    Dim PLC_NO_TEMP As String

    Private Sub ShowAlarmMotor(sender As Object)
        Dim Arr_Add As New Alarm_motor
        Dim Arr_str(5) As String
        Dim Color_rows As Color
        Dim ctrl As TAT_MQTT_CTRL.ctrlTAT_ = CType(sender, TAT_MQTT_CTRL.ctrlTAT_)

        If Dt_temp.Columns.Count = 0 Then
            Dt_temp.Columns.Add("Motor_code")
            Dt_temp.Columns.Add("Obj_Name")
            Dt_temp.Columns.Add("Obj_Type")
            Dt_temp.Columns.Add("Index")
        Else
            If fn_filter_data(" Motor_code  = '" + ctrl.M_Code + "'", Dt_temp).Rows.Count > 0 Then Exit Sub
        End If

        If (ctrl.status_err1 = True) And (ctrl.Visible = True) Then
            Arr_Add.c_name = ctrl.M_Code
            Select Case ctrl.ControlType.ToString
                Case "MOTOR"
                    Arr_Add.c_dis = "Motor Error !"
                Case "SLIDE"
                    Arr_Add.c_dis = "Slide not open !"
                Case "FLAP"
                    Arr_Add.c_dis = "Flap box Error !"
            End Select
            Arr_Add.c_time = ctrl.time_error
            Arr_Add.c_address = ctrl.Ad_Err1
            Arr_Add.c_plc_no = ctrl.PLC_Station_No
            Color_rows = Color.Red
        End If

        If (ctrl.status_err2 = True) And (ctrl.Visible = True) Then
            Arr_Add.c_name = ctrl.M_Code

            Select Case ctrl.ControlType.ToString
                Case "MOTOR"
                    Arr_Add.c_dis = "Motor Tip !"
                    Color_rows = Color.Red
                Case "SLIDE"
                    Arr_Add.c_dis = "Slide not close !"
                    Color_rows = Color.Yellow
                Case "FLAP"
                    Arr_Add.c_dis = "Flap box Error !"
                    Color_rows = Color.Red
            End Select
            Arr_Add.c_time = ctrl.time_error
            Arr_Add.c_address = ctrl.Ad_Err2
            Arr_Add.c_plc_no = ctrl.PLC_Station_No
        End If

        Dr = Dt_temp.NewRow
        Dr("Motor_code") = Arr_Add.c_name
        Dr("Obj_Name") = ctrl.Name
        Dr("Obj_Type") = sender.GetType.Name
        Dr("Index") = Dt_temp.Rows.Count
        Dt_temp.Rows.Add(Dr)

        Arr_Add.n_no = Dt_temp.Rows.Count
        Arr_str(0) = Arr_Add.n_no
        Arr_str(1) = Arr_Add.c_name
        Arr_str(2) = Arr_Add.c_dis
        Arr_str(3) = Arr_Add.c_time
        Arr_str(4) = Arr_Add.c_address
        Arr_str(5) = Arr_Add.c_plc_no
        DataGridView1.Rows.Add(Arr_str.ToArray)
        DataGridView1.Rows(Arr_Add.n_no - 1).DefaultCellStyle.BackColor = Color.Black
        DataGridView1.Rows(Arr_Add.n_no - 1).DefaultCellStyle.ForeColor = Color_rows
        DataGridView1.Rows(Arr_Add.n_no - 1).DefaultCellStyle.Font = New Font("Tahoma", 8, FontStyle.Bold)
    End Sub

    Private Sub ShowAlarmHi_Lo(sender As Object, ByVal chkBinRef As Boolean)
        Dim Arr_Add As New Alarm_motor
        Dim Arr_str(5) As String
        Dim Color_rows As Color
        Dim ctrl As TAT_CtrlBin.ctrlBin_ = CType(sender, TAT_CtrlBin.ctrlBin_)

        If chkBinRef = True Then
            Arr_Add.c_name = ctrl.mqtt_bin_ref_config_.BIN_CODE
        Else
            Arr_Add.c_name = ctrl.mqtt_bin_parameter_config_.BIN_CODE
        End If

        If Dt_temp.Columns.Count = 0 Then
            Dt_temp.Columns.Add("Motor_code")
            Dt_temp.Columns.Add("Obj_Name")
            Dt_temp.Columns.Add("Obj_Type")
            Dt_temp.Columns.Add("Index")
        Else
            If fn_filter_data(" Motor_code  = '" + Arr_Add.c_name + "'", Dt_temp).Rows.Count > 0 Then Exit Sub
        End If

        If chkBinRef = True Then
            If (ctrl.mqtt_bin_ref_status_.STA_HI = True) And (ctrl.Visible = True) Then
                Arr_Add.c_dis = "Bin High !"
                Arr_Add.c_time = ctrl.mqtt_bin_ref_status_.LAST_UPDATE
                Arr_Add.c_address = ctrl.mqtt_bin_ref_config_.M_HI
                Arr_Add.c_plc_no = Replace(ctrl.CCB.ToString, "CCB", "")
                Color_rows = Color.Red
            End If

            If (ctrl.mqtt_bin_ref_status_.STA_LO = True) And (ctrl.Visible = True) Then
                Arr_Add.c_dis = "Bin Low !"
                Arr_Add.c_time = ctrl.mqtt_bin_ref_status_.LAST_UPDATE
                Arr_Add.c_address = ctrl.mqtt_bin_ref_config_.M_HI
                Arr_Add.c_plc_no = Replace(ctrl.CCB.ToString, "CCB", "")
                Color_rows = Color.Yellow
            End If
        Else
            If (ctrl.mqtt_bin_parameter_status_.STA_HI = True) And (ctrl.Visible = True) Then
                Arr_Add.c_dis = "Bin High !"
                Arr_Add.c_time = ctrl.mqtt_bin_parameter_status_.LAST_UPDATE
                Arr_Add.c_address = ctrl.mqtt_bin_parameter_config_.M_HI
                Arr_Add.c_plc_no = Replace(ctrl.CCB.ToString, "CCB", "")
                Color_rows = Color.Red
            End If

            If (ctrl.mqtt_bin_parameter_status_.STA_LO = True) And (ctrl.Visible = True) Then
                Arr_Add.c_dis = "Bin Low !"
                Arr_Add.c_time = ctrl.mqtt_bin_parameter_status_.LAST_UPDATE
                Arr_Add.c_address = ctrl.mqtt_bin_parameter_config_.M_HI
                Arr_Add.c_plc_no = Replace(ctrl.CCB.ToString, "CCB", "")
                Color_rows = Color.Yellow
            End If
        End If



        Dr = Dt_temp.NewRow
        Dr("Motor_code") = Arr_Add.c_name
        Dr("Obj_Name") = ctrl.Name
        Dr("Obj_Type") = sender.GetType.Name
        Dr("Index") = Dt_temp.Rows.Count
        Dt_temp.Rows.Add(Dr)

        Arr_Add.n_no = Dt_temp.Rows.Count
        Arr_str(0) = Arr_Add.n_no
        Arr_str(1) = Arr_Add.c_name
        Arr_str(2) = Arr_Add.c_dis
        Arr_str(3) = Arr_Add.c_time
        Arr_str(4) = Arr_Add.c_address
        Arr_str(5) = Arr_Add.c_plc_no
        DataGridView1.Rows.Add(Arr_str.ToArray)
        DataGridView1.Rows(Arr_Add.n_no - 1).DefaultCellStyle.BackColor = Color.Black
        DataGridView1.Rows(Arr_Add.n_no - 1).DefaultCellStyle.ForeColor = Color_rows
        DataGridView1.Rows(Arr_Add.n_no - 1).DefaultCellStyle.Font = New Font("Tahoma", 8, FontStyle.Bold)

    End Sub

    Private Sub ClearAlarm(dtCtrl As DataTable)
        Dim drTemp As DataRow = dtCtrl.Rows(0)
        Dim tmpIndex As Integer = dtCtrl.Rows(0)("Index")
        Dt_temp.Rows.RemoveAt(tmpIndex)
        DataGridView1.Rows.Remove(DataGridView1.Rows(tmpIndex))
        If Dt_temp.Rows.Count > 0 Then
            For i = 0 To Dt_temp.Rows.Count - 1
                Dt_temp.Rows(i)("Index") = i
                DataGridView1.Rows(i).Cells(0).Value = i + 1
            Next
        Else
            DataGridView1.Rows.Clear()
            Me.Hide()
        End If

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        For i As Integer = 0 To Scada.Count - 1
            For Each ctrl As Control In Scada(i).Controls
                If TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
                    If (CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_).status_err1 = True) And (ctrl.Visible = True) Then
                        ShowAlarmMotor(ctrl)
                    ElseIf (CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_).status_err2 = True) And (ctrl.Visible = True) Then
                        ShowAlarmMotor(ctrl)
                    ElseIf (CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_).status_err2 = False) And (CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_).status_err1 = False) Then
                        If Dt_temp.Rows.Count > 0 Then
                            Using dtCtrl As DataTable = fn_filter_data(" Motor_code  = '" + CType(ctrl, TAT_MQTT_CTRL.ctrlTAT_).M_Code + "'", Dt_temp)
                                If dtCtrl.Rows.Count > 0 Then
                                    ClearAlarm(dtCtrl)
                                End If
                            End Using
                        End If
                    End If
                ElseIf TypeOf ctrl Is TAT_CtrlBin.ctrlBin_ Then
                    Dim Bin_ As TAT_CtrlBin.ctrlBin_ = ctrl
                    'If Bin_.BinNo = 399 Then
                    '    MsgBox("Debug")
                    'End If

                    If (Bin_.mqtt_bin_parameter_status_.STA_HI = True) And (Bin_.Visible = True) Then
                        ShowAlarmHi_Lo(ctrl, False)
                    ElseIf (Bin_.mqtt_bin_parameter_status_.STA_LO = True) And (Bin_.Visible = True) Then
                        ShowAlarmHi_Lo(ctrl, False)
                        'ElseIf (Bin_.mqtt_bin_parameter_status_.STA_HI = False) And (Bin_.mqtt_bin_parameter_status_.STA_LO = False) Then
                        '    If Dt_temp.Rows.Count > 0 Then
                        '        Using dtCtrl As DataTable = fn_filter_data(" Motor_code  = '" +
                        '             CType(ctrl, TAT_CtrlBin.ctrlBin_).mqtt_bin_parameter_config_.BIN_CODE + "'", Dt_temp)
                        '            If dtCtrl.Rows.Count > 0 Then
                        '                ClearAlarm(dtCtrl)
                        '            End If
                        '        End Using
                        '    End If
                    ElseIf (Bin_.mqtt_bin_ref_status_.STA_HI = True) And (Bin_.Visible = True) Then
                        ShowAlarmHi_Lo(ctrl, True)
                    ElseIf (Bin_.mqtt_bin_ref_status_.STA_LO = True) And (Bin_.Visible = True) Then
                        ShowAlarmHi_Lo(ctrl, True)
                    ElseIf ((Bin_.mqtt_bin_parameter_status_.STA_HI = False) And (Bin_.mqtt_bin_parameter_status_.STA_LO = False)) And
                        ((Bin_.mqtt_bin_ref_status_.STA_HI = False) And (Bin_.mqtt_bin_ref_status_.STA_LO = False)) Then
                        If Dt_temp.Rows.Count > 0 Then
                            Using dtCtrl As DataTable = fn_filter_data(" Motor_code  = '" +
                                 CType(ctrl, TAT_CtrlBin.ctrlBin_).mqtt_bin_parameter_config_.BIN_CODE + "'", Dt_temp)
                                If dtCtrl.Rows.Count > 0 Then
                                    ClearAlarm(dtCtrl)
                                End If
                            End Using
                            Using dtCtrl As DataTable = fn_filter_data(" Motor_code  = '" +
                                 CType(ctrl, TAT_CtrlBin.ctrlBin_).mqtt_bin_ref_config_.BIN_CODE + "'", Dt_temp)
                                If dtCtrl.Rows.Count > 0 Then
                                    ClearAlarm(dtCtrl)
                                End If
                            End Using
                        End If
                    End If
                End If
            Next
        Next

        If CheckRow < CInt(DataGridView1.Rows.Count) Then
            Me.Show()
            CheckRow = DataGridView1.Rows.Count
        ElseIf DataGridView1.Rows.Count = 0 Then
            Me.Hide()
            CheckRow = 0
        Else
            CheckRow = DataGridView1.Rows.Count
        End If

        PictureBox1.Visible = Not PictureBox1.Visible

    End Sub

    Private Sub TimerCheck_Tick(sender As Object, e As EventArgs) Handles TimerCheck.Tick
        '        Try
        '            Dt_temp = New DataTable
        '            Dt_temp.Columns.Add("Motor_code")
        '            DataGridView1.Rows.Clear()
        '            Dim Arr_Add As Alarm_motor
        '            Dim Arr_str() As String
        '            Dim num As Integer
        '            Dim Color_rows As Color
        '            For i As Integer = 0 To Scada.Count - 1
        '                For Each ctrl As Control In Scada(i).Controls
        '                    Arr_Add = New Alarm_motor
        '                    ReDim Arr_str(5)
        '                    If TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
        '                        Dim ctrl_ As TAT_MQTT_CTRL.ctrlTAT_ = ctrl
        '                        If (ctrl_.status_err1 = True) And (ctrl_.Visible = True) Then
        '                            Arr_Add.c_name = ctrl_.M_Code
        '                            Select Case ctrl_.ControlType.ToString
        '                                Case "MOTOR"
        '                                    Arr_Add.c_dis = "Motor Tip !"
        '                                Case "SLIDE"
        '                                    Arr_Add.c_dis = "Slide not open !"
        '                                Case "FLAP"
        '                                    Arr_Add.c_dis = "Flap box Error !"
        '                            End Select
        '                            Arr_Add.c_time = ctrl_.time_error
        '                            Arr_Add.c_address = ctrl_.Ad_Err1
        '                            Arr_Add.c_plc_no = ctrl_.PLC_Station_No
        '                            Color_rows = Color.Red
        '                        End If

        '                        If (ctrl_.status_err2 = True) And (ctrl_.Visible = True) Then
        '                            Arr_Add.c_name = ctrl_.M_Code

        '                            Select Case ctrl_.ControlType.ToString
        '                                Case "MOTOR"
        '                                    Arr_Add.c_dis = "Motor Error !"
        '                                    Color_rows = Color.Red
        '                                Case "SLIDE"
        '                                    Arr_Add.c_dis = "Slide not close !"
        '                                    Color_rows = Color.Yellow
        '                                Case "FLAP"
        '                                    Arr_Add.c_dis = "Flap box Error !"
        '                                    Color_rows = Color.Red
        '                            End Select
        '                            Arr_Add.c_time = ctrl_.time_error
        '                            Arr_Add.c_address = ctrl_.Ad_Err2
        '                            Arr_Add.c_plc_no = ctrl_.PLC_Station_No
        '                        End If
        '                    ElseIf TypeOf ctrl Is TAT_CtrlBin.ctrlBin_ Then
        '                        Dim Bin_ As TAT_CtrlBin.ctrlBin_ = ctrl
        '                        If (Bin_.mqtt_bin_parameter_status_.STA_HI = True) And (Bin_.Visible = True) Then
        '                            Arr_Add.c_name = Bin_.mqtt_bin_parameter_config_.BIN_CODE
        '                            Arr_Add.c_dis = "High Destination"
        '                            Arr_Add.c_time = Bin_.mqtt_bin_parameter_status_.LAST_UPDATE
        '                            Color_rows = Color.Red
        '                        End If
        '                    End If
        '                    If Arr_Add.c_name Is Nothing Then GoTo Nex

        '                    If fn_filter_data(" Motor_code  = '" + Arr_Add.c_name + "'", Dt_temp).Rows.Count > 0 Then GoTo Nex
        '                    Dr = Dt_temp.NewRow
        '                    Dr("Motor_code") = Arr_Add.c_name
        '                    Dt_temp.Rows.Add(Dr)
        '                    num += 1
        '                    Arr_Add.n_no = CStr(num)
        '                    Arr_str(0) = Arr_Add.n_no
        '                    Arr_str(1) = Arr_Add.c_name
        '                    Arr_str(2) = Arr_Add.c_dis
        '                    Arr_str(3) = Arr_Add.c_time
        '                    Arr_str(4) = Arr_Add.c_address
        '                    Arr_str(5) = Arr_Add.c_plc_no
        '                    DataGridView1.Rows.Add(Arr_str.ToArray)
        '                    DataGridView1.Rows(num - 1).DefaultCellStyle.BackColor = Color.Black
        '                    DataGridView1.Rows(num - 1).DefaultCellStyle.ForeColor = Color_rows
        '                    DataGridView1.Rows(num - 1).DefaultCellStyle.Font = New Font("Tahoma", 8, FontStyle.Bold)

        'Nex:
        '                Next
        '            Next
        '            If PictureBox1.Visible = True Then
        '                PictureBox1.Visible = False
        '            Else
        '                PictureBox1.Visible = True
        '            End If
        '            If CheckRow < CInt(DataGridView1.Rows.Count) Then
        '                Me.Show()
        '                CheckRow = DataGridView1.Rows.Count
        '            ElseIf DataGridView1.Rows.Count = 0 Then
        '                Me.Hide()
        '                CheckRow = 0
        '            Else
        '                CheckRow = DataGridView1.Rows.Count
        '            End If
        'Catch ex As Exception
        '    Dim strMessage = "Error Number :  " & Err.Number & " [Alarm_Des/TimerCheck_Tick]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
        '    LogError.writeErr(strMessage)
        'End Try
    End Sub

    Public Function fn_filter_data(query_Datatabel As String, selectDataTabel As DataTable) As DataTable
        Try
            Dim dv As New DataView(selectDataTabel)
            dv.RowFilter = query_Datatabel
            Dim dtv As New DataTable
            dtv = dv.ToTable
            fn_filter_data = dtv
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [Alarm_Des/fn_filter_data]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If M_reset Is Nothing Then Exit Sub
            Dim MX_TEMP As New cls_MXCompronent
            MX_TEMP.MxCom1 = frmMxComponent.AxActUtlType1
            Call MX_TEMP.mxSetLogicalNumber(MX_TEMP.MxCom1, PLC_NO_TEMP)
            MX_TEMP.mxOpenPort(MX_TEMP.MxCom1)
            MX_TEMP.mxDevSetM(MX_TEMP.MxCom1, M_reset, 0)
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [Alarm_Des/Button1_Click]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        Try
            M_reset = DataGridView1.Rows.Item(e.RowIndex).Cells(4).Value.ToString
            PLC_NO_TEMP = DataGridView1.Rows.Item(e.RowIndex).Cells(5).Value.ToString
        Catch ex As Exception
            If InStr(ex.Message, "Object reference not") > 0 Then
                M_reset = "M0"
                Exit Sub
            End If

        End Try

    End Sub


    Private Sub DataGridView1_MouseHover(sender As Object, e As EventArgs) Handles DataGridView1.MouseHover
        TimerCheck.Enabled = False
    End Sub

    Private Sub DataGridView1_MouseLeave(sender As Object, e As EventArgs) Handles DataGridView1.MouseLeave
        TimerCheck.Enabled = True
    End Sub

    Private Sub Alarm_Des_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Me.TopMost = True
            Me.Opacity = 50
            Me.Icon = My.Resources.SYS_AlarmIcon_256x256

            NotifyIcon1.Icon = My.Resources.SYS_AlarmIcon_256x256
            Button2.Text = "ALARM DESCRIPTION "
            Button4.Text = Application.ProductName
            NotifyIcon1.Text = Button2.Text
            Me.MaximumSize = New Size(Me.Size)
            Me.MinimumSize = New Size(Me.Size)

            DataGridView1.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter


            DataGridView1.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            DataGridView1.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            Dt_temp = New DataTable
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [Alarm_Des/Button1_Click]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Hide()
        NotifyIcon1.BalloonTipTitle = Button2.Text
        NotifyIcon1.BalloonTipText = "Double-click the icon to open again."
        Me.NotifyIcon1.Visible = True
        Me.NotifyIcon1.ShowBalloonTip(0)
    End Sub


    Private Sub NotifyIcon1_DoubleClick(sender As Object, e As EventArgs) Handles NotifyIcon1.DoubleClick
        Me.Show()
    End Sub

    Private Sub Alarm_Des_Activated(sender As Object, e As EventArgs) Handles Me.Activated

    End Sub
End Class