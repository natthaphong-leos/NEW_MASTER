<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmAlarm_Description
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAlarm_Description))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.pnlFilter = New System.Windows.Forms.Panel()
        Me.txtFilter = New System.Windows.Forms.TextBox()
        Me.lblCaption = New System.Windows.Forms.Label()
        Me.lblAppName = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.btnAcknow = New System.Windows.Forms.Button()
        Me.btnHide = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.dgvAlarm = New System.Windows.Forms.DataGridView()
        Me.cMotor_Name = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cDescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.cTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.strType = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ctrlName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.lblLang = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        Me.pnlFilter.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel3.SuspendLayout()
        CType(Me.dgvAlarm, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.lblLang)
        Me.Panel1.Controls.Add(Me.pnlFilter)
        Me.Panel1.Controls.Add(Me.lblAppName)
        Me.Panel1.Controls.Add(Me.Panel2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(839, 90)
        Me.Panel1.TabIndex = 0
        '
        'pnlFilter
        '
        Me.pnlFilter.Controls.Add(Me.txtFilter)
        Me.pnlFilter.Controls.Add(Me.lblCaption)
        Me.pnlFilter.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlFilter.Location = New System.Drawing.Point(117, 50)
        Me.pnlFilter.Name = "pnlFilter"
        Me.pnlFilter.Size = New System.Drawing.Size(722, 40)
        Me.pnlFilter.TabIndex = 2
        '
        'txtFilter
        '
        Me.txtFilter.BackColor = System.Drawing.Color.Black
        Me.txtFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtFilter.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.txtFilter.ForeColor = System.Drawing.Color.White
        Me.txtFilter.Location = New System.Drawing.Point(56, 9)
        Me.txtFilter.Name = "txtFilter"
        Me.txtFilter.Size = New System.Drawing.Size(654, 23)
        Me.txtFilter.TabIndex = 1
        Me.txtFilter.Text = "filter"
        '
        'lblCaption
        '
        Me.lblCaption.AutoSize = True
        Me.lblCaption.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblCaption.ForeColor = System.Drawing.Color.Lime
        Me.lblCaption.Location = New System.Drawing.Point(6, 11)
        Me.lblCaption.Name = "lblCaption"
        Me.lblCaption.Size = New System.Drawing.Size(53, 16)
        Me.lblCaption.TabIndex = 0
        Me.lblCaption.Text = "Filter : "
        '
        'lblAppName
        '
        Me.lblAppName.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblAppName.Font = New System.Drawing.Font("Tahoma", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblAppName.ForeColor = System.Drawing.Color.Red
        Me.lblAppName.Location = New System.Drawing.Point(117, 0)
        Me.lblAppName.Name = "lblAppName"
        Me.lblAppName.Size = New System.Drawing.Size(722, 50)
        Me.lblAppName.TabIndex = 1
        Me.lblAppName.Text = "APP NAME"
        Me.lblAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.PictureBox1)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(117, 90)
        Me.Panel2.TabIndex = 1
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = Global.Project.My.Resources.Resources.SYS_WarningTriangle
        Me.PictureBox1.Location = New System.Drawing.Point(9, 6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(97, 77)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.btnAcknow)
        Me.Panel3.Controls.Add(Me.btnHide)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel3.Location = New System.Drawing.Point(0, 480)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(839, 71)
        Me.Panel3.TabIndex = 1
        '
        'btnAcknow
        '
        Me.btnAcknow.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnAcknow.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btnAcknow.ForeColor = System.Drawing.Color.Lime
        Me.btnAcknow.Location = New System.Drawing.Point(565, 12)
        Me.btnAcknow.Name = "btnAcknow"
        Me.btnAcknow.Size = New System.Drawing.Size(128, 47)
        Me.btnAcknow.TabIndex = 7
        Me.btnAcknow.Text = "ACKNOW"
        Me.btnAcknow.UseVisualStyleBackColor = True
        '
        'btnHide
        '
        Me.btnHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnHide.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btnHide.ForeColor = System.Drawing.Color.Yellow
        Me.btnHide.Location = New System.Drawing.Point(699, 12)
        Me.btnHide.Name = "btnHide"
        Me.btnHide.Size = New System.Drawing.Size(128, 47)
        Me.btnHide.TabIndex = 6
        Me.btnHide.Text = "HIDE"
        Me.btnHide.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "ALARM"
        Me.NotifyIcon1.Visible = True
        '
        'dgvAlarm
        '
        Me.dgvAlarm.AllowUserToAddRows = False
        Me.dgvAlarm.AllowUserToDeleteRows = False
        Me.dgvAlarm.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.dgvAlarm.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvAlarm.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvAlarm.BackgroundColor = System.Drawing.Color.Black
        Me.dgvAlarm.BorderStyle = System.Windows.Forms.BorderStyle.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvAlarm.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvAlarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAlarm.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.cMotor_Name, Me.cStatus, Me.cDescription, Me.cTime, Me.strType, Me.ctrlName})
        Me.dgvAlarm.Location = New System.Drawing.Point(12, 96)
        Me.dgvAlarm.Name = "dgvAlarm"
        Me.dgvAlarm.ReadOnly = True
        Me.dgvAlarm.RowHeadersVisible = False
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.dgvAlarm.RowsDefaultCellStyle = DataGridViewCellStyle8
        Me.dgvAlarm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvAlarm.Size = New System.Drawing.Size(815, 381)
        Me.dgvAlarm.TabIndex = 2
        '
        'cMotor_Name
        '
        Me.cMotor_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.cMotor_Name.DataPropertyName = "cMotor_Name"
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.Black
        Me.cMotor_Name.DefaultCellStyle = DataGridViewCellStyle3
        Me.cMotor_Name.FillWeight = 20.0!
        Me.cMotor_Name.HeaderText = "NAME"
        Me.cMotor_Name.Name = "cMotor_Name"
        Me.cMotor_Name.ReadOnly = True
        '
        'cStatus
        '
        Me.cStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.cStatus.DataPropertyName = "cStatus"
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.Black
        Me.cStatus.DefaultCellStyle = DataGridViewCellStyle4
        Me.cStatus.FillWeight = 10.0!
        Me.cStatus.HeaderText = "STATUS"
        Me.cStatus.Name = "cStatus"
        Me.cStatus.ReadOnly = True
        '
        'cDescription
        '
        Me.cDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.cDescription.DataPropertyName = "cDescription"
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.Black
        Me.cDescription.DefaultCellStyle = DataGridViewCellStyle5
        Me.cDescription.FillWeight = 50.0!
        Me.cDescription.HeaderText = "DETAIL"
        Me.cDescription.Name = "cDescription"
        Me.cDescription.ReadOnly = True
        '
        'cTime
        '
        Me.cTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.cTime.DataPropertyName = "cTime"
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.Black
        DataGridViewCellStyle6.Format = "G"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.cTime.DefaultCellStyle = DataGridViewCellStyle6
        Me.cTime.FillWeight = 20.0!
        Me.cTime.HeaderText = "EVENT TIME"
        Me.cTime.Name = "cTime"
        Me.cTime.ReadOnly = True
        '
        'strType
        '
        Me.strType.DataPropertyName = "strType"
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.Black
        Me.strType.DefaultCellStyle = DataGridViewCellStyle7
        Me.strType.HeaderText = "TYPE"
        Me.strType.Name = "strType"
        Me.strType.ReadOnly = True
        Me.strType.Visible = False
        Me.strType.Width = 56
        '
        'ctrlName
        '
        Me.ctrlName.DataPropertyName = "ctrlName"
        Me.ctrlName.HeaderText = "CTRL_NAME"
        Me.ctrlName.Name = "ctrlName"
        Me.ctrlName.ReadOnly = True
        Me.ctrlName.Visible = False
        Me.ctrlName.Width = 91
        '
        'lblLang
        '
        Me.lblLang.Location = New System.Drawing.Point(798, 6)
        Me.lblLang.Name = "lblLang"
        Me.lblLang.Size = New System.Drawing.Size(39, 13)
        Me.lblLang.TabIndex = 3
        Me.lblLang.Text = "-"
        Me.lblLang.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmAlarm_Description
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(839, 551)
        Me.ControlBox = False
        Me.Controls.Add(Me.dgvAlarm)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.ForeColor = System.Drawing.Color.White
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAlarm_Description"
        Me.Text = "ALARM DESCRIPTION"
        Me.Panel1.ResumeLayout(False)
        Me.pnlFilter.ResumeLayout(False)
        Me.pnlFilter.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel3.ResumeLayout(False)
        CType(Me.dgvAlarm, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Panel2 As Panel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents pnlFilter As Panel
    Friend WithEvents txtFilter As TextBox
    Friend WithEvents lblCaption As Label
    Friend WithEvents lblAppName As Label
    Friend WithEvents Panel3 As Panel
    Friend WithEvents btnHide As Button
    Friend WithEvents btnAcknow As Button
    Friend WithEvents Timer1 As Timer
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents dgvAlarm As DataGridView
    Friend WithEvents cMotor_Name As DataGridViewTextBoxColumn
    Friend WithEvents cStatus As DataGridViewTextBoxColumn
    Friend WithEvents cDescription As DataGridViewTextBoxColumn
    Friend WithEvents cTime As DataGridViewTextBoxColumn
    Friend WithEvents strType As DataGridViewTextBoxColumn
    Friend WithEvents ctrlName As DataGridViewTextBoxColumn
    Friend WithEvents lblLang As Label
End Class
