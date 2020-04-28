Imports GdiGaming

Public Class EnemySpaceship
    Inherits GameObject

    Private Shared _Random As New Random

    Private _Destination As Vector2

    Private _Speed As Single = 72.0F
    Public Property Speed() As Single
        Get
            Return _Speed
        End Get
        Set(ByVal value As Single)
            _Speed = value
        End Set
    End Property

    Public Sub New()
        Dim spaceshipSprite As New AnimatedSprite
        spaceshipSprite.SpriteSheetName = "AirVehicles"
        spaceshipSprite.AnimationRate = 0.75F
        spaceshipSprite.AddFramesHV(1, 4, 48, 48, 1)
        spaceshipSprite.Name = "Spaceship"
        Sprites.Add(spaceshipSprite)
        HasCollision = True
        CollisionRadius = 24
        _ZOrder = 2
    End Sub

    Protected Overrides Sub OnCollision(ByVal e As GdiGaming.CollisionEventArgs)
        MyBase.OnCollision(e)
        If TypeOf e.CollisionObject Is ChopperMissile Then
            Dim scene As ExampleScene = CType(e.Engine.CurrentScene, ExampleScene)
            scene.Enemies.Remove(Me)
            scene.Objects.Remove(Me)
            scene.Objects.Remove(e.CollisionObject)
            Dim boom As New Explosion
            boom.Position = Position
            scene.Objects.Add(boom)
            Dim killScore As Integer = 200 + (CType(e.Engine.CurrentScene, ExampleScene).LevelManager.Level - 1) * 50
            Dim playerScore As Integer = e.Engine.Resources.GetIntegerAsset("PlayerScore")
            playerScore += killScore
            e.Engine.Resources.SetIntegerAsset("PlayerScore", playerScore)
            Dim popup As New PopupLabel
            popup.Position = Position
            popup.Text = "+" & killScore.ToString
            scene.HudLayers(0).Labels.Add(popup)
        End If
        If TypeOf e.CollisionObject Is EnemySpaceship Then
            Position = Vector2.GetVectorToward(Position, e.CollisionObject.Position, -_Speed * e.Time.LastFrame)
            _Destination = GetRandomDestination(e.Engine.Canvas.DisplayRectangle.Size)
        End If
    End Sub

    Protected Overrides Sub OnLoad(ByVal e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)
        _Destination = GetRandomDestination(e.Engine.Canvas.DisplayRectangle.Size)
    End Sub

    Protected Overrides Sub OnUpdate(ByVal e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        If Position.DistanceTo(_Destination) < CollisionRadius Then
            _Destination = GetRandomDestination(e.Engine.Canvas.DisplayRectangle.Size)
        End If
        Position = Vector2.GetVectorToward(Position, _Destination, _Speed * e.Time.LastFrame)
    End Sub

    Private Function GetRandomDestination(ByVal area As Size) As Vector2
        Return New Vector2((area.Width - CollisionRadius * 2) * _Random.NextDouble, (area.Height - CollisionRadius * 2) * _Random.NextDouble)
    End Function
End Class
