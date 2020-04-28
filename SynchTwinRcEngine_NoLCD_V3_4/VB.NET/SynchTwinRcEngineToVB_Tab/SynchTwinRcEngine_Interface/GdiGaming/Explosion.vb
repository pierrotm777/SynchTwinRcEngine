Imports GdiGaming

Public Class Explosion
    Inherits GameObject

    Private _ElapsedTime As Single
    Private _DisplayTime As Single = 0.5F

    Public Sub New()
        Dim explosion As New AnimatedSprite
        explosion.SpriteSheetName = "Explosion"
        explosion.AnimationRate = _DisplayTime
        explosion.AddFramesHV(4, 4, 48, 48)
        explosion.Name = "Explosion"
        Sprites.Add(explosion)
        _ZOrder = 3
    End Sub

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)
        e.Engine.Audio.Play("Audio_002", False)
    End Sub

    Protected Overrides Sub OnUpdate(ByVal e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        If _ElapsedTime >= _DisplayTime Then
            e.Engine.CurrentScene.Objects.Remove(Me)
        End If
        _ElapsedTime += e.Time.LastFrame
    End Sub
End Class
