Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Globalization
Imports System.Drawing.Drawing2D '======== ADD BY APICHAT FOR DRAW LINE
Imports System.Reflection       '======== ADD BY APICHAT FOR DRAW LINE

Public Class frm_Page_1
#Region "VARIABLES"
    Dim Error_Check As Boolean
    Dim LogError As New clsIO
    Dim Loot_time As Int16 = 0
    Public Scada As New cls_scada_Control(Me)
    Dim TmpDataObj(4) As Int16
    Dim TmpDataObj2W(4) As Int32
    Dim f_validate As Boolean
    Dim isRed As Boolean = True
#End Region
#Region "INITIALIZATION"
    Public Sub LoadForm()
        '============ FOR DRAW LINE BY APICHAT ============
        Me.DoubleBuffered = True
        AddHandler Me.Paint, AddressOf Form1_Paint
        AddHandler Me.MouseDown, AddressOf Form1_MouseDown
        AddHandler Me.MouseMove, AddressOf Form1_MouseMove
        ' AddHandler Me.MouseUp, AddressOf Form1_MouseUp
        '======================= END =======================

        Scada.Connect_MQTT_Scada()
        ' Scada = New cls_scada_Control(Me)
        Me.Cursor = Cursors.Default

        For Each ctrl As Control In Me.Controls
            If TypeOf (ctrl) Is Line.Line Then
                ctrl.Hide()
            End If
        Next

        If Scada.iStatusMqtt And Scada.iStatusPLC Then
            Try
                CheckStatusReal = Scada.cStatusTCP
                Scada.Get_Property(Me)
                Timer_Run.Enabled = True

                Me.Enabled = True
            Catch ex As Exception
                '============ Msg Error
                If Error_Check = True Then Exit Sub
                Error_Check = True
                Dim strMessage = "Error Number :  " & Err.Number & " [" + Me.Name + "]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
                LogError.writeErr(strMessage)
            End Try
        Else
            Me.Enabled = False
            CheckStatusReal = Scada.cStatusTCP
        End If
        '============ For multi language
        AddHandler Langauge_Change, AddressOf ChangeLanguage_InForm
        ChangeLanguage_InForm(MDI_FRM.tsCboLanguage.Text, CnBatching)
        '============ For Test IO
        'If Mode_Test_IO = True Then
        '    'Auto_Verify_Object(Me, CnBatching)

        '    Dim Verify As New clsVerifyObject(Me, CnBatching)
        '    Verify.Verify_Object()
        'End If
    End Sub

    '============ For multi language
    Private Sub ChangeLanguage_InForm(ByVal strLangCode As String, ByRef cnDB As clsDB)
        change_language_in_collection(Me.Controls, cnDB, strLangCode, Application.ProductName, Me.Name)
        change_Object_InterlockMsg_In_collection(Me.Controls, cnDB, strLangCode)
        change_Object_Control_In_collection(Me.Controls, cnDB, strLangCode, Application.ProductName, Me.Name)
    End Sub
#End Region
#Region "FORM EVENT"
    Private Sub frm_Page_1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            If Me.BackgroundImage IsNot Nothing Then
                Me.Size = Me.BackgroundImage.Size
            Else
                Me.Size = New Size(1920, 1080)
            End If
        Catch ex As Exception
            Me.Size = New Size(1920, 1080)
        End Try
        Call LoadForm()
    End Sub

    Private Sub frm_Page_1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        '============ For Test IO
        If Mode_Test_IO = True Then
            pnlTest_IO.Visible = True
            Dim Verify As New clsVerifyObject(Me, CnBatching)
            Verify.Verify_Object()
            'Set_Obj_Mode_TestIO(Me.Controls)
        End If

        '======== ADD BY APICHAT FOR DRAW LINE
        Dim loaded = LoadTatLinesDB(Me.Name, CnBatching)
        If loaded IsNot Nothing AndAlso loaded.Count > 0 Then
            _lines.Clear()
            _lines.AddRange(loaded)
            Invalidate()
        End If
    End Sub
