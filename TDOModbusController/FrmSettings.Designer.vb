<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmSettings
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
        Me.ViewSettings = New System.Windows.Forms.PropertyGrid()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ViewSettings
        '
        Me.ViewSettings.HelpVisible = False
        Me.ViewSettings.Location = New System.Drawing.Point(12, 13)
        Me.ViewSettings.Name = "ViewSettings"
        Me.ViewSettings.PropertySort = System.Windows.Forms.PropertySort.NoSort
        Me.ViewSettings.Size = New System.Drawing.Size(450, 491)
        Me.ViewSettings.TabIndex = 0
        Me.ViewSettings.ToolbarVisible = False
        '
        'BtnClose
        '
        Me.BtnClose.Location = New System.Drawing.Point(364, 510)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(98, 28)
        Me.BtnClose.TabIndex = 1
        Me.BtnClose.Text = "Save and Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'FrmSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(474, 550)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.ViewSettings)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.MaximizeBox = False
        Me.Name = "FrmSettings"
        Me.Text = "Settings"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ViewSettings As System.Windows.Forms.PropertyGrid
    Friend WithEvents BtnClose As System.Windows.Forms.Button
End Class
