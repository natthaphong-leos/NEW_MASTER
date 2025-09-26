<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMenu_Motor
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.ctxMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuHeader = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuMode_Auto = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMode_Manual = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuStart = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuStop = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuJog = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuManualLQ = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuResetError = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuUnderRepair = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSelect_Auto = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSelect_Manual = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuStartBatch = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMonitoring = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeBatchPreset = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuResetBatchQ = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuChangeDestBin = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuParameter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSelect_Hold = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuTopenOverflow = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuPidAuto = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPidManual = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuPidConfig = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuResetPulse = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuParameterLQ_HA = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuBinParameter = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuBucketParameter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHammParameter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuMixerParameter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuAnalogParameter = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetValue = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSetDelayTime = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetDelayTimeHigh = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetDelayTimeLow = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator8 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSetJogTime = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetLowTime = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetHightTime = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetWeight = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator9 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuShowRoute = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuHideRoute = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetCleanTime = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsSeparator10 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuSetTarJog = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSetActJog = New System.Windows.Forms.ToolStripMenuItem()
        Me.ctxMenuStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'ctxMenuStrip
        '
        Me.ctxMenuStrip.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuHeader, Me.tsSeparator1, Me.mnuMode_Auto, Me.mnuMode_Manual, Me.tsSeparator2, Me.mnuStart, Me.mnuStop, Me.mnuJog, Me.mnuManualLQ, Me.mnuResetError, Me.mnuUnderRepair, Me.tsSeparator3, Me.mnuSelect_Auto, Me.mnuSelect_Manual, Me.mnuStartBatch, Me.mnuMonitoring, Me.mnuChangeBatchPreset, Me.mnuResetBatchQ, Me.mnuChangeDestBin, Me.mnuParameter, Me.mnuSelect_Hold, Me.mnuTopenOverflow, Me.tsSeparator4, Me.mnuPidAuto, Me.mnuPidManual, Me.mnuPidConfig, Me.tsSeparator5, Me.mnuResetPulse, Me.mnuParameterLQ_HA, Me.mnuBinParameter, Me.tsSeparator6, Me.mnuBucketParameter, Me.mnuHammParameter, Me.mnuMixerParameter, Me.mnuAnalogParameter, Me.mnuSetValue, Me.tsSeparator7, Me.mnuSetDelayTime, Me.mnuSetDelayTimeHigh, Me.mnuSetDelayTimeLow, Me.tsSeparator8, Me.mnuSetJogTime, Me.mnuSetLowTime, Me.mnuSetHightTime, Me.mnuSetWeight, Me.tsSeparator9, Me.mnuShowRoute, Me.mnuHideRoute, Me.mnuSetCleanTime, Me.tsSeparator10, Me.mnuSetTarJog, Me.mnuSetActJog})
        Me.ctxMenuStrip.Name = "ctxMenuStrip"
        Me.ctxMenuStrip.Size = New System.Drawing.Size(238, 988)
        '
        'mnuHeader
        '
        Me.mnuHeader.Name = "mnuHeader"
        Me.mnuHeader.Size = New System.Drawing.Size(237, 22)
        Me.mnuHeader.Text = "HEADER"
        '
        'tsSeparator1
        '
        Me.tsSeparator1.Name = "tsSeparator1"
        Me.tsSeparator1.Size = New System.Drawing.Size(234, 6)
        '
        'mnuMode_Auto
        '
        Me.mnuMode_Auto.Image = Global.Project.My.Resources.Resources.UI_Apply_16x16
        Me.mnuMode_Auto.Name = "mnuMode_Auto"
        Me.mnuMode_Auto.Size = New System.Drawing.Size(237, 22)
        Me.mnuMode_Auto.Tag = "Global"
        Me.mnuMode_Auto.Text = "AUTO"
        '
        'mnuMode_Manual
        '
        Me.mnuMode_Manual.Image = Global.Project.My.Resources.Resources.UI_SetRedToBlack_16x16
        Me.mnuMode_Manual.Name = "mnuMode_Manual"
        Me.mnuMode_Manual.Size = New System.Drawing.Size(237, 22)
        Me.mnuMode_Manual.Tag = "Global"
        Me.mnuMode_Manual.Text = "MANUAL"
        '
        'tsSeparator2
        '
        Me.tsSeparator2.Name = "tsSeparator2"
        Me.tsSeparator2.Size = New System.Drawing.Size(234, 6)
        '
        'mnuStart
        '
        Me.mnuStart.Image = Global.Project.My.Resources.Resources.UI_Play_16x16
        Me.mnuStart.Name = "mnuStart"
        Me.mnuStart.Size = New System.Drawing.Size(237, 22)
        Me.mnuStart.Tag = "Global"
        Me.mnuStart.Text = "MANUAL START"
        '
        'mnuStop
        '
        Me.mnuStop.Image = Global.Project.My.Resources.Resources.UI_Stop_16x16
        Me.mnuStop.Name = "mnuStop"
        Me.mnuStop.Size = New System.Drawing.Size(237, 22)
        Me.mnuStop.Tag = "Global"
        Me.mnuStop.Text = "MANUAL STOP"
        '
        'mnuJog
        '
        Me.mnuJog.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleHalfCircular_16x16
        Me.mnuJog.Name = "mnuJog"
        Me.mnuJog.Size = New System.Drawing.Size(237, 22)
        Me.mnuJog.Tag = "Global"
        Me.mnuJog.Text = "MANUAL OPERATE"
        '
        'mnuManualLQ
        '
        Me.mnuManualLQ.Image = Global.Project.My.Resources.Resources.UI_GaugeShowCaptions_16x16
        Me.mnuManualLQ.Name = "mnuManualLQ"
        Me.mnuManualLQ.Size = New System.Drawing.Size(237, 22)
        Me.mnuManualLQ.Tag = "Global"
        Me.mnuManualLQ.Text = "MANUAL LIQUID"
        '
        'mnuResetError
        '
        Me.mnuResetError.Image = Global.Project.My.Resources.Resources.UI_Refresh_16x16
        Me.mnuResetError.Name = "mnuResetError"
        Me.mnuResetError.Size = New System.Drawing.Size(237, 22)
        Me.mnuResetError.Tag = "Global"
        Me.mnuResetError.Text = "RESET ERROR"
        '
        'mnuUnderRepair
        '
        Me.mnuUnderRepair.Image = Global.Project.My.Resources.Resources.UI_Cancel_16x16
        Me.mnuUnderRepair.Name = "mnuUnderRepair"
        Me.mnuUnderRepair.Size = New System.Drawing.Size(237, 22)
        Me.mnuUnderRepair.Tag = "Global"
        Me.mnuUnderRepair.Text = "UNDER REPAIR"
        '
        'tsSeparator3
        '
        Me.tsSeparator3.Name = "tsSeparator3"
        Me.tsSeparator3.Size = New System.Drawing.Size(234, 6)
        '
        'mnuSelect_Auto
        '
        Me.mnuSelect_Auto.Image = Global.Project.My.Resources.Resources.UI_Apply_16x16
        Me.mnuSelect_Auto.Name = "mnuSelect_Auto"
        Me.mnuSelect_Auto.Size = New System.Drawing.Size(237, 22)
        Me.mnuSelect_Auto.Tag = "Global"
        Me.mnuSelect_Auto.Text = "AUTO"
        '
        'mnuSelect_Manual
        '
        Me.mnuSelect_Manual.Image = Global.Project.My.Resources.Resources.UI_SetRedToBlack_16x16
        Me.mnuSelect_Manual.Name = "mnuSelect_Manual"
        Me.mnuSelect_Manual.Size = New System.Drawing.Size(237, 22)
        Me.mnuSelect_Manual.Tag = "Global"
        Me.mnuSelect_Manual.Text = "MANUAL"
        '
        'mnuStartBatch
        '
        Me.mnuStartBatch.Image = Global.Project.My.Resources.Resources.UI_Redo_16x16
        Me.mnuStartBatch.Name = "mnuStartBatch"
        Me.mnuStartBatch.Size = New System.Drawing.Size(237, 22)
        Me.mnuStartBatch.Tag = "Global"
        Me.mnuStartBatch.Text = "START BATCH"
        '
        'mnuMonitoring
        '
        Me.mnuMonitoring.Image = Global.Project.My.Resources.Resources.UI_Reading_16x16
        Me.mnuMonitoring.Name = "mnuMonitoring"
        Me.mnuMonitoring.Size = New System.Drawing.Size(237, 22)
        Me.mnuMonitoring.Tag = "Global"
        Me.mnuMonitoring.Text = "FORMULA MONITORING"
        '
        'mnuChangeBatchPreset
        '
        Me.mnuChangeBatchPreset.Image = Global.Project.My.Resources.Resources.UI_Edit_16x16
        Me.mnuChangeBatchPreset.Name = "mnuChangeBatchPreset"
        Me.mnuChangeBatchPreset.Size = New System.Drawing.Size(237, 22)
        Me.mnuChangeBatchPreset.Tag = "Global"
        Me.mnuChangeBatchPreset.Text = "CHANGE BATCH PRESET"
        '
        'mnuResetBatchQ
        '
        Me.mnuResetBatchQ.Image = Global.Project.My.Resources.Resources.UI_Delete_16x16
        Me.mnuResetBatchQ.Name = "mnuResetBatchQ"
        Me.mnuResetBatchQ.Size = New System.Drawing.Size(237, 22)
        Me.mnuResetBatchQ.Tag = "Global"
        Me.mnuResetBatchQ.Text = "RESET BATCH QUEUE"
        '
        'mnuChangeDestBin
        '
        Me.mnuChangeDestBin.Image = Global.Project.My.Resources.Resources.UI_DocumentMap_16x16
        Me.mnuChangeDestBin.Name = "mnuChangeDestBin"
        Me.mnuChangeDestBin.Size = New System.Drawing.Size(237, 22)
        Me.mnuChangeDestBin.Tag = "Global"
        Me.mnuChangeDestBin.Text = "CHANGE DESTINATION"
        '
        'mnuParameter
        '
        Me.mnuParameter.Image = Global.Project.My.Resources.Resources.UI_PageSetup_16x16
        Me.mnuParameter.Name = "mnuParameter"
        Me.mnuParameter.Size = New System.Drawing.Size(237, 22)
        Me.mnuParameter.Tag = "Global"
        Me.mnuParameter.Text = "PARAMETER"
        '
        'mnuSelect_Hold
        '
        Me.mnuSelect_Hold.Image = Global.Project.My.Resources.Resources.UI_Next_16x16
        Me.mnuSelect_Hold.Name = "mnuSelect_Hold"
        Me.mnuSelect_Hold.Size = New System.Drawing.Size(237, 22)
        Me.mnuSelect_Hold.Tag = "Global"
        Me.mnuSelect_Hold.Text = "HOLD"
        '
        'mnuTopenOverflow
        '
        Me.mnuTopenOverflow.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuTopenOverflow.Name = "mnuTopenOverflow"
        Me.mnuTopenOverflow.Size = New System.Drawing.Size(237, 22)
        Me.mnuTopenOverflow.Tag = "Global"
        Me.mnuTopenOverflow.Text = "SET DELAY TIME OVERFLOW"
        '
        'tsSeparator4
        '
        Me.tsSeparator4.Name = "tsSeparator4"
        Me.tsSeparator4.Size = New System.Drawing.Size(234, 6)
        '
        'mnuPidAuto
        '
        Me.mnuPidAuto.Image = Global.Project.My.Resources.Resources.UI_Forward_16x16
        Me.mnuPidAuto.Name = "mnuPidAuto"
        Me.mnuPidAuto.Size = New System.Drawing.Size(237, 22)
        Me.mnuPidAuto.Tag = "Global"
        Me.mnuPidAuto.Text = "PID AUTO"
        '
        'mnuPidManual
        '
        Me.mnuPidManual.Image = Global.Project.My.Resources.Resources.UI_TouchMode_16x16
        Me.mnuPidManual.Name = "mnuPidManual"
        Me.mnuPidManual.Size = New System.Drawing.Size(237, 22)
        Me.mnuPidManual.Tag = "Global"
        Me.mnuPidManual.Text = "PID MANUAL"
        '
        'mnuPidConfig
        '
        Me.mnuPidConfig.Image = Global.Project.My.Resources.Resources.UI_Technology_16x16
        Me.mnuPidConfig.Name = "mnuPidConfig"
        Me.mnuPidConfig.Size = New System.Drawing.Size(237, 22)
        Me.mnuPidConfig.Tag = "Global"
        Me.mnuPidConfig.Text = "PID CONFIG"
        '
        'tsSeparator5
        '
        Me.tsSeparator5.Name = "tsSeparator5"
        Me.tsSeparator5.Size = New System.Drawing.Size(234, 6)
        '
        'mnuResetPulse
        '
        Me.mnuResetPulse.Image = Global.Project.My.Resources.Resources.UI_Recurrence_16x16
        Me.mnuResetPulse.Name = "mnuResetPulse"
        Me.mnuResetPulse.Size = New System.Drawing.Size(237, 22)
        Me.mnuResetPulse.Tag = "Global"
        Me.mnuResetPulse.Text = "RESET PULSE"
        '
        'mnuParameterLQ_HA
        '
        Me.mnuParameterLQ_HA.Image = Global.Project.My.Resources.Resources.UI_SolidOrangeDataBar_16x16
        Me.mnuParameterLQ_HA.Name = "mnuParameterLQ_HA"
        Me.mnuParameterLQ_HA.Size = New System.Drawing.Size(237, 22)
        Me.mnuParameterLQ_HA.Tag = "Global"
        Me.mnuParameterLQ_HA.Text = "LIQUID PARAMETER"
        '
        'mnuBinParameter
        '
        Me.mnuBinParameter.Image = Global.Project.My.Resources.Resources.UI_SolidGreenDataBar_16x16
        Me.mnuBinParameter.Name = "mnuBinParameter"
        Me.mnuBinParameter.Size = New System.Drawing.Size(237, 22)
        Me.mnuBinParameter.Tag = "Global"
        Me.mnuBinParameter.Text = "BIN PARAMETER"
        '
        'tsSeparator6
        '
        Me.tsSeparator6.Name = "tsSeparator6"
        Me.tsSeparator6.Size = New System.Drawing.Size(234, 6)
        '
        'mnuBucketParameter
        '
        Me.mnuBucketParameter.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleRightQuarterCircular_32x32
        Me.mnuBucketParameter.Name = "mnuBucketParameter"
        Me.mnuBucketParameter.Size = New System.Drawing.Size(237, 22)
        Me.mnuBucketParameter.Tag = "Global"
        Me.mnuBucketParameter.Text = "BUCKET PARAMETER"
        '
        'mnuHammParameter
        '
        Me.mnuHammParameter.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleLinearVertical_16x16
        Me.mnuHammParameter.Name = "mnuHammParameter"
        Me.mnuHammParameter.Size = New System.Drawing.Size(237, 22)
        Me.mnuHammParameter.Tag = "Global"
        Me.mnuHammParameter.Text = "HAMMER MILL PARAMETER"
        '
        'mnuMixerParameter
        '
        Me.mnuMixerParameter.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleLinearHorizontal_16x16
        Me.mnuMixerParameter.Name = "mnuMixerParameter"
        Me.mnuMixerParameter.Size = New System.Drawing.Size(237, 22)
        Me.mnuMixerParameter.Tag = "Global"
        Me.mnuMixerParameter.Text = "MIXER PARAMETER"
        '
        'mnuAnalogParameter
        '
        Me.mnuAnalogParameter.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleThreeForthCircular_16x16
        Me.mnuAnalogParameter.Name = "mnuAnalogParameter"
        Me.mnuAnalogParameter.Size = New System.Drawing.Size(237, 22)
        Me.mnuAnalogParameter.Tag = "Global"
        Me.mnuAnalogParameter.Text = "ANALOG PARAMETER"
        '
        'mnuSetValue
        '
        Me.mnuSetValue.Image = Global.Project.My.Resources.Resources.UI_Edit_16x16
        Me.mnuSetValue.Name = "mnuSetValue"
        Me.mnuSetValue.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetValue.Tag = "Global"
        Me.mnuSetValue.Text = "SET VALUE"
        '
        'tsSeparator7
        '
        Me.tsSeparator7.Name = "tsSeparator7"
        Me.tsSeparator7.Size = New System.Drawing.Size(234, 6)
        '
        'mnuSetDelayTime
        '
        Me.mnuSetDelayTime.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuSetDelayTime.Name = "mnuSetDelayTime"
        Me.mnuSetDelayTime.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetDelayTime.Tag = "Global"
        Me.mnuSetDelayTime.Text = "SET DELAY TIME"
        '
        'mnuSetDelayTimeHigh
        '
        Me.mnuSetDelayTimeHigh.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuSetDelayTimeHigh.Name = "mnuSetDelayTimeHigh"
        Me.mnuSetDelayTimeHigh.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetDelayTimeHigh.Tag = "Global"
        Me.mnuSetDelayTimeHigh.Text = "SET DELAY TIME (HIGH)"
        '
        'mnuSetDelayTimeLow
        '
        Me.mnuSetDelayTimeLow.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuSetDelayTimeLow.Name = "mnuSetDelayTimeLow"
        Me.mnuSetDelayTimeLow.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetDelayTimeLow.Tag = "Global"
        Me.mnuSetDelayTimeLow.Text = "SET DELAY TIME (LOW)"
        '
        'tsSeparator8
        '
        Me.tsSeparator8.Name = "tsSeparator8"
        Me.tsSeparator8.Size = New System.Drawing.Size(234, 6)
        '
        'mnuSetJogTime
        '
        Me.mnuSetJogTime.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuSetJogTime.Name = "mnuSetJogTime"
        Me.mnuSetJogTime.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetJogTime.Tag = "Global"
        Me.mnuSetJogTime.Text = "SET JOG TIME"
        '
        'mnuSetLowTime
        '
        Me.mnuSetLowTime.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuSetLowTime.Name = "mnuSetLowTime"
        Me.mnuSetLowTime.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetLowTime.Tag = "Global"
        Me.mnuSetLowTime.Text = "SET LOW TIME"
        '
        'mnuSetHightTime
        '
        Me.mnuSetHightTime.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuSetHightTime.Name = "mnuSetHightTime"
        Me.mnuSetHightTime.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetHightTime.Tag = "Global"
        Me.mnuSetHightTime.Text = "SET HIGHT TIME"
        '
        'mnuSetWeight
        '
        Me.mnuSetWeight.Image = Global.Project.My.Resources.Resources.UI_GaugeShowCaptions_16x16
        Me.mnuSetWeight.Name = "mnuSetWeight"
        Me.mnuSetWeight.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetWeight.Tag = "Global"
        Me.mnuSetWeight.Text = "SET WEIGHT"
        '
        'tsSeparator9
        '
        Me.tsSeparator9.Name = "tsSeparator9"
        Me.tsSeparator9.Size = New System.Drawing.Size(234, 6)
        '
        'mnuShowRoute
        '
        Me.mnuShowRoute.Image = Global.Project.My.Resources.Resources.UI_MoveUp_16x16
        Me.mnuShowRoute.Name = "mnuShowRoute"
        Me.mnuShowRoute.Size = New System.Drawing.Size(237, 22)
        Me.mnuShowRoute.Tag = "Global"
        Me.mnuShowRoute.Text = "SHOW ROUTE"
        '
        'mnuHideRoute
        '
        Me.mnuHideRoute.Image = Global.Project.My.Resources.Resources.UI_MoveDown_16x16
        Me.mnuHideRoute.Name = "mnuHideRoute"
        Me.mnuHideRoute.Size = New System.Drawing.Size(237, 22)
        Me.mnuHideRoute.Tag = "Global"
        Me.mnuHideRoute.Text = "HIDE ROUTE"
        '
        'mnuSetCleanTime
        '
        Me.mnuSetCleanTime.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuSetCleanTime.Name = "mnuSetCleanTime"
        Me.mnuSetCleanTime.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetCleanTime.Tag = "Global"
        Me.mnuSetCleanTime.Text = "SET CLEAN TIME"
        '
        'tsSeparator10
        '
        Me.tsSeparator10.Name = "tsSeparator10"
        Me.tsSeparator10.Size = New System.Drawing.Size(234, 6)
        '
        'mnuSetTarJog
        '
        Me.mnuSetTarJog.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuSetTarJog.Name = "mnuSetTarJog"
        Me.mnuSetTarJog.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetTarJog.Tag = "Global"
        Me.mnuSetTarJog.Text = "SET TARGET JOG"
        '
        'mnuSetActJog
        '
        Me.mnuSetActJog.Image = Global.Project.My.Resources.Resources.UI_GaugeStyleFullCircular_16x16
        Me.mnuSetActJog.Name = "mnuSetActJog"
        Me.mnuSetActJog.Size = New System.Drawing.Size(237, 22)
        Me.mnuSetActJog.Tag = "Global"
        Me.mnuSetActJog.Text = "SET ACTUAL JOG"
        '
        'frmMenu_Motor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Name = "frmMenu_Motor"
        Me.Text = "frmMenu"
        Me.ctxMenuStrip.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ctxMenuStrip As ContextMenuStrip
    Friend WithEvents mnuHeader As ToolStripMenuItem
    Friend WithEvents tsSeparator1 As ToolStripSeparator
    Friend WithEvents mnuMode_Auto As ToolStripMenuItem
    Friend WithEvents mnuMode_Manual As ToolStripMenuItem
    Friend WithEvents tsSeparator2 As ToolStripSeparator
    Friend WithEvents mnuStart As ToolStripMenuItem
    Friend WithEvents mnuStop As ToolStripMenuItem
    Friend WithEvents mnuJog As ToolStripMenuItem
    Friend WithEvents mnuManualLQ As ToolStripMenuItem
    Friend WithEvents mnuResetError As ToolStripMenuItem
    Friend WithEvents mnuUnderRepair As ToolStripMenuItem
    Friend WithEvents tsSeparator3 As ToolStripSeparator
    Friend WithEvents mnuSelect_Auto As ToolStripMenuItem
    Friend WithEvents mnuSelect_Manual As ToolStripMenuItem
    Friend WithEvents mnuStartBatch As ToolStripMenuItem
    Friend WithEvents mnuMonitoring As ToolStripMenuItem
    Friend WithEvents mnuChangeBatchPreset As ToolStripMenuItem
    Friend WithEvents mnuResetBatchQ As ToolStripMenuItem
    Friend WithEvents mnuChangeDestBin As ToolStripMenuItem
    Friend WithEvents mnuParameter As ToolStripMenuItem
    Friend WithEvents mnuSelect_Hold As ToolStripMenuItem
    Friend WithEvents mnuTopenOverflow As ToolStripMenuItem
    Friend WithEvents tsSeparator4 As ToolStripSeparator
    Friend WithEvents mnuPidAuto As ToolStripMenuItem
    Friend WithEvents mnuPidManual As ToolStripMenuItem
    Friend WithEvents mnuPidConfig As ToolStripMenuItem
    Friend WithEvents tsSeparator5 As ToolStripSeparator
    Friend WithEvents mnuResetPulse As ToolStripMenuItem
    Friend WithEvents mnuParameterLQ_HA As ToolStripMenuItem
    Friend WithEvents mnuBinParameter As ToolStripMenuItem
    Friend WithEvents tsSeparator6 As ToolStripSeparator
    Friend WithEvents mnuBucketParameter As ToolStripMenuItem
    Friend WithEvents mnuHammParameter As ToolStripMenuItem
    Friend WithEvents mnuMixerParameter As ToolStripMenuItem
    Friend WithEvents mnuAnalogParameter As ToolStripMenuItem
    Friend WithEvents mnuSetValue As ToolStripMenuItem
    Friend WithEvents tsSeparator7 As ToolStripSeparator
    Friend WithEvents mnuSetDelayTime As ToolStripMenuItem
    Friend WithEvents mnuSetDelayTimeHigh As ToolStripMenuItem
    Friend WithEvents mnuSetDelayTimeLow As ToolStripMenuItem
    Friend WithEvents tsSeparator8 As ToolStripSeparator
    Friend WithEvents mnuSetJogTime As ToolStripMenuItem
    Friend WithEvents mnuSetLowTime As ToolStripMenuItem
    Friend WithEvents mnuSetHightTime As ToolStripMenuItem
    Friend WithEvents mnuSetWeight As ToolStripMenuItem
    Friend WithEvents tsSeparator9 As ToolStripSeparator
    Friend WithEvents mnuShowRoute As ToolStripMenuItem
    Friend WithEvents mnuHideRoute As ToolStripMenuItem
    Friend WithEvents mnuSetCleanTime As ToolStripMenuItem
    Friend WithEvents tsSeparator10 As ToolStripSeparator
    Friend WithEvents mnuSetTarJog As ToolStripMenuItem
    Friend WithEvents mnuSetActJog As ToolStripMenuItem
End Class
