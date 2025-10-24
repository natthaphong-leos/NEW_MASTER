Module mdlLineFunction
    Public Ctrl_Hiden As Boolean = False
    Public Lst_HidenCtrl As New List(Of Control)

    ' ใช้แบบใหม่เป็นค่าเริ่มต้น (OR = "|")
    Public defaultCondition As String = "|"

    Public Sub Hide_All_Control(ByRef frm As Form)
        For Each ctrl As Control In frm.Controls
            If ctrl.Visible = True Then
                If ctrl.Name <> "pnlLine" AndAlso ctrl.Name <> "btnLineManage" Then
                    ctrl.Visible = False
                    Lst_HidenCtrl.Add(ctrl)
                End If
            End If
        Next
    End Sub

    Public Sub Show_Hiden_Control()
        For Each ctrl As Control In Lst_HidenCtrl
            ctrl.Visible = True
        Next
        Lst_HidenCtrl.Clear()
    End Sub

    ' ====== NEW: แปลง operator legacy (*,+) -> ใหม่ (&,|) ======
    Private Function NormalizeOperators(expr As String) As String
        If String.IsNullOrWhiteSpace(expr) Then Return expr
        Return expr.Replace("*", "&").Replace("+", "|")
    End Function

    ' tokenize รองรับเฉพาะ (&,|) หลัง normalize แล้ว
    Private Function Tokenize(expr As String) As List(Of String)
        Dim tokens As New List(Of String)
        Dim sb As New System.Text.StringBuilder()

        For Each ch As Char In expr.Replace(" ", "")
            Select Case ch
                Case "("c, ")"c, "&"c, "|"c
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
            Case "&" : Return 2 ' AND
            Case "|" : Return 1 ' OR
            Case Else : Return 0
        End Select
    End Function

    Private Function ToRPN(tokens As List(Of String)) As List(Of String)
        Dim output As New List(Of String)
        Dim ops As New Stack(Of String)

        For Each tok In tokens
            If tok = "&" OrElse tok = "|" Then
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
                If ops.Count > 0 AndAlso ops.Peek() = "(" Then ops.Pop()
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

        ' กรณีเป็น LINE* ให้เช็คจากคอลเลกชัน line ในฟอร์ม
        Dim field = parent.GetType().GetField("_lines", Reflection.BindingFlags.Public Or Reflection.BindingFlags.Instance)
        Dim line As TatLine = Nothing

        If UCase(Left(key, 4)) = "LINE" Then
            If field IsNot Nothing Then
                Dim lines = TryCast(field.GetValue(parent), List(Of TatLine))
                If lines IsNot Nothing Then
                    line = lines.FirstOrDefault(Function(l) String.Equals(l.Name, key, StringComparison.OrdinalIgnoreCase))
                End If
            End If
            If line IsNot Nothing Then
                Return line.Visible
            End If
            Return False
        End If

        ' คุม NRE เมื่อหาไม่เจอ
        If arr Is Nothing OrElse arr.Length = 0 Then
            Return False
        End If

        Dim ctrl As Control = arr(0)
        If TypeOf ctrl Is CheckBox Then
            Return DirectCast(ctrl, CheckBox).Checked
        ElseIf TypeOf ctrl Is TAT_MQTT_CTRL.ctrlTAT_ Then
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
                Case "&"
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
    ' ===== End utilities =====

    ' เรียกใช้จากฟังก์ชันเดิมของโอม
    Public Function LineVisibilityCondition(tmpCondition As String, parent As Control) As Boolean
        If String.IsNullOrWhiteSpace(tmpCondition) Then Return False  ' ไม่มีเงื่อนไข = แสดง

        ' รองรับของเก่า (*,+) โดย normalize เป็น (&,|)
        Dim expr As String = NormalizeOperators(tmpCondition)

        Dim tokens = Tokenize(expr)
        Dim rpn = ToRPN(tokens)
        Return EvalRPN(rpn, parent)
    End Function
End Module