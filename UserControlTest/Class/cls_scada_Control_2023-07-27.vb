
Imports TAT_CtrlAlarm.ctrlScale_
Imports TAT_CtrlAlarm.ctrlSurgeBin_
Imports TAT_CtrlAlarm.ctrlMixer_
Imports TAT_CtrlAlarm.ctrlLiquid_
Imports TAT_CtrlAlarm.ctrlHandAdd_
Imports System.Text
Imports System.Net.Sockets
Imports System.Net
Imports System.IO
Imports uPLibrary.Networking.M2Mqtt
Imports uPLibrary.Networking.M2Mqtt.Messages
Imports Newtonsoft.Json

Public Class cls_scada_Control
    '   ====================================================
    '   ======================  ตัวแปร   ========================
    '   ====================================================
#Region "variable"
    '   ================================ NEw Class
    Public cn_Batching As New clsDB("BATCHING")
    Public cn_Route As New clsDB("AUTO_ROUTE")
    Public cn_Premix As New clsDB("PREMIX")
    Dim LogError As New clsIO
    '   ================================ String  Boolen Oher
    Dim sql As String
    Public IpAddressMqtt As String
    Public MqttUser As String
    Public MqttPass As String
    Dim Form_Scada As Form
    '   ================================  CON PLC
    Public MX As New cls_MXCompronent
    '   ================================ LINE    
    Public Line_(2000) As Control
    Public Arrlay As Integer

    Dim ANALOG_PORT
    Dim Str As String
    Dim NAME_ As String
