Option Strict On
Option Explicit On
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Windows.Forms

Public Class frmUpdateNotification

    ' === Event ส่งกลับให้ MDI เรียก PerformUpdateAsync ===
    Public Event UpdateRequested As EventHandler
    Public Event UpdateSkipped As EventHandler

    Public Sub New()
        InitializeComponent()
    End Sub

    ' ===== 1. วาดไอคอน "i" =====
    Private Sub ApplyInfoIcon(pb As PictureBox)
        If pb Is Nothing Then Exit Sub
        Dim w As Integer = If(pb.Width > 0, pb.Width, 40)
        Dim h As Integer = If(pb.Height > 0, pb.Height, 40)
        Dim bmp As New Bitmap(w, h)

        Using g As Graphics = Graphics.FromImage(bmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            Dim d As Integer = Math.Min(w, h)
            g.FillEllipse(Brushes.White, 2, 2, d - 4, d - 4)

            Dim fontSize As Single = CSng(Math.Max(10, d * 0.55))
            Using f As New Font("Segoe UI", fontSize, FontStyle.Bold)
                Dim sf As New StringFormat With {
                    .Alignment = StringAlignment.Center,
                    .LineAlignment = StringAlignment.Center
                }
                Using br As New SolidBrush(Color.DodgerBlue)
                    g.DrawString("i", f, br, New RectangleF(0, 0, w, h), sf)
                End Using
            End Using
        End Using

        pb.Image = bmp
    End Sub

    ' ===== 2. แยกชื่อไฟล์ =====
    Private Sub ParseUpdateFileName(input As String,
                                   ByRef programName As String,
                                   ByRef details As String)
        programName = "-"
        details = "-"

        If String.IsNullOrWhiteSpace(input) Then Exit Sub

        Dim onlyName As String = Path.GetFileName(input)
        Dim withoutExt As String = Path.GetFileNameWithoutExtension(onlyName)
        Dim parts As String() = withoutExt.Split(New Char() {"_"c}, 3, StringSplitOptions.None)

        If parts.Length >= 1 Then programName = parts(0).Trim()
        If parts.Length = 3 Then
            details = parts(2).Trim()
        ElseIf parts.Length = 2 Then
            details = parts(1).Trim()
        End If
    End Sub

    ' ===== 3. แปลงขนาดไฟล์ =====
    Private Function FormatFileSize(bytes As Long) As String
        Dim kb As Double = bytes / 1024.0
        Dim mb As Double = kb / 1024.0

        If mb >= 1 Then
            Return $"{mb:F2} MB"
        ElseIf kb >= 1 Then
            Return $"{kb:F2} KB"
        Else
            Return $"{bytes} bytes"
        End If
    End Function

    ' ===== 4. ตั้งค่าข้อมูล Label =====
    Public Sub SetUpdateInfo(fileName As String,
                             fileDate As DateTime,
                             fileSize As Long)
        Dim sizeText As String = FormatFileSize(fileSize)
        Dim dateText As String = fileDate.ToString("yyyy-MM-dd HH:mm:ss")

        Dim prog As String
        Dim details As String
        ParseUpdateFileName(fileName, prog, details)

        lblFileName.Text = $"File: {prog}"
        lblFileDetails.Text = $"Details: {details}"
        lblFileSize.Text = $"Size: {sizeText}"
        lblFileDate.Text = $"Date: {dateText}"
    End Sub

    ' ===== 5. โหลดฟอร์ม =====
    Private Sub frmUpdateNotification_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyInfoIcon(picIcon)

        ' ให้เด้งกลางจอเสมอ
        Me.StartPosition = FormStartPosition.CenterScreen
    End Sub

    ' ===== 6. ปุ่ม Update Now =====
    Private Sub btnUpdateNow_Click(sender As Object, e As EventArgs) Handles btnUpdateNow.Click
        RaiseEvent UpdateRequested(Me, EventArgs.Empty)
        Me.Close()
    End Sub

    ' ===== 7. ปุ่ม Update Later =====
    Private Sub btnUpdateLater_Click(sender As Object, e As EventArgs) Handles btnUpdateLater.Click
        RaiseEvent UpdateSkipped(Me, EventArgs.Empty)
        Me.Close()
    End Sub

End Class
