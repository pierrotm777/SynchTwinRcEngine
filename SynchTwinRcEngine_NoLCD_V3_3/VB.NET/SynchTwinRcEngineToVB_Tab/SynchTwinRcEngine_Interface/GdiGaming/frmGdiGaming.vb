Imports GdiGaming

Public Class frmGdiGaming
    Private Sub Form1_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        GameEngine1.StartGame()
        If GameEngine1.CurrentSceneName = "Pause" Then
            GameEngine1.CurrentSceneName = "Example Scene 1"
        End If
    End Sub

    Private Sub Form1_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        If GameEngine1.CurrentSceneName = "Example Scene 1" Then
            GameEngine1.CurrentSceneName = "Pause"
        End If
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        If Not GameEngine1.EngineState = GdiGaming.GameEngineState.Stopped Then
            GameEngine1.EndGame()
            e.Cancel = True
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Add the game's initial scene to the engine; replace "ExampleScene" with your custom scene
        GameEngine1.Scenes.Add(New TitleScene)
        GameEngine1.Scenes.Add(New ExampleScene)
        GameEngine1.Scenes.Add(New PauseScene)
    End Sub

    Private Sub GameEngine1_FrameComplete(ByVal sender As Object, ByVal e As GdiGaming.GameEngineEventArgs) Handles GameEngine1.FrameComplete
        'Uncomment the following line to easily monitor the framerate during development:
        Me.Text = e.Engine.FrameRate.ToString & " " & e.Engine.CurrentSceneName
    End Sub

    Private Sub GameEngine1_GameStopped(ByVal sender As Object, ByVal e As System.EventArgs) Handles GameEngine1.GameStopped
        Me.Close()
    End Sub
End Class



