
Imports ACTSUPPORTLib
Imports AxActUtlTypeLib

Public Class cls_MXComponent_RQ
    '   ==============================================================================================================
    '   ======================================      ตัวแปร        ======================================================
    '   ==============================================================================================================
    ' Option Explicit
    Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (dest As Int32, Source As Int32, ByVal numBytes As Long)
    Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Long, ByVal hWndInsertAfter As Long, ByVal X As Long, ByVal Y As Long, ByVal cx As Long, ByVal cy As Long, ByVal wFlags As Long) As Long
    '   ========================= HEK
    Const conHwndTopmost = -1
    Const conSwpNoActivate = &H10
    Const conSwpShowWindow = &H40
    '   ========================= Get Error
    Public LangMsg As String
    Public MsgErrorMx As String
    '============================= AxActUtlType Object
    Public MxCom1 As New AxActUtlType
    Public MxCom2 As New AxActUtlType
    Public MxCom3 As New AxActUtlType
    Public MxCom4 As New AxActUtlType
    Public MxCom(6) As AxActUtlType
    '   ========================= ActSupport Object  Get Error PLc
    Public MxComSup1 As New ActSupport
    '   ========================= Boolean Check PLC Station Connect
    Public PLC_Con(0 To 9) As Boolean
    '   ========================= String Error Magbox
    Dim ErrorOpenPort As String = "Can't connect open port PLC " & vbCrLf & " Do you want to retry open again <Yes/No> ? " & vbCrLf & vbCrLf
    Dim ErrorRead As String = "Read data from PLC error." & vbCrLf & "Do you want to 'Retry to Read' <Yes/No> ?" & vbCrLf & vbCrLf
    Dim ErrorWrite As String = "Writh data to PLC error." & vbCrLf & "Do you want to 'Retry to Read' <Yes/No> ?" & vbCrLf & vbCrLf

    Public Const actOen = "OPEN"
    Public Const actRead = "READ"
    Public Const actWrite = "WRITE"

    Dim TextError As String
    '   ========================= LOG
    Dim PLCstation As Integer

    Public Sub New() '====================== ADD BY APICHAT
        For i = 1 To UBound(MxCom) - 1
            MxCom(i) = New AxActUtlType
        Next
        MxCom(1) = Frm_ComponentMX.AxActUtlType1
        MxCom(2) = Frm_ComponentMX.AxActUtlType2
        MxCom(3) = Frm_ComponentMX.AxActUtlType3
        MxCom(4) = Frm_ComponentMX.AxActUtlType4
        MxCom(5) = Frm_ComponentMX.AxActUtlType5
    End Sub

    '   =================================== Message Erro PLC    =====================================
    '   ======================================================================================
    Public Function mxGetErrorMsg(IObject As Object, ErrCode As Long, asAction As String) As String    'Get Error message
        Dim ret As Long
        Dim ErrMsg As String = ""
        Dim retErrMsg As String
        Dim returnStr As String = ""
        If ret = 0 Then
            ret = IObject.GetErrorMessage(ErrCode, ErrMsg)
            retErrMsg = "Code : " & ErrCode & vbCrLf & ErrMsg & vbCrLf & vbCrLf
        Else
            retErrMsg = "Invalid Error Code" & ErrCode
        End If
        Select Case UCase(asAction)
            Case Is = "OPEN"
                returnStr = retErrMsg & ErrorOpenPort
            Case Is = "READ"
                returnStr = retErrMsg & ErrorRead
            Case Is = "WRITE"
                returnStr = retErrMsg & ErrorWrite
        End Select
        Return returnStr
    End Function

    ''' <summary>
    ''' mxSetLogicalNumber   เป็น Function  ที่เอาไว้ทำการ  Add 
    ''' </summary>
    ''' <param name="IObject"></param>
    ''' <param name="LogicalNumber"></param>
    ''' <remarks></remarks>
    Public Sub mxSetLogicalNumber(IObject As AxActUtlType, LogicalNumber As Integer) 'Set station no on object
        IObject.ActLogicalStationNumber = LogicalNumber
        If Err.Number <> 0 Then MsgBox(Err.Description)
        PLCstation = LogicalNumber
    End Sub

    Public Function ReMX(IObject As AxActUtlType) As Integer
        IObject.Close()
        mxSetLogicalNumber(IObject, 2)
        ReMX = mxOpenPort(IObject)
    End Function

    ''' <summary>
    '''  mxOpenPort เป็น Function  ที่เอาไว้ทำการ Connect PLC ตาม Station PLC ที่ได้กำหนด ใน  Function [mxSetLogicalNumber]
    ''' </summary>
    ''' <param name="IObject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function mxOpenPort(IObject As AxActUtlType) As Integer    'Open communication
        Dim ret As Long
        Dim count As Integer
