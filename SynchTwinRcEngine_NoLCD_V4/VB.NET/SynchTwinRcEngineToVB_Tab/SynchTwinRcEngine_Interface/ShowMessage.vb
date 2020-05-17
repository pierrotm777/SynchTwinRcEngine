Imports System.Text

Public Module ShowMessage

#Region " Default values "

    Public DefaultTitle As String = "ShowMsg Demo"

#End Region

#Region " ShowMsg methods - message only "

    Public Function ShowMsg(ByVal Text As String) As DialogResult
        Return ShowMsg(Text, ShowMsgImage.Alert, DefaultTitle)
    End Function

    Public Function ShowMsg(ByVal Text As String, ByVal Icon As ShowMsgImage) As DialogResult
        Return ShowMsg(Text, Icon, DefaultTitle)
    End Function


    Public Function ShowMsg(ByVal Text As String, ByVal Icon As ShowMsgImage, ByVal Title As String) As DialogResult
        Dim SMF As New ShowMsgForm

        'Set the title bar
        SMF.Text = Title

        'Select an image and sound based on the Icon parameter
        Select Case Icon
            Case ShowMsgImage.Alert
                SMF.MessagePictureBox.Image = My.Resources.Warning
                SMF.Sound = Media.SystemSounds.Asterisk
            Case ShowMsgImage.Confirm
                SMF.MessagePictureBox.Image = My.Resources.Confirm
                SMF.Sound = Media.SystemSounds.Question
            Case ShowMsgImage.Critical
                SMF.MessagePictureBox.Image = My.Resources.NotAllowed
                SMF.Sound = Media.SystemSounds.Hand
            Case ShowMsgImage.Info
                SMF.MessagePictureBox.Image = My.Resources.Info
                SMF.Sound = Media.SystemSounds.Asterisk
            Case ShowMsgImage.Security
                SMF.MessagePictureBox.Image = My.Resources.Lock
                SMF.Sound = Media.SystemSounds.Beep
            Case ShowMsgImage.UnderConstruction
                SMF.MessagePictureBox.Image = My.Resources.UnderConstruction
                SMF.Sound = Media.SystemSounds.Asterisk
        End Select

        'Set other properties
        SMF.TextLabel.Text = Text
        SMF.QuestionTextLabel.Text = ""
        SMF.Button1.Visible = True
        SMF.Button1.Text = "OK"
        SMF.Button1.DialogResult = DialogResult.OK
        SMF.Button2.Visible = False
        SMF.Button3.Visible = False

        'Resize the form
        SMF.SizeForm()

        'Set its starting position
        SMF.StartPosition = FormStartPosition.CenterScreen

        'Display the form modally and return its DialogResult
        Return SMF.ShowDialog
    End Function

#End Region


#Region " ShowMsg methods - get confirmation "

    Public Function ShowMsg(ByVal Text As String, ByVal Question As String, ByVal Buttons As ShowMsgButtons) As DialogResult
        Return ShowMsg(Text, Question, Buttons, ShowMsgDefaultButton.Button1, DefaultTitle)
    End Function

    Public Function ShowMsg(ByVal Text As String, ByVal Question As String, ByVal Buttons As ShowMsgButtons, ByVal DefaultButton As ShowMsgDefaultButton) As DialogResult
        Return ShowMsg(Text, Question, Buttons, DefaultButton, DefaultTitle)
    End Function

    Public Function ShowMsg(ByVal Text As String, ByVal Question As String, ByVal Buttons As ShowMsgButtons, ByVal DefaultButton As ShowMsgDefaultButton, ByVal Title As String) As DialogResult
        Dim SMF As New ShowMsgForm

        SMF.MessagePictureBox.Image = My.Resources.Confirm
        SMF.Text = Title
        SMF.TextLabel.Text = Text
        SMF.QuestionTextLabel.Text = ""
        SMF.ClickLabel.Text = Question
        SMF.Sound = Media.SystemSounds.Question

        Select Case Buttons
            Case ShowMsgButtons.ContinueCancel
                SMF.Button1.Visible = True
                SMF.Button1.Text = If(My.Settings.Language = "French", "Annuler", "Cancel")
                SMF.Button1.DialogResult = DialogResult.Cancel
                SMF.Button2.Visible = True
                SMF.Button2.Text = If(My.Settings.Language = "French", "Continuer", "Continue")
                SMF.Button2.DialogResult = DialogResult.OK
                SMF.Button3.Visible = False

            Case ShowMsgButtons.YesNo
                SMF.Button1.Visible = True
                SMF.Button1.Text = If(My.Settings.Language = "French", "Non", "No")
                SMF.Button1.DialogResult = DialogResult.No
                SMF.Button2.Visible = True
                SMF.Button2.Text = If(My.Settings.Language = "French", "Oui", "Yes")
                SMF.Button2.DialogResult = DialogResult.Yes
                SMF.Button3.Visible = False

            Case ShowMsgButtons.OkOnly
                SMF.Button1.Visible = True
                SMF.Button1.Text = "OK"
                SMF.Button1.DialogResult = DialogResult.OK
                SMF.Button2.Visible = False
                SMF.Button3.Visible = False

            Case ShowMsgButtons.YesNoCancel
                SMF.Button1.Visible = True
                SMF.Button1.Text = If(My.Settings.Language = "French", "Annuler", "Cancel")
                SMF.Button1.DialogResult = DialogResult.Cancel
                SMF.Button2.Visible = True
                SMF.Button2.Text = If(My.Settings.Language = "French", "Non", "No")
                SMF.Button2.DialogResult = DialogResult.No
                SMF.Button3.Visible = True
                SMF.Button3.Text = If(My.Settings.Language = "French", "Oui", "Yes")
                SMF.Button3.DialogResult = DialogResult.Yes
        End Select

        Select Case DefaultButton
            Case ShowMsgDefaultButton.Button1
                SMF.AcceptButton = SMF.Button1
            Case ShowMsgDefaultButton.Button2
                SMF.AcceptButton = SMF.Button2
            Case ShowMsgDefaultButton.Button3
                SMF.AcceptButton = SMF.Button3
        End Select

        SMF.SizeForm()
        SMF.StartPosition = FormStartPosition.CenterScreen
        Return SMF.ShowDialog()
    End Function

