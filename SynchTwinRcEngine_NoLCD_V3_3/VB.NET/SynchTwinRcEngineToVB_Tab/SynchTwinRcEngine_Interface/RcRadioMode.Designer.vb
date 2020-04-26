<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RcRadioMode
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
        Me.labelRcRadioMode = New System.Windows.Forms.Label()
        Me.PictureBoxMode = New System.Windows.Forms.PictureBox()
        Me.ComboBox_Select_ModeType = New System.Windows.Forms.ComboBox()
        Me.Label_Select_ModeRc = New System.Windows.Forms.Label()
        Me.ComboBoxSignalType = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        CType(Me.PictureBoxMode, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'labelRcRadioMode
        '
        Me.labelRcRadioMode.AutoSize = True
        Me.labelRcRadioMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelRcRadioMode.Location = New System.Drawing.Point(150, 22)
        Me.labelRcRadioMode.Name = "labelRcRadioMode"
        Me.labelRcRadioMode.Size = New System.Drawing.Size(223, 17)
        Me.labelRcRadioMode.TabIndex = 56
        Me.labelRcRadioMode.Text = "Configuration des canaux RC:"
        '
        'PictureBoxMode
        '
        Me.PictureBoxMode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBoxMode.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources.mode4
        Me.PictureBoxMode.Location = New System.Drawing.Point(207, 60)
        Me.PictureBoxMode.Name = "PictureBoxMode"
        Me.PictureBoxMode.Size = New System.Drawing.Size(319, 258)
        Me.PictureBoxMode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxMode.TabIndex = 57
        Me.PictureBoxMode.TabStop = False
        '
        'ComboBox_Select_ModeType
        '
        Me.ComboBox_Select_ModeType.FormattingEnabled = True
        Me.ComboBox_Select_ModeType.Items.AddRange(New Object() {"1", "2", "3", "4"})
        Me.ComboBox_Select_ModeType.Location = New System.Drawing.Point(91, 123)
        Me.ComboBox_Select_ModeType.Name = "ComboBox_Select_ModeType"
        Me.ComboBox_Select_ModeType.Size = New System.Drawing.Size(96, 21)
        Me.ComboBox_Select_ModeType.TabIndex = 58
        '
        'Label_Select_ModeRc
        '
        Me.Label_Select_ModeRc.AutoSize = True
        Me.Label_Select_ModeRc.Location = New System.Drawing.Point(12, 126)
        Me.Label_Select_ModeRc.Name = "Label_Select_ModeRc"
        Me.Label_Select_ModeRc.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label_Select_ModeRc.Size = New System.Drawing.Size(68, 13)
        Me.Label_Select_ModeRc.TabIndex = 59
        Me.Label_Select_ModeRc.Text = "Mode Radio:"
        Me.Label_Select_ModeRc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxSignalType
        '
        Me.ComboBoxSignalType.FormattingEnabled = True
        Me.ComboBoxSignalType.Items.AddRange(New Object() {"CPPM", "SBUS", "IBUS"})
        Me.ComboBoxSignalType.Location = New System.Drawing.Point(91, 211)
        Me.ComboBoxSignalType.Name = "ComboBoxSignalType"
        Me.ComboBoxSignalType.Size = New System.Drawing.Size(96, 21)
        Me.ComboBoxSignalType.TabIndex = 60
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 214)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 61
        Me.Label1.Text = "Signal Type:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RcRadioMode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(538, 355)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBoxSignalType)
        Me.Controls.Add(Me.Label_Select_ModeRc)
        Me.Controls.Add(Me.ComboBox_Select_ModeType)
        Me.Controls.Add(Me.PictureBoxMode)
        Me.Controls.Add(Me.labelRcRadioMode)
        Me.Name = "RcRadioMode"
        Me.Text = "RcRadioMode"
        CType(Me.PictureBoxMode, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents labelRcRadioMode As System.Windows.Forms.Label
    Friend WithEvents PictureBoxMode As System.Windows.Forms.PictureBox
    Friend WithEvents ComboBox_Select_ModeType As System.Windows.Forms.ComboBox
    Friend WithEvents Label_Select_ModeRc As System.Windows.Forms.Label
    Friend WithEvents ComboBoxSignalType As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class