RetryOpen:
        ret = 1
        count = 0
        Do While count <= 2 And ret <> 0
            ret = IObject.Open()
            If ret <> 0 Then
                count = count + 1
            End If
        Loop
        If count > 2 Then
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actOen)
            If MsgBox(MsgErrorMx, vbYesNo + vbCritical, "Open Port") = vbYes Then
                GoTo RetryOpen
            Else
                mxOpenPort = 0
            End If
        Else
            mxOpenPort = 1
        End If
    End Function

    '   ==========================================================================-==
    '   =========================           Get  Device M > int16    =============================-==
    '   ==========================================================================-==
    ''' <summary>
    '''   mxDevSetM เป็น Function  ส่งข้อมูลเข้า PLC โดยระบุ M Address เช่น M4000  และค่าที่จะ force  0 : Off , 1 : On   (Arrlay Short(16 Bit))
    ''' </summary>
    ''' <param name="IObject"></param>
    ''' <param name="IAddDevice_M"></param>
    ''' <param name="OptSet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function mxDevSetM(IObject As AxActUtlType, IAddDevice_M As String, OptSet As Int16, Optional StatusLog As Boolean = False) As Integer   'Set M on/off......1=on, 2=off
        Dim ret As Long
        Dim count As Long
RetryRead:
        ret = 1
        count = 0
        Do While count <= 2 And ret <> 0
            ret = IObject.SetDevice2(IAddDevice_M, OptSet)
            If ret <> 0 Then
                count = count + 1
            End If
        Loop
        If count >= 2 Then
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actWrite)
            TextError = "[" + CStr(PLCstation) + "]" & MsgErrorMx & vbNewLine & "[mxDevSetM(" + IAddDevice_M + ")] "
            'Log.writeLogEvent(Application.StartupPath, "Error mxDevSetM :[" + TextError + "]  " + vbNewLine + "")
            If MsgBox(TextError, vbYesNo + vbCritical, "Write Error") = vbYes Then
                GoTo RetryRead
            End If
        End If
        mxDevSetM = ret
        '   ================================ LOG
        If StatusLog = False Then Exit Function
        'If ret = 0 Then Log.writeLogEvent(Application.StartupPath, "[" + CStr(PLCstation) + "]" & "mxDevSetM PLC Address:" + CStr(IAddDevice_M) + "/Data= " + CStr(OptSet) + " " + vbNewLine + "")
    End Function
    '   ==========================================================================-==
    '   =========================           Get  Device M > int16    =============================-==
    '   ==========================================================================-==
    ''' <summary>
    ''' mxDevGetM เป็น Function  อ่านข้อมูลจาก PLC โดยระบุ M Address เช่น M4000  จะ   Return อยู๋ในรูปแบบของ  (Arrlay Short(16 Bit))
    ''' </summary>
    ''' <param name="IObject"></param>
    ''' <param name="IAddDevice_M"></param>
    ''' <param name="IpsData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function mxDevGetM(IObject As AxActUtlType, IAddDevice_M As String, ByRef IpsData As Short, Optional StatusLog As Boolean = False) As Long  'Get M on/off......1=on, 2=off
        Dim ret As Long
        Dim count As Long
