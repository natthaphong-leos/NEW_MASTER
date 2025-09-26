Imports System.Threading
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Data
Imports System.Net.Sockets
Imports System.Net

Public Class Class_Tcp
    Public IP_Address As String
    Dim client As TcpClient
    Sub New(str_ip_address As String, Port As Integer)
        IP_Address = str_ip_address
        client = New TcpClient(IP_Address, Port)

    End Sub

    ''' <summary>
    ''' Return 0:ส่งคำรองขอข้อมูล , >0:อ่านค่ามาได้
    ''' </summary>
    ''' <param name="Data">สำหรับรองรับค่าที่ได้</param>
    ''' <returns></returns>
    Public Function Read_TCP(ByRef Data() As Int16) As Integer

        Try
            Dim ns As NetworkStream = client.GetStream()


            If client.Client.Available > 0 Then

                Read_TCP = client.Client.Available
                Dim toReceive(client.Client.Available) As Byte

                ns.Read(toReceive, 0, toReceive.Length)
                Dim txt As String = Encoding.ASCII.GetString(toReceive)
                ReDim Data(toReceive.Count \ 2)
                System.Buffer.BlockCopy(toReceive, 0, Data, 0, toReceive.Length)


                System.Buffer.BlockCopy(Data, 1 * 2, Data, 0, (Data.Length - 1) * 2)


            Else
                Read_TCP = 0
                Dim B(0) As Byte
                B(0) = 82

                ns.Write(B, 0, 1)

            End If

        Catch ex As Exception

        End Try






    End Function

End Class
