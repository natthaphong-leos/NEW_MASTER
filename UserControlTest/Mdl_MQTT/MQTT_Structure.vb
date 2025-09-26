Public Structure Device_List
    '======================================= CONFIG
    Dim M_Code As String
    Dim Ad_Service As String
    Dim Ad_Output As String
    Dim Ad_Auto As String
    Dim Ad_Run As String
    Dim Ad_Run2 As String
    Dim Ad_Err1 As String
    Dim Ad_Err2 As String
    Dim Ad_Bin_High As String
    Dim Ad_Bin_Low As String
    Dim Ad_Coverlock As String
    Dim Tdly_Open As String
    Dim Tdly_Close As String
    Dim Tdly_Step As String
    Dim Ad_Parameter_PID As String
    Dim Ad_Parameter_Current As String
    Dim Ad_Ton_Per_Hour As String
    Dim cmdLine_String As String
    Dim Tdly_On As String
    Dim Tdly_Off As String
    Dim PLC_Station_No As String
    Dim ControlMode As String
    Dim ControlType As String
    Dim menuStart As Boolean
    Dim Index As String

    '=============================== ANALOG
    Dim Ad_Auto_PID As String
    Dim Ad_Analog_Hi As String
    Dim Ad_Analog_Lo As String
    Dim Ad_Analog_MV As String
    Dim Ad_Analog_PV As String
    Dim Ad_Analog_RPM As String
    Dim Ad_Analog_SV As String


    '=============================== LQ/HA
    Dim STR_No As String
    Dim STR_Name As String
    Dim Parameter_Type As String

    '=============================== BIN PARAMETER
    Dim Bin_Number As String
    Dim Bin_Step As String

    '=============================== MANUAL OPERATE
    Dim OP_Bin_index As String
    Dim OP_Scale_No As String

    '=============================== MANUAL LIQUID
    Dim OP_LQ_Name As String
    Dim OP_LQ_TN As String
    Dim OP_LQ_TS As String


    '======================================= STATUS
    Dim status_service As Boolean
    Dim status_output As Boolean
    Dim status_auto As Boolean
    Dim status_run As Boolean
    Dim status_err1 As Boolean
    Dim status_err2 As Boolean
    Dim status_coverlock As Boolean
    Dim status_interlock As Boolean
    Dim status_pid As Boolean

    '==================================== BIN
    Dim Device_Index_Hi As String
    Dim Device_Index_Lo As String
End Structure
