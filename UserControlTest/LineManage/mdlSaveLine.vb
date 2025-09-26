Imports System.Reflection
Imports Newtonsoft.Json
Module mdlSaveLine
    Private strAppName As String = Assembly.GetExecutingAssembly().GetName().Name

    Public Sub Save_lineProperty(frmName As String, lines As IEnumerable(Of TatLine), cnDB As clsDB)
        Delete_old_line(frmName, cnDB)

        If lines Is Nothing Then Return
        If lines.Count > 0 Then
            Dim sql As String = CommandBuilder(frmName, lines)
            cnDB.ExecuteNoneQuery(sql)
        End If

    End Sub

    Private Sub Delete_old_line(frmName As String, cnDB As clsDB)
        Dim strSQL As String = "DELETE dbo.line_draw_property WHERE c_app_name ='" & strAppName & "' AND c_form_name = '" & frmName & "'"
        cnDB.ExecuteNoneQuery(strSQL)
    End Sub

    Private Function CommandBuilder(frmName As String, lines As IEnumerable(Of TatLine)) As String
        Dim sql As String = "INSERT INTO dbo.line_draw_property (id,c_app_name,c_form_name,c_json_line) VALUES "
        Dim values As New List(Of String)

        For Each ln In lines
            Dim tmpID As String = ln.Id.ToString()
            Dim json As String = JsonConvert.SerializeObject(ln)

            ' Escape single quote เพื่อกันพังใน SQL
            Dim safeJson As String = json.Replace("'", "''")
            Dim safeApp As String = strAppName.Replace("'", "''")
            Dim safeForm As String = frmName.Replace("'", "''")

            values.Add(String.Format("('{0}','{1}','{2}','{3}')",
                                 tmpID, safeApp, safeForm, safeJson))
        Next

        'sql.Append(String.Join(",", values))
        'sql.Append(";")
        'Return sql.ToString()

        sql += String.Join(",", values) & ";"
        Return sql.ToString
    End Function

    Public Function LoadTatLinesDB(frmName As String, cnDB As clsDB) As List(Of TatLine)
        Dim result As New List(Of TatLine)
        Dim strSQL As String = "SELECT * FROM dbo.line_draw_property WHERE c_app_name ='" & strAppName & "' AND c_form_name = '" & frmName & "'"
        Dim dt As DataTable = cnDB.ExecuteDataTable(strSQL)

        For Each row As DataRow In dt.Rows
            Dim json As String = row("c_json_line")
            Try
                Dim ln As TatLine = JsonConvert.DeserializeObject(Of TatLine)(json)
                If ln IsNot Nothing Then result.Add(ln)
            Catch
                ' แถวเสีย -> ข้าม
            End Try
        Next
        Return result
    End Function
End Module
