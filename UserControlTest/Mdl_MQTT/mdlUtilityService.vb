Imports System.IO


Module mdlUtilityService
    Dim IO As New clsIO

    Public Function Get_Manual_Bin_CmdLine(ByVal stnNo As String, ByVal scNo As String, ByVal dtSC As DataTable) As String
        Dim dtv As New DataTable
        Dim addHeader As String
        Dim strSC_No As String
        Dim addWeight As String
        Dim addSpeed As String
        Dim addJogTime As String
        Dim addTN As String
        Dim addTS As String

        If dtSC.Rows.Count <> 0 Then
            dtv = fn_filter_data("n_plc_station = '" & stnNo & "' AND scale_no = '" & scNo & "'", dtSC)
            If dtv.Rows.Count > 0 Then
                If IsDBNull(dtv.Rows(0)("d_address")) = False Then
                    addHeader = dtv.Rows(0)("d_address")
                    addSpeed = CheckAdd(addHeader, 25)
                    addWeight = CheckAdd(addHeader, 30)
                Else
                    addHeader = "D0"
                    addSpeed = "D0"
                    addWeight = "D0"
                End If
                strSC_No = dtv.Rows(0)("scale_no")
                addJogTime = dtv.Rows(0)("d_jog_time")
                'If IsDBNull(dtv.Rows(0)("time_joging")) = False Then
                '    addTN = Replace(dtv.Rows(0)("time_joging"), "T", "TN")
                '    addTS = Replace(dtv.Rows(0)("time_joging"), "T", "TS")
                'Else
                '    addTN = "TN0"
                '    addTS = "TS0"
                'End If

                If IsDBNull(dtv.Rows(0)("time_joging")) = False And dtv.Rows(0)("time_joging").ToString <> "" Then
                    addTN = Replace(dtv.Rows(0)("time_joging"), "T", "TN")
                    addTS = Replace(dtv.Rows(0)("time_joging"), "T", "TS")
                Else
                    addTN = "TN0"
                    addTS = "TS0"
                End If

                Return strSC_No & " " & addWeight & " " & addJogTime & " " & addSpeed & " " & addTN & " " & addTS

            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function
    Public Function Get_Parameter_HA_SB_CmdLine(ByVal strName As String, ByVal strNo As String, ByVal PLC_stnNo As String, ByVal dtSB_HA As DataTable) As String
        Dim dtv As New DataTable
        Dim strAdd As String
        Dim strLocation As String
        Dim tmpCmd_Line As String

        If dtSB_HA.Rows.Count > 0 Then
            dtv = fn_filter_data("n_plc_station = '" & PLC_stnNo & "' AND motor_name = '" & strName & "'", dtSB_HA)
            If dtv.Rows.Count > 0 Then
                'strAdd = dtv.Rows(0)("d_address")
                If IsDBNull(dtv.Rows(0)("d_address")) Then strAdd = "D0" Else strAdd = dtv.Rows(0)("d_address")
                If IsDBNull(dtv.Rows(0)("c_location")) Then strLocation = "HANDADD" Else strLocation = dtv.Rows(0)("c_location")
                tmpCmd_Line = PLC_stnNo & " " & strNo & " " & strAdd & " " & strName & " " & strLocation & " localhost ASA "
                Return tmpCmd_Line
            Else
                Return ""
            End If
        Else
            Return ""
        End If

    End Function

    Public Function Get_Parameter_Bin_CmdLine(ByVal stnNo As String, ByVal strBinNo As String, ByVal strBin_Step As String, ByVal strM_Low As String, ByVal dtBin As DataTable, ByVal strUserCode As String) As String
        Dim dtv As New DataTable
        Dim strBin_Code As String
        Dim add_bin As String
        Dim strBin_index As String
        Dim strSc_No As String
        Dim strLocation As String
        Dim tmpCmd_Line As String

        If dtBin.Rows.Count > 0 Then
            dtv = fn_filter_data("n_plc_station = '" & stnNo & "' AND bin_no = '" & strBinNo & "'", dtBin)
            If dtv.Rows.Count > 0 Then
                '1 23 7  B024 D5690 3  localhost ASA BATCHING_1 -  2 /bin_update  100 localhost M8768 1
                strBin_Code = dtv.Rows(0)("bin_code")
                add_bin = dtv.Rows(0)("d_address")
                strBin_index = dtv.Rows(0)("bin_index")
                strSc_No = dtv.Rows(0)("scale_no")
                strLocation = Replace(dtv.Rows(0)("c_location"), " ", "_")
                tmpCmd_Line = stnNo & " " & strBinNo & " " & strBin_index & " " & strBin_Code & " " & add_bin & " " & strSc_No & " localhost ASA " &
                              strLocation & " - " & stnNo & " /bin_update " & strBin_Step & " localhost " & strM_Low & " " & stnNo & " " & strUserCode
                Return tmpCmd_Line
            Else
                Return ""
            End If
        Else
            Return ""
        End If
    End Function

    Public Function fn_filter_data(query_Datatabel As String, selectDataTabel As DataTable) As DataTable
        Try
            Dim dv As New DataView(selectDataTabel)
            dv.RowFilter = query_Datatabel
            Dim dtv As New DataTable
            dtv = dv.ToTable
            fn_filter_data = dtv
        Catch ex As Exception
            'Log.writeLogEvent(Application.StartupPath, "::" + ex.Message + " ::In Function [Change_Data_BIN]")
        End Try
    End Function

    Function CheckAdd(asAddressBegin As String, intCount As Integer) As String
        On Error GoTo ErrHandle

        Dim RetAdd As String

        Select Case UCase(Microsoft.VisualBasic.Left(asAddressBegin, 1))
            Case Is = "R"
                RetAdd = Microsoft.VisualBasic.Left(asAddressBegin, 1) & Mid$(asAddressBegin, 2) + intCount
            Case Is = "Z"
                RetAdd = Microsoft.VisualBasic.Left(asAddressBegin, 2) & Mid$(asAddressBegin, 3) + intCount
            Case Is = "D"
                RetAdd = Microsoft.VisualBasic.Left(asAddressBegin, 1) & Mid$(asAddressBegin, 2) + intCount
        End Select
        CheckAdd = RetAdd


