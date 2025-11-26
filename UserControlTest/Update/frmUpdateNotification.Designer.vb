<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpdateNotification
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUpdateNotification))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblFileDetails = New System.Windows.Forms.Label()
        Me.btnUpdateLater = New System.Windows.Forms.Button()
        Me.btnUpdateNow = New System.Windows.Forms.Button()
        Me.lblFileDate = New System.Windows.Forms.Label()
        Me.lblFileSize = New System.Windows.Forms.Label()
        Me.lblFileName = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.picIcon = New System.Windows.Forms.PictureBox()
        Me.Panel1.SuspendLayout()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.lblFileDetails)
        Me.Panel1.Controls.Add(Me.btnUpdateLater)
        Me.Panel1.Controls.Add(Me.btnUpdateNow)
        Me.Panel1.Controls.Add(Me.lblFileDate)
        Me.Panel1.Controls.Add(Me.lblFileSize)
        Me.Panel1.Controls.Add(Me.lblFileName)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Location = New System.Drawing.Point(2, 72)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(546, 196)
        Me.Panel1.TabIndex = 0
        '
        'lblFileDetails
        '
        Me.lblFileDetails.AutoSize = True
        Me.lblFileDetails.BackColor = System.Drawing.Color.White
        Me.lblFileDetails.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileDetails.Location = New System.Drawing.Point(37, 62)
        Me.lblFileDetails.Name = "lblFileDetails"
        Me.lblFileDetails.Size = New System.Drawing.Size(59, 17)
        Me.lblFileDetails.TabIndex = 7
        Me.lblFileDetails.Text = "Details: -"
        '
        'btnUpdateLater
        '
        Me.btnUpdateLater.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnUpdateLater.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUpdateLater.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdateLater.Location = New System.Drawing.Point(416, 148)
        Me.btnUpdateLater.Name = "btnUpdateLater"
        Me.btnUpdateLater.Size = New System.Drawing.Size(120, 40)
        Me.btnUpdateLater.TabIndex = 6
        Me.btnUpdateLater.Text = "Later"
        Me.btnUpdateLater.UseVisualStyleBackColor = False
        '
        'btnUpdateNow
        '
        Me.btnUpdateNow.BackColor = System.Drawing.Color.DodgerBlue
        Me.btnUpdateNow.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnUpdateNow.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnUpdateNow.ForeColor = System.Drawing.Color.White
        Me.btnUpdateNow.Location = New System.Drawing.Point(290, 148)
        Me.btnUpdateNow.Name = "btnUpdateNow"
        Me.btnUpdateNow.Size = New System.Drawing.Size(120, 40)
        Me.btnUpdateNow.TabIndex = 5
        Me.btnUpdateNow.Text = "Update Now"
        Me.btnUpdateNow.UseVisualStyleBackColor = False
        '
        'lblFileDate
        '
        Me.lblFileDate.AutoSize = True
        Me.lblFileDate.BackColor = System.Drawing.Color.White
        Me.lblFileDate.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileDate.Location = New System.Drawing.Point(37, 110)
        Me.lblFileDate.Name = "lblFileDate"
        Me.lblFileDate.Size = New System.Drawing.Size(47, 17)
        Me.lblFileDate.TabIndex = 4
        Me.lblFileDate.Text = "Date: -"
        '
        'lblFileSize
        '
        Me.lblFileSize.AutoSize = True
        Me.lblFileSize.BackColor = System.Drawing.Color.White
        Me.lblFileSize.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileSize.Location = New System.Drawing.Point(37, 86)
        Me.lblFileSize.Name = "lblFileSize"
        Me.lblFileSize.Size = New System.Drawing.Size(43, 17)
        Me.lblFileSize.TabIndex = 3
        Me.lblFileSize.Text = "Size: -"
        '
        'lblFileName
        '
        Me.lblFileName.AutoSize = True
        Me.lblFileName.BackColor = System.Drawing.Color.White
        Me.lblFileName.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFileName.Location = New System.Drawing.Point(37, 38)
        Me.lblFileName.Name = "lblFileName"
        Me.lblFileName.Size = New System.Drawing.Size(39, 17)
        Me.lblFileName.TabIndex = 2
        Me.lblFileName.Text = "File: -"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(17, 12)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(316, 17)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "A new update has been detected for this application."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.White
        Me.Label1.Location = New System.Drawing.Point(59, 22)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(454, 25)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "SCADA Control System Has New Update Available"
        '
        'picIcon
        '
        Me.picIcon.Location = New System.Drawing.Point(16, 15)
        Me.picIcon.Name = "picIcon"
        Me.picIcon.Size = New System.Drawing.Size(40, 40)
        Me.picIcon.TabIndex = 4
        Me.picIcon.TabStop = False
        '
        'frmUpdateNotification
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DodgerBlue
        Me.ClientSize = New System.Drawing.Size(550, 270)
        Me.Controls.Add(Me.picIcon)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmUpdateNotification"
        Me.Text = "NEW UPDATE AVAILABLE"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.picIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents lblFileName As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblFileSize As Label
    Friend WithEvents picIcon As PictureBox
    Friend WithEvents btnUpdateNow As Button
    Friend WithEvents lblFileDate As Label
    Friend WithEvents btnUpdateLater As Button
    Friend WithEvents lblFileDetails As Label
End Class
