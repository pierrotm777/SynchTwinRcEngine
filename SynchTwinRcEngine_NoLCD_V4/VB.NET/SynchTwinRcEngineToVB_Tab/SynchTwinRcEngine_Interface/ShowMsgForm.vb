Imports System.Drawing
Imports System.Drawing.Printing
Imports System.Media

Friend Class ShowMsgForm

#Region " Storage "

    Private _Sound As SystemSound

    'Private WithEvents PD As Printing.PrintDocument = Nothing
    'Private FormImage As Bitmap = Nothing

#End Region

#Region " Properties "

    Friend Property Sound() As SystemSound
        Get
            Return _Sound
        End Get
        Set(ByVal value As System.Media.SystemSound)
            _Sound = value
        End Set
    End Property

#End Region

#Region " Constructors "

    Public Sub New()
        InitializeComponent()
        Me.Text = ""
        Me.Icon = My.Resources.BiMoteur
    End Sub

#End Region

#Region " Event handlers "

    'Private Sub PD_PrintPage(ByVal sender As Object, ByVal e As PrintPageEventArgs) Handles PD.PrintPage
    '    e.Graphics.DrawImage(FormImage, 100, 100)
    'End Sub

    Private Sub ClickLabel_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles ClickLabel.LinkClicked
        Try
            ' Change the color of the link text by setting LinkVisited 
            ' to True.
            ClickLabel.LinkVisited = True
            ' Call the Process.Start method to open the default browser 
            ' with a URL:
            System.Diagnostics.Process.Start("mailto:" & ClickLabel.Text)
        Catch ex As Exception
            ' The error message
            MessageBox.Show("Unable to open link that was clicked.")
        End Try
    End Sub


    'Private Sub ClickLabel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClickLabel.Click
    '    If PD IsNot Nothing Then Exit Sub 'A print is already in progress

    '    'Get an image of the form
    '    Dim G As Graphics = Me.CreateGraphics()
    '    FormImage = New Bitmap(Me.Width, Me.Height, G)
    '    Dim FG As Graphics = Graphics.FromImage(FormImage)
    '    Dim dc1 As IntPtr = G.GetHdc
    '    Dim dc2 As IntPtr = FG.GetHdc

    '    Dim WidthDiff As Integer = (Me.Width - Me.ClientRectangle.Width)
    '    Dim HeightDiff As Integer = (Me.Height - Me.ClientRectangle.Height)
    '    Dim BorderSize As Integer = WidthDiff \ 2
    '    Dim HeightTitleBar As Integer = HeightDiff - BorderSize

    '    Win32.BitBlt(dc2, 0, 0, _
    '    Me.ClientRectangle.Width + WidthDiff, Me.ClientRectangle.Height + HeightDiff, _
    '    dc1, 0 - BorderSize, 0 - HeightTitleBar, Win32.SRCCOPY)

    '    G.ReleaseHdc(dc1)
    '    FG.ReleaseHdc(dc2)
    '    FG = Nothing
    '    G = Nothing

    '    'And print it
    '    PD = New Printing.PrintDocument
    '    PD.Print()

    '    'Then clean up
    '    PD.Dispose()
    '    FormImage = Nothing
    'End Sub


    Private Sub ShowMsgForm_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'If the user clicks on the Close Form button in the upper right,
        'it will have the same effect as clicking the accept button
        If e.CloseReason = CloseReason.UserClosing AndAlso Me.AcceptButton IsNot Nothing Then
            Me.DialogResult = CType(Me.AcceptButton, Button).DialogResult
        End If
        QuestionTextLabel.ForeColor = SystemColors.ControlText
    End Sub

    Private Sub ShowMsgForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Give the focus to the accept button, providing a visual clue as to which button it is.
        Dim Btn As Button = TryCast(Me.AcceptButton, Button)

        If Btn IsNot Nothing AndAlso Btn.Visible Then
            Btn.Select()
        Else
            Me.Button1.Select()
        End If

        'Me.StartPosition = FormStartPosition.CenterParent 
        CenterForm(Me, Form1)

        Timer1.Enabled = True
    End Sub

    Public Shared Sub CenterForm(ByVal frm As Form, Optional ByVal parent As Form = Nothing)
        ' Note: call this from frm's Load event!
        Dim r As Rectangle
        If parent IsNot Nothing Then
            r = parent.RectangleToScreen(parent.ClientRectangle)
        Else
            r = Screen.FromPoint(frm.Location).WorkingArea
        End If

        Dim x = r.Left + (r.Width - frm.Width) \ 2
        Dim y = r.Top + (r.Height - frm.Height) \ 2
        frm.Location = New Point(x, y)
    End Sub

#End Region

#Region " Methods "

    Protected Overrides Sub OnShown(ByVal e As System.EventArgs)
        'Intercept OnShown to play the sound
        If _Sound Is Nothing Then
            SystemSounds.Asterisk.Play()
        Else
            _Sound.Play()
        End If

        'Then carry on as usual
        MyBase.OnShown(e)
    End Sub

    Public Sub SizeForm()
        Dim NewHeight As Integer = 0
        Dim NewWidth As Integer = 0

        'Icon stays where it is.
        'TextLabel.Left and QuestionLabel.Left are already correct

        NewWidth = Math.Max(TextLabel.Width, QuestionTextLabel.Width) + TextLabel.Left + 4
        If NewWidth < 275 Then NewWidth = 275

        Button1.Left = NewWidth - Button1.Width - 4
        Button2.Left = Button1.Left - Button2.Width - 4

        If QuestionTextLabel.Text = "" Then
            ClickLabel.Top = TextLabel.Top + TextLabel.Height + 8
        Else
            QuestionTextLabel.Top = TextLabel.Top + TextLabel.Height + 8
            ClickLabel.Top = QuestionTextLabel.Top + QuestionTextLabel.Height + 8
        End If
        LabelMailTo.Top = ClickLabel.Top

        Button1.Top = ClickLabel.Top + ClickLabel.Height + 8
        Button2.Top = Button1.Top
        Button3.Top = Button1.Top

        NewHeight = Math.Max(Button1.Top + Button1.Height, MessagePictureBox.Top + MessagePictureBox.Height) + 4

        Me.SetClientSizeCore(NewWidth, NewHeight)
    End Sub

    'fermeture auto du message après 3 secondes
    Private Sub Timer1_Tick(sender As System.Object, e As System.EventArgs) Handles Timer1.Tick
        Me.Close()
    End Sub
#End Region

End Class
