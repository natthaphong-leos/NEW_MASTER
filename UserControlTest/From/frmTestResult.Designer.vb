<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTestResult
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmTestResult))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblFrmName = New System.Windows.Forms.Label()
        Me.lblAppName = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.lblPercent = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblQty = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tpMotor = New System.Windows.Forms.TabPage()
        Me.lblMotor_Percent = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.lblMotor_Qty = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dgvMotor = New System.Windows.Forms.DataGridView()
        Me.clmObj_Type = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmMcode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmPLC = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmLocation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmUser = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.clmComment = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.tpSlide = New System.Windows.Forms.TabPage()
        Me.lblSlide_Percent = New System.Windows.Forms.Label()
        Me.dgvSlide = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn8 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.lblSlide_Qty = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.tpFlap = New System.Windows.Forms.TabPage()
        Me.lblWayer_Percent = New System.Windows.Forms.Label()
        Me.dgvFlap = New System.Windows.Forms.DataGridView()
        Me.DataGridViewTextBoxColumn9 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn10 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn11 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn12 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn13 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn14 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn15 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DataGridViewTextBoxColumn16 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblWayer_Qty = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblM_PLC = New System.Windows.Forms.Label()
        Me.lblM_User = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.lblM_Obj = New System.Windows.Forms.Label()
        Me.lblM_Index = New System.Windows.Forms.Label()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.lblM_Status = New System.Windows.Forms.Label()
        Me.lblM_Code = New System.Windows.Forms.Label()
        Me.lblM_Name = New System.Windows.Forms.Label()
        Me.txtM_Comment = New System.Windows.Forms.TextBox()
        Me.ctxMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsMenu_Reset = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.tpMotor.SuspendLayout()
        CType(Me.dgvMotor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpSlide.SuspendLayout()
        CType(Me.dgvSlide, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpFlap.SuspendLayout()
        CType(Me.dgvFlap, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.ctxMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblFrmName)
        Me.GroupBox1.Controls.Add(Me.lblAppName)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.lblPercent)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.lblQty)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 9)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(972, 92)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "SUMMARY TEST"
        '
        'lblFrmName
        '
        Me.lblFrmName.AutoSize = True
        Me.lblFrmName.ForeColor = System.Drawing.Color.DarkMagenta
        Me.lblFrmName.Location = New System.Drawing.Point(117, 43)
        Me.lblFrmName.Name = "lblFrmName"
        Me.lblFrmName.Size = New System.Drawing.Size(14, 16)
        Me.lblFrmName.TabIndex = 7
        Me.lblFrmName.Text = "-"
        '
        'lblAppName
        '
        Me.lblAppName.AutoSize = True
        Me.lblAppName.ForeColor = System.Drawing.Color.DarkViolet
        Me.lblAppName.Location = New System.Drawing.Point(117, 19)
        Me.lblAppName.Name = "lblAppName"
        Me.lblAppName.Size = New System.Drawing.Size(14, 16)
        Me.lblAppName.TabIndex = 6
        Me.lblAppName.Text = "-"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 43)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 16)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "FORM NAME : "
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 19)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(87, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "APP NAME : "
        '
        'lblPercent
        '
        Me.lblPercent.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPercent.AutoSize = True
        Me.lblPercent.Font = New System.Drawing.Font("Tahoma", 26.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPercent.ForeColor = System.Drawing.Color.Blue
        Me.lblPercent.Location = New System.Drawing.Point(775, 19)
        Me.lblPercent.Name = "lblPercent"
        Me.lblPercent.Size = New System.Drawing.Size(191, 42)
        Me.lblPercent.TabIndex = 3
        Me.lblPercent.Text = "100.00 %"
        Me.lblPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(638, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(131, 16)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "TOTAL PROGRESS : "
        '
        'lblQty
        '
        Me.lblQty.ForeColor = System.Drawing.Color.Blue
        Me.lblQty.Location = New System.Drawing.Point(131, 67)
        Me.lblQty.Name = "lblQty"
        Me.lblQty.Size = New System.Drawing.Size(162, 16)
        Me.lblQty.TabIndex = 1
        Me.lblQty.Text = "0 / 0 รายการ"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "TOTAL OBJECT : "
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpMotor)
        Me.TabControl1.Controls.Add(Me.tpSlide)
        Me.TabControl1.Controls.Add(Me.tpFlap)
        Me.TabControl1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(12, 107)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(972, 536)
        Me.TabControl1.TabIndex = 1
        '
        'tpMotor
        '
        Me.tpMotor.Controls.Add(Me.lblMotor_Percent)
        Me.tpMotor.Controls.Add(Me.Label8)
        Me.tpMotor.Controls.Add(Me.lblMotor_Qty)
        Me.tpMotor.Controls.Add(Me.Label6)
        Me.tpMotor.Controls.Add(Me.dgvMotor)
        Me.tpMotor.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpMotor.Location = New System.Drawing.Point(4, 25)
        Me.tpMotor.Name = "tpMotor"
        Me.tpMotor.Padding = New System.Windows.Forms.Padding(3)
        Me.tpMotor.Size = New System.Drawing.Size(964, 507)
        Me.tpMotor.TabIndex = 0
        Me.tpMotor.Text = "MOTOR"
        Me.tpMotor.UseVisualStyleBackColor = True
        '
        'lblMotor_Percent
        '
        Me.lblMotor_Percent.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.lblMotor_Percent.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMotor_Percent.ForeColor = System.Drawing.Color.Blue
        Me.lblMotor_Percent.Location = New System.Drawing.Point(825, 475)
        Me.lblMotor_Percent.Name = "lblMotor_Percent"
        Me.lblMotor_Percent.Size = New System.Drawing.Size(133, 27)
        Me.lblMotor_Percent.TabIndex = 5
        Me.lblMotor_Percent.Text = "0 %"
        Me.lblMotor_Percent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(822, 459)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(136, 16)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "MOTOR PROGRESS : "
        '
        'lblMotor_Qty
        '
        Me.lblMotor_Qty.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.lblMotor_Qty.ForeColor = System.Drawing.Color.Blue
        Me.lblMotor_Qty.Location = New System.Drawing.Point(122, 459)
        Me.lblMotor_Qty.Name = "lblMotor_Qty"
        Me.lblMotor_Qty.Size = New System.Drawing.Size(162, 16)
        Me.lblMotor_Qty.TabIndex = 3
        Me.lblMotor_Qty.Text = "0 / 0 รายการ"
        '
        'Label6
        '
        Me.Label6.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(6, 459)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(110, 16)
        Me.Label6.TabIndex = 2
        Me.Label6.Text = "TOTAL MOTOR : "
        '
        'dgvMotor
        '
        Me.dgvMotor.AllowUserToAddRows = False
        Me.dgvMotor.AllowUserToDeleteRows = False
        Me.dgvMotor.AllowUserToResizeRows = False
        Me.dgvMotor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvMotor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvMotor.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.clmObj_Type, Me.clmIndex, Me.clmMcode, Me.clmName, Me.clmPLC, Me.clmLocation, Me.clmDate, Me.clmStatus, Me.clmUser, Me.clmComment})
        Me.dgvMotor.Location = New System.Drawing.Point(6, 8)
        Me.dgvMotor.MultiSelect = False
        Me.dgvMotor.Name = "dgvMotor"
        Me.dgvMotor.ReadOnly = True
        Me.dgvMotor.RowHeadersVisible = False
        Me.dgvMotor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvMotor.Size = New System.Drawing.Size(952, 444)
        Me.dgvMotor.TabIndex = 0
        '
        'clmObj_Type
        '
        Me.clmObj_Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.clmObj_Type.DataPropertyName = "c_type"
        Me.clmObj_Type.FillWeight = 10.0!
        Me.clmObj_Type.HeaderText = "TYPE"
        Me.clmObj_Type.Name = "clmObj_Type"
        Me.clmObj_Type.ReadOnly = True
        '
        'clmIndex
        '
        Me.clmIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.clmIndex.DataPropertyName = "n_index"
        Me.clmIndex.FillWeight = 10.0!
        Me.clmIndex.HeaderText = "INDEX"
        Me.clmIndex.Name = "clmIndex"
        Me.clmIndex.ReadOnly = True
        '
        'clmMcode
        '
        Me.clmMcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.clmMcode.DataPropertyName = "motor_code"
        Me.clmMcode.FillWeight = 18.0!
        Me.clmMcode.HeaderText = "M.CODE"
        Me.clmMcode.Name = "clmMcode"
        Me.clmMcode.ReadOnly = True
        '
        'clmName
        '
        Me.clmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.clmName.DataPropertyName = "c_obj_name"
        Me.clmName.FillWeight = 10.0!
        Me.clmName.HeaderText = "NAME"
        Me.clmName.Name = "clmName"
        Me.clmName.ReadOnly = True
        '
        'clmPLC
        '
        Me.clmPLC.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.clmPLC.DataPropertyName = "n_plc_station"
        Me.clmPLC.FillWeight = 10.0!
        Me.clmPLC.HeaderText = "PLC"
        Me.clmPLC.Name = "clmPLC"
        Me.clmPLC.ReadOnly = True
        '
        'clmLocation
        '
        Me.clmLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.clmLocation.DataPropertyName = "c_location"
        Me.clmLocation.FillWeight = 15.0!
        Me.clmLocation.HeaderText = "LOCATION"
        Me.clmLocation.Name = "clmLocation"
        Me.clmLocation.ReadOnly = True
        '
        'clmDate
        '
        Me.clmDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.clmDate.DataPropertyName = "dt_test_date"
        DataGridViewCellStyle4.Format = "g"
        DataGridViewCellStyle4.NullValue = Nothing
        Me.clmDate.DefaultCellStyle = DataGridViewCellStyle4
        Me.clmDate.FillWeight = 17.0!
        Me.clmDate.HeaderText = "TEST DATE"
        Me.clmDate.Name = "clmDate"
        Me.clmDate.ReadOnly = True
        '
        'clmStatus
        '
        Me.clmStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.clmStatus.DataPropertyName = "f_complete_test"
        Me.clmStatus.FillWeight = 10.0!
        Me.clmStatus.HeaderText = "TEST STATUS"
        Me.clmStatus.Name = "clmStatus"
        Me.clmStatus.ReadOnly = True
        '
        'clmUser
        '
        Me.clmUser.DataPropertyName = "c_user"
        Me.clmUser.HeaderText = "USER"
        Me.clmUser.Name = "clmUser"
        Me.clmUser.ReadOnly = True
        Me.clmUser.Visible = False
        '
        'clmComment
        '
        Me.clmComment.DataPropertyName = "c_comment"
        Me.clmComment.HeaderText = "COMMENT"
        Me.clmComment.Name = "clmComment"
        Me.clmComment.ReadOnly = True
        Me.clmComment.Visible = False
        '
        'tpSlide
        '
        Me.tpSlide.Controls.Add(Me.lblSlide_Percent)
        Me.tpSlide.Controls.Add(Me.dgvSlide)
        Me.tpSlide.Controls.Add(Me.Label10)
        Me.tpSlide.Controls.Add(Me.lblSlide_Qty)
        Me.tpSlide.Controls.Add(Me.Label12)
        Me.tpSlide.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpSlide.Location = New System.Drawing.Point(4, 25)
        Me.tpSlide.Name = "tpSlide"
        Me.tpSlide.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSlide.Size = New System.Drawing.Size(964, 507)
        Me.tpSlide.TabIndex = 1
        Me.tpSlide.Text = "SLIDE"
        Me.tpSlide.UseVisualStyleBackColor = True
        '
        'lblSlide_Percent
        '
        Me.lblSlide_Percent.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.lblSlide_Percent.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSlide_Percent.ForeColor = System.Drawing.Color.Blue
        Me.lblSlide_Percent.Location = New System.Drawing.Point(834, 476)
        Me.lblSlide_Percent.Name = "lblSlide_Percent"
        Me.lblSlide_Percent.Size = New System.Drawing.Size(124, 27)
        Me.lblSlide_Percent.TabIndex = 12
        Me.lblSlide_Percent.Text = "0 %"
        Me.lblSlide_Percent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvSlide
        '
        Me.dgvSlide.AllowUserToAddRows = False
        Me.dgvSlide.AllowUserToDeleteRows = False
        Me.dgvSlide.AllowUserToResizeRows = False
        Me.dgvSlide.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvSlide.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSlide.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.DataGridViewTextBoxColumn4, Me.DataGridViewTextBoxColumn5, Me.DataGridViewTextBoxColumn6, Me.DataGridViewTextBoxColumn7, Me.DataGridViewTextBoxColumn8, Me.Column1, Me.Column2})
        Me.dgvSlide.Location = New System.Drawing.Point(6, 8)
        Me.dgvSlide.MultiSelect = False
        Me.dgvSlide.Name = "dgvSlide"
        Me.dgvSlide.ReadOnly = True
        Me.dgvSlide.RowHeadersVisible = False
        Me.dgvSlide.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSlide.Size = New System.Drawing.Size(952, 444)
        Me.dgvSlide.TabIndex = 11
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn1.DataPropertyName = "c_type"
        Me.DataGridViewTextBoxColumn1.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn1.HeaderText = "TYPE"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        Me.DataGridViewTextBoxColumn1.ReadOnly = True
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn2.DataPropertyName = "n_index"
        Me.DataGridViewTextBoxColumn2.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn2.HeaderText = "INDEX"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        Me.DataGridViewTextBoxColumn2.ReadOnly = True
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn3.DataPropertyName = "motor_code"
        Me.DataGridViewTextBoxColumn3.FillWeight = 18.0!
        Me.DataGridViewTextBoxColumn3.HeaderText = "M.CODE"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        Me.DataGridViewTextBoxColumn3.ReadOnly = True
        '
        'DataGridViewTextBoxColumn4
        '
        Me.DataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn4.DataPropertyName = "c_obj_name"
        Me.DataGridViewTextBoxColumn4.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn4.HeaderText = "NAME"
        Me.DataGridViewTextBoxColumn4.Name = "DataGridViewTextBoxColumn4"
        Me.DataGridViewTextBoxColumn4.ReadOnly = True
        '
        'DataGridViewTextBoxColumn5
        '
        Me.DataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn5.DataPropertyName = "n_plc_station"
        Me.DataGridViewTextBoxColumn5.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn5.HeaderText = "PLC"
        Me.DataGridViewTextBoxColumn5.Name = "DataGridViewTextBoxColumn5"
        Me.DataGridViewTextBoxColumn5.ReadOnly = True
        '
        'DataGridViewTextBoxColumn6
        '
        Me.DataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn6.DataPropertyName = "c_location"
        Me.DataGridViewTextBoxColumn6.FillWeight = 15.0!
        Me.DataGridViewTextBoxColumn6.HeaderText = "LOCATION"
        Me.DataGridViewTextBoxColumn6.Name = "DataGridViewTextBoxColumn6"
        Me.DataGridViewTextBoxColumn6.ReadOnly = True
        '
        'DataGridViewTextBoxColumn7
        '
        Me.DataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn7.DataPropertyName = "dt_test_date"
        DataGridViewCellStyle5.Format = "g"
        DataGridViewCellStyle5.NullValue = Nothing
        Me.DataGridViewTextBoxColumn7.DefaultCellStyle = DataGridViewCellStyle5
        Me.DataGridViewTextBoxColumn7.FillWeight = 17.0!
        Me.DataGridViewTextBoxColumn7.HeaderText = "TEST DATE"
        Me.DataGridViewTextBoxColumn7.Name = "DataGridViewTextBoxColumn7"
        Me.DataGridViewTextBoxColumn7.ReadOnly = True
        '
        'DataGridViewTextBoxColumn8
        '
        Me.DataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn8.DataPropertyName = "f_complete_test"
        Me.DataGridViewTextBoxColumn8.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn8.HeaderText = "TEST STATUS"
        Me.DataGridViewTextBoxColumn8.Name = "DataGridViewTextBoxColumn8"
        Me.DataGridViewTextBoxColumn8.ReadOnly = True
        '
        'Column1
        '
        Me.Column1.DataPropertyName = "c_user"
        Me.Column1.HeaderText = "USER"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Visible = False
        '
        'Column2
        '
        Me.Column2.DataPropertyName = "c_comment"
        Me.Column2.HeaderText = "COMMENT"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Visible = False
        '
        'Label10
        '
        Me.Label10.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(831, 459)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(127, 16)
        Me.Label10.TabIndex = 9
        Me.Label10.Text = "SLIDE PROGRESS : "
        '
        'lblSlide_Qty
        '
        Me.lblSlide_Qty.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.lblSlide_Qty.ForeColor = System.Drawing.Color.Blue
        Me.lblSlide_Qty.Location = New System.Drawing.Point(122, 459)
        Me.lblSlide_Qty.Name = "lblSlide_Qty"
        Me.lblSlide_Qty.Size = New System.Drawing.Size(162, 16)
        Me.lblSlide_Qty.TabIndex = 8
        Me.lblSlide_Qty.Text = "0 / 0 รายการ"
        '
        'Label12
        '
        Me.Label12.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(6, 459)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(110, 16)
        Me.Label12.TabIndex = 7
        Me.Label12.Text = "TOTAL MOTOR : "
        '
        'tpFlap
        '
        Me.tpFlap.Controls.Add(Me.lblWayer_Percent)
        Me.tpFlap.Controls.Add(Me.dgvFlap)
        Me.tpFlap.Controls.Add(Me.Label14)
        Me.tpFlap.Controls.Add(Me.lblWayer_Qty)
        Me.tpFlap.Controls.Add(Me.Label16)
        Me.tpFlap.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tpFlap.Location = New System.Drawing.Point(4, 25)
        Me.tpFlap.Name = "tpFlap"
        Me.tpFlap.Size = New System.Drawing.Size(964, 507)
        Me.tpFlap.TabIndex = 2
        Me.tpFlap.Text = "FLAP BOX"
        Me.tpFlap.UseVisualStyleBackColor = True
        '
        'lblWayer_Percent
        '
        Me.lblWayer_Percent.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblWayer_Percent.Font = New System.Drawing.Font("Tahoma", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWayer_Percent.ForeColor = System.Drawing.Color.Blue
        Me.lblWayer_Percent.Location = New System.Drawing.Point(822, 474)
        Me.lblWayer_Percent.Name = "lblWayer_Percent"
        Me.lblWayer_Percent.Size = New System.Drawing.Size(136, 27)
        Me.lblWayer_Percent.TabIndex = 13
        Me.lblWayer_Percent.Text = "0%"
        Me.lblWayer_Percent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'dgvFlap
        '
        Me.dgvFlap.AllowUserToAddRows = False
        Me.dgvFlap.AllowUserToDeleteRows = False
        Me.dgvFlap.AllowUserToResizeRows = False
        Me.dgvFlap.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvFlap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFlap.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn9, Me.DataGridViewTextBoxColumn10, Me.DataGridViewTextBoxColumn11, Me.DataGridViewTextBoxColumn12, Me.DataGridViewTextBoxColumn13, Me.DataGridViewTextBoxColumn14, Me.DataGridViewTextBoxColumn15, Me.DataGridViewTextBoxColumn16, Me.Column3, Me.Column4})
        Me.dgvFlap.Location = New System.Drawing.Point(6, 8)
        Me.dgvFlap.MultiSelect = False
        Me.dgvFlap.Name = "dgvFlap"
        Me.dgvFlap.ReadOnly = True
        Me.dgvFlap.RowHeadersVisible = False
        Me.dgvFlap.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFlap.Size = New System.Drawing.Size(952, 444)
        Me.dgvFlap.TabIndex = 12
        '
        'DataGridViewTextBoxColumn9
        '
        Me.DataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn9.DataPropertyName = "c_type"
        Me.DataGridViewTextBoxColumn9.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn9.HeaderText = "TYPE"
        Me.DataGridViewTextBoxColumn9.Name = "DataGridViewTextBoxColumn9"
        Me.DataGridViewTextBoxColumn9.ReadOnly = True
        '
        'DataGridViewTextBoxColumn10
        '
        Me.DataGridViewTextBoxColumn10.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn10.DataPropertyName = "n_index"
        Me.DataGridViewTextBoxColumn10.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn10.HeaderText = "INDEX"
        Me.DataGridViewTextBoxColumn10.Name = "DataGridViewTextBoxColumn10"
        Me.DataGridViewTextBoxColumn10.ReadOnly = True
        '
        'DataGridViewTextBoxColumn11
        '
        Me.DataGridViewTextBoxColumn11.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn11.DataPropertyName = "motor_code"
        Me.DataGridViewTextBoxColumn11.FillWeight = 18.0!
        Me.DataGridViewTextBoxColumn11.HeaderText = "M.CODE"
        Me.DataGridViewTextBoxColumn11.Name = "DataGridViewTextBoxColumn11"
        Me.DataGridViewTextBoxColumn11.ReadOnly = True
        '
        'DataGridViewTextBoxColumn12
        '
        Me.DataGridViewTextBoxColumn12.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn12.DataPropertyName = "c_obj_name"
        Me.DataGridViewTextBoxColumn12.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn12.HeaderText = "NAME"
        Me.DataGridViewTextBoxColumn12.Name = "DataGridViewTextBoxColumn12"
        Me.DataGridViewTextBoxColumn12.ReadOnly = True
        '
        'DataGridViewTextBoxColumn13
        '
        Me.DataGridViewTextBoxColumn13.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn13.DataPropertyName = "n_plc_station"
        Me.DataGridViewTextBoxColumn13.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn13.HeaderText = "PLC"
        Me.DataGridViewTextBoxColumn13.Name = "DataGridViewTextBoxColumn13"
        Me.DataGridViewTextBoxColumn13.ReadOnly = True
        '
        'DataGridViewTextBoxColumn14
        '
        Me.DataGridViewTextBoxColumn14.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn14.DataPropertyName = "c_location"
        Me.DataGridViewTextBoxColumn14.FillWeight = 15.0!
        Me.DataGridViewTextBoxColumn14.HeaderText = "LOCATION"
        Me.DataGridViewTextBoxColumn14.Name = "DataGridViewTextBoxColumn14"
        Me.DataGridViewTextBoxColumn14.ReadOnly = True
        '
        'DataGridViewTextBoxColumn15
        '
        Me.DataGridViewTextBoxColumn15.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn15.DataPropertyName = "dt_test_date"
        DataGridViewCellStyle6.Format = "g"
        DataGridViewCellStyle6.NullValue = Nothing
        Me.DataGridViewTextBoxColumn15.DefaultCellStyle = DataGridViewCellStyle6
        Me.DataGridViewTextBoxColumn15.FillWeight = 17.0!
        Me.DataGridViewTextBoxColumn15.HeaderText = "TEST DATE"
        Me.DataGridViewTextBoxColumn15.Name = "DataGridViewTextBoxColumn15"
        Me.DataGridViewTextBoxColumn15.ReadOnly = True
        '
        'DataGridViewTextBoxColumn16
        '
        Me.DataGridViewTextBoxColumn16.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.DataGridViewTextBoxColumn16.DataPropertyName = "f_complete_test"
        Me.DataGridViewTextBoxColumn16.FillWeight = 10.0!
        Me.DataGridViewTextBoxColumn16.HeaderText = "TEST STATUS"
        Me.DataGridViewTextBoxColumn16.Name = "DataGridViewTextBoxColumn16"
        Me.DataGridViewTextBoxColumn16.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.DataPropertyName = "c_user"
        Me.Column3.HeaderText = "USER"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Visible = False
        '
        'Column4
        '
        Me.Column4.DataPropertyName = "c_comment"
        Me.Column4.HeaderText = "COMMENT"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Visible = False
        '
        'Label14
        '
        Me.Label14.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(819, 459)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(139, 16)
        Me.Label14.TabIndex = 9
        Me.Label14.Text = "WAYER PROGRESS : "
        '
        'lblWayer_Qty
        '
        Me.lblWayer_Qty.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblWayer_Qty.ForeColor = System.Drawing.Color.Blue
        Me.lblWayer_Qty.Location = New System.Drawing.Point(122, 459)
        Me.lblWayer_Qty.Name = "lblWayer_Qty"
        Me.lblWayer_Qty.Size = New System.Drawing.Size(162, 16)
        Me.lblWayer_Qty.TabIndex = 8
        Me.lblWayer_Qty.Text = "0 / 0 รายการ"
        '
        'Label16
        '
        Me.Label16.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(6, 459)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(110, 16)
        Me.Label16.TabIndex = 7
        Me.Label16.Text = "TOTAL MOTOR : "
        '
        'btnRefresh
        '
        Me.btnRefresh.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnRefresh.Location = New System.Drawing.Point(16, 736)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(88, 30)
        Me.btnRefresh.TabIndex = 2
        Me.btnRefresh.Text = "REFRESH"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox2.Controls.Add(Me.lblM_PLC)
        Me.GroupBox2.Controls.Add(Me.lblM_User)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.lblM_Obj)
        Me.GroupBox2.Controls.Add(Me.lblM_Index)
        Me.GroupBox2.Controls.Add(Me.btnUpdate)
        Me.GroupBox2.Controls.Add(Me.lblM_Status)
        Me.GroupBox2.Controls.Add(Me.lblM_Code)
        Me.GroupBox2.Controls.Add(Me.lblM_Name)
        Me.GroupBox2.Controls.Add(Me.txtM_Comment)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(22, 645)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(951, 86)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "DEVICE DETAIL"
        '
        'lblM_PLC
        '
        Me.lblM_PLC.AutoSize = True
        Me.lblM_PLC.ForeColor = System.Drawing.Color.MidnightBlue
        Me.lblM_PLC.Location = New System.Drawing.Point(273, 19)
        Me.lblM_PLC.Name = "lblM_PLC"
        Me.lblM_PLC.Size = New System.Drawing.Size(31, 13)
        Me.lblM_PLC.TabIndex = 9
        Me.lblM_PLC.Text = "XXX"
        '
        'lblM_User
        '
        Me.lblM_User.ForeColor = System.Drawing.Color.MidnightBlue
        Me.lblM_User.Location = New System.Drawing.Point(314, 63)
        Me.lblM_User.Name = "lblM_User"
        Me.lblM_User.Size = New System.Drawing.Size(166, 16)
        Me.lblM_User.TabIndex = 8
        Me.lblM_User.Text = "XXXXX"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(247, 63)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(63, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "TEST BY "
        '
        'lblM_Obj
        '
        Me.lblM_Obj.AutoSize = True
        Me.lblM_Obj.ForeColor = System.Drawing.Color.MidnightBlue
        Me.lblM_Obj.Location = New System.Drawing.Point(6, 63)
        Me.lblM_Obj.Name = "lblM_Obj"
        Me.lblM_Obj.Size = New System.Drawing.Size(93, 13)
        Me.lblM_Obj.TabIndex = 6
        Me.lblM_Obj.Text = "OBJECT NAME"
        '
        'lblM_Index
        '
        Me.lblM_Index.AutoSize = True
        Me.lblM_Index.ForeColor = System.Drawing.Color.MidnightBlue
        Me.lblM_Index.Location = New System.Drawing.Point(60, 19)
        Me.lblM_Index.Name = "lblM_Index"
        Me.lblM_Index.Size = New System.Drawing.Size(31, 13)
        Me.lblM_Index.TabIndex = 5
        Me.lblM_Index.Text = "XXX"
        '
        'btnUpdate
        '
        Me.btnUpdate.BackColor = System.Drawing.Color.White
        Me.btnUpdate.Image = CType(resources.GetObject("btnUpdate.Image"), System.Drawing.Image)
        Me.btnUpdate.Location = New System.Drawing.Point(873, 16)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(72, 60)
        Me.btnUpdate.TabIndex = 4
        Me.btnUpdate.UseVisualStyleBackColor = False
        '
        'lblM_Status
        '
        Me.lblM_Status.BackColor = System.Drawing.Color.Red
        Me.lblM_Status.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblM_Status.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblM_Status.Location = New System.Drawing.Point(314, 18)
        Me.lblM_Status.Name = "lblM_Status"
        Me.lblM_Status.Size = New System.Drawing.Size(166, 29)
        Me.lblM_Status.TabIndex = 3
        Me.lblM_Status.Text = "NOT PASS"
        Me.lblM_Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblM_Code
        '
        Me.lblM_Code.AutoSize = True
        Me.lblM_Code.ForeColor = System.Drawing.Color.MidnightBlue
        Me.lblM_Code.Location = New System.Drawing.Point(6, 42)
        Me.lblM_Code.Name = "lblM_Code"
        Me.lblM_Code.Size = New System.Drawing.Size(41, 13)
        Me.lblM_Code.TabIndex = 2
        Me.lblM_Code.Text = "CODE"
        '
        'lblM_Name
        '
        Me.lblM_Name.AutoSize = True
        Me.lblM_Name.ForeColor = System.Drawing.Color.MidnightBlue
        Me.lblM_Name.Location = New System.Drawing.Point(6, 19)
        Me.lblM_Name.Name = "lblM_Name"
        Me.lblM_Name.Size = New System.Drawing.Size(52, 13)
        Me.lblM_Name.TabIndex = 1
        Me.lblM_Name.Text = "MOTOR"
        '
        'txtM_Comment
        '
        Me.txtM_Comment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtM_Comment.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtM_Comment.Location = New System.Drawing.Point(486, 16)
        Me.txtM_Comment.Multiline = True
        Me.txtM_Comment.Name = "txtM_Comment"
        Me.txtM_Comment.Size = New System.Drawing.Size(381, 60)
        Me.txtM_Comment.TabIndex = 0
        '
        'ctxMenu
        '
        Me.ctxMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsMenu_Reset})
        Me.ctxMenu.Name = "ContextMenuStrip1"
        Me.ctxMenu.Size = New System.Drawing.Size(150, 26)
        '
        'tsMenu_Reset
        '
        Me.tsMenu_Reset.Name = "tsMenu_Reset"
        Me.tsMenu_Reset.Size = New System.Drawing.Size(149, 22)
        Me.tsMenu_Reset.Text = "RESET STATUS"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 1000
        '
        'frmTestResult
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(996, 770)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.btnRefresh)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmTestResult"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TESTING RESULT"
        Me.TopMost = True
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.tpMotor.ResumeLayout(False)
        Me.tpMotor.PerformLayout()
        CType(Me.dgvMotor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpSlide.ResumeLayout(False)
        Me.tpSlide.PerformLayout()
        CType(Me.dgvSlide, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpFlap.ResumeLayout(False)
        Me.tpFlap.PerformLayout()
        CType(Me.dgvFlap, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ctxMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblPercent As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblQty As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents tpMotor As TabPage
    Friend WithEvents dgvMotor As DataGridView
    Friend WithEvents tpSlide As TabPage
    Friend WithEvents tpFlap As TabPage
    Friend WithEvents lblFrmName As Label
    Friend WithEvents lblAppName As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents lblMotor_Percent As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents lblMotor_Qty As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents lblSlide_Qty As Label
    Friend WithEvents Label12 As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents lblWayer_Qty As Label
    Friend WithEvents Label16 As Label
    Friend WithEvents dgvSlide As DataGridView
    Friend WithEvents dgvFlap As DataGridView
    Friend WithEvents lblSlide_Percent As Label
    Friend WithEvents lblWayer_Percent As Label
    Friend WithEvents btnRefresh As Button
    Friend WithEvents clmObj_Type As DataGridViewTextBoxColumn
    Friend WithEvents clmIndex As DataGridViewTextBoxColumn
    Friend WithEvents clmMcode As DataGridViewTextBoxColumn
    Friend WithEvents clmName As DataGridViewTextBoxColumn
    Friend WithEvents clmPLC As DataGridViewTextBoxColumn
    Friend WithEvents clmLocation As DataGridViewTextBoxColumn
    Friend WithEvents clmDate As DataGridViewTextBoxColumn
    Friend WithEvents clmStatus As DataGridViewTextBoxColumn
    Friend WithEvents clmUser As DataGridViewTextBoxColumn
    Friend WithEvents clmComment As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn1 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn4 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn5 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn6 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn7 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn8 As DataGridViewTextBoxColumn
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn9 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn10 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn11 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn12 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn13 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn14 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn15 As DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn16 As DataGridViewTextBoxColumn
    Friend WithEvents Column3 As DataGridViewTextBoxColumn
    Friend WithEvents Column4 As DataGridViewTextBoxColumn
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblM_PLC As Label
    Friend WithEvents lblM_User As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents lblM_Obj As Label
    Friend WithEvents lblM_Index As Label
    Friend WithEvents btnUpdate As Button
    Friend WithEvents lblM_Status As Label
    Friend WithEvents lblM_Code As Label
    Friend WithEvents lblM_Name As Label
    Friend WithEvents txtM_Comment As TextBox
    Friend WithEvents ctxMenu As ContextMenuStrip
    Friend WithEvents tsMenu_Reset As ToolStripMenuItem
    Friend WithEvents Timer1 As Timer
End Class