ExitHere:
        Exit Function

ErrHandle:
        MsgBox("Error Number : " & Err.Number & vbCrLf & "Error Description : " & Err.Description & vbCrLf & "Error In : CheckAdd", vbExclamation + vbOKOnly, "Func_GetFlagStopMotor")
        Resume ExitHere

    End Function

    Public Sub Call_EXE(Cmd As String, Agument_Commandlind As String, OBJECT_NAME As String)
        Try
            Select Case Cmd
                Case Is = "Set Delay Time"
                    IO.Open_Application("Delay_time", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Set Delay Time ,CMD:" + Agument_Commandlind + ",EXE:Delay_time]")
                Case Is = "Parameter"
                    IO.Open_Application("Liquid_parameter", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Parameter ,CMD:" + Agument_Commandlind + ",EXE:Liquid_parameter]")
                Case Is = "Manual Operate Bin"
                    'IO.Open_Application("Manual_Bin", Agument_Commandlind)
                    IO.Open_Application("Manual_Operate_Bin", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Manual Operate Bin ,CMD:" + Agument_Commandlind + ",EXE:Manual_Bin]")
                Case Is = "Manual Operate LQ"
                    IO.Open_Application("Parameter_Config", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Manual Operate LQ ,CMD:" + Agument_Commandlind + ",EXE:Manual Liquid]")
                Case Is = "BinParameter"
                    IO.Open_Application("BinParameter", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :BinParameter ,CMD:" + Agument_Commandlind + ",EXE:Bin_Parameter]")
                Case Is = "Scale Parameter"
                    IO.Open_Application("Scale Parameter", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Scale Parameter ,CMD:" + Agument_Commandlind + ",EXE:Scale Parameter]")
                Case Is = "Molass Parameter"
                    IO.Open_Application("Molass Parameter", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Molass Parameter ,CMD:" + Agument_Commandlind + ",EXE:Molass Parameter]")

                Case Is = "Parameter_surgebin"
                    IO.Open_Application("Parameter_surgebin", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Parameter_surgebin ,CMD:" + Agument_Commandlind + ",EXE:Parameter_surgebin]")
                Case Is = "Liquid_parameter"
                    IO.Open_Application("Liquid_parameter", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Liquid_parameter ,CMD:" + Agument_Commandlind + ",EXE:Liquid_parameter]")
                Case Is = "Mixer Parameter"
                    IO.Open_Application("Mixer Parameter", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Mixer Parameter ,CMD:" + Agument_Commandlind + ",EXE:Mixer Parameter]")
                Case Is = "Handadd Parameter"
                    'IO.Open_Application("Handadd Parameter", Agument_Commandlind)
                    IO.Open_Application("Parameter_Config", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click : Handadd Parameter ,CMD:" + Agument_Commandlind + ",EXE: Handadd Parameter]")
                Case Is = "Formula_Monitoring"
                    IO.Open_Application("Monitoring_Formula", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click : Handadd Parameter ,CMD:" + Agument_Commandlind + ",EXE: Handadd Parameter]")
                Case Is = "Current_Config"
                    IO.Open_Application("Current_Config", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click : Handadd Parameter ,CMD:" + Agument_Commandlind + ",EXE: Handadd Parameter]")

                Case Is = "PID Config"
                    IO.Open_Application("PID Parameter", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click : Handadd Parameter ,CMD:" + Agument_Commandlind + ",EXE: Handadd Parameter]")

                Case Is = "ObjectProperty"
                    IO.Open_Application("ObjectProperty", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Set Delay Time ,CMD:" + Agument_Commandlind + ",EXE:ObjectProperty]")

                Case Is = "Parameter_Config"
                    IO.Open_Application("Parameter_Config", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Parameter_Config ,CMD:" + Agument_Commandlind + ",EXE:Parameter_Config]")

                Case Is = "Monitoring_Formula"
                    IO.Open_Application("Monitoring_Formula", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Monitoring_Formula ,CMD:" + Agument_Commandlind + ",EXE:Monitoring_Formula]")

                Case Is = "ANALOG_PARAMETER"
                    IO.Open_Application("ANALOG_PARAMETER", Agument_Commandlind)
                    IO.writeLogEvent(Application.StartupPath, "::" + OBJECT_NAME + "[Click :Set Delay Time ,CMD:" + Agument_Commandlind + ",EXE:ANALOG_PARAMETER]")
                Case Else
                    MsgBox("NO COMMAND ", vbCritical, "Error Command")
                    Exit Sub


            End Select

        Catch ex As Exception
            IO.writeLogEvent(Application.StartupPath, "" + ex.Message + "::[CLICK IN MENU MOTO]")
        End Try
    End Sub

    Public Sub Show_PidInForm(ByVal frm As Form, ByRef DataREG() As Int16, ByVal M() As Boolean, ByVal bChanging As Boolean, ByRef nDelay As Integer)
        For Each tmpControls As Control In frm.Controls
            If TypeOf tmpControls Is TAT_UTILITY_CTRL.ctrlPID_ Then
                Dim ctrl As TAT_UTILITY_CTRL.ctrlPID_ = tmpControls
                With ctrl
                    If .Ref_Object <> "" Then
                        Dim ctrlMotor As TAT_MQTT_CTRL.ctrlTAT_ = CType(frm.Controls(.Ref_Object), TAT_MQTT_CTRL.ctrlTAT_)
                        .Address_AutoPID = ctrlMotor.Ad_Auto_PID
                        .Address_Hi = ctrlMotor.Ad_Analog_HI
                        .Address_Lo = ctrlMotor.Ad_Analog_LO
                        .Address_PV = ctrlMotor.Ad_Analog_PV
                        .Address_SV = ctrlMotor.Ad_Analog_SV
                        .Address_MV = ctrlMotor.Ad_Analog_MV

                        .PV_Value = ctrlMotor.objStatus.VAL_PV
                        .SV_Value = ctrlMotor.objStatus.VAL_SV
                        .MV_Value = ctrlMotor.objStatus.VAL_MV
                        .Status_AutoPID = ctrlMotor.status_pid
                        .M_Code = ctrlMotor.M_Code
                        If bChanging = False Then
                            If nDelay = 0 Then
                                .PV_PointerHi = ctrlMotor.objStatus.VAL_HI
                                .PV_PointerLo = ctrlMotor.objStatus.VAL_LO
                                .SV_PointerValue = ctrlMotor.objStatus.VAL_SV
                                .MV_PointerValue = ctrlMotor.objStatus.VAL_MV
                            Else
                                nDelay = nDelay - 1
                            End If

                        End If
                    Else
                        If .Address_PV <> "" AndAlso .Address_PV <> "R0" Then .PV_Value = DataREG(getNumAdd(.Address_PV))
                        If .Address_SV <> "" AndAlso .Address_SV <> "R0" Then .SV_Value = DataREG(getNumAdd(.Address_SV))
                        If .Address_MV <> "" AndAlso .Address_MV <> "R0" Then .MV_Value = DataREG(getNumAdd(.Address_MV))
                        If .Address_AutoPID <> "" AndAlso .Address_AutoPID <> "R0" Then .Status_AutoPID = M(getNumAdd(.Address_AutoPID))

                        If bChanging = False Then
                            If nDelay = 0 Then
                                If .Address_Hi <> "" AndAlso .Address_Hi <> "R0" Then .PV_PointerHi = DataREG(getNumAdd(.Address_Hi))
                                If .Address_Lo <> "" AndAlso .Address_Lo <> "R0" Then .PV_PointerLo = DataREG(getNumAdd(.Address_Lo))
                                If .Address_SV <> "" AndAlso .Address_SV <> "R0" Then .SV_PointerValue = DataREG(getNumAdd(.Address_SV))
                                If .Address_MV <> "" AndAlso .Address_MV <> "R0" Then .MV_PointerValue = DataREG(getNumAdd(.Address_MV))
                            Else
                                nDelay = nDelay - 1
                            End If

                        End If
                    End If
                End With
            ElseIf TypeOf tmpControls Is TAT_UTILITY_CTRL.ctrlMV_ Then
                Dim ctrl As TAT_UTILITY_CTRL.ctrlMV_ = tmpControls
                With ctrl
                    If .Ref_Object <> "" Then
                        Dim ctrlMotor As TAT_MQTT_CTRL.ctrlTAT_ = CType(frm.Controls(.Ref_Object), TAT_MQTT_CTRL.ctrlTAT_)
                        .Address_MV = ctrlMotor.Ad_Analog_MV
                        .MV_Value = ctrlMotor.objStatus.VAL_MV
                        .M_Code = ctrlMotor.M_Code
                        If bChanging = False Then
                            If nDelay = 0 Then
                                .MV_PointerValue = ctrlMotor.objStatus.VAL_MV
                            Else
                                nDelay = nDelay - 1
                            End If
                        End If
                    Else
                        If .Address_MV <> "" AndAlso .Address_MV <> "R0" Then .MV_Value = DataREG(getNumAdd(.Address_MV))
                        If bChanging = False Then
                            If nDelay = 0 Then
                                If .Address_MV <> "" AndAlso .Address_MV <> "R0" Then .MV_PointerValue = DataREG(getNumAdd(.Address_MV))
                            Else
                                nDelay = nDelay - 1
                            End If
                        End If
                    End If
                End With
            ElseIf TypeOf tmpControls Is GroupBox Or TypeOf tmpControls Is Panel Then
                Call Show_PidInGroupbox(tmpControls, DataREG, M, bChanging, nDelay, frm)
            End If
        Next
    End Sub

    Public Sub Show_PidInGroupbox(ByVal GbCtrl As Control, ByRef DataREG() As Int16, ByVal M() As Boolean, ByVal bChanging As Boolean, ByRef nDelay As Integer, ByVal frm As Form)
        For Each tmpControls As Control In GbCtrl.Controls
            If TypeOf tmpControls Is TAT_UTILITY_CTRL.ctrlPID_ Then
                Dim ctrl As TAT_UTILITY_CTRL.ctrlPID_ = tmpControls
                With ctrl
                    If .Ref_Object <> "" Then
                        Dim ctrlMotor As TAT_MQTT_CTRL.ctrlTAT_ = CType(frm.Controls(.Ref_Object), TAT_MQTT_CTRL.ctrlTAT_)
                        .Address_AutoPID = ctrlMotor.Ad_Auto_PID
                        .Address_Hi = ctrlMotor.Ad_Analog_HI
                        .Address_Lo = ctrlMotor.Ad_Analog_LO
                        .Address_PV = ctrlMotor.Ad_Analog_PV
                        .Address_SV = ctrlMotor.Ad_Analog_SV
                        .Address_MV = ctrlMotor.Ad_Analog_MV

                        .PV_Value = ctrlMotor.objStatus.VAL_PV
                        .SV_Value = ctrlMotor.objStatus.VAL_SV
                        .MV_Value = ctrlMotor.objStatus.VAL_MV
                        .Status_AutoPID = ctrlMotor.status_pid
                        .M_Code = ctrlMotor.M_Code
                        If bChanging = False Then
                            If nDelay = 0 Then
                                .PV_PointerHi = ctrlMotor.objStatus.VAL_HI
                                .PV_PointerLo = ctrlMotor.objStatus.VAL_LO
                                .SV_PointerValue = ctrlMotor.objStatus.VAL_SV
                                .MV_PointerValue = ctrlMotor.objStatus.VAL_MV
                            Else
                                nDelay = nDelay - 1
                            End If

                        End If
                    Else
                        If .Address_PV <> "" AndAlso .Address_PV <> "R0" Then .PV_Value = DataREG(getNumAdd(.Address_PV))
                        If .Address_SV <> "" AndAlso .Address_SV <> "R0" Then .SV_Value = DataREG(getNumAdd(.Address_SV))
                        If .Address_MV <> "" AndAlso .Address_MV <> "R0" Then .MV_Value = DataREG(getNumAdd(.Address_MV))
                        If .Address_AutoPID <> "" AndAlso .Address_AutoPID <> "R0" Then .Status_AutoPID = M(getNumAdd(.Address_AutoPID))

                        If bChanging = False Then
                            If nDelay = 0 Then
                                If .Address_Hi <> "" AndAlso .Address_Hi <> "R0" Then .PV_PointerHi = DataREG(getNumAdd(.Address_Hi))
                                If .Address_Lo <> "" AndAlso .Address_Lo <> "R0" Then .PV_PointerLo = DataREG(getNumAdd(.Address_Lo))
                                If .Address_SV <> "" AndAlso .Address_SV <> "R0" Then .SV_PointerValue = DataREG(getNumAdd(.Address_SV))
                                If .Address_MV <> "" AndAlso .Address_MV <> "R0" Then .MV_PointerValue = DataREG(getNumAdd(.Address_MV))
                            Else
                                nDelay = nDelay - 1
                            End If

                        End If
                    End If
                End With
            ElseIf TypeOf tmpControls Is TAT_UTILITY_CTRL.ctrlMV_ Then
                Dim ctrl As TAT_UTILITY_CTRL.ctrlMV_ = tmpControls
                With ctrl
                    If .Ref_Object <> "" Then
                        Dim ctrlMotor As TAT_MQTT_CTRL.ctrlTAT_ = CType(frm.Controls(.Ref_Object), TAT_MQTT_CTRL.ctrlTAT_)
                        .Address_MV = ctrlMotor.Ad_Analog_MV
                        .MV_Value = ctrlMotor.objStatus.VAL_MV
                        .M_Code = ctrlMotor.M_Code
                        If bChanging = False Then
                            If nDelay = 0 Then
                                .MV_PointerValue = ctrlMotor.objStatus.VAL_MV
                            Else
                                nDelay = nDelay - 1
                            End If
                        End If
                    Else
                        If .Address_MV <> "" AndAlso .Address_MV <> "R0" Then .MV_Value = DataREG(getNumAdd(.Address_MV))
                        If bChanging = False Then
                            If nDelay = 0 Then
                                If .Address_MV <> "" AndAlso .Address_MV <> "R0" Then .MV_PointerValue = DataREG(getNumAdd(.Address_MV))
                            Else
                                nDelay = nDelay - 1
                            End If
                        End If
                    End If
                End With
            End If
        Next
    End Sub

    Public Sub Show_AnalogInForm(ByVal frm As Form, ByRef DataREG() As Int16)
        Dim ctrl As TAT_UTILITY_CTRL.lblAnalog_

        For Each tmpControls As Control In frm.Controls
            If TypeOf tmpControls Is TAT_UTILITY_CTRL.lblAnalog_ Then
                ctrl = tmpControls
                If ctrl.Word_Lenght = 2 Then
                    Dim nValue(1) As Int32
                    System.Buffer.BlockCopy(DataREG, getNumAdd(ctrl.Address) * 2, nValue, 0, ctrl.Word_Lenght * 2)
                    ctrl.Analog_Value = nValue(0)
                ElseIf ctrl.Word_Lenght > 2 Then
                    Dim nValue(1) As Single
                    System.Buffer.BlockCopy(DataREG, getNumAdd(ctrl.Address) * 2, nValue, 0, 4)
                    ctrl.Analog_Value = nValue(0)
                Else
                    ctrl.Analog_Value = DataREG(getNumAdd(ctrl.Address))
                End If
            ElseIf TypeOf tmpControls Is GroupBox Or TypeOf tmpControls Is Panel Then
                Call ShowAnalog_InGroupBox(tmpControls, DataREG)
            End If
        Next

    End Sub
    Public Sub ShowAnalog_InGroupBox(GBctrl As Control, ByRef DataREG() As Int16)
        Dim ctrl As TAT_UTILITY_CTRL.lblAnalog_

        For Each Dctrl As Control In GBctrl.Controls
            If TypeOf Dctrl Is TAT_UTILITY_CTRL.lblAnalog_ Then
                ctrl = Dctrl
                If ctrl.Word_Lenght = 2 Then
                    Dim nValue(1) As Int32
                    System.Buffer.BlockCopy(DataREG, getNumAdd(ctrl.Address) * 2, nValue, 0, ctrl.Word_Lenght * 2)
                    ctrl.Analog_Value = nValue(0)
                ElseIf ctrl.Word_Lenght > 2 Then
                    Dim nValue(1) As Single
                    System.Buffer.BlockCopy(DataREG, getNumAdd(ctrl.Address) * 2, nValue, 0, ctrl.Word_Lenght * 2)
                    ctrl.Analog_Value = nValue(0)
                Else
                    ctrl.Analog_Value = DataREG(getNumAdd(ctrl.Address))
                End If
            End If
        Next

    End Sub

    Function Show_AnalogValue(ByRef ctrl As TAT_UTILITY_CTRL.lblAnalog_, ByRef DataREG() As Int16) As Long
        If ctrl.Word_Lenght = 2 Then
            Dim nValue(1) As Int32
            System.Buffer.BlockCopy(DataREG, getNumAdd(ctrl.Address), nValue, 0, 2)
            ctrl.Analog_Value = nValue(0)
        ElseIf ctrl.Word_Lenght > 2 Then
            Dim nValue(1) As Single
            System.Buffer.BlockCopy(DataREG, getNumAdd(ctrl.Address), nValue, 0, 2)
            ctrl.Analog_Value = nValue(0)
        Else
            ctrl.Analog_Value = DataREG(getNumAdd(ctrl.Address))
        End If
    End Function

    Function getNumAdd(asAddress As String) As Long
        On Error GoTo ErrHandle
        Dim RetAdd As Long

        Select Case UCase(Microsoft.VisualBasic.Left(asAddress, 1))
            Case "R"
                RetAdd = CLng(Mid$(asAddress, 2))
            Case "Z"
                RetAdd = CLng(Mid$(asAddress, 3))
            Case "M"
                RetAdd = CLng(Mid$(asAddress, 2))
        End Select
        getNumAdd = RetAdd

ExitHere:
        Exit Function

ErrHandle:
        MsgBox("Error Number : " & Err.Number & vbCrLf & "Error Description : " & Err.Description & vbCrLf & "Error In : getNumAdd", vbExclamation + vbOKOnly, "Func_getNumAdd")
        Resume ExitHere
    End Function

End Module