#End Region
#Region "RUNTIME STATUS"
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer_Run.Tick
        Try
            If BW_ReadPLC.IsBusy = False Then
                BW_ReadPLC.RunWorkerAsync()

                Call CheckStatusForScada()

            End If
            btn_remote.Text = RemoteControl
            If UCase(RemoteControl) = "REMOTE CONTROL" Then
                btn_remote.BackColor = Color.LimeGreen
            ElseIf UCase(RemoteControl) = "LOCAL CONTROL" Then
                btn_remote.BackColor = Color.Red
            Else
                btn_remote.BackColor = Color.DimGray
            End If

            Call Show_AnalogInForm(Me, Scada.MqttAnalogData)
            Call Show_PidInForm(Me, Scada.MqttAnalogData, Scada.Mqtt_M_Data, Scada.ctrlPID_Changing, Scada.ctrlPID_Delay)
            Call FunctionWithTimer()

        Catch ex As Exception
            '==== Msg Error
            If Error_Check = True Then Exit Sub
            Error_Check = True
            Dim strMessage = "Error Number :  " & Err.Number & " [" & Me.Name & "]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Public Sub CheckStatusForScada()
        If Scada.iStatusMqtt And Scada.iStatusPLC Then
            CheckStatusReal = Scada.cStatusTCP
            Me.Enabled = True
        Else
            CheckStatusReal = Scada.cStatusTCP
            Me.Enabled = False
        End If
    End Sub
#End Region
#Region "READ PLC"
    Private Sub BW_ReadPLC_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BW_ReadPLC.DoWork
        Try
            'TmpDataObj(0) = Scada.MqttAnalogData(28233)
            'TmpDataObj(1) = Scada.MqttAnalogData(28234)
            'Buffer.BlockCopy(TmpDataObj, 0 * 2, TmpDataObj2W, 0, 4)
        Catch ex As Exception
            '==== Msg Error
            If Error_Check = True Then Exit Sub
            Error_Check = True
            Dim strMessage = "Error Number :  " & Err.Number & " [" & Me.Name & "]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub

    Private Sub BW_ReadPLC_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BW_ReadPLC.RunWorkerCompleted
        Try
            Show_Line()
        Catch ex As Exception
            '==== Msg Error
            If Error_Check = True Then Exit Sub
            Error_Check = True
            Dim strMessage = "Error Number :  " & Err.Number & " [" + Me.Name + "]" & vbNewLine & "Error Description :  " & ex.Message & vbCrLf & "Error at : " & ex.StackTrace
            LogError.writeErr(strMessage)
        End Try
    End Sub
#End Region
#Region "TEST IO"
    '===================== FOR REPORT TEST IO
    Private Sub btnVerify_Click(sender As Object, e As EventArgs) Handles btnVerify.Click
        'Dim strTest As String = ""
        'strTest = $"{CtrlTAT_28.Location.X},{CtrlTAT_28.Location.Y}"
        'MsgBox(strTest)
        Dim Verify As New clsVerifyObject(Me, CnBatching)
        Verify.Verify_Object()
    End Sub
