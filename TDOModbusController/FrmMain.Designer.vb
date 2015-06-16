<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmMain))
        Me.BtnMPList = New System.Windows.Forms.Button()
        Me.BtnClose = New System.Windows.Forms.Button()
        Me.BtnSettings = New System.Windows.Forms.Button()
        Me.BtnDBCnn = New System.Windows.Forms.Button()
        Me.TxBStatus = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnModbusCnn = New System.Windows.Forms.Button()
        Me.AlarmMonkey = New System.ComponentModel.BackgroundWorker()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.LblErrCount = New System.Windows.Forms.Label()
        Me.BtnStart = New System.Windows.Forms.Button()
        Me.BtnStop = New System.Windows.Forms.Button()
        Me.BtnExt = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'BtnMPList
        '
        Me.BtnMPList.Location = New System.Drawing.Point(283, 12)
        Me.BtnMPList.Name = "BtnMPList"
        Me.BtnMPList.Size = New System.Drawing.Size(89, 25)
        Me.BtnMPList.TabIndex = 0
        Me.BtnMPList.Text = "MPList"
        Me.BtnMPList.UseVisualStyleBackColor = True
        '
        'BtnClose
        '
        Me.BtnClose.Location = New System.Drawing.Point(300, 188)
        Me.BtnClose.Name = "BtnClose"
        Me.BtnClose.Size = New System.Drawing.Size(72, 25)
        Me.BtnClose.TabIndex = 1
        Me.BtnClose.Text = "Close"
        Me.BtnClose.UseVisualStyleBackColor = True
        '
        'BtnSettings
        '
        Me.BtnSettings.Location = New System.Drawing.Point(283, 43)
        Me.BtnSettings.Name = "BtnSettings"
        Me.BtnSettings.Size = New System.Drawing.Size(89, 25)
        Me.BtnSettings.TabIndex = 2
        Me.BtnSettings.Text = "Settings"
        Me.BtnSettings.UseVisualStyleBackColor = True
        '
        'BtnDBCnn
        '
        Me.BtnDBCnn.Location = New System.Drawing.Point(283, 74)
        Me.BtnDBCnn.Name = "BtnDBCnn"
        Me.BtnDBCnn.Size = New System.Drawing.Size(89, 38)
        Me.BtnDBCnn.TabIndex = 3
        Me.BtnDBCnn.Text = "Test DB Connection"
        Me.BtnDBCnn.UseVisualStyleBackColor = True
        '
        'TxBStatus
        '
        Me.TxBStatus.Location = New System.Drawing.Point(12, 162)
        Me.TxBStatus.Name = "TxBStatus"
        Me.TxBStatus.Size = New System.Drawing.Size(360, 20)
        Me.TxBStatus.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 146)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(67, 13)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Last Activity:"
        '
        'BtnModbusCnn
        '
        Me.BtnModbusCnn.Location = New System.Drawing.Point(283, 118)
        Me.BtnModbusCnn.Name = "BtnModbusCnn"
        Me.BtnModbusCnn.Size = New System.Drawing.Size(89, 38)
        Me.BtnModbusCnn.TabIndex = 6
        Me.BtnModbusCnn.Text = "Test MODBUS Connection"
        Me.BtnModbusCnn.UseVisualStyleBackColor = True
        '
        'AlarmMonkey
        '
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 194)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(146, 13)
        Me.Label2.TabIndex = 9
        Me.Label2.Text = "Errors Count Since Last Start:"
        '
        'LblErrCount
        '
        Me.LblErrCount.AutoSize = True
        Me.LblErrCount.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblErrCount.Location = New System.Drawing.Point(161, 194)
        Me.LblErrCount.Name = "LblErrCount"
        Me.LblErrCount.Size = New System.Drawing.Size(14, 13)
        Me.LblErrCount.TabIndex = 10
        Me.LblErrCount.Text = "0"
        '
        'BtnStart
        '
        Me.BtnStart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStart.Location = New System.Drawing.Point(15, 12)
        Me.BtnStart.Name = "BtnStart"
        Me.BtnStart.Size = New System.Drawing.Size(109, 38)
        Me.BtnStart.TabIndex = 11
        Me.BtnStart.Text = "Start"
        Me.BtnStart.UseVisualStyleBackColor = True
        '
        'BtnStop
        '
        Me.BtnStop.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnStop.Location = New System.Drawing.Point(15, 56)
        Me.BtnStop.Name = "BtnStop"
        Me.BtnStop.Size = New System.Drawing.Size(109, 38)
        Me.BtnStop.TabIndex = 12
        Me.BtnStop.Text = "Stop"
        Me.BtnStop.UseVisualStyleBackColor = True
        '
        'BtnExt
        '
        Me.BtnExt.Location = New System.Drawing.Point(188, 118)
        Me.BtnExt.Name = "BtnExt"
        Me.BtnExt.Size = New System.Drawing.Size(89, 38)
        Me.BtnExt.TabIndex = 13
        Me.BtnExt.Text = "Test External Command"
        Me.BtnExt.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BackgroundImage = CType(resources.GetObject("PictureBox1.BackgroundImage"), System.Drawing.Image)
        Me.PictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.PictureBox1.InitialImage = Nothing
        Me.PictureBox1.Location = New System.Drawing.Point(148, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(129, 100)
        Me.PictureBox1.TabIndex = 14
        Me.PictureBox1.TabStop = False
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 225)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.BtnExt)
        Me.Controls.Add(Me.BtnStop)
        Me.Controls.Add(Me.BtnStart)
        Me.Controls.Add(Me.LblErrCount)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.BtnModbusCnn)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.TxBStatus)
        Me.Controls.Add(Me.BtnDBCnn)
        Me.Controls.Add(Me.BtnSettings)
        Me.Controls.Add(Me.BtnClose)
        Me.Controls.Add(Me.BtnMPList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "FrmMain"
        Me.Text = "TDOModbusController 0.9.1.9"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnMPList As System.Windows.Forms.Button
    Friend WithEvents BtnClose As System.Windows.Forms.Button
    Friend WithEvents BtnSettings As System.Windows.Forms.Button
    Friend WithEvents BtnDBCnn As System.Windows.Forms.Button
    Friend WithEvents TxBStatus As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents BtnModbusCnn As System.Windows.Forms.Button
    Friend WithEvents AlarmMonkey As System.ComponentModel.BackgroundWorker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents LblErrCount As System.Windows.Forms.Label
    Friend WithEvents BtnStart As System.Windows.Forms.Button
    Friend WithEvents BtnStop As System.Windows.Forms.Button
    Friend WithEvents BtnExt As System.Windows.Forms.Button
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox

End Class
