Imports System.IO
Imports uPLibrary.Networking.M2Mqtt
Module mdlParameter
    Public dbSucceeded As Boolean

    Public Structure StrDB_Name
        Public Odbc As String
        Public Uid As String
        Public Pwd As String
    End Structure
    Public client As MqttClient

    Public Structure StrMqtt_Config
        Public IpAddressMqtt As String
        Public StationMqtt As Int16
        Public DB_IpAddress As String
        Public DB_Name As String
        Public DB_ConType As String
        Public DbUser As String
        Public DbPass As String
        Public MqttUser As String
        Public MqttPass As String
        Public TmpConfig As String
        Public RouteConfig As Boolean
        Public HardLock As Boolean
        Public Mode_TestIO As Boolean
        Public Confirm_TestIO As Boolean
    End Structure

    Public Batching_Conf As DB_Config
    Public Auto_Route_Conf As DB_Config
    Public Dev_Center_Conf As DB_Config
    Public Premix_Conf As DB_Config
    Public Structure DB_Config
        Public Name As String
        Public IPAddress As String
        Public User As String
        Public Password As String
        Public Connection_Type As String
    End Structure

    Public StrMqtt_Config_ As StrMqtt_Config

    Public Structure mqtt_AlarmScale
        Dim SCALE_WEIGHT As Int32
        Dim TARGET_WEIGHT As Int32
        Dim ACTUAL_WEIGHT As Int32
        Dim PRODUCTION_RUN As Int32
        Dim BATCH_PRESET As Int16
        Dim BATCH_COUNT As Int16
    End Structure

    Public Structure UserLogon
        Public UserCode As String
        Public UserName As String
        Public UserType As String
        Public EmployeeId As String
    End Structure
    Public UserLogon_ As UserLogon
    Public RemoteControl As String = "Connect..."

    Public Structure StatusReadPort
        Public strPlc As String
        Public strPort() As String
    End Structure

    Public StatusReadPort_ As New StatusReadPort
    Public tmpCheckStatusMqtt As String
    Public tmpCountCheck As Int16
    Public tmpCheckConnectMqtt As Boolean
    Public CheckStatusReal As String

    Public statusMQTT As MQTT_Status
    Public Structure MQTT_Status
        Public DATE_TIME As String
        Public COUNT As String
    End Structure

    Public Function Get_ConfigPath() As String
        Dim intCountIndex, i As Integer
        Dim strUsePath As String
        Dim str_chkPath() As String
        Dim tmpChkPath As String
        str_chkPath = Split(Application.StartupPath, "\")

        intCountIndex = UBound(str_chkPath) + 1

        For i = 0 To intCountIndex - 2
            strUsePath = strUsePath + str_chkPath(i) + "\"
        Next
        tmpChkPath = strUsePath & "_Config"

        '============================================== กรณีรันจากโค้ด
        If Directory.Exists(tmpChkPath) = False Then
            strUsePath = ""
            For i = 0 To intCountIndex - 5
                strUsePath = strUsePath + str_chkPath(i) + "\"
            Next
        End If


        Return strUsePath

    End Function
End Module
