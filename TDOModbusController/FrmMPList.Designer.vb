<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMPList
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
        Me.ViewMPList = New System.Windows.Forms.DataGridView()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnPaste = New System.Windows.Forms.Button()
        Me.TTipBtnPaste = New System.Windows.Forms.ToolTip(Me.components)
        Me.BtnLoad = New System.Windows.Forms.Button()
        Me.BtnSave = New System.Windows.Forms.Button()
        CType(Me.ViewMPList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ViewMPList
        '
        Me.ViewMPList.AllowUserToOrderColumns = True
        Me.ViewMPList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ViewMPList.Location = New System.Drawing.Point(12, 12)
        Me.ViewMPList.Name = "ViewMPList"
        Me.ViewMPList.Size = New System.Drawing.Size(1153, 398)
        Me.ViewMPList.TabIndex = 0
        '
        'BtnClose
        '
        Me.BtnClose.Location = New System.Drawing.Point(1069, 428)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(96, 28)
        Me.BtnClose.TabIndex = 1
        Me.BtnClose.Text = "Save and Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnPaste
        '
        Me.BtnPaste.Location = New System.Drawing.Point(115, 420)
        Me.BtnPaste.Name = "BtnPaste"
        Me.BtnPaste.Size = New System.Drawing.Size(96, 36)
        Me.BtnPaste.TabIndex = 2
        Me.BtnPaste.Text = "Paste from Clipboard"
        Me.TTipBtnPaste.SetToolTip(Me.BtnPaste, "Pasting from clipboard works with " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "copying from Excel or from any text " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "with co" & _
        "lumns separated with tab.")
        Me.BtnPaste.UseVisualStyleBackColor = True
        '
        'TTipBtnPaste
        '
        Me.TTipBtnPaste.AutomaticDelay = 300
        '
        'BtnLoad
        '
        Me.BtnLoad.Location = New System.Drawing.Point(12, 420)
        Me.BtnLoad.Name = "BtnLoad"
        Me.BtnLoad.Size = New System.Drawing.Size(97, 36)
        Me.BtnLoad.TabIndex = 3
        Me.BtnLoad.Text = "Load From XML"
        Me.BtnLoad.UseVisualStyleBackColor = True
        '
        'BtnSave
        '
        Me.BtnSave.Location = New System.Drawing.Point(217, 420)
        Me.BtnSave.Name = "BtnSave"
        Me.BtnSave.Size = New System.Drawing.Size(93, 36)
        Me.BtnSave.TabIndex = 4
        Me.BtnSave.Text = "Save to XML"
        Me.BtnSave.UseVisualStyleBackColor = True
        '
        'FrmMPList
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1175, 468)
        Me.Controls.Add(Me.BtnSave)
        Me.Controls.Add(Me.BtnLoad)
        Me.Controls.Add(Me.BtnPaste)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.ViewMPList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Name = "FrmMPList"
        Me.Text = "FrmMPList"
        CType(Me.ViewMPList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ViewMPList As System.Windows.Forms.DataGridView
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnPaste As System.Windows.Forms.Button
    Friend WithEvents TTipBtnPaste As System.Windows.Forms.ToolTip
    Friend WithEvents BtnLoad As System.Windows.Forms.Button
    Friend WithEvents BtnSave As System.Windows.Forms.Button
End Class
