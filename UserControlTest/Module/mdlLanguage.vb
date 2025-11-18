Module mdlLanguage

    Public structApp_Language As language_config

    Public Structure language_config
        Public strLang_Main As String
        Public strLang_Sub As String
        Public blSubTitle As Boolean
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

    ' ====== CACHE กลางสำหรับข้อความหลายภาษา ======
    ' key: strLangCode | originalText
    Private ReadOnly _langCache As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

    ' CACHE สำหรับข้อความแบบ Application-specific
    ' key: strLangCode | AppName | Mainform | ControlName
    Private ReadOnly _appTextCache As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)

    Public Sub RaiseEvent_ChangeLanguage(ByVal strLangCode As String, ByRef cnDB As clsDB)
        ' เวลาเปลี่ยนภาษา เคลียร์ cache เก่าทิ้งก่อน
        _langCache.Clear()
        _appTextCache.Clear()

        RaiseEvent Langauge_Change(strLangCode, cnDB)
    End Sub

    '================ CONFIG ภาษา =================
    Public Function Get_LanguageConfig(strAppName As String, cnDB As clsDB) As language_config
        Dim config As New language_config

        Dim strSQL As String = "Select TOP(1)* From thaisia.system_language WHERE c_app_name = '" & strAppName & "'"
        Dim dtTemp As DataTable = cnDB.ExecuteDataTable(strSQL)

        If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
            config.strLang_Main = dtTemp.Rows(0)("main_language").ToString()
            config.strLang_Sub = dtTemp.Rows(0)("sub_language").ToString()
            config.blSubTitle = (UCase(dtTemp.Rows(0)("enable_subtitle").ToString()) = "T")
        Else
            config.strLang_Main = ""
            config.strLang_Sub = ""
            config.blSubTitle = False
        End If

        Return config
    End Function

    '======================== เตรียม list ภาษาใน ComboBox / ToolStripComboBox =================
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

    '======================== ดึง Text ตามภาษา =================
    Public Function Get_Text_MultiLanguage(strTableName As String, strLangCode As String, cnDB As clsDB) As DataTable
        Dim strSQL As String = "SELECT * FROM thaisia." & strTableName & " WHERE c_language_code = '" & strLangCode & "'"
        Dim dtTemp As DataTable = cnDB.ExecuteDataTable(strSQL)
        If dtTemp IsNot Nothing AndAlso dtTemp.Rows.Count > 0 Then
            Return dtTemp.Copy()
        Else
            Return Nothing
        End If
    End Function

    ' เตรียมข้อความ Alarm จากตาราง language ล่วงหน้า (สำหรับ exe alarm)
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
        If dt Is Nothing Then Return message

        Dim rows() As DataRow = dt.Select("n_error_code = '" & Alarm_Code & "'")
        If rows IsNot Nothing AndAlso rows.Length > 0 Then
            message = rows(0)("c_text").ToString()
        End If
        Return message
    End Function

    '======================== เปลี่ยนภาษาใน Control Collection =================
    Public Sub change_language_in_collection(ByRef controlCollection As IEnumerable, cnDB As clsDB, strLang As String, strAppName As String, Mainform As String)
        Dim tmpText As String

        For Each control As Object In controlCollection
            ' ถ้า control เป็น groupbox/panel/tablelayout → dive ลงไปใน children
            If TypeOf control Is GroupBox OrElse
               TypeOf control Is Panel OrElse
               TypeOf control Is TableLayoutPanel Then

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
                Select Case UCase(CStr(control.Tag))
                    Case "GLOBAL"
                        tmpText = Query_Text_By_LanguageCode(control.Text, strLang, cnDB)
                        control.Text = If(strLang = "EN", UCase(tmpText), tmpText)

                    Case "APP"
                        tmpText = Query_Text_By_Application(control, strAppName, Mainform, strLang, cnDB)
                        control.Text = If(strLang = "EN", UCase(tmpText), tmpText)
                End Select
            End If
        Next
    End Sub

    Public Sub change_language_in_toolStrip(ByRef toolStripCollection As ToolStripItemCollection, cnDB As clsDB, strLang As String)
        Dim tmpText As String

        For Each control As Object In toolStripCollection
            Select Case UCase(CStr(control.Tag))
                Case "GLOBAL"
                    tmpText = Query_Text_By_LanguageCode(control.Text, strLang, cnDB)
                    control.Text = If(strLang = "EN", UCase(tmpText), tmpText)
            End Select
        Next
    End Sub

    '======================== เปลี่ยนภาษา Interlock Message ใน ctrlTAT_ =================
    Public Sub change_Object_InterlockMsg_In_collection(ByRef controlCollection As IEnumerable, cnDB As clsDB, strLang As String)
        For Each control As Object In controlCollection
            If TypeOf control Is TAT_MQTT_CTRL.ctrlTAT_ Then
                Change_Object_InterlockMsg(control, cnDB, strLang)
            End If
        Next
    End Sub

    Public Sub Change_Object_InterlockMsg(ByRef ctrl As TAT_MQTT_CTRL.ctrlTAT_, cnDB As clsDB, strLang As String)
        If ctrl.strAlarmMessage = "" Then
            ctrl.Set_interlock_message("")
            Exit Sub
        End If

        Dim arrMsg() As String = ctrl.strAlarmMessage.Split("|"c)

        For i As Integer = 0 To arrMsg.Length - 1
            If strLang = "EN" Then
                ' ภาษา EN ใช้ข้อความเดิม (สมมติว่า default คือ EN)
                arrMsg(i) = arrMsg(i)
            Else
                arrMsg(i) = Query_Text_By_LanguageCode(arrMsg(i), strLang, cnDB)
            End If
        Next

        Dim changed_message As String = String.Join("|", arrMsg)
        ctrl.Set_interlock_message(changed_message)
    End Sub

    '======================== เปลี่ยนภาษาใน Control พิเศษ (Custom Alarm Controls) =================
    Public Sub change_Object_Control_In_collection(ByRef controlCollection As IEnumerable, cnDB As clsDB, strLang As String, strAppName As String, Mainform As String)
        Dim tmpText As String
        Dim tmpTag As String = ""

        For Each control As Object In controlCollection
            tmpTag = CStr(control.Tag)

            If TypeOf control Is TAT_CtrlAlarm.ctrlScale_ OrElse
               TypeOf control Is TAT_CtrlAlarm.ctrlMixer_ OrElse
               TypeOf control Is TAT_CtrlAlarm.ctrlEvonik_ OrElse
               TypeOf control Is TAT_CtrlAlarm.ctrlLoadout_ OrElse
               TypeOf control Is TAT_CtrlAlarm.ctrlOther_ OrElse
               TypeOf control Is TAT_CtrlAlarm.ctrlScaleLq_ Then

                For Each controlLabel As Control In control.Controls
                    If TypeOf controlLabel Is TableLayoutPanel Then
                        change_Object_Control_In_collectionPanel(tmpTag, controlLabel, cnDB, strLang, strAppName, Mainform)
                    ElseIf TypeOf controlLabel Is Label Then
                        Select Case UCase(tmpTag)
                            Case "GLOBAL"
                                tmpText = Query_Text_By_LanguageCode(controlLabel.Text, strLang, cnDB)
                                controlLabel.Text = If(strLang = "EN", UCase(tmpText), tmpText)
                            Case "APP"
                                tmpText = Query_Text_By_Application(controlLabel.Text, strAppName, Mainform, strLang, cnDB)
                                controlLabel.Text = If(strLang = "EN", UCase(tmpText), tmpText)
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
                        controlLabel.Text = If(strLang = "EN", tmpText, tmpText)
                    Case "APP"
                        tmpText = Query_Text_By_Application(controlLabel.Text, strAppName, Mainform, strLang, cnDB)
                        controlLabel.Text = If(strLang = "EN", UCase(tmpText), tmpText)
                End Select
            End If
        Next
    End Sub

    '======================== ฟังก์ชันหลัก: Query Text ตาม Language Code (มี CACHE) =================
    Public Function Query_Text_By_LanguageCode(strFind As String, strLangCode As String, cnDB As clsDB) As String
        If String.IsNullOrWhiteSpace(strFind) Then
            Return ""
        End If

        Dim key As String = strLangCode & "|" & strFind
        Dim cached As String = Nothing

        ' 1) ถ้ามีใน cache แล้ว → ดึงจาก cache เลย
        If _langCache.TryGetValue(key, cached) Then
            Return cached
        End If

        ' 2) ยังไม่มี → ยิง Stored Procedure
        Dim Pm_textFind As String = "N'" & strFind & "'"
        Dim strSql As String = "Exec dbo.FD_ChangeLanguage_by_code '" & strLangCode & "'," & Pm_textFind
        Dim tmpDt As DataTable = cnDB.ExecuteDataTable(strSql)

        Dim result As String
        If tmpDt IsNot Nothing AndAlso tmpDt.Rows.Count > 0 Then
            result = tmpDt.Rows(0)("text_result").ToString()
        Else
            result = strFind
        End If

        ' 3) เก็บลง cache แล้วคืนค่า
        _langCache(key) = result
        Return result
    End Function

    '======================== ฟังก์ชัน: Query Text แบบ Application-specific (มี CACHE) =================
    Public Function Query_Text_By_Application(ctrl As Object, strAppName As String, Mainform As String, strLangCode As String, cnDB As clsDB) As String
        ' ctrl จะเป็น Control / ToolStripItem ที่มี Name และ Text
        Dim ctrlName As String = ""
        Dim defaultText As String = ""

        Try
            ctrlName = CStr(ctrl.Name)
        Catch
            ctrlName = ""
        End Try

        Try
            defaultText = CStr(ctrl.Text)
        Catch
            defaultText = ""
        End Try

        ' สร้าง key สำหรับ cache
        Dim key As String = strLangCode & "|" & strAppName & "|" & Mainform & "|" & ctrlName
        Dim cached As String = Nothing

        ' 1) ถ้ามีใน cache แล้ว → ใช้เลย
        If _appTextCache.TryGetValue(key, cached) Then
            Return cached
        End If

        ' 2) ยังไม่มี → ยิง Stored Procedure
        Dim strSql As String = "Exec dbo.FD_Select_languageText_By_App '" &
                               strAppName & "','" &
                               Mainform & "','" &
                               ctrlName & "','" &
                               strLangCode & "'"

        Dim tmpDt As DataTable = cnDB.ExecuteDataTable(strSql)

        Dim result As String
        If tmpDt IsNot Nothing AndAlso tmpDt.Rows.Count > 0 Then
            result = tmpDt.Rows(0)("c_text").ToString()
        Else
            result = defaultText
        End If

        ' 3) เก็บใน cache แล้วคืนค่า
        _appTextCache(key) = result
        Return result
    End Function


    '================================= SAVE DEFAULT TEXT ENGLISH FOR EACH APPLICATION =================
    Public Sub Save_AppLanguage_default(ByRef controlCollection As IEnumerable, cnDB As clsDB, strAppName As String, Mainform As String)
        For Each control As Object In controlCollection
            ' ถ้า control เป็น groupbox ให้เรียกฟังก์ชันนี้อีกครั้ง
            If TypeOf control Is GroupBox OrElse
               TypeOf control Is Panel OrElse
               TypeOf control Is TableLayoutPanel Then

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
                If UCase(CStr(control.Tag)) = "APP" Then
                    Insert_Default_Text_By_App(control, strAppName, Mainform, cnDB)
                End If
            End If
        Next
    End Sub

    Public Sub Insert_Default_Text_By_App(ctrl As Object, strAppName As String, Mainform As String, cnDB As clsDB)
        Dim strSql As String = "Exec dbo.FD_Insert_Default_language_By_App '" &
                               strAppName & "','" &
                               Mainform & "','" &
                               ctrl.Name & "','" &
                               ctrl.Text & "','EN'"
        cnDB.ExecuteNoneQuery(strSql)
    End Sub

    '================================= SAVE DEFAULT TEXT ENGLISH FOR GLOBAL =================
    Public Sub Save_TextGlobal_default(ByRef controlCollection As IEnumerable, cnDB As clsDB)
        For Each control As Object In controlCollection
            ' ถ้า control เป็น groupbox ให้เรียกฟังก์ชันนี้อีกครั้ง
            If TypeOf control Is GroupBox OrElse
               TypeOf control Is Panel OrElse
               TypeOf control Is TableLayoutPanel Then

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
                If UCase(CStr(control.Tag)) = "GLOBAL" Then
                    Insert_Default_Text_Global(control, cnDB)
                End If
            End If
        Next
    End Sub

    Public Sub Insert_Default_Text_Global(ctrl As Object, cnDB As clsDB)
        Dim strSql As String = "Exec dbo.FD_Insert_Default_language_Global '" &
                               ctrl.Text & "','EN','" & ctrl.Text & "'"
        cnDB.ExecuteNoneQuery(strSql)
    End Sub

End Module
