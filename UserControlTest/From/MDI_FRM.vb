Option Strict On
Option Explicit On
Option Infer Off

Imports System.Globalization
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.IO
Imports System.Diagnostics
Imports System.Threading.Tasks
Imports System.Configuration
Imports System.Net
Imports System.Net.Sockets
Imports System.Linq
Imports TAT_CtrlRoute
Imports uPLibrary.Networking.M2Mqtt

' =============== PATH NOTES ===============
' - FIXED BUG VARIABLE DOUBLE READ MQTT
' - UPDATE CONFIRM TEST IO WITH NEW PROCEDURE
' - UPDATE frm_login WITH NEW PERMISSION
' - ADD ASSEMBLY CREATER VERSION
' ==========================================

' MDI FORM UPDATE BY: NATTHAPHONG 07/11/2025

Public Class MDI_FRM
#Region "# BUTTON OTHER"
    Private Sub ToolBarToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ToolBarToolStripMenuItem.Click
        Me.ToolStrip_Top.Visible = Me.ToolBarToolStripMenuItem.Checked
    End Sub

    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        For Each child As Form In Me.MdiChildren
            Try : child.Close() : Catch : End Try
        Next
    End Sub

    Private Sub CascadeToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub
    Private Sub TileHorizontalToolStripMenuItem1_Click(sender As Object, e As EventArgs)
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub
#End Region
#Region "# VARIABLE"
    Public Frm_arr As ArrayList

    '==== USE LOGIN STATUS ===
    Public Route_Status As Boolean
    Public Alarm_Status As Boolean
    Public Main_Scada As Boolean

    '==== FLAG AUTO CALL EXE ===
    Public Call_Route_EXE As Boolean
    Public Route_Batching_Status As Boolean
    Public Route_Conveyer_Status As Boolean

    Public Call_StartBatch_EXE As Boolean
    Public Start_Batch_Status As Boolean

    Public Get_Report_Status As Boolean

    Private Const EXE_PREFIX As String = "TAT01_"
    Private Const EXE_SUFFIX_CONVEYER As String = "_ROUTECONTROL_Conveyer"
    Private Const EXE_SUFFIX_BATCHING As String = "_ROUTECONTROL_Batching"

    Public frm_Alarm_Des As Alarm_Des
    Public frmAlarm_Des_New As frmAlarm_Description

    Private frm1 As Boolean
    Private m_ChildFormNumber As Integer
    Private ReadOnly io As clsIO = New clsIO()
    Public Add_source As String
    Public Add_source_Code As String
    Public Add_Destination As String

    Private Error_Check As Boolean
    Private ReadOnly LogError As clsIO = New clsIO()

    Private client As MqttClient
    Private KeyShow As Boolean

    Public HASP As Cls_Hasp = New Cls_Hasp()
    Public HASP_Status As Integer
    Public HASP_Check_Time As DateTime
    Public CheckHasp As CheckHardlock.CheckHardlock.clsHardlock = New CheckHardlock.CheckHardlock.clsHardlock()

    Private tmpDatetimeForce As Date
    Private tmpMinute As Long
    Private tmpCheckHardLock As Boolean = False

    Public CnBatching As clsDB

    Private locationValue As String

    Private _isLoggedIn As Boolean = False
    Private _canProductionTime As Boolean
    Private _canJobAssignment As Boolean
    Private _canMainScada As Boolean
    Private _canAlarm As Boolean
    Private _canRoute As Boolean
    Private _canStartBatch As Boolean
