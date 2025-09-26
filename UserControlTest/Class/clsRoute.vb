Public Class clsRoute
    Dim dt As DataTable
    Dim DEVICE_MQTT_Ctrl As TAT_MQTT_CTRL.ctrlTAT_
    Dim Line_thickness As Integer
    Dim Color_ON As Color
    Dim Color_OFF As Color
    Dim WithEvents menu As New ContextMenuStrip
    Dim str1 As String
    Dim str2 As String
    Dim str3 As String
    Dim str4 As String
    Dim From As Form
    Dim check_Type As String
    '   ====
    Dim S_top As Point
    Dim S_But As Point
    Dim S_Left As Point
    Dim S_Right As Point
    Dim S_Height As Single
    Dim S_Width As Single
    Dim D_top As Point
    Dim D_But As Point
    Dim D_Left As Point
    Dim D_Right As Point
    Dim D_Height As Single
    Dim D_Width As Single
    '   ====

    Dim count As Int16
    Dim Color_ON_ As Brush
    Dim Plus As Single
    Dim io As New clsIO
    Dim Station_PLC_ As String
    'Dim Form1 As Form
    '   ================ Show MENU Rout
    Public Sub ShowMenu_(ByRef sender As Object, ByRef point As Point, Route_status As String, Form1 As Form, ByRef Station_PLC As String)
        Station_PLC_ = Station_PLC
        If TypeOf sender Is TAT_MQTT_CTRL.ctrlTAT_ Then
            DEVICE_MQTT_Ctrl = sender
            If MDI_FRM.Add_source = "" Then
                str1 = "Add source"
                Dim item1 = menu.Items.Add(str1)
                AddHandler item1.Click, AddressOf Set_Route
                item1.Tag = str1
            Else
                Dim item1 = menu.Items.Add("Add Destination")
                AddHandler item1.Click, AddressOf Set_Route
                item1.Tag = "Add Destination"

            End If

            Dim item2 = menu.Items.Add(New ToolStripSeparator)
            'AddHandler item2.Click, AddressOf process_Route

            Dim item3 = menu.Items.Add("From Bin")
            AddHandler item3.Click, AddressOf Set_Route
            item3.Tag = "FROMBIN"

            Dim item4 = menu.Items.Add(New ToolStripSeparator)


            Dim item5 = menu.Items.Add("To Bin")
            AddHandler item5.Click, AddressOf Set_Route
            item5.Tag = "TOBIN"
        End If

        menu.Show(sender, point)
    End Sub

#Region "Config Route MQTT Object"
    Private Sub Set_Route(sender As Object, e As EventArgs)
        Dim item = CType(sender, ToolStripMenuItem)
        Dim selection = item.ToString
        Dim tmpMotorID As String = Get_MotorID(DEVICE_MQTT_Ctrl.M_Code, DEVICE_MQTT_Ctrl.PLC_Station_No)
        Select Case UCase(selection)
            Case "ADD SOURCE"
                'MsgBox(selection)
                If tmpMotorID <> "" Then
                    MDI_FRM.Add_source = Format(CInt(tmpMotorID), "0000") ' DEVICE_motor.Tag
                    MDI_FRM.Add_source_Code = Replace(DEVICE_MQTT_Ctrl.M_Code, "-", "")
                    MsgBox(MDI_FRM.Add_source)
                End If

            Case "ADD DESTINATION"
                'MsgBox(selection)
                If MDI_FRM.Add_source <> Format(CInt(tmpMotorID), "0000") And MDI_FRM.Add_source <> "" And tmpMotorID <> "" Then
                    Func_AddOutLet(MDI_FRM.Add_source, MDI_FRM.Add_source_Code, Format(CInt(tmpMotorID), "0000"), Replace(DEVICE_MQTT_Ctrl.M_Code, "-", ""), "T")
                End If
            Case "FROM BIN"
                'MsgBox(selection)
                io.Open_Application("Config_DestBin", " " + DEVICE_MQTT_Ctrl.PLC_Station_No + "  ROUTE " + DEVICE_MQTT_Ctrl.M_Code + " FROMBIN")
            Case "TO BIN"
                'MsgBox(selection)
                io.Open_Application("Config_DestBin", " " + DEVICE_MQTT_Ctrl.PLC_Station_No + "  ROUTE " + DEVICE_MQTT_Ctrl.M_Code + " TOBIN")
        End Select
    End Sub

    Private Sub Set_Bin(sender As Object, e As EventArgs)
        Dim item = CType(sender, ToolStripMenuItem)
        Dim selection = item.Tag
        Select Case selection
            Case Is = "FROMBIN"
                io.Open_Application("Config_DestBin", " " + DEVICE_MQTT_Ctrl.PLC_Station_No + "  ROUTE " + DEVICE_MQTT_Ctrl.M_Code + " FROMBIN")
            Case Is = "TOBIN"
                io.Open_Application("Config_DestBin", " " + DEVICE_MQTT_Ctrl.PLC_Station_No + "  ROUTE " + DEVICE_MQTT_Ctrl.M_Code + " TOBIN")
        End Select
    End Sub

