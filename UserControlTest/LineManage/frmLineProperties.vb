Imports System.Drawing.Drawing2D

Public Class frmLineProperties
    Private ReadOnly _original As Object ' keep reference in case it's not exactly clsTATLine
    Private _working As Object ' editable clone/working ref

    ' ยิงผลกลับให้ฟอร์มหลัก
    Public Event LineApplied(result As TatLine)   ' กด Apply
    Public Event LineCommitted(result As TatLine) ' กด OK

    ' ==== NEW: operator tokens ====
    Private Const AND_OP As String = "&"
    Private Const OR_OP As String = "|"
    Private defaultCondition As String = OR_OP

    Public Sub New(line As Object)
        _original = line
        ' Create a shallow copy for editing (ถ้าต้องการ deep clone ให้แก้ตรงนี้)
        _working = line

        InitializeComponent()
        LoadFromLine(Me._working)
    End Sub

    Public ReadOnly Property Result As Object
        Get
            Return _working
        End Get
    End Property

    Private Sub PickColor(sender As Object, e As EventArgs) Handles btnColor.Click
        Using cd As New ColorDialog()
            If cd.ShowDialog(Me) = DialogResult.OK Then
                btnColor.BackColor = cd.Color
                btnColor.ForeColor = If(CalcLuma(cd.Color) < 128, Color.White, Color.Black)
            End If
        End Using
    End Sub

    Private Shared Function CalcLuma(c As Color) As Integer
        Return CInt(0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B)
    End Function

    ' =========================
    ' Mapping between UI <-> clsTATLine (ADJUST HERE to fit your model)
    ' =========================
    Private Sub LoadFromLine(line As Object)
        If line Is Nothing Then Return

        ' ตั้งค่า KeyPreview ไว้รับ Alt+Shortcut
        Me.KeyPreview = True

        ' ตั้งค่า Tag ปุ่ม (กันลืมตั้งใน Designer)
        If String.IsNullOrEmpty(CStr(btnAnd.Tag)) Then btnAnd.Tag = AND_OP
        If String.IsNullOrEmpty(CStr(btnOr.Tag)) Then btnOr.Tag = OR_OP

        ' Try common property names via late-binding to avoid compile errors if your class differs.
        txtName.Text = GetPropString(line, "Name", GetPropString(line, "LineName", ""))
        nudWidth.Value = CDec(GetPropSingle(line, "Width", GetPropSingle(line, "PenWidth", 2.0F)))
        Dim color As Color = GetPropColor(line, "Color", GetPropColor(line, "LineColor", Color.Lime))
        btnColor.BackColor = color

        ' ==== NEW: แปลง legacy (* / +) -> (& / |) ตอนโหลดขึ้นจอ
        Dim rawCond As String = GetPropString(line, "Condition", "")
        txtCondition.Text = MapLegacyToNew(rawCond)

        Dim styleName As String = GetPropEnumName(Of DashStyle)(line, "DashStyle", "Solid")
        Dim idx = cmbStyle.FindStringExact(styleName)
        If idx >= 0 Then
            cmbStyle.SelectedIndex = idx
        Else
            cmbStyle.SelectedIndex = cmbStyle.FindStringExact("Solid")
        End If

        ' ตั้งสีปุ่มตาม defaultCondition
        ApplyDefaultButtonsColor()
    End Sub

    Private Sub SaveIntoLine(line As Object)
        SetProp(line, "Name", txtName.Text)
        SetProp(line, "LineName", txtName.Text)
        SetProp(line, "Width", CSng(nudWidth.Value))
        SetProp(line, "PenWidth", CSng(nudWidth.Value))
        SetProp(line, "Color", btnColor.BackColor)
        SetProp(line, "LineColor", btnColor.BackColor)

        ' ==== NEW: บันทึกกลับเป็นรูปแบบใหม่ (& / |)
        SetProp(line, "Condition", txtCondition.Text)

        Dim ds As DashStyle = CType([Enum].Parse(GetType(DashStyle), CStr(cmbStyle.SelectedItem)), DashStyle)
        SetProp(line, "DashStyle", ds)
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        SaveIntoLine(_working)
        RaiseEvent LineCommitted(_working) ' ถ้าต้องการ strict ให้ CType(_working, TatLine)
        Me.DialogResult = DialogResult.OK
    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        SaveIntoLine(_working)
        RaiseEvent LineApplied(_working)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

