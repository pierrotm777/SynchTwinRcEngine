Imports System.ComponentModel

Public Class Terminal

    Private Sub Terminal_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles MyBase.Closing
        'Me.WindowState = FormWindowState.Maximized
        Me.Visible = False
        e.Cancel = True
    End Sub

End Class