#End Region
#Region "# FORM LOAD NEW"
    Private Sub EnsureCnBatching()
        If CnBatching Is Nothing Then
            CnBatching = New clsDB(
                Batching_Conf.Name,
                Batching_Conf.User,
                Batching_Conf.Password,
                Batching_Conf.IPAddress,
                Batching_Conf.Connection_Type
            )
        End If
    End Sub

    Public Sub New()
        InitializeComponent()

        Me.IsMdiContainer = True
        Me.WindowState = FormWindowState.Normal
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.ShowInTaskbar = True

        EnableMdiDoubleBuffering()

        Dim ensureHandleCreated As IntPtr = Me.Handle
    End Sub

    Private Sub EnableMdiDoubleBuffering()
        For Each ctl As Control In Me.Controls
            If TypeOf ctl Is MdiClient Then
                Dim p As PropertyInfo = GetType(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic Or BindingFlags.Instance)
                If p IsNot Nothing Then
                    p.SetValue(ctl, True, Nothing)
                End If
                Exit For
            End If
        Next

        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint Or ControlStyles.UserPaint, True)
        Me.UpdateStyles()
    End Sub

    Private Async Sub MDIParent1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Try
            DisableControlsDuringLoad(True)

            Await InitializeAsync().ConfigureAwait(True)
            ShowPagesAfterShown()

            DisableControlsDuringLoad(False)

        Catch ex As Exception
            Try : LogError.writeErr("Load/InitializeAsync: " & ex.ToString()) : Catch : End Try
            MessageBox.Show("Failed to initialize: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
        Finally
            DisableControlsDuringLoad(False)
        End Try
    End Sub

    ' ===== Method =====
    Private Sub DisableControlsDuringLoad(disable As Boolean)
        Try
            ' Disable/Enable
            If ToolStrip_Top IsNot Nothing Then
                ToolStrip_Top.Enabled = Not disable
            End If

            ' Loading cursor
            If disable Then
                Me.UseWaitCursor = True
                Me.Cursor = Cursors.WaitCursor
            Else
                Me.UseWaitCursor = False
                Me.Cursor = Cursors.Default
            End If

        Catch ex As Exception
            Try : LogError.writeErr("DisableControlsDuringLoad: " & ex.ToString()) : Catch : End Try
        End Try
    End Sub

    Public Async Function InitializeAsync(Optional ByVal splash As LOAD = Nothing) As Task
        Try
            SafeInvoke(Sub()
                           Frm_arr = New ArrayList()

                           If frm_Page_2 IsNot Nothing Then
                               frm_Page_2.MdiParent = Me
                               Frm_arr.Add(frm_Page_2)
                           End If

                           If frm_Page_1 IsNot Nothing Then
                               frm_Page_1.MdiParent = Me
                               Frm_arr.Add(frm_Page_1)
                           End If

                           Dim Show_EditLine As Boolean
                           Boolean.TryParse(ConfigurationManager.AppSettings("Mode_EditLine"), Show_EditLine)

                           If frm_Page_1 IsNot Nothing AndAlso Not frm_Page_1.IsDisposed Then
                               frm_Page_1.btnLineManage.Visible = Show_EditLine
                           End If

                           If frm_Page_2 IsNot Nothing AndAlso Not frm_Page_2.IsDisposed Then
                               frm_Page_2.btnLineManage.Visible = Show_EditLine
                           End If

                           Close_EXE()
                       End Sub)

            Await Task.Run(
                Sub()
                    Try
                        SafeInvoke(Sub() splash?.SetStatus("Record the program start event..."))
                        SafeInvoke(Sub() splash?.SetProgress(15))

                        Dim strLogMsg As String =
                                       "Open Application " & vbCrLf &
                                       "NAME : " & Assembly.GetExecutingAssembly().GetName().Name & vbCrLf &
                                       "APPLICATION DATE : " & FileDateTime(Application.ExecutablePath) & vbCrLf &
                                       "PATH : " & Application.StartupPath
                        WriteToEventLog(strLogMsg, 1000, 1, EventLogEntryType.Information)

                        ' UI text
                        SafeInvoke(Sub()
                                       splash?.SetStatus("Setup Components...")
                                       splash?.SetProgress(25)

                                       btn_PRODUCTION_TIME.Enabled = False
                                       btn_JOB_ASSIGNMENT.Enabled = False

                                       Dim exeDate As String = FileDateTime(Application.ExecutablePath).ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.GetCultureInfo("en-GB"))

                                       Me.Text = $"{Application.ProductName} [{Application.ProductVersion}] [{Application.CompanyName}] [{exeDate}]"
                                       Me.lblIPComputer.Text = "IP : " & GetIPAddress()
                                   End Sub)

                        ' Read config
                        SafeInvoke(Sub() splash?.SetStatus("Read Config..."))
                        MyConfigApp()

                        ' Connect DB
                        SafeInvoke(Sub()
                                       splash?.SetStatus("Connect to database...")
                                       splash?.SetProgress(45)
                                   End Sub)

                        CnBatching = New clsDB(
                                       Batching_Conf.Name,
                                       Batching_Conf.User,
                                       Batching_Conf.Password,
                                       Batching_Conf.IPAddress,
                                       Batching_Conf.Connection_Type
                                   )

                        Dim testConnect As Boolean = CnBatching.TestConnection()

                        Do While Not testConnect
                            Dim msg As String =
                                           "Cannot connect to Database " & Batching_Conf.Name.ToUpperInvariant() & vbCrLf &
                                           "IP Address " & Batching_Conf.IPAddress & vbCrLf & vbCrLf &
                                           "Please check your network connection or the configuration file 'DB_Config' and try again." & vbCrLf & vbCrLf &
                                           "Press [OK] to exit, or [Cancel] to reconnect."

                            Dim result As DialogResult = DialogResult.None

                            SafeInvoke(Sub()
                                           Dim owner As IWin32Window = TryCast(splash, IWin32Window)
                                           If owner Is Nothing Then owner = Me
                                           result = MessageBox.Show(owner, msg,
                                                                               "Connection Database Error",
                                                                               MessageBoxButtons.OKCancel,
                                                                               MessageBoxIcon.[Error])
                                       End Sub)

                            If result = DialogResult.OK Then
                                SafeInvoke(Sub() ExitEndProcess())
                                Return
                            End If

                            testConnect = CnBatching.TestConnection()
                        Loop

                        ' I/O test
                        SafeInvoke(Sub()
                                       splash?.SetStatus("Load I/O test mode...")
                                       splash?.SetProgress(60)
                                   End Sub)
                        func_get_mode_testIO(CnBatching)
                        func_get_Option_ConfirmtestIO(CnBatching)

                        ' Language
                        SafeInvoke(Sub()
                                       splash?.SetStatus("Load language...")
                                       splash?.SetProgress(70)
                                   End Sub)

                        structApp_Language = Get_LanguageConfig("MAIN_SCADA", CnBatching)

                        SafeInvoke(Sub()
                                       prepare_language_item(tsCboLanguage, CnBatching)
                                       tsCboLanguage.Text = structApp_Language.strLang_Main
                                   End Sub)

                        ' Rendering
                        SafeInvoke(Sub()
                                       splash?.SetStatus("Rendering Objects...")
                                       splash?.SetProgress(80)
                                   End Sub)

                        io.writeLogEvent(Application.StartupPath, vbNewLine & vbNewLine & "##################################### START PROGRAM #####################################")

                        ' Icon + finalize UI
                        SafeInvoke(Sub()
                                       Me.Icon = My.Resources.SmartFeedmill
                                       tsCboLanguage.Text = structApp_Language.strLang_Main
                                       Close_EXE()
                                       splash?.SetProgress(90)
                                   End Sub)

                    Catch ex As Exception
                        If Error_Check Then Return
                        Error_Check = True

                        Dim strMessage As String =
                                       "Error Number : " & Err.Number & " [" & Me.Name & "]" & vbNewLine &
                                       "Error Description : " & ex.Message & vbCrLf &
                                       "Error at : " & ex.StackTrace

                        Try : LogError.writeErr(strMessage) : Catch : End Try
                        Throw
                    End Try
                End Sub).ConfigureAwait(True)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub SafeInvoke(ByVal action As Action)
        If action Is Nothing Then Return
        If Me.IsHandleCreated AndAlso Me.InvokeRequired Then
            Me.Invoke(action)
        Else
            action()
        End If
    End Sub

    Private Sub ExitEndProcess()
        Try : Close_EXE() : Catch : End Try
        Try : Process.GetCurrentProcess().Kill() : Catch : End Try
    End Sub

    Public Sub ShowPagesAfterShown()
        Try
            If frm_Page_1 IsNot Nothing AndAlso Not frm_Page_1.IsDisposed Then
                With frm_Page_1
                    .StartPosition = FormStartPosition.Manual
                    .Location = New Point(0, 0)
                    .WindowState = FormWindowState.Normal
                    .Show()
                    .BringToFront()
                End With
            End If

            Me.PerformLayout()
            Me.Refresh()

        Catch ex As Exception
            Try : LogError.writeErr("ShowPagesAfterShown: " & ex.ToString()) : Catch : End Try
        End Try
    End Sub

    Private Function GetIPAddress() As String
        Try
            Dim host As IPHostEntry = Dns.GetHostEntry(Dns.GetHostName())
            For Each ip As IPAddress In host.AddressList
                If ip.AddressFamily = AddressFamily.InterNetwork AndAlso Not IPAddress.IsLoopback(ip) Then
                    Return ip.ToString()
                End If
            Next
        Catch ex As Exception
            Try : LogError.writeErr("GetIPAddress: " & ex.ToString()) : Catch : End Try
        End Try
        Return String.Empty
    End Function
#End Region
#Region "# LOG ON EVENT"
    Public Sub Is_Logon()
        EnsureCnBatching()
        If Not CnBatching.TestConnection() Then
            MessageBox.Show("Database Not Connected. Please check network or configuration file.", "DB Error", MessageBoxButtons.OK, MessageBoxIcon.[Error])
            Exit Sub
        End If

        frmAlarm_Des_New = New frmAlarm_Description({frm_Page_1, frm_Page_2}, CnBatching)
        frmAlarm_Des_New.Show()

        Try
            Dim routeNos As List(Of String) = GetRouteNos()

            ' เคลียร์ประวัติการเปิด EXE ล่าสุด
            lastOpenedExeNames_Conveyer.Clear()
            lastOpenedExeNames_Batching.Clear()

            ' อ่าน App.config สำหรับเรียก Route
            Boolean.TryParse(ConfigurationManager.AppSettings("Call_Route"), Call_Route_EXE)

            ' ตรวจว่ามี ctrlMixer_ / CtrlMixer_ บนฟอร์มหรือไม่ -> ถ้ามีให้เปิดโหมด Batching
            Route_Batching_Status = FormsHaveMixer(New Form() {frm_Page_1, frm_Page_2})

            If Call_Route_EXE Then
                If Route_Batching_Status Then
                    ' เปิดโปรเซสฝั่ง Batching
                    For Each rn As String In routeNos
                        Dim exeName As String = BuildExeNameFromRoute_Batching(rn)
                        If String.IsNullOrWhiteSpace(exeName) Then Continue For
                        Dim param As String = BuildRouteParam(rn)
                        io.Open_Application_For_Route_Batching(exeName, param)
                        lastOpenedExeNames_Batching.Add(exeName)
                    Next

                Else
                    ' เปิดโปรเซสฝั่ง Conveyer
                    For Each rn As String In routeNos
                        Dim exeName As String = BuildExeNameFromRoute_Conveyer(rn)
                        If String.IsNullOrWhiteSpace(exeName) Then Continue For
                        Dim param As String = BuildRouteParam(rn)
                        io.Open_Application_For_Route(exeName, param)
                        lastOpenedExeNames_Conveyer.Add(exeName)
                    Next
                End If
            End If

            Custom_CloseApp()
            Custom_OpenApp()

        Catch ex As Exception
            If Error_Check Then Exit Sub
            Error_Check = True
            Dim strMessage As String =
            "Error Number :  " & Err.Number & " [" & Me.Name & "]" & vbNewLine &
            "Error Description :  " & ex.Message & vbCrLf &
            "Error at : " & ex.StackTrace
            Try : LogError.writeErr(strMessage) : Catch : End Try
        End Try
    End Sub