RetryRead:
        ret = 1
        count = 0
        Do While count <= 2 And ret <> 0
            ret = IObject.GetDevice2(IAddDevice_M, IpsData)
            If ret <> 0 Then
                count = count + 1
            End If
        Loop
        If count >= 2 Then
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actRead)
            TextError = "[" + CStr(PLCstation) + "]" & MsgErrorMx & vbNewLine & "[mxDevGetM(" + IAddDevice_M + ")] "
            'Log.writeLogEvent(Application.StartupPath, "Error mxDevGetM :[" + CStr(TextError) + "]  " + vbNewLine + "")
            If MsgBox(TextError, vbYesNo + vbCritical, "Write Error") = vbYes Then
                GoTo RetryRead
            End If
        End If
        mxDevGetM = ret
        '   ================================ LOG
        If StatusLog = False Then Exit Function
        'If ret = 0 Then Log.writeLogEvent(Application.StartupPath, "[" + CStr(PLCstation) + "]" & "mxDevGetM PLC Address:" + CStr(IAddDevice_M) + "/Data= " + CStr(IpsData) + " " + vbNewLine + "")
    End Function

    '   =====================================================================
    '   =========================         READ PLC   =============================-==
    '   =====================================================================
    ''' <summary>
    ''' mxReadBlock เป็น Function  อ่าข้อมูลจาก PLC โดยระบุ Address เช่น R500, D3444,ZR544  ระบุจำนวน ที่ต้องการอ่าน โดย จะเก็บอยู๋ในรูปแบบของ  (Arrlay integer(32 Bit))
    ''' </summary>
    ''' <param name="IObject"> </param>
    ''' <param name="IAddDevice"></param>
    ''' <param name="Isize"></param>
    ''' <param name="RetData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function mxReadBlock(IObject As AxActUtlType, IAddDevice As String, Isize As Integer, ByVal RetData() As Integer, Optional StatusLog As Boolean = False) As Integer   'Read Batch
        Dim ret As Integer
        Dim count As Long
RetryRead:
        ret = 1
        count = 0
        Do While count <= 2 And ret <> 0
            ret = IObject.ReadDeviceBlock(IAddDevice, Isize, RetData(0))
            If ret <> 0 Then
                count = count + 1
            End If
        Loop
        If count > 2 Then
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actRead)
            TextError = "[" + CStr(PLCstation) + "]" & MsgErrorMx & vbNewLine & "[mxReadBlock(" + IAddDevice + ")] "
            'Log.writeLogEvent(Application.StartupPath, "Error mxReadBlock :[" + TextError + "]  " + vbNewLine + "")
            If MsgBox(TextError, vbYesNo + vbCritical, "Read Error") = vbYes Then
                GoTo RetryRead
            End If
        End If
        mxReadBlock = ret
        '   ================================ LOG
        If StatusLog = False Then Exit Function
        Dim Data As String = ""
        For i As Integer = 0 To Isize - 1
            Data = Data & CStr(RetData(i)) & "   "
        Next
        'If ret = 0 Then Log.writeLogEvent(Application.StartupPath, "[" + CStr(PLCstation) + "]" & "mxReadBlock PLC Address:" + IAddDevice + "/Isize = " + CStr(Isize) + " /Data= " + Data + " " + vbNewLine + "")
    End Function

    ''' <summary>
    '''   ''' mxReadBlock2 เป็น Function  อ่าข้อมูลจาก PLC โดยระบุ Address เช่น R500, D3444,ZR544  ระบุจำนวน ที่ต้องการอ่าน โดย จะเก็บอยู๋ในรูปแบบของ  (Arrlay Short(16 Bit))
    ''' </summary>
    ''' <param name="IObject"></param>
    ''' <param name="IAddDevice"></param>
    ''' <param name="Isize"></param>
    ''' <param name="RetData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function mxReadBlock2(ByVal IObject As AxActUtlType, ByVal IAddDevice As String, ByVal Isize As Integer, ByVal RetData() As Short, Optional StatusLog As Boolean = False) As Long   'Read Batch
        Dim ret As Integer
        Dim count As Integer
RetryRead:
        ret = 1
        count = 0
        Do While count <= 2 And ret <> 0
            ret = IObject.ReadDeviceBlock2(IAddDevice, Isize, RetData(0))
            If ret <> 0 Then
                count = count + 1
            End If
        Loop
        If count > 2 Then
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actWrite)
            TextError = "[" + CStr(PLCstation) + "]" & MsgErrorMx & vbNewLine & "[mxReadBlock2(" + IAddDevice + ")] "
            'Log.writeLogEvent(Application.StartupPath, "Error mxReadBlock2 :[" + TextError + "]  " + vbNewLine + "")
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actRead)
            If MsgBox(TextError, vbYesNo + vbCritical, "Read Error") = vbYes Then
                GoTo RetryRead
            End If
        End If
        mxReadBlock2 = ret
        '   ================================ LOG
        If StatusLog = False Then Exit Function
        Dim Data As String = ""
        For i As Integer = 0 To Isize - 1
            Data = Data & CStr(RetData(i)) & "   "
        Next
        'If ret = 0 Then Log.writeLogEvent(Application.StartupPath, "[" + CStr(PLCstation) + "]" & "mxReadBlock2 PLC Address:" + IAddDevice + "/Isize = " + CStr(Isize) + " /Data= " + Data + " " + vbNewLine + "")
    End Function
    Public Function mxReadRandom(IObject As AxActUtlType, IAddDevice As String, Isize As Integer, ByVal RetData() As Short, Optional StatusLog As Boolean = False) As Integer  'Read random
        Dim ret As Integer
        Dim count As Long
RetryRead:
        ret = 1
        count = 0
        Do While count <= 2 And ret <> 0
            ret = IObject.ReadDeviceRandom2(IAddDevice, Isize, RetData(0))
            If ret <> 0 Then
                count = count + 1
            End If
        Loop
        If count > 2 Then
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actWrite)
            TextError = "[" + CStr(PLCstation) + "]" & MsgErrorMx & vbNewLine & "[mxReadRandom(" + IAddDevice + ")] "
            'Log.writeLogEvent(Application.StartupPath, "Error mxReadRandom :[" + TextError + "]  " + vbNewLine + "")
            If MsgBox(TextError, vbYesNo + vbCritical, "Read Error") = vbYes Then
                GoTo RetryRead
            End If
        End If
        mxReadRandom = ret
        '   ================================ LOG
        If StatusLog = False Then Exit Function
        Dim Data As String = ""
        For i As Integer = 0 To Isize - 1
            Data = Data & CStr(RetData(i)) & "   "
        Next
        'If ret = 0 Then Log.writeLogEvent(Application.StartupPath, "[" + CStr(PLCstation) + "]" & "mxReadRandom PLC Address:" + IAddDevice + "/Isize = " + CStr(Isize) + " /Data= " + Data + " " + vbNewLine + "")
    End Function

    '   ==========================================================================-==
    '   =================================         WRITE PLC           =========================-==
    '   ==========================================================================-==
    ''' <summary>
    '''   mxWriteBlock เป็น Function  ส่งข้อมูลเข้า  PLC โดยระบุ Address เช่น R500, D3444,ZR544  ระบุจำนวน ที่ต้องการอ่าน โดย RetDataที่จะส่งเข้าไป Try ต้องเป็น(Arrlay Short(16 Bit))
    ''' </summary>
    ''' <param name="IObject"></param>
    ''' <param name="IAddDevice"></param>
    ''' <param name="Isize"></param>
    ''' <param name="RetData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function mxWriteBlock(IObject As AxActUtlType, IAddDevice As String, Isize As Integer, ByVal RetData() As Short, Optional StatusLog As Boolean = False) As Integer     'Write random
        Dim ret As Integer
        Dim count As Integer
