Imports System.Timers
Imports uPLibrary.Networking.M2Mqtt ' ใช้ M2Mqtt Library

Public Class clsMqttHeartbeatMonitor
    Private _mqttClient As MqttClient
    Private _heartbeatCounter As Integer = 0
    Private WithEvents _timer As Timer
    Private _heartbeatTimeout As Integer = 5000 ' ระยะเวลารอ (5 วินาที)
    Private _notifyIcon As NotifyIcon ' เพิ่ม NotifyIcon สำหรับแสดงการแจ้งเตือน
    Public StatusActive As Boolean = False
    Private ClientName As String = ""
    Private frmName As String = ""

    ' เหตุการณ์ที่จะถูกเรียกเมื่อไม่มีการตอบสนองจาก MQTT
    Public Event ConnectionLost(clientName As String)

    ' Constructor รับ MqttClient เข้ามา และเริ่มการตรวจสอบ Heartbeat
    Public Sub New(client As MqttClient, strName As String, strFormName As String)
        _mqttClient = client
        ClientName = strName
        frmName = strFormName

        ' ผูกเหตุการณ์เมื่อมีการรับ Publish message จาก Mqtt
        AddHandler _mqttClient.MqttMsgPublishReceived, AddressOf OnMqttMessageReceived

        ' ตั้งค่า Timer ให้นับทุก 1 วินาที
        _timer = New Timer(1000)
        _timer.AutoReset = True
        '_timer.Start()

        ' ตั้งค่า NotifyIcon
        _notifyIcon = New NotifyIcon()
        _notifyIcon.Icon = SystemIcons.Exclamation ' ใช้ไอคอนที่เป็นเครื่องหมายตกใจ (หรือใช้ไอคอนที่กำหนดเอง)
        _notifyIcon.Visible = True
    End Sub

    ' ฟังก์ชันเรียกใช้ทุกครั้งที่ Timer ติ๊ก (ทุก 1 วินาที)
    Private Sub OnTimedEvent(source As Object, e As ElapsedEventArgs) Handles _timer.Elapsed
        ' เพิ่มค่า heartbeat ทีละ 1
        _heartbeatCounter += 1

        ' ถ้า heartbeat มากกว่าหรือเท่ากับ 5 แสดงว่าไม่มีการตอบสนอง
        If _heartbeatCounter >= 5 Then
            ' หยุด Timer และแจ้งเหตุการณ์ว่า Connection Lost
            _timer.Stop()
            If StatusActive = True Then
                StatusActive = False
                RaiseEvent ConnectionLost(ClientName)
                _heartbeatCounter = 0
                ' แสดง Notification
                ShowConnectionLostNotification()
            End If

        End If
    End Sub

    Private Sub ShowConnectionLostNotification()
        Dim strMessage As String = "Client (" & ClientName & ") connection has been lost." & vbCrLf
        strMessage += "FORM : " & strMessage
        _notifyIcon.BalloonTipTitle = "MQTT Connection Lost"
        _notifyIcon.BalloonTipText = frmName
        _notifyIcon.BalloonTipIcon = ToolTipIcon.Warning
        _notifyIcon.ShowBalloonTip(3000) ' แสดงการแจ้งเตือนเป็นเวลา 3 วินาที
        Keeplog(strMessage, EventLogEntryType.Error)
    End Sub

    Private Sub ShowConnectionActiveNotification()
        Dim strMessage As String = "Client (" & ClientName & ") has been actived." & vbCrLf
        strMessage += "FORM : " & frmName
        '_notifyIcon.BalloonTipTitle = "MQTT Activated"
        '_notifyIcon.BalloonTipText = strMessage
        '_notifyIcon.BalloonTipIcon = ToolTipIcon.Info
        '_notifyIcon.ShowBalloonTip(3000) ' แสดงการแจ้งเตือนเป็นเวลา 3 วินาที
        Keeplog(strMessage, EventLogEntryType.Information)
    End Sub

    ' ฟังก์ชันที่เรียกเมื่อมี Publish message เข้ามา
    Private Sub OnMqttMessageReceived(sender As Object, e As uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs)
        If StatusActive = False Then
            StatusActive = True
            'Dim strMessage As String = "Client (" & ClientName & ") has been actived."
            ShowConnectionActiveNotification()
            'Keeplog(strMessage, EventLogEntryType.Information)
            _timer.Start()
        End If
        ' Reset ค่า heartbeat เมื่อมี message ใหม่เข้ามา
        _heartbeatCounter = 0
    End Sub

    ' หยุดการตรวจสอบ
    Public Sub StopMonitoring()
        _timer.Stop()
        RemoveHandler _mqttClient.MqttMsgPublishReceived, AddressOf OnMqttMessageReceived
    End Sub

    Private Sub Keeplog(strMessage As String, EventType As EventLogEntryType)
        WriteToEventLog(strMessage, 6000, 1, EventType)
    End Sub
End Class

