<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_production_time
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
        Me.chkShift = New System.Windows.Forms.CheckBox()
        Me.S1 = New System.Windows.Forms.DateTimePicker()
        Me.S1_ = New System.Windows.Forms.DateTimePicker()
        Me.S2 = New System.Windows.Forms.DateTimePicker()
        Me.S2_ = New System.Windows.Forms.DateTimePicker()
        Me.S3 = New System.Windows.Forms.DateTimePicker()
        Me.S3_ = New System.Windows.Forms.DateTimePicker()
        Me.dtpTime = New System.Windows.Forms.DateTimePicker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.pan_Shift = New System.Windows.Forms.Panel()
        Me.rbShift3 = New System.Windows.Forms.RadioButton()
        Me.rbShift2 = New System.Windows.Forms.RadioButton()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.rbShift1 = New System.Windows.Forms.RadioButton()
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.btSave = New System.Windows.Forms.Button()
        Me.btClose = New System.Windows.Forms.Button()
        Me.optAuto = New System.Windows.Forms.RadioButton()
        Me.optManual = New System.Windows.Forms.RadioButton()
        Me.pan_Shift.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkShift
        '
        Me.chkShift.AutoSize = True
        Me.chkShift.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.chkShift.Location = New System.Drawing.Point(304, 8)
        Me.chkShift.Name = "chkShift"
        Me.chkShift.Size = New System.Drawing.Size(57, 20)
        Me.chkShift.TabIndex = 11
        Me.chkShift.Text = "Shift"
        Me.chkShift.UseVisualStyleBackColor = True
        '
        'S1
        '
        Me.S1.Enabled = False
        Me.S1.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.S1.Location = New System.Drawing.Point(35, 26)
        Me.S1.Name = "S1"
        Me.S1.ShowUpDown = True
        Me.S1.Size = New System.Drawing.Size(68, 20)
        Me.S1.TabIndex = 13
        '
        'S1_
        '
        Me.S1_.Enabled = False
        Me.S1_.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.S1_.Location = New System.Drawing.Point(125, 26)
        Me.S1_.Name = "S1_"
        Me.S1_.ShowUpDown = True
        Me.S1_.Size = New System.Drawing.Size(68, 20)
        Me.S1_.TabIndex = 14
        '
        'S2
        '
        Me.S2.Enabled = False
        Me.S2.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.S2.Location = New System.Drawing.Point(35, 52)
        Me.S2.Name = "S2"
        Me.S2.ShowUpDown = True
        Me.S2.Size = New System.Drawing.Size(68, 20)
        Me.S2.TabIndex = 15
        '
        'S2_
        '
        Me.S2_.Enabled = False
        Me.S2_.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.S2_.Location = New System.Drawing.Point(125, 52)
        Me.S2_.Name = "S2_"
        Me.S2_.ShowUpDown = True
        Me.S2_.Size = New System.Drawing.Size(68, 20)
        Me.S2_.TabIndex = 16
        '
        'S3
        '
        Me.S3.Enabled = False
        Me.S3.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.S3.Location = New System.Drawing.Point(35, 78)
        Me.S3.Name = "S3"
        Me.S3.ShowUpDown = True
        Me.S3.Size = New System.Drawing.Size(68, 20)
        Me.S3.TabIndex = 17
        '
        'S3_
        '
        Me.S3_.Enabled = False
        Me.S3_.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.S3_.Location = New System.Drawing.Point(125, 78)
        Me.S3_.Name = "S3_"
        Me.S3_.ShowUpDown = True
        Me.S3_.Size = New System.Drawing.Size(68, 20)
        Me.S3_.TabIndex = 18
        '
        'dtpTime
        '
        Me.dtpTime.Format = System.Windows.Forms.DateTimePickerFormat.Time
        Me.dtpTime.Location = New System.Drawing.Point(134, 33)
        Me.dtpTime.MaxDate = New Date(3998, 12, 31, 0, 0, 0, 0)
        Me.dtpTime.Name = "dtpTime"
        Me.dtpTime.ShowUpDown = True
        Me.dtpTime.Size = New System.Drawing.Size(68, 20)
        Me.dtpTime.TabIndex = 20
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 9)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(122, 16)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "P&roduction Date :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(120, 16)
        Me.Label3.TabIndex = 23
        Me.Label3.Text = "P&roduction Time :"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(109, 30)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(10, 13)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "-"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(109, 57)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(10, 13)
        Me.Label5.TabIndex = 25
        Me.Label5.Text = "-"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(109, 80)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(10, 13)
        Me.Label6.TabIndex = 26
        Me.Label6.Text = "-"
        '
        'pan_Shift
        '
        Me.pan_Shift.Controls.Add(Me.rbShift3)
        Me.pan_Shift.Controls.Add(Me.rbShift2)
        Me.pan_Shift.Controls.Add(Me.Label8)
        Me.pan_Shift.Controls.Add(Me.Label7)
        Me.pan_Shift.Controls.Add(Me.Label1)
        Me.pan_Shift.Controls.Add(Me.rbShift1)
        Me.pan_Shift.Controls.Add(Me.Label6)
        Me.pan_Shift.Controls.Add(Me.S1)
        Me.pan_Shift.Controls.Add(Me.Label5)
        Me.pan_Shift.Controls.Add(Me.S1_)
        Me.pan_Shift.Controls.Add(Me.Label4)
        Me.pan_Shift.Controls.Add(Me.S2)
        Me.pan_Shift.Controls.Add(Me.S2_)
        Me.pan_Shift.Controls.Add(Me.S3)
        Me.pan_Shift.Controls.Add(Me.S3_)
        Me.pan_Shift.Location = New System.Drawing.Point(301, 29)
        Me.pan_Shift.Name = "pan_Shift"
        Me.pan_Shift.Size = New System.Drawing.Size(206, 106)
        Me.pan_Shift.TabIndex = 30
        '
        'rbShift3
        '
        Me.rbShift3.AutoSize = True
        Me.rbShift3.Checked = True
        Me.rbShift3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.rbShift3.Location = New System.Drawing.Point(139, 5)
        Me.rbShift3.Name = "rbShift3"
        Me.rbShift3.Size = New System.Drawing.Size(61, 17)
        Me.rbShift3.TabIndex = 27
        Me.rbShift3.TabStop = True
        Me.rbShift3.Text = "Shift 3"
        Me.rbShift3.UseVisualStyleBackColor = True
        '
        'rbShift2
        '
        Me.rbShift2.AutoSize = True
        Me.rbShift2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.rbShift2.Location = New System.Drawing.Point(72, 5)
        Me.rbShift2.Name = "rbShift2"
        Me.rbShift2.Size = New System.Drawing.Size(61, 17)
        Me.rbShift2.TabIndex = 27
        Me.rbShift2.Text = "Shift 2"
        Me.rbShift2.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label8.Location = New System.Drawing.Point(5, 78)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(29, 16)
        Me.Label8.TabIndex = 23
        Me.Label8.Text = "3 : "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label7.Location = New System.Drawing.Point(5, 55)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(29, 16)
        Me.Label7.TabIndex = 23
        Me.Label7.Text = "2 : "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label1.Location = New System.Drawing.Point(5, 28)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(29, 16)
        Me.Label1.TabIndex = 23
        Me.Label1.Text = "1 : "
        '
        'rbShift1
        '
        Me.rbShift1.AutoSize = True
        Me.rbShift1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.rbShift1.Location = New System.Drawing.Point(5, 5)
        Me.rbShift1.Name = "rbShift1"
        Me.rbShift1.Size = New System.Drawing.Size(61, 17)
        Me.rbShift1.TabIndex = 27
        Me.rbShift1.Text = "Shift 1"
        Me.rbShift1.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpDate.Location = New System.Drawing.Point(134, 8)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(100, 20)
        Me.dtpDate.TabIndex = 31
        '
        'btSave
        '
        Me.btSave.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btSave.Location = New System.Drawing.Point(39, 105)
        Me.btSave.Name = "btSave"
        Me.btSave.Size = New System.Drawing.Size(75, 28)
        Me.btSave.TabIndex = 6
        Me.btSave.Text = "SAVE"
        Me.btSave.UseVisualStyleBackColor = True
        '
        'btClose
        '
        Me.btClose.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btClose.Location = New System.Drawing.Point(188, 105)
        Me.btClose.Name = "btClose"
        Me.btClose.Size = New System.Drawing.Size(75, 28)
        Me.btClose.TabIndex = 7
        Me.btClose.Text = "CLOSE"
        Me.btClose.UseVisualStyleBackColor = True
        '
        'optAuto
        '
        Me.optAuto.AutoSize = True
        Me.optAuto.Checked = True
        Me.optAuto.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.optAuto.Location = New System.Drawing.Point(15, 59)
        Me.optAuto.Name = "optAuto"
        Me.optAuto.Size = New System.Drawing.Size(248, 17)
        Me.optAuto.TabIndex = 32
        Me.optAuto.TabStop = True
        Me.optAuto.Text = "AUTOMATIC CHANGE PRODUCTION DATE"
        Me.optAuto.UseVisualStyleBackColor = True
        '
        'optManual
        '
        Me.optManual.AutoSize = True
        Me.optManual.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.optManual.Location = New System.Drawing.Point(15, 81)
        Me.optManual.Name = "optManual"
        Me.optManual.Size = New System.Drawing.Size(227, 17)
        Me.optManual.TabIndex = 32
        Me.optManual.Text = "MANUAL CHANGE PRODUCTION DATE"
        Me.optManual.UseVisualStyleBackColor = True
        '
        'frm_production_time
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(510, 140)
        Me.ControlBox = False
        Me.Controls.Add(Me.optManual)
        Me.Controls.Add(Me.optAuto)
        Me.Controls.Add(Me.dtpDate)
        Me.Controls.Add(Me.pan_Shift)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.dtpTime)
        Me.Controls.Add(Me.btClose)
        Me.Controls.Add(Me.chkShift)
        Me.Controls.Add(Me.btSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frm_production_time"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Set ProductionTime"
        Me.TopMost = True
        Me.pan_Shift.ResumeLayout(False)
        Me.pan_Shift.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout

End Sub
    Friend WithEvents chkShift As System.Windows.Forms.CheckBox
    Friend WithEvents S1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents S1_ As System.Windows.Forms.DateTimePicker
    Friend WithEvents S2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents S2_ As System.Windows.Forms.DateTimePicker
    Friend WithEvents S3 As System.Windows.Forms.DateTimePicker
    Friend WithEvents S3_ As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents pan_Shift As System.Windows.Forms.Panel
    Friend WithEvents dtpDate As DateTimePicker
    Friend WithEvents btSave As Button
    Friend WithEvents btClose As Button
    Friend WithEvents rbShift3 As RadioButton
    Friend WithEvents rbShift2 As RadioButton
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents rbShift1 As RadioButton
    Friend WithEvents optAuto As RadioButton
    Friend WithEvents optManual As RadioButton
End Class
