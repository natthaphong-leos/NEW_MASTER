Imports System.Diagnostics

Public NotInheritable Class LOAD
    Private Const TIMER_INTERVAL_MS As Integer = 15
    Private Const MIN_STEP_UI As Integer = 1

    Private _isPaused As Boolean = False

    Private _jumpActive As Boolean = False
    Private _jumpFrom As Double = 0.0
    Private _jumpTo As Double = 0.0
    Private _jumpDurationMs As Integer = 220
    Private _jumpClock As Stopwatch

    Private _lastPaintedValue As Integer = -1

    Private Sub LOAD_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.ControlBox = False
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.TopMost = True
        Me.ShowInTaskbar = False

        Try
            Version.Text = String.Format(Version.Text,
                                         My.Application.Info.Version.Major,
                                         My.Application.Info.Version.Minor)
        Catch
        End Try

        Try
            ProgressBar1.Style = ProgressBarStyle.Continuous
            ProgressBar1.Minimum = 0
            ProgressBar1.Maximum = 100
            ProgressBar1.Value = 0
            _lastPaintedValue = -1
            UpdatePercentUI()
        Catch
        End Try

        Timer1.Interval = TIMER_INTERVAL_MS
        Me.Refresh()
        Application.DoEvents()
    End Sub

    '==================== Status & Progress Management ====================

    Public Sub SetStatus(text As String)
        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Return
        Try
            If Me.InvokeRequired Then
                Me.BeginInvoke(CType(Sub() SafeSetStatus(text), MethodInvoker))
            Else
                SafeSetStatus(text)
            End If
        Catch
        End Try
    End Sub

    Private Sub SafeSetStatus(text As String)
        Try
            If lblStatus IsNot Nothing AndAlso Not lblStatus.IsDisposed Then
                lblStatus.Text = text
                lblStatus.Refresh()
            End If
        Catch
        End Try
    End Sub

    '==================== กระโดด + สมูทสั้น ๆ ====================
    Public Sub SetProgress(percent As Integer)
        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Return
        Try
            If Me.InvokeRequired Then
                Me.BeginInvoke(CType(Sub() SafeSetProgressSmoothJump(percent), MethodInvoker))
            Else
                SafeSetProgressSmoothJump(percent)
            End If
        Catch
        End Try
    End Sub

    Private Sub SafeSetProgressSmoothJump(percent As Integer)
        Try
            If ProgressBar1 Is Nothing OrElse ProgressBar1.IsDisposed Then Return

            Dim target As Integer = Math.Max(0, Math.Min(100, percent))
            Dim current As Integer = ProgressBar1.Value

            If target = current Then
                PaintProgress(current)
                Exit Sub
            End If

            _jumpFrom = current
            _jumpTo = target

            Dim dist As Integer = Math.Abs(target - current)
            If dist <= 5 Then
                _jumpDurationMs = 160
            ElseIf dist <= 20 Then
                _jumpDurationMs = 220
            Else
                _jumpDurationMs = 320
            End If

            _jumpActive = True
            If _jumpClock Is Nothing Then _jumpClock = Stopwatch.StartNew() Else _jumpClock.Restart()

            If Not Timer1.Enabled Then
                Timer1.Start()
            End If
        Catch
        End Try
    End Sub

    '==================== กระโดดทันที (ไม่มีสมูท) ====================
    Public Sub SetProgressImmediate(percent As Integer)
        If Not Me.IsHandleCreated OrElse Me.IsDisposed Then Return
        Try
            If Me.InvokeRequired Then
                Me.BeginInvoke(CType(Sub() SafeSetProgressImmediate(percent), MethodInvoker))
            Else
                SafeSetProgressImmediate(percent)
            End If
        Catch
        End Try
    End Sub

    Private Sub SafeSetProgressImmediate(percent As Integer)
        Try
            Dim p As Integer = Math.Max(0, Math.Min(100, percent))
            _jumpActive = False
            PaintProgress(p, force:=True)
            If Timer1.Enabled Then Timer1.Stop()
        Catch
        End Try
    End Sub

    Public Sub PauseProgress()
        _isPaused = True
        _jumpActive = False
    End Sub

    Public Sub ResumeProgress()
        _isPaused = False
    End Sub

    '==================== Timer: ทำงานเฉพาะตอนมีอนิเมชัน jump ====================
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            If ProgressBar1 Is Nothing OrElse ProgressBar1.IsDisposed Then Return
            If _isPaused Then Return

            If _jumpActive Then
                Dim t As Double = _jumpClock.Elapsed.TotalMilliseconds / Math.Max(1, _jumpDurationMs)
                If t >= 1.0 Then
                    _jumpActive = False
                    PaintProgress(CInt(Math.Round(_jumpTo)), force:=True)
                Else
                    Dim eased As Double = 1.0 - Math.Pow(1.0 - t, 3.0)
                    Dim val As Double = _jumpFrom + (_jumpTo - _jumpFrom) * eased
                    PaintProgress(CInt(Math.Round(val)))
                End If
            Else
                If Timer1.Enabled Then Timer1.Stop()
            End If
        Catch
        End Try
    End Sub

    '==================== วาดค่าและตัวเลข % ====================
    Private Sub PaintProgress(value As Integer, Optional force As Boolean = False)
        Try
            value = Math.Max(0, Math.Min(100, value))

            If Not force AndAlso value = _lastPaintedValue Then Exit Sub

            If value < ProgressBar1.Value Then
                ProgressBar1.Value = ProgressBar1.Minimum
            ElseIf value > ProgressBar1.Value Then
                Dim nudge As Integer = Math.Min(value + 1, ProgressBar1.Maximum)
                ProgressBar1.Value = nudge
            End If

            ProgressBar1.Value = value
            ProgressBar1.Refresh()

            _lastPaintedValue = value
            UpdatePercentUI()
        Catch
            Try
                ProgressBar1.Value = Math.Max(ProgressBar1.Minimum, Math.Min(ProgressBar1.Maximum, value))
                ProgressBar1.Refresh()
            Catch
            End Try
        End Try
    End Sub

    Private Sub UpdatePercentUI()
        Try
            If lblLoadPercent Is Nothing OrElse lblLoadPercent.IsDisposed Then Return
            If ProgressBar1 Is Nothing OrElse ProgressBar1.IsDisposed Then Return

            lblLoadPercent.Text = ProgressBar1.Value.ToString() & "%"
            lblLoadPercent.Refresh()
        Catch
        End Try
    End Sub
End Class