#End Region
#Region "# GET ROUTE PROPERTIES"
    ' เก็บชื่อ exe ที่เปิดล่าสุด แยก Conveyer / Batching
    Private ReadOnly lastOpenedExeNames_Conveyer As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly lastOpenedExeNames_Batching As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    ' --------- Utilities: ค้นหา/วนทุกคอนโทรลชนิดที่ระบุ ----------
    Private Iterator Function Descendants(Of T As Control)(root As Control) As IEnumerable(Of T)
        For Each child As Control In root.Controls
            If TypeOf child Is T Then
                Yield DirectCast(child, T)
            End If
            For Each d As T In Descendants(Of T)(child)
                Yield d
            Next
        Next
    End Function

    ' อ่าน RouteNo จาก ctrlRouteConveyer_ เท่านั้น (เลข route ใช้แหล่งเดียว)
    Private Function TryGetRouteNo(ctrl As Control, ByRef route As String) As Boolean
        route = Nothing
        If ctrl Is Nothing Then Return False

        If TypeOf ctrl Is ctrlRouteConveyer_ Then
            Dim c As ctrlRouteConveyer_ = DirectCast(ctrl, ctrlRouteConveyer_)
            Dim rnValue As [Enum] = c.RouteNo
            Dim rn As String = [Enum].GetName(GetType(ctrlRouteConveyer_.ROUTE_NO), rnValue)
            If Not String.IsNullOrWhiteSpace(rn) Then
                route = rn.Trim()
                Return True
            End If
        End If
        Return False
    End Function

    ' ตรวจว่ามี Mixer ไหม
    Private Function FormsHaveMixer(forms As IEnumerable(Of Form)) As Boolean
        If forms Is Nothing Then Return False
        For Each f As Form In forms
            If f Is Nothing OrElse f.IsDisposed Then Continue For
            For Each ctl As Control In f.Controls
                If HasMixerRecursive(ctl) Then Return True
            Next
        Next
        Return False
    End Function

    Private Function HasMixerRecursive(root As Control) As Boolean
        If root Is Nothing Then Return False
        Dim tName As String = root.GetType().Name
        If String.Equals(tName, "ctrlMixer_", StringComparison.OrdinalIgnoreCase) Then Return True
        For Each c As Control In root.Controls
            If HasMixerRecursive(c) Then Return True
        Next
        Return False
    End Function

    ' เก็บ RouteNo ทั้งหมดจากคอนโทรล
    Private Sub CollectRoutes(ctrl As Control, acc As HashSet(Of String))
        If ctrl Is Nothing Then Return
        Dim rn As String = Nothing
        If TryGetRouteNo(ctrl, rn) Then acc.Add(rn)
        For Each child As Control In ctrl.Controls
            CollectRoutes(child, acc)
        Next
    End Sub

    Private Function GetRouteNosFromForms(forms As IEnumerable(Of Form)) As List(Of String)
        Dim hs As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        If forms Is Nothing Then Return New List(Of String)()
        For Each f As Form In forms
            If f Is Nothing OrElse f.IsDisposed Then Continue For
            For Each c As Control In f.Controls
                CollectRoutes(c, hs)
            Next
        Next
        Return hs.ToList()
    End Function

    Private Function GetRouteNos() As List(Of String)
        Dim forms As Form() = New Form() {frm_Page_1, frm_Page_2}
        Return GetRouteNosFromForms(forms)
    End Function

    Private Function GetRouteLocationsFromForm(root As Control) As List(Of String)
        Dim results As New List(Of String)()
        Dim seen As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

        For Each c As ctrlRouteConveyer_ In Descendants(Of ctrlRouteConveyer_)(root)
            Try
                Dim raw As String = c.mqtt_selectroute_config_.LOCATION
                If String.IsNullOrWhiteSpace(raw) Then Continue For

                If Route_Batching_Status Then
                    Dim token As String = raw.ToUpperInvariant()
                    Dim sql As String =
                    "SELECT TOP 1 station_name " &
                    "FROM thaisia.motor_station " &
                    "WHERE c_class = '" & token & "'"

                    Dim dt As DataTable = CnRoute.ExecuteDataTable(sql)

                    Dim toAdd As String = token
                    If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                        Dim locVal As Object = dt.Rows(0)("station_name")
                        If locVal IsNot Nothing AndAlso locVal IsNot DBNull.Value Then
                            Dim val As String = Convert.ToString(locVal)
                            toAdd = val.Trim().Replace(" "c, "_"c).ToUpperInvariant()
                        End If
                    End If

                    If toAdd.Length > 0 AndAlso seen.Add(toAdd) Then
                        results.Add(toAdd)
                    End If
                Else
                    Dim token As String = raw.Trim().Replace(" "c, "_"c).ToUpperInvariant()
                    If token.Length > 0 AndAlso seen.Add(token) Then
                        results.Add(token)
                    End If
                End If

            Catch ex As Exception
                Try : LogError.writeErr("GetRouteLocationsFromForm: " & ex.Message) : Catch : End Try
            End Try
        Next

        Return results
    End Function

    Private Function BuildLocationPipeListFromForm(root As Control) As String
        Dim items As List(Of String) = GetRouteLocationsFromForm(root)
        If items Is Nothing OrElse items.Count = 0 Then Return String.Empty
        Return "|" & String.Join("|", items) & "|"
    End Function

    Private Function NormalizeRouteToken(routeNo As String) As String
        If String.IsNullOrWhiteSpace(routeNo) Then Return Nothing
        Dim r As String = routeNo.Trim()
        If Not r.StartsWith("ROUTE_", StringComparison.OrdinalIgnoreCase) Then
            r = "ROUTE_" & r
        End If
        Return r.ToUpperInvariant()
    End Function

    Private Function BuildExeNameFromRoute_Conveyer(routeNo As String) As String
        Dim core As String = NormalizeRouteToken(routeNo)
        If core Is Nothing Then Return Nothing
        Return EXE_PREFIX & core & EXE_SUFFIX_CONVEYER
    End Function

    Private Function BuildExeNameFromRoute_Batching(routeNo As String) As String
        Dim core As String = NormalizeRouteToken(routeNo)
        If core Is Nothing Then Return Nothing
        Return EXE_PREFIX & core & EXE_SUFFIX_BATCHING
    End Function

    Private Function BuildExeNameFromRoute(routeNo As String) As String
        Return BuildExeNameFromRoute_Conveyer(routeNo)
    End Function

    Private Function BuildExeNamesFromRoutes(routeNos As IEnumerable(Of String), ByVal isBatching As Boolean) As HashSet(Of String)
        Dim hs As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        For Each rn As String In routeNos
            Dim exeName As String = If(isBatching, BuildExeNameFromRoute_Batching(rn),
                                              BuildExeNameFromRoute_Conveyer(rn))
            If Not String.IsNullOrWhiteSpace(exeName) Then hs.Add(exeName)
        Next
        Return hs
    End Function

    Private Function BuildRouteParam(routeNo As String) As String
        If String.IsNullOrWhiteSpace(routeNo) Then Return String.Empty
        Dim token As String = routeNo
        If token.StartsWith("ROUTE_", StringComparison.OrdinalIgnoreCase) Then
            token = "Route_" & token.Substring(6)
        ElseIf Not token.StartsWith("Route_", StringComparison.Ordinal) Then
            token = "Route_" & token
        End If
        Return $"ROUTE {token} 1"
    End Function
#End Region
#Region "# CONFIG APP"
    Private Sub MyConfigApp()
        Try
            Dim reader As AppSettingsReader = New AppSettingsReader()
            With StrMqtt_Config_
                .StationMqtt = CShort(reader.GetValue("CCB", GetType(Int16)))
                .IpAddressMqtt = CStr(reader.GetValue("Server_IP", GetType(String)))
                .DbUser = CStr(reader.GetValue("USERNAME", GetType(String)))
                .DbPass = CStr(reader.GetValue("PASSWORD", GetType(String)))
                .MqttUser = CStr(reader.GetValue("MqttUser", GetType(String)))
                .MqttPass = CStr(reader.GetValue("MqttPass", GetType(String)))
                .HardLock = CBool(reader.GetValue("HardLock", GetType(Boolean)))
                .TmpConfig = " " & CStr(.StationMqtt) & " " & CStr(.IpAddressMqtt) & " " & CStr(.MqttUser) & " " & CStr(.MqttPass) & " "
                .RouteConfig = CBool(reader.GetValue("Route_Config", GetType(Boolean)))
                .Mode_TestIO = CBool(reader.GetValue("Mode_TestIO", GetType(Boolean)))
                .Confirm_TestIO = CBool(reader.GetValue("Confirm_TestIO", GetType(Boolean)))
            End With

            Read_DB_Config()

        Catch ex As Exception
            Dim strMessage As String =
                "Error Number :  " & Err.Number & " [MDI_FRM/MyConfigApp]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            Try : LogError.writeErr(strMessage) : Catch : End Try
        End Try
    End Sub
