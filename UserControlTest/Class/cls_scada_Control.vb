
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
Imports System.Reflection

Public Class cls_scada_Control
    '   ====================================================
    '   ======================  ตัวแปร   ========================
    '   ====================================================
#Region "variable"
    '   ================================ NEw Class
    'Public cn_Batching As New clsDB("BATCHING", StrMqtt_Config_.DbUser, StrMqtt_Config_.DbPass)
    'Public cn_Route As New clsDB("AUTO_ROUTE", StrMqtt_Config_.DbUser, StrMqtt_Config_.DbPass)
    'Public cn_Premix As New clsDB("PREMIX", StrMqtt_Config_.DbUser, StrMqtt_Config_.DbPass)
    Public cn_Batching As New clsDB(Batching_Conf.Name, Batching_Conf.User, Batching_Conf.Password, Batching_Conf.IPAddress, Batching_Conf.Connection_Type)
    Public cn_Route As New clsDB(Auto_Route_Conf.Name, Auto_Route_Conf.User, Auto_Route_Conf.Password, Auto_Route_Conf.IPAddress, Auto_Route_Conf.Connection_Type)
    Public cn_Premix As New clsDB(Premix_Conf.Name, Premix_Conf.User, Premix_Conf.Password, Premix_Conf.IPAddress, Premix_Conf.Connection_Type)
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

    '================= ADD BY APICHAT FOR CHECK STATUS MQTT
    Dim monitorMqtt_Main As clsMqttHeartbeatMonitor
    Dim monitorMqtt_Bin As clsMqttHeartbeatMonitor
    Dim monitorMqtt_Device As clsMqttHeartbeatMonitor
    Dim monitorMqtt_Motor As clsMqttHeartbeatMonitor
    Dim monitorMqtt_Alarm As clsMqttHeartbeatMonitor
    Dim monitorMqtt_Route As clsMqttHeartbeatMonitor
    Public Event MQTT_ConnectionLost(clientName As String)

    '================= ADD BY APICHAT FOR SHOW LINE
    Dim dtLine As DataTable
    Dim dtConfigLine As DataTable

    Private objName As String '===== For Keep log

    Public Structure mqtt_analog_status
        Dim NAME As String
        Dim DATA As Int16
    End Structure

    Dim mqtt_analog_status_(200000) As mqtt_analog_status
    Dim StrStatus As String
    Public MqttAnalogData(200000) As Int16
    Public Mqtt_M_Data(200000) As Boolean

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
            ReDim MqttAnalogData(120000)
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
                Dim TopicScada() As String = {"CCB" & PLC_NO & "/SCADA/#"}
                Dim TopicAnalog() As String = {"CCB" & PLC_NO & "/ANALOG/#"}
                Dim TopicAlarm() As String = {"CCB" & PLC_NO & "/ALARM/#"}
                Dim TopicStatus() As String = {"CCB" & PLC_NO & "/STATUS_LINE/#"}
                Dim QosScada() As Byte = {0}
                Dim QosAnalog() As Byte = {0}
                Dim QosAlarm() As Byte = {0}
                Dim QosStatus() As Byte = {0}
                client.Subscribe(TopicScada, QosScada)
                client.Subscribe(TopicAnalog, QosAnalog)
                client.Subscribe(TopicAlarm, QosAlarm)
                client.Subscribe(TopicStatus, QosStatus)
                Start_MqttChecker(monitorMqtt_Main, client, "clientMain")
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

    Private Sub Start_MqttChecker(ByRef checker As clsMqttHeartbeatMonitor, ByRef client As MqttClient, ByVal clientName As String)
        If checker Is Nothing Then
            checker = New clsMqttHeartbeatMonitor(client, clientName, Form_Scada.Name)
            AddHandler checker.ConnectionLost, AddressOf OnConnectionLost
        End If
    End Sub

    Private Sub OnConnectionLost(clientName)
        RaiseEvent MQTT_ConnectionLost(clientName)
    End Sub

    Public Function Check_MQTT_Active() As Boolean
        Dim status As New List(Of Boolean)
        If monitorMqtt_Main IsNot Nothing Then
            status.Add(monitorMqtt_Main.StatusActive)
        End If
        If monitorMqtt_Bin IsNot Nothing Then
            status.Add(monitorMqtt_Bin.StatusActive)
        End If
        If monitorMqtt_Device IsNot Nothing Then
            status.Add(monitorMqtt_Device.StatusActive)
        End If
        If monitorMqtt_Motor IsNot Nothing Then
            status.Add(monitorMqtt_Motor.StatusActive)
        End If
        If monitorMqtt_Alarm IsNot Nothing Then
            status.Add(monitorMqtt_Alarm.StatusActive)
        End If
        If monitorMqtt_Route IsNot Nothing Then
            status.Add(monitorMqtt_Route.StatusActive)
        End If
        If status.Count > 0 Then
            For Each st In status
                If st = False Then
                    Return False
                End If
            Next
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub Client_MqttPublishReceived_Scada(ByVal sender As Object, ByVal e As MqttMsgPublishEventArgs)
        Try
            If client IsNot Nothing AndAlso client.IsConnected() Then
                If InStr(e.Topic.ToString(), "CCB" & PLC_NO & "/ANALOG/ANALOG_1") > 0 Then
                    StrStatus = Encoding.Default.GetString(e.Message)
                    Dim TmpStatus = JsonConvert.DeserializeObject(Of mqtt_analog_status)(StrStatus)
                    MqttAnalogData(RegularExpressions.Regex.Replace(TmpStatus.NAME, "[^0-9]", "")) = TmpStatus.DATA
                ElseIf InStr(e.Topic.ToString(), "CCB" & PLC_NO & "/ANALOG/ANALOG_2") > 0 Then
                    StrStatus = Encoding.Default.GetString(e.Message)
                    Dim TmpStatus = JsonConvert.DeserializeObject(Of mqtt_analog_status)(StrStatus)
                    MqttAnalogData(RegularExpressions.Regex.Replace(TmpStatus.NAME, "[^0-9]", "")) = TmpStatus.DATA
                ElseIf InStr(e.Topic.ToString(), "CCB" & PLC_NO & "/ANALOG/ANALOG_3") > 0 Then
                    StrStatus = Encoding.Default.GetString(e.Message)
                    Dim TmpStatus = JsonConvert.DeserializeObject(Of mqtt_analog_status)(StrStatus)
                    MqttAnalogData(RegularExpressions.Regex.Replace(TmpStatus.NAME, "[^0-9]", "")) = TmpStatus.DATA
                ElseIf InStr(e.Topic.ToString(), "CCB" & PLC_NO & "/SCADA/M/") > 0 Then
                    StrStatus = Encoding.Default.GetString(e.Message)
                    Dim TmpStatus = JsonConvert.DeserializeObject(Of strMData)(StrStatus)
                    Mqtt_M_Data(RegularExpressions.Regex.Replace(TmpStatus.NAME, "[^0-9]", "")) = TmpStatus.DATA
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
                ElseIf e.Topic.ToString = "CCB" & PLC_NO & "/STATUS_LINE/STATUS" Then
                    StrStatus = Encoding.Default.GetString(e.Message)
                    statusMQTT = JsonConvert.DeserializeObject(Of MQTT_Status)(StrStatus)
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
            '===================================== GET LINE CONFIG
            Dim tmpAppName As String = Assembly.GetExecutingAssembly().GetName().Name
            dtConfigLine = GetLine_Config(tmpAppName, Form_Scada.Name)

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
                    'Start_MqttChecker(monitorMqtt_Alarm, clientAlarm, "clientAlarm")
                    '//=== SURGE BIN
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlSurgeBin_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlSurgeBin_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlSurgeBin_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlSurgeBin_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlSurgeBin_).SubMqtt(clientAlarm)
                    'Start_MqttChecker(monitorMqtt_Alarm, clientAlarm, "clientAlarm")
                    '//=== LIQUID SCALE
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlScaleLq_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScaleLq_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScaleLq_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScaleLq_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlScaleLq_).SubMqtt(clientAlarm)
                    'Start_MqttChecker(monitorMqtt_Alarm, clientAlarm, "clientAlarm")
                    '//=== LIQUID
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlLiquid_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLiquid_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLiquid_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLiquid_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLiquid_).SubMqtt(clientAlarm)
                    'Start_MqttChecker(monitorMqtt_Alarm, clientAlarm, "clientAlarm")
                    '//=== HANDADD
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlHandAdd_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlHandAdd_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlHandAdd_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlHandAdd_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlHandAdd_).SubMqtt(clientAlarm)
                    'Start_MqttChecker(monitorMqtt_Alarm, clientAlarm, "clientAlarm")
                    '//=== LOAD OUT
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlLoadout_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLoadout_).IpAddress = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLoadout_).UserName = MqttUser
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLoadout_).Password = MqttPass
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlLoadout_).SubMqtt(clientAlarm)
                    'Start_MqttChecker(monitorMqtt_Alarm, clientAlarm, "clientAlarm")
                    '//=== OTHER
                ElseIf TypeOf ctrl Is TAT_CtrlAlarm.ctrlOther_ Then
                    DirectCast(ctrl, TAT_CtrlAlarm.ctrlOther_).SubMqtt(clientAlarm)
                    'Start_MqttChecker(monitorMqtt_Alarm, clientAlarm, "clientAlarm")
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
                    Dim tmpCtrl As TAT_CtrlRoute.ctrlRouteConveyer_
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.ROUTE_MouseUp_, AddressOf CtrlRoute_MouseDown
                    AddHandler tmpCtrl.AKN_MouseUp_, AddressOf AKN_MouseUp
                    AddHandler tmpCtrl.COM_MouseUp_, AddressOf COMP_MouseUp
                    '//=== JOG
                ElseIf TypeOf ctrl Is TAT_CtrlRoute.ctrlJogRoute_ Then
                    DirectCast(ctrl, TAT_CtrlRoute.ctrlJogRoute_).SubMqtt(client)
                    Dim tmpCtrl As TAT_CtrlRoute.ctrlJogRoute_
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.JOG_MouseUp_, AddressOf CtrlJogRoute_MouseDown
                    '==================================== add 2023-07-27 by nattapong
                    '//=== AUTO CHANGE
                ElseIf TypeOf ctrl Is TAT_CtrlRoute.ctrlAutoChage_ Then
                    DirectCast(ctrl, TAT_CtrlRoute.ctrlAutoChage_).SubMqtt(client)
                    Dim tmpCtrl As TAT_CtrlRoute.ctrlAutoChage_
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.SetJogTime_MouseUp_, AddressOf CtrlAutoChange_MouseUp
                    AddHandler tmpCtrl.SetLowTime_MouseUp_, AddressOf CtrlAutoChange_MouseUp
                    AddHandler tmpCtrl.SetHightTime_MouseUp_, AddressOf CtrlAutoChange_MouseUp
                    '//=== Bin
                ElseIf TypeOf ctrl Is TAT_CtrlBin.ctrlBin_ Then
                    DirectCast(ctrl, TAT_CtrlBin.ctrlBin_).SubMqtt(clientBin)
                    Dim tmpCtrl As TAT_CtrlBin.ctrlBin_
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.MouseUp, AddressOf Bin_MouseDown
                    '//=== Liquid
                ElseIf TypeOf ctrl Is TAT_CtrlBin.ctrlLq_ Then
                    DirectCast(ctrl, TAT_CtrlBin.ctrlLq_).SubMqtt(clientBin)
                    Dim tmpCtrl As TAT_CtrlBin.ctrlLq_
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.MouseUp, AddressOf Bin_MouseDown
                    '//=== SELECTOR MQTT
                ElseIf TypeOf ctrl Is TAT_CtrlSelector.ctrlSelectorMode_ Then
                    DirectCast(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).IpAddress_ = IpAddressMqtt
                    DirectCast(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).UserName_ = MqttUser
                    DirectCast(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).Password_ = MqttPass
                    DirectCast(ctrl, TAT_CtrlSelector.ctrlSelectorMode_).SubMqtt(client)
                    Dim tmpCtrl As TAT_CtrlSelector.ctrlSelectorMode_
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.Select_MouseUp_, AddressOf CtrlSelect_MouseDown
                ElseIf TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
                    DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).SubMqtt(clientMotor)
                    'Start_MqttChecker(monitorMqtt_Motor, clientMotor, "clientMotor")
                    Dim tmpCtrl As TAT_MQTT_CTRL.ctrlTAT_
                    tmpCtrl = ctrl
                    AddHandler tmpCtrl.CtrlSelected, AddressOf Motor_MouseDown
                    AddHandler tmpCtrl.OnStatusChanged, AddressOf Ctrl_StatusChanged
                    AddHandler tmpCtrl.OnAlarmMessageChanged, AddressOf ctrl_OnAlarmMessageChanged '====== FOR MULTI LANGUAGE
                ElseIf TypeOf ctrl Is MQTT_CTRL_OTHERDEVICE.ctrlDevice Then
                    DirectCast(ctrl, MQTT_CTRL_OTHERDEVICE.ctrlDevice).SubMqtt(clientDevice)
                    'Start_MqttChecker(monitorMqtt_Device, clientDevice, "clientDevice")
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
                ElseIf TypeOf ctrl Is TAT_UTILITY_CTRL.ctrlLine_ Then
                    Dim tmpCtrl As TAT_UTILITY_CTRL.ctrlLine_ = ctrl
                    Set_LineProperty(tmpCtrl)
                    AddHandler tmpCtrl.MouseDown, AddressOf CtrlLine_MouseDown
                ElseIf TypeOf ctrl Is Line.Line Then
                    Arrlay = CInt(ctrl.Name.Replace("Line", ""))
                    Line_(Arrlay) = ctrl
                End If
            Next
            dtLine = GetLine_in_Collection(From_scada.Controls) '=============== FOR SHOW LINE
        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number & " [cls_scada_Contral/Get_Property]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Public Sub Reload_Line(From_scada As Form)
        Dim tmpAppName As String = Assembly.GetExecutingAssembly().GetName().Name
        dtConfigLine = GetLine_Config(tmpAppName, Form_Scada.Name)
        For Each ctrl As Control In From_scada.Controls
            If TypeOf ctrl Is TAT_UTILITY_CTRL.ctrlLine_ Then
                Dim tmpCtrl As TAT_UTILITY_CTRL.ctrlLine_ = ctrl
                Set_LineProperty(tmpCtrl)
                'AddHandler tmpCtrl.MouseDown, AddressOf CtrlLine_MouseDown
            End If
        Next
        dtLine = GetLine_in_Collection(From_scada.Controls, "FINISH") '=============== FOR SHOW LINE
    End Sub

    Public Sub ReloadDB_LineConfig(From_scada As Form)
        Dim tmpAppName As String = Assembly.GetExecutingAssembly().GetName().Name
        dtConfigLine = GetLine_Config(tmpAppName, Form_Scada.Name)
        For Each ctrl As Control In From_scada.Controls
            If TypeOf ctrl Is TAT_UTILITY_CTRL.ctrlLine_ Then
                Dim tmpCtrl As TAT_UTILITY_CTRL.ctrlLine_ = ctrl
                Set_LineProperty(tmpCtrl)
                'AddHandler tmpCtrl.MouseDown, AddressOf CtrlLine_MouseDown
            End If
        Next
        dtLine = GetLine_in_Collection(From_scada.Controls, "EDIT") '=============== FOR SHOW LINE
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
                    '//=== LOAD OUT
                ElseIf TypeOf Dctrl Is TAT_CtrlAlarm.ctrlLoadout_ Then
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlLoadout_).IpAddress = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlLoadout_).UserName = MqttUser
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlLoadout_).Password = MqttPass
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlLoadout_).SubMqtt(client)
                    '//=== OTHER
                ElseIf TypeOf Dctrl Is TAT_CtrlAlarm.ctrlOther_ Then
                    DirectCast(Dctrl, TAT_CtrlAlarm.ctrlOther_).SubMqtt(client)
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
                    Dim tmpCtrl As TAT_CtrlRoute.ctrlRouteConveyer_
                    tmpCtrl = Dctrl
                    AddHandler tmpCtrl.ROUTE_MouseUp_, AddressOf CtrlRoute_MouseDown
                    AddHandler tmpCtrl.AKN_MouseUp_, AddressOf AKN_MouseUp
                    AddHandler tmpCtrl.COM_MouseUp_, AddressOf COMP_MouseUp
                    '//=== JOG
                ElseIf TypeOf Dctrl Is TAT_CtrlRoute.ctrlJogRoute_ Then
                    DirectCast(Dctrl, TAT_CtrlRoute.ctrlJogRoute_).SubMqtt(client)
                    Dim tmpCtrl As TAT_CtrlRoute.ctrlJogRoute_
                    tmpCtrl = Dctrl
                    AddHandler tmpCtrl.JOG_MouseUp_, AddressOf CtrlJogRoute_MouseDown
                    '==================================== add 2023-07-27 by nattapong
                    '//=== AUTO CHANGE
                ElseIf TypeOf Dctrl Is TAT_CtrlRoute.ctrlAutoChage_ Then
                    DirectCast(Dctrl, TAT_CtrlRoute.ctrlAutoChage_).SubMqtt(client)
                    Dim tmpCtrl As TAT_CtrlRoute.ctrlAutoChage_
                    tmpCtrl = Dctrl
                    AddHandler tmpCtrl.SetJogTime_MouseUp_, AddressOf CtrlAutoChange_MouseUp
                    AddHandler tmpCtrl.SetLowTime_MouseUp_, AddressOf CtrlAutoChange_MouseUp
                    AddHandler tmpCtrl.SetHightTime_MouseUp_, AddressOf CtrlAutoChange_MouseUp
                    '//=== Bin
                ElseIf TypeOf Dctrl Is TAT_CtrlBin.ctrlBin_ Then
                    DirectCast(Dctrl, TAT_CtrlBin.ctrlBin_).SubMqtt(clientBin)
                    Dim tmpCtrl As TAT_CtrlBin.ctrlBin_
                    tmpCtrl = Dctrl
                    AddHandler tmpCtrl.MouseUp, AddressOf Bin_MouseDown
                    '//=== Liquid
                ElseIf TypeOf Dctrl Is TAT_CtrlBin.ctrlLq_ Then
                    DirectCast(Dctrl, TAT_CtrlBin.ctrlLq_).SubMqtt(clientBin)
                    Dim tmpCtrl As TAT_CtrlBin.ctrlLq_
                    tmpCtrl = Dctrl
                    AddHandler tmpCtrl.MouseUp, AddressOf Bin_MouseDown
                    '//=== SELECTOR MQTT
                ElseIf TypeOf Dctrl Is TAT_CtrlSelector.ctrlSelectorMode_ Then
                    DirectCast(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).SubMqtt(client)
                    DirectCast(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).IpAddress_ = IpAddressMqtt
                    DirectCast(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).UserName_ = MqttUser
                    DirectCast(Dctrl, TAT_CtrlSelector.ctrlSelectorMode_).Password_ = MqttPass
                    Dim tmpCtrl As TAT_CtrlSelector.ctrlSelectorMode_
                    tmpCtrl = Dctrl
                    AddHandler tmpCtrl.Select_MouseUp_, AddressOf CtrlSelect_MouseDown
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
            '=============== FOR CONFIG LINE
            If Line_EditMode Then
                tempObject_Condition = CType(sender, TAT_MQTT_CTRL.ctrlTAT_)
                frmConfig_Line.Add_Condition()
                Exit Sub
            End If

            '=============== FOR DRAW LINE BY APICHAT
            Dim field = Form_Scada.GetType().GetField("LineManage_Mode", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
            If field IsNot Nothing Then
                Dim blMode = field.GetValue(Form_Scada)
                If blMode Then
                    Dim fieldProp = Form_Scada.GetType().GetField("_propForm", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
                    If fieldProp IsNot Nothing Then
                        Dim frmProp As frmLineProperties = fieldProp.GetValue(Form_Scada)
                        If frmProp IsNot Nothing Then
                            Dim tmpName As String = DirectCast(sender, TAT_MQTT_CTRL.ctrlTAT_).Name
                            frmProp.Add_Condition(tmpName)
                        End If
                    End If
                    Exit Sub
                End If
            End If

            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Dim motor = TryCast(sender, TAT_MQTT_CTRL.ctrlTAT_)
                If motor IsNot Nothing AndAlso Not String.IsNullOrEmpty(motor.SelectorControl) Then
                    Dim foundControl As Control =
                    Application.OpenForms.OfType(Of Form)().
                        SelectMany(Function(f) f.Controls.Find(motor.SelectorControl, True)).
                        FirstOrDefault()

                    If foundControl IsNot Nothing AndAlso TypeOf foundControl Is MQTT_CTRL_OTHERDEVICE.ctrlDevice Then
                        Dim dev = DirectCast(foundControl, MQTT_CTRL_OTHERDEVICE.ctrlDevice)
                        Show_Menu(sender, New Point(e.X, e.Y), dev.Status_Run)
                        Exit Sub
                    End If
                End If

                Show_Menu(sender, New Point(e.X, e.Y), statusRemote)

            ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
                If StrMqtt_Config_.RouteConfig Then
                    Dim hostForm As Form = TryCast(TryCast(sender, Control)?.FindForm(), Form)
                    Dim menu As New clsRoute
                    menu.ShowMenu_(sender, New Point(e.X, e.Y), "", hostForm, PLC_NO)
                End If
            End If

        Catch ex As Exception
            Dim strMessage =
                "Error Number :  " & Err.Number &
                " [cls_scada_Contral/Motor_MouseDown]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    '============================================== ADD FOR CONFIG LINE BY APICHAT
    Private Sub CtrlLine_MouseDown(sender As Object, e As MouseEventArgs)
        If Line_EditMode = True Then
            Dim tmpApp As String = Assembly.GetExecutingAssembly().GetName().Name
            Dim tmpFrmName As String = Form_Scada.Name
            If frmConfig_Line.Visible = True AndAlso addContinuousMode = True Then '============== When config's form is opened and already click add member
                Dim selectedLine As TAT_UTILITY_CTRL.ctrlLine_ = CType(sender, TAT_UTILITY_CTRL.ctrlLine_)
                If Is_MainLine(selectedLine.Name) = True Then
                    frmConfig_Line.Alarm_Config(selectedLine.Name, "MAIN")
                ElseIf Is_MemberLine(selectedLine.Name, tmpApp, tmpFrmName) <> "" Then
                    frmConfig_Line.Alarm_Config(selectedLine.Name, "MEMBER", Is_MemberLine(selectedLine.Name, tmpApp, tmpFrmName))
                Else
                    tempContinueLine = CType(sender, TAT_UTILITY_CTRL.ctrlLine_)
                    '===================== Highlight Selected Line
                    selectedLine.lineColor = Color.Red
                    'selectedLine.BringToFront()
                    frmConfig_Line.Add_Continue()
                End If
                'Exit Sub
            ElseIf frmConfig_Line.Visible = True Then '============== When config's form is opened but want change to other line
                Dim selectedLine As TAT_UTILITY_CTRL.ctrlLine_ = CType(sender, TAT_UTILITY_CTRL.ctrlLine_)
                Dim tmpMainLine As String = Is_MemberLine(DirectCast(sender, TAT_UTILITY_CTRL.ctrlLine_).Name, tmpApp, tmpFrmName)
                If tmpMainLine <> "" Then '================= If selected line is member will show main line in config's form
                    If Check_ExistLine(tmpMainLine) = True Then '=========Line is still use
                        frmConfig_Line.Closing_Form()
                        Show_ConfigLine(CType(Form_Scada.Controls(tmpMainLine), TAT_UTILITY_CTRL.ctrlLine_))
                        frmConfig_Line.Alarm_Config(selectedLine.Name, "MEMBER", tmpMainLine)
                    Else '=========Line is deleted
                        frmConfig_Line.Closing_Form()
                        delete_lineConfig(Assembly.GetExecutingAssembly().GetName().Name, Form_Scada.Name, tmpMainLine) 'Delete in db
                        Show_ConfigLine(CType(sender, TAT_UTILITY_CTRL.ctrlLine_))
                    End If
                Else '================= If selected line isn't member will show config's form with this line
                    frmConfig_Line.Closing_Form()
                    Show_ConfigLine(CType(sender, TAT_UTILITY_CTRL.ctrlLine_))
                End If
                'frmConfig_Line.Closing_Form()
                'Show_ConfigLine(selectedLine)
            Else
                Dim selectedLine As TAT_UTILITY_CTRL.ctrlLine_ = CType(sender, TAT_UTILITY_CTRL.ctrlLine_)
                Dim tmpMainLine As String = Is_MemberLine(DirectCast(sender, TAT_UTILITY_CTRL.ctrlLine_).Name, tmpApp, tmpFrmName)
                If tmpMainLine <> "" Then '================= If selected line is member will show main line in config's form
                    If Check_ExistLine(tmpMainLine) = True Then '=========Line is still use
                        Show_ConfigLine(CType(Form_Scada.Controls(tmpMainLine), TAT_UTILITY_CTRL.ctrlLine_))
                        frmConfig_Line.Alarm_Config(selectedLine.Name, "MEMBER", tmpMainLine)
                    Else '=========Line is deleted
                        delete_lineConfig(Assembly.GetExecutingAssembly().GetName().Name, Form_Scada.Name, tmpMainLine) 'Delete in db
                        Show_ConfigLine(CType(sender, TAT_UTILITY_CTRL.ctrlLine_))
                    End If
                Else '================= If selected line isn't member will show config's form with this line
                    Show_ConfigLine(CType(sender, TAT_UTILITY_CTRL.ctrlLine_))
                End If
            End If
            'If e.Button = Windows.Forms.MouseButtons.Left Then
            '    'Dim ctrlLine As TAT_UTILITY_CTRL.ctrlLine_ = CType(sender, TAT_UTILITY_CTRL.ctrlLine_)
            '    frmConfig_Line.appName = Assembly.GetExecutingAssembly().GetName().Name
            '    frmConfig_Line.frmParent = Form_Scada
            '    frmConfig_Line.formName = Form_Scada.Name
            '    frmConfig_Line.tmpLine = CType(sender, TAT_UTILITY_CTRL.ctrlLine_)
            '    frmConfig_Line.Show()
            'End If
        End If
    End Sub
    Private Sub Show_ConfigLine(selectedLine As TAT_UTILITY_CTRL.ctrlLine_)
        frmConfig_Line.appName = Assembly.GetExecutingAssembly().GetName().Name
        frmConfig_Line.frmParent = Form_Scada
        frmConfig_Line.formName = Form_Scada.Name
        frmConfig_Line.tmpLine = selectedLine
        frmConfig_Line.clsScada = Me
        frmConfig_Line.Show()
    End Sub
    Private Function Check_ExistLine(strLineName As String) As Boolean
        Dim foundControl As Control = Form_Scada.Controls.Find(strLineName, True).FirstOrDefault()

        If foundControl IsNot Nothing Then
            ' Control ถูกพบ
            Return True
        Else
            ' Control ไม่ถูกพบ
            Return False
        End If
    End Function
    Private Sub delete_lineConfig(strAppName As String, strFormName As String, strLineName As String)
        Dim strSQL As String
        strSQL = "EXEC dbo.delete_lineConfig "
        strSQL += "'" & strLineName & "',"
        strSQL += "'" & strAppName & "',"
        strSQL += "'" & strFormName & "'"
        CnBatching.ExecuteNoneQuery(strSQL)
        ReloadDB_LineConfig(Form_Scada)
    End Sub

    Private Sub OtherDevice_MouseDown(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Show_Menu(sender, New Point(e.X, e.Y), statusRemote)
            End If

        Catch ex As Exception
            Dim strMessage =
                "Error Number :  " & Err.Number &
                " [cls_scada_Contral/OtherDevice_MouseDown]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub Bin_MouseDown(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Show_Menu(sender, New Point(e.X, e.Y), statusRemote)

            ElseIf e.Button = Windows.Forms.MouseButtons.Left Then
                If StrMqtt_Config_.RouteConfig Then
                    Dim menu As New clsRoute
                    menu.ShowMenu_(sender, New Point(e.X, e.Y), "", Form_Scada, PLC_NO)
                End If
            End If

        Catch ex As Exception
            Dim strMessage As String =
                "Error Number :  " & Err.Number &
                "[cls_scada_Contral/Bin_MouseDown]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub CtrlAnalog_MouseDown(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Show_AnalogMenu(sender, New Point(e.X, e.Y))
            End If

        Catch ex As Exception
            Dim strMessage As String =
                "Error Number :  " & Err.Number & vbNewLine &
                " [cls_scada_Contral/CtrlAnalog_MouseDown]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub CtrlRoute_MouseDown(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Show_RouteMenu(sender, New Point(e.X, e.Y))
            End If

        Catch ex As Exception
            Dim strMessage As String =
                "Error Number :  " & Err.Number & vbNewLine &
                " [cls_scada_Contral/CtrlRoute_MouseDown]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub CtrlJogRoute_MouseDown(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Left Then
                JOG_MouseUp(sender, e)
            ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                Show_JogRouteMenu(sender, New Point(e.X, e.Y))
            End If

        Catch ex As Exception
            Dim strMessage =
                "Error Number :  " & Err.Number &
                " [cls_scada_Contral/CtrlJogRoute_MouseDown]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub CtrlAutoChange_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Left Then
                JOG_MouseUp(sender, e)
            ElseIf e.Button = Windows.Forms.MouseButtons.Right Then
                Show_AutoChangeRouteMenu(sender, New Point(e.X, e.Y))
            End If

        Catch ex As Exception
            Dim strMessage =
                "Error Number :  " & Err.Number &
                " [cls_scada_Contral/CtrlAutoChange_MouseUp]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub CtrlSelect_MouseDown(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Right Then
                Show_SelectMenu(sender, New Point(e.X, e.Y))
            End If

        Catch ex As Exception
            Dim strMessage =
                "Error Number :  " & Err.Number &
                " [cls_scada_Contral/CtrlSelect_MouseDown]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub


    Private Sub CtrlPID_OnPointerChange(sender As Object, IdxPointer As Integer)
        ctrlPID_Changing = True
        ctrlPID_Delay = 2
    End Sub

    Private Sub CtrlPID_PointerValue_Changed(sender As Object, pointerName As String, ptValue As Integer)
        Try
            ' Check Login and Permission & Handle Event
            If Not RequireScada() Then
                ctrlPID_Changing = False
                Exit Sub
            End If

            mnuSetPID_Pointer(sender, pointerName, ptValue)

        Catch ex As Exception
            Dim strMessage = "Error Number :  " & Err.Number &
                         " [cls_scada_Contral/CtrlPID_PointerValue_Changed]" & vbNewLine &
                         "Error Description :  " & ex.Message & vbCrLf &
                         "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        Finally
            ctrlPID_Changing = False
        End Try
    End Sub


    '=============================================== ADD FOR Event status changing motor,slide,flapbox
    Private Sub Ctrl_StatusChanged(sender As Object)
        Dim tmpCtrl As TAT_MQTT_CTRL.ctrlTAT_ = CType(sender, TAT_MQTT_CTRL.ctrlTAT_)
        '================ Show Line
        Dim dtCheck As New DataTable
        dtCheck = fn_filter_data("condition like '%" & tmpCtrl.Name & "%'", dtLine)
        If dtCheck.Rows.Count > 0 Then
            For i As Integer = 0 To dtCheck.Rows.Count - 1
                Dim tmpLine As TAT_UTILITY_CTRL.ctrlLine_ = CType(Form_Scada.Controls(dtCheck.Rows(i)("line_name")), TAT_UTILITY_CTRL.ctrlLine_)
                If tmpLine.InvokeRequired Then
                    tmpLine.Invoke(New Action(Sub() tmpLine.Visible = EvaluateVisibilityCondition(tmpLine)))
                Else
                    tmpLine.Visible = EvaluateVisibilityCondition(tmpLine)
                End If

            Next
        End If
    End Sub

    '=================== FOR MULTI LANGUAGE
    Private Sub ctrl_OnAlarmMessageChanged(sender As Object)
        Dim tmpCtrl As TAT_MQTT_CTRL.ctrlTAT_ = CType(sender, TAT_MQTT_CTRL.ctrlTAT_)
        'If tmpCtrl.ControlType.ToString = "MOTOR" AndAlso tmpCtrl.Index = 164 Then
        '    Debug.Print("TEST")
        'End If
        Change_Object_InterlockMsg(tmpCtrl, cn_Batching, structApp_Language.strLang_Main)
    End Sub

    Private Sub Set_LineProperty(ByRef tmpLine As TAT_UTILITY_CTRL.ctrlLine_)
        Dim dtCheck As New DataTable
        dtCheck = fn_filter_data("obj_name = '" & tmpLine.Name & "'", dtConfigLine)
        If dtCheck.Rows.Count > 0 Then
            tmpLine.VisibilityCondition = dtCheck.Rows(0)("condition_line")
            tmpLine.ContinuousLine = dtCheck.Rows(0)("continuous_line")
        End If
    End Sub

    Private Function Is_MemberLine(strLineName As String, strAppName As String, strFormName As String) As String
        Dim dtCheck As New DataTable
        Dim strQuery As String = "(continuous_line like '%" & strLineName & "' Or continuous_line like '%" & strLineName & ",%') and (c_app_name = '" & strAppName & "' and form_name = '" & strFormName & "')"
        dtCheck = fn_filter_data(strQuery, dtConfigLine)
        If dtCheck.Rows.Count > 0 Then
            Return dtCheck.Rows(0)("obj_name")
        Else
            Return ""
        End If
    End Function

    Private Function Is_MainLine(strLineName As String) As Boolean
        Dim dtCheck As New DataTable
        dtCheck = fn_filter_data("obj_name = '" & strLineName & "'", dtConfigLine)
        If dtCheck.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function

    '   #################################################################################################### R NATTAPONG
    Private Sub R_MouseDoubleClick(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If sender.SetValue_ = True Then
                    If sender.WORD_ = True Then
                        Dim send_long(1) As Int32
                        Dim DataInput As Double = InputBox(sender.Tag, "Input Numeric Value")
                        send_long(0) = DataInput * sender.Divide_
                        MX.mxWriteBlockLong(MX.MxCom1, sender.Name, 1, send_long)
                    Else
                        Dim DATA(1) As Int16
                        Dim DATA1 As Double = InputBox(sender.Tag, "Input Numeric Value")
                        DATA(0) = DATA1 * sender.Divide_
                        MX.mxWriteBlock(MX.MxCom1, sender.Name, 1, DATA)
                    End If
                End If
            End If

        Catch ex As Exception
            Dim strMessage =
                "Error Number :  " & Err.Number &
                " [cls_scada_Contral/R_MouseDoubleClick]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub


    '   #################################################################################################### RouteConveyer NATTAPONG
    Private Sub AKN_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
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
            Dim strMessage =
                "Error Number :  " & Err.Number &
                " [cls_scada_Contral/AKN_MouseUp]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub JOG_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If e.Button = Windows.Forms.MouseButtons.Left Then
                If sender.Mqtt_selectroute_status_.STA_JOG = True Then
                    MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectroute_config_.M_JOG, 0)
                Else
                    MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectroute_config_.M_JOG, 1)
                End If
            End If

        Catch ex As Exception
            Dim strMessage =
                "Error Number :  " & Err.Number &
                " [cls_scada_Contral/JOG_MouseUp]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub COMP_MouseUp(sender As Object, e As MouseEventArgs)
        Try
            ' Check Login and Permission
            If Not RequireScada() Then Exit Sub

            ' Handle Event
            If Not RequireScada() Then Exit Sub

            If e.Button = Windows.Forms.MouseButtons.Left Then
                If sender.Mqtt_selectroute_status_.STA_REQ = True Then
                    MX.mxDevSetM(MX.MxCom1, sender.mqtt_selectroute_config_.M_COM, 1)
                End If
            End If

        Catch ex As Exception
            Dim strMessage =
                "Error Number :  " & Err.Number &
                " [cls_scada_Contral/COMP_MouseUp]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
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

#Region "Keep Log"
    Private Sub WriteLog_Action(strMenu As String, strCommand As String, ret As Integer)
        Dim strLog As String
        strLog = "ACTION : " & strMenu & vbCrLf
        strLog += "USER : " & UserLogon_.UserName & vbCrLf
        strLog += "OBJECT NAME : " & objName & vbCrLf
        strLog += "COMMAND : " & strCommand & vbCrLf
        strLog += "RETURN : " & ret

        Dim eventID As Long = 0
        If InStr(strCommand, "Call_EXE") > 0 Then
            eventID = 3000
        ElseIf InStr(strCommand, "EXEC") Then
            eventID = 2000
        Else
            eventID = 1000
        End If

        WriteToEventLog(strLog, eventID, 1, EventLogEntryType.Information)
    End Sub
    Private Sub WarningLog_Action(strMenu As String, strCommand As String, Message As String)
        Dim strLog As String
        strLog = "ACTION : " & strMenu & vbCrLf
        strLog += "USER : " & UserLogon_.UserName & vbCrLf
        strLog += "OBJECT NAME : " & objName & vbCrLf
        strLog += "COMMAND : " & strCommand & vbCrLf
        strLog += "MESSAGE : " & Message

        Dim eventID As Long = 0
        If InStr(strCommand, "Call_EXE") > 0 Then
            eventID = 3000
        ElseIf InStr(strCommand, "EXEC") Then
            eventID = 2000
        Else
            eventID = 1000
        End If

        WriteToEventLog(strLog, eventID, 1, EventLogEntryType.Warning)
    End Sub

    Private Sub ErrorLog_Action(strMenu As String, strCommand As String, Message As String)
        Dim strLog As String
        strLog = "ACTION : " & strMenu & vbCrLf
        strLog += "USER : " & UserLogon_.UserName & vbCrLf
        strLog += "OBJECT NAME : " & objName & vbCrLf
        strLog += "COMMAND : " & strCommand & vbCrLf
        strLog += "MESSAGE : " & Message

        Dim eventID As Long = 0
        If InStr(strCommand, "Call_EXE") > 0 Then
            eventID = 3000
        ElseIf InStr(strCommand, "EXEC") Then
            eventID = 2000
        Else
            eventID = 1000
        End If

        WriteToEventLog(strLog, eventID, 1, EventLogEntryType.Error)
    End Sub
#End Region

End Class
