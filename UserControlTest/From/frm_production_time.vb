Imports System.Globalization

Public Class frm_production_time
    Dim Cn As New clsDB(Batching_Conf.Name, Batching_Conf.User, Batching_Conf.Password, Batching_Conf.IPAddress, Batching_Conf.Connection_Type)
    Private Sub frm_production_time_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call Get_DateTime()
    End Sub

    Private Sub Get_DateTime()
        Dim strsql As String
        Dim rsProduciton_Location As New DataTable
        Dim tmp_PDate As Date
        Dim tmp_PTime As String
        Try
            strsql = " Select * From thaisia.current_status Order By c_location "
            rsProduciton_Location = Cn.ExecuteDataTable(strsql)
            If rsProduciton_Location.Rows.Count > 0 Then
                'DateTime.ParseExact(rsProduciton_Location.Rows(0)("c_production_date"), "dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("en-GB")).ToString()
                tmp_PDate = DateTime.ParseExact(rsProduciton_Location.Rows(0)("c_production_date"), "dd/MM/yyyy", CultureInfo.CreateSpecificCulture("en-GB")).ToString()
                tmp_PTime = rsProduciton_Location.Rows(0)("c_production_time") & ""
                dtpDate.Value = tmp_PDate
                dtpTime.Text = tmp_PTime
                If Trim(UCase(rsProduciton_Location.Rows(0)("c_autoupdate"))) = "M" Then
                    optAuto.Checked = False
                    optManual.Checked = True
                Else
                    optAuto.Checked = True
                    optManual.Checked = False
                End If
            End If

            Dim Shift As String = rsProduciton_Location.Rows(0)("c_use_shift")

            If Shift = "0" Then
                chkShift.Checked = False
                pan_Shift.Enabled = False
            Else
                chkShift.Checked = True
                pan_Shift.Enabled = True
                If rbShift1.Checked = True Then
                    S1.Enabled = True
                    S1_.Enabled = True
                    Dim tim() As String = CStr(rsProduciton_Location.Rows(0)("c_shift1")).Split("-")
                    S1.Text = tim(0)
                    S1_.Text = tim(1)
                ElseIf rbShift2.Checked = True Then
                    S1.Enabled = True
                    S1_.Enabled = True
                    S2.Enabled = True
                    S2_.Enabled = True
                    Dim tim() As String = CStr(rsProduciton_Location.Rows(0)("c_shift1")).Split("-")
                    S1.Text = tim(0)
                    S1_.Text = tim(1)
                    Dim tim1() As String = CStr(rsProduciton_Location.Rows(0)("c_shift2")).Split("-")
                    S2.Text = tim1(0)
                    S2_.Text = tim1(1)
                ElseIf rbShift3.Checked = True Then
                    S1.Enabled = True
                    S1_.Enabled = True
                    S2.Enabled = True
                    S2_.Enabled = True
                    S3.Enabled = True
                    S3_.Enabled = True
                    Dim tim() As String = CStr(rsProduciton_Location.Rows(0)("c_shift1")).Split("-")
                    S1.Text = tim(0)
                    S1_.Text = tim(1)
                    Dim tim1() As String = CStr(rsProduciton_Location.Rows(0)("c_shift2")).Split("-")
                    S2.Text = tim1(0)
                    S2_.Text = tim1(1)
                    Dim tim2() As String = CStr(rsProduciton_Location.Rows(0)("c_shift3")).Split("-")
                    S3.Text = tim2(0)
                    S3_.Text = tim2(1)
                End If
            End If
            If IsDBNull(rsProduciton_Location.Rows(0)("c_use_shift")) = True Then
                chkShift.Checked = False
                pan_Shift.Enabled = False
            End If
            '   ====

        Catch ex As Exception
            MsgBox("Error Number : " & Err.Number & vbCrLf & "Error Description : " & ex.Message & "Error In Function : Get_DateTime", vbExclamation, "Error")
        End Try
    End Sub

    Dim LogError As New clsIO
    Private Sub btSave_Click(sender As Object, e As EventArgs) Handles btSave.Click
        Try
            Update_Production_Setting()
        Catch ex As Exception
            '==== Msg Error
            '        If Error_Check = True Then Exit Sub
            '   Error_Check = True
            Dim strMessage = "Error Number :  " & Err.Number & " [" + Me.Name + "]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace

            LogError.writeErr(strMessage)
        End Try

    End Sub


    Sub Update_Production_Setting()
        Dim strsql As String
        Dim strTmp_Mode As String
        Dim tmpDateSave As String
        Dim tmpTimeSave As String
        Try
            If optAuto.Checked = True Then
                strTmp_Mode = "A"
            Else
                strTmp_Mode = "M"
            End If
            tmpDateSave = dtpDate.Text
            tmpTimeSave = dtpTime.Text
            strsql = " Update thaisia.current_status  SET "
            strsql = strsql & " c_production_date = '" & tmpDateSave & "', "
            strsql = strsql & " c_production_time = '" & tmpTimeSave & "', "
            strsql = strsql & " c_autoupdate = '" & strTmp_Mode & "' "
            Cn.ExecuteNoneQuery(strsql)

            If chkShift.Checked = True Then
                strsql = " Update thaisia.current_status  SET  n_num_shift = '1'  ,c_use_shift = '1'  ,"
                If rbShift1.Checked = True Then
                    strsql = strsql & " c_shift1 = '" & S1.Text & "-" + S1_.Text + "' "

                ElseIf rbShift2.Checked = True Then
                    strsql = strsql & " c_shift1 = '" & S1.Text & "-" + S1_.Text + "' ,"
                    strsql = strsql & " c_shift2 = '" & S2.Text & "-" + S2_.Text + "' "

                ElseIf rbShift3.Checked = True Then
                    strsql = strsql & " c_shift1 = '" & S1.Text & "-" + S1_.Text + "' ,"
                    strsql = strsql & " c_shift2 = '" & S2.Text & "-" + S2_.Text + "' ,"
                    strsql = strsql & " c_shift3 = '" & S3.Text & "-" + S3_.Text + "' "

                End If
            Else
                strsql = " Update thaisia.current_status  SET  n_num_shift = '0' ,c_use_shift = '0'  "
            End If
            Cn.ExecuteNoneQuery(strsql)
        Catch ex As Exception
            MsgBox("Error Number : " & Err.Number & vbCrLf & "Error Description : " & ex.Message & "Error In Function : Update_Production_Setting", vbExclamation, "Error")
        End Try

    End Sub

    Private Sub btClose_Click(sender As Object, e As EventArgs) Handles btClose.Click
        Me.Close()
    End Sub

    Private Sub c_shift_CheckedChanged(sender As Object, e As EventArgs) Handles chkShift.CheckedChanged

        If chkShift.Checked = True Then
            pan_Shift.Enabled = True
        Else
            pan_Shift.Enabled = False
        End If
    End Sub

    Private Sub rbShift1_CheckedChanged(sender As Object, e As EventArgs) Handles rbShift1.CheckedChanged, rbShift2.CheckedChanged, rbShift3.CheckedChanged
        S1.Enabled = False
        S1_.Enabled = False
        S2.Enabled = False
        S2_.Enabled = False
        S3.Enabled = False
        S3_.Enabled = False
        If rbShift1.Checked = True Then
            S1.Enabled = True
            S1_.Enabled = True
        ElseIf rbShift2.Checked = True Then
            S1.Enabled = True
            S1_.Enabled = True
            S2.Enabled = True
            S2_.Enabled = True
        ElseIf rbShift3.Checked = True Then
            S1.Enabled = True
            S1_.Enabled = True
            S2.Enabled = True
            S2_.Enabled = True
            S3.Enabled = True
            S3_.Enabled = True
        End If
    End Sub

End Class