#End Region
#Region "# READ DB CONFIG"
    Private Sub Read_DB_Config()
        Dim strPath As String = Get_ConfigPath() & "_Config\DB_Config.txt"
        Dim ReadFile As IniFile = New IniFile(strPath)

        With Batching_Conf
            .Name = ReadFile.GetString("DB_Batching", "NAME", "error")
            .IPAddress = ReadFile.GetString("DB_Batching", "IP_ADDRESS ", "error")
            .User = ReadFile.GetString("DB_Batching", "USER ", "error")
            .Password = ReadFile.GetString("DB_Batching", "PASSWORD ", "error")
            .Connection_Type = ReadFile.GetString("DB_Batching", "CONNECTION_TYPE ", "error")
        End With

        With Auto_Route_Conf
            .Name = ReadFile.GetString("DB_Auto_Route", "NAME", "error")
            .IPAddress = ReadFile.GetString("DB_Auto_Route", "IP_ADDRESS ", "error")
            .User = ReadFile.GetString("DB_Auto_Route", "USER ", "error")
            .Password = ReadFile.GetString("DB_Auto_Route", "PASSWORD ", "error")
            .Connection_Type = ReadFile.GetString("DB_Auto_Route", "CONNECTION_TYPE ", "error")
        End With

        With Dev_Center_Conf
            .Name = ReadFile.GetString("DB_Dev_Center", "NAME", "error")
            .IPAddress = ReadFile.GetString("DB_Dev_Center", "IP_ADDRESS ", "error")
            .User = ReadFile.GetString("DB_Dev_Center", "USER ", "error")
            .Password = ReadFile.GetString("DB_Dev_Center", "PASSWORD ", "error")
            .Connection_Type = ReadFile.GetString("DB_Dev_Center", "CONNECTION_TYPE ", "error")
        End With

        With Premix_Conf
            .Name = ReadFile.GetString("DB_Premix", "NAME", "error")
            .IPAddress = ReadFile.GetString("DB_Premix", "IP_ADDRESS ", "error")
            .User = ReadFile.GetString("DB_Premix", "USER ", "error")
            .Password = ReadFile.GetString("DB_Premix", "PASSWORD ", "error")
            .Connection_Type = ReadFile.GetString("DB_Premix", "CONNECTION_TYPE ", "error")
        End With
    End Sub