#End Region
#Region "DRAW LINE" '======== ADD BY APICHAT FOR DRAW LINE
    '============ MOVE PANEL DRAW LINE : BY NATTHAPHONG
    Private isDragging As Boolean = False
    Private startPoint As Point

    Private Sub pnlLine_MouseDown(sender As Object, e As MouseEventArgs) Handles pnlLine.MouseDown
        If e.Button = MouseButtons.Left Then
            isDragging = True
            ' เก็บตำแหน่งเมาส์ตอนเริ่มลาก
            startPoint = e.Location
        End If
    End Sub

    Private Sub pnlLine_MouseMove(sender As Object, e As MouseEventArgs) Handles pnlLine.MouseMove
        If isDragging Then
            ' คำนวณตำแหน่งใหม่ของ Panel จากตำแหน่งเมาส์ที่ขยับ
            Dim p As Point = pnlLine.Location
            p.X += e.X - startPoint.X
            p.Y += e.Y - startPoint.Y
            pnlLine.Location = p
        End If
    End Sub

    Private Sub pnlLine_MouseUp(sender As Object, e As MouseEventArgs) Handles pnlLine.MouseUp
        If e.Button = MouseButtons.Left Then
            isDragging = False
        End If
    End Sub
    '============ 

    Private Sub btnEditLine_Click(sender As Object, e As EventArgs)
        If Line_EditMode = True Then
            Line_EditMode = False
            HideAll_Line(Me.Controls)
            Scada.Reload_Line(Me)
        Else
            Line_EditMode = True
            ShowAll_Line(Me.Controls)
        End If
    End Sub

    Private Sub btnLineManage_Click(sender As Object, e As EventArgs) Handles btnLineManage.Click
        If LineManage_Mode = False Then
            LineManage_Mode = True
            btnLineManage.Text = "CLOSE LINE MANAGEMENT MODE"
            Show_AllLines()
            Timer_Line.Enabled = False
        Else
            '============= Ask For Save Here
            If _HasChanges Then
                If MsgBox("Do you want to save any changes?", MsgBoxStyle.YesNo, "SAVE CHANGES") = MsgBoxResult.Yes Then
                    btnSave.PerformClick()
                End If
            End If
            LineManage_Mode = False
            btnLineManage.Text = "OPEN LINE MANAGEMENT MODE"
            Timer_Line.Enabled = True
            _selectedLine = Nothing
            _selectedLineId = Guid.Empty
            If _drawMode Then
                btnDrawLine.PerformClick()
            End If

            If Ctrl_Hiden Then
                Show_Hiden_Control()
            End If



            Invalidate()
        End If

        pnlLine.Visible = LineManage_Mode
        Me.BeginInvoke(Sub()
                           Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                       End Sub)
    End Sub

    Private Sub btnHide_Control_Click(sender As Object, e As EventArgs) Handles btnHide_Control.Click
        If Ctrl_Hiden = False Then
            Hide_All_Control(Me)
            Ctrl_Hiden = True
            btnHide_Control.Text = "SHOW ALL CONTROLS"
        Else
            Show_Hiden_Control()
            Ctrl_Hiden = False
            btnHide_Control.Text = "HIDE ALL CONTROLS"
        End If
        Me.BeginInvoke(Sub()
                           Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                       End Sub)
    End Sub

    ' ----- STATE -----
    Public LineManage_Mode As Boolean = False

    Private _drawMode As Boolean = False

    ' เส้นที่สร้างเสร็จแล้ว (เป็น object)
    Public ReadOnly _lines As New List(Of TatLine)

    ' เส้นที่กำลังวาด (ยังไม่ generate)
    Private ReadOnly _working As New List(Of PointF)

    ' สไตล์เริ่มต้นสำหรับเส้นใหม่ (ปรับได้ runtime)
    Private _activeColor As Color = Color.Lime
    Private _activeWidth As Single = 3.0F
    Private _activeDash As DashStyle = DashStyle.Solid

    Private _mousePos As PointF = PointF.Empty
    Private Const POINT_RADIUS As Single = 5.0F
    Private Const HIT_RADIUS As Single = 8.0F

    Private _selectedLineId As Guid = Guid.Empty ' เส้นที่ถูกเลือก (ถ้าต้องการเลือกด้วยการคลิก)
    Private _selectedLine As TatLine = Nothing

    Private _RunMode As Boolean = False ' โหมดรัน (ถ้าใช้ Timer ตรวจเงื่อนไข)
    Private _ModifyMode As Boolean = False ' โหมดแก้ไขเส้นที่เลือก
    Private _HasChanges As Boolean = False ' มีการเปลี่ยนแปลงเส้นหรือไม่

    Private Sub btnDrawLine_Click(sender As Object, e As EventArgs) Handles btnDrawLine.Click
        _drawMode = Not _drawMode
        btnDrawLine.Text = If(_drawMode, "Exit Draw", "Draw Mode")
        Me.Cursor = If(_drawMode, Cursors.Cross, Cursors.Default)
        _working.Clear()
        Me.BeginInvoke(Sub()
                           Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                       End Sub)
        Invalidate()
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        GenerateCurrentAsLine()
        Me.BeginInvoke(Sub()
                           Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                       End Sub)
    End Sub

    Private Sub GenerateCurrentAsLine()
        Dim newLine As TatLine = Nothing
        If _working.Count >= 2 Then
            Dim line As New TatLine(_working) With {
                .Name = $"Line{_lines.Count + 1}",
                .Color = _activeColor,
                .Width = _activeWidth,
                .DashStyle = _activeDash,
                .Visible = True,
                .ManageMode = LineManage_Mode
            }
            _lines.Add(line)
            newLine = line
            _HasChanges = True
        End If

        'Using dlg As New frmLineProperties(newLine)
        '    If dlg.ShowDialog(Me) = DialogResult.OK Then
        '        Dim finalLine = CType(dlg.Result, TatLine) ' ถ้าคลาสชื่อ clsTATLine
        '        ' เพิ่มลง collection วาดเส้น และสั่งวาดใหม่
        '        _lines.Add(finalLine)
        '        Me.Invalidate() ' หรือ PictureBox/Control ที่คุณวาดอยู่
        '    End If
        'End Using
        _working.Clear()
        Invalidate()
    End Sub

    ' ----- Mouse -----
    Private Sub Form1_MouseDown(sender As Object, e As MouseEventArgs)
        If _drawMode Then
            Dim p As New PointF(e.X, e.Y)

            If e.Button = MouseButtons.Left Then
                ' เพิ่มจุดลงเส้นที่กำลังวาด (รองรับ Shift ตรึงแกน)
                If _working.Count > 0 AndAlso IsShiftDown() Then
                    p = SnapToAxis(_working.Last(), p)
                End If
                _working.Add(p)
                Invalidate()

            ElseIf e.Button = MouseButtons.Right Then
                ' คลิกขวาลบบนจุดใน working (เหมือนเดิม)
                Dim idx = HitTestPointInWorking(p)
                If idx >= 0 Then
                    _working.RemoveAt(idx)
                    Invalidate()
                End If
            End If
        ElseIf _ModifyMode Then
            Dim p As New PointF(e.X, e.Y)
            If e.Button = MouseButtons.Left Then
                'Return
                ' เพิ่มจุดลงเส้นที่กำลังวาด (รองรับ Shift ตรึงแกน)
                If _selectedLine.Points.Count > 0 AndAlso IsShiftDown() Then
                    p = SnapToAxis(_selectedLine.Points.Last(), p)
                End If
                _selectedLine.Points.Add(p)
                Set_HasChanges(True)
                Invalidate()
            ElseIf e.Button = MouseButtons.Right Then
                Dim idx = HitTestPointInModify(p)
                If idx >= 0 Then
                    _selectedLine.Points.RemoveAt(idx)
                    Set_HasChanges(True)
                    Invalidate()
                End If
            End If

        Else
            If Not LineManage_Mode Then Return ' ถ้าโหมดรัน ห้ามแก้ไขเส้น

            ' โหมดไม่แก้ไข: ลองเลือกเส้นที่คลิกโดน (เพื่อจัดการภายหลัง)
            Dim hit = _lines.LastOrDefault(Function(ln) ln.HitTest(New PointF(e.X, e.Y), 6.0F))
            '======= For Add Condition
            If hit IsNot Nothing AndAlso _propForm IsNot Nothing Then
                _propForm.Add_Condition(hit.Name)
                Return
            End If

            _selectedLineId = If(hit IsNot Nothing, hit.Id, Guid.Empty)
            Invalidate()

            If hit IsNot Nothing Then
                btnLineProperty.Visible = True
                btnModify.Visible = True
                _selectedLine = hit
            Else
                btnLineProperty.Visible = False
                btnModify.Visible = False
                _selectedLine = Nothing
            End If


        End If
    End Sub

    Private Sub Form1_MouseMove(sender As Object, e As MouseEventArgs)
        _mousePos = New PointF(e.X, e.Y)
        If _drawMode OrElse _ModifyMode Then Invalidate()
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        '==== Key Delete
        If e.KeyCode = Keys.Delete Then
            If LineManage_Mode AndAlso _selectedLineId <> Guid.Empty Then
                DeleteLine(_selectedLineId)
                _selectedLineId = Guid.Empty
                btnLineProperty.Visible = False
                btnModify.Visible = False
                _selectedLine = Nothing
                Invalidate()
            End If
        End If

        '==== Key Enter
        If e.KeyCode = Keys.Enter Then
            'Draw Mode
            If _drawMode Then
                btnGenerate.PerformClick()
                Invalidate()
                'GenerateCurrentAsLine()
            End If
            'Modify Mode
            If _ModifyMode Then
                btnApply.PerformClick()
            End If
        End If

        '==== Key Esc
        If e.KeyCode = Keys.Escape Then
            'Draw Mode
            If _drawMode Then
                GenerateCurrentAsLine()
                btnDrawLine.PerformClick()
            End If
            'Modify Mode
            If _ModifyMode Then
                'btnCancel.PerformClick()
                If _ModifyMode AndAlso _selectedLine IsNot Nothing Then
                    _selectedLine.CancelAdjust()
                End If
                ExitAdjustMode()
            End If
        End If

        '===== Ctrl + Shift + P = Properties
        If e.Modifiers = (Keys.Control Or Keys.Shift) AndAlso e.KeyCode = Keys.P Then
            btnLineProperty.PerformClick()
        End If
        'If e.KeyCode = Keys.ControlKey AndAlso e.KeyCode = Keys.ShiftKey AndAlso e.KeyCode = Keys.P Then
        '    btnLineProperty.PerformClick()
        'End If

        '===== Ctrl + Shift + M = Modify
        If e.Modifiers = (Keys.Control Or Keys.Shift) AndAlso e.KeyCode = Keys.M Then
            btnModify.PerformClick()
        End If
        'If e.KeyCode = Keys.Control AndAlso e.KeyCode = Keys.Shift AndAlso e.KeyCode = Keys.M Then
        '    btnModify.PerformClick()
        'End If

    End Sub

    ' ----- Paint -----
    Private Sub Form1_Paint(sender As Object, e As PaintEventArgs)
        'If LineManage_Mode = False Then Exit Sub
        Dim g = e.Graphics
        g.SmoothingMode = SmoothingMode.AntiAlias

        ' 1) วาดเส้น (object) ทุกเส้น
        For Each ln In _lines
            ln.Draw(g)
        Next

        ' 2) เน้นเส้นที่เลือก (ถ้ามี) ด้วยการวาดกรอบ Bounds (ออปชัน)
        If _selectedLineId <> Guid.Empty Then
            Dim sel = _lines.FirstOrDefault(Function(x) x.Id = _selectedLineId)
            If sel IsNot Nothing AndAlso sel.Visible Then
                Using penSel As New Pen(Color.Red, 1.0F)
                    penSel.DashStyle = DashStyle.Solid
                    Dim rc = sel.GetBounds()
                    rc.Inflate(6, 6)
                    g.DrawRectangle(penSel, rc.X, rc.Y, rc.Width, rc.Height)
                End Using
            End If
        Else
            btnLineProperty.Visible = False
            btnModify.Visible = False
        End If

        ' 3) วาด working points + live preview
        Using br As New SolidBrush(Color.Red),
              pen As New Pen(Color.Black, 1.0F),
              f As New Font("Segoe UI", 9.0F, FontStyle.Bold)
            For i = 0 To _working.Count - 1
                Dim c = _working(i)
                Dim r As New RectangleF(c.X - POINT_RADIUS, c.Y - POINT_RADIUS, POINT_RADIUS * 2, POINT_RADIUS * 2)
                g.FillEllipse(br, r)
                g.DrawEllipse(pen, r)
                g.DrawString((i + 1).ToString(), f, Brushes.Black, c.X + POINT_RADIUS + 3, c.Y - POINT_RADIUS - 3)
            Next
        End Using

        If _drawMode AndAlso _working.Count >= 1 Then
            Using penDash As New Pen(Color.Red, 1.0F)
                penDash.DashStyle = DashStyle.Dash
                For i = 0 To _working.Count - 2
                    g.DrawLine(penDash, _working(i), _working(i + 1))
                Next
                Dim lastP = _working.Last()
                Dim liveTo = If(IsShiftDown(), SnapToAxis(lastP, _mousePos), _mousePos)
                g.DrawLine(penDash, lastP, liveTo)
            End Using
        End If

        If _ModifyMode AndAlso _selectedLine.Points.Count >= 1 Then
            Using penDash As New Pen(Color.Red, 1.0F)
                penDash.DashStyle = DashStyle.Dash
                For i = 0 To _selectedLine.Points.Count - 2
                    g.DrawLine(penDash, _selectedLine.Points(i), _selectedLine.Points(i + 1))
                Next
                Dim lastP = _selectedLine.Points.Last()
                Dim liveTo = If(IsShiftDown(), SnapToAxis(lastP, _mousePos), _mousePos)
                g.DrawLine(penDash, lastP, liveTo)
            End Using
        End If
    End Sub

    ' ----- Helpers -----
    Private Function HitTestPointInWorking(clickPoint As PointF) As Integer
        For i = 0 To _working.Count - 1
            If Distance(clickPoint, _working(i)) <= HIT_RADIUS Then Return i
        Next
        Return -1
    End Function

    Private Function HitTestPointInModify(clickPoint As PointF) As Integer
        If _selectedLine Is Nothing Then Return -1
        For i = 0 To _selectedLine.Points.Count - 1
            If Distance(clickPoint, _selectedLine.Points(i)) <= HIT_RADIUS Then Return i
        Next
        Return -1
    End Function

    Private Shared Function Distance(a As PointF, b As PointF) As Single
        Dim dx = a.X - b.X
        Dim dy = a.Y - b.Y
        Return CSng(Math.Sqrt(dx * dx + dy * dy))
    End Function

    Private Shared Function IsShiftDown() As Boolean
        Return (Control.ModifierKeys And Keys.Shift) = Keys.Shift
    End Function

    Private Shared Function SnapToAxis(fromPt As PointF, toPt As PointF) As PointF
        Dim dx = Math.Abs(toPt.X - fromPt.X)
        Dim dy = Math.Abs(toPt.Y - fromPt.Y)
        If dx >= dy Then
            Return New PointF(toPt.X, fromPt.Y) ' แนวนอน
        Else
            Return New PointF(fromPt.X, toPt.Y) ' แนวตั้ง
        End If
    End Function

    ' ====== Public API: คุมวัตถุเส้นจากที่อื่น ======
    Public Function GetLineById(id As Guid) As TatLine
        Return _lines.FirstOrDefault(Function(l) l.Id = id)
    End Function

    Public Sub ShowLine(id As Guid, Optional isVisible As Boolean = True)
        Dim ln = GetLineById(id)
        If ln IsNot Nothing Then
            ln.Visible = isVisible
            Invalidate()
        End If
    End Sub

    Public Sub HideLine(id As Guid)
        ShowLine(id, False)
    End Sub

    Public Sub SetLineStyle(id As Guid, color As Color, Optional width As Single = 2.0F, Optional dash As DashStyle = DashStyle.Solid)
        Dim ln = GetLineById(id)
        If ln IsNot Nothing Then
            ln.Color = color
            ln.Width = width
            ln.DashStyle = dash
            Invalidate()
        End If
    End Sub

    Public Sub DeleteLine(id As Guid)
        Dim idx = _lines.FindIndex(Function(l) l.Id = id)
        If idx >= 0 Then
            _lines.RemoveAt(idx)
            If _selectedLineId = id Then _selectedLineId = Guid.Empty
            Invalidate()
        End If
    End Sub

    Private Sub Timer_Line_Tick(sender As Object, e As EventArgs) Handles Timer_Line.Tick
        Dim HasChanged As Boolean = False
        For Each ln In _lines
            ln.ManageMode = LineManage_Mode
            Dim blResult = LineVisibilityCondition(ln.Condition, Me)
            If ln.Visible <> blResult Then
                ln.Visible = blResult
                HasChanged = True
            End If
        Next
        If HasChanged Then Invalidate()
        'Invalidate()
    End Sub
    Private Sub Show_AllLines()
        For Each ln In _lines
            ln.Visible = True
            ln.ManageMode = LineManage_Mode
        Next
        Invalidate()
    End Sub

    Public _propForm As frmLineProperties
    Private Sub btnLineProperty_Click(sender As Object, e As EventArgs) Handles btnLineProperty.Click
        If _selectedLine Is Nothing Then Return

        ' ถ้ายังไม่มีหรือถูกปิดไปแล้ว -> สร้างใหม่และ Hook event
        If _propForm Is Nothing OrElse _propForm.IsDisposed Then
            _propForm = New frmLineProperties(_selectedLine)

            AddHandler _propForm.LineApplied,
                Sub(res As TatLine)
                    ' อัพเดตตัวที่เลือกอยู่แบบ real-time (ยังไม่ปิดหน้าต่าง)
                    ApplyLineChange(res, commit:=False)
                End Sub

            AddHandler _propForm.LineCommitted,
                Sub(res As TatLine)
                    ' ยืนยันผล (OK) แล้วสั่งวาดใหม่
                    ApplyLineChange(res, commit:=True)
                End Sub

            AddHandler _propForm.FormClosed,
                Sub()
                    ' ป้องกัน Memory leak
                    RemoveHandler _propForm.LineApplied, Nothing
                    RemoveHandler _propForm.LineCommitted, Nothing
                    _propForm = Nothing
                End Sub
        Else
            ' ถ้าเปิดอยู่แล้ว โฟกัสมันขึ้นมา
            _propForm.Focus()
        End If

        If frmProperty_Location <> Point.Empty Then
            _propForm.StartPosition = FormStartPosition.Manual
            _propForm.Location = frmProperty_Location
        Else
            _propForm.StartPosition = FormStartPosition.CenterScreen
        End If
        _propForm.Show(Me) ' Owner = ฟอร์มหลัก
        Me.BeginInvoke(Sub()
                           Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                       End Sub)
    End Sub
    Private Sub ApplyLineChange(res As TatLine, commit As Boolean)
        ' อัปเดต _selectedLine หรือแทนที่ในคอลเลกชัน _lines
        ' แล้วรีเฟรชการวาด
        ' ถ้าต้องหา index:
        Dim idx = _lines.IndexOf(_selectedLine)
        If idx >= 0 Then
            _lines(idx) = res
            _HasChanges = True
        End If

        ' ถ้ามีแคช/Geometry ต้อง Recompute ตรงนี้ด้วย
        Me.Invalidate()  ' หรือ control ที่ใช้วาด
        If commit Then
            _propForm.Close()
        End If
        Me.BeginInvoke(Sub()
                           Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                       End Sub)
    End Sub

    Private Sub btnModify_Click(sender As Object, e As EventArgs) Handles btnModify.Click
        If _selectedLine IsNot Nothing Then
            _ModifyMode = True
            _selectedLine.IsAdjusting = True
            _selectedLine.BeginAdjust()
            Me.Cursor = Cursors.Cross
            Me.BeginInvoke(Sub()
                               Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                           End Sub)
            Invalidate()
        End If
        Me.BeginInvoke(Sub()
                           Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                       End Sub)
    End Sub
    Private Sub Set_HasChanges(blChanged As Boolean)
        _HasChanges = blChanged
        If blChanged Then
            btnApply.Visible = True
            btnCancel.Visible = True
        Else
            btnApply.Visible = False
            btnCancel.Visible = False
        End If
    End Sub
    ' ===== ปุ่ม Apply : ยืนยันการปรับแต่ง =====
    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        If _ModifyMode AndAlso _selectedLine IsNot Nothing Then
            _selectedLine.ApplyAdjust()
        End If
        ExitAdjustMode()
        Me.BeginInvoke(Sub()
                           Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                       End Sub)
    End Sub

    ' ===== ปุ่ม Cancel : กลับสภาพเดิมก่อนปรับแต่ง =====
    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        If _ModifyMode AndAlso _selectedLine IsNot Nothing Then
            _selectedLine.CancelAdjust()
        End If
        ExitAdjustMode()
        Me.BeginInvoke(Sub()
                           Me.ActiveControl = Nothing    ' ฟอร์มรับโฟกัสเอง (ไม่มีคอนโทรลไหนถูกเลือก)
                       End Sub)
    End Sub
    Private Sub ExitAdjustMode()
        _ModifyMode = False
        _selectedLine = Nothing
        _selectedLineId = Guid.Empty
        Set_HasChanges(False)
        Me.Cursor = Cursors.Default
        Invalidate()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Save_lineProperty(Me.Name, _lines, CnBatching)
        MessageBox.Show("บันทึกข้อมูลเส้นเรียบร้อย", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information)
        _HasChanges = False
    End Sub
