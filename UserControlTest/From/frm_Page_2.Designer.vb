<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frm_Page_2
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.Label42 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.pnlLine = New System.Windows.Forms.Panel()
        Me.Label36 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.btnModify = New System.Windows.Forms.Button()
        Me.btnLineProperty = New System.Windows.Forms.Button()
        Me.btnGenerate = New System.Windows.Forms.Button()
        Me.btnDrawLine = New System.Windows.Forms.Button()
        Me.btnHide_Control = New System.Windows.Forms.Button()
        Me.btnLineManage = New System.Windows.Forms.Button()
        Me.pnlTest_IO = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnVerify = New System.Windows.Forms.Button()
        Me.btn_remote = New System.Windows.Forms.Button()
        Me.BW_LOAD = New System.ComponentModel.BackgroundWorker()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.BW_ReadPLC = New System.ComponentModel.BackgroundWorker()
        Me.ContextMenuSTartBatch = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.StartBatchToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.Timer_Run = New System.Windows.Forms.Timer(Me.components)
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Timer_Micro = New System.Windows.Forms.Timer(Me.components)
        Me.Timer_Line = New System.Windows.Forms.Timer(Me.components)
        Me.pnlLine.SuspendLayout()
        Me.pnlTest_IO.SuspendLayout()
        Me.ContextMenuSTartBatch.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label42
        '
        Me.Label42.AutoSize = True
        Me.Label42.BackColor = System.Drawing.Color.Transparent
        Me.Label42.ForeColor = System.Drawing.Color.White
        Me.Label42.Location = New System.Drawing.Point(1207, 477)
        Me.Label42.Name = "Label42"
        Me.Label42.Size = New System.Drawing.Size(14, 13)
        Me.Label42.TabIndex = 10683
        Me.Label42.Text = "A"
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.BackColor = System.Drawing.Color.Transparent
        Me.Label34.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label34.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label34.ForeColor = System.Drawing.Color.Yellow
        Me.Label34.Location = New System.Drawing.Point(414, 434)
        Me.Label34.Name = "Label34"
        Me.Label34.Size = New System.Drawing.Size(68, 13)
        Me.Label34.TabIndex = 21721
        Me.Label34.Text = "MM1FAF01"
        '
        'pnlLine
        '
        Me.pnlLine.BackColor = System.Drawing.Color.Transparent
        Me.pnlLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlLine.Controls.Add(Me.Label36)
        Me.pnlLine.Controls.Add(Me.Label35)
        Me.pnlLine.Controls.Add(Me.Label1)
        Me.pnlLine.Controls.Add(Me.Label33)
        Me.pnlLine.Controls.Add(Me.btnSave)
        Me.pnlLine.Controls.Add(Me.btnCancel)
        Me.pnlLine.Controls.Add(Me.btnApply)
        Me.pnlLine.Controls.Add(Me.btnModify)
        Me.pnlLine.Controls.Add(Me.btnLineProperty)
        Me.pnlLine.Controls.Add(Me.btnGenerate)
        Me.pnlLine.Controls.Add(Me.btnDrawLine)
        Me.pnlLine.Controls.Add(Me.btnHide_Control)
        Me.pnlLine.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlLine.Location = New System.Drawing.Point(1211, 965)
        Me.pnlLine.Name = "pnlLine"
        Me.pnlLine.Size = New System.Drawing.Size(699, 105)
        Me.pnlLine.TabIndex = 27305
        Me.pnlLine.Visible = False
        '
        'Label36
        '
        Me.Label36.AutoSize = True
        Me.Label36.ForeColor = System.Drawing.Color.Yellow
        Me.Label36.Location = New System.Drawing.Point(301, 76)
        Me.Label36.Name = "Label36"
        Me.Label36.Size = New System.Drawing.Size(391, 13)
        Me.Label36.TabIndex = 31
        Me.Label36.Text = "For delete click on the line and key 'delete' button on your keyboard"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.ForeColor = System.Drawing.Color.Yellow
        Me.Label35.Location = New System.Drawing.Point(301, 59)
        Me.Label35.Name = "Label35"
        Me.Label35.Size = New System.Drawing.Size(146, 13)
        Me.Label35.TabIndex = 30
        Me.Label35.Text = "Ctrl + Shift + M = Modify"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.Color.Yellow
        Me.Label1.Location = New System.Drawing.Point(301, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(164, 13)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "Ctrl + Shift + P = Properties"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.ForeColor = System.Drawing.Color.Yellow
        Me.Label33.Location = New System.Drawing.Point(301, 18)
        Me.Label33.Name = "Label33"
        Me.Label33.Size = New System.Drawing.Size(75, 14)
        Me.Label33.TabIndex = 28
        Me.Label33.Text = "SHORT KEY"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(201, 4)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(89, 23)
        Me.btnSave.TabIndex = 27
        Me.btnSave.Text = "SAVE"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(237, 77)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(53, 23)
        Me.btnCancel.TabIndex = 26
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        Me.btnCancel.Visible = False
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(176, 77)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(53, 23)
        Me.btnApply.TabIndex = 25
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        Me.btnApply.Visible = False
        '
        'btnModify
        '
        Me.btnModify.Location = New System.Drawing.Point(176, 52)
        Me.btnModify.Name = "btnModify"
        Me.btnModify.Size = New System.Drawing.Size(114, 23)
        Me.btnModify.TabIndex = 24
        Me.btnModify.Text = "Modify"
        Me.btnModify.UseVisualStyleBackColor = True
        Me.btnModify.Visible = False
        '
        'btnLineProperty
        '
        Me.btnLineProperty.Location = New System.Drawing.Point(176, 28)
        Me.btnLineProperty.Name = "btnLineProperty"
        Me.btnLineProperty.Size = New System.Drawing.Size(114, 23)
        Me.btnLineProperty.TabIndex = 19
        Me.btnLineProperty.Text = "Properties"
        Me.btnLineProperty.UseVisualStyleBackColor = True
        Me.btnLineProperty.Visible = False
        '
        'btnGenerate
        '
        Me.btnGenerate.Location = New System.Drawing.Point(4, 77)
        Me.btnGenerate.Name = "btnGenerate"
        Me.btnGenerate.Size = New System.Drawing.Size(104, 23)
        Me.btnGenerate.TabIndex = 17
        Me.btnGenerate.Text = "GENERATE"
        Me.btnGenerate.UseVisualStyleBackColor = True
        '
        'btnDrawLine
        '
        Me.btnDrawLine.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDrawLine.Location = New System.Drawing.Point(4, 40)
        Me.btnDrawLine.Name = "btnDrawLine"
        Me.btnDrawLine.Size = New System.Drawing.Size(104, 31)
        Me.btnDrawLine.TabIndex = 1
        Me.btnDrawLine.Text = "Draw Mode"
        Me.btnDrawLine.UseVisualStyleBackColor = True
        '
        'btnHide_Control
        '
        Me.btnHide_Control.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnHide_Control.Location = New System.Drawing.Point(4, 4)
        Me.btnHide_Control.Name = "btnHide_Control"
        Me.btnHide_Control.Size = New System.Drawing.Size(158, 28)
        Me.btnHide_Control.TabIndex = 0
        Me.btnHide_Control.Text = "HIDE ALL CONTROLS"
        Me.btnHide_Control.UseVisualStyleBackColor = True
        '
        'btnLineManage
        '
        Me.btnLineManage.Location = New System.Drawing.Point(1642, 934)
        Me.btnLineManage.Name = "btnLineManage"
        Me.btnLineManage.Size = New System.Drawing.Size(265, 23)
        Me.btnLineManage.TabIndex = 27304
        Me.btnLineManage.Text = "OPEN LINE MANAGEMENT MODE"
        Me.btnLineManage.UseVisualStyleBackColor = True
        '
        'pnlTest_IO
        '
        Me.pnlTest_IO.BackColor = System.Drawing.Color.Transparent
        Me.pnlTest_IO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTest_IO.Controls.Add(Me.Label2)
        Me.pnlTest_IO.Controls.Add(Me.btnVerify)
        Me.pnlTest_IO.Location = New System.Drawing.Point(1738, 871)
        Me.pnlTest_IO.Name = "pnlTest_IO"
        Me.pnlTest_IO.Size = New System.Drawing.Size(173, 57)
        Me.pnlTest_IO.TabIndex = 27303
        Me.pnlTest_IO.Visible = False
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.Yellow
        Me.Label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(171, 23)
        Me.Label2.TabIndex = 25449
        Me.Label2.Text = "TEST I/O MODE"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnVerify
        '
        Me.btnVerify.Location = New System.Drawing.Point(3, 26)
        Me.btnVerify.Name = "btnVerify"
        Me.btnVerify.Size = New System.Drawing.Size(165, 23)
        Me.btnVerify.TabIndex = 25448
        Me.btnVerify.Text = "VERIFY"
        Me.btnVerify.UseVisualStyleBackColor = True
        '
        'btn_remote
        '
        Me.btn_remote.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btn_remote.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btn_remote.Location = New System.Drawing.Point(1769, 10)
        Me.btn_remote.Name = "btn_remote"
        Me.btn_remote.Size = New System.Drawing.Size(141, 33)
        Me.btn_remote.TabIndex = 27301
        Me.btn_remote.Text = "REMOTE CONTROL"
        Me.btn_remote.UseVisualStyleBackColor = True
        '
        'BW_ReadPLC
        '
        '
        'ContextMenuSTartBatch
        '
        Me.ContextMenuSTartBatch.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.ContextMenuSTartBatch.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StartBatchToolStripMenuItem, Me.ToolStripSeparator1})
        Me.ContextMenuSTartBatch.Name = "ContextMenuSTartBatch"
        Me.ContextMenuSTartBatch.Size = New System.Drawing.Size(116, 32)
        '
        'StartBatchToolStripMenuItem
        '
        Me.StartBatchToolStripMenuItem.Name = "StartBatchToolStripMenuItem"
        Me.StartBatchToolStripMenuItem.Size = New System.Drawing.Size(115, 22)
        Me.StartBatchToolStripMenuItem.Text = "Validate"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(112, 6)
        '
        'Timer_Run
        '
        Me.Timer_Run.Interval = 500
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'Timer_Micro
        '
        Me.Timer_Micro.Enabled = True
        Me.Timer_Micro.Interval = 3000
        '
        'Timer_Line
        '
        Me.Timer_Line.Enabled = True
        Me.Timer_Line.Interval = 500
        '
        'frm_Page_2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DimGray
        Me.BackgroundImage = Global.Project.My.Resources.Resources.BG_MainScada
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.ClientSize = New System.Drawing.Size(1920, 1080)
        Me.Controls.Add(Me.pnlLine)
        Me.Controls.Add(Me.btnLineManage)
        Me.Controls.Add(Me.pnlTest_IO)
        Me.Controls.Add(Me.btn_remote)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.Name = "frm_Page_2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.pnlLine.ResumeLayout(False)
        Me.pnlLine.PerformLayout()
        Me.pnlTest_IO.ResumeLayout(False)
        Me.ContextMenuSTartBatch.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label42 As Label
    Friend WithEvents Label34 As Label
    Friend WithEvents pnlLine As Panel
    Friend WithEvents Label36 As Label
    Friend WithEvents Label35 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label33 As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnApply As Button
    Friend WithEvents btnModify As Button
    Friend WithEvents btnLineProperty As Button
    Friend WithEvents btnGenerate As Button
    Friend WithEvents btnDrawLine As Button
    Friend WithEvents btnHide_Control As Button
    Friend WithEvents btnLineManage As Button
    Friend WithEvents pnlTest_IO As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents btnVerify As Button
    Public WithEvents btn_remote As Button
    Friend WithEvents BW_LOAD As System.ComponentModel.BackgroundWorker
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents BW_ReadPLC As System.ComponentModel.BackgroundWorker
    Friend WithEvents ContextMenuSTartBatch As ContextMenuStrip
    Friend WithEvents StartBatchToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Public WithEvents Timer_Run As Timer
    Public WithEvents Timer1 As Timer
    Friend WithEvents Timer_Micro As Timer
    Friend WithEvents Timer_Line As Timer
End Class
