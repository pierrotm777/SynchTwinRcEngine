﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.ComboBox_Select_ChannelsOrder = New System.Windows.Forms.ComboBox()
        Me.label_Select_ChannelOrder = New System.Windows.Forms.Label()
        Me.ComboBoxSignalType = New System.Windows.Forms.ComboBox()
        Me.labelInputFormat = New System.Windows.Forms.Label()
        Me.labelOrderChannels = New System.Windows.Forms.Label()
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
        'ComboBox_Select_ChannelsOrder
        '
        Me.ComboBox_Select_ChannelsOrder.FormattingEnabled = True
        Me.ComboBox_Select_ChannelsOrder.Items.AddRange(New Object() {"AETR", "AERT", "ARET", "ARTE", "ATRE", "ATER", "EATR", "EART", "ERAT", "ERTA", "ETRA", "ETAR", "TEAR", "TERA", "TREA", "TRAE", "TARE", "TAER", "RETA", "REAT", "RAET", "RATE", "RTAE", "RTEA"})
        Me.ComboBox_Select_ChannelsOrder.Location = New System.Drawing.Point(91, 74)
        Me.ComboBox_Select_ChannelsOrder.Name = "ComboBox_Select_ChannelsOrder"
        Me.ComboBox_Select_ChannelsOrder.Size = New System.Drawing.Size(96, 21)
        Me.ComboBox_Select_ChannelsOrder.TabIndex = 58
        '
        'label_Select_ChannelOrder
        '
        Me.label_Select_ChannelOrder.AutoSize = True
        Me.label_Select_ChannelOrder.Location = New System.Drawing.Point(12, 77)
        Me.label_Select_ChannelOrder.Name = "label_Select_ChannelOrder"
        Me.label_Select_ChannelOrder.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.label_Select_ChannelOrder.Size = New System.Drawing.Size(74, 13)
        Me.label_Select_ChannelOrder.TabIndex = 59
        Me.label_Select_ChannelOrder.Text = "Ordre canaux:"
        Me.label_Select_ChannelOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ComboBoxSignalType
        '
        Me.ComboBoxSignalType.FormattingEnabled = True
        Me.ComboBoxSignalType.Items.AddRange(New Object() {"CPPM", "SBUS", "SRXL", "SUMD", "IBUS", "JETI"})
        Me.ComboBoxSignalType.Location = New System.Drawing.Point(91, 211)
        Me.ComboBoxSignalType.Name = "ComboBoxSignalType"
        Me.ComboBoxSignalType.Size = New System.Drawing.Size(96, 21)
        Me.ComboBoxSignalType.TabIndex = 60
        '
        'labelInputFormat
        '
        Me.labelInputFormat.AutoSize = True
        Me.labelInputFormat.Location = New System.Drawing.Point(12, 214)
        Me.labelInputFormat.Name = "labelInputFormat"
        Me.labelInputFormat.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.labelInputFormat.Size = New System.Drawing.Size(66, 13)
        Me.labelInputFormat.TabIndex = 61
        Me.labelInputFormat.Text = "Signal Type:"
        Me.labelInputFormat.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'labelOrderChannels
        '
        Me.labelOrderChannels.AutoSize = True
        Me.labelOrderChannels.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelOrderChannels.Location = New System.Drawing.Point(198, 77)
        Me.labelOrderChannels.Name = "labelOrderChannels"
        Me.labelOrderChannels.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.labelOrderChannels.Size = New System.Drawing.Size(40, 13)
        Me.labelOrderChannels.TabIndex = 62
        Me.labelOrderChannels.Text = "AETR"
        Me.labelOrderChannels.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RcRadioMode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(538, 355)
        Me.Controls.Add(Me.labelOrderChannels)
        Me.Controls.Add(Me.labelInputFormat)
        Me.Controls.Add(Me.ComboBoxSignalType)
        Me.Controls.Add(Me.label_Select_ChannelOrder)
        Me.Controls.Add(Me.ComboBox_Select_ChannelsOrder)
        Me.Controls.Add(Me.labelRcRadioMode)
        Me.Name = "RcRadioMode"
        Me.Text = "RcRadioMode"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents labelRcRadioMode As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Select_ChannelsOrder As System.Windows.Forms.ComboBox
    Friend WithEvents label_Select_ChannelOrder As System.Windows.Forms.Label
    Friend WithEvents ComboBoxSignalType As System.Windows.Forms.ComboBox
    Friend WithEvents labelInputFormat As System.Windows.Forms.Label
    Friend WithEvents labelOrderChannels As System.Windows.Forms.Label
End Class