RetryRead:
        ret = 1
        count = 0
        Do While count <= 2 And ret <> 0
            ret = IObject.WriteDeviceBlock2(IAddDevice, Isize, RetData(0))
            If ret <> 0 Then
                count = count + 1
            End If
        Loop
        If count > 2 Then
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actWrite)
            TextError = "[" + CStr(PLCstation) + "]" & MsgErrorMx & vbNewLine & "[mxWriteBlock(" + IAddDevice + ")] "
            'Log.writeLogEvent(Application.StartupPath, "Error mxWriteBlock :[" + TextError + "]  " + vbNewLine + "")

            If MsgBox(TextError, vbYesNo + vbCritical, "Write Error") = vbYes Then
                GoTo RetryRead
            End If
        End If
        mxWriteBlock = ret

        '   ================================ LOG
        If StatusLog = False Then Exit Function
        Dim Data As String = ""
        For i As Integer = 0 To Isize - 1
            Data = Data & CStr(RetData(i)) & "   "
        Next

        'If ret = 0 Then Log.writeLogEvent(Application.StartupPath, "[" + CStr(PLCstation) + "]" & "mxWriteBlock PLC Address:" + IAddDevice + "/Isize = " + CStr(Isize) + " /Data= " + Data + " " + vbNewLine + "")
    End Function



    '    Public Function mxWriteBlock2(IObject As AxActUtlType, IAddDevice As String, Isize As Integer, ByVal RetData() As Long) As Long     'Write random
    '        Dim ret As Integer
    '        Dim count As Integer
    'RetryRead:
    '        ret = 1
    '        count = 0
    '        Do While count <= 2 And ret <> 0
    '            ret = IObject.WriteDeviceBlock2(IAddDevice, Isize, RetData(0))
    '            If ret <> 0 Then
    '                count = count + 1
    '            End If
    '        Loop
    '        If count > 2 Then
    '            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actWrite)
    '            TextError = "[" + CStr(PLCstation) + "]" & MsgErrorMx & vbNewLine & "[mxWriteBlock(" + IAddDevice + ")] "
    '            'Log.writeLogEvent(Application.StartupPath, "Error mxWriteBlock :[" + TextError + "]  " + vbNewLine + "")

    '            If MsgBox(TextError, vbYesNo + vbCritical, "Write Error") = vbYes Then
    '                GoTo RetryRead
    '            End If
    '        End If
    '        mxWriteBlock2 = ret

    '        '   ================================ LOG
    '        'If StatusLog = False Then Exit Function
    '        'Dim Data As String = ""
    '        'For i As Integer = 0 To Isize - 1
    '        '    Data = Data & CStr(RetData(i)) & "   "
    '        'Next

    '        'If ret = 0 Then Log.writeLogEvent(Application.StartupPath, "[" + CStr(PLCstation) + "]" & "mxWriteBlock PLC Address:" + IAddDevice + "/Isize = " + CStr(Isize) + " /Data= " + Data + " " + vbNewLine + "")
    '    End Function



    ''' <summary>
    '''   mxWriteBlock เป็น Function  ส่งข้อมูลเข้า  PLC โดยระบุ Address เช่น R500, D3444,ZR544  ระบุจำนวน ที่ต้องการอ่าน โดย RetDataที่จะส่งเข้าไป Try ต้องเป็น(Arrlay Short(32 Bit))
    ''' </summary>
    ''' <param name="IObject"></param>
    ''' <param name="IAddDevice"></param>
    ''' <param name="Isize"></param>
    ''' <param name="DataLong"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function mxWriteBlockLong(IObject As AxActUtlType, IAddDevice As String, Isize As Integer, ByRef DataLong() As Integer, Optional StatusLog As Boolean = False) As Long      'Write random
        Dim ret As Integer
        Dim count As Integer
RetryRead:
        ret = 1
        count = 0
        Do While count <= 2 And ret <> 0
            ret = IObject.WriteDeviceBlock(IAddDevice, Isize, DataLong(0))
            If ret <> 0 Then
                count = count + 1
            End If
        Loop
        If count > 2 Then
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actWrite)

            TextError = "[" + CStr(PLCstation) + "]" & MsgErrorMx & "[mxWriteBlockLong(" + IAddDevice + ")] "
            'Log.writeLogEvent(Application.StartupPath, "Error mxWriteBlockLong :[" + TextError + "]")
            If MsgBox(TextError, vbYesNo + vbCritical, "Write Error") = vbYes Then
                GoTo RetryRead
            End If
        End If
        mxWriteBlockLong = ret

        '   ================================ LOG
        If StatusLog = False Then Exit Function
        Dim Data As String = ""
        For i As Integer = 0 To DataLong.LongCount - 1
            Data = Data & CStr(DataLong(i)) & "   "
        Next
        'Log.writeLogEvent(Application.StartupPath, "[" + CStr(PLCstation) + "]" & "mxWriteBlockLong PLC Address:" + IAddDevice + "/Isize = " + CStr(Isize) + " /Data= " + Data + " " + vbNewLine + "")

    End Function



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="IObject"></param>
    ''' <param name="IAddDevice"></param>
    ''' <param name="Isize"></param>
    ''' <param name="RetData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function mxWriteRandom(IObject As AxActUtlType, IAddDevice As String, Isize As Integer, RetData() As Short, StatusLog As Boolean) As Long     'Write random
        Dim ret As Integer
        Dim count As Integer
