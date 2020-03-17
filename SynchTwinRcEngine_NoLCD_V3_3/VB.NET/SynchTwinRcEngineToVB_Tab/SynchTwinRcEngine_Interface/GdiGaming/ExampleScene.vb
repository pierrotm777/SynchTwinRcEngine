Imports GdiGaming

Public Class ExampleScene
    Inherits GameScene

    Private _LevelManager As LevelManager
    Public ReadOnly Property LevelManager As LevelManager
        Get
            Return _LevelManager
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Example Scene 1"
        End Get
    End Property

    Protected _Enemies As New List(Of EnemySpaceship)
    Public ReadOnly Property Enemies As List(Of EnemySpaceship)
        Get
            Return _Enemies
        End Get
    End Property

    Protected _Player As PlayerChopper
    Public ReadOnly Property Player As PlayerChopper
        Get
            Return _Player
        End Get
    End Property

    Protected Overrides Sub OnLoad(ByVal e As GdiGaming.GameEngineEventArgs)
        Backgrounds.Add(New CloudyBackground)

        _Player = New PlayerChopper
        _Player.Position = New Vector2(320, 400)
        Objects.Add(_Player)

        e.Engine.Resources.SetIntegerAsset("PlayerScore", 100)

        Dim mainHud As New HudLayer
        HudLayers.Add(mainHud)

        Dim lifeLabel As New PlayerLifeLabel(_Player)
        lifeLabel.Position -= New Vector2(0, e.Engine.Canvas.Height / 2 * 0.9)
        mainHud.Labels.Add(lifeLabel)

        Dim scoreLabel As New ScoreLabel
        scoreLabel.Position = lifeLabel.Position + New Vector2(0, 32)
        mainHud.Labels.Add(scoreLabel)

        _LevelManager = New LevelManager(Me)
        Objects.Add(_LevelManager)

        e.Engine.Audio.Play("NewSong2", True)
        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub OnUpdate(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        If e.Engine.Input.WasKeyPressed(Keys.P) Then
            e.Engine.CurrentSceneName = "Pause"
        End If
        If e.Engine.Audio.IsPaused Then
            e.Engine.Audio.ResumeAll()
        End If
    End Sub
End Class
