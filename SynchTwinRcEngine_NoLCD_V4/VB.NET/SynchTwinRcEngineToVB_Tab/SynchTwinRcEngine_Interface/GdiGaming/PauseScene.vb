Imports GdiGaming

Public Class PauseScene
    Inherits GameScene

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Pause"
        End Get
    End Property

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        Dim hud As New HudLayer
        Dim pauseLabel As New HudLabel
        pauseLabel.Text = "Pause"
        pauseLabel.FontName = "Segoui UI"
        pauseLabel.FontSize = 48
        pauseLabel.FontStyle = FontStyle.Bold
        pauseLabel.TextColor = Color.Orange
        hud.Labels.Add(pauseLabel)
        HudLayers.Add(hud)
        MyBase.OnLoad(e)
    End Sub

    Protected Overrides Sub OnUpdate(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        If Not e.Engine.Audio.IsPaused Then
            e.Engine.Audio.PauseAll()
        End If
        If e.Engine.Input.WasKeyPressed(Keys.P) Then
            e.Engine.CurrentSceneName = "Example Scene 1"
        End If
    End Sub
End Class
