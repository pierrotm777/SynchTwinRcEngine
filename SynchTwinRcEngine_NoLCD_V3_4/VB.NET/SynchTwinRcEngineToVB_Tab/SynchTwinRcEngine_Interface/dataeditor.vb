Imports System.Reflection
Imports System.CodeDom.Compiler
Imports Microsoft.VisualBasic

Public Class dataeditor
    Dim avrdude As New libavrdude
    Dim if_checked As Boolean

    Private Sub DataGridView1_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles DataGridView1.CellBeginEdit

    End Sub
    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub

    Private Sub DataGridView1_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellEndEdit
        Dim strbyte As String = DataGridView1.Item(e.ColumnIndex, e.RowIndex).Value.ToString
        If avrdude.verify_fuse_byte("0x" & strbyte) <> 1 Then
            MsgBox("Invalid byte entered at row " & e.RowIndex + 1 & " and column " & e.ColumnIndex + 1 & ".It should be XX where XX means the hex value and if your byte has got only one character then it should be written" & vbCrLf & "in 0X form.Wrong bytes are marked with red color.")
            DataGridView1.Item(e.ColumnIndex, e.RowIndex).Style.ForeColor = Color.Red
        Else
            DataGridView1.Item(e.ColumnIndex, e.RowIndex).Style.ForeColor = Color.Black
        End If
    End Sub

    Private Sub DataGridView1_RowsAdded(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowsAddedEventArgs) Handles DataGridView1.RowsAdded
        Dim a As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            a = i * 8

            DataGridView1.Rows(i).HeaderCell.Value = Hex(a).ToString
        Next i
    End Sub
    Private Sub load_eeprom_data(ByVal filename As String)
        If filename <> "" Then
            Dim strm As IO.StreamReader = New IO.StreamReader(filename)
            'begin file_processing
            Dim line_str As String = ""
            Dim address_Str As String = ""
            Dim data_content As String = ""
            Dim byte_count As String = ""
            Dim hex_data As String = ""
            Dim byte_count_int As Integer = 0
            Dim byte_per_16byte As String = ""
            Dim address As Integer = 0
            Dim spread_sheet_row As Integer = 0
            Dim data_content1 As String = ""
            Dim data_content2 As String = ""
            Dim data_content3 As String = ""
            Dim data_content4 As String = ""
            While Not strm.EndOfStream = True
                line_str = RTrim(LTrim(strm.ReadLine))
                byte_count = Mid(line_str, 2, 2)
                byte_count_int = avrdude.hextodec(byte_count)
                'MsgBox(byte_count_int)
                If (Mid(line_str, 2, 2)) <> "00" Then
                    address_Str = Mid(line_str, 4, 4)
                    address = avrdude.hextodec(address_Str)
                    data_content = Mid(line_str, 10, byte_count_int * 2)
                    hex_data = hex_data & vbCrLf & (address_Str & "(" & address & ")" & "  " & data_content)
                    'here,we assume that the number of bytes will be either less than 8 or 8 or less 16 bytes or 16bytes
                    'our spreadsheet is 8byte based so we need to split it
                    'assuming a 16 byte data page
                    Dim column_cnt As Integer = 0
                    Try
                        If byte_count_int > 16 Then
                            Dim addr2 As Integer = 0
                            Dim addr3 As Integer = 0
                            Dim addr4 As Integer = 0
                            data_content1 = Mid(data_content, 1, 16)
                            data_content2 = Mid(data_content, 17, 16)
                            data_content3 = Mid(data_content, 33, 16)
                            data_content4 = Mid(data_content, 49, 16)
                            address = address / 8
                            addr2 = address + 1
                            addr3 = address + 2
                            addr4 = address + 3
                            'MsgBox(address & " " & data_content1 & vbCrLf & addr2 & " " & data_content2 & vbCrLf & addr3 & " " & data_content3 & vbCrLf & addr4 & " " & data_content4)
                            For column_cnt = 0 To 7
                                If avrdude.verify_fuse_byte("0x" & Mid(data_content1, 1 + column_cnt * 2, 2)) <> 1 Then
                                    MsgBox("Invalid Byte found.The file could not be loaded.")
                                    reset()
                                    Try
                                        strm.Close()
                                    Catch ex As Exception

                                    End Try
                                    Exit Sub
                                End If
                                DataGridView1.Item(column_cnt, address).Value = Mid(data_content1, 1 + column_cnt * 2, 2)
                            Next
                            For column_cnt = 0 To 7
                                If avrdude.verify_fuse_byte("0x" & Mid(data_content2, 1 + column_cnt * 2, 2)) <> 1 Then
                                    MsgBox("Invalid Byte found.The file could not be loaded.")
                                    reset()
                                    Try
                                        strm.Close()
                                    Catch ex As Exception

                                    End Try
                                    Exit Sub
                                End If
                                DataGridView1.Item(column_cnt, addr2).Value = Mid(data_content2, 1 + column_cnt * 2, 2)
                            Next
                            If byte_count_int < 24 Then
                                For column_cnt = 0 To (byte_count_int - 17)
                                    If avrdude.verify_fuse_byte("0x" & Mid(data_content3, 1 + column_cnt * 2, 2)) <> 1 Then
                                        MsgBox("Invalid Byte found.The file could not be loaded.")
                                        reset()
                                        Try
                                            strm.Close()
                                        Catch ex As Exception

                                        End Try
                                        Exit Sub
                                    End If
                                    DataGridView1.Item(column_cnt, addr3).Value = Mid(data_content3, 1 + column_cnt * 2, 2)
                                Next
                            Else
                                For column_cnt = 0 To 7
                                    If avrdude.verify_fuse_byte("0x" & Mid(data_content3, 1 + column_cnt * 2, 2)) <> 1 Then
                                        reset()
                                        MsgBox("Invalid Byte found.The file could not be loaded.")
                                        Try
                                            strm.Close()
                                        Catch ex As Exception

                                        End Try
                                        Exit Sub
                                    End If
                                    DataGridView1.Item(column_cnt, addr3).Value = Mid(data_content3, 1 + column_cnt * 2, 2)
                                Next
                                'MsgBox("writing")
                                For column_cnt = 0 To Math.Abs(byte_count_int - 25)
                                    If avrdude.verify_fuse_byte("0x" & Mid(data_content4, 1 + column_cnt * 2, 2)) <> 1 Then
                                        MsgBox("Invalid Byte found.The file could not be loaded.")
                                        reset()
                                        Try
                                            strm.Close()
                                        Catch ex As Exception

                                        End Try
                                        Exit Sub
                                    End If
                                    'MsgBox(Mid(data_content4, 1 + column_cnt * 2, 2))
                                    DataGridView1.Item(column_cnt, addr4).Value = Mid(data_content4, 1 + column_cnt * 2, 2)
                                Next
                            End If
                        ElseIf byte_count_int > 8 And byte_count_int <= 16 Then
                            Dim addr2 As Integer = 0
                            data_content1 = Mid(data_content, 1, 16)
                            data_content2 = Mid(data_content, 17, (byte_count_int * 2) - 16)
                            address = address / 8
                            addr2 = address + 1
                            For column_cnt = 0 To 7
                                If avrdude.verify_fuse_byte("0x" & Mid(data_content1, 1 + column_cnt * 2, 2)) <> 1 Then
                                    MsgBox("Invalid Byte found.The file could not be loaded.")
                                    reset()
                                    Try
                                        strm.Close()
                                    Catch ex As Exception

                                    End Try
                                    Exit Sub
                                End If
                                DataGridView1.Item(column_cnt, address).Value = Mid(data_content1, 1 + column_cnt * 2, 2)
                            Next
                            For column_cnt = 0 To (byte_count_int - 9)
                                If avrdude.verify_fuse_byte("0x" & Mid(data_content2, 1 + column_cnt * 2, 2)) <> 1 Then
                                    MsgBox("Invalid Byte found.The file could not be loaded.")
                                    reset()
                                    Try
                                        strm.Close()
                                    Catch ex As Exception

                                    End Try
                                    Exit Sub
                                End If
                                DataGridView1.Item(column_cnt, addr2).Value = Mid(data_content2, 1 + column_cnt * 2, 2)
                            Next
                        Else
                            address = address / 8
                            For column_cnt = 0 To (byte_count_int - 1)
                                If avrdude.verify_fuse_byte("0x" & Mid(data_content, 1 + column_cnt * 2, 2)) <> 1 Then
                                    MsgBox("Invalid Byte found.The file could not be loaded.")
                                    reset()
                                    Try
                                        strm.Close()
                                    Catch ex As Exception

                                    End Try
                                    Exit Sub
                                End If
                                DataGridView1.Item(column_cnt, address).Value = Mid(data_content, 1 + column_cnt * 2, 2)
                            Next
                        End If
                       
                    Catch ex As Exception
                        MessageBox.Show(ex.Message, "eeprom error")
                        reset()
                        Exit While
                    End Try

                End If
            End While
            Try
                strm.Close()
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub cmdopen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdopen.Click
        Dim filebuffer As String = ""
        ofd.Filter = "Hex files|*.hex|EEP Files|*.eep"
        ofd.ShowDialog()
        If ofd.FileName <> "" Then
            If FileIO.FileSystem.FileExists(ofd.FileName) Then load_eeprom_data(ofd.FileName)
        End If
    End Sub
    Private Sub reset()
        Dim flash_mem As String = ""
        Dim eeprom_mem As String = ""
        Dim flash_mem_int As Integer = 0
        Dim eeprom_mem_int As Integer = 0
        Dim i As Integer = 0
        Dim value_of_row_header As Integer = 0
        lblmcu.Text = cbochiplist.Text
        flash_mem = avrdude.get_memory_details(lblmcu.Text, "flash")
        eeprom_mem = avrdude.get_memory_details(lblmcu.Text, "eeprom")
        lblflash.Text = flash_mem
        lbleeprom.Text = eeprom_mem
        eeprom_mem_int = Int(eeprom_mem)
        DataGridView1.Rows.Clear()
        For flash_mem_int = 1 To (eeprom_mem_int / 8)
            DataGridView1.Rows.Add()
            'dataeditor.DataGridView1.Rows.Item(flash_mem_int - 1).HeaderCell.Style.Alignment =DataGridViewContentAlignment.MiddleCenter

            For i = 0 To 7
                DataGridView1.Item(i, flash_mem_int - 1).Value = "FF"
            Next
            DataGridView1.Rows.Item(flash_mem_int - 1).HeaderCell.Value = Hex((flash_mem_int - 1) * 8).ToString
        Next
    End Sub

    Private Sub dataeditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        avrdude.load_chip_list(cbochiplist)
    End Sub

    Private Sub GroupBox2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox2.Enter

    End Sub

    Private Sub cbochiplist_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbochiplist.SelectedIndexChanged
        'MsgBox(dataeditor.DataGridView1.RowCount)
    End Sub

    Private Sub cbochiplist_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbochiplist.SelectedValueChanged
    End Sub

    Private Sub cbochiplist_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbochiplist.TextChanged
        Dim flash_mem As String = ""
        Dim eeprom_mem As String = ""
        Dim flash_mem_int As Integer = 0
        Dim eeprom_mem_int As Integer = 0
        Dim i As Integer = 0
        Dim value_of_row_header As Integer = 0
        lblmcu.Text = cbochiplist.Text
        flash_mem = avrdude.get_memory_details(lblmcu.Text, "flash")
        eeprom_mem = avrdude.get_memory_details(lblmcu.Text, "eeprom")
        lblflash.Text = flash_mem
        lbleeprom.Text = eeprom_mem
        eeprom_mem_int = Int(eeprom_mem)
        DataGridView1.Rows.Clear()
        For flash_mem_int = 1 To (eeprom_mem_int / 8)
            DataGridView1.Rows.Add()
            'dataeditor.DataGridView1.Rows.Item(flash_mem_int - 1).HeaderCell.Style.Alignment =DataGridViewContentAlignment.MiddleCenter

            For i = 0 To 7
                DataGridView1.Item(i, flash_mem_int - 1).Value = "FF"
            Next
            DataGridView1.Rows.Item(flash_mem_int - 1).HeaderCell.Value = Hex((flash_mem_int - 1) * 8).ToString
        Next

    End Sub

    Private Sub cmdsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdsave.Click
        Dim filebuffer As String = ""
        Dim rowcounter As Integer = 0
        Dim columncounter As Integer = 0
        Dim scanline1 As Integer = 0
        Dim scanline2 As Integer = 0
        Dim tobewritten As Integer = 0
        Dim strbyte1 As String = ""
        Dim strbyte2 As String = ""
        Dim strline1 As String = ""
        Dim strline2 As String = ""
        Dim strfinalline As String = ""
        Dim strfinal As String = ""
        Dim n As Integer = 0
        For rowcounter = 1 To (Val(RTrim(LTrim(lbleeprom.Text))) / 8) Step 2
            For columncounter = 0 To 7
                strbyte1 = DataGridView1.Item(columncounter, rowcounter - 1).Value
                strbyte2 = DataGridView1.Item(columncounter, rowcounter).Value
                DataGridView1.Item(columncounter, rowcounter - 1).Style.ForeColor = Color.Black
                DataGridView1.Item(columncounter, rowcounter).Style.ForeColor = Color.Black
                If strbyte1 <> "FF" Then
                    tobewritten = 1
                End If
                If strbyte2 <> "FF" Then
                    tobewritten = 1
                End If
            Next
            If tobewritten = 1 Then
                For columncounter = 0 To 7
                    strbyte1 = DataGridView1.Item(columncounter, rowcounter - 1).Value
                    strbyte2 = DataGridView1.Item(columncounter, rowcounter).Value
                    If avrdude.verify_fuse_byte("0x" & strbyte1) <> 1 Then
                        MsgBox("Invalid byte entered at row " & rowcounter & " and column " & columncounter + 1 & ".It should be XX where XX means the hex value and if your byte has got only one character then it should be written" & vbCrLf & "in 0X form.Wrong bytes are marked with red color.")
                        DataGridView1.Item(columncounter, rowcounter - 1).Style.ForeColor = Color.Red
                        Exit Sub
                    End If
                    If avrdude.verify_fuse_byte("0x" & strbyte2) <> 1 Then
                        MsgBox("Invalid byte entered at row " & rowcounter + 1 & " and column " & columncounter + 1 & ".It should be XX where XX means the hex value and if your byte has got only one character then it should be written" & vbCrLf & "in 0X form.Wrong bytes are marked with red color.")
                        DataGridView1.Item(columncounter, rowcounter).Style.ForeColor = Color.Red
                        Exit Sub
                    End If
                    strline1 = strline1 & strbyte1
                    strline2 = strline2 & strbyte2
                Next
                strfinalline = ":10" & avrdude.formatsp(DataGridView1.Rows.Item(rowcounter - 1).HeaderCell.Value) & "00" & strline1 & strline2
                strfinalline = strfinalline & avrdude.getchecksum(strfinalline)
                If n > 0 Then
                    strfinal = strfinal & vbCrLf & strfinalline
                Else
                    strfinal = strfinalline
                End If
                strline1 = ""
                strline2 = ""
            End If
            tobewritten = 0
            n = n + 1
        Next
        strfinal = strfinal & vbCrLf & ":00000001FF"
        sfd.Filter = "Hex files|*.hex|EEP Files|*.eep"
        sfd.ShowDialog()
        If sfd.FileName <> "" Then
            Try
                Using strm_writer As IO.StreamWriter = New IO.StreamWriter(sfd.FileName)
                    strm_writer.Write(strfinal)
                    strm_writer.Close()
                End Using

                If if_checked = True Then
                    'Form1.rdowrveeprom.Select()
                    Form1.txtfilename.Text = sfd.FileName
                    MsgBox("File Saved.Now this dialog will close and the eeprom file location will be loaded in the file name textbox.You just have to click Write to chip button")
                    Me.Close()
                Else
                    MsgBox("File Saved.")
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        End If
    End Sub

    Private Sub tmr_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmr.Tick
        If lblmcu.Text = "" Then
            cmdopen.Enabled = False
            cmdsave.Enabled = False
        Else
            cmdopen.Enabled = True
            cmdsave.Enabled = True
        End If
        If Form1.lblavrchip.Text <> "" Then
            chksaveclose.Enabled = True
            cmdreadeeprom.Enabled = True
        Else
            cmdreadeeprom.Enabled = False
            chksaveclose.Enabled = False
        End If
    End Sub

    Private Sub eepromtab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles eepromtab.Click

    End Sub

    Private Sub chksaveclose_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chksaveclose.CheckedChanged
        If chksaveclose.Checked Then
            cmdsave.Text = "Save and write"
            if_checked = True
        Else
            if_checked = False
            cmdsave.Text = "Save..."
        End If
    End Sub

    Private Sub cmdreadeeprom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdreadeeprom.Click
        Dim fldr_fix As Integer = 0
        Dim file_copy As Integer = 0
        Dim file_delete As Integer = 0
        Dim destination_file As String = ""
        Dim command_string As String = ""
        Dim outputstr As String = ""
        Dim destination As String = ""
        Dim copyfile As Integer = 0
        Dim tempfilenm As String = ""
        If Form1.global_chipname = "" Then
            MsgBox("No chip selected, use 'Read' before !")
            Exit Sub
        End If
        If avrdude.get_avr_signature = "-1" Then
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("chip read error...either no chip is connected or the chip is nonresponsive")
            Form1.global_chipname = ""
            Form1.lblavrchip.Text = ""
            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            Exit Sub
        End If
        destination = "eeprom"
        fldr_fix = avrdude.folder_fix()
        If fldr_fix <> 0 Then
            cmdopen.Enabled = False
            cmdsave.Enabled = False
            chksaveclose.Enabled = False
            cmdreadeeprom.Enabled = False
            tempfilenm = libavrdude.folderpath & "\readfile" & Rnd() & ".eep"
            'show_programpic(True)
            Form1.txtoutput.Text = "Please Wait..."
            Form1.set_sck_option()
            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            command_string = " -c " & programmer_hw & " -p " & Form1.global_chipname & " -U " & destination & ":r:" & tempfilenm & ":i"
            avrdude.avrdude_command("avrdude", Form1.bitclock & command_string, outputstr)
            Form1.txtoutput.Text = outputstr
            Threading.Thread.Sleep(100)
            'show_programpic(False)
            avrdude.process_output_string(outputstr, 7)
            'enable_control(1, 1, 1, 1, , , 1, , , , , 1, 1, 1, 1)
            load_eeprom_data(tempfilenm)
            Try
                FileIO.FileSystem.DeleteFile(tempfilenm)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("Error creating folder")
        End If
        cmdopen.Enabled = True
        cmdsave.Enabled = True
        chksaveclose.Enabled = True
        cmdreadeeprom.Enabled = True
        'Form1.set_fuse_boxes(Form1.set_avr_fuses)
    End Sub

End Class