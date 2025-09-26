Module mdlShowLine
    Private dtLine As DataTable
    '============================== CONFIG LINE
    Public Line_EditMode As Boolean = False
    Public tempObject_Condition As TAT_MQTT_CTRL.ctrlTAT_ = Nothing
    Public tempContinueLine As TAT_UTILITY_CTRL.ctrlLine_ = Nothing
    Public addConditionMode As Boolean = False
    Public addContinuousMode As Boolean = False
    Public frmConfig_Location As Point = New Point(0, 0)

    'Public Function EvaluateVisibilityCondition(tmpLine As TAT_UTILITY_CTRL.ctrlLine_) As Boolean
    '    Dim conditions = tmpLine.VisibilityCondition.Split("|"c)
    '    For Each condition In conditions
    '        Dim controls = condition.Split("*"c)
    '        If controls.All(Function(c) DirectCast(tmpLine.Parent.Controls(c), TAT_MQTT_CTRL.ctrlTAT_).status_run) Then
    '            Return True
    '        End If
    '    Next
    '    Return False
    'End Function
#Region "Function Showline New"
    Private Function Tokenize(expr As String) As List(Of String)
        Dim tokens As New List(Of String)
        Dim sb As New System.Text.StringBuilder()
        For Each ch As Char In expr.Replace(" ", "")
            Select Case ch
                Case "("c, ")"c, "*"c, "|"c
                    If sb.Length > 0 Then
                        tokens.Add(sb.ToString())
                        sb.Clear()
                    End If
                    tokens.Add(ch)
                Case Else
                    sb.Append(ch)
            End Select
        Next
        If sb.Length > 0 Then tokens.Add(sb.ToString())
        Return tokens
    End Function

    Private Function Precedence(op As String) As Integer
        Select Case op
            Case "*"
                Return 2 ' AND
            Case "|"
                Return 1 ' OR
            Case Else
                Return 0
        End Select
    End Function

    Private Function ToRPN(tokens As List(Of String)) As List(Of String)
        Dim output As New List(Of String)
        Dim ops As New Stack(Of String)

        For Each tok In tokens
            If tok = "*" OrElse tok = "|" Then
                While ops.Count > 0 AndAlso ops.Peek() <> "(" AndAlso Precedence(ops.Peek()) >= Precedence(tok)
                    output.Add(ops.Pop())
                End While
                ops.Push(tok)
            ElseIf tok = "(" Then
                ops.Push(tok)
            ElseIf tok = ")" Then
                While ops.Count > 0 AndAlso ops.Peek() <> "("
                    output.Add(ops.Pop())
                End While
                If ops.Count > 0 AndAlso ops.Peek() = "(" Then ops.Pop() ' pop "("
            Else
                ' identifier (control name)
                output.Add(tok)
            End If
        Next

        While ops.Count > 0
            output.Add(ops.Pop())
        End While
        Return output
    End Function

    Private Function GetControlState(parent As Control, key As String) As Boolean
        ' หา control ตามชื่อ (ค้นลึกทั้งลูกหลาน)
        Dim arr = parent.Controls.Find(key, True)
        If arr Is Nothing OrElse arr.Length = 0 Then Return False
        'Dim c = TryCast(arr(0), TAT_MQTT_CTRL.ctrlTAT_)
        'If c Is Nothing Then Return False
        'Return c.status_run

        Dim ctrl As Control = arr(0)
        If TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
            Return DirectCast(ctrl, TAT_MQTT_CTRL.ctrlTAT_).status_run
        Else
            Return ctrl.Visible
        End If

    End Function

    Private Function EvalRPN(rpn As List(Of String), parent As Control) As Boolean
        Dim st As New Stack(Of Boolean)
        ' cache ผลลัพธ์ของแต่ละ control ป้องกันการค้นหาซ้ำ
        Dim cache As New Dictionary(Of String, Boolean)(StringComparer.OrdinalIgnoreCase)

        For Each tok In rpn
            Select Case tok
                Case "*"
                    Dim b = st.Pop() : Dim a = st.Pop()
                    st.Push(a AndAlso b)
                Case "|"
                    Dim b = st.Pop() : Dim a = st.Pop()
                    st.Push(a OrElse b)
                Case Else
                    Dim val As Boolean
                    If Not cache.TryGetValue(tok, val) Then
                        val = GetControlState(parent, tok)
                        cache(tok) = val
                    End If
                    st.Push(val)
            End Select
        Next
        Return If(st.Count > 0, st.Pop(), False)
    End Function

    ' เรียกใช้จากฟังก์ชันเดิมของโอม
    Public Function EvaluateVisibilityCondition(tmpLine As TAT_UTILITY_CTRL.ctrlLine_) As Boolean
        Dim expr As String = tmpLine.VisibilityCondition
        If String.IsNullOrWhiteSpace(expr) Then Return True ' ไม่มีเงื่อนไข = แสดง

        Dim tokens = Tokenize(expr)
        Dim rpn = ToRPN(tokens)
        Return EvalRPN(rpn, tmpLine.Parent)
    End Function
