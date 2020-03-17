Imports System.IO
Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles

Public Class UcV_ProgressBar

    Enum Look As Byte
        LookIntegralRectangle = 0
        LookRideau = 1
        LookSmooth = 2
    End Enum

    Private Z_Value As Integer = 50
    Private Z_Mini As Integer = 0
    Private Z_Maxi As Integer = 100
    Private Z_Mode As Look = Look.LookIntegralRectangle

    Private EpaisseurRectangle As Integer = 5
    Private EspaceEntreRectangle As Integer = 1
    Private Marge As Integer = 3

#Region "Global"

    Private Sub LanceRedraw()

        Dim g As Graphics = Me.CreateGraphics()
        Dim e As New PaintEventArgs(g, ClientRectangle)
        RedrawMe(e)
        g.Dispose()

    End Sub

    Public Sub New()

        ' Cet appel est requis par le Concepteur Windows Form.
        InitializeComponent()

    End Sub

    Private Sub UcProgressBar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Application.EnableVisualStyles()

    End Sub

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)

        MyBase.OnPaint(e)
        RedrawMe(e)

    End Sub

#End Region

#Region "Dessin"

    Private Sub RedrawMe(ByVal e As PaintEventArgs)
        Select Case Z_Mode
            Case Look.LookRideau
                DrawStore(e)
            Case Look.LookSmooth
                DrawSmooth(e)
            Case Look.LookIntegralRectangle
                IntegralRectangle(e)
        End Select
    End Sub

    Private Sub DrawSmooth(ByVal e As PaintEventArgs)
        Dim Hauteur As Integer, Largeur As Integer, CurX As Integer, CurY As Integer
        Dim Rect As Rectangle

        'cadre
        ProgressBarRenderer.DrawVerticalBar(e.Graphics, ClientRectangle)

        'defini la hauteur a dessiner (valeur en cours)
        Hauteur = (ClientRectangle.Height - Marge * 2) * (_Value / (_Maxi - _Mini))

        'recalcule la hauteur pour qu'elle corresponde a un multiple de rectangles
        Dim HauteurRectangle As Integer = ProgressBarRenderer.ChunkThickness + ProgressBarRenderer.ChunkSpaceThickness * 2
        Dim HauteurCorrigee As Integer = Int(Hauteur / HauteurRectangle)

        HauteurCorrigee *= HauteurRectangle

        'calcul de la largeur
        Largeur = ClientRectangle.Width - Marge * 2

        'coordonnees du rectangle
        CurX = Me.ClientRectangle.X + Marge
        CurY = ClientRectangle.Height - HauteurCorrigee - Marge

        'contenu rectangles complets
        Rect = New Rectangle(CurX, CurY, Largeur, HauteurCorrigee)
        ProgressBarRenderer.DrawVerticalChunks(e.Graphics, Rect)

        '====================================================================================

        'contenu rectangle partiel
        Dim HauteurSolde As Integer = Hauteur - HauteurCorrigee

        If HauteurSolde > 0 Then
            CurY = ClientRectangle.Height - Hauteur - Marge
            Rect = New Rectangle(CurX, CurY, Largeur, HauteurSolde)
            ProgressBarRenderer.DrawVerticalChunks(e.Graphics, Rect)
        End If

    End Sub

    Private Sub IntegralRectangle(ByVal e As PaintEventArgs)
        Dim Hauteur As Integer, Largeur As Integer, CurX As Integer, CurY As Integer
        Dim Rect As Rectangle

        'cadre
        ProgressBarRenderer.DrawVerticalBar(e.Graphics, ClientRectangle)

        'defini la hauteur a dessiner (valeur en cours)
        Hauteur = Int((ClientRectangle.Height - Marge * 2) * (_Value / (_Maxi - _Mini)))

        'recalcule la hauteur pour qu'elle corresponde a un multiple de rectangles
        Dim HauteurRectangle As Integer = ProgressBarRenderer.ChunkThickness + ProgressBarRenderer.ChunkSpaceThickness * 2
        Dim HauteurCorrigee As Integer = Int(Hauteur / HauteurRectangle)
        HauteurCorrigee *= HauteurRectangle

        'calcul de la largeur
        Largeur = ClientRectangle.Width - Marge * 2

        'coordonnees du rectangle
        CurX = Me.ClientRectangle.X + Marge
        CurY = ClientRectangle.Height - HauteurCorrigee - Marge

        'contenu
        Rect = New Rectangle(CurX, CurY, Largeur, HauteurCorrigee)
        ProgressBarRenderer.DrawVerticalChunks(e.Graphics, Rect)

    End Sub

    Private Sub DrawStore(ByVal e As PaintEventArgs)
        Dim Hauteur As Integer, Largeur As Integer, CurX As Integer, CurY As Integer
        Dim Rect As Rectangle

        'cadre
        ProgressBarRenderer.DrawVerticalBar(e.Graphics, ClientRectangle)

        'defini la hauteur a dessiner (valeur en cours)
        Hauteur = Int((ClientRectangle.Height - Marge * 2) * (_Value / (_Maxi - _Mini)))

        'recalcule la hauteur
        Dim HauteurRectangle As Integer = ProgressBarRenderer.ChunkThickness + ProgressBarRenderer.ChunkSpaceThickness * 2
        Dim HauteurCorrigee As Integer = (Hauteur / HauteurRectangle) * HauteurRectangle

        'calcul de la largeur
        Largeur = ClientRectangle.Width - Marge * 2

        'coordonnees du rectangle
        CurX = Me.ClientRectangle.X + Marge
        CurY = ClientRectangle.Height - HauteurCorrigee - Marge

        'contenu
        Rect = New Rectangle(CurX, CurY, Largeur, HauteurCorrigee)
        ProgressBarRenderer.DrawVerticalChunks(e.Graphics, Rect)

    End Sub

#End Region

#Region "Proprietes"

    Public Property _Value() As Integer
        Get
            Return Z_Value
        End Get
        Set(ByVal value As Integer)
            'If value > Z_Maxi Or value < Z_Mini Then
            '    MsgBox("La valeur doit être comprise entre les bornes MINI et MAXI !", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "ProgressBar Alert !")
            'Else
            Z_Value = value
            LanceRedraw()
            'End If
        End Set
    End Property

    Public Property _Maxi() As Integer
        Get
            Return Z_Maxi
        End Get
        Set(ByVal value As Integer)
            If value < Z_Mini + 1 Then
                MsgBox("La valeur doit être supérieure au MINI !", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "ProgressBar Alert !")
            Else
                Z_Maxi = value
                LanceRedraw()
            End If
        End Set
    End Property

    Public Property _Mini() As Integer
        Get
            Return Z_Mini
        End Get
        Set(ByVal value As Integer)
            If value > Z_Maxi - 1 Then
                MsgBox("La valeur doit être au inférieure au MAXI !", MsgBoxStyle.Critical Or MsgBoxStyle.OkOnly, "ProgressBar Alert !")
            Else
                Z_Mini = value
                LanceRedraw()
            End If
        End Set
    End Property

    Public Property _Dessin() As Look
        Get
            Return Z_Mode
        End Get
        Set(ByVal value As Look)
            Z_Mode = value
            LanceRedraw()
        End Set
    End Property

#End Region

End Class

