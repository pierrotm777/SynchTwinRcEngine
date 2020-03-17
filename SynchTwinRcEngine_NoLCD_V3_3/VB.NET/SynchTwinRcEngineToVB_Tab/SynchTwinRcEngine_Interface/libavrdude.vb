Imports System.Threading

Public Class libavrdude
    Public predef_folder As String = Application.StartupPath & "\predefs"
    Public boardfilepath As String = Application.StartupPath & "\boardinfo.txt"
    Private Declare Auto Function PlaySound Lib "winmm.dll" _
    (ByVal lpszSoundName As String, ByVal hModule As Integer, _
     ByVal dwFlags As Integer) As Integer

    Private Const SND_FILENAME As Integer = &H20000
    Public Const folderpath = "C:\Avrpaltemp_folder"
    Dim global_filename As String = ""
    Dim usbasp_class As New usbasp
    Dim outputstr As String
    Dim appp As String
    Dim argumentq As String
    Dim avrdude_filepath As String = Application.StartupPath & "\avrdude.conf"
    Public Function verify_fuse_byte(ByVal fuse_str As String) As Integer
        Dim hex_num1 As String = ""
        Dim hex_num2 As String = ""
        Dim ascval As Integer = 0
        Dim return_data As String = 1
        If (Mid(fuse_str, 1, 2) <> "0x") Then
            return_data = -1
            Return return_data
        End If
        If fuse_str.Length <> 4 Then
            return_data = -2
            Return return_data
        End If
        hex_num1 = Mid(fuse_str, 3, 1)
        hex_num2 = Mid(fuse_str, 4, 1)
        ascval = Asc(hex_num1)
        If check_ascval(ascval) = -3 Then
            return_data = -3
            Return return_data
        End If
        ascval = Asc(hex_num2)
        If check_ascval(ascval) = -3 Then
            return_data = -3
            Return return_data
        End If
        Return return_data
    End Function
    Function check_ascval(ByVal ascval As String) As Integer
        If ascval < 48 Then
            Return -3
        End If
        If ascval > 57 And ascval < 65 Then
            Return -3
        End If
        If ascval > 70 And ascval < 97 Then
            Return -3
        End If
        If ascval > 102 Then
            Return -3
        End If
        Return 1
    End Function
    Public Sub runcommand()
        Dim oup As String = ""
        Dim strm As IO.StreamReader
        Using prcavrdude As Process = New Process
            prcavrdude.StartInfo.FileName = Mid(Application.ExecutablePath, 1, InStrRev(Application.ExecutablePath, "\")) & "avrdude"
            prcavrdude.StartInfo.UseShellExecute = False
            prcavrdude.StartInfo.Arguments = argumentq
            prcavrdude.StartInfo.CreateNoWindow = True
            prcavrdude.StartInfo.RedirectStandardError = True

            ' tmrusbcheck.Enabled = False

            prcavrdude.Start()
            prcavrdude.WaitForExit()
            strm = prcavrdude.StandardError

            While strm.EndOfStream = False
                oup = oup & vbCrLf & strm.ReadLine
            End While
            strm.Close()
            prcavrdude.Close()
        End Using
        outputstr = oup
    End Sub
    Public Sub getboardnames(ByRef cboboard As ComboBox)
        cboboard.Items.Clear()
        If FileIO.FileSystem.FileExists(boardfilepath) Then
            Using strm As IO.StreamReader = New IO.StreamReader(boardfilepath)
                Dim rdline As String = ""
                While Not strm.EndOfStream = True
                    rdline = LTrim(strm.ReadLine)
                    If InStr(rdline, "board_name") > 0 Then
                        Dim strdiv As String = ""
                        strdiv = LTrim(RTrim(Mid(rdline, InStr(rdline, "=") + 1, rdline.Length)))
                        cboboard.Items.Add(strdiv)
                    End If
                End While
            End Using
        Else
            MsgBox("Fatal Error!!!")
        End If
    End Sub
    Public Sub getboarddetails(ByVal board_name As String, ByRef baud As String, ByRef mcu As String, ByRef protocol As String)
        Dim strbuf As String = ""
        If FileIO.FileSystem.FileExists(boardfilepath) Then
            Using strm As IO.StreamReader = New IO.StreamReader(boardfilepath)
                Dim rdline As String = ""
                Dim condition As Integer = 0

                While Not strm.EndOfStream = True
                    rdline = LTrim(RTrim(strm.ReadLine))
                    If InStr(rdline, "board_name") > 0 Then
                        Dim strdiv As String = ""
                        strdiv = LTrim(RTrim(Mid(rdline, InStr(rdline, "=") + 1, rdline.Length)))
                        If strdiv = LTrim(RTrim(board_name)) Then
                            condition = 1
                        End If
                    End If
                    If condition = 1 Then
                        If InStr(rdline, "upload_protocol") Then
                            protocol = LTrim(RTrim(Mid(rdline, InStr(rdline, "=") + 1, rdline.Length)))
                        End If
                        If InStr(rdline, "upload_speed") Then
                            baud = LTrim(RTrim(Mid(rdline, InStr(rdline, "=") + 1, rdline.Length)))
                        End If
                        If InStr(rdline, "mcu") Then
                            mcu = LTrim(RTrim(Mid(rdline, InStr(rdline, "=") + 1, rdline.Length)))
                        End If
                        'strbuf &= vbCrLf & rdline
                        If rdline = "##############################################################" Then
                            Exit While
                        End If
                    End If
                End While
            End Using
            'MsgBox(strbuf)
        End If
    End Sub

    Public Sub getboarddetailsfrommcu(ByRef mcu As String, ByVal board_name As String)
        If FileIO.FileSystem.FileExists(boardfilepath) Then
            Using strm As IO.StreamReader = New IO.StreamReader(boardfilepath)
                Dim rdline As String = ""
                While Not strm.EndOfStream = True
                    rdline = LTrim(strm.ReadLine)
                    If InStr(rdline, "board_name") > 0 Then
                        board_name = LTrim(RTrim(Mid(rdline, InStr(rdline, "=") + 1, rdline.Length)))
                    End If
                    If InStr(rdline, "mcu") > 0 Then
                        Dim strdiv As String = ""
                        strdiv = LTrim(RTrim(Mid(rdline, InStr(rdline, "=") + 1, rdline.Length)))
                        'MessageBox.Show(strdiv & vbCrLf & mcu)
                        If String.Compare(mcu, strdiv, True) Then
                            mcu = strdiv
                            Exit While
                        End If
                    End If
                End While

            End Using
        Else
            MsgBox("Fatal Error!!!")
        End If

    End Sub

    Public Sub avrdude_command(ByVal app As String, ByVal argument As String, ByRef outputstring As String)
        appp = app
        argumentq = argument
        Dim t As Threading.Thread
        t = New Threading.Thread(AddressOf Me.runcommand)
        t.Start()
        While t.IsAlive
            Application.DoEvents()
        End While
        Try
            t.Abort()
        Catch ex As Exception

        End Try

        outputstring = outputstr
        appp = ""
        argumentq = ""
    End Sub

    Public Function get_avr_signature() As String
        Dim outputstr As String = ""
        Dim signature_byte As String = ""
        Dim avr_signature_line As Integer = 0
        Dim frmmain_bitclock As String = ""
        frmmain_bitclock = " -B " & My.Settings.bitclock_slow
        Threading.Thread.Sleep(10)
        'MessageBox.Show(frmmain_bitclock & " -c " & programmer_hw & " -p m16")
        'avrdude_command("avrdude", frmmain_bitclock & " -c " & programmer_hw & " -p m16", outputstr)
        avrdude_command("avrdude", frmmain_bitclock & " -c " & programmer_hw & " -p m16 -F", outputstr)

        'MessageBox.Show(outputstr)
        If My.Settings.bitclock_fast <> "0" Then
            frmmain_bitclock = " -B " & My.Settings.bitclock_fast
        Else
            frmmain_bitclock = ""
        End If
        Threading.Thread.Sleep(10)
        avr_signature_line = InStr(outputstr, "avrdude: Device signature = ")
        If avr_signature_line > 0 Then
            signature_byte = Mid(outputstr, avr_signature_line + 28, 8)
            'MessageBox.Show(signature_byte)
            Return signature_byte
        Else
            Return "-1"
        End If
    End Function

    Public Function get_avr_chip_name(ByVal signature As String) As String
        'Dim filestrm As New IO.StreamReader(Mid(Application.ExecutablePath, 1, Application.ExecutablePath.Length - 15) & "\signature_data.txt")
        Dim filestrm As New IO.StreamReader("signature_data.txt")
        Dim signature_string As String = ""
        Dim chip_name As String = ""
        'MessageBox.Show(signature)
        If signature = "-1" Then
            Return signature
            Exit Function
        Else
            Dim modded_signature_String As String = Mid(signature, 1, 4) & " 0x" & Mid(signature, 5, 2) & " 0x" & Mid(signature, 7, 2)
            'MessageBox.Show(modded_signature_String)
            Do While filestrm.EndOfStream = False
                signature_string = filestrm.ReadLine
                'MessageBox.Show(signature_string)
                'If InStr(signature_string.ToUpper, modded_signature_String.ToUpper) > 0 Then
                '    chip_name = Mid(signature_string, 1, InStr(signature_string, ",") - 1)
                '    Exit Do
                'End If
                If signature_string.Contains(modded_signature_String, StringComparison.OrdinalIgnoreCase) = True Then
                    Dim sp() As String = signature_string.Split(",")
                    chip_name = sp(0)
                    'MessageBox.Show(modded_signature_String)
                    Exit Do
                End If
            Loop
            If chip_name.Length = 0 Then
                chip_name = "Unknown"
            End If
            Return chip_name
        End If
    End Function

    Public Function folder_fix()
        Dim fldr As New IO.DirectoryInfo(folderpath)
        If fldr.Exists = False Then
            Try

                FileSystem.MkDir(folderpath)
                Return 1
            Catch
                Return 0
            End Try
        Else
            Return 2
        End If
    End Function
    Public Function copy_file_to_temp(ByVal filename As String, ByRef filepath As String) As Integer
        Dim fn As String = ""
        Dim fldr As New IO.DirectoryInfo(folderpath)
        fn = FileIO.FileSystem.GetName(filename)
        If fldr.Exists = True Then
            Try
                FileIO.FileSystem.CopyFile(filename, folderpath & "\" & fn.Replace(" ", "_"), True)
                global_filename = folderpath & "\" & fn.Replace(" ", "_")
                filepath = global_filename
                Return 1
            Catch ex As Exception
                Return 0
            End Try
        Else
            Return -1
        End If
    End Function
    Public Function delete_temp_file() As Integer
        Dim fldr As New IO.DirectoryInfo(folderpath)
        If fldr.Exists = True Then
            Try
                FileIO.FileSystem.DeleteFile(global_filename)
                Return 1
            Catch ex As Exception
                Return 0
            End Try
        Else
            Return -1
        End If
    End Function
    Public Sub delete_file(ByVal filename As String)
        Try
            FileIO.FileSystem.DeleteFile(filename)
        Catch ex As Exception
            '    MsgBox(ex.Message)
        End Try
    End Sub
    Public Sub set_global_filename(ByVal filename As String)
        global_filename = filename
    End Sub
    Public Function copy_temp_file(ByVal filename As String) As Integer
        Dim fldr As New IO.DirectoryInfo(folderpath)
        If fldr.Exists = True Then
            Try
                If FileIO.FileSystem.FileExists(global_filename) = False Then Return -1
                FileIO.FileSystem.CopyFile(global_filename, filename)
                Return 1
            Catch ex As Exception
                Return 0
            End Try
        Else
            Return -1
        End If
    End Function
    Public Sub playsnd()
        Try
            'PlaySound(Mid(Application.ExecutablePath, 1, Application.ExecutablePath.Length - 15) & "\twaing.wav", 0, SND_FILENAME)
            PlaySound("twaing.wav", 0, SND_FILENAME)
        Catch ex As Exception
            MsgBox("Error playing sound.Better uncheck the sound playing option")
        End Try
    End Sub
    Public Sub process_output_string(ByVal outpstring As String, ByVal output_command_source As Integer, Optional ByVal fuse_int As Integer = 0)
        Dim output_state As Integer = 0
        Select Case output_command_source
            Case 1 'for writing flash
                'first check the successful case
                If InStr(outpstring, "flash written") Then
                    output_state += 1
                End If
                If InStr(outpstring, "flash verified") Then
                    output_state += 10
                End If
                If InStr(outpstring, "avrdude: safemode: Fuses OK") Then
                    output_state += 100
                End If

                'Now start processing the output
                If output_state = 111 Then
                    MsgBox("AVR flash successfully written")
                Else
                    'If My.Settings.enablefunnysound = True Then
                    '    Dim t As Threading.Thread
                    '    t = New Threading.Thread(AddressOf playsnd)
                    '    t.Start()
                    'End If
                    'MsgBox("Error writing flash")
                End If
                output_state = 0
            Case 2
                If InStr(outpstring, "eeprom written") Then
                    output_state += 1
                End If
                If InStr(outpstring, "eeprom verified") Then
                    output_state += 10
                End If
                If InStr(outpstring, "avrdude: safemode: Fuses OK") Then
                    output_state += 100
                End If

                'Now start processing the output
                If output_state = 111 Then
                    MsgBox("AVR eeprom successfully written")
                Else
                    'If My.Settings.enablefunnysound = True Then
                    '    Dim t As Threading.Thread
                    '    t = New Threading.Thread(AddressOf playsnd)
                    '    t.Start()
                    'End If
                    MsgBox("Error writing eeprom")
                End If
                output_state = 0

            Case 3
                If InStr(outpstring, "reading flash memory") Then
                    output_state += 1
                End If
                If InStr(outpstring, "writing output file") Then
                    output_state += 10
                End If
                If output_state = 11 Then
                    MsgBox("AVR Flash successfully read.")
                Else
                    MsgBox("Error Reading Flash")
                End If
                output_state = 0
            Case 4
                If InStr(outpstring, "flash verified") Then
                    output_state += 1
                End If
                If output_state = 1 Then
                    MsgBox("AVR flash successfully verified")
                Else
                    Dim t As Threading.Thread
                    t = New Threading.Thread(AddressOf playsnd)
                    t.Start()
                    MsgBox("Error Verifying flash")
                End If
                output_state = 0
            Case 5
                If InStr(outpstring, "avrdude: erasing chip") Then
                    output_state += 1
                End If

                'now process output
                If output_state = 1 Then
                    MsgBox("AVR successfully erased")
                Else
                    'If My.Settings.enablefunnysound = True Then
                    '    Dim t As Threading.Thread
                    '    t = New Threading.Thread(AddressOf playsnd)
                    '    t.Start()
                    'End If
                    'MsgBox("Error erasing flash")
                End If
                output_state = 0
            Case 6
                If InStr(outpstring, "eeprom verified") Then
                    output_state += 1
                End If
                If output_state = 1 Then
                    MsgBox("AVR eeprom successfully verified")
                Else
                    Dim t As Threading.Thread
                    t = New Threading.Thread(AddressOf playsnd)
                    t.Start()
                    MsgBox("Error Verifying eeprom")
                End If
                output_state = 0
            Case 7
                If InStr(outpstring, "reading eeprom memory") Then
                    output_state += 1
                End If
                If InStr(outpstring, "writing output file") Then
                    output_state += 10
                End If
                If output_state = 11 Then
                    MsgBox("AVR EEPROM successfully read.")
                Else
                    MsgBox("Error Reading Flash")
                End If
                output_state = 0
            Case 8
                If fuse_int = 111 Then
                    If InStr(outpstring, "writing efuse (1 bytes)") Then
                        output_state += 1
                    End If
                    If InStr(outpstring, "1 bytes of efuse verified") Then
                        output_state += 1
                    End If
                    If InStr(outpstring, "writing hfuse (1 bytes)") Then
                        output_state += 10
                    End If
                    If InStr(outpstring, "1 bytes of hfuse verified") Then
                        output_state += 10
                    End If
                    If InStr(outpstring, "writing lfuse (1 bytes)") Then
                        output_state += 100
                    End If
                    If InStr(outpstring, "1 bytes of lfuse verified") Then
                        output_state += 100
                    End If
                    If output_state = 222 Then
                        MsgBox("Fuses Successfully Written")
                    Else
                        MsgBox("Error Writing Fuses")
                    End If
                ElseIf fuse_int = 11 Then
                    If InStr(outpstring, "writing hfuse (1 bytes)") Then
                        output_state += 1
                    End If
                    If InStr(outpstring, "1 bytes of hfuse verified") Then
                        output_state += 1
                    End If
                    If InStr(outpstring, "writing lfuse (1 bytes)") Then
                        output_state += 10
                    End If
                    If InStr(outpstring, "1 bytes of lfuse verified") Then
                        output_state += 10
                    End If
                    If output_state = 22 Then
                        MsgBox("Fuses Successfully Written")
                    Else
                        MsgBox("Error Writing Fuses")
                    End If
                Else
                    MsgBox("Incorrect Fuse Information")
                End If
            Case 9
                If fuse_int = 111 Then
                    If InStr(outpstring, "reading efuse memory") Then
                        output_state += 1
                    End If
                    If InStr(outpstring, "writing output file") Then
                        output_state += 1
                    End If
                    If InStr(outpstring, "reading hfuse memory") Then
                        output_state += 10
                    End If
                    If InStr(outpstring, "reading lfuse memory") Then
                        output_state += 100
                    End If
                    If InStrRev(outpstring, "writing output file") <> InStr(outpstring, "writing output file") Then
                        output_state += 100
                    End If
                    If output_state = 212 Then
                        MsgBox("AVR Fuses successfully read.")
                    Else
                        MsgBox("Error Reading Fuses")
                    End If
                ElseIf fuse_int = 11 Then
                    If InStr(outpstring, "reading hfuse memory") Then
                        output_state += 1
                    End If
                    If InStr(outpstring, "writing output file") Then
                        output_state += 1
                    End If
                    If InStr(outpstring, "reading lfuse memory") Then
                        output_state += 10
                    End If
                    If InStrRev(outpstring, "writing output file") <> InStr(outpstring, "writing output file") Then
                        output_state += 10
                    End If
                    If output_state = 22 Then
                        MsgBox("AVR Fuses successfully read.")
                    Else
                        MsgBox("Error Reading Fuses")
                    End If
                Else
                    MsgBox("Incorrect Fuse information")
                End If
                output_state = 0
        End Select
    End Sub
    Public Sub read_fuses(ByVal chipname As String, ByRef efuse As String, ByRef hfuse As String, ByRef lfuse As String)
        Dim fldr_fix As Integer = 0
        Dim file_copy As Integer = 0
        Dim file_delete As Integer = 0
        Dim copied_filename As String = ""
        Dim command_string As String = ""
        Dim outputstr As String = ""
        Dim efuse_file As String = ""
        Dim hfuse_file As String = ""
        Dim lfuse_file As String = ""
        Dim fusestring As String = ""

        'If chipname = "" Then
        '    If My.Settings.enablefunnysound = True Then playsnd()
        '    MsgBox("No chip selected")
        '    Exit Sub
        'End If
        If get_avr_signature() = "-1" Then
            'If My.Settings.enablefunnysound = True Then playsnd()
            MsgBox("chip read error...either no chip is connected or the chip is nonresponsive")
            Form1.global_chipname = ""
            Form1.lblavrchip.Text = ""
            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            Exit Sub
        End If
        fldr_fix = folder_fix()
        If fldr_fix <> 0 Then
            efuse_file = folderpath & "\efuse" & Rnd() & ".hex"
            hfuse_file = folderpath & "\hfuse" & Rnd() & ".hex"
            lfuse_file = folderpath & "\lfuse" & Rnd() & ".hex"
            'MessageBox.Show(chipname & vbCrLf & getfuses(chipname).ToString)
            If getfuses(chipname) = 111 Then
                fusestring = " -U efuse:r:" & efuse_file & ":i -U hfuse:r:" & hfuse_file & ":i -U lfuse:r:" & lfuse_file & ":i "
            ElseIf getfuses(chipname) = 11 Then
                fusestring = " -U hfuse:r:" & hfuse_file & ":i -U lfuse:r:" & lfuse_file & ":i "
            End If

            'show_programpic(True)
            Form1.txtoutput.Text = "Please Wait..."
            Form1.set_sck_option()
            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            command_string = " -c " & programmer_hw & " -p " & chipname & fusestring
            'MessageBox.Show("avrdude " & command_string)
            avrdude_command("avrdude", Form1.bitclock & command_string, outputstr)
            Form1.txtoutput.Text = outputstr
            Threading.Thread.Sleep(100)
            'show_programpic(False)
            process_output_string(outputstr, 9, getfuses(chipname))
            'enable_control(1, 1, 1, 1, , 1, 1, 1, , , , 1, 1, 1, 1)
            'Form1.set_fuse_boxes(getfuses(chipname))
            If getfuses(chipname) = 111 Then
                read_fuse_file(efuse_file, efuse)
                read_fuse_file(hfuse_file, hfuse)
                read_fuse_file(lfuse_file, lfuse)
                Threading.Thread.Sleep(100)
                delete_file(efuse_file)
                delete_file(hfuse_file)
                delete_file(lfuse_file)
            Else
                efuse = ""
                read_fuse_file(hfuse_file, hfuse)
                read_fuse_file(lfuse_file, lfuse)
                Threading.Thread.Sleep(100)
                delete_file(hfuse_file)
                delete_file(lfuse_file)
            End If
        Else
            'If My.Settings.enablefunnysound = True Then playsnd()
            MsgBox("temp folder could not be created.")
            Exit Sub
            'Form1.set_fuse_boxes(getfuses(Form1.lblavrchip.Text))
        End If


    End Sub
    Public Sub read_fuse_file(ByVal filepath As String, ByRef fuse_byte As String)
        Dim file_reader As IO.StreamReader
        Dim fileline As String = ""
        If FileIO.FileSystem.FileExists(filepath) Then
            file_reader = New IO.StreamReader(filepath)

            fileline = file_reader.ReadLine
            fuse_byte = "0x" & Mid(fileline, fileline.Length - 3, 2)
            file_reader.Close()
        Else
            '    MsgBox("File Read Error")
        End If
    End Sub
    Public Sub read_calibration_file(ByVal filepath As String, ByRef fuse_byte As String)
        Dim file_reader As IO.StreamReader
        Dim byte_length As Integer = 0
        Dim fileline As String = ""
        If FileIO.FileSystem.FileExists(filepath) Then
            file_reader = New IO.StreamReader(filepath)

            fileline = file_reader.ReadLine
            byte_length = hextodec(Mid(fileline, 2, 2))
            fuse_byte = "0x" & Mid(fileline, fileline.Length - (byte_length * 2) - 1, byte_length * 2)
            file_reader.Close()
        Else
            MsgBox("File Read Error")
        End If
    End Sub
    Public Function getfuses(ByVal uc As String) As Integer
        'On Error GoTo erx
        Dim strx As String, intx As Integer
        Dim output As Integer
        Dim txtfilenm As String = ""
        output = 0

        If FileIO.FileSystem.FileExists(avrdude_filepath) Then
            Using txtstrm As IO.StreamReader = New IO.StreamReader(avrdude_filepath)
                intx = 0

                Do While Not txtstrm.EndOfStream = True
                    strx = txtstrm.ReadLine
                    If strx = "# PART DEFINITIONS" Then
                        intx = 0
                    End If
                    If strx = "# " & uc And intx = 0 Then
                        intx = 1
                    End If
                    If strx = "#------------------------------------------------------------" And intx = 1 Then
                        'txtstrm.SkipLine
                        intx = 2
                        strx = ""
                    End If
                    If intx = 2 Then
                        txtfilenm = txtfilenm & vbCrLf & strx
                    End If
                    If strx = "#------------------------------------------------------------" And intx = 2 Then
                        intx = 3
                        Exit Do
                    End If
                Loop
                'Text1.Text = txtfilenm
                strx = "memory " & Chr(34) & "hfuse" & Chr(34)
                If InStr(txtfilenm, strx) > 0 Then
                    output = output + 1
                End If
                strx = "memory " & Chr(34) & "lfuse" & Chr(34)
                If InStr(txtfilenm, strx) > 0 Then
                    output = output + 10
                End If
                strx = "memory " & Chr(34) & "efuse" & Chr(34)
                If InStr(txtfilenm, strx) > 0 Then
                    output = output + 100
                End If
                intx = -1
                getfuses = output
                'erx:
                'If Err.Number > 0 Then
                'MsgBox Err.Description
                'End If
            End Using
            'Return getfuses
        Else
            Return Nothing
            MsgBox("Reading Error.Set avrdude.conf file path from settings")
        End If

    End Function
    Public Function get_memory_details(ByVal chip As String, ByVal memtype As String) As String
        'On Error GoTo erx
        Dim strx As String, intx As Integer
        Dim output As Integer
        Dim sizeinfo As String = ""
        output = 0

        If FileIO.FileSystem.FileExists(avrdude_filepath) Then
            Using txtstrm As IO.StreamReader = New IO.StreamReader(avrdude_filepath)
                intx = 0

                Do While Not txtstrm.EndOfStream = True
                    strx = txtstrm.ReadLine
                    If strx = "# PART DEFINITIONS" Then
                        intx = 0
                    End If
                    If strx = "# " & chip And intx = 0 Then
                        intx = 1

                    End If
                    If strx = "#------------------------------------------------------------" And intx = 1 Then
                        'txtstrm.SkipLine
                        intx = 2
                    End If
                    If LTrim(strx) = "memory " & Chr(34) & memtype & Chr(34) And intx = 2 Then
                        intx = 3
                    End If
                    If Mid(LTrim(strx), 1, 4) = "size" And intx = 3 Then

                        sizeinfo = Mid(strx, InStr(strx, "=") + 1, (strx.Length - (InStr(strx, "=") + 1)))
                        intx = 4
                    End If
                    If strx = "#------------------------------------------------------------" And intx = 4 Then
                        intx = 5
                        Exit Do
                    End If
                Loop
                'Text1.Text = txtfilenm

                Return LTrim(sizeinfo)
            End Using
        Else
            MsgBox("Reading Error.Set avrdude.conf file path from settings")
        End If
    End Function
    Public Function abs(ByVal number As Integer) As Integer
        If number >= 0 Then
            Return number
        Else
            Return number * -1
        End If
    End Function
    Public Function hextodec(ByVal input_hex As String) As Integer
        Dim input_hex_converted As String = ""
        Dim conversionarray As Integer() = {15, 14, 13, 12, 11, 10}
        Dim i As Integer = 0
        Dim current_digit As Integer = 0
        Dim sum As Integer = 0
        Dim ascval As Integer = 0
        input_hex_converted = RTrim(LTrim(input_hex.ToUpper))
        For i = 1 To input_hex_converted.Length
            ascval = Asc(Mid(input_hex_converted, i, 1))
            If ascval >= 65 Then
                current_digit = conversionarray(70 - ascval) * (16 ^ ((input_hex_converted.Length - i)))
            Else
                current_digit = Val(Mid(input_hex_converted, i, 1)) * (16 ^ (input_hex_converted.Length - i))
            End If
            sum += current_digit
        Next
        Return sum
    End Function
    Public Function dectohex(ByVal input_dec As Integer, Optional ByVal sp_func As Integer = 0) As String
        Dim input_hex_converted As Integer = 0
        Dim str_hex As String = ""
        str_hex = Hex(input_dec)
        If sp_func = 1 And str_hex.Length = 1 Then
            str_hex = "0" & str_hex
        End If
        Return str_hex
    End Function
    Public Function hextobin(ByVal inputhex As String) As String
        Dim inputdec As Integer = 0
        Dim binbstr As String = ""
        Dim divide As Integer = 0
        Dim modulus As Integer = 0
        Dim i As Integer = 0
        Dim outbinarystr As String = ""
        inputdec = hextodec(inputhex)
        'MsgBox(inputdec)
        divide = inputdec / 2
        While (divide > 0)
            modulus = inputdec Mod 2
            divide = Math.Floor(inputdec / 2)
            binbstr = binbstr & modulus
            inputdec = divide
        End While
        For i = binbstr.Length To 1 Step -1
            outbinarystr = outbinarystr & Mid(binbstr, i, 1)
        Next
        Return outbinarystr
    End Function
    Public Function formatsp(ByVal inputstr As String) As String
        If inputstr.Length = 1 Then
            Return "000" & inputstr
        ElseIf inputstr.Length = 2 Then
            Return "00" & inputstr
        ElseIf inputstr.Length = 3 Then
            Return "0" & inputstr
        Else
            Return inputstr
        End If
    End Function
    Public Function getchecksum(ByVal inputline As String) As String
        Dim number_splitted As String = ""
        Dim i As Integer = 0
        Dim sum As Integer = 0
        Dim sumstr As String = ""
        Dim hex_sum_least_significant_byte As String = ""
        Dim twoscomp As Integer = 0
        For i = 2 To inputline.Length - 1 Step 2
            number_splitted = Mid(inputline, i, 2)
            sum += Val(hextodec(number_splitted))
        Next
        sumstr = dectohex(sum)
        If sumstr.Length > 2 Then
            hex_sum_least_significant_byte = Mid(sumstr, sumstr.Length - 1, 2)
        Else
            hex_sum_least_significant_byte = sumstr
        End If
        twoscomp = (255 - hextodec(hex_sum_least_significant_byte)) + 1
        Return dectohex(twoscomp, 1)
    End Function

    Public Sub load_chip_list(ByVal destination As ComboBox)
        Dim line_Str As String = ""
        Dim chip_Str As String = ""
        If FileIO.FileSystem.FileExists(Mid(Application.ExecutablePath, 1, Application.ExecutablePath.Length - 15) & "\signature_data.txt") Then
            Using filestrm As IO.StreamReader = New IO.StreamReader(Mid(Application.ExecutablePath, 1, Application.ExecutablePath.Length - 15) & "\signature_data.txt")
                While Not filestrm.EndOfStream = True
                    line_Str = filestrm.ReadLine
                    chip_Str = Mid(line_Str, 1, InStr(line_Str, ",") - 1)
                    destination.Items.Add(chip_Str)
                End While
            End Using
        End If
    End Sub

    Public Function predef_folder_fix() As Boolean
        If IO.Directory.Exists(predef_folder) Then
            Return True
        Else
            Try
                IO.Directory.CreateDirectory(predef_folder)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End If
    End Function
    Public Function set_fuse_file_value(ByVal mcuname As String, ByVal fusepredefname As String, ByVal fusearray() As String, ByVal numelements As Integer) As Integer
        If predef_folder_fix() = False Then
            Return -1
        End If
        Dim filestring As String = ""
        Dim filename As String = predef_folder & "\" & mcuname & "fusepredef.dat"
        Try
            Using strmreader As IO.StreamReader = New IO.StreamReader(filename, True)
                Dim filestr As String = ""
                filestr = strmreader.ReadToEnd
                If InStr(filestr, fusepredefname) Then
                    strmreader.Close()
                    Return -3
                End If
                strmreader.Close()
            End Using
        Catch ex As Exception

        End Try
        Try
            Using strmwriter As IO.StreamWriter = New IO.StreamWriter(filename, True)
                Dim i As Integer = 0
                filestring = fusepredefname
                For i = 0 To (numelements - 1)
                    filestring = filestring & "," & fusearray(i)
                Next
                strmwriter.WriteLine(filestring)
                strmwriter.Close()
            End Using
        Catch ex As Exception
            Return -1
        End Try
        Return 1
    End Function
    Public Function load_fuse_predefs(ByVal mcuname As String, ByVal cbopredeflist As ComboBox)
        Dim filename As String = predef_folder & "\" & mcuname & "fusepredef.dat"
        Dim linestr As String = ""
        Dim predefbox As String = ""
        If FileIO.FileSystem.FileExists(filename) Then
            Using strmreader As IO.StreamReader = New IO.StreamReader(filename)
                cbopredeflist.Items.Clear()
                Do While Not strmreader.EndOfStream
                    linestr = strmreader.ReadLine
                    predefbox = Mid(linestr, 1, InStr(linestr, ",") - 1)
                    cbopredeflist.Items.Add(predefbox)
                Loop
                Return 1
            End Using
        Else
            Return -1
        End If
    End Function
    Public Function read_fuse_predefs(ByVal mcuname As String, ByVal lstndex As Integer, ByRef fusearray As String)
        Dim filename As String = predef_folder & "\" & mcuname & "fusepredef.dat"
        Dim linestr As String = ""
        Dim fusecount As Integer = getfuses(mcuname)
        Dim n As Integer = 0
        Dim i As Integer = 0
        If FileIO.FileSystem.FileExists(filename) Then
            Using strmreader As IO.StreamReader = New IO.StreamReader(filename)
                Do While Not strmreader.EndOfStream
                    linestr = strmreader.ReadLine
                    n = n + 1
                    If n = lstndex + 1 Then
                        fusearray = Mid(linestr, InStr(linestr, ",") + 1)
                    End If
                Loop
                Return n
            End Using
        Else
            Return -1
        End If
    End Function
    Public Sub fixfuse(ByVal src As TextBox)
        Dim srcx As String = src.Text
        If InStr(src.Text, "0x") <> 1 Then
            src.Text = "0x" & srcx
        End If
    End Sub
End Class