#End Region
    Dim DTMenu As DataTable
    Dim PLC_NO As String
    Public Node As Int16
    Public cStatusTCP As String
    Public iStatusMqtt As Boolean
    Public iStatusPLC As Boolean
    Public statusRemote As Boolean '============= ADD BY APICHAT

    Dim remanescale As String
    Dim client As MqttClient
    Dim clientBin As MqttClient
    Dim clientDevice As MqttClient
    Dim clientMotor As MqttClient
    Dim clientAlarm As MqttClient
    Dim clientRoute As MqttClient

    Public Structure mqtt_analog_status
        Dim ANALOG() As strAnalogData
        'Dim ANALOG2() As strAnalogData
    End Structure
    Public Structure mqtt_analog2_status
        Dim ANALOG() As strAnalogData
    End Structure
    Structure strAnalogData
        Dim NAME As String
        Dim DATA As Int16
    End Structure

    Dim mqtt_analog_status_ As New mqtt_analog_status
    Dim mqtt_analog2_status_ As New mqtt_analog_status
    Dim StrStatus As String
    Public MqttAnalogData(80000) As Int16
    Public MqttAnalogData2(80000) As Int16
    Public Mqtt_M_Data(40000) As Boolean

    Public Structure mqtt_m_status
        Dim DATE_TIME As String
        Dim M() As strMData
    End Structure
    Structure strMData
        Dim NAME As String
        Dim DATA As Boolean
    End Structure

    Dim mqtt_m_status_ As New mqtt_m_status

    Public ctrlPID_Changing As Boolean
    Public ctrlPID_Delay As Integer = 0
    Public Sub New(ByVal From_scada As Form)
        Try
            Dim reader As New System.Configuration.AppSettingsReader
            PLC_NO = reader.GetValue("CCB", GetType(Int16))
            IpAddressMqtt = reader.GetValue("Server_IP", GetType(String))
            MqttUser = reader.GetValue("MqttUser", GetType(String))
            MqttPass = reader.GetValue("MqttPass", GetType(String))
            Form_Scada = From_scada
            Form_Scada.Size = New Size(1915, 950) ' SET SIZE
            '// CONNECT PLC
            Connect_PLC(PLC_NO)
            Dim Data(1) As Int16
            If MX.PLC_Con(1) = True Then
                MX.mxReadBlock2(MX.MxCom1, "R65110", 1, Data)
                Node = Data(0)
                iStatusPLC = True
            Else
                cStatusTCP = "Can't connect open port PLC"
                iStatusPLC = False
            End If
            '============================== Edit By APICHAT
            ReDim mqtt_analog_status_.ANALOG(80000)
            ReDim MqttAnalogData(80000)
            ReDim mqtt_analog2_status_.ANALOG(80000)
            ReDim MqttAnalogData2(80000)
            ReDim StatusReadPort_.strPort(10)
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/New]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace

            LogError.writeErr(strMessage)
        End Try
    End Sub

    '//// for log
    Dim Check_log As New clsIO

    '=============================== MQTT BY NATTAPONG
    Public Sub Connect_MQTT_Scada()
        Try
            client = New MqttClient(IpAddressMqtt)
            Dim clientId As String = Guid.NewGuid().ToString()
            AddHandler client.MqttMsgPublishReceived, AddressOf Client_MqttPublishReceived_Scada
            client.Connect(clientId, MqttUser, MqttPass)
            If (client IsNot Nothing AndAlso client.IsConnected()) Then
                Dim TopicScada() As String = {"CCB" & PLC_NO & "/#"}
                Dim QosScada() As Byte = {0}
                Dim TopicAnalog() As String = {"CCB" & PLC_NO & "/ANALOG/#"}
                Dim QosAnalog() As Byte = {0}
                client.Subscribe(TopicScada, QosScada)
                client.Subscribe(TopicAnalog, QosAnalog)
                iStatusMqtt = True
                tmpCheckConnectMqtt = True
            Else
                iStatusMqtt = False
                tmpCheckConnectMqtt = False
            End If

            If clientBin IsNot Nothing Then
                If clientBin.IsConnected Then clientBin.Disconnect()
            End If
            clientBin = New MqttClient(IpAddressMqtt)
            Dim clientId2 As String = Guid.NewGuid().ToString()
            clientBin.Connect(clientId2, MqttUser, MqttPass)

            If clientDevice IsNot Nothing Then
                If clientDevice.IsConnected Then clientDevice.Disconnect()
            End If
            clientDevice = New MqttClient(IpAddressMqtt)
            Dim clientId3 As String = Guid.NewGuid().ToString()
            clientDevice.Connect(clientId3, MqttUser, MqttPass)

            If clientMotor IsNot Nothing Then
                If clientMotor.IsConnected Then clientMotor.Disconnect()
            End If
            clientMotor = New MqttClient(IpAddressMqtt)
            Dim clientId4 As String = Guid.NewGuid().ToString()
            clientMotor.Connect(clientId4, MqttUser, MqttPass)

            If clientAlarm IsNot Nothing Then
                If clientAlarm.IsConnected Then clientAlarm.Disconnect()
            End If
            clientAlarm = New MqttClient(IpAddressMqtt)
            Dim clientId5 As String = Guid.NewGuid().ToString()
            clientAlarm.Connect(clientId5, MqttUser, MqttPass)

            If clientRoute IsNot Nothing Then
                If clientRoute.IsConnected Then clientRoute.Disconnect()
            End If
            clientRoute = New MqttClient(IpAddressMqtt)
            Dim clientId6 As String = Guid.NewGuid().ToString()
            clientRoute.Connect(clientId6, MqttUser, MqttPass)

        Catch ex As Exception
            iStatusMqtt = False
            If client.IsConnected = False Then Exit Sub
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/Connect_MQTT_Scada]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub Client_MqttPublishReceived_Scada(ByVal sender As Object, ByVal e As MqttMsgPublishEventArgs)
        Try
            If (client IsNot Nothing AndAlso client.IsConnected()) Then
                If e.Topic.ToString() = "CCB" & PLC_NO & "/ANALOG/ANALOG_1" Then
                    StrStatus = Encoding.Default.GetString(e.Message)
                    Dim TmpStatus = JsonConvert.DeserializeObject(Of mqtt_analog_status)(StrStatus)
                    With mqtt_analog_status_
                        For j As Int32 = 0 To 1017
                            If TmpStatus.ANALOG(j).NAME IsNot Nothing Then .ANALOG(j).NAME = TmpStatus.ANALOG(j).NAME '========== EDIT BY APICHAT
                            .ANALOG(28010 + j).DATA = TmpStatus.ANALOG(j).DATA
                            MqttAnalogData(28010 + j) = TmpStatus.ANALOG(j).DATA
                        Next
                    End With
                ElseIf e.Topic.ToString() = "CCB" & PLC_NO & "/ANALOG/ANALOG_2" Then
                    StrStatus = Encoding.Default.GetString(e.Message)
                    Dim TmpStatus = JsonConvert.DeserializeObject(Of mqtt_analog2_status)(StrStatus)
                    With mqtt_analog2_status_
                        For j As Int32 = 0 To 6000
                            If TmpStatus.ANALOG(j).NAME IsNot Nothing Then .ANALOG(j).NAME = TmpStatus.ANALOG(j).NAME '========== EDIT BY APICHAT
                            .ANALOG(64600 + j).DATA = TmpStatus.ANALOG(j).DATA
                            MqttAnalogData2(64600 + j) = TmpStatus.ANALOG(j).DATA
                        Next
                    End With
                ElseIf e.Topic.ToString() = "CCB" & PLC_NO & "/SCADA/M" Then
                    StrStatus = Encoding.Default.GetString(e.Message)
                    Dim TmpStatus = JsonConvert.DeserializeObject(Of mqtt_m_status)(StrStatus)
                    mqtt_m_status_ = TmpStatus
                    For n As Int32 = 4000 To UBound(mqtt_m_status_.M)
                        Mqtt_M_Data(n) = mqtt_m_status_.M(n).DATA
                    Next
                ElseIf e.Topic.ToString() = "CCB" & PLC_NO & "/SCADA/REMOTE/STATUS/REMOTE" Then
                    RemoteControl = Encoding.Default.GetString(e.Message)
                    If UCase(RemoteControl) = "REMOTE CONTROL" Then '=========== ADD BY APICHAT
                        statusRemote = True
                    Else
                        statusRemote = False
                    End If
                ElseIf e.Topic.ToString() = "CCB" & PLC_NO & "/STATUS_LINE/UDP" Then
                    StrStatus = Encoding.Default.GetString(e.Message)
                    'Dim TmpStatus = JsonConvert.DeserializeObject(StrStatus)
                    StatusReadPort_ = JsonConvert.DeserializeObject(Of StatusReadPort)(StrStatus)
                End If
            Else
                iStatusMqtt = False
            End If
        Catch ex As Exception
            iStatusMqtt = False
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/Client_MqttPublishReceived_Scada]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    '=============================== END MQTT BY NATTAPONG
    '   // CONNECT PLC
    Public Sub Connect_PLC(Number_PLC As Int16)
        Try
            MX.MxCom1 = frmMxComponent.AxActUtlType1
            Call MX.mxSetLogicalNumber(MX.MxCom1, Number_PLC)
            If MX.PLC_Con(1) = False Then
                MX.PLC_Con(1) = MX.ReMX(MX.MxCom1, Number_PLC)
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/Connect_PLC]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    '   ==================================================================
    '   ==================  Get DATA From Database ================
    '   ==================================================================
    Public Sub Get_Property(From_scada As Form)
        Try
            Get_Database_For_Mqtt_Ctrl() '========== ADD BY APICHAT
            For Each ctrl As Control In From_scada.Controls   '   // LOOP GET  PROPERTY  
                NAME_ = ""
                If TypeOf (ctrl) Is GroupBox Or TypeOf (ctrl) Is Panel Then  '=============== ADD BY APICHAT
                    Call Get_PropertyForGroupBox(ctrl)
                    '//=== SCALE nattapong MQTT
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlScale_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScale_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScale_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScale_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScale_).SubMqtt(clientAlarm)
                    '//=== MIXER
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlMixer_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlMixer_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlMixer_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlMixer_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlMixer_).SubMqtt(clientAlarm)
                    '//=== SURGE BIN
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlSurgeBin_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlSurgeBin_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlSurgeBin_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlSurgeBin_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlSurgeBin_).SubMqtt(clientAlarm)
                    '//=== LIQUID SCALE
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlScaleLq_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScaleLq_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScaleLq_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScaleLq_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScaleLq_).SubMqtt(clientAlarm)
                    '//=== LIQUID
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlLiquid_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLiquid_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLiquid_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLiquid_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLiquid_).SubMqtt(clientAlarm)
                    '//=== HANDADD
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlHandAdd_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlHandAdd_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlHandAdd_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlHandAdd_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlHandAdd_).SubMqtt(clientAlarm)
                    '//=== ANALOG
                ElseIf TypeOf ctrl Is ctrlAnalog.R Then
                    DirectCast(ctrl, ctrlAnalog.R).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, ctrlAnalog.R).UserName = MqttUser
                    DirectCast(ctrl, ctrlAnalog.R).Password = MqttPass
                    'DirectCast(ctrl, ctrlAnalog.R).SubMqtt()
                    AddHandler CType(ctrl, ctrlAnalog.R).R_DoubleClick_, AddressOf R_MouseDoubleClick
                    '//=== ROUTE
                ElseIf TypeOf ctrl Is TAT_CtrlRoute.ctrlRouteConveyer_ Then
                    DirectCast(ctrl, TAT_CtrlRoute.ctrlRouteConveyer_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlRoute.ctrlRouteConveyer_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlRoute.ctrlRouteConveyer_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlRoute.ctrlRouteConveyer_).SubMqtt(client)
                    AddHandler CType(ctrl, TAT_CtrlRoute.ctrlRouteConveyer_).tsmSetCleanTime_MouseUp_, AddressOf tsmSetCleanTime_MouseUp
                    AddHandler CType(ctrl, TAT_CtrlRoute.ctrlRouteConveyer_).AKN_MouseUp_, AddressOf AKN_MouseUp
                    AddHandler CType(ctrl, TAT_CtrlRoute.ctrlRouteConveyer_).COM_MouseUp_, AddressOf COMP_MouseUp
                    '//=== JOG
                ElseIf TypeOf ctrl Is TAT_CtrlRoute.ctrlJogRoute_ Then
                    DirectCast(ctrl, TAT_CtrlRoute.ctrlJogRoute_).SubMqtt(client)
                    AddHandler CType(ctrl, TAT_CtrlRoute.ctrlJogRoute_).tsmSetCleanTime_MouseUp_, AddressOf tsmSetJogTime_MouseUp
                    AddHandler CType(ctrl, TAT_CtrlRoute.ctrlJogRoute_).JOG_MouseUp_, AddressOf JOG_MouseUp
                    '==================================== add 2023-07-27 by nattapong
                    '//=== AUTO CHANGE
                ElseIf TypeOf ctrl Is TAT_CtrlRoute.ctrlAutoChage_ Then
                    DirectCast(ctrl, TAT_CtrlRoute.ctrlAutoChage_).SubMqtt(client)
                    AddHandler CType(ctrl, TAT_CtrlRoute.ctrlAutoChage_).tsmSetJogTime_MouseUp_, AddressOf tsmSetJogTime_MouseUp
                    AddHandler CType(ctrl, TAT_CtrlRoute.ctrlAutoChage_).tsmSetLowTime_MouseUp_, AddressOf tsmSetLowTime_MouseUp
                    AddHandler CType(ctrl, TAT_CtrlRoute.ctrlAutoChage_).tsmSetHightTime_MouseUp_, AddressOf tsmSetHightTime_MouseUp
                    AddHandler CType(ctrl, TAT_CtrlRoute.ctrlAutoChage_).tsmSetWeight_MouseUp_, AddressOf tsmSetWeight_MouseUp
                    '//=== Bin
                ElseIf TypeOf ctrl Is TAT_CtrlBin.ctrlBin_ Then
                    DirectCast(ctrl, TAT_CtrlBin.ctrlBin_).SubMqtt(clientBin)
                    '//=== Liquid
                ElseIf TypeOf ctrl Is TAT_CtrlBin.ctrlLq_ Then
                    DirectCast(ctrl, TAT_CtrlBin.ctrlLq_).SubMqtt(clientBin)
                    '//=== SELECTOR MQTT
                ElseIf TypeOf ctrl Is TAT_CtrlSelector.ctrlSelectorMode_ Then
                    DirectCast(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).IpAddress_ = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).UserName_ = MqttUser
                    DirectCast(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).Password_ = MqttPass
                    DirectCast(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).SubMqtt(client)
                    AddHandler CType(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).AUTO_MouseUp_, AddressOf AUTO_MouseUp
                    AddHandler CType(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).MANUAL_MouseUp_, AddressOf MANUAL_MouseUp
                    AddHandler CType(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).HOLD_MouseUp_, AddressOf HOLD_MouseUp
                ElseIf TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
                    DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).SubMqtt(clientMotor)
                    Dim tmpCtrl As TAT_MQTT_CTRL.ctrlTAT_
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.CtrlSelected, AddressOf Motor_MouseDown
                ElseIf TypeOf ctrl Is MQTT_CTRL_OTHERDEVICE.ctrlDevice Then
                    DirectCast(ctrl, MQTT_CTRL_OTHERDEVICE.ctrlDevice).SubMqtt(clientDevice)
                    Dim tmpCtrl As MQTT_CTRL_OTHERDEVICE.ctrlDevice
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.CtrlSelected, AddressOf OtherDevice_MouseDown
                ElseIf TypeOf ctrl Is TAT_UTILITY_CTRL.lblAnalog_ Then
                    Dim tmpCtrl As TAT_UTILITY_CTRL.lblAnalog_
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.MouseDown, AddressOf CtrlAnalog_MouseDown
                ElseIf TypeOf ctrl Is TAT_UTILITY_CTRL.ctrlPID_ Then
                    Dim tmpCtrl As TAT_UTILITY_CTRL.ctrlPID_ = ctrl
                    AddHandler tmpCtrl.OnPointerChange, AddressOf CtrlPID_OnPointerChange
                    AddHandler tmpCtrl.PointerValue_Changed, AddressOf CtrlPID_PointerValue_Changed
                ElseIf TypeOf ctrl Is TAT_UTILITY_CTRL.ctrlMV_ Then
                    Dim tmpCtrl As TAT_UTILITY_CTRL.ctrlMV_ = ctrl
                    AddHandler tmpCtrl.OnPointerChange, AddressOf CtrlPID_OnPointerChange
                    AddHandler tmpCtrl.PointerValue_Changed, AddressOf CtrlPID_PointerValue_Changed
                    AddHandler tmpCtrl.MouseDown, AddressOf CtrlAnalog_MouseDown
                ElseIf TypeOf ctrl Is Line.Line Then
                    Arrlay = CInt(ctrl.Name.Replace("Line", ""))
                    Line_(Arrlay) = ctrl
                End If
            Next
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/Get_Property]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace

            LogError.writeErr(strMessage)
        End Try
    End Sub

    '============================================ CHECK CONTROL IN GROUPBOX BY APICHAT
    Public Sub Get_PropertyForGroupBox(GBctrl As Control)
        Try
            For Each Dctrl As Control In GBctrl.Controls
                '//=== SCALE nattapong MQTT
                If TypeOf Dctrl Is TAT_CtrlAlarm.ctrlScale_ Then
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlScale_).IpAddress = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlScale_).UserName = MqttUser
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlScale_).Password = MqttPass
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlScale_).SubMqtt(client)
                    '//=== MIXER
                ElseIf TypeOf Dctrl Is TAT_CtrlAlarm.ctrlMixer_ Then
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlMixer_).IpAddress = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlMixer_).UserName = MqttUser
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlMixer_).Password = MqttPass
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlMixer_).SubMqtt(client)
                    '//=== SURGE BIN
                ElseIf TypeOf Dctrl Is TAT_CtrlAlarm.ctrlSurgeBin_ Then
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlSurgeBin_).IpAddress = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlSurgeBin_).UserName = MqttUser
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlSurgeBin_).Password = MqttPass
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlSurgeBin_).SubMqtt(client)
                    '//=== LIQUID SCALE
                ElseIf TypeOf Dctrl Is TAT_CtrlAlarm.ctrlScaleLq_ Then
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlScaleLq_).IpAddress = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlScaleLq_).UserName = MqttUser
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlScaleLq_).Password = MqttPass
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlScaleLq_).SubMqtt(client)
                    '//=== LIQUID
                ElseIf TypeOf Dctrl Is TAT_CtrlAlarm.ctrlLiquid_ Then
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlLiquid_).IpAddress = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlLiquid_).UserName = MqttUser
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlLiquid_).Password = MqttPass
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlLiquid_).SubMqtt(client)
                    '//=== HANDADD
                ElseIf TypeOf Dctrl Is TAT_CtrlAlarm.ctrlHandAdd_ Then
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlHandAdd_).IpAddress = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlHandAdd_).UserName = MqttUser
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlHandAdd_).Password = MqttPass
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlHandAdd_).SubMqtt(client)
                    '//=== ANALOG
                ElseIf TypeOf Dctrl Is ctrlAnalog.R Then
                    DirectCast(Dctrl, ctrlAnalog.R).IpAddress = IpAddressMqtt
                    DirectCast(Dctrl, ctrlAnalog.R).UserName = MqttUser
                    DirectCast(Dctrl, ctrlAnalog.R).Password = MqttPass
                    'DirectCast(Dctrl, ctrlAnalog.R).SubMqtt()
                    AddHandler CType(Dctrl, ctrlAnalog.R).R_DoubleClick_, AddressOf R_MouseDoubleClick
                    '//=== ROUTE
                ElseIf TypeOf Dctrl Is TAT_CtrlRoute.ctrlRouteConveyer_ Then
                    DirectCast(Dctrl, TAT_CtrlRoute.ctrlRouteConveyer_).IpAddress = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlRoute.ctrlRouteConveyer_).UserName = MqttUser
                    DirectCast(Dctrl, TAT_CtrlRoute.ctrlRouteConveyer_).Password = MqttPass
                    DirectCast(Dctrl, TAT_CtrlRoute.ctrlRouteConveyer_).SubMqtt(client)
                    AddHandler CType(Dctrl, TAT_CtrlRoute.ctrlRouteConveyer_).tsmSetCleanTime_MouseUp_, AddressOf tsmSetCleanTime_MouseUp
                    AddHandler CType(Dctrl, TAT_CtrlRoute.ctrlRouteConveyer_).AKN_MouseUp_, AddressOf AKN_MouseUp
                    AddHandler CType(Dctrl, TAT_CtrlRoute.ctrlRouteConveyer_).COM_MouseUp_, AddressOf COMP_MouseUp
                    '//=== JOG
                ElseIf TypeOf Dctrl Is TAT_CtrlRoute.ctrlJogRoute_ Then
                    DirectCast(Dctrl, TAT_CtrlRoute.ctrlJogRoute_).SubMqtt(client)
                    AddHandler CType(Dctrl, TAT_CtrlRoute.ctrlJogRoute_).tsmSetCleanTime_MouseUp_, AddressOf tsmSetJogTime_MouseUp
                    AddHandler CType(Dctrl, TAT_CtrlRoute.ctrlJogRoute_).JOG_MouseUp_, AddressOf JOG_MouseUp
                    '==================================== add 2023-07-27 by nattapong
                    '//=== AUTO CHANGE
                ElseIf TypeOf Dctrl Is TAT_CtrlRoute.ctrlAutoChage_ Then
                    DirectCast(Dctrl, TAT_CtrlRoute.ctrlAutoChage_).SubMqtt(client)
                    AddHandler CType(Dctrl, TAT_CtrlRoute.ctrlAutoChage_).tsmSetJogTime_MouseUp_, AddressOf tsmSetJogTime_MouseUp
                    AddHandler CType(Dctrl, TAT_CtrlRoute.ctrlAutoChage_).tsmSetLowTime_MouseUp_, AddressOf tsmSetLowTime_MouseUp
                    AddHandler CType(Dctrl, TAT_CtrlRoute.ctrlAutoChage_).tsmSetHightTime_MouseUp_, AddressOf tsmSetHightTime_MouseUp
                    AddHandler CType(Dctrl, TAT_CtrlRoute.ctrlAutoChage_).tsmSetWeight_MouseUp_, AddressOf tsmSetWeight_MouseUp
                    '//=== Bin
                ElseIf TypeOf Dctrl Is TAT_CtrlBin.ctrlBin_ Then
                    DirectCast(Dctrl, TAT_CtrlBin.ctrlBin_).SubMqtt(clientBin)
                    '//=== Liquid
                ElseIf TypeOf Dctrl Is TAT_CtrlBin.ctrlLq_ Then
                    DirectCast(Dctrl, TAT_CtrlBin.ctrlLq_).SubMqtt(clientBin)
                    '//=== SELECTOR MQTT
                ElseIf TypeOf Dctrl Is TAT_CtrlSelector.ctrlSelectorMode_ Then
                    DirectCast(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).SubMqtt(client)
                    DirectCast(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).IpAddress_ = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).UserName_ = MqttUser
                    DirectCast(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).Password_ = MqttPass
                    AddHandler CType(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).AUTO_MouseUp_, AddressOf AUTO_MouseUp
                    AddHandler CType(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).MANUAL_MouseUp_, AddressOf MANUAL_MouseUp
                    AddHandler CType(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).HOLD_MouseUp_, AddressOf HOLD_MouseUp
                ElseIf TypeOf Dctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
                    DirectCast(Dctrl, TAT_MQTT_CTRL.ctrlTAT_).SubMqtt(clientMotor)
                    Dim tmpCtrl As TAT_MQTT_CTRL.ctrlTAT_
                    tmpCtrl = Dctrl
                    AddHandler tmpCtrl.CtrlSelected, AddressOf Motor_MouseDown
                ElseIf TypeOf Dctrl Is MQTT_CTRL_OTHERDEVICE.ctrlDevice Then
                    DirectCast(Dctrl, MQTT_CTRL_OTHERDEVICE.ctrlDevice).SubMqtt(clientDevice)
                    Dim tmpCtrl As MQTT_CTRL_OTHERDEVICE.ctrlDevice
                    tmpCtrl = Dctrl
                    AddHandler tmpCtrl.CtrlSelected, AddressOf OtherDevice_MouseDown
                ElseIf TypeOf Dctrl Is TAT_UTILITY_CTRL.lblAnalog_ Then
                    Dim tmpCtrl As TAT_UTILITY_CTRL.lblAnalog_
                    tmpCtrl = Dctrl
                    AddHandler tmpCtrl.MouseDown, AddressOf CtrlAnalog_MouseDown
                ElseIf TypeOf Dctrl Is TAT_UTILITY_CTRL.ctrlPID_ Then
                    Dim tmpCtrl As TAT_UTILITY_CTRL.ctrlPID_ = Dctrl
                    AddHandler tmpCtrl.OnPointerChange, AddressOf CtrlPID_OnPointerChange
                    AddHandler tmpCtrl.PointerValue_Changed, AddressOf CtrlPID_PointerValue_Changed
                ElseIf TypeOf Dctrl Is TAT_UTILITY_CTRL.ctrlMV_ Then
                    Dim tmpCtrl As TAT_UTILITY_CTRL.ctrlMV_ = Dctrl
                    AddHandler tmpCtrl.OnPointerChange, AddressOf CtrlPID_OnPointerChange
                    AddHandler tmpCtrl.PointerValue_Changed, AddressOf CtrlPID_PointerValue_Changed
                ElseIf TypeOf Dctrl Is Line.Line Then
                    Arrlay = CInt(Dctrl.Name.Replace("Line", ""))
                    Line_(Arrlay) = Dctrl
                End If
            Next
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/Get_PropertyForGroupBox]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace

            LogError.writeErr(strMessage)
        End Try
    End Sub


