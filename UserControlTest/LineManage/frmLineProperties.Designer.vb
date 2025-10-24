Imports System.Drawing.Drawing2D

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmLineProperties
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
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.lblName = New System.Windows.Forms.Label()
        Me.lblWidth = New System.Windows.Forms.Label()
        Me.lblStyle = New System.Windows.Forms.Label()
        Me.lblColor = New System.Windows.Forms.Label()
        Me.lblCondition = New System.Windows.Forms.Label()
        Me.txtName = New System.Windows.Forms.TextBox()
        Me.nudWidth = New System.Windows.Forms.NumericUpDown()
        Me.cmbStyle = New System.Windows.Forms.ComboBox()
        Me.btnColor = New System.Windows.Forms.Button()
        Me.txtCondition = New System.Windows.Forms.TextBox()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.lblHint1 = New System.Windows.Forms.Label()
        Me.btnAnd = New System.Windows.Forms.Button()
        Me.btnOr = New System.Windows.Forms.Button()
        CType(Me.nudWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Location = New System.Drawing.Point(12, 9)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(38, 13)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Label1"
        '
        'lblName
        '
        Me.lblName.AutoSize = True
        Me.lblName.Location = New System.Drawing.Point(12, 44)
        Me.lblName.Name = "lblName"
        Me.lblName.Size = New System.Drawing.Size(34, 13)
        Me.lblName.TabIndex = 1
        Me.lblName.Text = "Name"
        '
        'lblWidth
        '
        Me.lblWidth.AutoSize = True
        Me.lblWidth.Location = New System.Drawing.Point(12, 72)
        Me.lblWidth.Name = "lblWidth"
        Me.lblWidth.Size = New System.Drawing.Size(35, 13)
        Me.lblWidth.TabIndex = 2
        Me.lblWidth.Text = "Width"
        '
        'lblStyle
        '
        Me.lblStyle.AutoSize = True
        Me.lblStyle.Location = New System.Drawing.Point(12, 102)
        Me.lblStyle.Name = "lblStyle"
        Me.lblStyle.Size = New System.Drawing.Size(31, 13)
        Me.lblStyle.TabIndex = 3
        Me.lblStyle.Text = "Style"
        '
        'lblColor
        '
        Me.lblColor.AutoSize = True
        Me.lblColor.Location = New System.Drawing.Point(12, 132)
        Me.lblColor.Name = "lblColor"
        Me.lblColor.Size = New System.Drawing.Size(32, 13)
        Me.lblColor.TabIndex = 4
        Me.lblColor.Text = "Color"
        '
        'lblCondition
        '
        Me.lblCondition.AutoSize = True
        Me.lblCondition.Location = New System.Drawing.Point(12, 162)
        Me.lblCondition.Name = "lblCondition"
        Me.lblCondition.Size = New System.Drawing.Size(52, 13)
        Me.lblCondition.TabIndex = 5
        Me.lblCondition.Text = "Condition"
        '
        'txtName
        '
        Me.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtName.Location = New System.Drawing.Point(76, 42)
        Me.txtName.Name = "txtName"
        Me.txtName.Size = New System.Drawing.Size(221, 21)
        Me.txtName.TabIndex = 6
        '
        'nudWidth
        '
        Me.nudWidth.Location = New System.Drawing.Point(76, 70)
        Me.nudWidth.Name = "nudWidth"
        Me.nudWidth.Size = New System.Drawing.Size(221, 21)
        Me.nudWidth.TabIndex = 7
        Me.nudWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'cmbStyle
        '
        Me.cmbStyle.FormattingEnabled = True
        Me.cmbStyle.Items.AddRange(New Object() {"Solid", "Dash", "Dot", "DashDot", "DashDotDot", "Custom"})
        Me.cmbStyle.Location = New System.Drawing.Point(76, 99)
        Me.cmbStyle.Name = "cmbStyle"
        Me.cmbStyle.Size = New System.Drawing.Size(221, 21)
        Me.cmbStyle.TabIndex = 8
        '
        'btnColor
        '
        Me.btnColor.Location = New System.Drawing.Point(76, 127)
        Me.btnColor.Name = "btnColor"
        Me.btnColor.Size = New System.Drawing.Size(221, 23)
        Me.btnColor.TabIndex = 9
        Me.btnColor.Text = "Color"
        Me.btnColor.UseVisualStyleBackColor = True
        '
        'txtCondition
        '
        Me.txtCondition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCondition.Location = New System.Drawing.Point(76, 160)
        Me.txtCondition.Multiline = True
        Me.txtCondition.Name = "txtCondition"
        Me.txtCondition.Size = New System.Drawing.Size(221, 69)
        Me.txtCondition.TabIndex = 10
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(76, 301)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(96, 23)
        Me.btnOK.TabIndex = 11
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(197, 301)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(100, 23)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "CANCEL"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(197, 272)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(100, 23)
        Me.btnApply.TabIndex = 13
        Me.btnApply.Text = "APPLY"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'lblHint1
        '
        Me.lblHint1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHint1.ForeColor = System.Drawing.Color.Red
        Me.lblHint1.Location = New System.Drawing.Point(71, 245)
        Me.lblHint1.Name = "lblHint1"
        Me.lblHint1.Size = New System.Drawing.Size(103, 13)
        Me.lblHint1.TabIndex = 14
        Me.lblHint1.Text = "OR : |        AND : &&"
        Me.lblHint1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnAnd
        '
        Me.btnAnd.Location = New System.Drawing.Point(247, 240)
        Me.btnAnd.Name = "btnAnd"
        Me.btnAnd.Size = New System.Drawing.Size(50, 24)
        Me.btnAnd.TabIndex = 15
        Me.btnAnd.Tag = "&"
        Me.btnAnd.Text = "AND"
        Me.btnAnd.UseVisualStyleBackColor = True
        '
        'btnOr
        '
        Me.btnOr.BackColor = System.Drawing.Color.PaleGreen
        Me.btnOr.Location = New System.Drawing.Point(197, 240)
        Me.btnOr.Name = "btnOr"
        Me.btnOr.Size = New System.Drawing.Size(50, 24)
        Me.btnOr.TabIndex = 16
        Me.btnOr.Tag = "|"
        Me.btnOr.Text = "OR"
        Me.btnOr.UseVisualStyleBackColor = False
        '
        'frmLineProperties
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(319, 330)
        Me.Controls.Add(Me.btnOr)
        Me.Controls.Add(Me.btnAnd)
        Me.Controls.Add(Me.lblHint1)
        Me.Controls.Add(Me.btnApply)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.txtCondition)
        Me.Controls.Add(Me.btnColor)
        Me.Controls.Add(Me.cmbStyle)
        Me.Controls.Add(Me.nudWidth)
        Me.Controls.Add(Me.txtName)
        Me.Controls.Add(Me.lblCondition)
        Me.Controls.Add(Me.lblColor)
        Me.Controls.Add(Me.lblStyle)
        Me.Controls.Add(Me.lblWidth)
        Me.Controls.Add(Me.lblName)
        Me.Controls.Add(Me.lblTitle)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Name = "frmLineProperties"
        Me.Text = "Line Properties"
        CType(Me.nudWidth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Label
    Friend WithEvents lblName As Label
    Friend WithEvents lblWidth As Label
    Friend WithEvents lblStyle As Label
    Friend WithEvents lblColor As Label
    Friend WithEvents lblCondition As Label
    Friend WithEvents txtName As TextBox
    Friend WithEvents nudWidth As NumericUpDown
    Friend WithEvents cmbStyle As ComboBox
    Friend WithEvents btnColor As Button
    Friend WithEvents txtCondition As TextBox
    Friend WithEvents btnOK As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnApply As Button
    Friend WithEvents lblHint1 As Label
    Friend WithEvents btnAnd As Button
    Friend WithEvents btnOr As Button
End Class
