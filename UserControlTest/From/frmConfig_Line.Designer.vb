<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfig_Line
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtLineCondition = New System.Windows.Forms.RichTextBox()
        Me.lblCountContinue = New System.Windows.Forms.Label()
        Me.lblCountCond = New System.Windows.Forms.Label()
        Me.btnAddObj = New System.Windows.Forms.Button()
        Me.btnCancel_Continue = New System.Windows.Forms.Button()
        Me.btnCancel_Condition = New System.Windows.Forms.Button()
        Me.btnNextAdd = New System.Windows.Forms.Button()
        Me.txtLineContinuous = New System.Windows.Forms.TextBox()
        Me.btnAdd_Or = New System.Windows.Forms.Button()
        Me.btnAdd_And = New System.Windows.Forms.Button()
        Me.txtObj_Name = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.btnCancelConfig = New System.Windows.Forms.Button()
        Me.btnSaveConfig = New System.Windows.Forms.Button()
        Me.lblAppName = New System.Windows.Forms.Label()
        Me.lblFormName = New System.Windows.Forms.Label()
        Me.lblAlarm = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtLineCondition)
        Me.GroupBox1.Controls.Add(Me.lblCountContinue)
        Me.GroupBox1.Controls.Add(Me.lblCountCond)
        Me.GroupBox1.Controls.Add(Me.btnAddObj)
        Me.GroupBox1.Controls.Add(Me.btnCancel_Continue)
        Me.GroupBox1.Controls.Add(Me.btnCancel_Condition)
        Me.GroupBox1.Controls.Add(Me.btnNextAdd)
        Me.GroupBox1.Controls.Add(Me.txtLineContinuous)
        Me.GroupBox1.Controls.Add(Me.btnAdd_Or)
        Me.GroupBox1.Controls.Add(Me.btnAdd_And)
        Me.GroupBox1.Controls.Add(Me.txtObj_Name)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 61)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(359, 482)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Property"
        '
        'txtLineCondition
        '
        Me.txtLineCondition.Location = New System.Drawing.Point(99, 63)
        Me.txtLineCondition.Name = "txtLineCondition"
        Me.txtLineCondition.Size = New System.Drawing.Size(254, 170)
        Me.txtLineCondition.TabIndex = 14
        Me.txtLineCondition.Text = ""
        '
        'lblCountContinue
        '
        Me.lblCountContinue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblCountContinue.ForeColor = System.Drawing.Color.Blue
        Me.lblCountContinue.Location = New System.Drawing.Point(6, 306)
        Me.lblCountContinue.Name = "lblCountContinue"
        Me.lblCountContinue.Size = New System.Drawing.Size(80, 13)
        Me.lblCountContinue.TabIndex = 13
        Me.lblCountContinue.Text = "0"
        Me.lblCountContinue.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCountCond
        '
        Me.lblCountCond.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblCountCond.ForeColor = System.Drawing.Color.Blue
        Me.lblCountCond.Location = New System.Drawing.Point(6, 92)
        Me.lblCountCond.Name = "lblCountCond"
        Me.lblCountCond.Size = New System.Drawing.Size(80, 13)
        Me.lblCountCond.TabIndex = 12
        Me.lblCountCond.Text = "0"
        Me.lblCountCond.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnAddObj
        '
        Me.btnAddObj.Location = New System.Drawing.Point(203, 239)
        Me.btnAddObj.Name = "btnAddObj"
        Me.btnAddObj.Size = New System.Drawing.Size(75, 23)
        Me.btnAddObj.TabIndex = 11
        Me.btnAddObj.Text = "ADD"
        Me.btnAddObj.UseVisualStyleBackColor = True
        '
        'btnCancel_Continue
        '
        Me.btnCancel_Continue.Location = New System.Drawing.Point(278, 453)
        Me.btnCancel_Continue.Name = "btnCancel_Continue"
        Me.btnCancel_Continue.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel_Continue.TabIndex = 10
        Me.btnCancel_Continue.Text = "CANCEL"
        Me.btnCancel_Continue.UseVisualStyleBackColor = True
        '
        'btnCancel_Condition
        '
        Me.btnCancel_Condition.Location = New System.Drawing.Point(278, 240)
        Me.btnCancel_Condition.Name = "btnCancel_Condition"
        Me.btnCancel_Condition.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel_Condition.TabIndex = 9
        Me.btnCancel_Condition.Text = "CANCEL"
        Me.btnCancel_Condition.UseVisualStyleBackColor = True
        Me.btnCancel_Condition.Visible = False
        '
        'btnNextAdd
        '
        Me.btnNextAdd.Location = New System.Drawing.Point(99, 453)
        Me.btnNextAdd.Name = "btnNextAdd"
        Me.btnNextAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnNextAdd.TabIndex = 8
        Me.btnNextAdd.Text = "ADD"
        Me.btnNextAdd.UseVisualStyleBackColor = True
        '
        'txtLineContinuous
        '
        Me.txtLineContinuous.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtLineContinuous.Location = New System.Drawing.Point(99, 278)
        Me.txtLineContinuous.Multiline = True
        Me.txtLineContinuous.Name = "txtLineContinuous"
        Me.txtLineContinuous.Size = New System.Drawing.Size(254, 169)
        Me.txtLineContinuous.TabIndex = 7
        '
        'btnAdd_Or
        '
        Me.btnAdd_Or.Location = New System.Drawing.Point(151, 239)
        Me.btnAdd_Or.Name = "btnAdd_Or"
        Me.btnAdd_Or.Size = New System.Drawing.Size(46, 23)
        Me.btnAdd_Or.TabIndex = 6
        Me.btnAdd_Or.Text = "OR"
        Me.btnAdd_Or.UseVisualStyleBackColor = True
        Me.btnAdd_Or.Visible = False
        '
        'btnAdd_And
        '
        Me.btnAdd_And.Location = New System.Drawing.Point(99, 239)
        Me.btnAdd_And.Name = "btnAdd_And"
        Me.btnAdd_And.Size = New System.Drawing.Size(46, 23)
        Me.btnAdd_And.TabIndex = 5
        Me.btnAdd_And.Text = "AND"
        Me.btnAdd_And.UseVisualStyleBackColor = True
        Me.btnAdd_And.Visible = False
        '
        'txtObj_Name
        '
        Me.txtObj_Name.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtObj_Name.Location = New System.Drawing.Point(99, 25)
        Me.txtObj_Name.Name = "txtObj_Name"
        Me.txtObj_Name.ReadOnly = True
        Me.txtObj_Name.Size = New System.Drawing.Size(254, 21)
        Me.txtObj_Name.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label3.Location = New System.Drawing.Point(17, 278)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Continuous"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label2.Location = New System.Drawing.Point(27, 63)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(59, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Condition"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.Label1.Location = New System.Drawing.Point(45, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name"
        '
        'btnCancelConfig
        '
        Me.btnCancelConfig.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btnCancelConfig.Location = New System.Drawing.Point(290, 548)
        Me.btnCancelConfig.Name = "btnCancelConfig"
        Me.btnCancelConfig.Size = New System.Drawing.Size(75, 23)
        Me.btnCancelConfig.TabIndex = 11
        Me.btnCancelConfig.Text = "CANCEL"
        Me.btnCancelConfig.UseVisualStyleBackColor = True
        '
        'btnSaveConfig
        '
        Me.btnSaveConfig.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.btnSaveConfig.Location = New System.Drawing.Point(209, 548)
        Me.btnSaveConfig.Name = "btnSaveConfig"
        Me.btnSaveConfig.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveConfig.TabIndex = 12
        Me.btnSaveConfig.Text = "SAVE"
        Me.btnSaveConfig.UseVisualStyleBackColor = True
        '
        'lblAppName
        '
        Me.lblAppName.AutoSize = True
        Me.lblAppName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblAppName.Location = New System.Drawing.Point(12, 9)
        Me.lblAppName.Name = "lblAppName"
        Me.lblAppName.Size = New System.Drawing.Size(94, 16)
        Me.lblAppName.TabIndex = 13
        Me.lblAppName.Text = "lblAppName"
        '
        'lblFormName
        '
        Me.lblFormName.AutoSize = True
        Me.lblFormName.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblFormName.Location = New System.Drawing.Point(251, 9)
        Me.lblFormName.Name = "lblFormName"
        Me.lblFormName.Size = New System.Drawing.Size(101, 16)
        Me.lblFormName.TabIndex = 14
        Me.lblFormName.Text = "lblFormName"
        '
        'lblAlarm
        '
        Me.lblAlarm.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.lblAlarm.ForeColor = System.Drawing.Color.Red
        Me.lblAlarm.Location = New System.Drawing.Point(15, 42)
        Me.lblAlarm.Name = "lblAlarm"
        Me.lblAlarm.Size = New System.Drawing.Size(356, 16)
        Me.lblAlarm.TabIndex = 15
        Me.lblAlarm.Text = "-"
        Me.lblAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmConfig_Line
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(380, 576)
        Me.Controls.Add(Me.lblAlarm)
        Me.Controls.Add(Me.lblFormName)
        Me.Controls.Add(Me.lblAppName)
        Me.Controls.Add(Me.btnSaveConfig)
        Me.Controls.Add(Me.btnCancelConfig)
        Me.Controls.Add(Me.GroupBox1)
        Me.KeyPreview = True
        Me.Name = "frmConfig_Line"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Config Line"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtObj_Name As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents btnCancel_Continue As Button
    Friend WithEvents btnCancel_Condition As Button
    Friend WithEvents btnNextAdd As Button
    Friend WithEvents txtLineContinuous As TextBox
    Friend WithEvents btnAdd_Or As Button
    Friend WithEvents btnAdd_And As Button
    Friend WithEvents btnCancelConfig As Button
    Friend WithEvents btnSaveConfig As Button
    Friend WithEvents lblAppName As Label
    Friend WithEvents lblFormName As Label
    Friend WithEvents btnAddObj As Button
    Friend WithEvents lblCountContinue As Label
    Friend WithEvents lblCountCond As Label
    Friend WithEvents lblAlarm As Label
    Friend WithEvents txtLineCondition As RichTextBox
End Class
