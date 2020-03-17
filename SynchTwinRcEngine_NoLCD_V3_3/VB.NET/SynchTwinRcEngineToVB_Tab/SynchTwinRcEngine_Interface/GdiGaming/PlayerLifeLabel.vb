Imports GdiGaming

Public Class PlayerLifeLabel
    Inherits HudLabel

    Protected _Player As PlayerChopper
    Public ReadOnly Property Player As PlayerChopper
        Get
            Return _Player
        End Get
    End Property

    Public Sub New(ByVal target As PlayerChopper)
        FontSize = 12
        FontStyle = Drawing.FontStyle.Bold
        TextColor = Color.Orange
        BackColor = Color.FromArgb(128, Color.Gray)
        _Player = target
    End Sub

    Protected Overrides Sub OnUpdate(ByVal e As GdiGaming.GameEngineEventArgs)
        Text = "  LIFE - " & _Player.HitsRemaining.ToString & "  "
        MyBase.OnUpdate(e)
    End Sub
End Class