#Region "event_center menu click right"

    Private Sub Motor_MouseDown(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                If MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng") = vbYes Then
                    frm_login.Show()
                    frm_login.TopMost = True
                End If
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Call Show_Menu(sender, New Point(e.X, e.Y), statusRemote)
            ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
                If StrMqtt_Config_.RouteConfig Then
                    Dim menu As New clsRoute
                    menu.ShowMenu_(sender, New Point(e.X, e.Y), "", Form_Scada, PLC_NO)
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/Motor_MouseDown]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub


    Private Sub OtherDevice_MouseDown(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Call Show_Menu(sender, New Point(e.X, e.Y), statusRemote)
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/OtherDevice_MouseDown]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace

            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub CtrlAnalog_MouseDown(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Call Show_AnalogMenu(sender, New Point(e.X, e.Y))
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/CtrlAnalog_MouseDown]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace

            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub CtrlPID_OnPointerChange(sender As Object, IdxPointer As Integer)
        ctrlPID_Changing = True
        ctrlPID_Delay = 2
    End Sub
    Private Sub CtrlPID_PointerValue_Changed(sender As Object, pointerName As String, ptValue As Integer)

        If MDI_FRM.Main_Scada = False Then
            MsgBox("Pls Logon scada", vbOKOnly + MsgBoxStyle.Information, "Worng")
            ctrlPID_Changing = False
            frm_login.Show()
            frm_login.TopMost = True
            Exit Sub
        End If

        Call mnuSetPID_Pointer(sender, pointerName, ptValue)
        ctrlPID_Changing = False
    End Sub



    '   #################################################################################################### R NATTAPONG
    Private Sub R_MouseDoubleClick(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If sender.SetValue_ = True Then
                    If sender.WORD_ = True Then
                        Dim send_long(1) As Int32
                        Dim DataInput As Double
                        DataInput = InputBox(sender.Tag, "Input Numeric Valus")
                        send_long(0) = DataInput * sender.Divide_
                        MX.mxWriteBlockLong(MX.MxCom1, sender.Name, 1, send_long)
                    Else
                        Dim DATA1 As Double
                        Dim DATA(1) As Int16
                        DATA1 = InputBox(sender.Tag, "Input Numeric Valus")
                        DATA(0) = DATA1 * sender.Divide_
                        MX.mxWriteBlock(MX.MxCom1, sender.Name, 1, DATA)
                    End If
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/R_MouseDoubleClick]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace

            LogError.writeErr(strMessage)
        End Try
    End Sub

    '   #################################################################################################### RouteConveyer NATTAPONG
    Private Sub AKN_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If sender.Mqtt_selectroute_status_.STA_REQ = True Then
                    If sender.Mqtt_selectroute_status_.STA_ACK = True Then
                        MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectroute_config_.M_ACK, 0)
                    Else
                        MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectroute_config_.M_ACK, 1)
                    End If
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/AKN_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace

            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub JOG_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If sender.Mqtt_selectroute_status_.STA_JOG = True Then
                    MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectroute_config_.M_JOG, 0)
                Else
                    MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectroute_config_.M_JOG, 1)
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/JOG_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace

            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub COMP_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If sender.Mqtt_selectroute_status_.STA_REQ = True Then
                    MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectroute_config_.M_COM, 1)
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/COMP_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
            'LOG.writeLogEvent(Application.StartupPath, "::" + ex.Message + "::" + sender.ToString + ":: In Function [COMP_MouseUp]")
        End Try
    End Sub

    Private Sub tsmSetCleanTime_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                Dim i_clean_time As Int16
                i_clean_time = InputBox("Please Input Clean Time", "Input Clean Time")
                Call Func_CleanlineRoute(sender.mqtt_selectroute_config_.ZR_ACT_CLEAN_TIME, i_clean_time)
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/tsmSetCleanTime_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
            'Log.writeLogEvent(Application.StartupPath, "::" + ex.Message + "::" + sender.ToString + ":: In Function [tsmSetCleanTime_MouseUp]")
        End Try

    End Sub

    Private Sub tsmSetJogTime_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                Dim i_clean_time As Int16
                i_clean_time = InputBox("Please Input " & sender.Tag & " Jog Time", "Input Jog Time")
                If sender.Tag = "Target" Then
                    Call Func_CleanlineRoute(sender.mqtt_selectroute_config_.R_TAR_JOG, i_clean_time)
                ElseIf sender.Tag = "Actual" Then
                    Call Func_CleanlineRoute(sender.mqtt_selectroute_config_.R_ACT_JOG, i_clean_time)
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/tsmSetJogTime_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
            'LOG.writeLogEvent(Application.StartupPath, "::" + ex.Message + "::" + sender.ToString + ":: In Function [tsmSetCleanTime_MouseUp]")
        End Try
    End Sub

    Function Func_CleanlineRoute(i_value As String, clean_time As Int16) As Int16
        Try
            Dim ret As Long
            Dim send_int(0) As Int16
            Try
                send_int(0) = clean_time * 10
                ret = MX.mxWriteBlock(MX.MxCom1, i_value, 1, send_int)
                Return ret
            Catch ex As Exception
                Return -1
            End Try
        Catch ex As Exception
            Return -1
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/Func_CleanlineRoute]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Function
    '==================================== add 2023-07-27 by nattapong
    Private Sub tsmSetLowTime_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                Dim i_clean_time As Int16
                i_clean_time = InputBox("Please Input " & sender.Tag & " Low Time", "Input Low Time")
                If sender.Tag = "Target" Then
                    Call Func_CleanlineRoute(sender.mqtt_selectroute_config_.ZR_TAR_LOW_TIME, i_clean_time)
                ElseIf sender.Tag = "Actual" Then
                    Call Func_CleanlineRoute(sender.mqtt_selectroute_config_.ZR_ACT_LOW_TIME, i_clean_time)
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/tsmSetLowTime_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
            'LOG.writeLogEvent(Application.StartupPath, "::" + ex.Message + "::" + sender.ToString + ":: In Function [tsmSetCleanTime_MouseUp]")
        End Try
    End Sub

    Private Sub tsmSetHightTime_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                Dim i_clean_time As Int16
                i_clean_time = InputBox("Please Input " & sender.Tag & " Low Time", "Input Low Time")
                If sender.Tag = "Target" Then
                    Call Func_CleanlineRoute(sender.mqtt_selectroute_config_.ZR_TAR_HIGH_TIME, i_clean_time)
                ElseIf sender.Tag = "Actual" Then
                    Call Func_CleanlineRoute(sender.mqtt_selectroute_config_.ZR_ACT_HIGH_TIME, i_clean_time)
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/tsmSetHightTime_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
            'LOG.writeLogEvent(Application.StartupPath, "::" + ex.Message + "::" + sender.ToString + ":: In Function [tsmSetCleanTime_MouseUp]")
        End Try
    End Sub

    Private Sub tsmSetWeight_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                Dim i_clean_time As Int16
                i_clean_time = InputBox("Please Input " & sender.Tag & " Weight (Kg)", "Input Weight")
                If sender.Tag = "Target" Then
                    Call Func_SetWeightRoute(sender.mqtt_selectroute_config_.ZR_TAR_WEIGHT, i_clean_time)
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/tsmSetWeight_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
            'LOG.writeLogEvent(Application.StartupPath, "::" + ex.Message + "::" + sender.ToString + ":: In Function [tsmSetCleanTime_MouseUp]")
        End Try
    End Sub

    Function Func_SetWeightRoute(i_value As String, iWeight As Int16) As Int16
        Try
            Dim ret As Long
            Dim send_int(1) As Int16
            Dim nWeight(0) As Int32
            Try
                nWeight(0) = iWeight * 1000
                System.Buffer.BlockCopy(nWeight, 0, send_int, 0 * 2, 8)
                ret = MX.mxWriteBlock(MX.MxCom1, i_value, 2, send_int)
                Return ret
            Catch ex As Exception
                Return -1
            End Try
        Catch ex As Exception
            Return -1
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/Func_SetWeightRoute]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Function

    '   #################################################################################################### Selector NATTAPONG
    Private Sub AUTO_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectmode_config_.M_AUTO, 1)
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/AUTO_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
            'LOG.writeLogEvent(Application.StartupPath, "::" + ex.Message + "::" + sender.ToString + ":: In Function [AUTO_MouseUp]")
        End Try
    End Sub
    Private Sub MANUAL_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectmode_config_.M_AUTO, 0)
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/MANUAL_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
            'Log.writeLogEvent(Application.StartupPath, "::" + ex.Message + "::" + sender.ToString + ":: In Function [MANUAL_MouseUp]")
        End Try
    End Sub

    Private Sub HOLD_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            If MDI_FRM.Main_Scada = False Then
                MsgBox("Please Logon scada", vbYesNo + MsgBoxStyle.Information, "Worng")
                frm_login.Show()
                frm_login.TopMost = True
                Exit Sub
            End If
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If sender.mqtt_selectmode_status_.STA_HOLD = True Then
                    MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectmode_config_.M_HOLD, 0)
                Else
                    MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectmode_config_.M_HOLD, 1)
                End If
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/HOLD_MouseUp]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
            'Log.writeLogEvent(Application.StartupPath, "::" + ex.Message + "::" + sender.ToString + ":: In Function [HOLD_MouseUp]")
        End Try
    End Sub
#End Region

    Public Sub ShowGraphic(Mstatus As Boolean, strLine As String)
        Try
            Dim strIndex() As String
            Dim intCountIndex As Integer
            strIndex = strLine.Split(",")
            intCountIndex = UBound(strIndex) + 1
            For I As Integer = 0 To strIndex.Count - 1
                If Mstatus = False Then

                    Line_(strIndex(I)).Hide()
                Else
                    Line_(strIndex(I)).Show()
                End If
            Next
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/ShowGraphic]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub
    Public Sub CheckStatusMqtt()
        Try
            If tmpCountCheck >= 10 Then
                If (client IsNot Nothing AndAlso client.IsConnected()) Then
                    client.Disconnect()
                    tmpCheckConnectMqtt = False
                End If
                'Connect_MQTT_Scada()
            End If
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/CheckStatusMqtt]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub
End Class