RetryRead:
        ret = 1
        count = 0
        Do While count <= 2 And ret <> 0
            ret = IObject.WriteDeviceRandom2(IAddDevice, Isize, RetData(0))
            If ret <> 0 Then
                count = count + 1
            End If
        Loop
        If count > 2 Then
            MsgErrorMx = mxGetErrorMsg(MxComSup1, ret, actWrite)
            TextError = "[" + CStr(PLCstation) + "]" & MsgErrorMx & vbNewLine & "[mxWriteRandom(" + IAddDevice + ")] "
            'Log.writeLogEvent(Application.StartupPath, "Error mxWriteRandom :[" + TextError + "]  " + vbNewLine + "")
            If MsgBox(TextError, "Write Error") = vbYes Then
                GoTo RetryRead
            End If
        End If
        mxWriteRandom = ret
        '   ================================ LOG
        If StatusLog = False Then Exit Function
        Dim Data As String = ""
        For i As Integer = 0 To RetData.LongCount - 1
            Data = Data & CStr(RetData(i)) & "   "
        Next
        'Log.writeLogEvent(Application.StartupPath, "[" + CStr(PLCstation) + "]" & "mxWriteBlockLong PLC Address:" + IAddDevice + "/Isize = " + CStr(Isize) + " /Data= " + Data + " " + vbNewLine + "")
    End Function

    '   ================================================================================================
    '   ======================================          Function  Other              =======================================
    '   ================================================================================================
    Sub ConvertLongToInt(DataLong() As Int32, DataSize As Long, RetDataInt() As Int16)
        'Todec (Right(Dtohex8(cwEmptyWt.Value * 1000), 4))
        Dim ValHex As String
        Dim iz, idx, idx1 As Integer
        Dim sBuf(99) As Int32
        Dim Send_Data(99) As Long
        Dim Zindex As Integer
        Dim send As String = ""

        idx = 0
        For idx1 = 0 To DataSize - 1
            Send_Data(idx) = Todec(Right(Dtohex8(DataLong(idx1)), 4))
            Send_Data(idx + 1) = Todec(Left(Dtohex8(DataLong(idx1)), 4))
            idx = idx + 2
        Next

        For Zindex = 0 To idx - 1
            'DatLen_Dat_Size - 1
            If Val(Send_Data(Zindex)) > 32767 Then
                ValHex = Hex(Val(Send_Data(Zindex)))
                Do Until Len(ValHex) >= 4
                    ValHex = "0" & ValHex
                Loop
                ValHex = "&H" & ValHex
                iz = Val(ValHex)
                sBuf(Zindex) = iz
            Else
                sBuf(Zindex) = Val(Send_Data(Zindex))
            End If
        Next Zindex
        Dim sBuf_(0) As Int32
        System.Buffer.BlockCopy(RetDataInt, 0 * 2, sBuf_, 0, 4)
        sBuf_(0) = sBuf_(0)
        '  CopyMemory(RetDataInt(0), sBuf(0), DataSize * 4)
    End Sub

    Function Dtohex8(EE As Long) As String
        Dim cc As String = ""
        Dim dd As String
        Dim i As Integer
        If Len(Trim(EE)) = 0 Then
            EE = 0
        End If
        dd = Hex(EE)
        For i = 1 To 8 - Len(dd)
            cc = cc & "0"
        Next i
        Dtohex8 = cc & dd
        If Len(Dtohex8) < 4 Then
            ' "PLC Control"
            Dtohex8 = "00000000"
        End If