#End Region
#Region "# EXTEND MONITOR"
    Public numberofmonitors As Integer = Screen.AllScreens.Length
    Private isExtendMode As Boolean = False
    Private ReadOnly Frm_arr_extend() As Form = {frm_Page_1, frm_Page_2}

    ' === NEW: จำว่าตอนนี้กำลัง extend ไปที่จอไหน (-1 = ไม่ได้ extend) ===
    Private extendScreenIndex As Integer = -1

    ' === NEW: หา index ของจอ primary ปัจจุบัน ===
    Private Function GetPrimaryScreenIndex() As Integer
        Dim all As Screen() = Screen.AllScreens   ' <<< ต้องใส่ As Screen()
        For i As Integer = 0 To all.Length - 1
            If all(i).Primary Then Return i
        Next
        Return 0
    End Function

    ' === NEW: จัดวางฟอร์มสองหน้าให้พอดีกับตำแหน่งจอจริง ===
    Private Sub LayoutForExtend(primaryForm As Form, extendForm As Form)
        If extendScreenIndex < 0 OrElse extendScreenIndex >= Screen.AllScreens.Length Then Exit Sub

        Dim all As Screen() = Screen.AllScreens   ' <<< ใส่ type ให้เหมือนกัน
        Dim primaryScreen As Screen = all(GetPrimaryScreenIndex())
        Dim extendScreen As Screen = all(extendScreenIndex)

        ' รวมกรอบของสองจอเป็นพื้นที่เดียว
        Dim unionBounds As Rectangle = Rectangle.Union(primaryScreen.Bounds, extendScreen.Bounds)

        ' ตั้งค่า MDI parent ให้กินพื้นที่ union ทั้งหมด
        Me.StartPosition = FormStartPosition.Manual
        Me.WindowState = FormWindowState.Normal
        Me.Bounds = unionBounds

        ' map ตำแหน่งจอให้กลายเป็นตำแหน่งภายใน union (0,0)
        Dim priLoc As New Point(primaryScreen.Bounds.X - unionBounds.X, primaryScreen.Bounds.Y - unionBounds.Y)
        Dim extLoc As New Point(extendScreen.Bounds.X - unionBounds.X, extendScreen.Bounds.Y - unionBounds.Y)
        Dim priSize As Size = primaryScreen.Bounds.Size
        Dim extSize As Size = extendScreen.Bounds.Size

        ' set MDI child ทั้งสอง
        For Each f As Form In New Form() {primaryForm, extendForm}
            If f IsNot Nothing AndAlso Not f.IsDisposed Then
                f.MdiParent = Me
                f.StartPosition = FormStartPosition.Manual
                f.WindowState = FormWindowState.Normal
            End If
        Next

        With primaryForm
            .Location = priLoc
            .Size = priSize
            .Show()
            .BringToFront()
        End With

        With extendForm
            .Location = extLoc
            .Size = extSize
            .Show()
            .BringToFront()
        End With

        isExtendMode = True
    End Sub

    Public Async Sub SelectP1()
        If btnPage_2.Visible = True Then
            btnPage_1.Enabled = False
            btnPage_2.Enabled = False

            ' หน่วงเวลา
            Await Task.Delay(1000)

            If numberofmonitors > 1 AndAlso isExtendMode AndAlso HasSecondPage() AndAlso extendScreenIndex >= 0 Then
                ' === แก้: วาง P1 ที่จอ primary และ P2 ที่จอ extend (ไม่ว่า extend จะอยู่ซ้าย/ขวา/บน/ล่าง) ===
                LayoutForExtend(frm_Page_1, frm_Page_2)
            Else
                ' โหมดปกติ: แสดงหน้าเดียว
                frm_Page_2.Hide()

                frm_Page_1.MdiParent = Me
                frm_Page_1.StartPosition = FormStartPosition.Manual
                frm_Page_1.WindowState = FormWindowState.Normal
                frm_Page_1.Location = New Point(0, 0)
                frm_Page_1.Size = Me.ClientSize
                frm_Page_1.Show()
                frm_Page_1.BringToFront()
            End If

            btnPage_1.Enabled = True
            btnPage_2.Enabled = True
        End If
    End Sub

    Public Async Sub SelectP2()
        If btnPage_1.Visible = True Then
            btnPage_1.Enabled = False
            btnPage_2.Enabled = False

            ' หน่วงเวลา
            Await Task.Delay(1000)

            If numberofmonitors > 1 AndAlso isExtendMode AndAlso HasSecondPage() AndAlso extendScreenIndex >= 0 Then
                ' === แก้: วาง P2 ที่จอ primary และ P1 ที่จอ extend (สลับกันกรณีเลือกหน้า 2 เป็นหลัก) ===
                LayoutForExtend(frm_Page_2, frm_Page_1)
            Else
                ' โหมดปกติ: แสดงหน้าเดียว
                frm_Page_1.Hide()

                frm_Page_2.MdiParent = Me
                frm_Page_2.StartPosition = FormStartPosition.Manual
                frm_Page_2.WindowState = FormWindowState.Normal
                frm_Page_2.Location = New Point(0, 0)
                frm_Page_2.Size = Me.ClientSize
                frm_Page_2.Show()
                frm_Page_2.BringToFront()
            End If

            btnPage_1.Enabled = True
            btnPage_2.Enabled = True
        End If
    End Sub

    Private Function HasSecondPage() As Boolean
        Try
            Return (btnPage_2 IsNot Nothing AndAlso btnPage_2.Visible AndAlso
                    Frm_arr_extend IsNot Nothing AndAlso Frm_arr_extend.Length >= 2 AndAlso
                    Frm_arr_extend(1) IsNot Nothing AndAlso Not Frm_arr_extend(1).IsDisposed)
        Catch
            Return False
        End Try
    End Function

    Private Sub BtnExtendMode_Click(sender As Object, e As EventArgs) Handles btnExtend.Click
        If isExtendMode Then
            If HasSecondPage() Then
                Dim result As DialogResult = MessageBox.Show("Do you want to exit extend mode?", "Confirm", MessageBoxButtons.YesNo)
                If result = DialogResult.Yes Then ExitExtendMode()
            Else
                ShowMonitorGridMenu()
            End If
        Else
            ShowMonitorGridMenu()
        End If
    End Sub

    Private Sub ExitExtendMode()
        isExtendMode = False
        extendScreenIndex = -1 ' === reset ===

        If Frm_arr_extend IsNot Nothing Then
            For i As Integer = 0 To Frm_arr_extend.Length - 1
                Dim f As Form = Frm_arr_extend(i)
                If f IsNot Nothing AndAlso Not f.IsDisposed Then
                    Try
                        f.Hide()
                        f.MdiParent = Nothing
                        f.WindowState = FormWindowState.Normal
                    Catch
                    End Try
                End If
            Next
        End If

        Me.StartPosition = FormStartPosition.WindowsDefaultBounds
        Me.WindowState = FormWindowState.Normal
        Me.Bounds = Screen.PrimaryScreen.WorkingArea

        Dim f0 As Form = Frm_arr_extend(0)
        If f0 IsNot Nothing AndAlso Not f0.IsDisposed Then
            f0.MdiParent = Me
            f0.StartPosition = FormStartPosition.Manual
            f0.WindowState = FormWindowState.Normal
            f0.Location = New Point(0, 0)
            f0.Size = New Size(Me.ClientSize.Width, Me.ClientSize.Height)
            f0.Show()
            f0.BringToFront()
        End If
        Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub ExtendMonitorToScreen(selectedIndex As Integer)
        If Screen.AllScreens.Length < 2 Then Return
        If selectedIndex < 0 OrElse selectedIndex >= Screen.AllScreens.Length Then Return
        If Not HasSecondPage() Then
            MoveMainToScreen(selectedIndex)
            Return
        End If

        Dim primaryScreen As Screen = Screen.AllScreens.First(Function(s As Screen) s.Primary)
        Dim extendScreen As Screen = Screen.AllScreens(selectedIndex)
        If primaryScreen.DeviceName = extendScreen.DeviceName Then Return

        ' === เก็บ index ไว้ใช้กับ SelectP1/SelectP2 ===
        extendScreenIndex = selectedIndex

        ' จัดวาง: หน้า 1 ที่จอ primary, หน้า 2 ที่จอ extend
        LayoutForExtend(frm_Page_1, frm_Page_2)
    End Sub

    Private Sub MoveMainToScreen(selectedIndex As Integer)
        If selectedIndex < 0 OrElse selectedIndex >= Screen.AllScreens.Length Then Return

        Dim target As Screen = Screen.AllScreens(selectedIndex)

        Dim f0 As Form = Frm_arr_extend(0)
        If f0 IsNot Nothing AndAlso Not f0.IsDisposed Then
            f0.Hide()
            f0.MdiParent = Me
        End If

        Me.StartPosition = FormStartPosition.Manual
        Me.WindowState = FormWindowState.Normal
        Me.Bounds = target.WorkingArea

        If f0 IsNot Nothing AndAlso Not f0.IsDisposed Then
            f0.MdiParent = Me
            f0.StartPosition = FormStartPosition.Manual
            f0.WindowState = FormWindowState.Normal
            f0.Location = New Point(0, 0)
            f0.Size = New Size(Me.ClientSize.Width, Me.ClientSize.Height)
            f0.Show()
            f0.BringToFront()
            Me.WindowState = FormWindowState.Maximized
        End If

        isExtendMode = True
        extendScreenIndex = -1 ' โหมด single page บนจออื่น ไม่ได้ใช้ extend 2 จอ
    End Sub

    Private Sub ShowMonitorGridMenu()
        Dim allScreens As List(Of Screen) = Screen.AllScreens.ToList()
        If allScreens.Count = 0 Then Return

        Dim primary As Screen = allScreens.FirstOrDefault(Function(s As Screen) s.Primary)
        If primary Is Nothing Then Return

        Dim hasTwoPages As Boolean = HasSecondPage()
        Dim currentScreen As Screen = Screen.FromControl(Me)

        Dim grid(2, 2) As Integer
        For y As Integer = 0 To 2
            For x As Integer = 0 To 2
                grid(x, y) = -1
            Next
        Next

        Dim pcx As Integer = primary.Bounds.X + primary.Bounds.Width \ 2
        Dim pcy As Integer = primary.Bounds.Y + primary.Bounds.Height \ 2

        For i As Integer = 0 To allScreens.Count - 1
            Dim s As Screen = allScreens(i)
            Dim scx As Integer = s.Bounds.X + s.Bounds.Width \ 2
            Dim scy As Integer = s.Bounds.Y + s.Bounds.Height \ 2

            Dim dx As Integer = Math.Sign(scx - pcx)
            Dim dy As Integer = Math.Sign(scy - pcy)

            Dim gx As Integer = dx + 1
            Dim gy As Integer = dy + 1

            If gx >= 0 AndAlso gx <= 2 AndAlso gy >= 0 AndAlso gy <= 2 Then
                grid(gx, gy) = i
            End If
        Next

        Dim form As Form = New Form With {
            .Text = If(hasTwoPages, "Please select extend monitor", "Select monitor to show main page"),
            .Size = New Size(600, 300),
            .FormBorderStyle = FormBorderStyle.FixedDialog,
            .StartPosition = FormStartPosition.CenterScreen,
            .MaximizeBox = False,
            .MinimizeBox = False
        }

        Dim tbl As TableLayoutPanel = New TableLayoutPanel With {
            .RowCount = 3,
            .ColumnCount = 3,
            .Dock = DockStyle.Fill,
            .Padding = New Padding(10)
        }
        For i As Integer = 0 To 2
            tbl.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 33.33F))
            tbl.RowStyles.Add(New RowStyle(SizeType.Percent, 33.33F))
        Next

        For y As Integer = 0 To 2
            For x As Integer = 0 To 2
                Dim btn As Button = New Button With {
                    .Dock = DockStyle.Fill,
                    .Font = New Font("Segoe UI", 12, FontStyle.Regular),
                    .FlatStyle = FlatStyle.Flat
                }

                Dim idx As Integer = grid(x, y)

                If idx = -1 Then
                    Dim isDiagonal As Boolean = (x = y) OrElse (x + y = 2)
                    If isDiagonal Then
                        btn.Text = ""
                        btn.Enabled = False
                        btn.BackColor = Color.White
                    Else
                        btn.Text = "Not found"
                        btn.Enabled = False
                        btn.BackColor = Color.LightGray
                    End If
                    tbl.Controls.Add(btn, x, y)
                    Continue For
                End If

                Dim scr As Screen = allScreens(idx)
                If hasTwoPages Then
                    If scr.Primary Then
                        btn.Text = "Primary (fixed)"
                        btn.Enabled = False
                        btn.BackColor = Color.LightBlue
                    Else
                        btn.Text = $"Extend (#{idx})"
                        btn.BackColor = Color.LightGreen
                        AddHandler btn.Click, Sub()
                                                  ExtendMonitorToScreen(idx)
                                                  form.Close()
                                              End Sub
                    End If
                Else
                    If scr.DeviceName = currentScreen.DeviceName Then
                        btn.Text = $"Current screen (#{idx})"
                        btn.Enabled = False
                        btn.BackColor = Color.LightBlue
                    Else
                        btn.Text = $"Show main page on screen #{idx}"
                        btn.BackColor = Color.LightGreen
                        AddHandler btn.Click, Sub()
                                                  MoveMainToScreen(idx)
                                                  form.Close()
                                              End Sub
                    End If
                End If

                tbl.Controls.Add(btn, x, y)
            Next
        Next

        form.Controls.Add(tbl)
        form.ShowDialog()
    End Sub
