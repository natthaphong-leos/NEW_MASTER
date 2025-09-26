Imports System.Windows.Forms


Public Class MDIParent2

    Private Sub ShowNewForm(ByVal sender As Object, ByVal e As EventArgs) Handles NewWindowToolStripMenuItem.Click
        ' Create a new instance of the child form.
        Dim ChildForm As New System.Windows.Forms.Form
        ' Make it a child of this MDI form before showing it.
        ChildForm.MdiParent = Me

        m_ChildFormNumber += 1
        ChildForm.Text = "Window " & m_ChildFormNumber

        ChildForm.Show()
    End Sub

    Private Sub OpenFile(ByVal sender As Object, ByVal e As EventArgs)
        Dim OpenFileDialog As New OpenFileDialog
        OpenFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        OpenFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
        If (OpenFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = OpenFileDialog.FileName
            ' TODO: Add code here to open the file.
        End If
    End Sub

    Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim SaveFileDialog As New SaveFileDialog
        SaveFileDialog.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        SaveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"

        If (SaveFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK) Then
            Dim FileName As String = SaveFileDialog.FileName
            ' TODO: Add code here to save the current contents of the form to a file.
        End If
    End Sub


    Private Sub ExitToolsStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.Close()
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Use My.Computer.Clipboard to insert the selected text or images into the clipboard
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Use My.Computer.Clipboard.GetText() or My.Computer.Clipboard.GetData to retrieve information from the clipboard.
    End Sub



    Private Sub CascadeToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CascadeToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.Cascade)
    End Sub

    Private Sub TileVerticalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileVerticalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileVertical)
    End Sub

    Private Sub TileHorizontalToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles TileHorizontalToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.TileHorizontal)
    End Sub

    Private Sub ArrangeIconsToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ArrangeIconsToolStripMenuItem.Click
        Me.LayoutMdi(MdiLayout.ArrangeIcons)
    End Sub

    Private Sub CloseAllToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseAllToolStripMenuItem.Click
        ' Close all child forms of the parent.
        For Each ChildForm As Form In Me.MdiChildren
            ChildForm.Close()
        Next
    End Sub

    Private m_ChildFormNumber As Integer

    Private Sub F1ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles F1ToolStripMenuItem.Click
        FrmBatching.MdiParent = Me
        FrmBatching.Show()
    End Sub

    Private Sub ToolBar1_ButtonClick(sender As Object, e As ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonClick
        Select Case e.Button.Name
            Case "btnBATCHING"
                MsgBox(e.Button.Name)
                FrmBatching.MdiParent = Me
                FrmBatching.Show()

            Case "btnMOLASSES"
                MsgBox(e.Button.Name)
                Form1.MdiParent = Me
                Form1.Show()
            Case "btnJob_Assingnment"
                MsgBox(e.Button.Name)
            Case "btnLOG_ON"
                MsgBox(e.Button.Name)
            Case "btnLOG_OFF"
                MsgBox(e.Button.Name)
            Case "btnSHOW_BIN_CODE"
                MsgBox(e.Button.Name)
            Case "DropDows"
                MsgBox(e.Button.Name)
               
            Case "btnSHOW_CODE"
                MsgBox(e.Button.Name)
            Case "btnPRODUCTION_TIME"
                MsgBox(e.Button.Name)
            Case "btnExit"
                MsgBox(e.Button.Name)
            Case "btnREFRESH"
                MsgBox(e.Button.Name)
        End Select
    End Sub


    Private Sub MDIParent2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
     
    End Sub
    Sub ddd()
        Dim dd As New ToolStripDropDownMenu
        dd.Items.Add("deee")
        dd.Show()

    End Sub


End Class