endline5:
    End Function

    Function Todec(KA As String) As Long
        Dim f As String
        Dim ff As String = ""
        Dim KA2 As String
        If KA = "" Then GoTo last4
        KA2 = Mid$(KA, 5, 4) + Mid$(KA, 1, 4)
        f = "&H" + KA2
        ff = Str(f)
        Todec = Val(ff)
last4:

        Todec = Val(ff)

    End Function

    Function Func_ConvertCode4Chr(buf() As Int16, begin As Integer) As String
        Dim backdata As String = ""
        Dim DatHex As String
        Dim LoopNo, Index As Integer

        Index = begin
        For LoopNo = 1 To 2
            DatHex = Hex(buf(Index))
            Do Until Len(DatHex) >= 2
                DatHex = "0" & DatHex
            Loop
            Index = Index + 1
            backdata = backdata & DatHex
        Next LoopNo
        If backdata = "0" Then
            Func_ConvertCode4Chr = ""
        Else
            Func_ConvertCode4Chr = Trim(Tochr20(Mid$(backdata, 3, 2)) + Tochr20(Mid$(backdata, 1, 2)) + Tochr20(Mid$(backdata, 7, 2)) + Tochr20(Mid$(backdata, 5, 2)))
        End If
    End Function

    Function CTohex(Deci As String) As String
        Dim aa As String
        Dim bb As String = ""
        If Deci = "" Then
            CTohex = "20"
            GoTo ENDLI
        End If
        aa = Asc(Deci)
        CTohex = Hex(aa)
ENDLI:
    End Function

    Function Tochr20(CHA As String) As String
        Dim H As String = ""
        Dim l As String = ""
        Dim RtTochr20 As String = ""
        If CHA = "00" Then
            Tochr20 = " "
            GoTo last5
        End If
        If CHA = "" Then
            GoTo last5
        End If

        H = "&H" + CHA

        Tochr20 = Chr(Str(H))

last5:
        Tochr20 = Chr(Str(H))
    End Function


    Sub GetFolder(asFolderName As String)
        Dim dd As String
        dd = Dir(asFolderName, vbDirectory + vbReadOnly)
        If dd = "" Then
            MkDir(asFolderName)
        End If
    End Sub



End Class