#End Region

#Region " ShowMsg methods - report exception "

    Public Function ShowMsg(ByVal Except As Exception) As DialogResult
        ShowMsg(Except, ShowMsgImage.Alert)
    End Function

    Public Function ShowMsg(ByVal Except As Exception, ByVal Icon As ShowMsgImage) As DialogResult
        Dim Msg As New StringBuilder
        Dim Title As String = ""

        Select Case True
            Case TypeOf Except Is System.Data.DataException
                Dim Err As System.Data.DataException = CType(Except, System.Data.DataException)

                Msg.AppendLine("Database error: " + Err.Message)

                If Except.InnerException IsNot Nothing Then
                    Msg.AppendLine()
                    Msg.AppendLine("Reason: " + Err.InnerException.Message)
                End If

            Case TypeOf Except Is System.Runtime.InteropServices.COMException
                Dim Err As System.Runtime.InteropServices.COMException = CType(Except, System.Runtime.InteropServices.COMException)

                Msg.AppendLine("COM error: " + Err.Message)
                If Except.InnerException IsNot Nothing Then
                    Msg.AppendLine()
                    Msg.AppendLine("Inner exception: " + Err.InnerException.Message)
                End If

            Case Else
                Msg.AppendLine("An exception of type " + Except.GetType.ToString + " was thrown.")
                Msg.AppendLine()
                Msg.AppendLine("Message: " + Except.Message)
        End Select

        If Except.Data IsNot Nothing AndAlso Except.Data.Count > 0 Then
            Msg.AppendLine()
            Msg.Append("Data:")
            For Each de As DictionaryEntry In Except.Data
                Msg.AppendLine()
                Msg.AppendFormat("    {0} : {1}", de.Key.ToString, de.Value.ToString)
            Next
        End If

        Return ShowMsg(Msg.ToString, Icon, "Exception occured")
    End Function

#End Region

#Region " ShowMsg methods - standardized messages "

    Public Function ShowAccessDenied() As DialogResult
        Return ShowMsg("You do not have sufficient permission to perform requested operation.", ShowMsgImage.Security, "Access violation")
    End Function

    Public Function ShowUnderConstruction() As DialogResult
        Return ShowMsg("The requested feature is unavailable at this time.", ShowMsgImage.UnderConstruction)
    End Function

    Public Function ShowDeprecated(ByVal Alternative As String) As DialogResult
        Dim SMF As New ShowMsgForm

        SMF.MessagePictureBox.Image = My.Resources.UnderConstruction
        SMF.Text = "Feature unavailable"
        SMF.TextLabel.Text = "The feature you tried to access is no longer available."
        SMF.QuestionTextLabel.Text = Alternative
        SMF.Sound = Media.SystemSounds.Asterisk

        SMF.Button1.Visible = True
        SMF.Button1.Text = "OK"
        SMF.Button1.DialogResult = DialogResult.OK
        SMF.Button2.Visible = False
        SMF.Button3.Visible = False

        'Resize the form
        SMF.SizeForm()

        'Set its starting position
        SMF.StartPosition = FormStartPosition.CenterScreen

        'Display the form modally and return its DialogResult
        Return SMF.ShowDialog
    End Function

#End Region

End Module
