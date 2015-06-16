Public Class FrmMPList

    Public Property MPointList As List(Of MPoint)
    Private MPoint As MPoint
    Public MPListBindSrc1 As New BindingSource

    Private Sub FrmMPList_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MPListBindSrc1.DataSource = MPointList
        ViewMPList.DataSource = MPListBindSrc1
    End Sub

    Private Sub BtnClose_Click(sender As Object, e As EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub

    Private Sub BtnLoad_Click(sender As Object, e As EventArgs) Handles BtnLoad.Click
        Dim openfiledialog1 As New OpenFileDialog
        openfiledialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
        Dim xmlFileName As String = vbNullString
        If openfiledialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            If MsgBox("The current data in the List will be removed. Proceed?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation Or MsgBoxStyle.DefaultButton2) = 2 Then GoTo TerminateSub
            FrmMain.ReadMPListFromXML(openfiledialog1.FileName)
            ViewMPList.DataSource = Nothing
            ViewMPList.DataSource = MPListBindSrc1
        End If
TerminateSub:
        openfiledialog1 = Nothing
    End Sub

    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        Dim savefiledialog1 As New SaveFileDialog
        savefiledialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*"
        Dim xmlFileName As String = vbNullString
        savefiledialog1.ShowDialog()
        FrmMain.WriteMPListToXML(savefiledialog1.FileName)
        savefiledialog1 = Nothing
    End Sub

    Private Sub BtnPaste_Click(sender As Object, e As EventArgs) Handles BtnPaste.Click

        If Not Clipboard.ContainsData("Text") Then MsgBox("Invalid data in clipboard") : Exit Sub
        If MsgBox("The current data in the List will be removed. Proceed?", MsgBoxStyle.OkCancel Or MsgBoxStyle.Exclamation Or MsgBoxStyle.DefaultButton2) = 2 Then Exit Sub

        MPointList.Clear()

        Try
            Dim s As String = Clipboard.GetText()
            Dim Rows() As String = s.Split(vbLf)
            For i = 0 To Rows.Length - 1
                Rows(i) = Rows(i).Replace(vbCr, "").Replace(vbLf, "")
                If Rows(i) <> "" Then
                    Dim Columns() As String = Rows(i).Split(vbTab)
                    MPointList.Add(New MPoint)
                    With MPointList.ElementAt(i)
                        If Columns.Length >= 1 Then .MPName = Columns(0)
                        If Columns.Length >= 2 Then .MPCode = If(Columns(1) <> vbNullString, CInt(Columns(1)), 0)
                        If Columns.Length >= 3 Then .AlarmHigh1 = Columns(2)
                        If Columns.Length >= 4 Then .AlarmHigh2 = Columns(3)
                        If Columns.Length >= 5 Then .AlarmHigh3 = Columns(4)
                        If Columns.Length >= 6 Then .AlarmHigh4 = Columns(5)
                        If Columns.Length >= 7 Then .AlarmLow1 = Columns(6)
                        If Columns.Length >= 8 Then .AlarmLow2 = Columns(7)
                        If Columns.Length >= 9 Then .AlarmLow3 = Columns(8)
                        If Columns.Length >= 10 Then .AlarmLow4 = Columns(9)
                        If Columns.Length >= 11 Then .LastAlarm = Columns(10)
                    End With
                End If
            Next
        Catch ex As Exception
            MessageBox.Show("INPUT ERROR" & ControlChars.NewLine & ex.Message)
        End Try

        ViewMPList.DataSource = Nothing
        ViewMPList.DataSource = MPListBindSrc1

    End Sub

End Class