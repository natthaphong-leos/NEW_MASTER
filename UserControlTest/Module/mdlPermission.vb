Imports System.Windows.Forms

Module mdlPermission
    Public Function RequireScada() As Boolean
        Dim mdi As MDI_FRM = MDI_FRM.Current()
        If mdi Is Nothing Then
            mdi = Application.OpenForms.OfType(Of MDI_FRM)().FirstOrDefault()
        End If
        If mdi Is Nothing Then
            MessageBox.Show("MDI form not found.", "SCADA",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

        If Not mdi.IsAuthenticated Then
            If MessageBox.Show("SCADA login required. Proceed to login?", "Action Required",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                If Not mdi.PromptLogin(mdi) Then Return False
            Else
                Return False
            End If
        End If

        If Not mdi.Main_Scada Then
            MessageBox.Show("You are logged in but don't have permission for Main SCADA.",
                            "SCADA", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Return True
    End Function

End Module
