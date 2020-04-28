Imports GdiGaming

Public Class PlayerChopper
    Inherits GameObject

    Private _BlinkTime As Single
    Private _FireWait As Single
    Private _FramesHidden As Integer
    Private _NewPosition As Vector2

    Private _FireRate As Single
    Public Property FireRate() As Single
        Get
            Return _FireRate
        End Get
        Set(ByVal value As Single)
            _FireRate = value
        End Set
    End Property

    Private _HitsRemaining As Integer
    Public Property HitsRemaining() As Integer
        Get
            Return _HitsRemaining
        End Get
        Set(ByVal value As Integer)
            _HitsRemaining = value
        End Set
    End Property

    Private _Speed As Single = 96.0F
    Public Property Speed() As Single
        Get
            Return _Speed
        End Get
        Set(ByVal value As Single)
            _Speed = value
        End Set
    End Property

    Public Sub New()
        HasCollision = True
        CollisionRadius = 14
        _Name = "Player"
        _ProcessInput = True
        _ZOrder = 1
        _HitsRemaining = 10
        _FireRate = 3.0

    End Sub

    Protected Overrides Sub OnCollision(ByVal e As GdiGaming.CollisionEventArgs)
        MyBase.OnCollision(e)
        If _BlinkTime = 0.0F Then
            If TypeOf e.CollisionObject Is EnemySpaceship Then
                Dim spaceShip As EnemySpaceship = DirectCast(e.CollisionObject, EnemySpaceship)
                spaceShip.Position -= Vector2.GetVectorToward(spaceShip.Position, Position, CollisionRadius)
                _BlinkTime = 2.0F
                _HitsRemaining -= 1
            End If
        End If
    End Sub

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)
        Dim chopperSprite As New AnimatedSprite
        chopperSprite.SpriteSheetName = "AirVehicles"
        chopperSprite.AnimationRate = 0.25F
        chopperSprite.AddFramesHV(1, 4, 48, 48)
        chopperSprite.Name = "Chopper"
        Sprites.Add(chopperSprite)
    End Sub

    Protected Overrides Sub OnInput(ByVal e As GdiGaming.GameEngineEventArgs)
        MyBase.OnInput(e)
        Dim input As GameInput = e.Engine.Input
        _RotationAngle = 0.0F
        _NewPosition = Vector2.Empty
        If input.IsKeyDown(Keys.W) OrElse input.IsKeyDown(Keys.Up) Then
            _NewPosition += New Vector2(0, -_Speed * e.Time.LastFrame)
            _RotationAngle = 0.0F
        End If
        If input.IsKeyDown(Keys.S) OrElse input.IsKeyDown(Keys.Down) Then
            _NewPosition += New Vector2(0, _Speed * e.Time.LastFrame)
            _RotationAngle = 180.0F
        End If
        If input.IsKeyDown(Keys.A) OrElse input.IsKeyDown(Keys.Left) Then
            _NewPosition += New Vector2(-_Speed * e.Time.LastFrame, 0)
            If input.IsKeyDown(Keys.W) OrElse input.IsKeyDown(Keys.Up) Then
                _RotationAngle = 315.0F
            ElseIf input.IsKeyDown(Keys.S) OrElse input.IsKeyDown(Keys.Down) Then
                _RotationAngle = 225.0F
            Else
                _RotationAngle = 270.0F
            End If
        End If
        If input.IsKeyDown(Keys.D) OrElse input.IsKeyDown(Keys.Right) Then
            _NewPosition += New Vector2(_Speed * e.Time.LastFrame, 0)
            If input.IsKeyDown(Keys.W) OrElse input.IsKeyDown(Keys.Up) Then
                _RotationAngle = 45.0F
            ElseIf input.IsKeyDown(Keys.S) OrElse input.IsKeyDown(Keys.Down) Then
                _RotationAngle = 135.0F
            Else
                _RotationAngle = 90.0F
            End If
        End If
        If input.WasKeyPressed(Keys.Space) Then
            If _FireWait = 0.0F Then
                _FireWait = 1.0F / _FireRate
                FireMissile(e.Engine)
            End If
        End If
    End Sub

    Protected Overrides Sub OnUpdate(ByVal e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        Dim bounds As Rectangle = e.Engine.Canvas.DisplayRectangle
        bounds.Inflate(-24, -24)
        Dim deltaPosition As Vector2 = Position + _NewPosition
        If bounds.Contains(deltaPosition.ToPoint) Then
            Position = deltaPosition
        End If
        If _HitsRemaining < 1 Then
            e.Engine.CurrentScene.Objects.Remove(Me)
            Dim boom As New Explosion
            boom.Position = Position
            e.Engine.CurrentScene.Objects.Add(boom)
        End If
        If _BlinkTime > 0 Then
            If _FramesHidden = 0 Then
                _Visible = Not _Visible
                _FramesHidden = 5
            End If
            _FramesHidden -= 1
            _BlinkTime -= e.Time.LastFrame
        Else
            _BlinkTime = 0.0F
            _FramesHidden = 0
            _Visible = True
        End If
        If _FireWait > 0 Then
            _FireWait -= e.Time.LastFrame
        Else
            _FireWait = 0.0F
        End If
    End Sub

    Private Sub FireMissile(ByVal engine As GameEngine)
        Dim missileHeading As Vector2 = _NewPosition
        If missileHeading = Vector2.Empty Then
            missileHeading = New Vector2(0, -_Speed * engine.Time.LastFrame)
        End If
        missileHeading *= 1.5F
        Dim missile As New ChopperMissile(missileHeading, _RotationAngle)
        missile.Position = Position
        engine.CurrentScene.Objects.Add(missile)
    End Sub
End Class
