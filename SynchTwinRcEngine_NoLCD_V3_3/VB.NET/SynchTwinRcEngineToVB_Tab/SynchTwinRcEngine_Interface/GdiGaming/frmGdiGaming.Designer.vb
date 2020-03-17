<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGdiGaming
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
        Me.GameEngine1 = New GdiGaming.GameEngine(Me.components)
        Me.RenderCanvas1 = New GdiGaming.RenderCanvas()
        Me.SuspendLayout()
        '
        'GameEngine1
        '
        Me.GameEngine1.Canvas = Me.RenderCanvas1
        Me.GameEngine1.ContainerControl = Me
        Me.GameEngine1.CurrentSceneName = ""
        Me.GameEngine1.TargetFrameRate = 30.0!
        '
        'RenderCanvas1
        '
        Me.RenderCanvas1.BackColor = System.Drawing.Color.CornflowerBlue
        Me.RenderCanvas1.Location = New System.Drawing.Point(0, 0)
        Me.RenderCanvas1.Name = "RenderCanvas1"
        Me.RenderCanvas1.Size = New System.Drawing.Size(640, 480)
        Me.RenderCanvas1.TabIndex = 0
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(640, 479)
        Me.Controls.Add(Me.RenderCanvas1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GameEngine1 As GdiGaming.GameEngine
    Friend WithEvents RenderCanvas1 As GdiGaming.RenderCanvas

End Class
