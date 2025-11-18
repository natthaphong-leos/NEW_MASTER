Imports System.Reflection
Imports System.Text
Imports AxActUtlTypeLib
Module mdlMenu
    Dim menu_Frm As New ContextMenuStrip
    'Dim gbDeviceConfig As mqtt_object_config
    'Dim gbDeviceStatus As mqtt_object_status
    Dim Mx As New cls_MXComponent_RQ
    'Dim Mx As New cls_MXCompronent
    Dim MxCom1 As AxActUtlType
    ''Public tmpCtrl As TAT_MQTT_CTRL.ctrlTAT_
    Public tmpCtrl As Device_List
    Public ctrlMotor_tmp As TAT_MQTT_CTRL.ctrlTAT_
    Public ctrlDevice_tmp As MQTT_CTRL_OTHERDEVICE.ctrlDevice
    Public ctrlAnalog_tmp As TAT_UTILITY_CTRL.lblAnalog_
    Public ctrBin_tmp As TAT_CtrlBin.ctrlBin_
    Public ctrLq_tmp As TAT_CtrlBin.ctrlLq_
    Public ctrlRoute_tmp As TAT_CtrlRoute.ctrlRouteConveyer_
    Public ctrlAutoChangRoute_tmp As TAT_CtrlRoute.ctrlJogRoute_
    Public ctrlAutoChangeRoute_tmp As TAT_CtrlRoute.ctrlAutoChage_
    Public ctrlSelect_tmp As TAT_CtrlSelector.ctrlSelectorMode_
    Public CnBatching As New clsDB(Batching_Conf.Name, Batching_Conf.User, Batching_Conf.Password, Batching_Conf.IPAddress, Batching_Conf.Connection_Type)
    Public CnRoute As New clsDB(Auto_Route_Conf.Name, Auto_Route_Conf.User, Auto_Route_Conf.Password, Auto_Route_Conf.IPAddress, Auto_Route_Conf.Connection_Type)
    Private ManualStart_On_AutoMode As Boolean '================ ADD FOR MENU IN AUTO MODE

    Private objName As String '===== For Keep log