#End Region
#Region "# MDI BUTTON & CONTROL"
    Private Sub btn_Show_bin_code_Click(sender As Object, e As EventArgs) Handles btn_Show_bin_code.Click
        ToolStripDropDownButton1.ShowDropDown()
    End Sub

    Private Sub CodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles btn_bin_code.Click
        SaveSetting("TAT", "BIN CONTROL", "BIN.SHOWMODE", "Code")
    End Sub

    Private Sub NameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles btn_bin_name.Click
        SaveSetting("TAT", "BIN CONTROL", "BIN.SHOWMODE", "Name")
    End Sub

    Private Sub InventoryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles btn_bin_inventory.Click
        SaveSetting("TAT", "BIN CONTROL", "BIN.SHOWMODE", "Inventory")
    End Sub

    Private Sub CapacityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles btn_bin_cap.Click
        SaveSetting("TAT", "BIN CONTROL", "BIN.SHOWMODE", "Capacity")
    End Sub

    Private Sub ShotNameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShortNameToolStripMenuItem.Click
        SaveSetting("TAT", "BIN CONTROL", "BIN.SHOWMODE", "ShortName")
    End Sub

    Private Sub ThaiToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ThaiToolStripMenuItem.Click
        SaveSetting("TAT", "BIN CONTROL", "BIN.SHOWMODE", "Other")
    End Sub

    Private Sub btn_JOB_ASSIGNMENT_Click(sender As Object, e As EventArgs) Handles btn_JOB_ASSIGNMENT.Click
        If Call_StartBatch_EXE Then
            Job_Assignment_StartBatch()
        Else
            Try
                Dim pipeList As String = BuildLocationPipeListFromForm(Me)

                io.CloseEXE("Job_Assignment")

                If String.IsNullOrEmpty(pipeList) Then Exit Sub
                io.Open_Application("Job_Assignment", "ROUTE " & pipeList)

            Catch ex As Exception
                If Error_Check Then Exit Sub
                Error_Check = True
                Dim strMessage As String =
                    "Error Number :  " & Err.Number & " [" & Me.Name & "]" & vbNewLine &
                    "Error Description :  " & ex.Message & vbCrLf &
                    "Error at : " & ex.StackTrace
                Try : LogError.writeErr(strMessage) : Catch : End Try
            End Try
        End If
    End Sub

    Private Sub btn_PRODUCTION_TIME_Click(sender As Object, e As EventArgs) Handles btn_PRODUCTION_TIME.Click
        Try
            frm_production_time.Show()
        Catch ex As Exception
            If Error_Check Then Exit Sub
            Error_Check = True
            Dim strMessage As String =
                "Error Number :  " & Err.Number & " [" & Me.Name & "]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            Try : LogError.writeErr(strMessage) : Catch : End Try
        End Try
    End Sub

    Public Shared Function Current() As MDI_FRM
        Return Application.OpenForms.OfType(Of MDI_FRM)().FirstOrDefault()
    End Function

    Public Property IsAuthenticated As Boolean
        Get
            Return _isLoggedIn
        End Get
        Set(value As Boolean)
            ApplyAuthState(value)
        End Set
    End Property

    Private Sub ApplyAuthState(ok As Boolean)
        _isLoggedIn = ok
        UpdateAuthUI()
    End Sub

    Public Sub ApplyPermissions(canProd As Boolean, canJob As Boolean, canMain As Boolean, canAlarm As Boolean, canRoute As Boolean, canStart As Boolean)
        _canProductionTime = canProd
        _canJobAssignment = canJob
        _canMainScada = canMain
        _canAlarm = canAlarm
        _canRoute = canRoute
        _canStartBatch = canStart
        Main_Scada = _canMainScada
        Alarm_Status = _canAlarm
        Route_Status = _canRoute
        Start_Batch_Status = _canStartBatch
        UpdateAuthUI()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        UpdateAuthUI()
    End Sub

    Public Function PromptLogin(owner As IWin32Window) As Boolean
        Dim ok As Boolean
        Using f As New frm_login()
            ok = (f.ShowDialog(owner) = DialogResult.OK)
        End Using
        IsAuthenticated = ok
        Return ok
    End Function

    Private Sub btn_LOG_ON_Click(sender As Object, e As EventArgs) Handles btn_LOG_ON.Click
        PromptLogin(Me)
    End Sub

    Private Sub btn_LOG_OFF_Click(sender As Object, e As EventArgs) Handles btn_LOG_OFF.Click
        Try
            Close_EXE()
            IsAuthenticated = False
        Catch ex As Exception
            If Error_Check Then Exit Sub
            Error_Check = True
            Dim strMessage As String =
                "Error Number :  " & Err.Number & " [" & Me.Name & "]" & vbNewLine &
                "Error Description :  " & ex.Message & vbCrLf &
                "Error at : " & ex.StackTrace
            Try : LogError.writeErr(strMessage) : Catch : End Try
        End Try
    End Sub

    Private Sub UpdateAuthUI()
        If Me.IsHandleCreated AndAlso Me.InvokeRequired Then
            Me.BeginInvoke(New MethodInvoker(AddressOf UpdateAuthUI))
            Return
        End If

        btn_LOG_OFF.Enabled = _isLoggedIn
        btn_LOG_ON.Enabled = Not _isLoggedIn

        btn_PRODUCTION_TIME.Enabled = _isLoggedIn AndAlso _canProductionTime
        btn_JOB_ASSIGNMENT.Enabled = _isLoggedIn AndAlso _canJobAssignment
        ' If there are other buttons, do the same.
    End Sub

    Private Function MakeGrayAndFade(src As Image, Optional opacity As Single = 0.55F) As Image
        If src Is Nothing Then Return Nothing
        Dim bmp As Bitmap = New Bitmap(src.Width, src.Height)
        Using g As Graphics = Graphics.FromImage(bmp)
            Dim cm As Imaging.ColorMatrix = New Imaging.ColorMatrix(New Single()() {
                New Single() {0.3F, 0.3F, 0.3F, 0, 0},
                New Single() {0.59F, 0.59F, 0.59F, 0, 0},
                New Single() {0.11F, 0.11F, 0.11F, 0, 0},
                New Single() {0, 0, 0, opacity, 0},
                New Single() {0, 0, 0, 0, 1}
            })
            Dim ia As Imaging.ImageAttributes = New Imaging.ImageAttributes()
            ia.SetColorMatrix(cm, Imaging.ColorMatrixFlag.Default, Imaging.ColorAdjustType.Bitmap)

            g.DrawImage(src,
                        New Rectangle(0, 0, src.Width, src.Height),
                        0, 0, src.Width, src.Height,
                        GraphicsUnit.Pixel, ia)
        End Using
        Return bmp
    End Function

    Private Sub btn_Show_Code_Click(sender As Object, e As EventArgs) Handles btn_Show_Code.Click
        Dim state As Tuple(Of Boolean, Image, Image) = TryCast(btn_Show_Code.Tag, Tuple(Of Boolean, Image, Image))

        If state Is Nothing Then
            Dim normal As Image = btn_Show_Code.Image
            Dim gray As Image = If(normal IsNot Nothing, MakeGrayAndFade(normal, 0.55F), Nothing)
            state = Tuple.Create(True, normal, gray)
        End If

        Dim isShowing As Boolean = Not state.Item1

        If isShowing Then
            If state.Item2 IsNot Nothing Then btn_Show_Code.Image = state.Item2
        Else
            If state.Item3 IsNot Nothing Then btn_Show_Code.Image = state.Item3
        End If

        btn_Show_Code.Tag = Tuple.Create(isShowing, state.Item2, state.Item3)

        frm_Page_1.SuspendLayout()
        For Each ctrl As Control In GetAllControls(frm_Page_1)
            If TypeOf ctrl Is Label Then
                Dim lbl As Label = DirectCast(ctrl, Label)
                If lbl.Name.StartsWith("LblMotorCode_", StringComparison.OrdinalIgnoreCase) Then
                    lbl.Visible = isShowing
                End If
            End If
        Next
        frm_Page_1.ResumeLayout()
    End Sub

    Private Function GetAllControls(root As Control) As List(Of Control)
        Dim list As New List(Of Control)()
        Dim stack As New Stack(Of Control)()
        stack.Push(root)

        While stack.Count > 0
            Dim current As Control = stack.Pop()
            For Each c As Control In current.Controls
                list.Add(c)
                stack.Push(c)
            Next
        End While
        Return list
    End Function

    Private Sub btnPage_1_Click(sender As Object, e As EventArgs) Handles btnPage_1.Click
        SelectP1()
    End Sub

    Private Sub btnPage_2_Click(sender As Object, e As EventArgs) Handles btnPage_2.Click
        SelectP2()
    End Sub

    Private Sub tsCboLanguage_TextChanged(sender As Object, e As EventArgs) Handles tsCboLanguage.TextChanged
        structApp_Language.strLang_Main = tsCboLanguage.Text
        RaiseEvent_ChangeLanguage(tsCboLanguage.Text, CnBatching)
    End Sub

    Public Shared ReadOnly AppCts As CancellationTokenSource = New CancellationTokenSource()
    Private _isExiting As Boolean = False

    Private Async Sub btn_exit_Click(sender As Object, e As EventArgs) Handles btn_exit.Click
        Await RequestExitAsync(showConfirm:=True)
    End Sub

    Public Async Function RequestExitAsync(Optional showConfirm As Boolean = True) As Task
        If Me.InvokeRequired Then
            Dim tcs As New TaskCompletionSource(Of Object)()
            Me.BeginInvoke(DirectCast(Async Sub()
                                          Try
                                              Await RequestExitAsync(showConfirm)
                                              tcs.TrySetResult(Nothing)
                                          Catch ex As Exception
                                              tcs.TrySetException(ex)
                                          End Try
                                      End Sub, MethodInvoker))
            Await tcs.Task
            Return
        End If

        If _isExiting Then Return
        _isExiting = True

        Try
            If showConfirm Then
                Dim res As DialogResult = MessageBox.Show(Me, "Do you want to exit the program?", "Confirm exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If res <> DialogResult.Yes Then
                    _isExiting = False
                    Return
                End If
            End If

            Dim closingForm As New frmClosing()
            closingForm.Show()
            closingForm.Refresh()

            AppCts.Cancel()

            Dim closedGracefully As Boolean = Await SafeShutdownAsync(TimeSpan.FromSeconds(3)).ConfigureAwait(True)

            Try : closingForm.Close() : closingForm.Dispose() : Catch : End Try

            Application.Exit()

            Await Task.Delay(1500).ConfigureAwait(False)
            If Not closedGracefully Then
                Try : Process.GetCurrentProcess().Kill() : Catch : End Try
            End If

        Catch ex As Exception
            Try : LogError.writeErr("RequestExitAsync error: " & ex.Message) : Catch : End Try
            Try : Process.GetCurrentProcess().Kill() : Catch : End Try
        End Try
    End Function

    Private Async Function SafeShutdownAsync(grace As TimeSpan) As Task(Of Boolean)
        Try
            Close_EXE()
            StopAllServicesQuiet()

            Dim children As Form() = Me.MdiChildren.ToArray()

            For Each f As Form In children
                Try
                    Dim mi As MethodInfo = f.GetType().GetMethod("StopInternalServices", BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)
                    If mi IsNot Nothing Then mi.Invoke(f, Nothing)
                Catch
                End Try
            Next

            For Each f As Form In children
                Try : f.Close() : Catch : End Try
                Try : f.Dispose() : Catch : End Try
            Next

            Dim sw As New Stopwatch()
            sw.Start()
            Do While sw.Elapsed < grace
                If IsProcessQuiescent() Then Return True
                Await Task.Delay(100).ConfigureAwait(False)
            Loop

        Catch ex As Exception
            Try : LogError.writeErr("SafeShutdownAsync error: " & ex.Message) : Catch : End Try
        End Try

        Return False
    End Function

    Private Sub StopAllServicesQuiet()
        Try
            ' Close real resources according to the project
        Catch
        End Try
    End Sub

    Private Function IsProcessQuiescent() As Boolean
        Try
            If Application.OpenForms.Count > 0 Then Return False
            Dim thCount As Integer = Process.GetCurrentProcess().Threads.Count
            If thCount > 10 Then Return False
            Return True
        Catch
            Return False
        End Try
    End Function

    Private Sub MDI_FRM_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If e.CloseReason = CloseReason.UserClosing AndAlso Not _isExiting Then
            e.Cancel = True
            Me.BeginInvoke(DirectCast(Async Sub() Await RequestExitAsync(showConfirm:=True), MethodInvoker))
            Exit Sub
        End If

        If Not _isExiting Then
            _isExiting = True
            Me.BeginInvoke(DirectCast(Async Sub() Await SafeShutdownAsync(TimeSpan.FromSeconds(3)), MethodInvoker))
        End If
    End Sub

    Public Sub Close_EXE()
        Try
            Dim routeNos As IEnumerable(Of String) = Enumerable.Empty(Of String)()
            Try
                routeNos = GetRouteNos()
            Catch ex As Exception
                Try : LogError.writeErr("GetRouteNos error: " & ex.Message) : Catch : End Try
            End Try

            For Each rn As String In routeNos
                Try
                    Dim exeName As String = BuildExeNameFromRoute(rn)
                    If Not String.IsNullOrWhiteSpace(exeName) Then io.CloseEXE(exeName)
                Catch ex As Exception
                    Try : LogError.writeErr($"CloseEXE({rn}) error: {ex.Message}") : Catch : End Try
                End Try
            Next

            If frmAlarm_Des_New IsNot Nothing Then
                Try
                    frmAlarm_Des_New.Close()
                    frmAlarm_Des_New.Dispose()
                Catch ex As Exception
                    Try : LogError.writeErr("frmAlarm_Des_New close error: " & ex.Message) : Catch : End Try
                Finally
                    frmAlarm_Des_New = Nothing
                End Try
            End If

            Custom_CloseApp()

        Catch ex As Exception
            Try : LogError.writeErr("Close_EXE outer error: " & ex.Message) : Catch : End Try
        End Try
    End Sub

    Private Sub MDI_FRM_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        Dim appName As String = Process.GetCurrentProcess.ProcessName
        Dim sameProcessTotal As Integer = Process.GetProcessesByName(appName).Length
        If sameProcessTotal > 1 Then
            End
        End If
    End Sub
#End Region
#Region "# TIMER & MQTT"
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            MDI_DateNow.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CreateSpecificCulture("en-GB"))

            If StatusReadPort_.strPort Is Nothing OrElse StatusReadPort_.strPlc Is Nothing Then
                Exit Sub
            End If

            With StatusReadPort_
                tstbStatusMqtt.Text = .strPort(0).ToString()
                tstbStatusPlc.Text = .strPlc.ToString()
            End With

            tstbUserNameLogin.Text = UserLogon_.UserName

            If btn_LOG_ON.Enabled = False Then
                If StrMqtt_Config_.HardLock Then
                    If CheckHasp.Hardlock() <> 0 Then
                        If tmpCheckHardLock = False Then
                            tmpDatetimeForce = DateTime.Now
                            tmpCheckHardLock = True
                        End If
                        tmpMinute = DateDiff(DateInterval.Minute, tmpDatetimeForce, DateTime.Now)
                        If tmpMinute >= 2 Then
                            tmpCheckHardLock = False
                            btn_LOG_OFF_Click(sender, e)
                            tstbLastUpdate.BackColor = Color.Red
                            tstbLastUpdate.ForeColor = Color.Yellow
                            tstbLastUpdate.Text = "Hardlock Not Found, Please check and make sure it is installed properly on the USB port."
                        Else
                            If tstbLastUpdate.BackColor = Color.Red Then
                                tstbLastUpdate.BackColor = Color.Yellow
                                tstbLastUpdate.ForeColor = Color.Red
                            Else
                                tstbLastUpdate.BackColor = Color.Red
                                tstbLastUpdate.ForeColor = Color.Yellow
                            End If
                            tstbLastUpdate.Text = "Hardlock Not Found, Please check and make sure it is installed properly on the USB port.(" & (2 - tmpMinute) & " Min. Can't control)"
                        End If
                    Else
                        tstbLastUpdate.BackColor = Color.White
                        tstbLastUpdate.ForeColor = Color.Black
                        tstbLastUpdate.Text = $"{Application.ProductName} [{Application.ProductVersion}] [{Application.CompanyName}] [{FileDateTime(Application.ExecutablePath)}]"
                        tmpCheckHardLock = False
                    End If
                Else
                    tstbLastUpdate.BackColor = Color.White
                    tstbLastUpdate.ForeColor = Color.Black
                    tstbLastUpdate.Text = $"{Application.ProductName} [{Application.ProductVersion}] [{Application.CompanyName}] [{FileDateTime(Application.ExecutablePath)}]"
                    tmpCheckHardLock = False
                End If
            End If

            CheckMQTT()

        Catch ex As Exception
            Try
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder()
                sb.AppendLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Error Number: {ex.HResult} [{Me.Name}]")
                sb.AppendLine($"Exception: {ex.GetType().FullName}")
                sb.AppendLine($"Message: {ex.Message}")
                sb.AppendLine("StackTrace:")
                sb.AppendLine(ex.StackTrace)
                If ex.InnerException IsNot Nothing Then
                    sb.AppendLine("InnerException:")
                    sb.AppendLine(ex.InnerException.ToString())
                End If
                LogError.writeErr(sb.ToString())
            Catch
            End Try
        End Try
    End Sub

    Private Sub CheckMQTT()
        Dim statusP1 As Boolean
        Try
            statusP1 = (frm_Page_1 IsNot Nothing AndAlso frm_Page_1.Scada IsNot Nothing AndAlso frm_Page_1.Scada.Check_MQTT_Active)
        Catch
            statusP1 = False
        End Try

        If statusP1 Then
            tstbMqttHeartbeat.BackColor = Color.LimeGreen
            tstbMqttHeartbeat.Text = "MQTT IS ACTIVED"
        Else
            tstbMqttHeartbeat.BackColor = Color.Red
            tstbMqttHeartbeat.Text = "MQTT IS NOT ACTIVE"
        End If
    End Sub
