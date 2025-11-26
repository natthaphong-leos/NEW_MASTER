Imports System.Windows.Forms
Imports System.Drawing

Public Class frmUpdateProgress
    Inherits Form
    Private lblInfo As Label
    Private progress As ProgressBar
    Private ReadOnly _anim As Timer
    Private _target As Integer = 0
    Private _current As Integer = 0

    Public Sub New()
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.ControlBox = False
        Me.Width = 400
        Me.Height = 120
        Me.TopMost = True
        Me.Text = "Updating SCADA System..."

        lblInfo = New Label() With {
            .Text = "System is updating. Please wait a moment...",
            .Dock = DockStyle.Top,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Height = 50,
            .Font = New Font("Segoe UI", 12, FontStyle.Regular),
            .Padding = New Padding(10)
        }

        progress = New ProgressBar() With {
            .Style = ProgressBarStyle.Continuous,
            .Minimum = 0,
            .Maximum = 1000,
            .Value = 0,
            .Dock = DockStyle.Bottom,
            .Height = 15
        }

        Me.Controls.Add(progress)
        Me.Controls.Add(lblInfo)

        _anim = New Timer() With {.Interval = 15}
        AddHandler _anim.Tick, AddressOf OnAnimTick
        _anim.Start()
    End Sub

    Private Sub OnAnimTick(sender As Object, e As EventArgs)
        If _current <> _target Then
            Dim delta = _target - _current
            Dim stepMove As Integer = Math.Max(1, Math.Abs(delta) \ 6)
            If delta > 0 Then
                _current = Math.Min(_target, _current + stepMove)
            Else
                _current = Math.Max(_target, _current - stepMove)
            End If
            SafeSetProgressValue(_current)
        End If
    End Sub

    Private Sub SafeSetProgressValue(v As Integer)
        v = Math.Max(progress.Minimum, Math.Min(progress.Maximum, v))

        If v < progress.Value Then
            Dim temp = Math.Min(progress.Maximum, v + 1)
            If temp > progress.Value Then progress.Value = temp
        End If

        progress.Value = v
    End Sub

    Public Sub SetProgress(pct As Integer, message As String)
        If Me.IsDisposed Then Return

        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(Of Integer, String)(AddressOf SetProgress), pct, message)
            Return
        End If

        If pct < 0 Then pct = 0
        If pct > 100 Then pct = 100
        _target = pct * 10

        If Not String.IsNullOrEmpty(message) Then
            lblInfo.Text = message
        End If
    End Sub

    Public Sub Done(Optional message As String = "Update completed.")
        If Me.IsDisposed Then Return

        If Me.InvokeRequired Then
            Me.BeginInvoke(New Action(Of String)(AddressOf Done), message)
            Return
        End If

        _target = progress.Maximum
        lblInfo.Text = message
    End Sub
End Class
