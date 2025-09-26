

Imports System.IO
Imports System.Threading.Thread
Imports System.Globalization


Public Class clsIO

    Dim strPath As String = Get_UsePath()

    Public Sub CreateFolderProject(ByVal LocalPath As String, ByVal DestPath As String)
        Try
            If Not Directory.Exists(LocalPath & "\TEXT_Active") Then Directory.CreateDirectory(LocalPath & "/TEXT_Active")
            If Not Directory.Exists(LocalPath & "\TEXT_Source") Then Directory.CreateDirectory(LocalPath & "/TEXT_Source")
            If Not Directory.Exists(LocalPath & "\TEXT_Sending") Then Directory.CreateDirectory(LocalPath & "/TEXT_Sending")
            If Not Directory.Exists(LocalPath & "\TEXT_Completed") Then Directory.CreateDirectory(LocalPath & "/TEXT_Completed")
            If Not Directory.Exists(LocalPath & "\TEXT_Error") Then Directory.CreateDirectory(LocalPath & "/TEXT_Error")
            If Not Directory.Exists(LocalPath & "\TEXT_EventLog") Then Directory.CreateDirectory(LocalPath & "/TEXT_EventLog")

        Catch ex As Exception

        End Try

    End Sub



    Public Function DeleteFile(ByVal Localfile As String) As Boolean

        Try
            File.Delete(Localfile)
            Return True
        Catch ex As Exception
            Return False
        End Try


    End Function


    Public Function ReturnFolderCurrentMonth(ByVal Localpath As String) As String

        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
        Dim d, m, y As String

        d = Today.Day
        m = Today.Month
        y = Today.Year

        If Today.Day.ToString().Length = 1 Then d = "0" & Today.Day
        If Today.Month.ToString().Length = 1 Then m = "0" & Today.Month

        If Not Directory.Exists(Localpath & "\" & y & "\" & m) Then
            Directory.CreateDirectory(Localpath & "\" & y & "\" & m)
        End If

        Return Localpath & "\" & y & "\" & m

    End Function

    Public Sub CoppyAllFile(ByVal Localpath As String, ByVal DestPath As String)

        Dim dirInfo As New IO.DirectoryInfo(Localpath)
        Dim fileObject As FileSystemInfo
        For Each fileObject In dirInfo.GetFileSystemInfos()
            If fileObject.Attributes = FileAttributes.Directory Then
                RecursiveSearch(fileObject.FullName)
            Else
                Console.WriteLine(fileObject.FullName)
            End If
        Next

    End Sub


    Public Sub writeLogEvent(ByVal LocalPath As String, ByVal str As String)
        Try


            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            Dim d, m, y As String
            d = Today.Day
            m = Today.Month
            y = Today.Year
            LocalPath = LocalPath.Substring(0, 3)
            Dim LogPath As String
            LogPath = ReturnFolderCurrentMonth("" + LocalPath + "Temp_SmartFeedmill\TAT_SMF\SCADA")
            Dim tmp_ProgramID As String = "TAT_SMF" ' Auto Route
            Dim tmp_ProgramTypeID As String = "MU"

            LogPath += "\" + tmp_ProgramID + "_" + tmp_ProgramTypeID + "_" + d + "" + m + "" + y + ".TXT"
            ' LogPath += "\" & y & "-" & m & "-" & d & "_Event.txt"
XX:
            My.Computer.FileSystem.WriteAllText(LogPath, Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("en-GB")) & ":::" & str & vbCrLf, True)
        Catch ex As Exception
            GoTo XX
            Dim Projext_name As String
            Projext_name = Application.ExecutablePath
            Projext_name = Projext_name.Replace(Application.StartupPath & "\", "")
            Projext_name = Projext_name.Replace(".EXE", "")
            MsgBox(Projext_name & " " & "FN: writeLogEvent  " & ex.Message)

        End Try

    End Sub

    ''' <summary>
    ''' Write Log
    ''' </summary>
    ''' <param name="LocalPath">Path of destination file</param>
    ''' <param name="nutrient">สารอาหาร</param>
    ''' <param name="str">Data</param>
    ''' <remarks></remarks>

    Public Sub writeLogData(ByVal LocalPath As String, ByVal nutrient As String, ByVal str As String)

        Try


            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            Dim d, m, y As String

            d = Today.Day
            m = Today.Month
            y = Today.Year

            Dim LogPath As String
            LogPath = ReturnFolderCurrentMonth(LocalPath)

            LogPath += "\" & y & "-" & m & "-" & d & "_" & nutrient & ".txt"

            My.Computer.FileSystem.WriteAllText(LogPath, str & vbCrLf, True)
        Catch ex As Exception
            Dim Projext_name As String
            Projext_name = Application.ExecutablePath
            Projext_name = Projext_name.Replace(Application.StartupPath & "\", "")
            Projext_name = Projext_name.Replace(".EXE", "")
            MsgBox(Projext_name & " " & "FN: writeLogData  " & ex.Message)
        End Try


    End Sub


    ''' <summary>
    ''' Write Log
    ''' </summary>
    ''' <param name="LocalPath">Path of destination file</param>
    ''' <param name="nutrient">สารอาหาร</param>
    ''' <param name="str">Data</param>
    ''' <remarks></remarks>

    Public Sub writeLogError(ByVal LocalPath As String, ByVal nutrient As String, ByVal str As String)

        Try

            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            Dim d, m, y As String

            d = Today.Day
            m = Today.Month
            y = Today.Year

            Dim LogPath As String
            LogPath = ReturnFolderCurrentMonth(LocalPath)

            LogPath += "\" & y & "-" & m & "-" & d & "_" & nutrient & "_ERROR.txt"

            My.Computer.FileSystem.WriteAllText(LogPath, str & vbCrLf, True)
        Catch ex As Exception
            Dim Projext_name As String
            Projext_name = Application.ExecutablePath
            Projext_name = Projext_name.Replace(Application.StartupPath & "\", "")
            Projext_name = Projext_name.Replace(".EXE", "")
            MsgBox(Projext_name & " " & "FN: writeLogError  " & ex.Message)
        End Try


    End Sub


    'Public Sub writeLogComplete(ByVal LocalPath As String, ByVal str As String)

    '    Try


    '        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
    '        Dim d, m, y As String

    '        d = Today.Day
    '        m = Today.Month
    '        y = Today.Year

    '        Dim LogPath As String
    '        LogPath = ReturnFolderCurrentMonth(LocalPath)

    '        LogPath += "\" & y & "-" & m & "-" & d & "_Event.txt"

    '        My.Computer.FileSystem.WriteAllText(LogPath, str & vbCrLf, True)
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try


    'End Sub


    Public Sub writeLogError(ByVal LocalPath As String, ByVal str As String)

        Try

            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            Dim d, m, y As String

            d = Today.Day
            m = Today.Month
            y = Today.Year


            Dim LogPath As String
            LogPath = ReturnFolderCurrentMonth(LocalPath)

            LogPath += "\" & y & "-" & m & "-" & d & "_Error.txt"


            My.Computer.FileSystem.WriteAllText(LogPath, str & vbCrLf, True)
        Catch ex As Exception

            Dim Projext_name As String
            Projext_name = Application.ExecutablePath
            Projext_name = Projext_name.Replace(Application.StartupPath & "\", "")
            Projext_name = Projext_name.Replace(".EXE", "")
            MsgBox(Projext_name & " " & "FN: writeLogError  " & ex.Message)
        End Try


    End Sub



    Public Function findFolder(ByVal path As String)

        Dim dir As New System.IO.DirectoryInfo(path)

        For Each g As System.IO.DirectoryInfo In dir.GetDirectories()
            MsgBox(g.FullName())
        Next

    End Function

    Public Function RecursiveSearch(ByVal path As String) As Boolean
        Dim dirInfo As New IO.DirectoryInfo(path)
        Dim fileObject As FileSystemInfo
        For Each fileObject In dirInfo.GetFileSystemInfos()
            If fileObject.Attributes = FileAttributes.Directory Then
                RecursiveSearch(fileObject.FullName)
            Else
                ' Console.WriteLine(fileObject.CreationTime)
            End If
        Next
        Return True
    End Function

    Public Function DeleteLogfile(ByVal path As String, ByVal dateCreateFile As Date) As Boolean

        If Directory.Exists(path) = False Then
            Exit Function
        End If

        Dim dirInfo As New IO.DirectoryInfo(path)
        Dim fileObject As FileSystemInfo
        For Each fileObject In dirInfo.GetFileSystemInfos()
            If fileObject.Attributes = FileAttributes.Directory Then
                DeleteLogfile(fileObject.FullName, dateCreateFile)
            Else
                If dateCreateFile.Date = fileObject.CreationTime.Date Then
                    File.Delete(fileObject.FullName)
                    Dim io As New clsIO
                    io.writeLogEvent(path, "Delete Logfile. " & fileObject.FullName)
                End If
            End If
        Next

        DeleteFolderMonth(path, dateCreateFile)

        Return True
    End Function


    Private Sub DeleteFolderMonth(ByVal Localpath As String, ByVal dateCreateFile As Date)

        Try
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            Dim Findpos As Integer

            Dim m, y, tmp As String
            m = dateCreateFile.Month
            y = dateCreateFile.Year

            Findpos = InStr(Localpath, y)
            tmp = Mid(Localpath, 1, Findpos - 1)



            Dim i As Byte
            i = 1

            While i < Int(m)
                If i < 10 Then
                    If Directory.Exists(tmp & y & "\0" & i.ToString) Then
                        Directory.Delete(tmp & y & "\0" & i.ToString, True)
                    End If
                Else
                    If Directory.Exists(tmp & y & "\" & i.ToString) Then
                        Directory.Delete(tmp & y & "\" & i.ToString, True)
                    End If
                End If
                i += 1
            End While




            'For i As Byte = 1 To Int(m)
            '    If i < 10 Then
            '        If Directory.Exists(tmp & y & "\0" & i.ToString) Then
            '            Directory.Delete(tmp & y & "\0" & i.ToString, True)
            '        End If
            '    Else
            '        If Directory.Exists(tmp & y & "\" & i.ToString) Then

            '            Directory.Delete(tmp & y & "\" & i.ToString, True)
            '        End If
            '    End If
            'Next

        Catch ex As Exception

        End Try
    End Sub

    Public Sub CloseEXE(EXE_name As String)
        Dim myProcess() As Process = System.Diagnostics.Process.GetProcessesByName(EXE_name)
        For Each myKill As Process In myProcess

            myKill.Kill()
        Next
    End Sub

    Public Sub Check_Program_run(EXE_name As String, AMG As String)
        Dim myProcess() As Process = System.Diagnostics.Process.GetProcessesByName(EXE_name)
        For Each myKill As Process In myProcess
            Exit Sub
        Next
        'Open_Application_For_Alarm(EXE_name, AMG)
    End Sub

    Public Function Open_Application_For_Route(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            path2 = path & "RouteControl_Conveyer\RouteControl_Conveyer.exe"
            path = path & "RouteControl_Conveyer\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Open_Application_For_Route_Batching(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            path2 = path & "RouteControl_Batching\RouteControl_Batching.exe"
            path = path & "RouteControl_Batching\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Open_Application_For_Bypass_Process(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            path2 = path & "AminosysBypass\Aminosys_Bypass.exe"
            path = path & "AminosysBypass\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Open_Application_For_Alarm(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            path2 = path & "Alarm\Alarm.exe"
            path = path & "Alarm\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Open_Application_For_Alarm_Scale(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            Name_Application = "TAT01_" & Name_Application & "_PARAMETER_ALARM_SCALE"
            path2 = path & "AlarmScale\AlarmScale.exe"
            path = path & "AlarmScale\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Open_Application_For_Alarm_Mixer(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            Name_Application = "TAT01_" & Name_Application & "_PARAMETER_ALARM_MIXER"
            path2 = path & "AlarmMixer\AlarmMixer.exe"
            path = path & "AlarmMixer\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Open_Application_For_Alarm_Handadd(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            Name_Application = "TAT01_" & Name_Application & "_PARAMETER_ALARM_HANDADD"
            path2 = path & "AlarmHandadd\AlarmHandAdd.exe"
            path = path & "AlarmHandadd\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Open_Application_For_Alarm_Surgebin(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            Name_Application = "TAT01_" & Name_Application & "_PARAMETER_ALARM_SURGEBIN"
            path2 = path & "AlarmSurgeBin\AlarmSurgeBin.exe"
            path = path & "AlarmSurgeBin\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Open_Application_For_Alarm_Liquid(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            Name_Application = "TAT01_" & Name_Application & "_PARAMETER_ALARM_LIQUID"
            path2 = path & "AlarmLiquid\AlarmLiquid.exe"
            path = path & "AlarmLiquid\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Open_Application_For_Alarm_Other(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            Name_Application = "TAT01_" & Name_Application & "_PARAMETER_ALARM_OTHER"
            path2 = path & "AlarmOther\AlarmOther.exe"
            path = path & "AlarmOther\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Open_Application_For_GET_REPORT(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            path2 = path & "GET REPORT\GET REPORT.exe"
            path = path & "GET REPORT\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Open_Application_For_START_BATCH(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            path2 = path & "start_batch\start_batch.exe"
            path = path & "start_batch\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Open_Application_For_ParameterDosing(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            Dim path2 As String
            path2 = path & "ParameterDosing\ParameterDosing.exe"
            path = path & "ParameterDosing\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)

            My.Computer.FileSystem.CopyFile(path2, path, True)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                CloseEXE(Name_Application)
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Function Open_Application_Bin(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            Dim path As String = strPath + "_Exe\"
            path = path & "RouteControl_Conveyer\" + Name_Application.Trim + ".exe"
            '   Process.Start(path, Commandline_Arguments)
            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function Open_Application_For_Parameter_Bin_Sub(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
            'Dim path2 As String
            'path2 = path & "Parameter_Current_Screw_Weight\Parameter_Current_Screw_Weight.exe"
            'path = path & "Parameter_Current_Screw_Weight\" + Name_Application.Trim + ".exe"

            Dim path As String = strPath + "_Exe\"
            path = path & "BinParameter\BinParameter.exe"
            '   Process.Start(path, Commandline_Arguments)

            If Commandline_Arguments = "" Then
                Process.Start(path)
            Else
                Process.Start(path, Commandline_Arguments)
                ' 1 1 8002 BATCHING
            End If
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    'Public Function Open_Application(Name_Application As String, Commandline_Arguments As String) As Boolean
    '    Try
    '        'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\"
    '        Dim path As String = strPath + "_Exe\"
    '        path = path & "" + Name_Application.Trim + "\" + Name_Application.Trim + ".exe"
    '        '   Process.Start(path, Commandline_Arguments)
    '        If Commandline_Arguments = "" Then
    '            Process.Start(path)
    '        Else
    '            Process.Start(path, Commandline_Arguments)
    '            ' 1 1 8002 BATCHING
    '        End If
    '        Return True
    '    Catch ex As Exception
    '        Return False
    '    End Try
    'End Function

    Public Function Open_Application(Name_Application As String, Commandline_Arguments As String) As Boolean
        Try
            Dim path As String = strPath + "_Exe\"
            path = path & "" + Name_Application.Trim + "\" + Name_Application.Trim + ".exe"
            If Commandline_Arguments = "" Then
                Process.Start(path)
                Return True
            Else
                Process.Start(path, Commandline_Arguments)
                Return True
            End If
            'Return True
        Catch ex As Exception
            MessageBox.Show("Fail to open program as location: " & strPath + "_Exe\" & Name_Application & Environment.NewLine &
                        "Error Details: " & ex.Message,
                        "Program Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    Public Function GetRoue_Location() As String()

        'Dim path As String = "" + Application.StartupPath.Substring(0, 3) + "SmartFeedmill\_Exe\_Config\RouteConfig_Client.TAT"
        Dim path As String = strPath + "_Exe\_Config\RouteConfig_Client.TAT"
        Dim ReadFile As New IniFile(path)
        Dim Location As String
        Dim location_str As String
        Dim strListLocation() As String
        For i As Integer = 0 To 20
            Location = ReadFile.GetString("Route_" + CStr(i) + "", "Location  ", "")
            If Location = "" Then GoTo X
            location_str = location_str & "|" & Location
X:
        Next
        Dim Str_long As String = location_str
        Try
            strListLocation = location_str.Split("|")
        Catch ex As Exception
            Exit Function
        End Try

        location_str = ""
        For i As Integer = 0 To strListLocation.Count - 1
            If strListLocation(i) = "" Then GoTo Z
            location_str = location_str & " c_location = '" & Replace(Trim(strListLocation(i)), "_", " ") & "' OR "
Z:
        Next
        Dim Arr_str(1) As String
        Arr_str(0) = location_str
        Arr_str(1) = Str_long
        Return Arr_str

    End Function

    Public Sub writeErr(ByVal str As String)
        Try
            MsgBox(str)
            Dim LocalPath As String = Application.StartupPath

            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            Dim d, m, y As String
            d = Today.Day
            m = Today.Month
            y = Today.Year
            LocalPath = LocalPath.Substring(0, 3)
            Dim LogPath As String = ""
            LogPath = ReturnFolderCurrentMonth("" + LocalPath + "Temp_SmartFeedmill\TAT_SMF\Error")
            Dim tmp_ProgramID As String = "TAT_SMF" ' Auto Route
            Dim tmp_ProgramTypeID As String = "MU"
            LogPath += "\" + tmp_ProgramID + "_" + tmp_ProgramTypeID + "_" + d + "" + m + "" + y + ".TXT"
            ' LogPath += "\" & y & "-" & m & "-" & d & "_Event.txt"
XX:
            My.Computer.FileSystem.WriteAllText(LogPath, Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("en-GB")) & ":::" & str & vbCrLf, True)
        Catch ex As Exception
            GoTo XX
            Dim Projext_name As String
            Projext_name = Application.ExecutablePath
            Projext_name = Projext_name.Replace(Application.StartupPath & "\", "")
            Projext_name = Projext_name.Replace(".EXE", "")
            MsgBox(Projext_name & " " & "FN: writeLogEvent  " & ex.Message)

        End Try

    End Sub

    Public Function Get_UsePath() As String
        Dim intCountIndex, i As Integer
        Dim strUsePath As String
        Dim str_chkPath() As String
        Dim tmpChkPath As String
        str_chkPath = Split(Application.StartupPath, "\")

        intCountIndex = UBound(str_chkPath) + 1

        For i = 0 To intCountIndex - 2
            strUsePath = strUsePath + str_chkPath(i) + "\"
        Next
        tmpChkPath = strUsePath & "_Config\connect_DB.txt"

        '============================================== กรณีรันจากโค้ด
        If File.Exists(tmpChkPath) = False Then
            strUsePath = ""
            For i = 0 To intCountIndex - 5
                strUsePath = strUsePath + str_chkPath(i) + "\"
            Next
        End If


        Return strUsePath

    End Function

End Class