Imports GdiGaming

Public Class CloudyBackground
    Inherits ScrollingBackground

    Public Sub New()
        _AutoScroll = True
        _BackgroundImage = "Clouds"
        _InvertDirection = True
        _ScrollDirection = Orientation.Vertical
        _ScrollSpeed = 96.0F
    End Sub
End Class
