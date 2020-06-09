Imports System.ComponentModel
Imports System.Text

Public Class RcRadioMode

    Private Sub RcRadioMode_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox_Select_ChannelsOrder.SelectedIndex = Form1.labelChannelOrderRadio.Text
        ComboBoxSignalType.Text = Form1.LabelSignalType.Text
    End Sub


    Private Sub ComboBox_Select_ModeType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_Select_ChannelsOrder.SelectedIndexChanged
        'AILERON = 0, ELEVATOR, THROTTLE, RUDDER
        Dim sb As New StringBuilder()
        For Each c As Char In ComboBox_Select_ChannelsOrder.SelectedItem
            Select Case c
                Case "A"
                    sb.Append("AILERON ")
                Case "E"
                    sb.Append("ELEVATOR ")
                Case "T"
                    sb.Append("THROTTLE ")
                Case "R"
                    sb.Append("RUDDER ")
            End Select

        Next
        sb.Length = sb.Length - 1 ''Remove last space on string
        labelOrderChannels.Text = sb.ToString()

        Form1.RcModeRadio = ComboBox_Select_ChannelsOrder.SelectedIndex
        Form1.labelChannelOrderRadio.Text = ComboBox_Select_ChannelsOrder.SelectedIndex


        'Modify the channel order based on your TX: AETR, TAER, RETA...
        'Examples: Flysky & DEVO is AETR, JR/Spektrum radio is TAER, Multiplex is AERT...
        'Default is AETR.
        '#define TAER
        'And for my Older Kraft TX then it would be
        '#defineEATR

    End Sub

    Private Sub ComboBoxSignalType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBoxSignalType.SelectedIndexChanged
        Form1.LabelSignalType.Text = ComboBoxSignalType.Text
    End Sub
    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As CancelEventArgs) Handles MyBase.Closing
        Me.Visible = False
        e.Cancel = True
    End Sub


End Class