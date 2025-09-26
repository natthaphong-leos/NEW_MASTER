Module mdlLanguage
    Public structApp_Language As language_config
    Public Structure language_config
        Dim strLang_Main As String
        Dim strLang_Sub As String
        Dim blSubTitle As Boolean
    End Structure

    Public strLanguage_Main As String
    Public strLanguage_Sub As String
    Public blSubTitle As Boolean
    Public dtGlobal_MainLanguage As DataTable
    Public dtGlobal_SubLanguage As DataTable
    Public dtApp_MainLanguage As DataTable
    Public dtApp_SubLanguage As DataTable
    Public dtAlarmMsg_Main As DataTable
    Public dtAlarmMsg_Sub As DataTable

    '========== EVENT
    Public Event Langauge_Change(ByVal strLangCode As String, ByRef cnDB As clsDB)
    Public Sub RaiseEvent_ChangeLanguage(ByVal strLangCode As String, ByRef cnDB As clsDB)
        RaiseEvent Langauge_Change(strLangCode, cnDB)
    End Sub


    Public Function Get_LanguageConfig(strAppName As String, cnDB As clsDB) As language_config
        Dim config As New language_config
        Dim main_lang As String
        Dim sub_lang As String
        Dim sub_title As Boolean

        Dim strSQL As String
        Dim dtTemp As New DataTable
        strSQL = "Select TOP(1)* From thaisia.system_language WHERE c_app_name = '" & strAppName & "'"
        dtTemp = cnDB.ExecuteDataTable(strSQL)
        If dtTemp.Rows.Count > 0 Then
            config.strLang_Main = dtTemp.Rows(0)("main_language")
            config.strLang_Sub = dtTemp.Rows(0)("sub_language")
            config.blSubTitle = IIf(UCase(dtTemp.Rows(0)("enable_subtitle")) = "T", True, False)
            Return config
        Else
            config.strLang_Main = ""
            config.strLang_Sub = ""
            config.blSubTitle = False
            Return config
        End If
    End Function

    'Public Function Get_LanguageConfig(strAppName As String, cnDB As clsDB) As Tuple(Of String, String, Boolean)
    '    Dim main_lang As String
    '    Dim sub_lang As String
    '    Dim sub_title As Boolean

    '    Dim strSQL As String
    '    Dim dtTemp As New DataTable
    '    strSQL = "Select TOP(1)* From thaisia.system_language WHERE c_app_name = '" & strAppName & "'"
    '    dtTemp = cnDB.ExecuteDataTable(strSQL)
    '    If dtTemp.Rows.Count > 0 Then
    '        main_lang = dtTemp.Rows(0)("main_language")
    '        sub_lang = dtTemp.Rows(0)("sub_language")
    '        sub_title = IIf(UCase(dtTemp.Rows(0)("enable_subtitle")) = "T", True, False)
    '        Return Tuple.Create(main_lang, sub_lang, sub_title)
    '    Else
    '        Return Tuple.Create("", "", False)
    '    End If
    'End Function

    '======================== Function for prepare text from database by any language
    'Public Sub prepare_language_item(ByRef cmb As ComboBox, cnDB As clsDB)
    '    Dim dtTemp As New DataTable
    '    Dim strSQL As String
    '    strSQL = "SELECT c_language_code FROM thaisia.language_config WHERE f_enable = 'T'"
    '    dtTemp = cnDB.ExecuteDataTable(strSQL)
    '    If dtTemp.Rows.Count > 0 Then
    '        cmb.Items.Clear()
    '        For i = 0 To dtTemp.Rows.Count - 1
    '            cmb.Items.Add(dtTemp.Rows(i)("c_language_code").ToString)
    '        Next
    '    Else
    '        cmb.Visible = False
    '    End If
    'End Sub
    Public Sub prepare_language_item(ByVal cmb As Object, ByVal cnDB As clsDB)
        Dim strSQL As String = "SELECT c_language_code FROM thaisia.language_config WHERE f_enable = 'T'"
        Dim dtTemp As DataTable = cnDB.ExecuteDataTable(strSQL)

        If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
            If TypeOf cmb Is ComboBox Then
                Dim cb As ComboBox = DirectCast(cmb, ComboBox)
                cb.Items.Clear()
                For Each row As DataRow In dtTemp.Rows
                    cb.Items.Add(CStr(row("c_language_code")))
                Next

            ElseIf TypeOf cmb Is ToolStripComboBox Then
                Dim tscb As ToolStripComboBox = DirectCast(cmb, ToolStripComboBox)
                Dim inner As ComboBox = tscb.ComboBox
                If inner IsNot Nothing Then
                    inner.Items.Clear()
                    For Each row As DataRow In dtTemp.Rows
                        inner.Items.Add(CStr(row("c_language_code")))
                    Next
                End If
            End If

        Else
            If TypeOf cmb Is ComboBox Then
                Dim cb As ComboBox = DirectCast(cmb, ComboBox)
                cb.Visible = False

            ElseIf TypeOf cmb Is ToolStripComboBox Then
                Dim tscb As ToolStripComboBox = DirectCast(cmb, ToolStripComboBox)
                tscb.Visible = False
            End If
        End If
    End Sub

    Public Function Get_Text_MultiLanguage(strTableName As String, strLangCode As String, cnDB As clsDB) As DataTable
        Dim dtTemp As New DataTable
        Dim strSQL As String
        strSQL = "SELECT * FROM thaisia." & strTableName & " WHERE c_language_code = '" & strLangCode & "'"
        dtTemp = cnDB.ExecuteDataTable(strSQL)
        If dtTemp.Rows.Count > 0 Then
            Return dtTemp.Copy
        Else
            Return Nothing
        End If
    End Function

    Public Sub Prepare_AlarmEXE_Text(language As language_config, cnDB As clsDB)
        With language
            If .strLang_Main <> "" Then
                dtAlarmMsg_Main = Get_Text_MultiLanguage("error_desc_language", .strLang_Main, cnDB)
            End If
            If .strLang_Sub <> "" Then
                dtAlarmMsg_Sub = Get_Text_MultiLanguage("error_desc_language", .strLang_Sub, cnDB)
            End If

        End With
    End Sub

    Public Function Get_AlarmMessage_ByAlarmCode(dt As DataTable, Alarm_Code As String) As String
        Dim message As String = ""
        If dt Is Nothing Then
            Return message
        End If
        Dim rows() As DataRow = dt.Select("n_error_code = '" & Alarm_Code & "'")
        If rows IsNot Nothing AndAlso rows.Length > 0 Then
            message = rows(0)("c_text").ToString()
        End If
        Return message
    End Function
    Public Sub change_language_in_collection(ByRef controlCollection As IEnumerable, cnDB As clsDB, strLang As String, strAppName As String, Mainform As String)
        Dim tmpText As String
        For Each control As Object In controlCollection
            ' ถ้า control เป็น groupbox ให้เรียกฟังก์ชันนี้อีกครั้ง
            If TypeOf control Is GroupBox Or TypeOf control Is Panel Or TypeOf control Is TableLayoutPanel Then
                change_language_in_collection(control.Controls, cnDB, strLang, strAppName, Mainform)
            ElseIf TypeOf control Is TabControl Then
                Dim tmpTP As TabControl = CType(control, TabControl)
                For Each tp As TabPage In tmpTP.TabPages
                    change_language_in_collection(tp.Controls, cnDB, strLang, strAppName, Mainform)
                Next
            ElseIf TypeOf control Is ToolStrip Then
                Dim tmpTS As ToolStrip = CType(control, ToolStrip)
                change_language_in_collection(tmpTS.Items, cnDB, strLang, strAppName, Mainform)
            ElseIf TypeOf control Is ContextMenuStrip Then
                Dim tmpCMS As ContextMenuStrip = CType(control, ContextMenuStrip)
                change_language_in_collection(tmpCMS.Items, cnDB, strLang, strAppName, Mainform)
            Else
                Select Case UCase(control.Tag)
                    Case "GLOBAL"
                        tmpText = Query_Text_By_LanguageCode(control.Text, strLang, cnDB)
                        control.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                    Case "APP"
                        tmpText = Query_Text_By_Application(control, strAppName, Mainform, strLang, cnDB)
                        control.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                End Select
            End If
        Next
    End Sub
    Public Sub change_language_in_toolStrip(ByRef toolStripCollection As ToolStripItemCollection, cnDB As clsDB, strLang As String)
        Dim tmpText As String
        For Each control As Object In toolStripCollection
            'If UCase(control.Text) = "UNDER REPAIR" Then
            '    Debug.Print("TEST")
            'End If

            Select Case UCase(control.Tag)
                Case "GLOBAL"

                    tmpText = Query_Text_By_LanguageCode(control.Text, strLang, cnDB)
                    control.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                    'Case "APP"
                    '    tmpText = Query_Text_By_Application(control, strAppName, Mainform, strLang, cnDB)
                    '    control.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
            End Select
        Next

    End Sub
    Public Sub change_Object_InterlockMsg_In_collection(ByRef controlCollection As IEnumerable, cnDB As clsDB, strLang As String)
        For Each control As Object In controlCollection
            If TypeOf control Is TAT_MQTT_CTRL.ctrlTAT_ Then
                Change_Object_InterlockMsg(control, cnDB, strLang)
            End If
        Next
    End Sub
    Public Sub Change_Object_InterlockMsg(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_, cnDB As clsDB, strLang As String)
        'If ctrl.ControlType.ToString = "MOTOR" AndAlso ctrl.Index = 164 Then
        '    Debug.Print("TEST")
        'End If
        If ctrl.strAlarmMessage = "" Then
            ctrl.Set_interlock_message("")
            Exit Sub
        End If
        Dim arrMsg() As String = ctrl.strAlarmMessage.Split("|"c)
        For i As Integer = 0 To arrMsg.Length - 1
            If strLang = "EN" Then
                arrMsg(i) = arrMsg(i)
            Else
                arrMsg(i) = Query_Text_By_LanguageCode(arrMsg(i), strLang, cnDB)
            End If
        Next
        Dim changed_message As String = String.Join("|", arrMsg)
        ctrl.Set_interlock_message(changed_message)
    End Sub

    Public Sub change_Object_Control_In_collection(ByRef controlCollection As IEnumerable, cnDB As clsDB, strLang As String, strAppName As String, Mainform As String)
        Dim tmpText As String
        Dim tmpTag As String = ""
        For Each control As Object In controlCollection
            tmpTag = control.Tag
            If TypeOf control Is TAT_CtrlAlarm.ctrlScale_ Then
                For Each controlLabel As Control In control.Controls
                    If TypeOf controlLabel Is TableLayoutPanel Then
                        change_Object_Control_In_collectionPanel(tmpTag, controlLabel, cnDB, strLang, strAppName, Mainform)
                    ElseIf TypeOf controlLabel Is Label Then
                        Select Case UCase(tmpTag)
                            Case "GLOBAL"
                                tmpText = Query_Text_By_LanguageCode(controlLabel.Text, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                            Case "APP"
                                tmpText = Query_Text_By_Application(controlLabel.Text, strAppName, Mainform, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                        End Select
                    End If
                Next
            ElseIf TypeOf control Is TAT_CtrlAlarm.ctrlMixer_ Then
                For Each controlLabel As Control In control.Controls
                    If TypeOf controlLabel Is TableLayoutPanel Then
                        change_Object_Control_In_collectionPanel(tmpTag, controlLabel, cnDB, strLang, strAppName, Mainform)
                    ElseIf TypeOf controlLabel Is Label Then
                        Select Case UCase(tmpTag)
                            Case "GLOBAL"
                                tmpText = Query_Text_By_LanguageCode(controlLabel.Text, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                            Case "APP"
                                tmpText = Query_Text_By_Application(controlLabel.Text, strAppName, Mainform, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                        End Select
                    End If
                Next
            ElseIf TypeOf control Is TAT_CtrlAlarm.ctrlEvonik_ Then
                For Each controlLabel As Control In control.Controls
                    If TypeOf controlLabel Is TableLayoutPanel Then
                        change_Object_Control_In_collectionPanel(tmpTag, controlLabel, cnDB, strLang, strAppName, Mainform)
                    ElseIf TypeOf controlLabel Is Label Then
                        Select Case UCase(tmpTag)
                            Case "GLOBAL"
                                tmpText = Query_Text_By_LanguageCode(controlLabel.Text, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                            Case "APP"
                                tmpText = Query_Text_By_Application(controlLabel.Text, strAppName, Mainform, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                        End Select
                    End If
                Next
            ElseIf TypeOf control Is TAT_CtrlAlarm.ctrlLoadout_ Then
                For Each controlLabel As Control In control.Controls
                    If TypeOf controlLabel Is TableLayoutPanel Then
                        change_Object_Control_In_collectionPanel(tmpTag, controlLabel, cnDB, strLang, strAppName, Mainform)
                    ElseIf TypeOf controlLabel Is Label Then
                        Select Case UCase(tmpTag)
                            Case "GLOBAL"
                                tmpText = Query_Text_By_LanguageCode(controlLabel.Text, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                            Case "APP"
                                tmpText = Query_Text_By_Application(controlLabel.Text, strAppName, Mainform, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                        End Select
                    End If
                Next
            ElseIf TypeOf control Is TAT_CtrlAlarm.ctrlOther_ Then
                For Each controlLabel As Control In control.Controls
                    If TypeOf controlLabel Is TableLayoutPanel Then
                        change_Object_Control_In_collectionPanel(tmpTag, controlLabel, cnDB, strLang, strAppName, Mainform)
                    ElseIf TypeOf controlLabel Is Label Then
                        Select Case UCase(tmpTag)
                            Case "GLOBAL"
                                tmpText = Query_Text_By_LanguageCode(controlLabel.Text, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                            Case "APP"
                                tmpText = Query_Text_By_Application(controlLabel.Text, strAppName, Mainform, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                        End Select
                    End If
                Next
            ElseIf TypeOf control Is TAT_CtrlAlarm.ctrlScaleLq_ Then
                For Each controlLabel As Control In control.Controls
                    If TypeOf controlLabel Is TableLayoutPanel Then
                        change_Object_Control_In_collectionPanel(tmpTag, controlLabel, cnDB, strLang, strAppName, Mainform)
                    ElseIf TypeOf controlLabel Is Label Then
                        Select Case UCase(tmpTag)
                            Case "GLOBAL"
                                tmpText = Query_Text_By_LanguageCode(controlLabel.Text, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                            Case "APP"
                                tmpText = Query_Text_By_Application(controlLabel.Text, strAppName, Mainform, strLang, cnDB)
                                controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                        End Select
                    End If
                Next
            End If
        Next
    End Sub

    Public Sub change_Object_Control_In_collectionPanel(tmpTag As String, control As Control, cnDB As clsDB, strLang As String, strAppName As String, Mainform As String)
        Dim tmpText As String
        For Each controlLabel As Control In control.Controls
            If TypeOf controlLabel Is TableLayoutPanel Then
                change_Object_Control_In_collectionPanel(tmpTag, controlLabel, cnDB, strLang, strAppName, Mainform)
            ElseIf TypeOf controlLabel Is Label Then
                Select Case UCase(tmpTag)
                    Case "GLOBAL"
                        tmpText = Query_Text_By_LanguageCode(controlLabel.Text, strLang, cnDB)
                        controlLabel.Text = IIf(strLang = "EN", tmpText, tmpText)
                    Case "APP"
                        tmpText = Query_Text_By_Application(controlLabel.Text, strAppName, Mainform, strLang, cnDB)
                        controlLabel.Text = IIf(strLang = "EN", UCase(tmpText), tmpText)
                End Select
            End If
        Next
    End Sub

    Public Function Query_Text_By_LanguageCode(strFind As String, strLangCode As String, cnDB As clsDB) As String
        Dim strSql As String
        Dim tmpDt As DataTable
        Dim Pm_textFind As String = "N'" & strFind & "'"
        strSql = "Exec dbo.FD_ChangeLanguage_by_code '" & strLangCode & "'," & Pm_textFind
        tmpDt = cnDB.ExecuteDataTable(strSql)
        If tmpDt.Rows.Count > 0 Then
            Return tmpDt.Rows(0)("text_result")
        Else
            Return strFind
        End If
    End Function
    Public Function Query_Text_By_Application(ctrl As Object, strAppName As String, Mainform As String, strLangCode As String, cnDB As clsDB) As String
        Dim strSql As String
        Dim tmpDt As DataTable
        strSql = "Exec dbo.FD_Select_languageText_By_App '" & strAppName & "','" & Mainform & "','" & ctrl.Name & "','" & strLangCode & "'"
        tmpDt = cnDB.ExecuteDataTable(strSql)
        If tmpDt.Rows.Count > 0 Then
            Return tmpDt.Rows(0)("c_text")
        Else
            Return ctrl.Text
        End If
    End Function


    '================================= SAVE DEFAULT TEXT ENGLISH FOR EACH APPLICATION
    Public Sub Save_AppLanguage_default(ByRef controlCollection As IEnumerable, cnDB As clsDB, strAppName As String, Mainform As String)
        For Each control As Object In controlCollection
            ' ถ้า control เป็น groupbox ให้เรียกฟังก์ชันนี้อีกครั้ง
            If TypeOf control Is GroupBox Or TypeOf control Is Panel Or TypeOf control Is TableLayoutPanel Then
                Save_AppLanguage_default(control.Controls, cnDB, strAppName, Mainform)
            ElseIf TypeOf control Is TabControl Then
                Dim tmpTP As TabControl = CType(control, TabControl)
                For Each tp As TabPage In tmpTP.TabPages
                    Save_AppLanguage_default(tp.Controls, cnDB, strAppName, Mainform)
                Next
            ElseIf TypeOf control Is ToolStrip Then
                Dim tmpTS As ToolStrip = CType(control, ToolStrip)
                Save_AppLanguage_default(tmpTS.Items, cnDB, strAppName, Mainform)
            Else
                If UCase(control.Tag) = "APP" Then
                    Insert_Default_Text_By_App(control, strAppName, Mainform, cnDB)
                End If
            End If
        Next
    End Sub
    Public Sub Insert_Default_Text_By_App(ctrl As Object, strAppName As String, Mainform As String, cnDB As clsDB)
        Dim strSql As String
        strSql = "Exec dbo.FD_Insert_Default_language_By_App '" & strAppName & "','" & Mainform & "','" & ctrl.Name & "','" & ctrl.Text & "','EN'"
        cnDB.ExecuteNoneQuery(strSql)
    End Sub

    '================================= SAVE DEFAULT TEXT ENGLISH FOR GLOBAL
    Public Sub Save_TextGlobal_default(ByRef controlCollection As IEnumerable, cnDB As clsDB)
        For Each control As Object In controlCollection
            ' ถ้า control เป็น groupbox ให้เรียกฟังก์ชันนี้อีกครั้ง
            If TypeOf control Is GroupBox Or TypeOf control Is Panel Or TypeOf control Is TableLayoutPanel Then
                Save_TextGlobal_default(control.Controls, cnDB)
            ElseIf TypeOf control Is TabControl Then
                Dim tmpTP As TabControl = CType(control, TabControl)
                For Each tp As TabPage In tmpTP.TabPages
                    Save_TextGlobal_default(tp.Controls, cnDB)
                Next
            ElseIf TypeOf control Is ToolStrip Then
                Dim tmpTS As ToolStrip = CType(control, ToolStrip)
                Save_TextGlobal_default(tmpTS.Items, cnDB)
            ElseIf TypeOf control Is ContextMenuStrip Then
                Dim tmpCMS As ContextMenuStrip = CType(control, ContextMenuStrip)
                Save_TextGlobal_default(tmpCMS.Items, cnDB)
            Else
                If UCase(control.Tag) = "GLOBAL" Then
                    Insert_Default_Text_Global(control, cnDB)
                End If
            End If
        Next
    End Sub
    Public Sub Insert_Default_Text_Global(ctrl As Object, cnDB As clsDB)
        Dim strSql As String
        strSql = "Exec dbo.FD_Insert_Default_language_Global '" & ctrl.Text & "','EN','" & ctrl.Text & "'"
        cnDB.ExecuteNoneQuery(strSql)
    End Sub
End Module
