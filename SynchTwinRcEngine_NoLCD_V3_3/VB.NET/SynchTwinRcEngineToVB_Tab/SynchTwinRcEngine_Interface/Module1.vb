Imports System.IO
Imports System.IO.Ports
Imports System.Runtime.CompilerServices

Module Module1
    Public pos As Integer = 0
    Public posAux As Integer = 0
    Public posRudder As Integer = 0
    Public ReadSettings As String
    Public array() As String
    Public MessageToSend As String
    Public ModeType As Integer
    Public SpeedArray() As String
    Public myRectangle As Rectangle
    Public HardwareArray() As String
    Public watch As Stopwatch
    Public pulseValue As Integer = 0
    Public pulseValueAux As Integer = 0
    Public pulseMinValue As Integer = 0
    Public pulseMaxValue As Integer = 0
    Public NewPulse As Integer
    Public NewPulseAux As Integer

    Public Const programmer_hw = "usbasp"

    Public Sub set_fuse_boxes(ByVal fusesint As Integer)
        If fusesint = 111 Then
            enable_control(, , , , , , , , 1, 1, 1, , , , )
        ElseIf fusesint = 11 Then
            enable_control(, , , , , , , , 1, 1, 0, , , , )
        End If
    End Sub

    Public Sub enable_control(Optional ByVal txtfile_name As Integer = 2, Optional ByVal cmdbrowse_enable As Integer = 2, Optional ByVal rdoflash As Integer = 2, Optional ByVal rdoeeprom As Integer = 2, Optional ByVal chksck_enable As Integer = 2, Optional ByVal cmdwrite_en As Integer = 2, Optional ByVal cmdread_En As Integer = 2, Optional ByVal cmdverify_en As Integer = 2, Optional ByVal txthfuse_en As Integer = 2, Optional ByVal txtlfuse_en As Integer = 2, Optional ByVal txtefuse_en As Integer = 2, Optional ByVal cmdwritefuse_en As Integer = 2, Optional ByVal cmdreadfuse_en As Integer = 2, Optional ByVal cmdflashbrowser_en As Integer = 2, Optional ByVal cmderase_chip_en As Integer = 2)
        With Form1
            If txtfile_name <> 2 Then
                .txtfilename.Enabled = txtfile_name
            End If
            If cmdbrowse_enable <> 2 Then
                .bttnbrowse.Enabled = cmdbrowse_enable
            End If
            'If rdoflash <> 2 Then
            '    .rdowrvflash.Enabled = rdoflash
            'End If
            'If rdoeeprom <> 2 Then
            '    .rdowrveeprom.Enabled = rdoeeprom
            'End If
            'If chksck_enable <> 2 Then
            '    .chksck.Enabled = chksck_enable
            'End If
            'If cmdwrite_en <> 2 Then
            '    .cmdwrite.Enabled = cmdwrite_en
            'End If
            'If cmdread_En <> 2 Then
            '    .cmdread.Enabled = cmdread_En
            '    .cmdcalibrationndlock.Enabled = cmdread_En
            'End If
            'If cmdverify_en <> 2 Then
            '    .cmdverify.Enabled = cmdverify_en
            'End If
            'If txthfuse_en <> 2 Then
            '    .txthfuse.Enabled = txthfuse_en
            'End If
            'If txtlfuse_en <> 2 Then
            '    .txtlfuse.Enabled = txtlfuse_en
            'End If
            'If txtefuse_en <> 2 Then
            '    .txtefuse.Enabled = txtefuse_en
            'End If
            'If cmdwritefuse_en <> 2 Then
            '    .cmdwritefuse.Enabled = cmdwritefuse_en
            'End If
            'If cmdreadfuse_en <> 2 Then
            '    .cmdreadfuse.Enabled = cmdreadfuse_en

            'End If
            ''If cmdflashbrowser_en <> 2 Then

            ''End If
            'If cmderase_chip_en <> 2 Then
            '    .cmderasechip.Enabled = cmderase_chip_en
            'End If
        End With
    End Sub

    Public Sub enable_fuse_control(Optional ByVal fusegrid As Integer = 2, Optional ByVal txtefuse As Integer = 2, Optional ByVal txthfuse As Integer = 2, Optional ByVal txtlfuse As Integer = 2, Optional ByVal cmdapply As Integer = 2, Optional ByVal cmdok As Integer = 2, Optional ByVal cmdfuseinfo As Integer = 2)
        'With frmfuseeditor
        '    If fusegrid <> 2 Then
        '        .fusegrid.Enabled = fusegrid
        '    End If
        '    If txtefuse <> 2 Then
        '        .txtefuse.Enabled = txtefuse
        '    End If
        '    If txthfuse <> 2 Then
        '        .txthfuse.Enabled = txthfuse
        '    End If
        '    If txtlfuse <> 2 Then
        '        .txtlfuse.Enabled = txtlfuse
        '    End If
        '    If cmdapply <> 2 Then
        '        .cmdapply.Enabled = cmdapply
        '    End If
        '    If cmdok <> 2 Then
        '        .cmdok.Enabled = cmdok
        '    End If
        '    If cmdfuseinfo <> 2 Then
        '        .cmdfuseinfo.Enabled = cmdfuseinfo
        '    End If
        'End With
    End Sub

    <Extension()>
    Public Function Contains(str As String, substring As String, comp As StringComparison) As Boolean
        If substring Is Nothing Then
            Throw New ArgumentNullException("substring", "substring cannot be null.")
        ElseIf Not [Enum].IsDefined(GetType(StringComparison), comp) Then
            Throw New ArgumentException("comp is not a member of StringComparison", "comp")
        End If
        Return str.IndexOf(substring, comp) >= 0
    End Function

End Module
