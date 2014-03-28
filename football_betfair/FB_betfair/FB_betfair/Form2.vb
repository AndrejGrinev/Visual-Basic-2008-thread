Public Class Form2
    Private Sub Chk1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Chk1.CheckedChanged
        If Chk1.Checked Then
            Me.TopMost = True
        Else
            Me.TopMost = False
        End If
    End Sub

    
End Class