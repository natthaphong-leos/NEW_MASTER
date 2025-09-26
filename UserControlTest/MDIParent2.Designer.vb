<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDIParent2
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDIParent2))
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.F1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.WindowsMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.CascadeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewWindowToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TileVerticalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TileHorizontalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CloseAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ArrangeIconsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolBar1 = New System.Windows.Forms.ToolBar()
        Me.btnBATCHING = New System.Windows.Forms.ToolBarButton()
        Me.btnMOLASSES = New System.Windows.Forms.ToolBarButton()
        Me.btnJob_Assingnment = New System.Windows.Forms.ToolBarButton()
        Me.btnLOG_ON = New System.Windows.Forms.ToolBarButton()
        Me.btnLOG_OFF = New System.Windows.Forms.ToolBarButton()
        Me.btn = New System.Windows.Forms.ToolBarButton()
        Me.DropDows = New System.Windows.Forms.ToolBarButton()
        Me.btnSHOW_CODE = New System.Windows.Forms.ToolBarButton()
        Me.btnPRODUCTION_TIME = New System.Windows.Forms.ToolBarButton()
        Me.btnExit = New System.Windows.Forms.ToolBarButton()
        Me.btnREFRESH = New System.Windows.Forms.ToolBarButton()
        Me.ToolBarButton1 = New System.Windows.Forms.ToolBarButton()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SssToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SssssToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.MenuStrip.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip
        '
        Me.MenuStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.MenuStrip.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.F1ToolStripMenuItem, Me.WindowsMenu})
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.MdiWindowListItem = Me.WindowsMenu
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Size = New System.Drawing.Size(1122, 24)
        Me.MenuStrip.TabIndex = 5
        Me.MenuStrip.Text = "MenuStrip"
        '
        'F1ToolStripMenuItem
        '
        Me.F1ToolStripMenuItem.Name = "F1ToolStripMenuItem"
        Me.F1ToolStripMenuItem.Size = New System.Drawing.Size(34, 20)
        Me.F1ToolStripMenuItem.Text = "F1"
        '
        'WindowsMenu
        '
        Me.WindowsMenu.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CascadeToolStripMenuItem, Me.NewWindowToolStripMenuItem, Me.TileVerticalToolStripMenuItem, Me.TileHorizontalToolStripMenuItem, Me.CloseAllToolStripMenuItem, Me.ArrangeIconsToolStripMenuItem})
        Me.WindowsMenu.Name = "WindowsMenu"
        Me.WindowsMenu.Size = New System.Drawing.Size(78, 20)
        Me.WindowsMenu.Text = "&Windows"
        '
        'CascadeToolStripMenuItem
        '
        Me.CascadeToolStripMenuItem.Name = "CascadeToolStripMenuItem"
        Me.CascadeToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.CascadeToolStripMenuItem.Text = "&Cascade"
        '
        'NewWindowToolStripMenuItem
        '
        Me.NewWindowToolStripMenuItem.Name = "NewWindowToolStripMenuItem"
        Me.NewWindowToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.NewWindowToolStripMenuItem.Text = "&New Window"
        '
        'TileVerticalToolStripMenuItem
        '
        Me.TileVerticalToolStripMenuItem.Name = "TileVerticalToolStripMenuItem"
        Me.TileVerticalToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.TileVerticalToolStripMenuItem.Text = "Tile &Vertical"
        '
        'TileHorizontalToolStripMenuItem
        '
        Me.TileHorizontalToolStripMenuItem.Name = "TileHorizontalToolStripMenuItem"
        Me.TileHorizontalToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.TileHorizontalToolStripMenuItem.Text = "Tile &Horizontal"
        '
        'CloseAllToolStripMenuItem
        '
        Me.CloseAllToolStripMenuItem.Name = "CloseAllToolStripMenuItem"
        Me.CloseAllToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.CloseAllToolStripMenuItem.Text = "C&lose All"
        '
        'ArrangeIconsToolStripMenuItem
        '
        Me.ArrangeIconsToolStripMenuItem.Name = "ArrangeIconsToolStripMenuItem"
        Me.ArrangeIconsToolStripMenuItem.Size = New System.Drawing.Size(169, 22)
        Me.ArrangeIconsToolStripMenuItem.Text = "&Arrange Icons"
        '
        'StatusStrip
        '
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 464)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(1122, 22)
        Me.StatusStrip.TabIndex = 7
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(38, 17)
        Me.ToolStripStatusLabel.Text = "Status"
        '
        'ToolBar1
        '
        Me.ToolBar1.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.btnBATCHING, Me.btnMOLASSES, Me.btnJob_Assingnment, Me.btnLOG_ON, Me.btnLOG_OFF, Me.btn, Me.DropDows, Me.btnSHOW_CODE, Me.btnPRODUCTION_TIME, Me.btnExit, Me.btnREFRESH, Me.ToolBarButton1})
        Me.ToolBar1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ToolBar1.DropDownArrows = True
        Me.ToolBar1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(222, Byte))
        Me.ToolBar1.ImageList = Me.ImageList1
        Me.ToolBar1.Location = New System.Drawing.Point(0, 24)
        Me.ToolBar1.Name = "ToolBar1"
        Me.ToolBar1.ShowToolTips = True
        Me.ToolBar1.Size = New System.Drawing.Size(1122, 43)
        Me.ToolBar1.TabIndex = 9
        '
        'btnBATCHING
        '
        Me.btnBATCHING.ImageIndex = 4
        Me.btnBATCHING.Name = "btnBATCHING"
        Me.btnBATCHING.Text = "BATCHING"
        '
        'btnMOLASSES
        '
        Me.btnMOLASSES.ImageKey = "1194984775760075334button-green_benji_park_01.svg.thumb.png"
        Me.btnMOLASSES.Name = "btnMOLASSES"
        Me.btnMOLASSES.Text = "MOLASSES"
        Me.btnMOLASSES.ToolTipText = "1"
        '
        'btnJob_Assingnment
        '
        Me.btnJob_Assingnment.Name = "btnJob_Assingnment"
        Me.btnJob_Assingnment.Text = "Job Assingnment"
        '
        'btnLOG_ON
        '
        Me.btnLOG_ON.Name = "btnLOG_ON"
        Me.btnLOG_ON.Text = "LOG ON"
        '
        'btnLOG_OFF
        '
        Me.btnLOG_OFF.Name = "btnLOG_OFF"
        Me.btnLOG_OFF.Text = "LOG OFF"
        '
        'btn
        '
        Me.btn.Name = "btn"
        Me.btn.Text = "SHOW BIN CODE"
        '
        'DropDows
        '
        Me.DropDows.Name = "DropDows"
        Me.DropDows.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        '
        'btnSHOW_CODE
        '
        Me.btnSHOW_CODE.Name = "btnSHOW_CODE"
        Me.btnSHOW_CODE.Text = "SHOW CODE"
        '
        'btnPRODUCTION_TIME
        '
        Me.btnPRODUCTION_TIME.Name = "btnPRODUCTION_TIME"
        Me.btnPRODUCTION_TIME.Text = "PRODUCTION TIME"
        '
        'btnExit
        '
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton
        Me.btnExit.Text = "Exit"
        '
        'btnREFRESH
        '
        Me.btnREFRESH.Name = "btnREFRESH"
        Me.btnREFRESH.Text = "REFRESH"
        '
        'ToolBarButton1
        '
        Me.ToolBarButton1.Name = "ToolBarButton1"
        Me.ToolBarButton1.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SssToolStripMenuItem, Me.SssssToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(100, 48)
        '
        'SssToolStripMenuItem
        '
        Me.SssToolStripMenuItem.Name = "SssToolStripMenuItem"
        Me.SssToolStripMenuItem.Size = New System.Drawing.Size(99, 22)
        Me.SssToolStripMenuItem.Text = "sss"
        '
        'SssssToolStripMenuItem
        '
        Me.SssssToolStripMenuItem.Name = "SssssToolStripMenuItem"
        Me.SssssToolStripMenuItem.Size = New System.Drawing.Size(99, 22)
        Me.SssssToolStripMenuItem.Text = "sssss"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "1194984775760075334button-green_benji_park_01.svg.thumb.png")
        Me.ImageList1.Images.SetKeyName(1, "device_square_off.gif")
        Me.ImageList1.Images.SetKeyName(2, "device_square_on.gif")
        Me.ImageList1.Images.SetKeyName(3, "dsadsa.gif")
        Me.ImageList1.Images.SetKeyName(4, "fall-round-button-hi.png")
        Me.ImageList1.Images.SetKeyName(5, "free-vector-roystonlodge-simple-glossy-circle-button-red-clip-art_117009_Roystonl" & _
        "odge_Simple_Glossy_Circle_Button_Red_clip_art_medium.png")
        Me.ImageList1.Images.SetKeyName(6, "mlx752w.jpg")
        '
        'MDIParent2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1122, 486)
        Me.Controls.Add(Me.ToolBar1)
        Me.Controls.Add(Me.MenuStrip)
        Me.Controls.Add(Me.StatusStrip)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Name = "MDIParent2"
        Me.Text = "MDIParent2"
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout

End Sub
    Friend WithEvents ArrangeIconsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CloseAllToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewWindowToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents WindowsMenu As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CascadeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileVerticalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TileHorizontalToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents F1ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnBATCHING As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnMOLASSES As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnJob_Assingnment As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnLOG_ON As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnLOG_OFF As System.Windows.Forms.ToolBarButton
    Friend WithEvents btn As System.Windows.Forms.ToolBarButton
    Friend WithEvents DropDows As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnSHOW_CODE As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnPRODUCTION_TIME As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnExit As System.Windows.Forms.ToolBarButton
    Friend WithEvents btnREFRESH As System.Windows.Forms.ToolBarButton
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Private WithEvents ToolBar1 As System.Windows.Forms.ToolBar
    Friend WithEvents ToolBarButton1 As System.Windows.Forms.ToolBarButton
    Friend WithEvents SssToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SssssToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList

End Class
