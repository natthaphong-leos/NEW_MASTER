Option Strict On
Option Infer On

Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.Data.Odbc

Public Class clsDB
    ' --- ฟิลด์หลัก (ใช้แบบ generic ให้จัดการได้ทั้ง SQL และ ODBC) ---
    Private ReadOnly dbConn As DbConnection
    Private ReadOnly dbComm As DbCommand
    Private ReadOnly dbAdap As DbDataAdapter
    Private dbTran As DbTransaction
    Private isTran As Boolean = False
    Private ReadOnly isSql As Boolean

    ' เก็บค่าเพื่อ debug/log ถ้าต้องการ
    Private ReadOnly DatabaseName_ As String
    Private ReadOnly UserNname_ As String
    Private ReadOnly Server_IP_ As String

    Public Sub New(DatabaseName As String,
                   UserNname As String,
                   Password As String,
                   Optional Server_IP As String = "",
                   Optional strType As String = "odbc")

        DatabaseName_ = DatabaseName
        UserNname_ = UserNname
        Server_IP_ = Server_IP

        Select Case UCase(strType)
            Case "SQL"
                isSql = True
                Dim cs = $"Data Source={Server_IP};Initial Catalog={DatabaseName};Persist Security Info=True;User ID={UserNname};Password={Password};"
                Dim c = New SqlConnection(cs)
                dbConn = c
                Dim cmd = New SqlCommand(String.Empty, c)
                dbComm = cmd
                dbAdap = New SqlDataAdapter(cmd)

            Case Else ' ODBC (ค่าเริ่มต้น)
                isSql = False
                Dim cs = $"DSN={DatabaseName};Uid={UserNname};Pwd={Password};"
                Dim c = New OdbcConnection(cs)
                dbConn = c
                Dim cmd = New OdbcCommand(String.Empty, c)
                dbComm = cmd
                dbAdap = New OdbcDataAdapter(cmd)
        End Select
    End Sub

    ' -------------------- Utilities --------------------
    Private Sub Open()
        If dbConn Is Nothing Then Throw New InvalidOperationException("dbConn is not initialized.")
        If dbConn.State <> ConnectionState.Open Then dbConn.Open()
    End Sub

    Private Sub Close()
        If dbConn IsNot Nothing AndAlso dbConn.State <> ConnectionState.Closed Then dbConn.Close()
    End Sub

    Private Sub keeplog(ByVal ex As Exception)
        ' TODO: ใส่ระบบ Log ของคุณที่นี่
        ' Debug.WriteLine(ex.ToString())
    End Sub

    ' -------------------- Public APIs เดิม (ปรับภายในให้ถูกต้อง) --------------------
    ' Test Connection
    Public Function TestConnection() As Boolean
        Try
            If Not isTran Then Open()
            Dim ok = (dbConn.State = ConnectionState.Open)
            If Not isTran Then Close()
            Return ok
        Catch ex As Exception
            keeplog(ex)
            If Not isTran Then
                Try : Close() : Catch : End Try
            End If
            Return False
        End Try
    End Function

    ' Select (Text)
    Public Function ExecuteDataTable(ByVal SQL As String) As DataTable
        If String.IsNullOrWhiteSpace(SQL) Then Return Nothing
        Try
            If Not isTran Then Open()

            dbComm.CommandText = SQL
            dbComm.CommandType = CommandType.Text

            Dim ds As New DataSet()
            dbAdap.SelectCommand = dbComm
            dbAdap.Fill(ds, "dt")

            If Not isTran Then Close()
            Return ds.Tables(0)
        Catch ex As Exception
            keeplog(ex)
            If Not isTran Then
                Try : Close() : Catch : End Try
            End If
            Return Nothing
        End Try
    End Function

    ' NonQuery (Text) – รักษาชื่อเดิม ExecuteNoneQuery
    Public Function ExecuteNoneQuery(ByVal SQL As String) As Integer
        Try
            If Not isTran Then Open()

            If Not String.IsNullOrWhiteSpace(SQL) Then
                dbComm.CommandText = SQL
                dbComm.CommandType = CommandType.Text
            End If

            Dim i = dbComm.ExecuteNonQuery()

            If Not isTran Then Close()
            Return i
        Catch ex As Exception
            keeplog(ex)
            If Not isTran Then
                Try : Close() : Catch : End Try
            End If
            Return 0
        End Try
    End Function

    ' เรียก Stored Procedure แบบไม่คืนค่า
    ' คงซิกเนเจอร์เดิม (Async Sub) แต่ทำงานแบบ synchronous เพื่อความเสถียร
    Public Async Sub ExecuteProcedure(sNameProcedure As String)
        Try
            If String.IsNullOrWhiteSpace(sNameProcedure) Then Exit Sub
            If Not isTran Then Open()

            dbComm.CommandText = sNameProcedure
            dbComm.CommandType = CommandType.StoredProcedure
            dbComm.ExecuteNonQuery()

            If Not isTran Then Close()
        Catch ex As Exception
            keeplog(ex)
            If Not isTran Then
                Try : Close() : Catch : End Try
            End If
        End Try
    End Sub

    ' Parameters
    Public Sub AddPara(ByVal sNamePara As String, ByVal sVal As Object)
        ' ใช้ Add กับชนิดพารามิเตอร์ตามโปรвайเดอร์ เพื่อเลี่ยง boxing ที่ไม่จำเป็น
        If isSql Then
            DirectCast(dbComm, SqlCommand).Parameters.AddWithValue(sNamePara, If(sVal, DBNull.Value))
        Else
            DirectCast(dbComm, OdbcCommand).Parameters.AddWithValue(sNamePara, If(sVal, DBNull.Value))
        End If
    End Sub

    Public Sub ClearPara()
        dbComm.Parameters.Clear()
    End Sub

    ' Select จาก Stored Procedure (คืน DataTable)
    Public Function SelectTableProcedure(ByVal sNameProcedure As String) As DataTable
        If String.IsNullOrWhiteSpace(sNameProcedure) Then Return Nothing
        Try
            If Not isTran Then Open()

            dbComm.CommandText = sNameProcedure
            dbComm.CommandType = CommandType.StoredProcedure

            Dim dt As New DataTable()
            Using rdr = dbComm.ExecuteReader()
                dt.Load(rdr, LoadOption.OverwriteChanges)
            End Using

            For Each dc As DataColumn In dt.Columns
                dc.ReadOnly = False
            Next

            If Not isTran Then Close()
            Return dt
        Catch ex As Exception
            keeplog(ex)
            If Not isTran Then
                Try : Close() : Catch : End Try
            End If
            Return Nothing
        End Try
    End Function

    ' -------------------- Transaction --------------------
    Public Sub BeginTransaction()
        Open()
        dbTran = dbConn.BeginTransaction()
        dbComm.Transaction = dbTran
        isTran = True
    End Sub

    Public Sub BackTransaction()
        Try
            If dbTran IsNot Nothing Then dbTran.Rollback()
        Finally
            Close()
            isTran = False
        End Try
    End Sub

    Public Sub CommitTransaction()
        Try
            If dbTran IsNot Nothing Then dbTran.Commit()
        Finally
            Close()
            isTran = False
        End Try
    End Sub
End Class
