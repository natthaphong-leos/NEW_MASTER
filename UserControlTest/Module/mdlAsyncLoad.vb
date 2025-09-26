Imports System.Threading
Imports System.Threading.Tasks
Imports System.Reflection
Imports System.IO
Imports System.Windows.Forms

Module mdlAsyncLoad
    Private appMutex As Mutex
    Private restartRequested As Boolean = False

    <STAThread>
    Sub Main()
        Do
            restartRequested = False
            RunApplication()
        Loop While restartRequested
    End Sub

    Private Sub RunApplication()
        Dim createdNew As Boolean

        Dim asm = Assembly.GetExecutingAssembly()
        Dim mutexName As String = $"Local\{asm.GetName().Name}"

        appMutex = New Mutex(True, mutexName, createdNew)
        If Not createdNew Then Return

        Try
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            AddHandler Application.ThreadException, AddressOf OnThreadException
            AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf OnUnhandledException
            Application.Run(New AppBootstrapContext())
        Finally
            Try : appMutex.ReleaseMutex() : Catch : End Try
        End Try
    End Sub

    Friend Sub RequestRestart()
        restartRequested = True
    End Sub

    Private Sub OnThreadException(sender As Object, e As Threading.ThreadExceptionEventArgs)
        Try
            MessageBox.Show("Unexpected error: " & e.Exception.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch
        End Try
    End Sub

    Private Sub OnUnhandledException(sender As Object, e As UnhandledExceptionEventArgs)
        Try
            Dim ex = TryCast(e.ExceptionObject, Exception)
            If ex IsNot Nothing Then
                MessageBox.Show("Fatal error: " & ex.Message,
                                "Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch
        End Try
    End Sub

    Private idleTcs As TaskCompletionSource(Of Object)

    Friend Function WaitForIdleAsync() As Task
        idleTcs = New TaskCompletionSource(Of Object)()
        AddHandler Application.Idle, AddressOf OnIdleOnce
        Return idleTcs.Task
    End Function

    Private Sub OnIdleOnce(sender As Object, e As EventArgs)
        RemoveHandler Application.Idle, AddressOf OnIdleOnce
        If idleTcs IsNot Nothing Then
            idleTcs.TrySetResult(Nothing)
            idleTcs = Nothing
        End If
    End Sub
End Module

Public Class AppBootstrapContext
    Inherits ApplicationContext

    Private ReadOnly splash As New LOAD()
    Private main As MDI_FRM

    Public Sub New()
        AddHandler splash.Shown, AddressOf OnSplashShown
        splash.Show()
    End Sub

    Private Async Sub OnSplashShown(sender As Object, e As EventArgs)
        Try
            SetSplash("Preparing the system...")
            splash.SetProgress(5)

            main = New MDI_FRM()

            Dim init = main.GetType().GetMethod(
                "InitializeAsync",
                BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic,
                Nothing,
                New Type() {GetType(LOAD)},
                Nothing
            )
            If init IsNot Nothing Then
                Dim task = TryCast(init.Invoke(main, New Object() {splash}), Task)
                If task IsNot Nothing Then Await task
            End If

            Me.MainForm = main

            If main.StartPosition = FormStartPosition.Manual Then
                main.StartPosition = FormStartPosition.CenterScreen
            End If

            main.WindowState = FormWindowState.Normal
            main.Show()

            Dim showPages = main.GetType().GetMethod(
                "ShowPagesAfterShown",
                BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic)
            If showPages IsNot Nothing Then
                showPages.Invoke(main, Nothing)
            End If

            Await WaitFormsReadyAsync(main, TimeSpan.FromSeconds(5))

            splash.SetStatus("Scada System Ready...")
            splash.SetProgress(100)
            Application.DoEvents()
            Await Task.Delay(1000)

            splash.Close()
            main.Activate()
            main.BringToFront()
            main.Focus()

        Catch ex As RestartRequestedException
            Try : splash.Close() : Catch : End Try
            mdlAsyncLoad.RequestRestart()
            Application.Exit()

        Catch ex As OperationCanceledException
            Try : splash.Close() : Catch : End Try
            Application.Exit()

        Catch ex As Exception
            MessageBox.Show("System startup failed:" & vbCrLf & ex.Message,
                            "Fatal", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Try : splash.Close() : Catch : End Try
            Application.Exit()
        End Try
    End Sub

    Private Async Function WaitFormsReadyAsync(mdi As MDI_FRM, timeout As TimeSpan) As Task
        Dim kids = mdi.MdiChildren
        Dim targets As Form()
        If kids IsNot Nothing AndAlso kids.Length > 0 Then
            targets = kids
        Else
            targets = New Form() {mdi}
        End If

        Dim tasks As New List(Of Task)
        For Each f In targets
            tasks.Add(WaitFirstPaintAsync(f))
        Next

        Dim all = Task.WhenAll(tasks)
        Await Task.WhenAny(all, Task.Delay(timeout))
    End Function

    Private Function WaitFirstPaintAsync(f As Form) As Task
        Dim tcs As New TaskCompletionSource(Of Object)()
        Dim painted As Boolean = False

        If f.Visible Then
            f.Invalidate()
            f.Update()
        End If

        Dim paintHandler As PaintEventHandler = Nothing
        Dim disposedHandler As EventHandler = Nothing

        paintHandler =
        Sub(sender As Object, e As PaintEventArgs)
            If painted Then Return
            painted = True
            RemoveHandler f.Paint, paintHandler
            RemoveHandler f.Disposed, disposedHandler
            tcs.TrySetResult(Nothing)
        End Sub

        disposedHandler =
        Sub(sender As Object, e As EventArgs)
            If Not painted Then
                RemoveHandler f.Paint, paintHandler
                RemoveHandler f.Disposed, disposedHandler
                tcs.TrySetResult(Nothing)
            End If
        End Sub

        AddHandler f.Paint, paintHandler
        AddHandler f.Disposed, disposedHandler

        Return tcs.Task
    End Function

    Private Sub SetSplash(msg As String)
        If splash Is Nothing OrElse splash.IsDisposed Then Return
        Try
            splash.SetStatus(msg)
        Catch
        End Try
    End Sub
End Class

Public Class RestartRequestedException
    Inherits Exception
    Public Sub New()
        MyBase.New("Application restart requested")
    End Sub
End Class