#End Region

#Region " >>> CUSTOM CALL/CLOSE EXE <<<"

    Private ReadOnly lastOpenedExeNames_Other As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

    ' ===================================== NOTE =====================================
    ' ROUTE ไม่ต้องเรียกผ่าน Custom_OpenApp Scada จะเรียกจาก Properties ของ control ใน frm
    ' แต่ถ้าเป็นพวก Start Batch / Get Report / Alarm ต้องเรียกผ่าน Custom_OpenApp
    ' Job Assignment สำหรับหน้า Start Batch ยังต้องเรียกผ่าน Job_Assignment_StartBatch 
    ' ================================================================================

    Public Sub Job_Assignment_StartBatch()
        'io.Open_Application("Job_Assignment", "ROUTE |BATCHING_1|BATCHING_2|")
    End Sub

    Public Sub Custom_OpenApp()
        Try
            ' อ่าน App.config สำหรับเรียกกรณี Start Batch
            Boolean.TryParse(ConfigurationManager.AppSettings("Start_Batch"), Call_StartBatch_EXE)

            If Start_Batch_Status And Call_StartBatch_EXE Then
                'io.Open_Application_For_START_BATCH("TAT01_START_BATCHING_1_MIXER_1", " 1 ZR89900 localhost ASA BATCHING_1 MIXER_1 AUTO")
                'io.Open_Application_For_START_BATCH("TAT01_START_BATCHING_2_MIXER_2", " 1 ZR90500 localhost ASA BATCHING_2 MIXER_2 AUTO")
            End If

            If Get_Report_Status = True Then
                'io.Open_Application_For_GET_REPORT("GET_REPORT_BATCHING_MIX1", "GET_REPORT_1 " & UserLogon_.UserName & "")
                'io.Open_Application_For_GET_REPORT("GET_REPORT_BATCHING_MIX2", "GET_REPORT_2 " & UserLogon_.UserName & "")
            End If

            If Alarm_Status = True Then
                With StrMqtt_Config_
                    'io.Open_Application_For_Alarm_Scale("SCALE_1", .TmpConfig & " Scale_1 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Scale("SCALE_2", .TmpConfig & " Scale_2 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Scale("SCALE_3", .TmpConfig & " Scale_3 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Scale("SCALE_6", .TmpConfig & " Scale_6 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Scale("SCALE_7", .TmpConfig & " Scale_7 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Surgebin("SURGEBIN_1", .TmpConfig & " SurgeBin_1 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Surgebin("SURGEBIN_4", .TmpConfig & " SurgeBin_4 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Surgebin("SURGEBIN_5", .TmpConfig & " SurgeBin_5 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Handadd("HANDADD_1", .TmpConfig & " Handadd_1 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Mixer("MIXER_1", .TmpConfig & " Mixer1 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Liquid("LIQUID_1", .TmpConfig & " Liquid_1 " & UserLogon_.UserName & " BATCHING_1")
                    'io.Open_Application_For_Alarm_Liquid("LIQUID_2", .TmpConfig & " Liquid_2 " & UserLogon_.UserName & " BATCHING_1")
                End With
            End If

        Catch ex As Exception
            Try : LogError.writeErr("Custom Open App: " & ex.Message) : Catch : End Try
        End Try
    End Sub

    Public Sub Custom_CloseApp()
        Try
            'io.CloseEXE("TAT01_START_BATCHING_1_MIXER_1")
            'io.CloseEXE("Job_Assignment")
            'io.CloseEXE("GET_REPORT_BATCHING_MIX1")
            'io.CloseEXE("TAT01_SCALE_1_PARAMETER_ALARM_SCALE")
            'io.CloseEXE("TAT01_SCALE_2_PARAMETER_ALARM_SCALE")
            'io.CloseEXE("TAT01_SCALE_3_PARAMETER_ALARM_SCALE")
            'io.CloseEXE("TAT01_SCALE_6_PARAMETER_ALARM_SCALE")
            'io.CloseEXE("TAT01_SCALE_7_PARAMETER_ALARM_SCALE")
            'io.CloseEXE("TAT01_MIXER_1_PARAMETER_ALARM_MIXER")
            'io.CloseEXE("TAT01_SURGEBIN_1_PARAMETER_ALARM_SURGEBIN")
            'io.CloseEXE("TAT01_SURGEBIN_4_PARAMETER_ALARM_SURGEBIN")
            'io.CloseEXE("TAT01_SURGEBIN_5_PARAMETER_ALARM_SURGEBIN")
            'io.CloseEXE("TAT01_HANDADD_1_PARAMETER_ALARM_HANDADD")
            'io.CloseEXE("TAT01_LIQUID_1_PARAMETER_ALARM_LIQUID")
            'io.CloseEXE("TAT01_LIQUID_2_PARAMETER_ALARM_LIQUID")
            If frmAlarm_Des_New IsNot Nothing Then frmAlarm_Des_New.Close()
        Catch ex As Exception
            Try : LogError.writeErr("Custom Close App: " & ex.Message) : Catch : End Try
        End Try
    End Sub
#End Region

End Class