#End Region

    Public Sub ShowLine_in_collection(ByRef controlCollection As Control.ControlCollection)
        For Each control As Control In controlCollection
            If TypeOf control Is GroupBox Or TypeOf control Is Panel Or TypeOf control Is TableLayoutPanel Then
                ShowLine_in_collection(control.Controls)
            ElseIf TypeOf control Is TAT_UTILITY_CTRL.ctrlLine_ Then
                Dim lineCtrl As TAT_UTILITY_CTRL.ctrlLine_ = CType(control, TAT_UTILITY_CTRL.ctrlLine_)
                If lineCtrl.VisibilityCondition <> "" Then
                    lineCtrl.Visible = EvaluateVisibilityCondition(lineCtrl)
                End If
            End If
        Next
    End Sub

    Public Function GetLine_in_Collection(ByRef controlCollection As Control.ControlCollection, Optional OptVisible As String = "") As DataTable
        dtLine = New DataTable
        dtLine.Columns.Add("line_name")
        dtLine.Columns.Add("condition")
        dtLine.PrimaryKey = New DataColumn() {dtLine.Columns("line_name")}

        For Each control As Control In controlCollection
            If TypeOf control Is TAT_UTILITY_CTRL.ctrlLine_ Then
                Dim lineCtrl As TAT_UTILITY_CTRL.ctrlLine_ = CType(control, TAT_UTILITY_CTRL.ctrlLine_)
                If OptVisible = "" Then
                    lineCtrl.Visible = False
                ElseIf OptVisible = "EDIT" Then
                    lineCtrl.Visible = True
                ElseIf OptVisible = "FINISH" Then
                    If lineCtrl.VisibilityCondition <> "" Then lineCtrl.Visible = EvaluateVisibilityCondition(lineCtrl)
                End If
                'lineCtrl.Visible = False
                Dim drLine As DataRow = dtLine.NewRow
                drLine("line_name") = lineCtrl.Name
                drLine("condition") = lineCtrl.VisibilityCondition
                dtLine.Rows.Add(drLine)
            End If
        Next
        Return dtLine
    End Function

    Public Function GetLine_Config(ByVal strAppName As String, ByVal frmName As String) As DataTable
        Dim strSQL As String = "SELECT * FROM dbo.LineProperty WHERE c_app_Name = '" & strAppName & "' AND form_name = '" & frmName & "'"
        Return CnBatching.ExecuteDataTable(strSQL)
    End Function

    Public Sub ShowAll_Line(ByRef controlCollection As Control.ControlCollection)
        For Each control As Control In controlCollection
            If TypeOf control Is TAT_UTILITY_CTRL.ctrlLine_ Then
                Dim lineCtrl As TAT_UTILITY_CTRL.ctrlLine_ = CType(control, TAT_UTILITY_CTRL.ctrlLine_)
                lineCtrl.Visible = True
            End If
        Next
    End Sub
    Public Sub HideAll_Line(ByRef controlCollection As Control.ControlCollection)
        For Each control As Control In controlCollection
            If TypeOf control Is TAT_UTILITY_CTRL.ctrlLine_ Then
                Dim lineCtrl As TAT_UTILITY_CTRL.ctrlLine_ = CType(control, TAT_UTILITY_CTRL.ctrlLine_)
                lineCtrl.Visible = False
            End If
        Next
    End Sub

    Public Sub Clear_AddCondition()
        tempObject_Condition = Nothing
        addConditionMode = False
    End Sub

    Public Sub Complete_Edit()
        tempObject_Condition = Nothing
        addConditionMode = False
        tempContinueLine = Nothing
        addContinuousMode = False
    End Sub

End Module
