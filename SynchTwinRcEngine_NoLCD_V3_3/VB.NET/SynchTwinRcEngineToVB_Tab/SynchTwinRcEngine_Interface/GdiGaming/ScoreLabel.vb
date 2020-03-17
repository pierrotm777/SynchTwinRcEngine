Imports GdiGaming

Public Class ScoreLabel
    Inherits HudLabel

    Protected Overrides Sub OnLoad(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnLoad(e)
        FontSize = 12
        FontStyle = Drawing.FontStyle.Bold
        TextColor = Color.Orange
        BackColor = Color.FromArgb(128, Color.Gray)
    End Sub

    Protected Overrides Sub OnUpdate(e As GdiGaming.GameEngineEventArgs)
        MyBase.OnUpdate(e)
        Dim score As Integer = e.Engine.Resources.GetIntegerAsset("PlayerScore")
        Text = "Score: " & score.ToString
    End Sub
End Class
