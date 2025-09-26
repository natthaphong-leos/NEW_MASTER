Imports CheckHardlock
Public Class Cls_Hasp
    Dim dev_PrinterPort As Long
    Dim ai_LptNum As Int32
    Public Const GET_HASP_CODE = 2
    Public Const READ_WORD = 3
    Public Const WRITE_WORD = 4
    Public Const GET_HASP_STATUS = 5
    Public Const GET_ID_NUM = 6
    Public Const READ_MEMO_BLOCK = 50
    Public Const WRITE_MEMO_BLOCK = 51
    Public Const NET_SET_SERVER_BY_NAME = 96

    ' A list of TimeHASP Services.
    Public Const TIMEHASP_SET_TIME = 70
    Public Const TIMEHASP_GET_TIME = 71
    Public Const TIMEHASP_SET_DATE = 72
    Public Const TIMEHASP_GET_DATE = 73
    Public Const TIMEHASP_WRITE_MEMORY = 74
    Public Const TIMEHASP_READ_MEMORY = 75
    Public Const TIMEHASP_WRITE_BLOCK = 76
    Public Const TIMEHASP_READ_BLOCK = 77
    Public Const TIMEHASP_GET_ID_NUM = 78

    ' A list of NetHASP Services.
    Public Const NET_LAST_STATUS = 40
    Public Const NET_GET_HASP_CODE = 41
    Public Const NET_LOGIN = 42
    Public Const NET_LOGOUT = 43
    Public Const NET_READ_WORD = 44
    Public Const NET_WRITE_WORD = 45
    Public Const NET_GET_ID_NUMBER = 46
    Public Const NET_SET_IDLE_TIME = 48
    Public Const NET_READ_MEMO_BLOCK = 52
    Public Const NET_WRITE_MEMO_BLOCK = 53

    Public Const OK = 0
    Public Const NET_READ_ERROR = 131
    Public Const NET_WRITE_ERROR = 132

    ' A list of LptNum codes for the different types of keys
    Public Const LPT_IBM_ALL_HASP25 = 0
    Public Const LPT_IBM_ALL_HASP36 = 50
    Public Const LPT_NEC_ALL_HASP36 = 60

    Public Const MEMO_BUFFER_SIZE = 20

    'TimeHASP maximum block size
    Public Const TIME_BUFFER_SIZE = 16

    ' Show parameters
    Public Const MODAL = 1
    Public Const max_int = 32767
    Public Const NO_HASP = "HASP plug not found !"


    Dim ID As Integer
    Dim PrgNum As Object
    Dim IdleTime As Short
    Dim RetStatus, dummy As Integer

    Public ProgramNum As Short
    Public Service As Short
    Public SeedCode As Short
    Public LptNum As Short
    Public Passw1 As Short
    Public Passw2 As Short
    Public p3, p1, p2, p4 As Integer

    Const IS_HASP = 1

    Dim gb_check_hasp As Boolean
    Dim gl_HaspCount As Int32
    Dim gl_NoHaspCount As Int32
    Const gc_HaspCountNo = 72  '3*4*6
    Const gc_NoHaspCountNo = 0

    Declare Sub hasp Lib "haspvb32.dll" (ByVal Service As Integer, ByVal seed As Integer, ByVal lpt As Integer, ByVal pass1 As Integer, ByVal pass2 As Integer, ByRef retcode1 As Integer, ByRef retcode2 As Integer, ByRef retcode3 As Integer, ByRef retcode4 As Integer)

    Public Declare Sub WriteHaspBlock Lib "haspvb32.dll" (ByVal Service&, Buff As Long, ByVal Length&)
    Public Declare Sub ReadHaspBlock Lib "haspvb32.dll" (ByVal Service&, Buff As Long, ByVal Length&)




    Public Function Check(ByVal Status As Integer) As Integer
        gb_check_hasp = True
        gl_HaspCount = gc_HaspCountNo
        gl_NoHaspCount = gc_NoHaspCountNo
        Check = Check_Hasp_New(dev_PrinterPort, Status)
    End Function



    Dim Number_Close As Integer
    Dim Team_time() As String
    Dim H As Integer ' 2 '//เก็บชั่วโมง
    Dim m As Integer '= 59 ' // เก็บนาที
    Dim s As Integer '= 60 ' // เก็บวินาที
    Dim ms As Boolean
    Function Check_Hasp_New(ai_LptNum As Int32, ByVal Status As Integer) As Integer
        Service = IS_HASP
        'Clear_Memo_Cnts
        Set_Args_New(ai_LptNum)
        Dim CheckHasp As New CheckHardlock.CheckHardlock.clsHardlock


        '  Dim isii As Integer
        Try

            Call hasp(Service, SeedCode, PrgNum, Passw1, Passw2, p1, p2, p3, p4)

            If p1 = 0 Then
                If CheckHasp.Hardlock() = 0 Then
                    p1 = 1
                Else
                    'MsgBox("Pls Check Hasp Again")
                    MsgBox("Hardlock Not Found, Please check and make sure it is installed properly on the printer port. ", vbApplicationModal + vbCritical)
                    End
                End If
            End If

            Check_Hasp_New = p1

        Catch ex As Exception
            Check_Hasp_New = p1
            Dim Projext_name As String
            Projext_name = Application.ExecutablePath
            Projext_name = Projext_name.Replace(Application.StartupPath & "\", "")
            Projext_name = Projext_name.Replace(".EXE", "")
            MsgBox(Projext_name & " " & "FN: Check_Hasp_New" & ex.Message)
        End Try


    End Function



    Function Set_Args_New(ai_LptNum As Integer)
        Passw1 = "12451"
        Passw2 = "32297"
        LptNum = ai_LptNum

    End Function

End Class
