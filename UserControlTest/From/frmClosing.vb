Public Class frmClosing
    Inherits Form

    Private lblInfo As Label
    Private progress As ProgressBar

    Public Sub New()
        Me.FormBorderStyle = FormBorderStyle.FixedDialog
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.ControlBox = False
        Me.Width = 400
        Me.Height = 120
        Me.TopMost = True

        lblInfo = New Label()
        lblInfo.Text = "Closing Scada System, please wait..."
        lblInfo.Dock = DockStyle.Top
        lblInfo.TextAlign = ContentAlignment.MiddleCenter
        lblInfo.Height = 50
        lblInfo.Font = New Font("Segoe UI", 12, FontStyle.Regular)

        progress = New ProgressBar()
        progress.Style = ProgressBarStyle.Marquee
        progress.MarqueeAnimationSpeed = 20
        progress.Dock = DockStyle.Bottom
        progress.MaximumSize = New Size(0, 15)

        Me.Controls.Add(progress)
        Me.Controls.Add(lblInfo)
    End Sub
End Class
