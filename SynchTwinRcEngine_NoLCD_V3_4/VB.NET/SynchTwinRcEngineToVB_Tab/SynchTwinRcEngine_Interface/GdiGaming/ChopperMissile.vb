Imports GdiGaming

Public Class ChopperMissile
    Inherits GameObject

    Private _Direction As Vector2

    Public Sub New(ByVal direction As Vector2, ByVal rotation As Single)
        _ZOrder = 1
        _Direction = direction
        _RotationAngle = rotation
    End Sub

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)
        Dim missileSprite As New AnimatedSprite
        missileSprite.SpriteSheetName = "AirVehicles"
        missileSprite.AnimationRate = 0.25F
        missileSprite.AddFramesHV(1, 4, 48, 48, 2)
        missileSprite.Name = "Missile"
        Sprites.Add(missileSprite)
        IsCollider = True
        CollisionRadius = 3
        e.Engine.Resources.ModifyIntegerAsset("PlayerScore", CType(e.Engine.CurrentScene, ExampleScene).LevelManager.Level * -5)
    End Sub

    Protected Overrides Sub OnUpdate(ByVal e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        Position += _Direction
        Dim bounds As Rectangle = e.Engine.Canvas.DisplayRectangle
        If Not bounds.Contains(Position.ToPoint) Then
            e.Engine.CurrentScene.Objects.Remove(Me)
        End If
    End Sub
End Class
