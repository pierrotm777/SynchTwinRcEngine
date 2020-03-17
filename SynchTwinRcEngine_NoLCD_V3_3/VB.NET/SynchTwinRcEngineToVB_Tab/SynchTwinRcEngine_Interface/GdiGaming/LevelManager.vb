Imports GdiGaming

Public Class LevelManager
    Inherits GameObject

    Private _Scene As ExampleScene
    Private _Random As New Random

    Private _ShowLevelTime As Single
    Private _IsLevelReady As Boolean
    Private _LevelText As String
    Private _LevelFont As Font
    Private _TextBounds As Rectangle

    Private _Level As Integer
    Public ReadOnly Property Level As Integer
        Get
            Return _Level
        End Get
    End Property

    Public Sub New(ByVal targetScene As ExampleScene)
        _IsLevelReady = True
        _Scene = targetScene
        _ShowLevelTime = -1.0F
        _ZOrder = 1000
        _LevelFont = New Font("Segoe UI", 32, FontStyle.Bold)
    End Sub

    Protected Sub SetupLevel()
        _IsLevelReady = False
        _Level += 1
        _ShowLevelTime = 2
        _LevelText = "LEVEL " & _Level.ToString
        Dim textSize As Size = TextRenderer.MeasureText(_LevelText, _LevelFont)
        _TextBounds = New Rectangle(_Scene.Engine.Canvas.DisplayRectangle.Width / 2 - textSize.Width / 2, _Scene.Engine.Canvas.DisplayRectangle.Height / 2 - textSize.Height / 2, textSize.Width, textSize.Height)
    End Sub

    Protected Overrides Sub OnDraw(ByVal e As GdiGaming.DrawEventArgs)
        MyBase.OnDraw(e)
        If _ShowLevelTime > 0.0F Then
            TextRenderer.DrawText(e.PaintEventArgs.Graphics, _LevelText, _LevelFont, _TextBounds, Color.Orange)
        End If
    End Sub

    Protected Overrides Sub OnUpdate(ByVal e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        Dim scene As ExampleScene = CType(e.Engine.CurrentScene, ExampleScene)

        If _Scene.Enemies.Count = 0 Then
            If _IsLevelReady Then
                SetupLevel()
            Else
                If _ShowLevelTime > 0.0F Then
                    _ShowLevelTime -= e.Time.LastFrame
                Else
                    SpawnShips()
                    _IsLevelReady = True
                End If
            End If
        End If
    End Sub

    Protected Sub SpawnShips()
        Dim x, y As Single
        For i As Integer = 1 To 10 + (2 * (_Level - 1))
            x = _Scene.Engine.Canvas.DisplayRectangle.Width * _Random.NextDouble
            y = _Random.Next(-150, -51)

            Dim ship As New EnemySpaceship
            ship.Speed += (_Level * 5)
            ship.Position = New Vector2(x, y)
            _Scene.Objects.Add(ship)
            _Scene.Enemies.Add(ship)
        Next
    End Sub
End Class
