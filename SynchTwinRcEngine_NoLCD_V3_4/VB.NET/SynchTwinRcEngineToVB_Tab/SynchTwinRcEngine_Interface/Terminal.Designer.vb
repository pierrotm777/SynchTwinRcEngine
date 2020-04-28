<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Terminal
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
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

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TextBoxTerminalComPort = New System.Windows.Forms.TextBox()
        Me.LabelTerminalComPort = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'TextBoxTerminalComPort
        '
        Me.TextBoxTerminalComPort.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxTerminalComPort.BackColor = System.Drawing.Color.Black
        Me.TextBoxTerminalComPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TextBoxTerminalComPort.ForeColor = System.Drawing.Color.DarkOrange
        Me.TextBoxTerminalComPort.Location = New System.Drawing.Point(2, 3)
        Me.TextBoxTerminalComPort.Multiline = True
        Me.TextBoxTerminalComPort.Name = "TextBoxTerminalComPort"
        Me.TextBoxTerminalComPort.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.TextBoxTerminalComPort.Size = New System.Drawing.Size(588, 390)
        Me.TextBoxTerminalComPort.TabIndex = 141
        '
        'LabelTerminalComPort
        '
        Me.LabelTerminalComPort.AutoSize = True
        Me.LabelTerminalComPort.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTerminalComPort.Location = New System.Drawing.Point(-57, 32)
        Me.LabelTerminalComPort.Name = "LabelTerminalComPort"
        Me.LabelTerminalComPort.Size = New System.Drawing.Size(64, 16)
        Me.LabelTerminalComPort.TabIndex = 138
        Me.LabelTerminalComPort.Text = "Terminal:"
        '
        'Terminal
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 394)
        Me.Controls.Add(Me.TextBoxTerminalComPort)
        Me.Controls.Add(Me.LabelTerminalComPort)
        Me.Name = "Terminal"
        Me.Text = "Terminal"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TextBoxTerminalComPort As System.Windows.Forms.TextBox
    Friend WithEvents LabelTerminalComPort As System.Windows.Forms.Label
End Class