#Region "Late-binding helpers"
    Private Shared Function GetPropString(obj As Object, name As String, Optional [default] As String = "") As String
        Dim pi = obj.GetType().GetProperty(name)
        If pi Is Nothing Then Return [default]
        Dim v = pi.GetValue(obj, Nothing)
        Return If(v Is Nothing, [default], v.ToString())
    End Function

    Private Shared Function GetPropSingle(obj As Object, name As String, Optional [default] As Single = 0.0F) As Single
        Dim pi = obj.GetType().GetProperty(name)
        If pi Is Nothing Then Return [default]
        Dim v = pi.GetValue(obj, Nothing)
        If v Is Nothing Then Return [default]
        Return Convert.ToSingle(v)
    End Function

    Private Shared Function GetPropInt(obj As Object, name As String, Optional [default] As Integer = 0) As Integer
        Dim pi = obj.GetType().GetProperty(name)
        If pi Is Nothing Then Return [default]
        Dim v = pi.GetValue(obj, Nothing)
        If v Is Nothing Then Return [default]
        Return Convert.ToInt32(v)
    End Function

    Private Shared Function GetPropBool(obj As Object, name As String, Optional [default] As Boolean = False) As Boolean
        Dim pi = obj.GetType().GetProperty(name)
        If pi Is Nothing Then Return [default]
        Dim v = pi.GetValue(obj, Nothing)
        If v Is Nothing Then Return [default]
        Return Convert.ToBoolean(v)
    End Function

    Private Shared Function GetPropColor(obj As Object, name As String, Optional [default] As Color = Nothing) As Color
        Dim pi = obj.GetType().GetProperty(name)
        If pi Is Nothing Then Return [default]
        Dim v = pi.GetValue(obj, Nothing)
        If v Is Nothing Then Return [default]
        If TypeOf v Is Color Then Return CType(v, Color)
        ' try ARGB int
        Return ColorTranslator.FromWin32(Convert.ToInt32(v))
    End Function

    Private Shared Function GetPropEnumName(Of T As Structure)(obj As Object, name As String, Optional [default] As String = "") As String
        Dim pi = obj.GetType().GetProperty(name)
        If pi Is Nothing Then Return [default]
        Dim v = pi.GetValue(obj, Nothing)
        If v Is Nothing Then Return [default]
        Return v.ToString()
    End Function

    Private Shared Sub SetProp(obj As Object, name As String, value As Object)
        Dim pi = obj.GetType().GetProperty(name)
        If pi Is Nothing Then Return
        Try
            If value IsNot Nothing AndAlso Not pi.PropertyType.IsAssignableFrom(value.GetType()) Then
                ' attempt simple conversions
                If pi.PropertyType Is GetType(Single) Then
                    value = Convert.ToSingle(value)
                ElseIf pi.PropertyType Is GetType(Integer) Then
                    value = Convert.ToInt32(value)
                ElseIf pi.PropertyType Is GetType(Boolean) Then
                    value = Convert.ToBoolean(value)
                ElseIf pi.PropertyType Is GetType(Color) AndAlso TypeOf value IsNot Color Then
                    value = ColorTranslator.FromHtml(value.ToString())
                ElseIf pi.PropertyType Is GetType(String) Then
                    value = value.ToString()
                ElseIf pi.PropertyType.IsEnum Then
                    value = [Enum].Parse(pi.PropertyType, value.ToString())
                End If
            End If
            pi.SetValue(obj, value, Nothing)
        Catch
            ' ignore mapping errors to stay flexible across different clsTATLine definitions
        End Try
    End Sub
#End Region

    ' ==== NEW: helpers สำหรับ operator แบบใหม่ และ mapping legacy ====
    Private Shared Function IsNewOp(ch As Char) As Boolean
        Return ch = CChar(AND_OP) OrElse ch = CChar(OR_OP)
    End Function

    Private Shared Function MapLegacyToNew(expr As String) As String
        If String.IsNullOrEmpty(expr) Then Return expr
        Return expr.Replace("*", AND_OP).Replace("+", OR_OP)
    End Function

    Public Sub Add_Condition(strObjName As String)
        Me.Activate()
        Dim cur As String = txtCondition.Text.Trim()

        If cur = "" Then
            txtCondition.Text = strObjName
        Else
            Dim lastCh As Char = cur(cur.Length - 1)
            If Not IsNewOp(lastCh) AndAlso lastCh <> "("c Then
                txtCondition.Text &= defaultCondition & strObjName
            Else
                txtCondition.Text &= strObjName
            End If
        End If
    End Sub

    Private Sub frmLineProperties_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ตั้งสีปุ่มเริ่มต้นตาม defaultCondition
        ApplyDefaultButtonsColor()
    End Sub

    Private Sub ApplyDefaultButtonsColor()
        btnOr.BackColor = If(defaultCondition = OR_OP, Color.PaleGreen, DefaultBackColor)
        btnAnd.BackColor = If(defaultCondition = AND_OP, Color.PaleGreen, DefaultBackColor)
    End Sub

    Private Sub btnOr_And_Click(sender As Object, e As EventArgs) Handles btnOr.Click, btnAnd.Click
        Dim tmpBtn As Button = CType(sender, Button)
        defaultCondition = CStr(tmpBtn.Tag) ' "|" หรือ "&"

        ' ปรับสี
        ApplyDefaultButtonsColor()

        ' auto-append operator ถ้าตัวท้ายยังไม่ใช่ operator
        Dim cur As String = txtCondition.Text.Trim()
        If cur <> "" Then
            Dim lastCh As Char = cur(cur.Length - 1)
            If Not IsNewOp(lastCh) Then
                txtCondition.Text &= defaultCondition
            End If
        End If
    End Sub

    Private Sub frmLineProperties_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' ใช้ e.Alt แทนการเช็ค e.KeyCode = Keys.Alt
        If e.Alt AndAlso e.KeyCode = Keys.A Then
            txtCondition.Text &= AND_OP
            e.Handled = True
        ElseIf e.Alt AndAlso e.KeyCode = Keys.O Then
            txtCondition.Text &= OR_OP
            e.Handled = True
        End If
    End Sub

    Private Sub frmLineProperties_Move(sender As Object, e As EventArgs) Handles Me.Move
        frmProperty_Location = Me.Location
    End Sub
End Class