#End Region

#Region " >>> MANUAL EDITCODE <<< "

    ' ===================================== NOTE =====================================
    ' FunctionWithTimer() สำหรับเรียกฟังก์ชันที่ต้องการให้ทำงานทุก ๆ ครั้งที่ Timer_Run tick
    ' ถ้าเป็นไปได้ พยายามเขียนโค้ดให้อยู่ใน Region " >>> MANUAL EDITCODE <<< " 
    ' ================================================================================

    Public Sub FunctionWithTimer()
        'WRITE SOME FUNCTION HERE
        'Alarm_TempBKE_M1106()
        'Alarm_TempBKE_M1116()
        'Alarm_TempBKE_MCC09()
        'Alarm_TempBKE_M1126()
        'Alarm_TempBKE_M2301()
    End Sub

    Private Sub Show_Line()
        'WRITE SOME FUNCTION HERE
    End Sub

    '============== For Show / Hide Alarm Label =================
    Private Sub ApplyStatus(lbl As Label, defaultColor As Color, ParamArray statusOrder() As Boolean)
        Dim target As Color
        If statusOrder.Length > 0 AndAlso statusOrder(0) Then
            target = Color.Red
        ElseIf statusOrder.Length > 1 AndAlso statusOrder(1) Then
            target = Color.Orange
        ElseIf statusOrder.Length > 2 AndAlso statusOrder(2) Then
            target = Color.Blue
        Else
            target = defaultColor
        End If

        If lbl.BackColor <> target Then
            SetBackColorSafe(lbl, target)
            If lbl.ForeColor <> Color.White Then SetForeColorSafe(lbl, Color.White)
        End If
    End Sub

    Private Sub SetBackColorSafe(lbl As Label, c As Color)
        If lbl.InvokeRequired Then
            lbl.BeginInvoke(Sub() lbl.BackColor = c)
        Else
            lbl.BackColor = c
        End If
    End Sub

    Private Sub SetForeColorSafe(lbl As Label, c As Color)
        If lbl.InvokeRequired Then
            lbl.BeginInvoke(Sub() lbl.ForeColor = c)
        Else
            lbl.ForeColor = c
        End If
    End Sub

#End Region

End Class