Public Class FrmSettings
    Public Property CSettings1 As CSettings

    Private Sub FrmSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ViewSettings.SelectedObject = CSettings1
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub
End Class