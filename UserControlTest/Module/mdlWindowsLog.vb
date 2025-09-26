Imports System.Diagnostics
Module mdlWindowsLog
    'Sub WriteToEventLog(strMessage As String, fType As String) '==> fType Is "I" : Information, "W" : Warning, "E" : Error
    '    Dim myLog As New EventLog()
    '    myLog.Source = "Test From " & Application.ProductName

    '    Select Case fType
    '        Case "I"
    '            ' Write an informational entry to the event log.    
    '            myLog.WriteEntry(strMessage, EventLogEntryType.Information)
    '        Case "W"
    '            ' Write a warning entry to the event log.    
    '            myLog.WriteEntry(strMessage, EventLogEntryType.Warning)
    '        Case "E"
    '            ' Write an error entry to the event log.    
    '            myLog.WriteEntry(strMessage, EventLogEntryType.Error)
    '    End Select

    'End Sub
    ''' <summary>
    '''   WriteToEventLog เป็น Function ที่ใช้ในการบันทึก Log ลงใน windows เพื่อให้สามารถเปิดดูได้จาก Event Viewer [
    '''   strMessage คือข้อความที่ต้องการให้บันทึก ,
    '''   eventID เลขที่ตามที่กำหนด เก็บเป็น Long ,
    '''   eventCatagory ค่า Catagory เก็บเป็น Integer ]
    ''' </summary>
    ''' <param name="strMessage">
    ''' strMessage คือข้อความที่ต้องการให้บันทึก
    ''' </param>
    ''' <param name="eventID">
    ''' eventID เลขที่ตามที่กำหนด เก็บเป็น Long
    ''' </param>
    ''' <param name="eventCatagory">
    ''' eventCatagory ค่า Catagory เก็บเป็น Integer
    ''' </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Sub WriteToEventLog(strMessage As String, eventID As Long, eventCatagory As Integer, eventType As EventLogEntryType)
        Dim sourceName As String = Application.ProductName
        Dim logName As String = "SmartFeedmill"

        Using eventLog As New EventLog(logName, ".", sourceName)
            Dim eventInstance As New EventInstance(eventID, eventCatagory, eventType)
            eventLog.WriteEvent(eventInstance, strMessage)
        End Using
    End Sub

    '============== For Keep log shutdown program

    Public Sub LogShutdown(strReason As String, EventLogType As EventLogEntryType)
        Dim strLogMsg As String = "Close Application " & vbCrLf
        strLogMsg += "REASON : " & strReason & vbCrLf
        strLogMsg += "TIME : " & DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") & vbCrLf
        strLogMsg += "PATH : " & My.Application.Info.DirectoryPath
        WriteToEventLog(strLogMsg, 4000, 1, EventLogType)
    End Sub

    Sub CreateEventSource()
        Dim sourceName As String = Application.ProductName
        Dim logName As String = "SmartFeedmill"
        'Dim sourceName As String = "SmartFeedmill"
        'Dim logName As String = Application.ProductName

        If Not EventLog.SourceExists(sourceName) Then
            Dim sourceData As New EventSourceCreationData(sourceName, logName)
            EventLog.CreateEventSource(sourceData)
        End If
    End Sub

    Sub DeleteEventSourceAndLog()
        Dim sourceName As String = Replace(Application.ProductName, " ", "_")
        Dim logName As String = "SmartFeedmill"

        If EventLog.SourceExists(sourceName) Then
            EventLog.DeleteEventSource(sourceName)
        End If

        If EventLog.Exists(logName) Then
            EventLog.Delete(logName)
        End If
    End Sub
End Module
