Imports GdiGaming

Public Class PopupLabel
    Inherits HudLabel

    Public Property FloatSpeed As Single = 18.0F
    Public Property DisplayTime As Single = 1.5F

    Private _LoadTime As Single

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)
        FontSize = 12
        FontStyle = Drawing.FontStyle.Bold
        TextColor = Color.Purple
        UseCenterPositioning = False
        _LoadTime = e.Time.SecondsElapsed
    End Sub

    Protected Overrides Sub OnUpdate(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        Position -= New Vector2(0, FloatSpeed * e.Time.LastFrame)
        If TextColor.A > 5 Then
            TextColor = Color.FromArgb(TextColor.A - 5, TextColor)
        End If
        If e.Time.SecondsElapsed - _LoadTime > DisplayTime Then
            e.Engine.CurrentScene.HudLayers(0).Labels.Remove(Me)
        End If
    End Sub
End Class