#Region "MENU ANALOG"
    Function Show_AnalogMenu(sender As Object, point As Point)
        objName = DirectCast(sender, Control).Name '===== For Keep log
        Dim menu_new As New frmMenu_Motor
        menu_Frm = menu_new.ctxMenuStrip

        '=================== For multi language
        change_language_in_toolStrip(menu_Frm.Items, CnBatching, MDI_FRM.tsCboLanguage.Text)

        Call StartUpMenu(menu_Frm)

        If TypeOf sender Is TAT_UTILITY_CTRL.lblAnalog_ Then
            ctrlAnalog_tmp = sender
        End If

        If ctrlAnalog_tmp.Analog_Code <> "" Then
            menu_Frm.Items("mnuHeader").Text = ctrlAnalog_tmp.Analog_Code
        Else
            menu_Frm.Items("mnuHeader").Text = ctrlAnalog_tmp.Name
        End If

        If ctrlAnalog_tmp.Ad_Parameter_Current <> "" And ctrlAnalog_tmp.Ad_Parameter_Current <> "D0" Then
            If menu_Frm.Items("tsSeparator6").Visible = False Then menu_Frm.Items("tsSeparator6").Visible = True
            menu_Frm.Items("mnuAnalogParameter").Visible = True
            AddHandler menu_Frm.Items("mnuAnalogParameter").Click, AddressOf mnuAnalogCurrent_Click
        End If

        If (ctrlAnalog_tmp.Address <> "" And ctrlAnalog_tmp.Address <> "R0") And ctrlAnalog_tmp.Enable_Set = True Then
            If menu_Frm.Items("tsSeparator6").Visible = False Then menu_Frm.Items("tsSeparator6").Visible = True
            menu_Frm.Items("mnuSetValue").Visible = True
            AddHandler menu_Frm.Items("mnuSetValue").Click, AddressOf mnuSetAnalogValue_Click
        End If

        menu_Frm.Show(sender, point)

    End Function

    Private Sub mnuAnalogCurrent_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = "-"
        Dim machineCode As String = ctrlAnalog_tmp.Analog_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim strCmd_Line As String
        Dim adR_TonHour As String
        If ctrlAnalog_tmp.CmdLine_String = "FEED" Then
            If ctrlAnalog_tmp.Ad_Ton_Per_Hour.Length > 0 Then adR_TonHour = ctrlAnalog_tmp.Ad_Ton_Per_Hour Else adR_TonHour = "R0"
        Else
            adR_TonHour = "R0"
        End If
        Dim tmpIndex() As String
        tmpIndex = Split(ctrlAnalog_tmp.Name, "_")
        strCmd_Line = ctrlAnalog_tmp.CmdLine_String & " " & ctrlAnalog_tmp.PLC_Station_No & " " & tmpIndex(1) & " " & ctrlAnalog_tmp.Ad_Parameter_Current & " " &
                      ctrlAnalog_tmp.CmdLine_String & "_" & ctrlAnalog_tmp.Analog_Code & " localhost " & adR_TonHour


        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""Current_Config"", " & strCmd_Line & ", " & ctrlAnalog_tmp.Name & ")"
        Try
            Call_EXE("Current_Config", strCmd_Line, ctrlAnalog_tmp.Name)
            '================================================== FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
        Catch ex As Exception
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Open Exe Error (NAME : Current_Config )" & vbCrLf & ex.StackTrace
            ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End Try
    End Sub

    Private Sub mnuSetAnalogValue_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = "-"
        Dim machineCode As String = ctrlAnalog_tmp.Analog_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim DataInput As Double
        Dim cInput As String
        cInput = InputBox(ctrlAnalog_tmp.Analog_Code, "Input Numeric Valus")
        If cInput <> "" Then
            If IsNumeric(cInput) = False Then
                Call MsgBox("PLEASE INPUT NUMERIC ONLY!", MsgBoxStyle.Critical, "ERROR")
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Invalid input data (DATA : " & cInput & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
                Exit Sub
            Else
                DataInput = CDbl(cInput)
            End If

            If ctrlAnalog_tmp.Maximum_Value <> 0 Then
                If DataInput > ctrlAnalog_tmp.Maximum_Value Then
                    Call MsgBox("MAXIMUM VALUE IS " & ctrlAnalog_tmp.Maximum_Value, MsgBoxStyle.Critical, "OVER VALUE")
                    '============================================= FOR KEEP LOG
                    Dim strMsg As String = "Input data is over from maximum (DATA : " & cInput & " MAXIMUM VALUE IS : " & ctrlAnalog_tmp.Maximum_Value & " )"
                    WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                    '============================================= FOR KEEP LOG
                    Exit Sub
                End If
            End If
            If ctrlAnalog_tmp.Maximum_Value <> 0 Then
                If DataInput < ctrlAnalog_tmp.Minimum_Value Then
                    Call MsgBox("MINIMUM VALUE IS " & ctrlAnalog_tmp.Minimum_Value, MsgBoxStyle.Critical, "UNDER VALUE")
                    '============================================= FOR KEEP LOG
                    Dim strMsg As String = "Input data is under from minimum (DATA : " & cInput & " MINIMUM VALUE IS : " & ctrlAnalog_tmp.Minimum_Value & " )"
                    WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                    '============================================= FOR KEEP LOG
                    Exit Sub
                End If
            End If

            If ConnectPLC(ctrlAnalog_tmp.PLC_Station_No) Then
                strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & ctrlAnalog_tmp.PLC_Station_No & "), " & ctrlAnalog_tmp.Address & ", " & ctrlAnalog_tmp.Word_Lenght & ", "
                If ctrlAnalog_tmp.Word_Lenght = 2 Then
                    Dim send_long(1) As Int32
                    Dim target_val(1) As Short
                    send_long(0) = DataInput * ctrlAnalog_tmp.Multiply
                    System.Buffer.BlockCopy(send_long, 0, target_val, 0, 4)
                    Mx.mxWriteBlock(Mx.MxCom(ctrlAnalog_tmp.PLC_Station_No), ctrlAnalog_tmp.Address, ctrlAnalog_tmp.Word_Lenght, target_val)
                    strCommand += target_val(0) & ")"
                ElseIf ctrlAnalog_tmp.Word_Lenght > 2 Then
                    Dim send_long(1) As Single
                    Dim target_val(1) As Short
                    send_long(0) = DataInput * ctrlAnalog_tmp.Multiply
                    System.Buffer.BlockCopy(send_long, 0, target_val, 0, 4)
                    Mx.mxWriteBlock(Mx.MxCom(ctrlAnalog_tmp.PLC_Station_No), ctrlAnalog_tmp.Address, ctrlAnalog_tmp.Word_Lenght, target_val)
                    strCommand += target_val(0) & ")"
                Else
                    Dim DATA(1) As Int16
                    DATA(0) = DataInput * ctrlAnalog_tmp.Multiply
                    Mx.mxWriteBlock(Mx.MxCom(ctrlAnalog_tmp.PLC_Station_No), ctrlAnalog_tmp.Address, 1, DATA)
                    strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & ctrlAnalog_tmp.PLC_Station_No & "), " & ctrlAnalog_tmp.Address & ", 1," & DATA(0) & ")"
                End If
                '============================================= FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '============================================= FOR KEEP LOG

                Mx.MxCom(ctrlAnalog_tmp.PLC_Station_No).Close()
            Else
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Can not connect PLC (STATION : " & ctrlAnalog_tmp.PLC_Station_No & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End If
        End If
    End Sub

    Public Sub mnuSetPID_Pointer(sender As Object, pointerName As String, ptValue As Integer)
        '=== FOR KEEP LOG
        objName = DirectCast(sender, Control).Name '===== For Keep log
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = "-"
        Dim machineCode As String = "-"
        Dim strCommand As String = "-"

        Dim strAddress As String = ""
        Dim stnPLC As Integer = 0
        Dim DATA(1) As Int16
        If TypeOf sender Is TAT_UTILITY_CTRL.ctrlPID_ Then
            Dim ctrl As TAT_UTILITY_CTRL.ctrlPID_ = sender
            machineCode = ctrl.M_Code '=== FOR KEEP LOG
            stnPLC = ctrl.PLC_Station_No
            Select Case pointerName
                Case "HI"
                    If ctrl.Address_Hi = "" Or ctrl.Address_Hi = "R0" Then Exit Sub
                    strAddress = ctrl.Address_Hi
                    DATA(0) = ptValue * ctrl.Multiply_PV
                    strMenu += " (SET HI)"'=== FOR KEEP LOG
                Case "LO"
                    If ctrl.Address_Lo = "" Or ctrl.Address_Lo = "R0" Then Exit Sub
                    strAddress = ctrl.Address_Lo
                    DATA(0) = ptValue * ctrl.Multiply_PV
                    strMenu += " (SET LO)"'=== FOR KEEP LOG
                Case "SV"
                    If ctrl.Address_SV = "" Or ctrl.Address_SV = "R0" Then Exit Sub
                    strAddress = ctrl.Address_SV
                    DATA(0) = ptValue * ctrl.Multiply_SV
                    strMenu += " (SET SV)"'=== FOR KEEP LOG
                Case "MV"
                    If ctrl.Address_MV = "" Or ctrl.Address_MV = "R0" Then Exit Sub
                    strAddress = ctrl.Address_MV
                    DATA(0) = ptValue * ctrl.Multiply_MV
                    strMenu += " (SET MV)" '=== FOR KEEP LOG
            End Select
        ElseIf TypeOf sender Is TAT_UTILITY_CTRL.ctrlMV_ Then
            Dim ctrl As TAT_UTILITY_CTRL.ctrlMV_ = sender
            machineCode = ctrl.M_Code '=== FOR KEEP LOG
            stnPLC = ctrl.PLC_Station_No
            Select Case pointerName
                Case "MV"
                    If ctrl.Address_MV = "" Or ctrl.Address_MV = "R0" Then Exit Sub
                    strAddress = ctrl.Address_MV
                    DATA(0) = ptValue * ctrl.Multiply_MV
                    strMenu += " (SET MV)" '=== FOR KEEP LOG
            End Select
        End If

        Dim ret As Integer
Retry_PLC:

        If ConnectPLC(stnPLC) Then
            ret = Mx.mxWriteBlock(Mx.MxCom(stnPLC), strAddress, 1, DATA)
            strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & stnPLC & "), " & strAddress & ", 1," & DATA(0).ToString & ")" '=== FOR KEEP LOG
            '============================================= FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '============================================= FOR KEEP LOG
            If ret = 0 Then
                Mx.MxCom(stnPLC).Close()
                'Else
                '    GoTo Retry_PLC
            End If
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & stnPLC & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If

    End Sub

#End Region

#Region "MENU MOTOR SLIDE FLAP DEVICE"
    Function Show_Menu(sender As Object, point As Point, status_remote As Boolean)
        objName = DirectCast(sender, Control).Name '===== For Keep log
        Dim menu_new As New frmMenu_Motor
        menu_Frm = menu_new.ctxMenuStrip

        '=================== For multi language
        'Save_TextGlobal_default(menu_new.Controls, CnBatching)
        change_language_in_toolStrip(menu_Frm.Items, CnBatching, MDI_FRM.tsCboLanguage.Text)
        'change_language_in_collection(menu_Frm.Controls, CnBatching, MDI_FRM.tsCboLanguage.Text, Application.ProductName, menu_new.Name)

        Call StartUpMenu(menu_Frm)

        If TypeOf sender Is TAT_MQTT_CTRL.ctrlTAT_ Then
            ctrlMotor_tmp = sender
            With ctrlMotor_tmp
                If .M_Code Is Nothing Then Exit Function
                If .M_Code.Length = 0 And .M_Code.Trim = "" Then Exit Function
                tmpCtrl.M_Code = .M_Code
                tmpCtrl.Index = .Index
                tmpCtrl.Ad_Auto = .Ad_Auto
                tmpCtrl.Ad_Auto_PID = .Ad_Auto_PID
                tmpCtrl.Ad_Bin_Low = .Ad_Bin_Low
                tmpCtrl.Ad_Coverlock = .Ad_Coverlock
                tmpCtrl.Ad_Err1 = .Ad_Err1
                tmpCtrl.Ad_Err2 = .Ad_Err2
                tmpCtrl.Ad_Output = .Ad_Output
                tmpCtrl.Ad_Parameter_Current = .Ad_Parameter_Current
                tmpCtrl.Ad_Parameter_PID = .Ad_Parameter_PID
                tmpCtrl.Ad_Run = .Ad_Run
                tmpCtrl.Ad_Run2 = .Ad_Run2
                tmpCtrl.Ad_Service = .Ad_Service
                tmpCtrl.Ad_Ton_Per_Hour = .Ad_Ton_Per_Hour
                tmpCtrl.Bin_Number = .Bin_Number
                tmpCtrl.Bin_Step = .Bin_Step
                tmpCtrl.cmdLine_String = .cmdLine_String
                tmpCtrl.ControlMode = .ControlMode.ToString
                tmpCtrl.ControlType = .ControlType.ToString
                tmpCtrl.STR_Name = .STR_Name
                tmpCtrl.STR_No = .STR_No
                tmpCtrl.Parameter_Type = .Parameter_Type
                tmpCtrl.OP_Scale_No = .OP_Scale_No
                tmpCtrl.OP_Bin_index = .OP_Bin_index
                tmpCtrl.OP_LQ_Name = .OP_LQ_Name
                tmpCtrl.OP_LQ_TN = .OP_LQ_TN
                tmpCtrl.OP_LQ_TS = .OP_LQ_TS
                tmpCtrl.PLC_Station_No = .PLC_Station_No
                tmpCtrl.Ad_Analog_Hi = .Ad_Analog_HI
                tmpCtrl.Ad_Analog_Lo = .Ad_Analog_LO
                tmpCtrl.Ad_Analog_MV = .Ad_Analog_MV
                tmpCtrl.Ad_Analog_PV = .Ad_Analog_PV
                tmpCtrl.Ad_Analog_RPM = .Ad_Analog_RPM
                tmpCtrl.Ad_Analog_SV = .Ad_Analog_SV

                '========================== STATUS 
                tmpCtrl.status_auto = .status_auto
                tmpCtrl.status_coverlock = .status_coverlock
                tmpCtrl.status_err1 = .status_err1
                tmpCtrl.status_err2 = .status_err2
                tmpCtrl.status_interlock = .status_interlock
                tmpCtrl.status_output = .status_output
                tmpCtrl.status_pid = .status_pid
                tmpCtrl.status_run = .status_run
                tmpCtrl.status_service = .status_service

                ManualStart_On_AutoMode = .start_In_AutoMode '================ ADD FOR MENU IN AUTO MODE

            End With
        ElseIf TypeOf sender Is MQTT_CTRL_OTHERDEVICE.ctrlDevice Then
            ctrlDevice_tmp = sender
            With ctrlDevice_tmp
                If .M_Code Is Nothing Then Exit Function
                If .M_Code.Length = 0 And .M_Code.Trim = "" Then Exit Function

                tmpCtrl.M_Code = .M_Code
                tmpCtrl.Index = .Index
                tmpCtrl.Ad_Auto = .Ad_Auto
                tmpCtrl.Ad_Auto_PID = .Ad_Auto_PID
                tmpCtrl.Ad_Bin_Low = .Ad_Bin_Low
                tmpCtrl.Ad_Coverlock = .Ad_Coverlock
                tmpCtrl.Ad_Err1 = .Ad_Err1
                tmpCtrl.Ad_Err2 = .Ad_Err2
                tmpCtrl.Ad_Output = .Ad_Output
                tmpCtrl.Ad_Parameter_Current = .Ad_Parameter_Current
                tmpCtrl.Ad_Parameter_PID = .Ad_Parameter_PID
                tmpCtrl.Ad_Run = .Ad_Run
                tmpCtrl.Ad_Run2 = .Ad_Run2
                tmpCtrl.Ad_Service = .Ad_Service
                tmpCtrl.Ad_Ton_Per_Hour = .Ad_Ton_Per_Hour
                tmpCtrl.Bin_Number = .Bin_Number
                tmpCtrl.Bin_Step = .Bin_Step
                tmpCtrl.cmdLine_String = .cmdLine_String
                tmpCtrl.ControlMode = "CONTROL"
                tmpCtrl.ControlType = "DEVICE"
                tmpCtrl.STR_Name = ""
                tmpCtrl.STR_No = ""
                tmpCtrl.Parameter_Type = ""
                tmpCtrl.OP_Scale_No = ""
                tmpCtrl.OP_Bin_index = ""
                tmpCtrl.OP_LQ_Name = ""
                tmpCtrl.OP_LQ_TN = ""
                tmpCtrl.OP_LQ_TS = ""
                tmpCtrl.PLC_Station_No = .PLC_Station_No
                tmpCtrl.menuStart = .Menu_Start

                '========================== STATUS 
                tmpCtrl.status_auto = .status_auto
                tmpCtrl.status_coverlock = .status_coverlock
                tmpCtrl.status_err1 = .status_err1
                tmpCtrl.status_err2 = .status_err2
                tmpCtrl.status_interlock = .status_interlock
                tmpCtrl.status_output = .status_output
                tmpCtrl.status_pid = .status_pid
                tmpCtrl.status_run = .Status_Run
                tmpCtrl.status_service = .status_service

            End With

        ElseIf TypeOf sender Is TAT_CtrlBin.ctrlBin_ Then
            ctrBin_tmp = sender
            With ctrBin_tmp
                If .mqtt_bin_parameter_config_.BIN_CODE Is Nothing Then Exit Function
                If .mqtt_bin_parameter_config_.BIN_CODE.Length = 0 And .mqtt_bin_parameter_config_.BIN_CODE.Trim = "" Then Exit Function
                tmpCtrl.M_Code = .mqtt_bin_parameter_config_.BIN_CODE
                tmpCtrl.Ad_Auto = "M0"
                tmpCtrl.Ad_Auto_PID = "M0"
                tmpCtrl.Ad_Coverlock = "M0"
                tmpCtrl.Ad_Err1 = "M0"
                tmpCtrl.Ad_Err2 = "M0"
                tmpCtrl.Ad_Output = "M0"
                tmpCtrl.Ad_Parameter_Current = "D0"
                tmpCtrl.Ad_Parameter_PID = "D0"
                tmpCtrl.Ad_Run = "M0"
                tmpCtrl.Ad_Run2 = "M0"
                tmpCtrl.Ad_Service = "M0"
                tmpCtrl.Ad_Ton_Per_Hour = ""
                tmpCtrl.Ad_Bin_High = .mqtt_bin_parameter_config_.M_HI
                tmpCtrl.Ad_Bin_Low = .mqtt_bin_parameter_config_.M_LO
                tmpCtrl.Bin_Number = .mqtt_bin_parameter_config_.BIN_NO
                tmpCtrl.Bin_Step = "100"
                tmpCtrl.ControlMode = "CONTROL"
                tmpCtrl.ControlType = "BIN"
                tmpCtrl.STR_Name = ""
                tmpCtrl.STR_No = ""
                tmpCtrl.Parameter_Type = ""
                tmpCtrl.OP_Scale_No = ""
                tmpCtrl.OP_Bin_index = ""
                tmpCtrl.OP_LQ_Name = ""
                tmpCtrl.OP_LQ_TN = ""
                tmpCtrl.OP_LQ_TS = ""
                tmpCtrl.PLC_Station_No = .mqtt_bin_parameter_config_.STATION_NO
                If .Bin_Ref_Status_ <> "" Then
                    tmpCtrl.Device_Index_Hi = .mqtt_bin_ref_config_.DEVICE_HI
                    tmpCtrl.Device_Index_Lo = .mqtt_bin_ref_config_.DEVICE_LO
                Else
                    tmpCtrl.Device_Index_Hi = .mqtt_bin_parameter_config_.DEVICE_HI
                    tmpCtrl.Device_Index_Lo = .mqtt_bin_parameter_config_.DEVICE_LO
                End If
            End With
        End If

        With tmpCtrl
            If .M_Code Is Nothing Then Exit Function
            If .M_Code.Length = 0 And .M_Code.Trim = "" Then Exit Function
            menu_Frm.Items("mnuHeader").Text = .M_Code
            'menu_Frm.Items("mnuHeader").Text = sender.ToString

            'Mx.MxCom1 = Frm_ComponentMX.AxActUtlType1
            'Mx.mxSetLogicalNumber(Mx.MxCom1, .Station_no)
            'Mx.MxCom1.Close()
            'Mx.PLC_Con(0) = Mx.mxOpenPort(Mx.MxCom1)
            'If Mx.PLC_Con(0) = False Then Exit Function

            'Call InitializeMenu(menu_Frm, strType)
            Call InitializeMenu_MainCtrl(menu_Frm, tmpCtrl, status_remote)

        End With

        menu_Frm.Show(sender, point)

    End Function

    Function ConnectPLC(stnNo As Integer) As Boolean
        'If Mx.MxCom(stnNo).ActLogicalStationNumber <> stnNo Then
        '    Mx.mxSetLogicalNumber(Mx.MxCom(stnNo), stnNo)

        'End If
        'If Mx.PLC_Con(stnNo) = False Then
        '    Mx.PLC_Con(stnNo) = Mx.mxOpenPort(Mx.MxCom(stnNo))
        'End If


        Mx.mxSetLogicalNumber(Mx.MxCom(stnNo), stnNo)
        Mx.PLC_Con(stnNo) = Mx.mxOpenPort(Mx.MxCom(stnNo))

        Return Mx.PLC_Con(stnNo)
    End Function

    Function StartUpMenu(ByRef tmpMenu As ContextMenuStrip)


        For Each i As ToolStripItem In tmpMenu.Items
            If i.Name <> "mnuHeader" Then
                i.Visible = False
            End If
        Next

    End Function

    Function InitializeMenu_MainCtrl(ByRef tmpMenu As ContextMenuStrip, ByRef ctrlMain As Device_List, ByVal stRemote As Boolean)
        With ctrlMain

            If UCase(tmpCtrl.ControlMode.ToString) = "SHOWONLY" Then
                Exit Function
            End If

            '====================================================== SET DELAY TIME
            If tmpCtrl.M_Code <> "" Then
                If tmpCtrl.ControlType = "BIN" Then
                    If tmpCtrl.Device_Index_Hi <> "" And tmpCtrl.Device_Index_Lo <> "" Then
                        If tmpMenu.Items("tsSeparator7").Visible = False Then tmpMenu.Items("tsSeparator7").Visible = True
                        tmpMenu.Items("mnuSetDelayTimeHigh").Visible = True
                        AddHandler tmpMenu.Items("mnuSetDelayTimeHigh").Click, AddressOf mnuSetDelayTimeHigh_Click
                        tmpMenu.Items("mnuSetDelayTimeLow").Visible = True
                        AddHandler tmpMenu.Items("mnuSetDelayTimeLow").Click, AddressOf mnuSetDelayTimeLow_Click
                    ElseIf tmpCtrl.Device_Index_Hi <> "" Then
                        If tmpMenu.Items("tsSeparator7").Visible = False Then tmpMenu.Items("tsSeparator7").Visible = True
                        tmpMenu.Items("mnuSetDelayTimeHigh").Visible = True
                        AddHandler tmpMenu.Items("mnuSetDelayTimeHigh").Click, AddressOf mnuSetDelayTimeHigh_Click
                    ElseIf tmpCtrl.Device_Index_Lo <> "" Then
                        If tmpMenu.Items("tsSeparator7").Visible = False Then tmpMenu.Items("tsSeparator7").Visible = True
                        tmpMenu.Items("mnuSetDelayTimeLow").Visible = True
                        AddHandler tmpMenu.Items("mnuSetDelayTimeLow").Click, AddressOf mnuSetDelayTimeLow_Click
                    End If
                Else
                    If tmpMenu.Items("tsSeparator7").Visible = False Then tmpMenu.Items("tsSeparator7").Visible = True
                    tmpMenu.Items("mnuSetDelayTime").Visible = True
                    AddHandler tmpMenu.Items("mnuSetDelayTime").Click, AddressOf mnuSetDelayTime_Click
                End If
            End If

            If stRemote = False Then
                Exit Function
            End If

            '=================================================== MENU AUTO - MANUAL
            If .Ad_Auto <> "M0" And .Ad_Auto.Length <> 0 Then
                tmpMenu.Items("tsSeparator1").Visible = True
                tmpMenu.Items("mnuMode_Auto").Visible = True
                tmpMenu.Items("mnuMode_Manual").Visible = True
                AddHandler tmpMenu.Items("mnuMode_Auto").Click, AddressOf mnuAuto_Click
                AddHandler tmpMenu.Items("mnuMode_Manual").Click, AddressOf mnuManual_Click

                If .status_auto = True Then
                    tmpMenu.Items("mnuMode_Auto").Enabled = False
                    tmpMenu.Items("mnuMode_Auto").Text = tmpMenu.Items("mnuMode_Auto").Text & " <<<<"
                    tmpMenu.Items("mnuMode_Manual").Enabled = True
                    tmpMenu.Items("mnuMode_Manual").Text = Replace(tmpMenu.Items("mnuMode_Manual").Text, " <<<<", "")
                    If ManualStart_On_AutoMode = False Then
                        tmpMenu.Items("mnuStart").Enabled = False
                        tmpMenu.Items("mnuStop").Enabled = False
                    Else
                        tmpMenu.Items("mnuStart").Enabled = True
                        tmpMenu.Items("mnuStop").Enabled = True
                    End If

                Else
                    tmpMenu.Items("mnuMode_Auto").Enabled = True
                    tmpMenu.Items("mnuMode_Auto").Text = Replace(tmpMenu.Items("mnuMode_Auto").Text, " <<<<", "")

                    tmpMenu.Items("mnuMode_Manual").Enabled = False
                    tmpMenu.Items("mnuMode_Manual").Text = tmpMenu.Items("mnuMode_Manual").Text & " <<<<"
                    tmpMenu.Items("mnuStart").Enabled = True
                    tmpMenu.Items("mnuStop").Enabled = True
                End If

            End If

            '=================================================== MENU AUTO - MANUAL PID
            If .Ad_Auto_PID <> "M0" And .Ad_Auto_PID.Length > 0 Then
                If tmpMenu.Items("tsSeparator4").Visible = False Then tmpMenu.Items("tsSeparator4").Visible = True
                tmpMenu.Items("mnuPidAuto").Visible = True
                tmpMenu.Items("mnuPidManual").Visible = True
                AddHandler tmpMenu.Items("mnuPidAuto").Click, AddressOf mnuAuto_PID_Click
                AddHandler tmpMenu.Items("mnuPidManual").Click, AddressOf mnuManual_PID_Click

                If .status_pid = True Then
                    tmpMenu.Items("mnuPidAuto").Enabled = False
                    tmpMenu.Items("mnuPidAuto").Text = tmpMenu.Items("mnuPidAuto").Text & " <<<<"

                    tmpMenu.Items("mnuPidManual").Enabled = True
                    tmpMenu.Items("mnuPidManual").Text = Replace(tmpMenu.Items("mnuPidManual").Text, " <<<<", "")
                Else
                    tmpMenu.Items("mnuPidAuto").Enabled = True
                    tmpMenu.Items("mnuPidAuto").Text = Replace(tmpMenu.Items("mnuPidAuto").Text, " <<<<", "")

                    tmpMenu.Items("mnuPidManual").Enabled = False
                    tmpMenu.Items("mnuPidManual").Text = tmpMenu.Items("mnuPidManual").Text & " <<<<"
                End If

            End If


            '=================================================== MENU Manual Start - Stop
            If .ControlType.ToString = "DEVICE" And .menuStart = True Then
                If .Ad_Output.Length > 0 And .Ad_Output <> "M0" Then
                    '=================== For multi language
                    If MDI_FRM.tsCboLanguage.Text <> "EN" Then
                        tmpMenu.Items("mnuStart").Text = Query_Text_By_LanguageCode("MANUAL START", MDI_FRM.tsCboLanguage.Text, CnBatching) & " [" & .Ad_Output & "] "
                        tmpMenu.Items("mnuStop").Text = Query_Text_By_LanguageCode("MANUAL STOP", MDI_FRM.tsCboLanguage.Text, CnBatching)
                    Else
                        tmpMenu.Items("mnuStart").Text = "MANUAL START" & " [" & .Ad_Output & "] "
                        tmpMenu.Items("mnuStop").Text = "MANUAL STOP"
                    End If
                    'tmpMenu.Items("mnuStart").Text = "MANUAL START" & " [" & .Ad_Output & "] "
                    'tmpMenu.Items("mnuStop").Text = "MANUAL STOP"
                    tmpMenu.Items("mnuStart").Visible = True
                    tmpMenu.Items("mnuStop").Visible = True

                    If tmpMenu.Items("tsSeparator2").Visible = False Then tmpMenu.Items("tsSeparator2").Visible = True
                    AddHandler tmpMenu.Items("mnuStart").Click, AddressOf mnuManualStart_Click
                    AddHandler tmpMenu.Items("mnuStop").Click, AddressOf mnuManualStop_Click
                ElseIf .Ad_Run.Length > 0 And .Ad_Run <> "M0" Then
                    '=================== For multi language
                    If MDI_FRM.tsCboLanguage.Text <> "EN" Then
                        tmpMenu.Items("mnuStart").Text = Query_Text_By_LanguageCode("MANUAL START", MDI_FRM.tsCboLanguage.Text, CnBatching) & " [" & .Ad_Output & "] "
                        tmpMenu.Items("mnuStop").Text = Query_Text_By_LanguageCode("MANUAL STOP", MDI_FRM.tsCboLanguage.Text, CnBatching)
                    Else
                        tmpMenu.Items("mnuStart").Text = "MANUAL START" & " [" & .Ad_Run & "] "
                        tmpMenu.Items("mnuStop").Text = "MANUAL STOP"
                    End If
                    'tmpMenu.Items("mnuStart").Text = "MANUAL START" & " [" & .Ad_Run & "] "
                    'tmpMenu.Items("mnuStop").Text = "MANUAL STOP"
                    tmpMenu.Items("mnuStart").Visible = True
                    tmpMenu.Items("mnuStop").Visible = True

                    If tmpMenu.Items("tsSeparator2").Visible = False Then tmpMenu.Items("tsSeparator2").Visible = True
                    AddHandler tmpMenu.Items("mnuStart").Click, AddressOf mnuManualStart_Click
                    AddHandler tmpMenu.Items("mnuStop").Click, AddressOf mnuManualStop_Click
                End If
            End If
            If .Ad_Output <> "M0" And .Ad_Output.Length <> 0 Then
                If tmpMenu.Items("tsSeparator2").Visible = False Then tmpMenu.Items("tsSeparator2").Visible = True

                AddHandler tmpMenu.Items("mnuStart").Click, AddressOf mnuManualStart_Click
                AddHandler tmpMenu.Items("mnuStop").Click, AddressOf mnuManualStop_Click

                Select Case .ControlType.ToString
                    Case "MOTOR"
                        If MDI_FRM.tsCboLanguage.Text <> "EN" Then '=================== For multi language
                            tmpMenu.Items("mnuStart").Text = Query_Text_By_LanguageCode("MANUAL START", MDI_FRM.tsCboLanguage.Text, CnBatching) & " [" & .Ad_Output & "] "
                            tmpMenu.Items("mnuStop").Text = Query_Text_By_LanguageCode("MANUAL STOP", MDI_FRM.tsCboLanguage.Text, CnBatching)
                        Else
                            tmpMenu.Items("mnuStart").Text = "MANUAL START" & " [" & .Ad_Output & "] "
                            tmpMenu.Items("mnuStop").Text = "MANUAL STOP"
                        End If
                        tmpMenu.Items("mnuStart").Visible = True
                        tmpMenu.Items("mnuStop").Visible = True
                    Case "SLIDE"
                        If MDI_FRM.tsCboLanguage.Text <> "EN" Then '=================== For multi language
                            tmpMenu.Items("mnuStart").Text = Query_Text_By_LanguageCode("MANUAL OPEN", MDI_FRM.tsCboLanguage.Text, CnBatching) & " [" & .Ad_Output & "] "
                            tmpMenu.Items("mnuStop").Text = Query_Text_By_LanguageCode("MANUAL CLOSE", MDI_FRM.tsCboLanguage.Text, CnBatching)
                        Else
                            tmpMenu.Items("mnuStart").Text = "MANUAL OPEN" & " [" & .Ad_Output & "] "
                            tmpMenu.Items("mnuStop").Text = "MANUAL CLOSE"
                        End If

                        tmpMenu.Items("mnuStart").Visible = True
                        tmpMenu.Items("mnuStop").Visible = True
                    Case "FLAP"
                        If MDI_FRM.tsCboLanguage.Text <> "EN" Then '=================== For multi language
                            tmpMenu.Items("mnuStart").Text = Query_Text_By_LanguageCode("MANUAL SELECT", MDI_FRM.tsCboLanguage.Text, CnBatching) & " [" & .Ad_Output & "] "
                        Else
                            tmpMenu.Items("mnuStart").Text = "MANUAL SELECT" & " [" & .Ad_Output & "] "
                        End If
                        tmpMenu.Items("mnuStart").Visible = True
                End Select
            End If

            '=================================================== MENU RESET ERROR
            If (.Ad_Coverlock <> "M0" And .Ad_Coverlock IsNot Nothing) Or (.Ad_Err1 <> "M0" And .Ad_Err1 IsNot Nothing) Or (.Ad_Err2 <> "M0" And .Ad_Err2 IsNot Nothing) Then
                If tmpMenu.Items("tsSeparator2").Visible = False Then tmpMenu.Items("tsSeparator2").Visible = True
                tmpMenu.Items("tsSeparator2").Visible = True
                tmpMenu.Items("mnuResetError").Visible = True

                AddHandler tmpMenu.Items("mnuResetError").Click, AddressOf mnuResetError_Click

            End If

            '=================================================== MENU UNDER REPAIR
            If .Ad_Service <> "M0" And .Ad_Service.Length <> 0 Then
                If tmpMenu.Items("tsSeparator2").Visible = False Then tmpMenu.Items("tsSeparator2").Visible = True
                tmpMenu.Items("mnuUnderRepair").Visible = True

                If .status_service = True Then
                    If MDI_FRM.tsCboLanguage.Text <> "EN" Then '=================== For multi language
                        tmpMenu.Items("mnuUnderRepair").Text = Query_Text_By_LanguageCode("SERVICE ON", MDI_FRM.tsCboLanguage.Text, CnBatching)
                    Else
                        tmpMenu.Items("mnuUnderRepair").Text = "SERVICE ON"
                    End If
                Else
                    If MDI_FRM.tsCboLanguage.Text <> "EN" Then '=================== For multi language
                        tmpMenu.Items("mnuUnderRepair").Text = Query_Text_By_LanguageCode("UNDER REPAIR", MDI_FRM.tsCboLanguage.Text, CnBatching)
                    Else
                        tmpMenu.Items("mnuUnderRepair").Text = "UNDER REPAIR"
                    End If
                End If

                AddHandler tmpMenu.Items("mnuUnderRepair").Click, AddressOf mnuUnderRepair_Click

            End If



            '====================================================== PID CONFIG
            If (.Ad_Parameter_PID IsNot Nothing And .Ad_Parameter_PID <> "D0") Then
                If tmpMenu.Items("tsSeparator4").Visible = False Then tmpMenu.Items("tsSeparator4").Visible = True
                tmpMenu.Items("mnuPidConfig").Visible = True
                AddHandler tmpMenu.Items("mnuPidConfig").Click, AddressOf mnuPidConfig_Click
            End If

            '====================================================== PARAMETER
            If (.Ad_Parameter_Current <> "D0" And .Ad_Parameter_Current IsNot Nothing) Then
                If tmpMenu.Items("tsSeparator6").Visible = False Then tmpMenu.Items("tsSeparator6").Visible = True
                tmpMenu.Items("mnuAnalogParameter").Visible = True

                AddHandler tmpMenu.Items("mnuAnalogParameter").Click, AddressOf mnuParameterCurrent_Click

            End If

            '====================================================== PARAMETER LQ/HA
            If (.STR_No.Length > 0) And (.STR_Name.Length > 0) And (.Parameter_Type.ToString <> "") Then
                If tmpMenu.Items("tsSeparator5").Visible = False Then tmpMenu.Items("tsSeparator5").Visible = True
                tmpMenu.Items("mnuParameterLQ_HA").Visible = True
                'mnuParameterLQ_HA
                Select Case .Parameter_Type.ToString
                    Case "0"
                        If MDI_FRM.tsCboLanguage.Text <> "EN" Then '=================== For multi language
                            tmpMenu.Items("mnuParameterLQ_HA").Text = Query_Text_By_LanguageCode("LIQUID PARAMETER", MDI_FRM.tsCboLanguage.Text, CnBatching)
                        Else
                            tmpMenu.Items("mnuParameterLQ_HA").Text = "LIQUID PARAMETER"
                        End If

                        AddHandler tmpMenu.Items("mnuParameterLQ_HA").Click, AddressOf mnuParameterLQ_Click
                    Case "1"
                        If MDI_FRM.tsCboLanguage.Text <> "EN" Then '=================== For multi language
                            tmpMenu.Items("mnuParameterLQ_HA").Text = Query_Text_By_LanguageCode("HANDADD PARAMETER", MDI_FRM.tsCboLanguage.Text, CnBatching)
                        Else
                            tmpMenu.Items("mnuParameterLQ_HA").Text = "HANDADD PARAMETER"
                        End If
                        'tmpMenu.Items("mnuParameterLQ_HA").Text = "HANDADD PARAMETER"
                        AddHandler tmpMenu.Items("mnuParameterLQ_HA").Click, AddressOf mnuParameterHA_Click
                End Select
            End If

            '====================================================== PARAMETER BIN
            If .Bin_Number <> "" And .Bin_Step <> "" Then
                If .Ad_Bin_Low = "" Then .Ad_Bin_Low = "M0"
                If tmpMenu.Items("tsSeparator5").Visible = False Then tmpMenu.Items("tsSeparator5").Visible = True
                tmpMenu.Items("mnuBinParameter").Visible = True
                AddHandler tmpMenu.Items("mnuBinParameter").Click, AddressOf mnuBinParameter_Click
            End If

            '====================================================== MANUAL OPERATE
            If IsNothing(.OP_Bin_index) = False And IsNothing(.OP_Scale_No) = False Then
                If .OP_Bin_index.Length > 0 And .OP_Scale_No.Length > 0 Then
                    If tmpMenu.Items("tsSeparator2").Visible = False Then tmpMenu.Items("tsSeparator2").Visible = True

                    tmpMenu.Items("mnuStart").Visible = False
                    tmpMenu.Items("mnuStop").Visible = False

                    If .status_auto = True Then
                        tmpMenu.Items("mnuJog").Enabled = False
                    Else
                        If MDI_FRM.tsCboLanguage.Text <> "EN" Then '=================== For multi language
                            tmpMenu.Items("mnuJog").Text = Query_Text_By_LanguageCode("MANUAL OPERATE", MDI_FRM.tsCboLanguage.Text, CnBatching) & " [" & .Ad_Output & "] "
                        Else
                            tmpMenu.Items("mnuJog").Text = "MANUAL OPERATE" & " [" & .Ad_Output & "] "
                        End If

                        tmpMenu.Items("mnuJog").Enabled = True
                    End If
                    tmpMenu.Items("mnuJog").Visible = True
                    AddHandler tmpMenu.Items("mnuJog").Click, AddressOf mnuManualOperate_Click
                End If
            End If

            If IsNothing(.OP_LQ_Name) = False Then
                If .OP_LQ_Name.Length > 0 Then
                    If tmpMenu.Items("tsSeparator2").Visible = False Then tmpMenu.Items("tsSeparator2").Visible = True

                    tmpMenu.Items("mnuStart").Visible = False
                    tmpMenu.Items("mnuStop").Visible = False

                    If .status_auto = True Then
                        tmpMenu.Items("mnuManualLQ").Enabled = False
                    Else
                        tmpMenu.Items("mnuManualLQ").Enabled = True
                    End If

                    tmpMenu.Items("mnuManualLQ").Visible = True
                    AddHandler tmpMenu.Items("mnuManualLQ").Click, AddressOf mnuManualLiquid_Click
                End If
            End If

        End With
    End Function
#End Region

#Region "MENU SELECT"
    Function Show_SelectMenu(sender As Object, point As Point)
        objName = DirectCast(sender, Control).Name '===== For Keep log
        Dim menu_new As New frmMenu_Motor
        menu_Frm = menu_new.ctxMenuStrip

        '=================== For multi language
        change_language_in_toolStrip(menu_Frm.Items, CnBatching, MDI_FRM.tsCboLanguage.Text)

        Call StartUpMenu(menu_Frm)

        If TypeOf sender Is TAT_CtrlSelector.ctrlSelectorMode_ Then
            ctrlSelect_tmp = sender
        End If
        menu_Frm.Items("mnuHeader").Text = ctrlSelect_tmp.mqtt_selectmode_config_.CODE
        With ctrlSelect_tmp.mqtt_selectmode_status_
            '=================================================== MENU AUTO - MANUAL
            If ctrlSelect_tmp.mqtt_selectmode_config_.M_AUTO <> "M0" And ctrlSelect_tmp.mqtt_selectmode_config_.M_AUTO.Length <> 0 Then
                menu_Frm.Items("tsSeparator3").Visible = True
                menu_Frm.Items("mnuSelect_Auto").Visible = True
                menu_Frm.Items("mnuSelect_Manual").Visible = True
                AddHandler menu_Frm.Items("mnuSelect_Auto").Click, AddressOf mnuSelectAuto_Click
                AddHandler menu_Frm.Items("mnuSelect_Manual").Click, AddressOf mnuSelectManual_Click

                If .STA_AUTO = True Then
                    menu_Frm.Items("mnuSelect_Auto").Enabled = False
                    menu_Frm.Items("mnuSelect_Auto").Text = menu_Frm.Items("mnuSelect_Auto").Text & " <<<<"
                    menu_Frm.Items("mnuSelect_Manual").Enabled = True
                    menu_Frm.Items("mnuSelect_Manual").Text = Replace(menu_Frm.Items("mnuSelect_Manual").Text, " <<<<", "")
                Else
                    menu_Frm.Items("mnuSelect_Auto").Enabled = True
                    menu_Frm.Items("mnuSelect_Auto").Text = Replace(menu_Frm.Items("mnuSelect_Auto").Text, " <<<<", "")
                    menu_Frm.Items("mnuSelect_Manual").Enabled = False
                    menu_Frm.Items("mnuSelect_Manual").Text = menu_Frm.Items("mnuSelect_Manual").Text & " <<<<"
                End If
            End If

            If ctrlSelect_tmp.mqtt_selectmode_config_.M_HOLD <> "M0" And ctrlSelect_tmp.mqtt_selectmode_config_.M_HOLD.Length <> 0 Then
                menu_Frm.Items("mnuSelect_Hold").Visible = True
                AddHandler menu_Frm.Items("mnuSelect_Hold").Click, AddressOf mnuSelectHold_Click
                If InStr(ctrlSelect_tmp.mqtt_selectmode_config_.CODE, "AUTO/MAN") > 0 Then
                    If .STA_HOLD = True Then
                        menu_Frm.Items("mnuSelect_Hold").Text = "BY PASS"
                    Else
                        menu_Frm.Items("mnuSelect_Hold").Text = "PASS"
                    End If
                Else
                    If .STA_HOLD = True Then
                        menu_Frm.Items("mnuSelect_Hold").Text = "UN HOLD"
                    Else
                        menu_Frm.Items("mnuSelect_Hold").Text = "HOLD"
                    End If
                    menu_Frm.Items("mnuMonitoring").Visible = True
                    AddHandler menu_Frm.Items("mnuMonitoring").Click, AddressOf mnuSelectMonitoring_Click
                End If
            End If
            menu_Frm.Items("mnuParameter").Visible = True
            AddHandler menu_Frm.Items("mnuParameter").Click, AddressOf mnuSelectParameter_Click
            If ctrlSelect_tmp.AddressChangeBatch_ <> "" Or ctrlSelect_tmp.AddressChangeBatch_ = "R0" Then
                menu_Frm.Items("mnuChangeBatchPreset").Visible = ctrlSelect_tmp.ChangeBatch_
                AddHandler menu_Frm.Items("mnuChangeBatchPreset").Click, AddressOf mnuSelectChangeBatch_Click ' CHANGE_BATCH_MouseUp
            End If
        End With

        menu_Frm.Show(sender, point)

    End Function

#End Region

#Region "MENU ROUTE"
    Function Show_RouteMenu(sender As Object, point As Point)
        objName = DirectCast(sender, Control).Name '===== For Keep log
        Dim menu_new As New frmMenu_Motor
        menu_Frm = menu_new.ctxMenuStrip

        '=================== For multi language
        change_language_in_toolStrip(menu_Frm.Items, CnBatching, MDI_FRM.tsCboLanguage.Text)

        Call StartUpMenu(menu_Frm)

        If TypeOf sender Is TAT_CtrlRoute.ctrlRouteConveyer_ Then
            ctrlRoute_tmp = sender
        End If
        menu_Frm.Items("mnuHeader").Text = "ROUTE " & ctrlRoute_tmp.mqtt_selectroute_config_.LOCATION

        'If menu_Frm.Items("tsSeparator8").Visible = False Then menu_Frm.Items("tsSeparator8").Visible = True
        'menu_Frm.Items("mnuSetJogTime").Visible = True
        'menu_Frm.Items("mnuSetLowTime").Visible = True
        'menu_Frm.Items("mnuSetHightTime").Visible = True
        'menu_Frm.Items("mnuSetWeight").Visible = True

        If menu_Frm.Items("tsSeparator9").Visible = False Then menu_Frm.Items("tsSeparator9").Visible = True
        menu_Frm.Items("mnuShowRoute").Visible = True
        AddHandler menu_Frm.Items("mnuShowRoute").Click, AddressOf mnuShowRoute_Click

        menu_Frm.Items("mnuHideRoute").Visible = True
        AddHandler menu_Frm.Items("mnuHideRoute").Click, AddressOf mnuHideRoute_Click

        menu_Frm.Items("mnuSetCleanTime").Visible = True
        AddHandler menu_Frm.Items("mnuSetCleanTime").Click, AddressOf mnuSetCleanTime_Click

        menu_Frm.Show(sender, point)

    End Function

    Function Show_JogRouteMenu(sender As Object, point As Point)
        objName = DirectCast(sender, Control).Name '===== For Keep log
        Dim menu_new As New frmMenu_Motor
        menu_Frm = menu_new.ctxMenuStrip

        '=================== For multi language
        change_language_in_toolStrip(menu_Frm.Items, CnBatching, MDI_FRM.tsCboLanguage.Text)

        Call StartUpMenu(menu_Frm)

        If TypeOf sender Is TAT_CtrlRoute.ctrlJogRoute_ Then
            ctrlAutoChangRoute_tmp = sender
        End If
        menu_Frm.Items("mnuHeader").Text = "ROUTE " & ctrlAutoChangRoute_tmp.mqtt_selectroute_config_.LOCATION

        If menu_Frm.Items("tsSeparator10").Visible = False Then menu_Frm.Items("tsSeparator10").Visible = True
        menu_Frm.Items("mnuSetTarJog").Visible = True
        AddHandler menu_Frm.Items("mnuSetTarJog").Click, AddressOf mnuSetTarJog_Click
        menu_Frm.Items("mnuSetActJog").Visible = True
        AddHandler menu_Frm.Items("mnuSetActJog").Click, AddressOf mnuSetActJog_Click

        menu_Frm.Show(sender, point)

    End Function

    Function Show_AutoChangeRouteMenu(sender As Object, point As Point)
        objName = DirectCast(sender, Control).Name '===== For Keep log
        Dim menu_new As New frmMenu_Motor
        menu_Frm = menu_new.ctxMenuStrip

        '=================== For multi language
        change_language_in_toolStrip(menu_Frm.Items, CnBatching, MDI_FRM.tsCboLanguage.Text)

        Call StartUpMenu(menu_Frm)

        If TypeOf sender Is TAT_CtrlRoute.ctrlAutoChage_ Then
            ctrlAutoChangeRoute_tmp = sender
        End If
        menu_Frm.Items("mnuHeader").Text = "ROUTE " & ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.LOCATION

        If menu_Frm.Items("tsSeparator8").Visible = False Then menu_Frm.Items("tsSeparator8").Visible = True
        menu_Frm.Items("mnuSetJogTime").Visible = True
        AddHandler menu_Frm.Items("mnuSetJogTime").Click, AddressOf mnuSetTarJog_Click
        menu_Frm.Items("mnuSetLowTime").Visible = True
        AddHandler menu_Frm.Items("mnuSetLowTime").Click, AddressOf mnuSetLowTime_Click
        menu_Frm.Items("mnuSetHightTime").Visible = True
        AddHandler menu_Frm.Items("mnuSetHightTime").Click, AddressOf mnuSetHightTime_Click
        menu_Frm.Items("mnuSetWeight").Visible = True
        AddHandler menu_Frm.Items("mnuSetWeight").Click, AddressOf mnuSetWeight_Click

        menu_Frm.Show(sender, point)

    End Function
#End Region

#Region "EVENT MOTOR, SLIDE, FLAP, DEVICE"

    Private Sub mnuAuto_Click(sender As Object, e As EventArgs)
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer
        If ConnectPLC(tmpCtrl.PLC_Station_No) Then
            'Call Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Auto, 1)
        ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Auto, 1)

            '============================================= FOR KEEP LOG
            strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & ")," & tmpCtrl.Ad_Auto & ",1)"
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '============================================= FOR KEEP LOG

            Mx.MxCom(tmpCtrl.PLC_Station_No).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & tmpCtrl.PLC_Station_No & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
        'Call Mx.mxDevSetM(Mx.MxCom1, tmpCtrl.objConfig.M_MODE, 1)
        'Mx.MxCom1.Close()
    End Sub

    Private Sub mnuManual_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer
        If ConnectPLC(tmpCtrl.PLC_Station_No) Then
            'Call Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Auto, 0)
            ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Auto, 0)

            '================================================== FOR KEEP LOG
            strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Auto & ",0)"
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG

            Mx.MxCom(tmpCtrl.PLC_Station_No).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & tmpCtrl.PLC_Station_No & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
        'Call Mx.mxDevSetM(Mx.MxCom1, tmpCtrl.objConfig.M_MODE, 0)
        'Mx.MxCom1.Close()
    End Sub

    Private Sub mnuAuto_PID_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        If ConnectPLC(tmpCtrl.PLC_Station_No) Then
            Call Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Auto_PID, 1)

            '================================================== FOR KEEP LOG
            strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Auto_PID & ",1)"
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG

            Mx.MxCom(tmpCtrl.PLC_Station_No).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & tmpCtrl.PLC_Station_No & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
    End Sub

    Private Sub mnuManual_PID_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        If ConnectPLC(tmpCtrl.PLC_Station_No) Then
            ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Auto_PID, 0)
            '================================================== FOR KEEP LOG
            strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Auto_PID & ",0)"
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
            Mx.MxCom(tmpCtrl.PLC_Station_No).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & tmpCtrl.PLC_Station_No & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
    End Sub


    Private Sub mnuManualStart_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer
        If ConnectPLC(tmpCtrl.PLC_Station_No) Then
            If tmpCtrl.ControlType.ToString = "DEVICE" Then
                If tmpCtrl.Ad_Output.Length = 0 Or tmpCtrl.Ad_Output = "M0" Then
                    ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Run, 1)
                    '================================================== FOR KEEP LOG
                    strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Run & ",1)"
                    WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                    '================================================== FOR KEEP LOG
                Else
                    ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Output, 1)
                    '================================================== FOR KEEP LOG
                    strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Output & ",1)"
                    WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                    '================================================== FOR KEEP LOG
                End If
            Else
                '=========== FOr test Function Test_IO
                If Mode_Test_IO = True Then
                    If Check_Already_Test(ctrlMotor_tmp, CnBatching) = False Then
                        func_start_test(ctrlMotor_tmp, CnBatching, UserLogon_.UserName)
                    End If
                End If
                ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Output, 1)
                '================================================== FOR KEEP LOG
                strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Output & ",1)"
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            End If

            Mx.MxCom(tmpCtrl.PLC_Station_No).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & tmpCtrl.PLC_Station_No & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
    End Sub

    Private Sub mnuManualStop_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer
        If ConnectPLC(tmpCtrl.PLC_Station_No) Then
            If tmpCtrl.ControlType.ToString = "DEVICE" Then
                If tmpCtrl.Ad_Output.Length = 0 Or tmpCtrl.Ad_Output = "M0" Then
                    ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Run, 0)
                    '================================================== FOR KEEP LOG
                    strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Run & ",0)"
                    WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                    '================================================== FOR KEEP LOG
                Else
                    ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Output, 0)
                    '================================================== FOR KEEP LOG
                    strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Output & ",0)"
                    WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                    '================================================== FOR KEEP LOG
                End If
            Else
                ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Output, 0)
                '================================================== FOR KEEP LOG
                strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Output & ",0)"
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            End If
            Mx.MxCom(tmpCtrl.PLC_Station_No).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & tmpCtrl.PLC_Station_No & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
    End Sub

    Private Sub mnuResetError_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer
        If ConnectPLC(tmpCtrl.PLC_Station_No) Then
            If tmpCtrl.Ad_Err1 <> "M0" And tmpCtrl.Ad_Err1.Length <> 0 Then
                ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Err1, 0)
                '================================================== FOR KEEP LOG
                strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Err1 & ",0)"
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            End If
            If tmpCtrl.Ad_Err2 <> "M0" And tmpCtrl.Ad_Err2.Length <> 0 Then
                ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Err2, 0)
                '================================================== FOR KEEP LOG
                strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Err2 & ",0)"
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            End If
            If tmpCtrl.Ad_Coverlock <> "M0" And tmpCtrl.Ad_Coverlock.Length <> 0 Then
                ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Coverlock, 0)
                '================================================== FOR KEEP LOG
                strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Coverlock & ",0)"
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            End If

            Mx.MxCom(tmpCtrl.PLC_Station_No).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & tmpCtrl.PLC_Station_No & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
    End Sub

    Private Sub mnuUnderRepair_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer
        If ConnectPLC(tmpCtrl.PLC_Station_No) Then
            If tmpCtrl.status_service = True Then
                ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Service, 0)
                '================================================== FOR KEEP LOG
                strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Service & ",0)"
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            Else
                ret = Mx.mxDevSetM(Mx.MxCom(tmpCtrl.PLC_Station_No), tmpCtrl.Ad_Service, 1)
                '================================================== FOR KEEP LOG
                strCommand = "Mx.mxDevSetM(Mx.MxCom(" & tmpCtrl.PLC_Station_No & "), " & tmpCtrl.Ad_Service & ",1)"
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            End If
            Mx.MxCom(tmpCtrl.PLC_Station_No).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & tmpCtrl.PLC_Station_No & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
    End Sub

    Private Sub mnuParameterCurrent_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim strCmd_Line As String
        Dim adR_TonHour As String
        Select Case tmpCtrl.cmdLine_String
            Case "BUCKET" '1 BUCKET BUCKET_1
                strCmd_Line = tmpCtrl.PLC_Station_No & " BUCKET BUCKET_" & tmpCtrl.Ad_Parameter_Current & " " & UserLogon_.UserCode
                Call_EXE("ANALOG_PARAMETER", strCmd_Line, "BUCKET_" & tmpCtrl.Index)

            Case "GRINDING" '1 GRINDING GRINDING_1 mixer
                strCmd_Line = tmpCtrl.PLC_Station_No & " GRINDING GRINDING_" & tmpCtrl.Ad_Parameter_Current & " " & UserLogon_.UserCode
                Call_EXE("ANALOG_PARAMETER", strCmd_Line, "GRINDING_" & tmpCtrl.Index)

            Case "MIXER" '1 MIXER MIXER_1 mixer
                strCmd_Line = tmpCtrl.PLC_Station_No & " MIXER MIXER_" & tmpCtrl.Ad_Parameter_Current & " " & UserLogon_.UserCode
                Call_EXE("ANALOG_PARAMETER", strCmd_Line, "MIXER_" & tmpCtrl.Index)

            Case "LIQUID" '1 LIQUID LIQUID_1 mixer
                strCmd_Line = tmpCtrl.PLC_Station_No & " LIQUID LIQUID_" & tmpCtrl.Ad_Parameter_Current & " " & UserLogon_.UserCode
                Call_EXE("ANALOG_PARAMETER", strCmd_Line, "LIQUID_" & tmpCtrl.Index)

            Case "PELLET" '1 PELLET PELLET_1 mixer
                strCmd_Line = tmpCtrl.PLC_Station_No & " PELLET PELLET_" & tmpCtrl.Ad_Parameter_Current & " " & UserLogon_.UserCode
                Call_EXE("ANALOG_PARAMETER", strCmd_Line, "PELLET_" & tmpCtrl.Index)

            Case "COOLER" '1 COOLER COOLER_1 mixer
                strCmd_Line = tmpCtrl.PLC_Station_No & " COOLER COOLER_" & tmpCtrl.Ad_Parameter_Current & " " & UserLogon_.UserCode
                Call_EXE("ANALOG_PARAMETER", strCmd_Line, "COOLER_" & tmpCtrl.Index)

            Case "BLOWER" '1 BLOWER BLOWER_GD_1 mixer
                strCmd_Line = tmpCtrl.PLC_Station_No & " BLOWER BLOWER_" & tmpCtrl.Ad_Parameter_Current & " " & UserLogon_.UserCode
                Call_EXE("ANALOG_PARAMETER", strCmd_Line, "BLOWER_" & tmpCtrl.Index)

            Case "ANALOGSENSORINPUT" '1 ANALOGSENSORINPUT ANALOGSENSORINPUT_1 mixer
                strCmd_Line = tmpCtrl.PLC_Station_No & " ANALOGSENSORINPUT ANALOGSENSORINPUT_" & tmpCtrl.Ad_Parameter_Current & " " & UserLogon_.UserCode
                Call_EXE("ANALOG_PARAMETER", strCmd_Line, "ANALOGSENSORINPUT_" & tmpCtrl.Index)
            Case Else
                If tmpCtrl.cmdLine_String = "FEED" Then
                    If tmpCtrl.Ad_Ton_Per_Hour.Length > 0 Then adR_TonHour = tmpCtrl.Ad_Ton_Per_Hour Else adR_TonHour = "R0"
                Else
                    adR_TonHour = "R0"
                End If
                strCmd_Line = tmpCtrl.cmdLine_String & " " & tmpCtrl.PLC_Station_No & " " & tmpCtrl.Index & " " & tmpCtrl.Ad_Parameter_Current & " " &
                              tmpCtrl.cmdLine_String & "_" & tmpCtrl.M_Code & " localhost " & adR_TonHour
                Call_EXE("Current_Config", strCmd_Line, tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index)
        End Select

        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""ANALOG_PARAMETER"", " & strCmd_Line & ", " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & ")"
        Try
            'Call_EXE("Current_Config", strCmd_Line, tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index)
            '================================================== FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
        Catch ex As Exception
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Open Exe Error (NAME : Current_Config )" & vbCrLf & ex.StackTrace
            ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End Try

    End Sub

    Private Sub mnuPidConfig_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim tmpMotor_ID As String
        Dim tmpCmd_Line As String
        tmpMotor_ID = Get_MotorID(tmpCtrl.M_Code, tmpCtrl.PLC_Station_No)
        tmpCmd_Line = tmpCtrl.PLC_Station_No & " " & tmpMotor_ID & " " & tmpCtrl.Ad_Parameter_PID & " " & "" + tmpCtrl.Ad_Output + " ASA localhost"

        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""PID Config"", " & tmpCmd_Line & ", " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & ")"
        Try
            Call_EXE("PID Config", tmpCmd_Line, tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index)
            '================================================== FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
        Catch ex As Exception
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Open Exe Error (NAME : PID Config )" & vbCrLf & ex.StackTrace
            ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End Try

    End Sub

    Private Sub mnuParameterLQ_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim tmpCmd_line As String
        tmpCmd_line = tmpCtrl.PLC_Station_No & " " & tmpCtrl.STR_No & " " & tmpCtrl.STR_Name & " localhost " & tmpCtrl.PLC_Station_No & " ASA "

        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""Parameter"", " & tmpCmd_line & ", " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & ")"
        Try
            Call_EXE("Parameter", tmpCmd_line, tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index)
            '================================================== FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
        Catch ex As Exception
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Open Exe Error (NAME : Parameter )" & vbCrLf & ex.StackTrace
            ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End Try
    End Sub

    Private Sub mnuParameterHA_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim tmpCmd_line As String
        tmpCmd_line = tmpCtrl.PLC_Station_No & " HANDADD " & tmpCtrl.STR_Name
        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""Handadd Parameter"", " & tmpCmd_line & ", " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & ")"
        If tmpCmd_line <> "" Then
            Try
                Call_EXE("Handadd Parameter", tmpCmd_line, tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index)
                '================================================== FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            Catch ex As Exception
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Open Exe Error (NAME : Handadd Parameter )" & vbCrLf & ex.StackTrace
                ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End Try

        End If

    End Sub

    Private Sub mnuBinParameter_Click()
        Dim m$ = NameOf(mnuBinParameter_Click)
        If Object.Equals(tmpCtrl, Nothing) Then Exit Sub

        Dim dev$ = $"{tmpCtrl.ControlType}_{tmpCtrl.Index}"
        Dim cmd$ = Get_Parameter_Bin_CmdLine(tmpCtrl.PLC_Station_No, tmpCtrl.Bin_Number, tmpCtrl.Bin_Step, tmpCtrl.Ad_Bin_Low, dt_Bin_Parameter_, UserLogon_.UserCode)
        If String.IsNullOrEmpty(cmd) Then Exit Sub

        Dim log$ = $"Call_EXE(""BinParameter"", ""{cmd.Replace("""", """""")}"", ""{dev.Replace("""", """""")}"")"

        Try
            Call_EXE("BinParameter", cmd, dev)
            WriteLog_Menu(m, dev, tmpCtrl.M_Code, log, 1)
        Catch ex As Exception
            ErrorLog_Menu(m, dev, tmpCtrl.M_Code, log, ex.ToString())
        End Try
    End Sub

    Private Sub mnuManualOperate_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim tmpCmd_Line As String
        Dim strFull_Cmd_Line As String
        tmpCmd_Line = Get_Manual_Bin_CmdLine(tmpCtrl.PLC_Station_No, tmpCtrl.OP_Scale_No.ToString, dt_Scale_Parameter_)
        If tmpCmd_Line = "" Then MsgBox("SCALE DATA NOT FOUND")

        strFull_Cmd_Line = tmpCtrl.PLC_Station_No & " " & tmpCtrl.OP_Bin_index.ToString & " " & tmpCtrl.Ad_Output & " " & UserLogon_.UserCode

        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""Manual Operate Bin"", " & strFull_Cmd_Line & ", " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & ")"
        Try                '=========== FOr test Function Test_IO
            If Mode_Test_IO = True Then
                If Check_Already_Test(ctrlMotor_tmp, CnBatching) = False Then
                    func_start_test(ctrlMotor_tmp, CnBatching, UserLogon_.UserName)
                End If
            End If
            Call_EXE("Manual Operate Bin", strFull_Cmd_Line, tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index)
            '================================================== FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
        Catch ex As Exception
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Open Exe Error (NAME : Manual Operate Bin )" & vbCrLf & ex.StackTrace
            ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End Try

    End Sub

    Private Sub mnuManualLiquid_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim tmpCmd_Line As String

        tmpCmd_Line = tmpCtrl.PLC_Station_No & " LIQUID " & tmpCtrl.OP_LQ_Name & " " & tmpCtrl.Ad_Output & " " & UserLogon_.UserCode
        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""Manual Operate LQ"", " & tmpCmd_Line & ", " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & ")"
        Try
            '=========== FOr test Function Test_IO
            If Mode_Test_IO = True Then
                If Check_Already_Test(ctrlMotor_tmp, CnBatching) = False Then
                    func_start_test(ctrlMotor_tmp, CnBatching, UserLogon_.UserName)
                End If
            End If
            Call_EXE("Manual Operate LQ", tmpCmd_Line, tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index)
            '================================================== FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
        Catch ex As Exception
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Open Exe Error (NAME : Manual Operate Bin )" & vbCrLf & ex.StackTrace
            ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End Try


    End Sub

    Private Sub mnuSetDelayTime_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim tmpCmd_line As String
        'tmpCmd_line = tmpCtrl.PLC_Station_No & " " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & " ASA " & tmpCtrl.PLC_Station_No
        'Call_EXE("Set Delay Time", tmpCmd_line, tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index)

        '========================= Call Object Property
        With StrMqtt_Config_
            tmpCmd_line = tmpCtrl.PLC_Station_No & " " & .IpAddressMqtt & " " & .MqttUser & " " & .MqttPass & " " & tmpCtrl.ControlType.ToString & "_" &
                tmpCtrl.Index & " " & UserLogon_.UserCode
        End With
        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""ObjectProperty"", " & tmpCmd_line & ", " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & ")"
        Try
            Call_EXE("ObjectProperty", tmpCmd_line, tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index)
            '================================================== FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
        Catch ex As Exception
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Open Exe Error (NAME : ObjectProperty )" & vbCrLf & ex.StackTrace
            ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End Try


    End Sub

    Private Sub mnuSetDelayTimeHigh_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim tmpCmd_line As String
        With StrMqtt_Config_
            tmpCmd_line = tmpCtrl.PLC_Station_No & " " & .IpAddressMqtt & " " & .MqttUser & " " & .MqttPass & " " & tmpCtrl.Device_Index_Hi & " " & UserLogon_.UserCode
        End With
        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""ObjectProperty"", " & tmpCmd_line & ", " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & ")"
        Try
            Call_EXE("ObjectProperty", tmpCmd_line, tmpCtrl.Device_Index_Hi)
            '================================================== FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
        Catch ex As Exception
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Open Exe Error (NAME : ObjectProperty )" & vbCrLf & ex.StackTrace
            ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End Try

    End Sub

    Private Sub mnuSetDelayTimeLow_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index
        Dim machineCode As String = tmpCtrl.M_Code
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim tmpCmd_line As String
        With StrMqtt_Config_
            tmpCmd_line = tmpCtrl.PLC_Station_No & " " & .IpAddressMqtt & " " & .MqttUser & " " & .MqttPass & " " & tmpCtrl.Device_Index_Lo & " " & UserLogon_.UserCode
        End With
        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""ObjectProperty"", " & tmpCmd_line & ", " & tmpCtrl.ControlType.ToString & "_" & tmpCtrl.Index & ")"
        Try
            Call_EXE("ObjectProperty", tmpCmd_line, tmpCtrl.Device_Index_Lo)
            '================================================== FOR KEEP LOG
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG
        Catch ex As Exception
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Open Exe Error (NAME : ObjectProperty )" & vbCrLf & ex.StackTrace
            ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End Try

    End Sub
#End Region

#Region "EVENT SELECTOR"

    Private Sub mnuSelectAuto_Click(sender As Object, e As EventArgs)
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = ctrlSelect_tmp.mqtt_selectmode_config_.NAME
        Dim machineCode As String = ctrlSelect_tmp.mqtt_selectmode_config_.CODE
        Dim strCommand As String = "-"
        Dim ret As Integer
        If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
            ret = Mx.mxDevSetM(Mx.MxCom(StrMqtt_Config_.StationMqtt), ctrlSelect_tmp.mqtt_selectmode_config_.M_AUTO, 1)

            '============================================= FOR KEEP LOG
            strCommand = "Mx.mxDevSetM(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & ")," & ctrlSelect_tmp.mqtt_selectmode_config_.M_AUTO & ",1)"
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '============================================= FOR KEEP LOG

            Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
        'Call Mx.mxDevSetM(Mx.MxCom1, tmpCtrl.objConfig.M_MODE, 1)
        'Mx.MxCom1.Close()
    End Sub

    Private Sub mnuSelectManual_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = ctrlSelect_tmp.mqtt_selectmode_config_.NAME
        Dim machineCode As String = ctrlSelect_tmp.mqtt_selectmode_config_.CODE
        Dim strCommand As String = "-"
        Dim ret As Integer
        If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
            ret = Mx.mxDevSetM(Mx.MxCom(StrMqtt_Config_.StationMqtt), ctrlSelect_tmp.mqtt_selectmode_config_.M_AUTO, 0)

            '================================================== FOR KEEP LOG
            strCommand = "Mx.mxDevSetM(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlSelect_tmp.mqtt_selectmode_config_.M_AUTO & ",0)"
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG

            Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
        'Call Mx.mxDevSetM(Mx.MxCom1, tmpCtrl.objConfig.M_MODE, 0)
        'Mx.MxCom1.Close()
    End Sub

    Private Sub mnuSelectHold_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = ctrlSelect_tmp.mqtt_selectmode_config_.NAME
        Dim machineCode As String = ctrlSelect_tmp.mqtt_selectmode_config_.CODE
        Dim strCommand As String = "-"
        Dim ret As Integer
        Dim iStatus As Int16
        If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
            If ctrlSelect_tmp.mqtt_selectmode_status_.STA_HOLD = True Then
                iStatus = 0
            Else
                iStatus = 1
            End If
            ret = Mx.mxDevSetM(Mx.MxCom(StrMqtt_Config_.StationMqtt), ctrlSelect_tmp.mqtt_selectmode_config_.M_HOLD, iStatus)

            '================================================== FOR KEEP LOG
            strCommand = "Mx.mxDevSetM(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlSelect_tmp.mqtt_selectmode_config_.M_HOLD & "," & iStatus & ")"
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
            '================================================== FOR KEEP LOG

            Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
        Else
            '============================================= FOR KEEP LOG
            Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
            WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
            '============================================= FOR KEEP LOG
        End If
        'Call Mx.mxDevSetM(Mx.MxCom1, tmpCtrl.objConfig.M_MODE, 0)
        'Mx.MxCom1.Close()
    End Sub

    Private Sub mnuSelectParameter_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = ctrlSelect_tmp.mqtt_selectmode_config_.NAME
        Dim machineCode As String = ctrlSelect_tmp.mqtt_selectmode_config_.CODE
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim TmpName As String
        Dim sName As String
        Dim TmpStrName As String
        Dim TmpStrIndex As String
        Dim getStr() As String = Split(ctrlSelect_tmp.mqtt_selectmode_config_.CODE, ".")
        Select Case getStr.Count
            Case 1
                TmpStrName = RegularExpressions.Regex.Replace(ctrlSelect_tmp.mqtt_selectmode_config_.CODE, "[^A-Z]", "")
                TmpStrIndex = RegularExpressions.Regex.Replace(ctrlSelect_tmp.mqtt_selectmode_config_.CODE, "[^0-9]", "")
            Case 2
                TmpStrName = RegularExpressions.Regex.Replace(getStr(1), "[^A-Z]", "")
                TmpStrIndex = RegularExpressions.Regex.Replace(getStr(1), "[^0-9]", "")
            Case 3
                TmpStrName = RegularExpressions.Regex.Replace(getStr(2), "[^A-Z]", "")
                TmpStrIndex = RegularExpressions.Regex.Replace(getStr(2), "[^0-9]", "")
        End Select
        Select Case TmpStrName
            Case "SC"
                TmpName = "SCALE_" & TmpStrIndex
                sName = "SCALE"
            Case "SB"
                TmpName = "SURGEBIN_" & TmpStrIndex
                sName = "SURGEBIN"
            Case "MIX", "MIXER"
                TmpName = "MIXER_" & TmpStrIndex
                sName = "MIXER"
            Case "HA"
                TmpName = "HANDADD_" & TmpStrIndex
                sName = "HANDADD"
            Case "ML"
                TmpName = "SCALE_" & TmpStrIndex
                sName = "MOLASS"
            Case "LOAD", "LD"
                TmpName = "LOAD_" & TmpStrIndex
                sName = "LOAD"
            Case "AUTOMANIL", "INLINE"
                TmpName = "INLINE_" & TmpStrIndex
                sName = "INLINE"
            Case Else
                sName = "ERROR"
                TmpName = ctrlSelect_tmp.mqtt_selectmode_config_.CODE
        End Select

        Dim strCmd_Line As String
        strCmd_Line = StrMqtt_Config_.StationMqtt & " " & sName & " " & TmpName
        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""Parameter_Config," & TmpName & """, " & strCmd_Line & ")"
        If strCmd_Line <> "" Then
            Try
                Call_EXE("Parameter_Config", strCmd_Line, ctrlSelect_tmp.mqtt_selectmode_config_.NAME)
                '================================================== FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            Catch ex As Exception
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Open Exe Error (NAME : ParameterConfig )" & vbCrLf & ex.StackTrace
                ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End Try
        End If
    End Sub

    Private Sub mnuSelectMonitoring_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = ctrlSelect_tmp.mqtt_selectmode_config_.NAME
        Dim machineCode As String = ctrlSelect_tmp.mqtt_selectmode_config_.CODE
        Dim strCommand As String = "-"
        Dim ret As Integer
        Dim TmpName As String
        Dim getStr() As String = Split(ctrlSelect_tmp.mqtt_selectmode_config_.CODE, ".")
        Dim TmpStrName As String = RegularExpressions.Regex.Replace(getStr(1), "[^A-Z]", "")
        Dim TmpStrIndex As String = RegularExpressions.Regex.Replace(getStr(1), "[^0-9]", "")
        Select Case TmpStrName
            Case "SC"
                TmpName = "SCALE_" & TmpStrIndex
            Case "SB"
                TmpName = "SURGEBIN_" & TmpStrIndex
            Case "MIX", "MIXER"
                TmpName = "MIXER_" & TmpStrIndex
            Case "HA"
                TmpName = "HANDADD_" & TmpStrIndex
            Case Else
                TmpName = ctrlSelect_tmp.mqtt_selectmode_config_.CODE
        End Select

        Dim strCmd_Line As String
        strCmd_Line = StrMqtt_Config_.StationMqtt & " " & StrMqtt_Config_.IpAddressMqtt & " " & StrMqtt_Config_.MqttUser & " " & StrMqtt_Config_.MqttPass & " " & TmpName
        '================================================== FOR KEEP LOG
        strCommand = "Call_EXE(""Monitoring_Formula," & TmpName & """, " & strCmd_Line & ")"
        If strCmd_Line <> "" Then
            Try
                Call_EXE("Monitoring_Formula", strCmd_Line, ctrlSelect_tmp.mqtt_selectmode_config_.NAME)
                '================================================== FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '================================================== FOR KEEP LOG
            Catch ex As Exception
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Open Exe Error (NAME : ParameterConfig )" & vbCrLf & ex.StackTrace
                ErrorLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End Try
        End If
    End Sub

    Private Sub mnuSelectChangeBatch_Click(sender As Object, e As EventArgs)
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = ctrlSelect_tmp.mqtt_selectmode_config_.NAME
        Dim machineCode As String = ctrlSelect_tmp.mqtt_selectmode_config_.CODE
        Dim strCommand As String = "-"
        Dim ret As Integer

        FUNC_Change_BatchPreset(ctrlSelect_tmp.LocationChangeBatch_, ctrlSelect_tmp.AddressChangeBatch_, ctrlSelect_tmp.mqtt_selectmode_config_.M_AUTO,
strMenu, deviceName, machineCode)

    End Sub

    Public Sub FUNC_Change_BatchPreset(Str_Location As String, as_add As String, as_Mode As String,
                                       strMenu As String, deviceName As String, machineCode As String)
        Try
            Dim strCommand As String = "-"

            Dim send_buf(1) As Int16
            Dim buf(15) As Int16
            Dim ret, retF As Long
            Dim ls_BatchPreset As String
            Dim intBatchPreset As Integer
            Dim intBatchCount As Integer
            Dim li_new_batch As String
            Dim c_Production_no As String
            Dim nProd(0) As Int32
            ls_BatchPreset = CheckAdd(as_add, 12)
            If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
                retF = Mx.mxReadBlock2(Mx.MxCom(StrMqtt_Config_.StationMqtt), as_add, 14, buf)
                If retF = 0 Then
                    System.Buffer.BlockCopy(buf, 10 * 2, nProd, 0 * 2, 4)
                    c_Production_no = Format(CLng(nProd(0)), "0000000000")
                    intBatchPreset = buf(12)
                    intBatchCount = buf(13)
                    If intBatchPreset = 0 Then
                        MsgBox("HAVE NO CURRENT PRODUCTION! ", vbInformation + vbApplicationModal, "Change Batch Preset")
                        Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
                        Exit Sub
                    Else
                        Mx.mxDevSetM(Mx.MxCom(StrMqtt_Config_.StationMqtt), as_Mode, 0)
                        li_new_batch = InputBox("PLEASE ENTER NEW BATCH PRESET " & vbCrLf & "MINIMUM IS : " & intBatchCount & " BATCH ", "NEW BATCH PRESET")
                        Mx.mxDevSetM(Mx.MxCom(StrMqtt_Config_.StationMqtt), as_Mode, 1)
                        If li_new_batch = "" Then
                            Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
                            Exit Sub
                        ElseIf IsNumeric(li_new_batch) = False Then
                            MsgBox("INVALID NUMBER OF BATCH :   " & li_new_batch & vbCrLf & vbCrLf & "PLEASE ENTER NEW VALUES", vbCritical + vbApplicationModal, "NEW BATCH PRESET")
                            Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
                            Exit Sub
                        ElseIf CInt(li_new_batch) > 999 Then
                            MsgBox("INVALID RANGE NUMBER OF BATCH (1-999) " & vbCrLf & "PLEASE ENTER NEW VALUES", vbCritical + vbApplicationModal, "NEW BATCH PRESET")
                            Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
                            Exit Sub
                        ElseIf CInt(li_new_batch) < intBatchCount Then
                            MsgBox("PLEASE INPUT MORE THAN CURRENT BATCH! " & vbCrLf & "CURRENT IS " & intBatchCount & " BATCH! >> NEW BATCH IS  " & li_new_batch, vbCritical, "NEW BATCH PRESET")
                            Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
                            Exit Sub
                            'ElseIf CInt(li_new_batch) > MaxLengthChagePreset Then
                            '    MsgBox("PLEASE INPUT MORE THAN CURRENT BATCH! " & vbCrLf & "MAXIMUM IS " & MaxLengthChagePreset & " BATCH! >> NEW BATCH IS " & li_new_batch, vbCritical, "NEW BATCH PRESET")
                            '    Exit Sub
                        Else
                            send_buf(0) = CInt(li_new_batch)
                            ret = Mx.mxWriteBlock(Mx.MxCom(StrMqtt_Config_.StationMqtt), ls_BatchPreset, 1, send_buf)
                            If ret = 0 Then
                                FuncChangeBatch(CInt(li_new_batch), c_Production_no, Str_Location, deviceName, machineCode)
                            End If
                        End If
                    End If
                End If

                '============================================= FOR KEEP LOG
                strCommand = "Mx.mxDevSetM(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & ")," & ls_BatchPreset & ",1," & send_buf(0) & ")"
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '============================================= FOR KEEP LOG

                Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
            Else
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End If
        Catch ex As Exception
            'Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/FUNC_Change_BatchPreset]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            'LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub FuncChangeBatch(New_Preset As Int16, Product_Number As String, strLocation As String, deviceName As String, machineCode As String)
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim strCommand As String = "-"
        Dim ret As Integer
        Try
            Dim sqlStr As String
            strLocation = Replace(strLocation, "_", " ")
            sqlStr = "EXEC [thaisia].[fd_Change_Batch_Preset]"
            sqlStr = sqlStr & " @sProduction_number = '" & Product_Number & "',"
            sqlStr = sqlStr & " @nNewBatch = " & New_Preset & ","
            sqlStr = sqlStr & " @sLocation = '" & strLocation & "';"
            strCommand = sqlStr
            CnBatching.ExecuteNoneQuery(sqlStr)
            '================================================= FOR KEEP LOG ===================================================== 
            WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)

        Catch ex As Exception
            'Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/FuncChangeBatch]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            'LogError.writeErr(strMessage)
        End Try
    End Sub

#End Region

#Region "EVENT ROUTE"
    Private Sub mnuShowRoute_Click()
        SaveSetting("TAT", "ROUTE CONTROL", "" & ctrlRoute_tmp.mqtt_selectroute_config_.LOCATION & ".SHOWMODE", "1")
    End Sub

    Private Sub mnuHideRoute_Click()
        SaveSetting("TAT", "ROUTE CONTROL", "" & ctrlRoute_tmp.mqtt_selectroute_config_.LOCATION & ".SHOWMODE", "0")
    End Sub

    Private Sub mnuSetCleanTime_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = "ROUTE"
        Dim machineCode As String = ctrlRoute_tmp.mqtt_selectroute_config_.LOCATION
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim DataInput As Double
        Dim cInput As String
        cInput = InputBox("Please Input Clean Time", "Input Clean Time")
        If cInput <> "" Then
            If IsNumeric(cInput) = False Then
                Call MsgBox("PLEASE INPUT NUMERIC ONLY!", MsgBoxStyle.Critical, "ERROR")
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Invalid input data (DATA : " & cInput & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
                Exit Sub
            Else
                DataInput = CDbl(cInput)
            End If

            If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
                'strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlRoute_tmp.mqtt_selectroute_config_.ZR_ACT_CLEAN_TIME & ", "
                Dim DATA(1) As Int16
                DATA(0) = DataInput * 10
                Mx.mxWriteBlock(Mx.MxCom(StrMqtt_Config_.StationMqtt), ctrlRoute_tmp.mqtt_selectroute_config_.ZR_ACT_CLEAN_TIME, 1, DATA)
                strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlRoute_tmp.mqtt_selectroute_config_.ZR_ACT_CLEAN_TIME & ", 1," & DATA(0) & ")"
                '============================================= FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '============================================= FOR KEEP LOG
                Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
            Else
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End If
        End If
    End Sub

    Private Sub mnuSetTarJog_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = "ROUTE"
        Dim machineCode As String = ctrlAutoChangRoute_tmp.mqtt_selectroute_config_.LOCATION
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim DataInput As Double
        Dim cInput As String
        cInput = InputBox("Please Input Target Jog Time", "Input Target Jog Time")
        If cInput <> "" Then
            If IsNumeric(cInput) = False Then
                Call MsgBox("PLEASE INPUT NUMERIC ONLY!", MsgBoxStyle.Critical, "ERROR")
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Invalid input data (DATA : " & cInput & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
                Exit Sub
            Else
                DataInput = CDbl(cInput)
            End If

            If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
                'strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlRoute_tmp.mqtt_selectroute_config_.ZR_ACT_CLEAN_TIME & ", "
                Dim DATA(1) As Int16
                DATA(0) = DataInput * 10
                Mx.mxWriteBlock(Mx.MxCom(StrMqtt_Config_.StationMqtt), ctrlAutoChangRoute_tmp.mqtt_selectroute_config_.R_TAR_JOG, 1, DATA)
                strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlAutoChangRoute_tmp.mqtt_selectroute_config_.R_TAR_JOG & ", 1," & DATA(0) & ")"
                '============================================= FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '============================================= FOR KEEP LOG
                Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
            Else
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End If
        End If
    End Sub

    Private Sub mnuSetActJog_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = "ROUTE"
        Dim machineCode As String = ctrlAutoChangRoute_tmp.mqtt_selectroute_config_.LOCATION
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim DataInput As Double
        Dim cInput As String
        cInput = InputBox("Please Input Actual Jog Time", "Input Actual Jog Time")
        If cInput <> "" Then
            If IsNumeric(cInput) = False Then
                Call MsgBox("PLEASE INPUT NUMERIC ONLY!", MsgBoxStyle.Critical, "ERROR")
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Invalid input data (DATA : " & cInput & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
                Exit Sub
            Else
                DataInput = CDbl(cInput)
            End If

            If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
                'strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlRoute_tmp.mqtt_selectroute_config_.ZR_ACT_CLEAN_TIME & ", "
                Dim DATA(1) As Int16
                DATA(0) = DataInput * 10
                Mx.mxWriteBlock(Mx.MxCom(StrMqtt_Config_.StationMqtt), ctrlAutoChangRoute_tmp.mqtt_selectroute_config_.R_ACT_JOG, 1, DATA)
                strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlAutoChangRoute_tmp.mqtt_selectroute_config_.R_ACT_JOG & ", 1," & DATA(0) & ")"
                '============================================= FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '============================================= FOR KEEP LOG
                Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
            Else
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End If
        End If
    End Sub

    Private Sub mnuSetLowTime_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = "ROUTE"
        Dim machineCode As String = ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.LOCATION
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim DataInput As Double
        Dim cInput As String

        Select Case strMenu
            Case ""
        End Select

        cInput = InputBox("Please Input Target Low level Time", "Input Target Low level Time")
        If cInput <> "" Then
            If IsNumeric(cInput) = False Then
                Call MsgBox("PLEASE INPUT NUMERIC ONLY!", MsgBoxStyle.Critical, "ERROR")
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Invalid input data (DATA : " & cInput & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
                Exit Sub
            Else
                DataInput = CDbl(cInput)
            End If

            If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
                Dim DATA(1) As Int16
                DATA(0) = DataInput * 10
                Mx.mxWriteBlock(Mx.MxCom(StrMqtt_Config_.StationMqtt), ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.ZR_TAR_LOW_TIME, 1, DATA)
                strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.ZR_TAR_LOW_TIME & ", 1," & DATA(0) & ")"
                '============================================= FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '============================================= FOR KEEP LOG
                Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
            Else
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End If
        End If
    End Sub

    Private Sub mnuSetHightTime_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = "ROUTE"
        Dim machineCode As String = ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.LOCATION
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim DataInput As Double
        Dim cInput As String

        Select Case strMenu
            Case ""
        End Select

        cInput = InputBox("Please Input Target Hight level Time", "Input Target Hight level Time")
        If cInput <> "" Then
            If IsNumeric(cInput) = False Then
                Call MsgBox("PLEASE INPUT NUMERIC ONLY!", MsgBoxStyle.Critical, "ERROR")
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Invalid input data (DATA : " & cInput & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
                Exit Sub
            Else
                DataInput = CDbl(cInput)
            End If

            If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
                Dim DATA(1) As Int16
                DATA(0) = DataInput * 10
                Mx.mxWriteBlock(Mx.MxCom(StrMqtt_Config_.StationMqtt), ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.ZR_TAR_HIGH_TIME, 1, DATA)
                strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.ZR_TAR_HIGH_TIME & ", 1," & DATA(0) & ")"
                '============================================= FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '============================================= FOR KEEP LOG
                Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
            Else
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End If
        End If
    End Sub

    Private Sub mnuSetWeight_Click()
        '=== FOR KEEP LOG
        Dim strMenu As String = MethodBase.GetCurrentMethod.Name
        Dim deviceName As String = "ROUTE"
        Dim machineCode As String = ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.LOCATION
        Dim strCommand As String = "-"
        Dim ret As Integer

        Dim DataInput As Double
        Dim cInput As String

        Select Case strMenu
            Case ""
        End Select

        cInput = InputBox("Please Input Target Weight", "Input Target Weight (Kg.)")
        If cInput <> "" Then
            If IsNumeric(cInput) = False Then
                Call MsgBox("PLEASE INPUT NUMERIC ONLY!", MsgBoxStyle.Critical, "ERROR")
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Invalid input data (DATA : " & cInput & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
                Exit Sub
            Else
                DataInput = CDbl(cInput)
            End If

            If ConnectPLC(StrMqtt_Config_.StationMqtt) Then
                Dim send_long(1) As Int32
                Dim target_val(1) As Short
                send_long(0) = DataInput * 1000
                System.Buffer.BlockCopy(send_long, 0, target_val, 0, 4)
                Mx.mxWriteBlock(Mx.MxCom(StrMqtt_Config_.StationMqtt), ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.ZR_TAR_WEIGHT, 2, target_val)
                strCommand = "Mx.mxWriteBlock(Mx.MxCom(" & StrMqtt_Config_.StationMqtt & "), " & ctrlAutoChangeRoute_tmp.mqtt_selectroute_config_.ZR_TAR_WEIGHT & ", 2," & target_val(0) & ")"
                '============================================= FOR KEEP LOG
                WriteLog_Menu(strMenu, deviceName, machineCode, strCommand, ret)
                '============================================= FOR KEEP LOG
                Mx.MxCom(StrMqtt_Config_.StationMqtt).Close()
            Else
                '============================================= FOR KEEP LOG
                Dim strMsg As String = "Can not connect PLC (STATION : " & StrMqtt_Config_.StationMqtt & " )"
                WarningLog_Menu(strMenu, deviceName, machineCode, strCommand, strMsg)
                '============================================= FOR KEEP LOG
            End If
        End If
    End Sub

#End Region

#Region "Option"
    Public dt_Scale_Parameter_ As DataTable
    Public dt_Bin_Parameter_ As DataTable
    Public dt_SB_HA_Parameter_ As DataTable

    Public Sub Get_Database_For_Mqtt_Ctrl()
        Dim strSql As String

        strSql = "Select * From thaisia.scale_parameter_"
        dt_Scale_Parameter_ = New DataTable
        dt_Scale_Parameter_ = CnBatching.ExecuteDataTable(strSql)

        strSql = "Select * from thaisia.bin_parameter_"
        dt_Bin_Parameter_ = New DataTable
        dt_Bin_Parameter_ = CnBatching.ExecuteDataTable(strSql)

        strSql = "Select * from thaisia.handadd_and_surgebin_parameter"
        dt_SB_HA_Parameter_ = New DataTable
        dt_SB_HA_Parameter_ = CnBatching.ExecuteDataTable(strSql)

    End Sub

    Function Get_MotorID(ByVal strMotor_Code As String, ByVal strPLC_Stn As String) As String
        Dim strSQL As String = ""
        Try
            Dim conn As New clsDB(Auto_Route_Conf.Name, Auto_Route_Conf.User, Auto_Route_Conf.Password, Auto_Route_Conf.IPAddress, Auto_Route_Conf.Connection_Type)
            Dim dtMotorID As New DataTable

            strSQL = "SELECT motor_id FROM thaisia.motor_config " &
              "WHERE motor_code = '" & strMotor_Code & "' AND " &
              "n_plc_station = '" & strPLC_Stn & "' "

            dtMotorID = conn.ExecuteDataTable(strSQL)

            If dtMotorID.Rows.Count > 0 Then
                Return dtMotorID.Rows(0)(0).ToString()
            Else
                Return ""
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred while retrieving the MotorID." & vbCrLf &
                     "SQL: " & strSQL & vbCrLf &
                     "Error details: " & ex.Message,
                     "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return ""
        End Try
    End Function
#End Region

#Region "Keep Log"
    Private Sub WriteLog_Menu(strMenu As String, deviceName As String, machineCode As String, strCommand As String, ret As Integer)
        Dim strLog As String
        strLog = "ACTION : " & strMenu & vbCrLf
        strLog += "USER : " & UserLogon_.UserName & vbCrLf
        strLog += "OBJECT NAME : " & objName & vbCrLf
        strLog += "DEVICE NAME : " & deviceName & vbCrLf
        strLog += "MACHINE CODE : " & machineCode & vbCrLf
        strLog += "COMMAND : " & strCommand & vbCrLf
        strLog += "RETURN : " & ret

        Dim eventID As Long = 0
        If InStr(strCommand, "Call_EXE") > 0 Then
            eventID = 3000
        Else
            eventID = 1000
        End If

        WriteToEventLog(strLog, eventID, 1, EventLogEntryType.Information)
    End Sub

    Private Sub WarningLog_Menu(strMenu As String, deviceName As String, machineCode As String, strCommand As String, Message As String)
        Dim strLog As String
        strLog = "ACTION : " & strMenu & vbCrLf
        strLog += "USER : " & UserLogon_.UserName & vbCrLf
        strLog += "OBJECT NAME : " & objName & vbCrLf
        strLog += "DEVICE NAME : " & deviceName & vbCrLf
        strLog += "MACHINE CODE : " & machineCode & vbCrLf
        strLog += "COMMAND : " & strCommand & vbCrLf
        strLog += "MESSAGE : " & Message

        Dim eventID As Long = 0
        If InStr(strCommand, "Call_EXE") > 0 Then
            eventID = 3000
        Else
            eventID = 1000
        End If

        WriteToEventLog(strLog, eventID, 1, EventLogEntryType.Warning)
    End Sub

    Private Sub ErrorLog_Menu(strMenu As String, deviceName As String, machineCode As String, strCommand As String, Message As String)
        Dim strLog As String
        strLog = "ACTION : " & strMenu & vbCrLf
        strLog += "USER : " & UserLogon_.UserName & vbCrLf
        strLog += "OBJECT NAME : " & objName & vbCrLf
        strLog += "DEVICE NAME : " & deviceName & vbCrLf
        strLog += "MACHINE CODE : " & machineCode & vbCrLf
        strLog += "COMMAND : " & strCommand & vbCrLf
        strLog += "MESSAGE : " & Message

        Dim eventID As Long = 0
        If InStr(strCommand, "Call_EXE") > 0 Then
            eventID = 3000
        Else
            eventID = 1000
        End If

        WriteToEventLog(strLog, eventID, 1, EventLogEntryType.Error)
    End Sub
#End Region



End Module
