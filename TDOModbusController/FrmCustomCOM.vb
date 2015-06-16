Public Class FrmCustomCOM

    Private Sub FrmCustomCOM_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TxtCOMInCustom.Text = FrmMain.COMInCustom
        TxtCOMOutCustom.Text = FrmMain.COMOutCustom
    End Sub

    Private Sub TxtCOMCustomIN_TextChanged(sender As Object, e As EventArgs) Handles TxtCOMInCustom.TextChanged
        FrmMain.COMInCustom = TxtCOMInCustom.Text
    End Sub

    Private Sub TxtCOMOutCustom_TextChanged(sender As Object, e As EventArgs) Handles TxtCOMOutCustom.TextChanged
        FrmMain.COMOutCustom = TxtCOMOutCustom.Text
    End Sub

End Class