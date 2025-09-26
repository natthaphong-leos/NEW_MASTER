Imports System.Windows.Forms
Imports System.Runtime.InteropServices

' ล็อกอินพุตเฉพาะบนฟอร์มที่กำหนด + แปะแผ่น Overlay ทับพื้นที่ทำงาน (MDI หรือทั้งฟอร์ม)
Friend NotInheritable Class UiInputGuard
    Implements IMessageFilter, IDisposable

    Private ReadOnly _host As Form
    Private ReadOnly _overlay As Panel
    Private ReadOnly _label As Label
    Private _installed As Boolean

    ' เมสเสจเมาส์/คีย์บอร์ดหลัก ๆ ที่ต้องบล็อก
    Private Shared ReadOnly BLOCKS As Integer() = {
        &H200,  ' WM_MOUSEMOVE
        &H201,  ' WM_LBUTTONDOWN
        &H202,  ' WM_LBUTTONUP
        &H203,  ' WM_LBUTTONDBLCLK
        &H204,  ' WM_RBUTTONDOWN
        &H205,  ' WM_RBUTTONUP
        &H20A,  ' WM_MOUSEWHEEL
        &H20B,  ' WM_XBUTTONDOWN
        &H20C,  ' WM_XBUTTONUP
        &H100,  ' WM_KEYDOWN
        &H101,  ' WM_KEYUP
        &H102   ' WM_CHAR
    }

    Public Sub New(host As Form, Optional overlayText As String = "Loading... Please wait")
        _host = host

        ' ----- สร้าง Overlay -----
        _overlay = New Panel() With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.FromArgb(80, Color.Gray), ' เทาโปร่งแสง
            .Visible = False,
            .Cursor = Cursors.AppStarting
        }

        _label = New Label() With {
            .AutoSize = True,
            .Font = New Font("Segoe UI", 12.0!, FontStyle.Bold),
            .ForeColor = Color.White,
            .BackColor = Color.Transparent,
            .Text = overlayText
        }
        _overlay.Controls.Add(_label)

        AddHandler _overlay.Resize, Sub() CenterLabel()

        Install()
    End Sub

    ' จัดกึ่งกลางข้อความบน overlay
    Private Sub CenterLabel()
        If _overlay Is Nothing OrElse _label Is Nothing Then Return
        _label.Left = Math.Max(0, (_overlay.Width - _label.Width) \ 2)
        _label.Top = Math.Max(0, (_overlay.Height - _label.Height) \ 2)
    End Sub

    ' วาง Overlay ทับ MdiClient ถ้ามี ไม่มีก็ทับทั้งฟอร์ม
    Private Sub Install()
        If _installed Then Return

        Dim mdiClient = _host.Controls.OfType(Of MdiClient)().FirstOrDefault()
        If mdiClient IsNot Nothing Then
            mdiClient.Controls.Add(_overlay)
        Else
            _host.Controls.Add(_overlay)
        End If
        _overlay.BringToFront()
        _overlay.Visible = True
        CenterLabel()

        _host.UseWaitCursor = True
        Application.AddMessageFilter(Me)
        _installed = True
    End Sub

    ' อัปเดตข้อความบน Overlay (เรียกจาก UI เท่านั้น; ถ้าเรียกจากแบ็กกราวด์ให้ใช้ SafeInvoke ของคุณ)
    Public Sub SetStatus(text As String)
        _label.Text = text
        CenterLabel()
    End Sub

    ' บล็อกเฉพาะเมสเสจที่มุ่งไปยังคอนโทรลภายใต้ host form เท่านั้น (จะไม่บล็อก Splash ที่อยู่อีกฟอร์ม)
    Public Function PreFilterMessage(ByRef m As Message) As Boolean Implements IMessageFilter.PreFilterMessage
        If Not _installed Then Return False
        If Not BLOCKS.Contains(m.Msg) Then Return False

        Dim target = Control.FromHandle(m.HWnd)
        If target Is Nothing Then Return False

        Dim targetForm = target.FindForm()
        If targetForm Is Nothing Then Return False

        ' บล็อกเมื่อเป้าหมายเป็น host เอง หรือคอนโทรลลูก ๆ ของ host
        If targetForm Is _host OrElse (target IsNot Nothing AndAlso (_host Is target OrElse _host.Contains(target))) Then
            Return True
        End If

        Return False
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        If Not _installed Then Return
        Application.RemoveMessageFilter(Me)
        _installed = False

        Try
            _overlay.Visible = False
            _overlay.Parent?.Controls.Remove(_overlay)
            _overlay.Dispose()
        Catch
        End Try

        _host.UseWaitCursor = False
    End Sub
End Class