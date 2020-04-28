Imports GdiGaming

Public Class TitleScene
    Inherits GameScene

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Title"
        End Get
    End Property

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)

        Dim hud As New HudLayer
        Dim background As New TitleBackground
        hud.Images.Add(background)
        Dim title As New TitleLabel
        hud.Labels.Add(title)
        Dim startButton As New TitleStartButton
        hud.Labels.Add(startButton)
        HudLayers.Add(hud)
    End Sub
End Class

Public Class TitleBackground
    Inherits HudImage

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)
        Dim backImage As Bitmap = e.Engine.CurrentScene.GetImageAsset("Clouds.png")

        Dim spriteLayer1 As New Sprite
        spriteLayer1.SpriteSheetName = "Clouds.png"
        spriteLayer1.SetFrameBounds(New Rectangle(Point.Empty, backImage.Size))

        Sprites.Add(spriteLayer1)

        Position = New Vector2(backImage.Width / 2, backImage.Height / 2)
    End Sub

    Protected Overrides Sub OnDraw(e As GdiGaming.DrawEventArgs)
        MyBase.OnDraw(e)
        Dim brsh As New System.Drawing.Drawing2D.HatchBrush(Drawing2D.HatchStyle.LightHorizontal, Color.FromArgb(48, Color.WhiteSmoke), Color.FromArgb(48, Color.DarkGray))
        e.PaintEventArgs.Graphics.FillRectangle(brsh, e.PaintEventArgs.ClipRectangle)
        brsh.Dispose()
    End Sub
End Class

Public Class TitleStartButton
    Inherits HudLabel

    Private _AlphaDelta As Integer = 5
    Private _AlphaStep As Integer = 5

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)
        Me.ProcessInput = True
        Me.Text = "  Start  "
        Me.FontSize = 48
        Me.BackColor = Color.DarkOrange
        Me.TextColor = Color.DeepSkyBlue
        Me.ZOrder = 10
        'Me.UseCenterPositioning = False
        'Me.AutoSize = False
        'Me.ProposedSize = New Size(235, 95)
        OnUpdate(e)
        'Me.Position = New Vector2(e.Engine.Canvas.Width / 2 - Me.ActualSize.Width / 2, e.Engine.Canvas.Height / 2 - Me.ActualSize.Height / 2)
    End Sub

    Protected Overrides Sub OnDraw(e As GdiGaming.DrawEventArgs)
        MyBase.OnDraw(e)
        Dim brsh As New SolidBrush(Color.FromArgb(_AlphaDelta, Color.DarkGray))
        e.PaintEventArgs.Graphics.FillRectangle(brsh, Me.GetRectangleCollider)
        brsh.Dispose()
    End Sub

    Protected Overrides Sub OnInput(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnInput(e)
        If e.Engine.Input.WasKeyPressed(Keys.Space) Then
            StartNewGame(e.Engine)
        End If
    End Sub

    Protected Overrides Sub OnMouseUp(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnMouseUp(e)
        StartNewGame(e.Engine)
    End Sub

    Protected Overrides Sub OnUpdate(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        _AlphaDelta += _AlphaStep
        If _AlphaDelta = 140 Then
            _AlphaStep = -5
        ElseIf _AlphaDelta = 5 Then
            _AlphaStep = 5
        End If
    End Sub

    Private Sub StartNewGame(engine As GameEngine)
        engine.CurrentSceneName = "Example Scene 1"
    End Sub
End Class

Public Class TitleLabel
    Inherits HudLabel

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)
        Me.Text = "Chopper Game!"
        Me.FontName = "Curlz MT"
        Me.FontSize = 60
        Me.FontStyle = Drawing.FontStyle.Bold
        Me.TextColor = Color.Orange
        Me.Position -= New Vector2(0.0F, e.Engine.Canvas.Height / 3)
        Me.ZOrder = 20
    End Sub
End Class