Imports System.ComponentModel

Public Class RcRadioMode

    Private Sub ComboBox_Select_Trottle_Channel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Width = 620
        Me.Height = 430

        ComboBox_Select_ModeType.SelectedIndex = Form1.RcModeRadio
        ComboBoxSignalType.SelectedIndex = Form1.RcSignalType


        Select Case ComboBox_Select_ModeType.SelectedIndex
            Case 0
                PictureBoxMode.Image = My.Resources.mode1
            Case 1
                PictureBoxMode.Image = My.Resources.mode2
            Case 2
                PictureBoxMode.Image = My.Resources.mode3
            Case 3
                PictureBoxMode.Image = My.Resources.mode4
        End Select


    End Sub

    Private Sub ComboBox_Select_ModeType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Select_ModeType.SelectedIndexChanged

        Select Case ComboBox_Select_ModeType.SelectedIndex
            Case 0
                PictureBoxMode.Image = My.Resources.mode1
            Case 1
                PictureBoxMode.Image = My.Resources.mode2
            Case 2
                PictureBoxMode.Image = My.Resources.mode3
            Case 3
                PictureBoxMode.Image = My.Resources.mode4
        End Select

        Form1.RcModeRadio = ComboBox_Select_ModeType.SelectedIndex
        Form1.labelModeRcRadio.Text = ComboBox_Select_ModeType.SelectedIndex + 1

    End Sub

    Private Sub ComboBoxSignalType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxSignalType.SelectedIndexChanged
        Form1.LabelSignalType.Text = ComboBoxSignalType.Text
    End Sub
    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As CancelEventArgs) Handles MyBase.Closing
        Me.Visible = False
        e.Cancel = True
    End Sub


End Class