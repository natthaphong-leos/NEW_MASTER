Imports System.ComponentModel

Public Class frmConfig_Line
    Public appName As String
    Public formName As String
    Public tmpLine As TAT_UTILITY_CTRL.ctrlLine_ = Nothing
    Public frmParent As Form
    Public clsScada As cls_scada_Control
    Private originalCondition As String
    Private originalContinuous As String
    Private originalLineColor As Color = Color.Lime


    Private Sub frmConfig_Line_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If tmpLine IsNot Nothing Then
            originalLineColor = tmpLine.lineColor
            tmpLine.lineColor = Color.Red
            lblAppName.Text = appName
            lblFormName.Text = formName
            txtObj_Name.Text = tmpLine.Name
            txtLineCondition.Text = tmpLine.VisibilityCondition
            txtLineContinuous.Text = tmpLine.ContinuousLine
            Highlight_OldLine()
            originalCondition = txtLineCondition.Text
            originalContinuous = txtLineContinuous.Text
        End If
        If frmConfig_Location <> New Point(0, 0) Then
            Me.Location = frmConfig_Location
        End If
    End Sub
    Private Sub Highlight_OldLine()
        If txtLineContinuous.Text.Trim <> "" Then
            Dim strLines() As String = Split(txtLineContinuous.Text.Trim, ",")
            For Each line As String In strLines
                Dim ctrlLine As TAT_UTILITY_CTRL.ctrlLine_ = CType(frmParent.Controls(line), TAT_UTILITY_CTRL.ctrlLine_)
                If ctrlLine IsNot Nothing Then
                    ctrlLine.lineColor = Color.Red
                End If
            Next
        End If
    End Sub

    Private Sub btnAddObj_Click(sender As Object, e As EventArgs) Handles btnAddObj.Click
        addConditionMode = True
        btnAddObj.Visible = False
        btnCancel_Condition.Visible = True
        btnAdd_And.Visible = True
        btnAdd_Or.Visible = True
    End Sub
    Private Sub btnAdd_And_Click(sender As Object, e As EventArgs) Handles btnAdd_And.Click
        txtLineCondition.Text += "*"
    End Sub

    Private Sub btnAdd_Or_Click(sender As Object, e As EventArgs) Handles btnAdd_Or.Click
        txtLineCondition.Text += "|"
    End Sub
    Private Sub frmConfig_Line_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Alt AndAlso e.KeyCode = Keys.A Then
            txtLineCondition.Text += "*"
        ElseIf e.Alt AndAlso e.KeyCode = Keys.O Then
            txtLineCondition.Text += "|"
        End If
    End Sub

    Private Sub btnCancel_Condition_Click(sender As Object, e As EventArgs) Handles btnCancel_Condition.Click
        txtLineCondition.Text = originalCondition
        addConditionMode = False
        btnAdd_And.Visible = False
        btnAdd_Or.Visible = False
        btnAddObj.Visible = True
    End Sub

    Public Sub Add_Condition()
        If tempObject_Condition IsNot Nothing Then
            Me.Activate()
            If txtLineCondition.Text = "" Then
                txtLineCondition.Text += tempObject_Condition.Name
            Else
                If Check_DuplicateCondition() Then
                    lblAlarm.Text = "'" & tempObject_Condition.Name & "' IS ALREADY IN CONDITION!"
                    Dim lastCharacter As String = txtLineCondition.Text.Substring(txtLineCondition.Text.Length - 1)
                    If lastCharacter <> "*" AndAlso lastCharacter <> "|" AndAlso txtLineCondition.Text <> "" Then
                        txtLineCondition.Text += "|" & tempObject_Condition.Name
                    Else
                        txtLineCondition.Text += tempObject_Condition.Name
                    End If
                    Highlight_DuplicateCondition(tempObject_Condition.Name)
                    Exit Sub
                Else
                    Dim lastCharacter As String = txtLineCondition.Text.Substring(txtLineCondition.Text.Length - 1)
                    If lastCharacter <> "*" AndAlso lastCharacter <> "|" AndAlso txtLineCondition.Text <> "" Then
                        txtLineCondition.Text += "|" & tempObject_Condition.Name
                    Else
                        txtLineCondition.Text += tempObject_Condition.Name
                    End If
                End If

            End If
            lblAlarm.Text = "-"
        End If
    End Sub
    Private Function Check_DuplicateCondition() As Boolean
        If txtLineCondition.Text.Trim <> "" Then
            Dim tmpCondition As String = Replace(txtLineCondition.Text, "*", ",")
            tmpCondition = Replace(tmpCondition, "|", ",")
            Dim strCond() As String = Split(tmpCondition.Trim, ",")
            For Each Obj As String In strCond
                If Obj = tempObject_Condition.Name Then
                    Return True
                End If
            Next
            Return False
        End If
    End Function
    Private Sub Highlight_DuplicateCondition(strCondition As String)
        Dim startIndex As Integer = 0
        While startIndex < txtLineCondition.TextLength
            startIndex = txtLineCondition.Find(strCondition, startIndex, RichTextBoxFinds.None)
            If startIndex = -1 Then Exit While

            ' กำหนด FontStyle เป็น Bold สำหรับข้อความที่ตำแหน่งนั้น
            txtLineCondition.Select(startIndex, strCondition.Length)
            txtLineCondition.SelectionFont = New Font(txtLineCondition.Font, FontStyle.Bold)
            txtLineCondition.SelectionColor = Color.Red

            ' ขยับตำแหน่งเริ่มต้นไปยังตำแหน่งถัดไป
            startIndex += strCondition.Length
        End While
    End Sub
    Private Sub Unhighlight_Condition()
        For i As Integer = 0 To txtLineCondition.TextLength - 1
            txtLineCondition.Select(i, 1)
            If txtLineCondition.SelectionColor = Color.Red And txtLineCondition.SelectionFont.Bold Then
                txtLineCondition.SelectionColor = Color.Black
                txtLineCondition.SelectionFont = New Font(txtLineCondition.Font, FontStyle.Regular)
            End If
        Next
        txtLineCondition.DeselectAll()
    End Sub

    Private Sub txtLineCondition_TextChanged(sender As Object, e As EventArgs)
        lblCountCond.Text = txtLineCondition.Text.Length
    End Sub

    Private Sub txtLineContinuous_TextChanged(sender As Object, e As EventArgs) Handles txtLineContinuous.TextChanged
        lblCountContinue.Text = txtLineContinuous.Text.Length
    End Sub


    '================================= CONTINUE LINE
    Private Sub btnNextAdd_Click(sender As Object, e As EventArgs) Handles btnNextAdd.Click
        addContinuousMode = True
        btnCancel_Continue.Visible = True
    End Sub

    Private Sub btnCancel_Continue_Click(sender As Object, e As EventArgs) Handles btnCancel_Continue.Click
        Return_Color_ContinueLine()
        txtLineContinuous.Text = originalContinuous
        addContinuousMode = False
        btnNextAdd.Visible = True
    End Sub
    Public Sub Add_Continue()
        If tempContinueLine IsNot Nothing Then
            Me.Activate()
            Unhighlight_Condition()
            If txtLineContinuous.Text <> "" Then
                'If InStr(txtLineContinuous.Text, tempContinueLine.Name) > 0 Then
                '    Exit Sub
                'End If
                If Check_DuplicateLine() Then
                    lblAlarm.Text = "'" & tempContinueLine.Name & "' IS ALREADY MEMBER!"
                    tempContinueLine.lineColor = originalLineColor
                    Exit Sub
                End If
                txtLineContinuous.Text += "," & tempContinueLine.Name
            Else
                txtLineContinuous.Text += tempContinueLine.Name
            End If
            lblAlarm.Text = "-"
        End If
    End Sub
    Private Function Check_DuplicateLine() As Boolean
        If txtLineContinuous.Text.Trim <> "" Then
            Dim strLines() As String = Split(txtLineContinuous.Text.Trim, ",")
            For Each line As String In strLines
                If line = tempContinueLine.Name Then
                    Return True
                End If
            Next
            Return False
        End If
    End Function

    Private Sub btnSaveConfig_Click(sender As Object, e As EventArgs) Handles btnSaveConfig.Click
        If txtLineContinuous.Text.EndsWith(",") Then
            txtLineContinuous.Text = txtLineContinuous.Text.Substring(0, txtLineContinuous.Text.Length - 1)
        End If
        If txtLineCondition.Text.EndsWith("*") Or txtLineCondition.Text.EndsWith("|") Then
            txtLineCondition.Text = txtLineCondition.Text.Substring(0, txtLineCondition.Text.Length - 1)
        End If
        tmpLine.VisibilityCondition = txtLineCondition.Text.Trim
        tmpLine.ContinuousLine = txtLineContinuous.Text.Trim
        tmpLine.lineColor = originalLineColor
        Dim strSQL As String
        strSQL = "EXEC dbo.Config_Line "
        strSQL += "'" & txtObj_Name.Text.Trim & "',"
        strSQL += "'" & lblAppName.Text.Trim & "',"
        strSQL += "'" & lblFormName.Text.Trim & "',"
        strSQL += "'" & txtLineCondition.Text.Trim & "',"
        strSQL += "'" & txtLineContinuous.Text.Trim & "'"
        CnBatching.ExecuteNoneQuery(strSQL)
        If txtLineContinuous.Text.Length < originalContinuous.Length Then

        End If
        Return_Color_ContinueLine()
        clsScada.ReloadDB_LineConfig(frmParent)
        Complete_Edit()
        lblAlarm.Text = "-"
        Me.Close()
    End Sub

    Private Sub btnCancelConfig_Click(sender As Object, e As EventArgs) Handles btnCancelConfig.Click
        tmpLine.lineColor = originalLineColor
        Return_Color_ContinueLine()
        Complete_Edit()
        lblAlarm.Text = "-"
        Me.Close()
    End Sub
    Private Sub Return_Color_ContinueLine()
        If txtLineContinuous.Text.Trim <> "" Then
            Dim strLines() As String = Split(txtLineContinuous.Text.Trim, ",")
            For Each line As String In strLines
                Dim ctrlLine As TAT_UTILITY_CTRL.ctrlLine_ = CType(frmParent.Controls(line), TAT_UTILITY_CTRL.ctrlLine_)
                If ctrlLine IsNot Nothing Then
                    ctrlLine.lineColor = originalLineColor
                End If
            Next
        End If
        If originalContinuous.Trim <> "" Then
            Dim strLines() As String = Split(originalContinuous.Trim, ",")
            For Each line As String In strLines
                Dim ctrlLine As TAT_UTILITY_CTRL.ctrlLine_ = CType(frmParent.Controls(line), TAT_UTILITY_CTRL.ctrlLine_)
                If ctrlLine IsNot Nothing Then
                    ctrlLine.lineColor = originalLineColor
                End If
            Next
        End If
    End Sub

    Public Sub Alarm_Config(strName As String, strType As String, Optional strMain As String = "")
        Unhighlight_Condition()
        Select Case strType
            Case "MAIN"
                lblAlarm.Text = "'" & strName & "' IS MAIN LINE!"
            Case "MEMBER"
                lblAlarm.Text = "'" & strName & "' IS MEMBER LINE OF '" & strMain & "'"
        End Select
    End Sub
    Public Sub Closing_Form()
        tmpLine.lineColor = originalLineColor
        Return_Color_ContinueLine()
        Complete_Edit()
        lblAlarm.Text = "-"
        Me.Close()
    End Sub

    Private Sub frmConfig_Line_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        frmConfig_Location = Me.Location
    End Sub


End Class