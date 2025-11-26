Option Strict On
Option Explicit On

Imports System.Configuration
Imports System.IO
Imports System.IO.Compression
Imports System.Linq
Imports System.Security.Cryptography
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports System.Threading.Tasks

Public Class clsSelfUpdate

    ' ============ PATH CONFIG ============
    Private ReadOnly updatePath As String
    Private ReadOnly backupPath As String
    Private ReadOnly logDir As String
    Private ReadOnly stationLogPath As String

    ' ============ RUNTIME ============
    Private ReadOnly localExe As String
    Private ReadOnly exeName As String
    Private ReadOnly stationID As String
    Private ReadOnly stationIP As String
    Private ReadOnly currentExeDate As DateTime

    Private Const BACKUP_KEEP_MAX As Integer = 20
    Private Const MOVE_APPLIED_ZIP As Boolean = False
    Private Const NETWORK_TIMEOUT_MS As Integer = 3000

    ' ★ Network availability tracking
    Private _networkAvailable As Boolean = True
    Private _lastNetworkCheck As DateTime = DateTime.MinValue

    Public Sub New()
        Dim baseShare As String = ConfigurationManager.AppSettings("Share_Location")
        updatePath = Path.Combine(baseShare, "_Update")
        backupPath = Path.Combine(baseShare, "_Backup")
        logDir = Path.Combine(baseShare, "_Log")

        localExe = Application.ExecutablePath
        exeName = Path.GetFileName(localExe)

        ' ★ เก็บวันที่ของ EXE ปัจจุบัน
        Try
            currentExeDate = File.GetLastWriteTime(localExe)
        Catch
            currentExeDate = DateTime.MinValue
        End Try

        ' Station identifiers
        stationIP = GetCurrentStationIP()
        stationID = GetStableStationID()

        Dim safeIP As String = stationIP.Replace(".", "_")
        Dim safeID As String = stationID.Replace(":", "").Replace("-", "")
        stationLogPath = Path.Combine(logDir, $"{safeIP}_{safeID}.json")

        Debug.WriteLine($"[UPDATE] Station Log Path: {stationLogPath}")
        Debug.WriteLine($"[UPDATE] Current EXE Date: {currentExeDate:yyyy-MM-dd HH:mm:ss}")

        CheckNetworkAvailability()
    End Sub

    Private Sub CheckNetworkAvailability()
        Try
            If (DateTime.Now - _lastNetworkCheck).TotalSeconds < 60 Then
                Return
            End If

            _lastNetworkCheck = DateTime.Now

            Dim checkTask = Task.Run(Function() As Boolean
                                         Try
                                             Return Directory.Exists(updatePath)
                                         Catch
                                             Return False
                                         End Try
                                     End Function)

            If checkTask.Wait(NETWORK_TIMEOUT_MS) Then
                _networkAvailable = checkTask.Result
            Else
                _networkAvailable = False
                Debug.WriteLine("[UPDATE] Network check timeout")
            End If

        Catch ex As Exception
            _networkAvailable = False
            Debug.WriteLine($"[UPDATE] Network check failed: {ex.Message}")
        End Try
    End Sub

    ' ------------------------------------------------------------
    '  PUBLIC API
    ' ------------------------------------------------------------

    Public Function HasPendingUpdate() As Boolean
        Try
            CheckNetworkAvailability()
            If Not _networkAvailable Then
                Debug.WriteLine("[UPDATE] Network unavailable - skipping update check")
                Return False
            End If

            Dim z As String = PickLatestZipNotApplied()
            Dim hasPending As Boolean = Not String.IsNullOrEmpty(z)

            Try
                If hasPending Then
                    Debug.WriteLine($"[UPDATE] Pending update found: {Path.GetFileName(z)}")
                Else
                    Debug.WriteLine("[UPDATE] No pending update")
                End If
            Catch
            End Try

            Return hasPending
        Catch ex As Exception
            Try
                Debug.WriteLine($"[UPDATE ERROR] HasPendingUpdate: {ex.Message}")
            Catch
            End Try
            Return False
        End Try
    End Function

    Public Async Function StartUpdateAsync(owner As Form) As Task
        Dim backupZipPath As String = Nothing

        Try
            CheckNetworkAvailability()
            If Not _networkAvailable Then
                MessageBox.Show(owner, "Cannot connect to update location. Please check your network connection.",
                              "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Function
            End If

            Dim latestZip As String = PickLatestZipNotApplied()
            If String.IsNullOrEmpty(latestZip) Then
                MessageBox.Show(owner, "There is no newer update pack that has not been installed yet.",
                              "Update", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If

            Dim progressForm As New frmUpdateProgress()
            progressForm.Show(owner)
            progressForm.Refresh()

            progressForm.SetProgress(10, "Preparing for update...")
            Await Task.Delay(150)

            ' ★ Backup และเก็บ path
            progressForm.SetProgress(25, "Backing up current files...")
            backupZipPath = Await BackupCurrentAsync()
            EnforceBackupRetention(BACKUP_KEEP_MAX)

            progressForm.SetProgress(50, "Extracting update file...")
            Dim newExePath As String = Await ExtractZipAndLocateExeAsync(latestZip)

            ' ★ ตรวจสอบว่าไฟล์ใหม่มีวันที่ใหม่กว่า
            Dim newExeDate As DateTime = File.GetLastWriteTime(newExePath)
            Debug.WriteLine($"[UPDATE] New EXE Date: {newExeDate:yyyy-MM-dd HH:mm:ss}")

            If newExeDate <= currentExeDate Then
                Throw New InvalidOperationException($"Update file is not newer than current version. Current: {currentExeDate:yyyy-MM-dd HH:mm:ss}, Update: {newExeDate:yyyy-MM-dd HH:mm:ss}")
            End If

            progressForm.SetProgress(70, "Saving update history...")
            WriteStructuredLog(latestZip)

            If MOVE_APPLIED_ZIP Then
                Try
                    Dim appliedDir = Path.Combine(Path.GetDirectoryName(latestZip), "_Applied")
                    Directory.CreateDirectory(appliedDir)
                    Dim dest = Path.Combine(appliedDir, Path.GetFileName(latestZip))
                    If File.Exists(dest) Then
                        Dim nameNoExt = Path.GetFileNameWithoutExtension(dest)
                        Dim ext = Path.GetExtension(dest)
                        Dim alt = Path.Combine(appliedDir, $"{nameNoExt}_{DateTime.Now:yyyyMMddHHmmss}{ext}")
                        File.Move(latestZip, alt)
                    Else
                        File.Move(latestZip, dest)
                    End If
                Catch ex As Exception
                    Debug.WriteLine($"[UPDATE WARN] Move applied zip failed: {ex.Message}")
                End Try
            End If

            progressForm.SetProgress(85, "Preparing to restart the program...")
            Dim batPath As String = WriteSelfUpdateBat(newExePath, localExe, backupZipPath)

            Dim psi As New ProcessStartInfo() With {
                .FileName = "cmd.exe",
                .Arguments = "/c " & Chr(34) & batPath & Chr(34),
                .CreateNoWindow = True,
                .UseShellExecute = False,
                .WindowStyle = ProcessWindowStyle.Hidden,
                .WorkingDirectory = Path.GetDirectoryName(localExe)
            }
            Process.Start(psi)

            progressForm.SetProgress(100, "Done, closing the program...")
            Await Task.Delay(300)

            Dim mdi = TryCast(owner, MDI_FRM)
            If mdi IsNot Nothing Then
                Await mdi.RequestExitAsync(showConfirm:=False, splashForm:=progressForm)
            Else
                Application.Exit()
            End If

        Catch ex As Exception
            ' ★ แจ้งเตือนและพยายาม Rollback
            Debug.WriteLine($"[UPDATE ERROR] Update failed: {ex.Message}")

            Dim errorMsg As String = "An error occurred during the update: " & ex.Message
            If Not String.IsNullOrEmpty(backupZipPath) AndAlso File.Exists(backupZipPath) Then
                errorMsg &= vbCrLf & vbCrLf & "A backup has been saved at:" & vbCrLf & backupZipPath
                errorMsg &= vbCrLf & vbCrLf & "You can manually restore from this backup if needed."
            End If

            MessageBox.Show(owner, errorMsg, "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Function

    ' ------------------------------------------------------------
    '  CORE LOGIC - ★ เพิ่มการตรวจสอบวันที่
    ' ------------------------------------------------------------

    Private Function PickLatestZipNotApplied() As String
        Try
            If Not _networkAvailable Then
                Debug.WriteLine("[UPDATE] Network unavailable")
                Return Nothing
            End If

            Dim getFilesTask = Task.Run(Function() As String()
                                            Try
                                                If Not Directory.Exists(updatePath) Then
                                                    Return Nothing
                                                End If
                                                Return Directory.GetFiles(updatePath, "*.zip", SearchOption.TopDirectoryOnly)
                                            Catch
                                                Return Nothing
                                            End Try
                                        End Function)

            If Not getFilesTask.Wait(NETWORK_TIMEOUT_MS) Then
                Debug.WriteLine("[UPDATE] GetFiles timeout")
                _networkAvailable = False
                Return Nothing
            End If

            Dim zipFiles() As String = getFilesTask.Result
            If zipFiles Is Nothing OrElse zipFiles.Length = 0 Then
                Debug.WriteLine("[UPDATE] No ZIP files found in update folder")
                Return Nothing
            End If

            Debug.WriteLine($"[UPDATE] Found {zipFiles.Length} ZIP file(s)")

            Dim exeNameNoExt As String = Path.GetFileNameWithoutExtension(exeName)
            Dim matches As String() =
                zipFiles.Where(Function(p)
                                   Dim zipName As String = Path.GetFileName(p)
                                   Dim isMatch As Boolean = zipName.IndexOf(exeNameNoExt, StringComparison.OrdinalIgnoreCase) >= 0
                                   Debug.WriteLine($"[UPDATE] Check ZIP: {zipName} -> Match: {isMatch}")
                                   Return isMatch
                               End Function).ToArray()

            If matches.Length = 0 Then
                Debug.WriteLine($"[UPDATE] No ZIP files match exe name: {exeNameNoExt}")
                Return Nothing
            End If

            Debug.WriteLine($"[UPDATE] Found {matches.Length} matching ZIP file(s)")

            Dim appliedSigs As HashSet(Of String) = LoadAppliedSignaturesFromLog()
            Debug.WriteLine($"[UPDATE] Applied signatures count: {appliedSigs.Count}")

            ' ★ เรียงตามวันที่ล่าสุดก่อน
            Array.Sort(matches, Function(a, b) File.GetLastWriteTime(b).CompareTo(File.GetLastWriteTime(a)))

            For i As Integer = 0 To matches.Length - 1
                Dim z As String = matches(i)

                If Not IsFileReady(z) Then
                    Debug.WriteLine($"[UPDATE] Skip (file not ready): {Path.GetFileName(z)}")
                    Continue For
                End If

                Dim zSig As String = GetZipSignature(z)
                Dim zDate As DateTime = File.GetLastWriteTime(z)

                ' ★ ตรวจสอบว่า ZIP ต้องใหม่กว่า EXE ปัจจุบัน
                If zDate <= currentExeDate Then
                    Debug.WriteLine($"[UPDATE] Skip (not newer): {Path.GetFileName(z)} - ZIP date: {zDate:yyyy-MM-dd HH:mm:ss} vs EXE: {currentExeDate:yyyy-MM-dd HH:mm:ss}")
                    Continue For
                End If

                Dim isApplied As Boolean = appliedSigs.Contains(zSig)

                Debug.WriteLine($"[UPDATE] Checking ZIP [{i}]: {Path.GetFileName(z)}")
                Debug.WriteLine($"  - Date: {zDate:yyyy-MM-dd HH:mm:ss}")
                Debug.WriteLine($"  - Signature: {zSig.Substring(0, Math.Min(16, zSig.Length))}...")
                Debug.WriteLine($"  - Is Applied: {isApplied}")
                Debug.WriteLine($"  - Is Newer: True")

                If Not isApplied Then
                    Debug.WriteLine($"[UPDATE] ✓ Selected: {Path.GetFileName(z)}")
                    Return z
                End If
            Next

            Debug.WriteLine("[UPDATE] No eligible update found (all ZIPs already applied or not newer)")
            Return Nothing

        Catch ex As Exception
            Debug.WriteLine($"[UPDATE ERROR] PickLatestZipNotApplied: {ex.Message}")
            Debug.WriteLine($"[UPDATE ERROR] Stack: {ex.StackTrace}")
            Return Nothing
        End Try
    End Function

    Private Function IsFileReady(path As String) As Boolean
        Try
            Using fs As New FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)
                Return fs.Length > 0
            End Using
        Catch
            Return False
        End Try
    End Function

    ' ------------------------------------------------------------
    '  BACKUP - ★ คืนค่า path ของ backup
    ' ------------------------------------------------------------

    Private Shared Function MakeSafeToken(value As String, fallback As String) As String
        If String.IsNullOrWhiteSpace(value) Then Return fallback
        Dim invalid() As Char = IO.Path.GetInvalidFileNameChars()
        Dim sb As New System.Text.StringBuilder(value.Length)
        For Each ch As Char In value
            If Array.IndexOf(invalid, ch) >= 0 Then
                sb.Append("_"c)
            Else
                sb.Append(ch)
            End If
        Next
        Return sb.ToString()
    End Function

    Private Async Function BackupCurrentAsync() As Threading.Tasks.Task(Of String)
        Return Await Threading.Tasks.Task.Run(
        Function() As String
            If Not Directory.Exists(backupPath) Then
                Directory.CreateDirectory(backupPath)
            End If

            Dim stamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss", Globalization.CultureInfo.InvariantCulture)
            Dim exeNoExt As String = Path.GetFileNameWithoutExtension(localExe)

            Dim safeIP As String = MakeSafeToken(stationIP.Replace(".", ""), "NOIP")
            Dim safeIDRaw As String = stationID.Replace(":", "").Replace("-", "")
            Dim safeID As String = MakeSafeToken(safeIDRaw, "NOID")
            Dim safeProg As String = MakeSafeToken(exeNoExt, "APP")

            Dim backupZipName As String = $"{safeIP}_{safeID}_{safeProg}_{stamp}.zip"
            Dim backupZip As String = Path.Combine(backupPath, backupZipName)

            Using fs As New FileStream(backupZip, FileMode.CreateNew, FileAccess.Write, FileShare.None)
                Using archive As New ZipArchive(fs, ZipArchiveMode.Create)
                    ' EXE
                    Dim entryExe = archive.CreateEntry(Path.GetFileName(localExe), CompressionLevel.Optimal)
                    Using es As Stream = entryExe.Open()
                        Using src As New FileStream(localExe, FileMode.Open, FileAccess.Read, FileShare.Read)
                            src.CopyTo(es)
                        End Using
                    End Using

                    ' .config (ถ้ามี)
                    Dim cfg As String = localExe & ".config"
                    If File.Exists(cfg) Then
                        Dim entryCfg = archive.CreateEntry(Path.GetFileName(cfg), CompressionLevel.Optimal)
                        Using es As Stream = entryCfg.Open()
                            Using src As New FileStream(cfg, FileMode.Open, FileAccess.Read, FileShare.Read)
                                src.CopyTo(es)
                            End Using
                        End Using
                    End If
                End Using
            End Using

            Debug.WriteLine($"[UPDATE] Backup created: {backupZip}")
            Return backupZip
        End Function)
    End Function

    Private Sub EnforceBackupRetention(keepCount As Integer)
        Try
            If keepCount < 1 Then Exit Sub
            If Not Directory.Exists(backupPath) Then Exit Sub

            Dim exeNoExt As String = Path.GetFileNameWithoutExtension(localExe)
            Dim safeIP As String = MakeSafeToken(stationIP.Replace(".", ""), "NOIP")
            Dim safeID As String = MakeSafeToken(stationID.Replace(":", "").Replace("-", ""), "NOID")
            Dim safeProg As String = MakeSafeToken(exeNoExt, "APP")

            Dim prefix As String = $"{safeIP}_{safeID}_{safeProg}_"

            Dim files As List(Of FileInfo) =
                Directory.EnumerateFiles(backupPath, "*.zip", SearchOption.TopDirectoryOnly).
                    Where(Function(p) Path.GetFileName(p).StartsWith(prefix, StringComparison.OrdinalIgnoreCase)).
                    Select(Function(p) New FileInfo(p)).
                    OrderByDescending(Function(fi) fi.CreationTimeUtc).
                    ToList()

            If files.Count <= keepCount Then Exit Sub

            For Each fi As FileInfo In files.Skip(keepCount)
                Try : fi.Delete() : Catch : End Try
            Next
        Catch
        End Try
    End Sub

    Private Async Function ExtractZipAndLocateExeAsync(zipPath As String) As Threading.Tasks.Task(Of String)
        Return Await Threading.Tasks.Task.Run(Function() As String
                                                  Dim tempDir As String = Path.Combine(Path.GetTempPath(), "SCADA_Update_" & Guid.NewGuid().ToString("N"))
                                                  Directory.CreateDirectory(tempDir)

                                                  Using archive = ZipFile.OpenRead(zipPath)
                                                      For Each entry As ZipArchiveEntry In archive.Entries
                                                          Dim destPath As String = Path.Combine(tempDir, entry.FullName)
                                                          If String.IsNullOrEmpty(entry.Name) Then
                                                              Directory.CreateDirectory(destPath)
                                                          Else
                                                              Dim destDir As String = Path.GetDirectoryName(destPath)
                                                              If Not String.IsNullOrEmpty(destDir) Then Directory.CreateDirectory(destDir)
                                                              entry.ExtractToFile(destPath, True)
                                                          End If
                                                      Next
                                                  End Using

                                                  Dim exeNew As String =
                                                      Directory.GetFiles(tempDir, "*.exe", SearchOption.AllDirectories).
                                                          OrderByDescending(Function(p) File.GetLastWriteTime(p)).
                                                          FirstOrDefault()

                                                  If String.IsNullOrEmpty(exeNew) Then
                                                      Throw New IOException("No .exe file found within the update pack.")
                                                  End If

                                                  Return exeNew
                                              End Function)
    End Function

    ' ★ เพิ่ม backupPath สำหรับ Rollback
    Private Function WriteSelfUpdateBat(srcExe As String, dstExe As String, backupPath As String) As String
        Dim batPath As String = Path.Combine(Path.GetTempPath(), "selfupdate_" & Guid.NewGuid().ToString("N") & ".bat")
        Dim batLog As String = Path.Combine(Path.GetTempPath(), "update_log.txt")
        Dim currentPID As Integer = Process.GetCurrentProcess().Id

        Dim dstNew As String = dstExe & ".new"
        If File.Exists(dstNew) Then File.Delete(dstNew)
        File.Copy(srcExe, dstNew, True)

        Dim dstDir As String = Path.GetDirectoryName(dstExe)

        Dim sb As New Text.StringBuilder()
        sb.AppendLine("@echo off")
        sb.AppendLine("chcp 65001 >nul")
        sb.AppendLine("setlocal enableextensions")
        sb.AppendLine("set EXE=" & Chr(34) & dstExe & Chr(34))
        sb.AppendLine("set SRC=" & Chr(34) & dstNew & Chr(34))
        sb.AppendLine("set PID=" & currentPID.ToString())
        sb.AppendLine("set LOG=" & Chr(34) & batLog & Chr(34))
        sb.AppendLine("set EXEDIR=" & Chr(34) & dstDir & Chr(34))
        If Not String.IsNullOrEmpty(backupPath) Then
            sb.AppendLine("set BACKUP=" & Chr(34) & backupPath & Chr(34))
        End If
        sb.AppendLine()

        sb.AppendLine("echo ===== UPDATE START ===== >> %LOG%")
        sb.AppendLine("echo Waiting for PID %PID% to exit... >> %LOG%")
        sb.AppendLine(":waitloop")
        sb.AppendLine("tasklist /FI ""PID eq %PID%"" 2>nul | find ""%PID%"" >nul")
        sb.AppendLine("if %ERRORLEVEL%==0 (")
        sb.AppendLine("  timeout /t 1 /nobreak >nul")
        sb.AppendLine("  goto :waitloop")
        sb.AppendLine(")")
        sb.AppendLine()

        sb.AppendLine("timeout /t 1 /nobreak >nul")

        sb.AppendLine("pushd %EXEDIR%")
        sb.AppendLine("if not %ERRORLEVEL%==0 (")
        sb.AppendLine("  echo [WARN] pushd failed, EXEDIR=%EXEDIR% >> %LOG%")
        sb.AppendLine(")")

        sb.AppendLine("echo Copying new version... >> %LOG%")
        sb.AppendLine("set RETRIES=0")
        sb.AppendLine(":copytry")
        sb.AppendLine("copy /Y %SRC% %EXE% >> %LOG% 2>&1")
        sb.AppendLine("if not %ERRORLEVEL%==0 (")
        sb.AppendLine("  set /a RETRIES+=1")
        sb.AppendLine("  if %RETRIES% LSS 5 (")
        sb.AppendLine("    echo [WARN] Copy failed, retry %RETRIES% >> %LOG%")
        sb.AppendLine("    timeout /t 1 /nobreak >nul")
        sb.AppendLine("    goto :copytry")
        sb.AppendLine("  ) else (")
        sb.AppendLine("    echo [ERROR] Copy failed after retries! >> %LOG%")
        sb.AppendLine("    echo [ERROR] Backup available at: %BACKUP% >> %LOG%")
        sb.AppendLine("    goto :end")
        sb.AppendLine("  )")
        sb.AppendLine(")")
        sb.AppendLine()

        sb.AppendLine("timeout /t 1 /nobreak >nul")
        sb.AppendLine("echo Launching new EXE... >> %LOG%")
        sb.AppendLine("start ""SCADA"" /D %EXEDIR% %EXE%")

        sb.AppendLine(":end")
        sb.AppendLine("popd >nul 2>&1")
        sb.AppendLine("echo ===== UPDATE END ===== >> %LOG%")
        sb.AppendLine("endlocal")
        sb.AppendLine("timeout /t 2 /nobreak >nul")
        sb.AppendLine("del /f /q ""%~f0""")

        File.WriteAllText(batPath, sb.ToString(), Text.Encoding.Default)
        Return batPath
    End Function

    ' ------------------------------------------------------------
    '  LOG - ★ เพิ่ม Username
    ' ------------------------------------------------------------

    Private Sub WriteStructuredLog(zipPath As String)
        Try
            Dim fi As New FileInfo(zipPath)
            Dim base As String = Path.GetFileNameWithoutExtension(zipPath)
            Dim parts() As String = base.Split("_"c)

            Dim programName As String = Path.GetFileNameWithoutExtension(localExe)
            Dim byName As String = If(parts.Length >= 2, parts(1), "")
            Dim detail As String = If(parts.Length >= 3, String.Join("_", parts.Skip(2)), "")
            Dim zipSig As String = GetZipSignature(zipPath)

            ' ★ เพิ่ม Windows Username
            Dim currentUser As String = Environment.UserName
            Try
                currentUser = Environment.UserDomainName & "\" & Environment.UserName
            Catch
                ' ถ้าไม่มี domain ใช้ username อย่างเดียว
            End Try

            Dim newEntry As New JObject From {
                {"Program", programName},
                {"ZipName", fi.Name},
                {"ZipDate", fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")},
                {"ZipSize", fi.Length},
                {"ZipSig", zipSig},
                {"By", byName},
                {"Detail", detail},
                {"StationID", stationID},
                {"StationIP", stationIP},
                {"ComputerName", Environment.MachineName},
                {"UpdatedBy", currentUser},
                {"Applied", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}
            }

            If Not Directory.Exists(logDir) Then Directory.CreateDirectory(logDir)

            Dim entries As JArray = LoadStationLog()

            Dim exists As Boolean = entries.OfType(Of JObject)().Any(
                Function(item)
                    Return String.Equals(item("ZipSig")?.ToString(), zipSig, StringComparison.OrdinalIgnoreCase)
                End Function)

            If Not exists Then
                entries.Add(newEntry)
                File.WriteAllText(stationLogPath, entries.ToString(Formatting.Indented), Text.Encoding.UTF8)
                Debug.WriteLine($"[UPDATE] Log written by {currentUser}: {stationLogPath}")
            Else
                Debug.WriteLine($"[UPDATE] Entry already exists, skipped")
            End If

        Catch ex As Exception
            Debug.WriteLine($"[UPDATE ERROR] WriteStructuredLog: {ex.Message}")
            Debug.WriteLine($"[UPDATE ERROR] Stack: {ex.StackTrace}")
        End Try
    End Sub

    Private Function LoadStationLog() As JArray
        Try
            If Not File.Exists(stationLogPath) Then Return New JArray()
            Dim jsonText As String = File.ReadAllText(stationLogPath, Text.Encoding.UTF8)
            If String.IsNullOrWhiteSpace(jsonText) Then Return New JArray()
            Return JArray.Parse(jsonText)
        Catch ex As Exception
            Debug.WriteLine($"[UPDATE ERROR] LoadStationLog: {ex.Message}")
            Return New JArray()
        End Try
    End Function

    Private Function LoadAppliedSignaturesFromLog() As HashSet(Of String)
        Dim setSig As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Try
            Dim logArray As JArray = LoadStationLog()

            Debug.WriteLine($"[UPDATE] Current Station: {stationIP} / {stationID}")
            Debug.WriteLine($"[UPDATE] Current Program: {Path.GetFileNameWithoutExtension(localExe)}")

            For Each item As JObject In logArray.OfType(Of JObject)()
                Try
                    Dim sig As String = item("ZipSig")?.ToString()
                    If Not String.IsNullOrWhiteSpace(sig) Then
                        setSig.Add(sig)
                        Debug.WriteLine($"[UPDATE] Applied: {item("ZipName")} [{sig.Substring(0, Math.Min(16, sig.Length))}...]")
                    End If
                Catch
                End Try
            Next

            Debug.WriteLine($"[UPDATE] Total applied: {setSig.Count}")

        Catch ex As Exception
            Debug.WriteLine($"[UPDATE ERROR] LoadAppliedSignaturesFromLog: {ex.Message}")
        End Try
        Return setSig
    End Function

    ' ------------------------------------------------------------
    '  STATION IDENTIFICATION
    ' ------------------------------------------------------------

    Private Function GetStableStationID() As String
        Try
            Dim nics = Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().
                Where(Function(nic)
                          Return nic.OperationalStatus = Net.NetworkInformation.OperationalStatus.Up AndAlso
                                 nic.NetworkInterfaceType <> Net.NetworkInformation.NetworkInterfaceType.Loopback AndAlso
                                 nic.GetPhysicalAddress().ToString().Length > 0
                      End Function).
                OrderBy(Function(nic) nic.Name).
                ToList()

            If nics.Count > 0 Then
                Dim mac As String = nics(0).GetPhysicalAddress().ToString()
                Dim computerName As String = Environment.MachineName
                Return $"{computerName}_{mac}"
            End If
        Catch
        End Try

        Return $"{Environment.MachineName}_{GetCurrentStationIP()}"
    End Function

    Private Function GetCurrentStationIP() As String
        Try
            Dim ipV4 = Net.Dns.GetHostAddresses(Net.Dns.GetHostName()).
                Where(Function(ip) ip.AddressFamily = Net.Sockets.AddressFamily.InterNetwork).
                FirstOrDefault()
            If ipV4 IsNot Nothing Then Return ipV4.ToString()
        Catch
        End Try
        Return "Unknown"
    End Function

    Private Function GetZipSignature(zipPath As String) As String
        Try
            Return ComputeSHA256Hex(zipPath)
        Catch ex As Exception
            Debug.WriteLine($"[UPDATE ERROR] GetZipSignature: {ex.Message}")
            Dim fi As New FileInfo(zipPath)
            Return $"{fi.Name}_{fi.LastWriteTimeUtc.Ticks}_{fi.Length}"
        End Try
    End Function

    Private Function ComputeSHA256Hex(filePath As String) As String
        Using sha As SHA256 = SHA256.Create()
            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                Dim bytes = sha.ComputeHash(fs)
                Return BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant()
            End Using
        End Using
    End Function

    Public Function GetLatestUpdateFile() As String
        Try
            CheckNetworkAvailability()
            If Not _networkAvailable Then
                Return Nothing
            End If
            Return PickLatestZipNotApplied()
        Catch ex As Exception
            Try
                Debug.WriteLine($"[UPDATE ERROR] GetLatestUpdateFile: {ex.Message}")
            Catch
            End Try
            Return Nothing
        End Try
    End Function

End Class