#End Region

    '================== ADD Route Database
    Public Sub Func_AddOutLet(OutLetSource As String, OutLetSourceName As String, OutLetDest As String, OutLetDestName As String, Position As String)
        Dim conn As New clsDB(Auto_Route_Conf.Name, Auto_Route_Conf.User, Auto_Route_Conf.Password, Auto_Route_Conf.IPAddress, Auto_Route_Conf.Connection_Type)
        Dim Sql As String
        Dim OutLetId As Long
        Dim OutLetName As String
        '      Dim rs As New ADODB.Recordset
        OutLetSourceName = Replace(OutLetSourceName, "_", "")
        OutLetDestName = Replace(OutLetDestName, "_", "")

        Sql = "SELECT c_max_id FROM thaisia.maxid WHERE thaisia.maxid.c_msg_id = 'OUTLET_CONFIG' "
        dt = New DataTable
        dt = conn.ExecuteDataTable(Sql)
        If dt.Rows.Count = 0 Then
            MessageBox.Show("NOT FOUND 'MAX ID' OF OUTLET CONFIG ")
            conn.ExecuteNoneQuery("INSERT INTO thaisia.maxid (c_msg_id, c_max_id) VALUES ('OUTLET_CONFIG', 1)")
            OutLetId = 1
        Else
            OutLetId = Val(dt.Rows(0)("C_MAX_ID")) + 1
            conn.ExecuteNoneQuery("UPDATE thaisia.maxid SET c_max_id = '" & OutLetId & "'  WHERE thaisia.maxid.c_msg_id = 'OUTLET_CONFIG' ")
        End If
        OutLetName = OutLetSourceName & "_" & OutLetDestName
        If OutLetName <> "" Then

            Sql = "SELECT n_sub_outlet_id,sub_code,source_obj,dest_obj FROM thaisia.outlet_config " &
                " WHERE  source_obj = '" & OutLetSource & "' AND dest_obj='" & OutLetDest & "'" &
                " AND sub_code='" & OutLetName & "'"
            dt = New DataTable
            dt = conn.ExecuteDataTable(Sql)

            If dt.Rows.Count > 0 Then
                MsgBox("This route already use ", vbCritical, "Config Route")
                If MsgBox("This route already use. DELETE <YES/NO> ?" & vbCrLf & " OUTLET NAME " & OutLetName, vbCritical + vbYesNo, "Config Route") = vbYes Then
                    conn.ExecuteNoneQuery("DELETE FROM thaisia.outlet_config  WHERE  thaisia.outlet_config.sub_code ='" & OutLetName & "'")
                End If
            Else

                Sql = "INSERT INTO thaisia.outlet_config(n_sub_outlet_id,sub_code,source_obj,dest_obj,dest_position)" &
                      "VALUES(" & CStr(OutLetId) & ",'" & CStr(OutLetName) & "','" & CStr(OutLetSource) & "','" & CStr(OutLetDest) & "','" + Position + "')"
                If MsgBox("ROUTE CODE : " & OutLetName, vbInformation + vbOKOnly, "Confirm") = vbOK Then
                    conn.ExecuteNoneQuery(Sql)
                End If
            End If
        End If

        MDI_FRM.Add_source = ""
        MDI_FRM.Add_Destination = ""
        Exit Sub

    End Sub
End Class