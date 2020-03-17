<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ShowMsgForm
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
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextLabel = New System.Windows.Forms.Label()
        Me.MessagePictureBox = New System.Windows.Forms.PictureBox()
        Me.QuestionTextLabel = New System.Windows.Forms.Label()
        Me.ClickLabel = New System.Windows.Forms.LinkLabel()
        Me.LabelMailTo = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        CType(Me.MessagePictureBox, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(48, 80)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(71, 23)
        Me.Button3.TabIndex = 23
        Me.Button3.Text = "Button3"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(120, 80)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(71, 23)
        Me.Button2.TabIndex = 21
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(192, 80)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(71, 23)
        Me.Button1.TabIndex = 20
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextLabel
        '
        Me.TextLabel.AutoSize = True
        Me.TextLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextLabel.Location = New System.Drawing.Point(56, 8)
        Me.TextLabel.MaximumSize = New System.Drawing.Size(600, 0)
        Me.TextLabel.Name = "TextLabel"
        Me.TextLabel.Size = New System.Drawing.Size(30, 13)
        Me.TextLabel.TabIndex = 19
        Me.TextLabel.Text = "Main"
        Me.TextLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'MessagePictureBox
        '
        Me.MessagePictureBox.Location = New System.Drawing.Point(4, 4)
        Me.MessagePictureBox.Name = "MessagePictureBox"
        Me.MessagePictureBox.Size = New System.Drawing.Size(48, 48)
        Me.MessagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.MessagePictureBox.TabIndex = 18
        Me.MessagePictureBox.TabStop = False
        '
        'QuestionTextLabel
        '
        Me.QuestionTextLabel.AutoSize = True
        Me.QuestionTextLabel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.QuestionTextLabel.Location = New System.Drawing.Point(56, 36)
        Me.QuestionTextLabel.MaximumSize = New System.Drawing.Size(450, 0)
        Me.QuestionTextLabel.Name = "QuestionTextLabel"
        Me.QuestionTextLabel.Size = New System.Drawing.Size(49, 13)
        Me.QuestionTextLabel.TabIndex = 22
        Me.QuestionTextLabel.Text = "Question"
        '
        'ClickLabel
        '
        Me.ClickLabel.AutoSize = True
        Me.ClickLabel.Location = New System.Drawing.Point(93, 64)
        Me.ClickLabel.Name = "ClickLabel"
        Me.ClickLabel.Size = New System.Drawing.Size(120, 13)
        Me.ClickLabel.TabIndex = 137
        Me.ClickLabel.TabStop = True
        Me.ClickLabel.Text = "pierrotm777@gmail.com"
        '
        'LabelMailTo
        '
        Me.LabelMailTo.AutoSize = True
        Me.LabelMailTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LabelMailTo.Location = New System.Drawing.Point(56, 64)
        Me.LabelMailTo.MaximumSize = New System.Drawing.Size(450, 0)
        Me.LabelMailTo.Name = "LabelMailTo"
        Me.LabelMailTo.Size = New System.Drawing.Size(37, 13)
        Me.LabelMailTo.TabIndex = 138
        Me.LabelMailTo.Text = "mailto:"
        '
        'Timer1
        '
        Me.Timer1.Enabled = True
        Me.Timer1.Interval = 3000
        '
        'ShowMsgForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(265, 106)
        Me.Controls.Add(Me.LabelMailTo)
        Me.Controls.Add(Me.ClickLabel)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.QuestionTextLabel)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextLabel)
        Me.Controls.Add(Me.MessagePictureBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ShowMsgForm"
        CType(Me.MessagePictureBox, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextLabel As System.Windows.Forms.Label
    Friend WithEvents MessagePictureBox As System.Windows.Forms.PictureBox
    Friend WithEvents QuestionTextLabel As System.Windows.Forms.Label
    Friend WithEvents ClickLabel As System.Windows.Forms.LinkLabel
    Friend WithEvents LabelMailTo As System.Windows.Forms.Label
    Public WithEvents Timer1 As System.Windows.Forms.Timer

End Class
