Imports System.Drawing.Drawing2D

Public Class TatLine
    Public Property Id As Guid = Guid.NewGuid()
    Public Property Name As String = ""
    Public Property Points As New List(Of PointF)
    Public Property Color As Color = Color.DodgerBlue
    Public Property Width As Single = 2.0F
    Public Property Visible As Boolean = True
    Public Property DashStyle As DashStyle = DashStyle.Solid
    Public Property Tag As Object = Nothing
    Public Property Condition As String = "" ' เงื่อนไขแสดงผล (ถ้ามี)

    Public Property IsAdjusting As Boolean ' เข้าโหมดปรับแต่ง

    Public Property ManageMode As Boolean = False 'โหมดดีไซน์ ไว้สำหรับแสดงความแตกต่างของเส้นที่ยังไม่ใส่เงื่อนไข


    ' backup เพื่อ Cancel ได้
    Private _backupPoints As List(Of PointF)

    'Public Sub New()
    'End Sub

    Public Sub New(points As IEnumerable(Of PointF))
        Me.Points = New List(Of PointF)(points)
    End Sub

    ' ---- เริ่ม/ยกเลิก/ยืนยัน โหมดปรับแต่ง ----
    Public Sub BeginAdjust()
        _backupPoints = Points.Select(Function(p) p).ToList()
        IsAdjusting = True
    End Sub

    Public Sub CancelAdjust()
        If _backupPoints IsNot Nothing Then
            Points = _backupPoints.Select(Function(p) p).ToList()
        End If
        _backupPoints = Nothing
        IsAdjusting = False
    End Sub

    Public Sub ApplyAdjust()
        _backupPoints = Nothing
        IsAdjusting = False
        ' เส้นกลับไปเป็น “เส้นทึบตามปกติ” หลังปรับจุดเสร็จ
    End Sub

    Public Sub Draw(g As Graphics)
        If Not Visible OrElse Points.Count < 2 Then
            Return
        End If

        If IsAdjusting Then
            ' วาดเส้นประ + จุดแก้ไข
            Using pen As New Pen(Color.Red, 1.0F)
                pen.DashStyle = DashStyle.Dash
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.DrawLines(pen, Points.ToArray())
            End Using


            ' 3) วาด working points + live preview
            Using br As New SolidBrush(Color.Red),
                  pen As New Pen(Color.Black, 1.0F),
                  f As New Font("Segoe UI", 9.0F, FontStyle.Bold)
                Dim POINT_RADIUS As Single = 5.0F
                For i = 0 To Points.Count - 1
                    Dim c = Points(i)
                    Dim r As New RectangleF(c.X - POINT_RADIUS, c.Y - POINT_RADIUS, POINT_RADIUS * 2, POINT_RADIUS * 2)
                    g.FillEllipse(br, r)
                    g.DrawEllipse(pen, r)
                    g.DrawString((i + 1).ToString(), f, Brushes.Black, c.X + POINT_RADIUS + 3, c.Y - POINT_RADIUS - 3)
                Next
            End Using
        Else
            If ManageMode AndAlso Condition = "" Then 'โหมดดีไซน์ และยังไม่ใส่เงื่อนไข
                Using pen As New Pen(Color.White, Width)
                    pen.DashStyle = DashStyle
                    For i = 0 To Points.Count - 2
                        g.DrawLine(pen, Points(i), Points(i + 1))
                    Next
                End Using
            Else
                Using pen As New Pen(Color, Width)
                    pen.DashStyle = DashStyle
                    For i = 0 To Points.Count - 2
                        g.DrawLine(pen, Points(i), Points(i + 1))
                    Next
                End Using
            End If
        End If


    End Sub

    ' ตรวจว่าคลิกโดนเส้นหรือไม่ (ระยะชน)
    Public Function HitTest(p As PointF, Optional tolerance As Single = 6.0F) As Boolean
        If Points.Count < 2 OrElse Not Visible Then Return False
        For i = 0 To Points.Count - 2
            If DistancePointToSegment(p, Points(i), Points(i + 1)) <= tolerance Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function GetBounds() As RectangleF
        If Points.Count = 0 Then Return RectangleF.Empty
        Dim minX = Points.Min(Function(pt) pt.X)
        Dim minY = Points.Min(Function(pt) pt.Y)
        Dim maxX = Points.Max(Function(pt) pt.X)
        Dim maxY = Points.Max(Function(pt) pt.Y)
        Return RectangleF.FromLTRB(minX, minY, maxX, maxY)
    End Function

    Private Shared Function DistancePointToSegment(p As PointF, a As PointF, b As PointF) As Single
        Dim vx = b.X - a.X
        Dim vy = b.Y - a.Y
        Dim wx = p.X - a.X
        Dim wy = p.Y - a.Y

        Dim c1 = vx * wx + vy * wy
        If c1 <= 0 Then
            Return CSng(Math.Sqrt((p.X - a.X) ^ 2 + (p.Y - a.Y) ^ 2))
        End If

        Dim c2 = vx * vx + vy * vy
        If c2 <= c1 Then
            Return CSng(Math.Sqrt((p.X - b.X) ^ 2 + (p.Y - b.Y) ^ 2))
        End If

        Dim t = c1 / c2 ' projection factor
        Dim projX = a.X + t * vx
        Dim projY = a.Y + t * vy
        Return CSng(Math.Sqrt((p.X - projX) ^ 2 + (p.Y - projY) ^ 2))
    End Function
End Class
