Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Imports AquaControls.AquaGauge
Imports ZedGraph
Imports System.Globalization

Imports System.Text
Imports System.ComponentModel


Public Class Form1

    Dim myCurve As LineItem
    Dim myPane As GraphPane
    Dim list As New PointPairList
    Dim list2 As New PointPairList
    Dim ModuleVersion As String = ""
    Dim InterfaceVersion As String = "0.6"
    Dim GraphIsReady As Boolean
    Dim GlowPlugIsOn As Boolean
    Dim RecorderIsOn As Boolean
    Dim PlayerIsOn As Boolean
    Dim SimualtionSpeed As Boolean = False
    Dim SDCardUsed As Boolean = False
    Dim avrdude As New libavrdude
    Dim brdsetok As Boolean
    Dim baud As String = ""
    Dim protocol As String = ""
    Dim mcu As String = ""
    Dim update_port_enabled As Boolean = True
    Public bitclock As String = ""
    Dim BaudRates() As String = {"9600", "14400", "19200", "28800", "38400", "57600", "115200"}

    Dim term As New Terminal
    Dim RcMode As New RcRadioMode

    Public RcModeRadio As String
    Public RcSignalType As String

    Dim servoPosition() As String
    Dim RecordFilesPath As String = Application.StartupPath & "\"

    Dim trd As Thread



#Region "COM Port série"
    Private Sub ButtonTerminal_Click(sender As System.Object, e As System.EventArgs) Handles ButtonTerminal.Click
        If term.Visible = False Then
            term.Show()
            term.Location = New Point(My.Settings.LocationX + Me.Width + 3, My.Settings.LocationY)
        Else
            term.Hide()
        End If
    End Sub

    Private Sub Button_Connect_Click(sender As System.Object, e As System.EventArgs) Handles Button_Connect.Click
        Try
            If term.Visible = True Then
                If My.Settings.Language = "French" Then
                    term.TextBoxTerminalComPort.AppendText(My.Resources.labelTerminalWelcome_FR & vbCrLf)
                ElseIf My.Settings.Language = "English" Then
                    term.TextBoxTerminalComPort.AppendText(My.Resources.labelTerminalWelcome_EN & vbCrLf)
                End If

            End If
            If SerialPort1.IsOpen Then
                PictureBoxConnectedOK.Image = My.Resources.rectangle_rouge
                SerialPort1.Close()
                'SerialPort1.Dispose()
                ShowMsg("Com Port " & My.Settings.COMPort & " is closed !!!", ShowMsgImage.Info, "Info")
                labelInfoNeedSaveCOM.Visible = False
            Else
                SerialPort1.Close()
                SerialPort1.Handshake = Handshake.None
                SerialPort1.Encoding = System.Text.Encoding.Default
                SerialPort1.ReadTimeout = 10000
                SerialPort1.DtrEnable = True 'nécessaire avec Leonardo ou Pro Micro
                SerialPort1.RtsEnable = True 'nécessaire avec Leonardo ou Pro Micro
                SerialPort1.Open()

                'SerialPort1.Write(vbCrLf) 'initialise le module en mode configuration

                PictureBoxConnectedOK.Image = My.Resources.rectangle_vert
                Thread.Sleep(500)

                SerialPort1.Write("0" & vbCr) 'positionne les moteurs à la postion 0°
                ButtonAuxMiddle.PerformClick()

                Thread.Sleep(500)
                SerialPort1.Write("999" & vbCr) 'read version module

                Thread.Sleep(500)
                SerialPort1.Write("998" & vbCr) 'read if the external voltage is read or not

            End If
            labelInfoNeedSaveCOM.Text = ""
        Catch ex As Exception
            PictureBoxConnectedOK.Image = My.Resources.rectangle_rouge
            'ShowMsg(ex.Message, ShowMsgImage.Alert, "Error")

            'MsgBox("1) Le module n'est pas connecté !!!" & vbCrLf & "2) Choisissez le bon port série et sauvegardez !!!" & vbCrLf & "Connectez le module et redémarrez !!!", vbOK)
        End Try

    End Sub

    Private Sub ButtonSauvegardeCOM_Click(sender As System.Object, e As System.EventArgs) Handles ButtonSauvegardeCOM.Click
        My.Settings.COMPort = ComboPort.SelectedItem.ToString
        My.Settings.SpeedIndex = Convert.ToByte(ComboBaudRate.SelectedIndex)
        My.Settings.Save()
    End Sub

    Private Sub ComboBaudRate_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboBaudRate.SelectedIndexChanged
        labelInfoNeedSaveCOM.Text = "You need to save the connection Settings (use 'Backup' button) !"
    End Sub


    ''' <summary>
    ''' Update the com port list after a click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ComboPort_MouseClick(sender As System.Object, e As System.EventArgs) Handles ComboPort.MouseClick
        labelInfoNeedSaveCOM.Text = "You need to save the connection Settings (use 'Backup' button) !"
        Try
            ComboPort.Items.Clear()
            For Each sport As String In SerialPort.GetPortNames
                ComboPort.Items.Add(sport)
            Next
        Catch
            ShowMsg("Aucun port trouvé", ShowMsgImage.Info, "Error")
        End Try

    End Sub
    Private Sub ComboPort_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles ComboPort.SelectedIndexChanged
        labelInfoNeedSaveCOM.Text = "You need to save the connection Settings (use 'Backup' button) !"
    End Sub

    Private Sub SerialPort1_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        'This sub gets called automatically when the com port recieves some data

        'Pause while all data is read
        Threading.Thread.Sleep(100) 'Threading.Thread.Sleep(300)

        'Move recieved data into the buffer
        Dim SerialMessagRecieved As String = SerialPort1.ReadExisting.Replace(vbCrLf, "")

        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText(SerialMessagRecieved & vbCrLf)
        End If


        Dim master As String = ""
        Dim slave As String = ""
        If My.Settings.Language = "French" Then
            master = "Maître"
            slave = "Esclave"
        ElseIf My.Settings.Language = "English" Then
            master = "Master"
            slave = "Slave"
        End If

        If Strings.Left(SerialMessagRecieved, 6) = "POWER|" Then 'return if external power is used or not
            Try
                If SerialMessagRecieved.Replace("POWER|", "") = "1" Then
                    TextVoltageExterne.Visible = True
                    ButtonReadTempVoltage.Visible = True
                    PictureBoxReadHardwareOnOff.Visible = True
                    labelExtervalVoltageUsed.Visible = False
                ElseIf SerialMessagRecieved.Replace("POWER|", "") = "0" Then
                    TextVoltageExterne.Visible = False
                    ButtonReadTempVoltage.Visible = False
                    PictureBoxReadHardwareOnOff.Visible = False
                    labelExtervalVoltageUsed.Visible = True
                End If
                If term.Visible = True Then
                    term.TextBoxTerminalComPort.AppendText("VB is Ready ..." & vbCrLf)
                End If
            Catch ex As Exception
                ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
            End Try
        End If

        If Strings.Left(SerialMessagRecieved, 5) = "FIRM|" Then 'return firmware's module version
            Try
                ModuleVersion = ""
                ModuleVersion = SerialMessagRecieved.Replace("FIRM|", "")
                labelInfoNeedSaveCOM.Visible = True
                labelInfoNeedSaveCOM.Text = "Version Module is: V" & ModuleVersion
            Catch ex As Exception
                ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
            End Try
        End If

        'If Strings.Left(SerialMessagRecieved, 3) = "SD|" Then 'return sd card is used
        '    Try
        '        If SerialMessagRecieved.Replace("SD|", "") = "0" Then
        '            SDCardUsed = False
        '            labelFRAMIsUsed.Text = "SD Card Used: NO"
        '            labelFRAMType.Text = "SD Card Type: NO"
        '            'labelSDCardSize.Text = "SD Card Size: NO"
        '            labelSDCardSize.Text = "SD Card FAT: NO"
        '        ElseIf SerialMessagRecieved.Replace("SD|", "") = "1" Then
        '            SDCardUsed = True
        '            labelFRAMIsUsed.Text = "SD Card Used: YES"
        '        End If

        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If
        'If Strings.Left(SerialMessagRecieved, 3) = "SD1" Then 'return sd is v1 type
        '    Try
        '        labelFRAMType.Text = "SD Card Type: SD1"
        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If
        'If Strings.Left(SerialMessagRecieved, 3) = "SD2" Then 'return sd is v2 type
        '    Try
        '        labelFRAMType.Text = "SD Card Type: SD2"
        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If
        'If Strings.Left(SerialMessagRecieved, 3) = "SDH" Then 'return sd is sdhc type
        '    Try
        '        labelFRAMType.Text = "SD Card Type: SDHC"
        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If
        'If Strings.Left(SerialMessagRecieved, 3) = "SDU" Then 'return sd is unknow type
        '    Try
        '        labelFRAMType.Text = "SD Card Type: Unknow"
        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If
        'If Strings.Left(SerialMessagRecieved, 4) = "SDN|" Then 'return sd isn't formatted
        '    Try
        '        'ShowMsg("You need to format the SD Card (FAT or FAT32 only)!!!", ShowMsgImage.Critical, "Erreur")
        '        labelSDCardSize.Text = "SD Card FAT: NO FAT !!!"
        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If
        'If Strings.Left(SerialMessagRecieved, 4) = "SDF|" Then 'return sd format type
        '    Try
        '        labelSDCardSize.Text = "SD Card FAT: " & SerialMessagRecieved.Replace("SDF|", "")
        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If
        'If Strings.Left(SerialMessagRecieved, 4) = "SDL|" Then 'return list
        '    Try
        '        Dim list As String = SerialMessagRecieved.Replace("SDL|", "")
        '        ListBoxSDListFiles.Items.Add(list)
        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If
        If Strings.Left(SerialMessagRecieved, 6) = "NOFRAM" Then 'fram isn't used
            Try
                SDCardUsed = False
                labelFRAMIsUsed.Text = "FRAM Used: NOT DEFINE"
                labelFRAMType.Text = "FRAM Type: NOT DEFINE"
                labelSDCardSize.Text = "FRAM Size: NOT DEFINE"

            Catch ex As Exception
                ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
            End Try
        End If

        'If Strings.Left(SerialMessagRecieved, 7) = "SDRErr|" Then
        '    Try
        '        Dim SDData As String = SerialMessagRecieved.Replace("SDRErr|", "")
        '        If SDData = "0" Then
        '            'SDCardUsed = True
        '            MessageBox.Show("Read Data from SD Card return no file found !")
        '        ElseIf SDData <> "0" Then
        '            'SDCardUsed = True
        '            MessageBox.Show("File " & SDData & " found on SD card !")
        '        End If
        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If

        'If Strings.Left(SerialMessagRecieved, 7) = "SDWErr|" Then
        '    Try
        '        Dim Err As String = SerialMessagRecieved.Replace("SDWErr|", "")
        '        If Err = "0" Then
        '            MessageBox.Show("Write Data to SD Card return no file found !")
        '        ElseIf Err <> "0" Then
        '            MessageBox.Show(Err)
        '        End If
        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If

        'list files in SD card
        'If Strings.Left(SerialMessagRecieved, 7) = "SDData|" Then 'receive SDData|DATALOG.TXT   2000-01-01 01:00:00 59866
        '    Try
        '        Dim Err As String = SerialMessagRecieved.Replace("SDData", "").Replace("   ", " ")

        '        'If GrahIsEnable = True Then 'lecture DataLogger
        '        ' Make up some data points based on the Sine function
        '        Dim i As Integer, x As Double ', y As Double, y2 As Double
        '        If i < 10000 Then
        '            x = New XDate(DateTime.Now) '(i + 11)
        '            list.Add(x, Convert.ToInt32(SevenSegmentArray1.Value))
        '            list2.Add(x, Convert.ToInt32(SevenSegmentArray2.Value))
        '            i = i + 1

        '        End If
        '        GraphIsReady = True

        '    Catch ex As Exception
        '        ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
        '    End Try
        'End If

        'lecture config du module arduino 
        If Strings.Left(SerialMessagRecieved, 3) = "LLA" Then 'return module's settings
            Try
                array = SerialMessagRecieved.Replace("LLA", "").Split("|")

                textCentreServo1.Text = array(0)
                textCentreServo2.Text = array(1)
                textIdleServo1.Text = array(2)
                textIdleServo2.Text = array(3)
                textTempsReponse.Text = array(4)
                textMaxiMoteurs.Text = array(5)
                textDebutSynchro.Text = array(6)
                textMiniGenerale.Text = array(7)
                textMaxiGenerale.Text = array(8)
                textAuxiliaireMode.Text = array(9)
                If array(10) = "1" Then CheckBoxInversionServo1.Checked = True Else CheckBoxInversionServo1.Checked = False
                If array(11) = "1" Then CheckBoxInversionServo2.Checked = True Else CheckBoxInversionServo2.Checked = False
                TextBoxDiffSpeedSimuConsigne.Text = array(12)
                If TextBoxDiffSpeedSimuConsigne.Text.Contains(".") Then
                    Dim s() As String = TextBoxDiffSpeedSimuConsigne.Text.Split(".")
                    TextBoxDiffSpeedSimuConsigne.Text = s(0)
                End If

                textNombrePales.Text = array(14)

                labelModeRcRadio.Text = array(15) + 1
                RcMode.ComboBox_Select_ModeType.Text = array(15) + 1

                TextVoltageInterne.Text = array(16) & "v"
                TextTempInterne.Text = array(17) & IIf(array(20) = "0", "°C", "°F")
                CheckBoxFahrenheitDegrees.Checked = IIf(array(20) = "0", False, True)

                If array(18) <> "NOTUSED" Then
                    labelExtervalVoltageUsed.Visible = False
                    TextVoltageExterne.Visible = True
                    TextVoltageExterne.Text = array(18) & "v"
                    labelExtervalVoltageUsed.Text = ""
                Else
                    labelExtervalVoltageUsed.Visible = True
                    TextVoltageExterne.Visible = False
                    labelExtervalVoltageUsed.Text = "Not Used"
                End If

                ButtonModuleType.Text = IIf(array(19) = "0", master, slave)

                Me.Text = "SynchTwinRcEngine Programer (module " & IIf(array(19) = 0, master & ")", slave & ")") 'type de module ajouté au titre

                If array(20) = "0" Then CheckBoxFahrenheitDegrees.Checked = False Else CheckBoxFahrenheitDegrees.Checked = True
                CheckBoxFahrenheitDegrees.Text = IIf(array(20) = "0", "°C", "°F")

                textMiniMotorRPM.Text = array(21)
                textMaxiMotorRPM.Text = array(22)

                Select Case array(23)
                    Case 0
                        LabelSignalType.Text = "CPPM"
                    Case 1
                        LabelSignalType.Text = "SBUS"
                    Case 2
                        LabelSignalType.Text = "SRXL"
                    Case 3
                        LabelSignalType.Text = "SUMD"
                    Case 4
                        LabelSignalType.Text = "IBUS"
                    Case 5
                        LabelSignalType.Text = "JETI"
                End Select
                RcMode.ComboBoxSignalType.SelectedIndex = array(23)

                LabelModifications.Enabled = False
                LabelModifications.Text = "..."

                LabelInterType.Text = ModeAuxiliaireTypeText(Convert.ToInt16(textAuxiliaireMode.Text))

            Catch ex As Exception
                ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
            End Try


        End If

        'lecture position canal moteur, auxiliaire,v1 et v2
        If Strings.Left(SerialMessagRecieved, 8) = "ALLINONE" Then 'return all values
            Dim AllInOne() As String
            AllInOne = SerialMessagRecieved.Replace("ALLINONE", "").Split("|")
            pulseValue = CInt(AllInOne(0))
            pulseValueAux = CInt(AllInOne(1))

            'SevenSegmentArray1.Value = AllInOne(2)
            'SevenSegmentArray2.Value = AllInOne(3)
            ''SevenSegmentArray1.Value = Convert.ToString(GetRandom(0, 20000))
            ''SevenSegmentArray2.Value = Convert.ToString(GetRandom(0, 20000))

            'AquaGaugeMoteur1.Value = Convert.ToInt32(SevenSegmentArray1.Value)
            'AquaGaugeMoteur2.Value = Convert.ToInt32(SevenSegmentArray2.Value)
            'AquaGaugeMoteur1.ValueToDigital = Convert.ToInt32(SevenSegmentArray1.Value)
            'AquaGaugeMoteur2.ValueToDigital = Convert.ToInt32(SevenSegmentArray2.Value)

            ''If GrahIsEnable = True Then 'lecture DataLogger
            '' Make up some data points based on the Sine function
            'Dim i As Integer, x As Double ', y As Double, y2 As Double
            'If i < 10000 Then
            '    x = New XDate(DateTime.Now) '(i + 11)
            '    list.Add(x, Convert.ToInt32(SevenSegmentArray1.Value))
            '    list2.Add(x, Convert.ToInt32(SevenSegmentArray2.Value))
            '    i = i + 1

            'End If
            'GraphIsReady = True

        End If

        'lecture position canal moteur
        If Strings.Left(SerialMessagRecieved, 1) = "M" Then 'return throttle stick position
            pulseValue = CInt(SerialMessagRecieved.Replace("M", ""))
            'If term.Visible = True Then
            '    term.TextBoxTerminalComPort.AppendText(pulseValue & vbCrLf)
            'End If
            If CheckBoxReadTracBarMotorOrMotorThrottle.Checked = True Then
                If RecorderIsOn = True Then

                    Dim t As New Threading.Thread(Sub() AddRecord(svfd.FileName, Str(pulseValue)))
                    t.Start()
                End If
            Else
                Select Case ModeType
                    Case 1 : textCentreServo1.Text = SerialMessagRecieved.Replace("M", "")
                    Case 2 : textCentreServo2.Text = SerialMessagRecieved.Replace("M", "")
                    Case 31 : textIdleServo1.Text = SerialMessagRecieved.Replace("M", "")
                    Case 32 : textIdleServo2.Text = SerialMessagRecieved.Replace("M", "")
                    Case 4 : textTempsReponse.Text = SerialMessagRecieved.Replace("M", "")
                    Case 51 : textMaxiMoteurs.Text = SerialMessagRecieved.Replace("M", "")
                    Case 52 : textDebutSynchro.Text = SerialMessagRecieved.Replace("M", "")
                    Case 53

                        textGeneralMinMaxStopWatch.Text = CStr(30 - watch.Elapsed.Seconds)
                        textGeneralMinMaxStopWatch.ForeColor = Color.Red

                        pulseMinValue = CInt(textMiniGenerale.Text)
                        pulseMaxValue = CInt(textMaxiGenerale.Text)

                        If watch.IsRunning And watch.Elapsed.TotalSeconds < 30 Then
                            If pulseValue < pulseMinValue Then
                                pulseMinValue = pulseValue
                                textMiniGenerale.Text = Trim(CStr(pulseMinValue))
                            End If
                            If pulseValue > pulseMaxValue Then
                                pulseMaxValue = pulseValue
                                textMaxiGenerale.Text = Trim(CStr(pulseMaxValue))
                            End If
                        Else
                            watch.Stop()
                            TimerRXMotors.Enabled = False
                            textGeneralMinMaxStopWatch.ForeColor = Color.Green
                            textGeneralMinMaxStopWatch.Text = "Min Max OK"
                            If My.Settings.Language = "French" Then
                                ButtonMiniMaxGeneral.Text = My.Resources.buttonRead_FR
                            ElseIf My.Settings.Language = "English" Then
                                ButtonMiniMaxGeneral.Text = My.Resources.buttonRead_EN
                            End If
                            EnableDisableButtonsRead(True)
                        End If
                End Select
            End If

        End If

        'lecture des capteurs de vitesse 1 et 2
        If Strings.Left(SerialMessagRecieved, 1) = "V" Then 'return motor's speeds
            Try
                SpeedArray = SerialMessagRecieved.Replace("V", "").Split("|")

                SevenSegmentArray1.Value = SpeedArray(0)
                SevenSegmentArray2.Value = SpeedArray(1)
                'SevenSegmentArray1.Value = Convert.ToString(GetRandom(0, 20000))
                'SevenSegmentArray2.Value = Convert.ToString(GetRandom(0, 20000))

                AquaGaugeMoteur1.Value = Convert.ToInt32(SevenSegmentArray1.Value)
                AquaGaugeMoteur2.Value = Convert.ToInt32(SevenSegmentArray2.Value)
                AquaGaugeMoteur1.ValueToDigital = Convert.ToInt32(SevenSegmentArray1.Value)
                AquaGaugeMoteur2.ValueToDigital = Convert.ToInt32(SevenSegmentArray2.Value)

                'If GrahIsEnable = True Then 'lecture DataLogger
                ' Make up some data points based on the Sine function
                Dim i As Integer, x As Double ', y As Double, y2 As Double
                If i < 10000 Then
                    x = New XDate(DateTime.Now) '(i + 11)
                    list.Add(x, Convert.ToInt32(SevenSegmentArray1.Value))
                    list2.Add(x, Convert.ToInt32(SevenSegmentArray2.Value))
                    i = i + 1

                End If
                GraphIsReady = True

            Catch ex As Exception
                ShowMsg(SerialMessagRecieved & vbCrLf & ex.Message, ShowMsgImage.Critical, "Erreur")
            End Try
        End If

        'lecture position canal auxiliaire
        If Strings.Left(SerialMessagRecieved, 1) = "A" Then 'return auxiliary stick position
            pulseValueAux = CInt(SerialMessagRecieved.Replace("A", ""))
            TextBoxAuxiliairePulse.Text = pulseValueAux.ToString
            'If term.Visible = True Then
            '    term.TextBoxTerminalComPort.AppendText(pulseValueAux & vbCrLf)
            'End If
        End If

        'lecture voltage arduino, temp arduino et voltage batterie RX
        If Strings.Left(SerialMessagRecieved, 3) = "STS" Then 'return external power voltage and internal temp
            HardwareArray = SerialMessagRecieved.Replace("STS", "").Split("|")
            TextVoltageInterne.Text = HardwareArray(0) & "v"
            TextTempInterne.Text = HardwareArray(1) & IIf(CheckBoxFahrenheitDegrees.Checked, "°F", "°C")
            TextVoltageExterne.Text = HardwareArray(2) & "v"
        End If

    End Sub

#End Region

#Region "Main Form"
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If My.Settings.Language = "French" Then
            Me.Text = "SynchTwinRcEngine Programmateur v" & InterfaceVersion & "                 Bienvenue   :-)"
        ElseIf My.Settings.Language = "English" Then
            Me.Text = "SynchTwinRcEngine Programer  v" & InterfaceVersion & "                 Welcome   :-)"
        End If
        Me.KeyPreview = True
        TabControl1.TabPages.Remove(TabPage8) 'TabControl1.TabPages.Insert(7, TabPage8)

        getRMValue() 'check language on exit

        Try
            Me.Location = New Point(My.Settings.LocationX, My.Settings.LocationY)

            LabelDEBUG.Enabled = True

            AquaGaugeMoteur1.Value = 0
            AquaGaugeMoteur2.Value = 0
            AquaGaugeMoteur1.Glossiness = 80
            AquaGaugeMoteur2.Glossiness = 80
            AquaGaugeMoteur1.ValueToDigital = True
            AquaGaugeMoteur2.ValueToDigital = True
            AquaGaugeMoteur1.DialText = "tr/mn"
            AquaGaugeMoteur2.DialText = "tr/mn"
            AquaGaugeMoteur1.DigitalValue = 0
            AquaGaugeMoteur2.DigitalValue = 0
            AquaGaugeMoteur1.DigitalValueDecimalPlaces = 0
            AquaGaugeMoteur2.DigitalValueDecimalPlaces = 0
            AquaGaugeMoteur1.PointerColor = Color.Black
            AquaGaugeMoteur2.PointerColor = Color.Black
            AquaGaugeMoteur1.DigitalValueColor = Color.LightCyan
            AquaGaugeMoteur2.DigitalValueColor = Color.LightCyan
            AquaGaugeMoteur1.MaxValue = Convert.ToInt16(textMaxiMotorRPM.Text) '30000
            AquaGaugeMoteur1.Threshold1Stop = Convert.ToInt16(textMiniMotorRPM.Text) '2000 'end idle zone
            AquaGaugeMoteur1.Threshold2Stop = Convert.ToInt16(textMaxiMotorRPM.Text) '30000 'end red zone
            AquaGaugeMoteur1.Threshold2Start = AquaGaugeMoteur1.Threshold2Stop - 4000 'start red zone
            AquaGaugeMoteur2.MaxValue = Convert.ToInt16(textMaxiMotorRPM.Text) '30000
            AquaGaugeMoteur2.Threshold1Stop = Convert.ToInt16(textMiniMotorRPM.Text) '2000 'end idle zone
            AquaGaugeMoteur2.Threshold2Stop = Convert.ToInt16(textMaxiMotorRPM.Text) '30000
            AquaGaugeMoteur2.Threshold2Start = AquaGaugeMoteur2.Threshold2Stop - 4000

            AquaGaugeMoteur1.Visible = False
            AquaGaugeMoteur2.Visible = False
            LabelVitesse1.Visible = False
            LabelVitesse2.Visible = False
            labelMotor1.Location = New Point(115, 11)
            labelMotor2.Location = New Point(406, 11)
            CheckBoxAnaDigi.Checked = True

            RichTextBoxSettingsHelp.Visible = False

            SevenSegmentArray1.ArrayCount = 5
            SevenSegmentArray1.Value = 0
            SevenSegmentArray1.DecimalShow = False
            SevenSegmentArray2.ArrayCount = 5
            SevenSegmentArray2.Value = 0
            SevenSegmentArray2.DecimalShow = False

            LabelModifications.Enabled = False
            LabelModifications.Text = "- - -"

            ProgressBarSaveSettings.Visible = False

            CheckBoxReadTracBarMotorOrMotorThrottle.Checked = False
            If My.Settings.Language = "French" Then
                CheckBoxReadTracBarMotorOrMotorThrottle.Text = "Lit '" & My.Resources.labelMotorsSimu_FR & "'"
            Else
                CheckBoxReadTracBarMotorOrMotorThrottle.Text = "Read '" & My.Resources.labelMotorsSimu_EN & "'"
            End If
            LabelOnSimuMoveThrottle.Visible = False
            LabelOnSimuMoveThrottle.Location = New Point(65, 45)

            '621; 428
            'Me.Location = New Point(100, 100)
            Me.Width = 620 'ou 989
            Me.Height = 430

            ComboBaudRate.Items.AddRange(BaudRates)
            'ComboBaudRate.SelectedIndex = 4
            ComboBaudRate.SelectedIndex = My.Settings.SpeedIndex
            Try
                'For Each sport As String In My.Computer.Ports.SerialPortNames
                '    ComboPort.Items.Add(sport)
                'Next
                For Each sport As String In SerialPort.GetPortNames
                    ComboPort.Items.Add(sport)
                Next
            Catch
                ShowMsg("Aucun port trouvé", ShowMsgImage.Info, "Error")
            End Try

            Try
                ComboPort.SelectedItem = My.Settings.COMPort

                SerialPort1.PortName = ComboPort.SelectedItem.ToString
                SerialPort1.BaudRate = ComboBaudRate.SelectedItem.ToString
                SerialPort1.DataBits = 8
                SerialPort1.Parity = Parity.None
                SerialPort1.StopBits = StopBits.One
                SerialPort1.Handshake = Handshake.None
                SerialPort1.Encoding = System.Text.Encoding.Default 'very important!

                ' Set the read/write timeouts
                SerialPort1.ReadTimeout = 500
                SerialPort1.WriteTimeout = 500
            Catch ex As Exception
                ComboPort.SelectedIndex = 0
                labelInfoNeedSaveCOM.Visible = True
                If My.Settings.Language = "French" Then
                    labelInfoNeedSaveCOM.Text = "Le module n'est pas connecté au port " & My.Settings.COMPort.ToString
                Else
                    labelInfoNeedSaveCOM.Text = "The module isn't connected to the port " & My.Settings.COMPort.ToString
                End If

                'ShowMsg("Le module n'est pas connecté au port " & My.Settings.COMPort.ToString, ShowMsgImage.Info, "Error")
            End Try

            ReadSettings = ""

            PictureBox1.Image = My.Resources.Mosquito
            PictureBoxSimuThrottle.Image = My.Resources.TrackBar
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
            PictureBoxConnectedOK.Image = My.Resources.rectangle_rouge

            PictureBoxAuxMini.Image = My.Resources.rectangle_rouge
            PictureBoxAuxMiddle.Image = My.Resources.rectangle_rouge
            PictureBoxAuxMaxi.Image = My.Resources.rectangle_rouge

            PictureBoxSimuOnOff.Image = My.Resources.rectangle_rouge

            PictureBoxGlowPlugOnOff.Image = My.Resources.rectangle_rouge
            GlowPlugIsOn = False

            PictureBoxRecorder.Image = My.Resources.rectangle_rouge
            RecorderIsOn = False
            PictureBoxPlayer.Image = My.Resources.rectangle_rouge
            PlayerIsOn = False

            PictureBoxReadHardwareOnOff.Image = My.Resources.rectangle_rouge

            TrackBarRudder.Visible = False
            TrackBarRudder.Location = New Point(58, 107)

            SetLanguages()

            labelInfoNeedSaveCOM.Text = ""
            labelInfoNeedSaveCOM.Visible = False


            'labelAuxiPosition.Enabled = False
            'TextBoxAuxiliairePulse.Enabled = False
            'ButtonReadAuxiliairePulse.Enabled = False
            'PictureBoxTimer2OnOff.Enabled = False
            'labelReverseAuxi.Enabled = False
            'CheckBoxInversionAux.Enabled = False


            CheckBoxChronoOnOff.Checked = True
            If My.Settings.Language = "French" Then
                CheckBoxChronoOnOff.Text = "Oui"
            ElseIf My.Settings.Language = "English" Then
                CheckBoxChronoOnOff.Text = "Yes"
            End If

        Catch ex As Exception
            ShowMsg(ex.Message, ShowMsgImage.Info, "Error")
        End Try
    End Sub

    Private Sub SetLanguages()
        ' Create the ToolTip and associate with the Form container.
        Dim toolTip1 As New ToolTip()
        ' Set up the delays for the ToolTip.
        toolTip1.AutoPopDelay = 5000
        toolTip1.InitialDelay = 500
        toolTip1.ReshowDelay = 500
        toolTip1.IsBalloon = True
        ' Force the ToolTip text to be displayed whether or not the form is active.
        toolTip1.ShowAlways = True
        toolTip1.ForeColor = Color.Red

        If My.Settings.Language = "French" Then
            toolTip1.SetToolTip(ButtonSauvegardeCOM, "Sauvegarde Port COM")
            toolTip1.SetToolTip(ButtonMiniMaxGeneral, "Lecture Min Max de votre radio (curseur Y inclus)!")
            If SimualtionSpeed = True Then
                toolTip1.SetToolTip(TrackBarMotors, "Simulation Manche moteurs")
                toolTip1.SetToolTip(ButtonServoTest, "Test mouvement servos")
            Else
                toolTip1.SetToolTip(TrackBarMotors, "Le bouton '" & ButtonSpeedSimuOn.Text & "' doit être utiliser" & vbCrLf &
                                    "pour voir fonctionner les servos !")
                toolTip1.SetToolTip(ButtonServoTest, "Le bouton '" & ButtonSpeedSimuOn.Text & "' doit être utiliser" & vbCrLf &
                                  "pour voir fonctionner les servos !")
            End If
            toolTip1.SetToolTip(ButtonSpeedSimuOn, "Activer mouvement servos")
            toolTip1.SetToolTip(CheckBoxChronoOnOff, "Utiliser Chronomètre")
            toolTip1.SetToolTip(ButtonPinOutHelp, "Affectation pins programmateur USBAsp")
            toolTip1.SetToolTip(TrackBarRudder, "Simulation Manche auxiliaire connecté  à la direction")
            toolTip1.SetToolTip(ButtonAuxMini, "Simulation Manche auxiliaire (position mini)")
            toolTip1.SetToolTip(ButtonAuxMiddle, "Simulation Manche auxiliaire (position milieu)")
            toolTip1.SetToolTip(ButtonAuxMaxi, "Simulation Manche auxiliaire (position maxi)")
            toolTip1.SetToolTip(ButtonReadTempVoltage, "Lecture Température,Voltage Externe et Interne")
            toolTip1.SetToolTip(lblinstallhw, "Click here for installing programmer drive if not installed.")
        ElseIf My.Settings.Language = "English" Then
            toolTip1.SetToolTip(ButtonSauvegardeCOM, "COM Port Save")
            toolTip1.SetToolTip(ButtonMiniMaxGeneral, "Read Min Max from your TX (cursor Y include)!")
            If SimualtionSpeed = True Then
                toolTip1.SetToolTip(TrackBarMotors, "Simulation Motors Throttle")
                toolTip1.SetToolTip(ButtonServoTest, "Test movement servos")
            Else
                toolTip1.SetToolTip(TrackBarMotors, "Button '" & ButtonSpeedSimuOn.Text & "' must be used" & vbCrLf &
                                    "for see the servos in moving !")
                toolTip1.SetToolTip(ButtonServoTest, "Button '" & ButtonSpeedSimuOn.Text & "' must be used" & vbCrLf &
                                    "for see the servos in moving !")
            End If
            toolTip1.SetToolTip(ButtonSpeedSimuOn, "Activate movement's servos")
            toolTip1.SetToolTip(CheckBoxChronoOnOff, "Use Chronometer")
            toolTip1.SetToolTip(ButtonPinOutHelp, "Pin out USBAsp programmer")
            toolTip1.SetToolTip(TrackBarRudder, "Simulation Auxiliary Throttle connected to rudder")
            toolTip1.SetToolTip(ButtonAuxMini, "Simulation Auxiliary Throttle (mini position)")
            toolTip1.SetToolTip(ButtonAuxMiddle, "Simulation Auxiliary Throttle (middle position)")
            toolTip1.SetToolTip(ButtonAuxMaxi, "Simulation Auxiliary Throttle (maxi position)")

            toolTip1.SetToolTip(ButtonReadTempVoltage, "Read Temperature,External Voltage and Interneal")
            toolTip1.SetToolTip(lblinstallhw, "Click here for installing programmer drive if not installed.")
        End If
    End Sub

    Private Sub Form1_Closing(ByVal sender As Object, ByVal e As CancelEventArgs) Handles Me.Closing
        Try

            TimerSpeeds.Enabled = False
            TimerDataLogger.Enabled = False
            TimerHardwareInfos.Enabled = False
            TimerRXAuxiliaire.Enabled = False
            TimerRXMotors.Enabled = False

            If SerialPort1.IsOpen() Then
                SerialPort1.Close()
            End If

            My.Settings.LocationX = Me.Location.X
            My.Settings.LocationY = Me.Location.Y
            My.Settings.Save()


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

#End Region

#Region "Settings"

    Private Sub textCentreServo1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textCentreServo1.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !" Else LabelModifications.Text = "Changes not saved !"

    End Sub
    Private Sub textCentreServo2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textCentreServo2.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    Private Sub textIdleServo1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textIdleServo1.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    Private Sub textIdleServo2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textIdleServo2.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    Private Sub textTempsReponse_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textTempsReponse.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    Private Sub textMaxiMoteurs_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textMaxiMoteurs.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    Private Sub textDebutSynchro_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textDebutSynchro.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    Private Sub textMiniGenerale_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textMiniGenerale.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    Private Sub textMaxiGenerale_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textMaxiGenerale.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    Private Sub textAuxiliaireMode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textAuxiliaireMode.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    'Private Sub textTelemetrieType_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    LabelModifications.Enabled = True
    '    LabelModifications.ForeColor = Color.Red
    '    If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    'End Sub
    Private Sub textAddresseI2C_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub
    Private Sub textNombrePales_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles textNombrePales.TextChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub

    Private Sub CheckBoxInversionServo1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxInversionServo1.CheckStateChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
        If My.Settings.Language = "French" Then
            If (CheckBoxInversionServo1.Checked = True) Then CheckBoxInversionServo1.Text = "Oui" Else CheckBoxInversionServo1.Text = "Non"
        ElseIf My.Settings.Language = "English" Then
            If (CheckBoxInversionServo1.Checked = True) Then CheckBoxInversionServo1.Text = "Yes" Else CheckBoxInversionServo1.Text = "No"
        End If
    End Sub
    Private Sub CheckBoxInversionServo2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxInversionServo2.CheckStateChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
        If My.Settings.Language = "French" Then
            If (CheckBoxInversionServo2.Checked = True) Then CheckBoxInversionServo2.Text = "Oui" Else CheckBoxInversionServo2.Text = "Non"
        ElseIf My.Settings.Language = "English" Then
            If (CheckBoxInversionServo2.Checked = True) Then CheckBoxInversionServo2.Text = "Yes" Else CheckBoxInversionServo2.Text = "No"
        End If
    End Sub
    Private Sub CheckBoxFahrenheitDegrees_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxFahrenheitDegrees.CheckStateChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
        If CheckBoxFahrenheitDegrees.Checked = False Then CheckBoxFahrenheitDegrees.Text = "°C" Else CheckBoxFahrenheitDegrees.Text = "°F"
    End Sub
    Private Sub CheckBoxInversionAux_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxInversionAux.CheckStateChanged
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"

        If My.Settings.Language = "French" Then
            If (CheckBoxInversionAux.Checked = True) Then CheckBoxInversionAux.Text = "Oui" Else CheckBoxInversionAux.Text = "Non"
        ElseIf My.Settings.Language = "English" Then
            If (CheckBoxInversionAux.Checked = True) Then CheckBoxInversionAux.Text = "Yes" Else CheckBoxInversionAux.Text = "No"
        End If
    End Sub

    Private Sub ButtonMoinsVitesseReponse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMoinsVitesseReponse.Click
        If Convert.ToInt16(textTempsReponse.Text) > 0 Then textTempsReponse.Text = Convert.ToString(Convert.ToInt16(textTempsReponse.Text) - 1)
    End Sub
    Private Sub ButtonPlusVitesseReponse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlusVitesseReponse.Click
        If Convert.ToInt16(textTempsReponse.Text) < 4 Then textTempsReponse.Text = Convert.ToString(Convert.ToInt16(textTempsReponse.Text) + 1)
    End Sub

    Private Sub ButtonMoinsModeAuxiliaire_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMoinsModeAuxiliaire.Click
        If Convert.ToInt16(textAuxiliaireMode.Text) > 1 Then textAuxiliaireMode.Text = Convert.ToString(Convert.ToInt16(textAuxiliaireMode.Text) - 1)
        LabelInterType.Text = ModeAuxiliaireTypeText(Convert.ToInt16(textAuxiliaireMode.Text))
    End Sub
    Private Sub ButtonPlusModeAuxiliaire_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlusModeAuxiliaire.Click
        If Convert.ToInt16(textAuxiliaireMode.Text) < 6 Then textAuxiliaireMode.Text = Convert.ToString(Convert.ToInt16(textAuxiliaireMode.Text) + 1)
        LabelInterType.Text = ModeAuxiliaireTypeText(Convert.ToInt16(textAuxiliaireMode.Text))
    End Sub

    'Private Sub ButtonMoinsTelemetrie_Click(sender As System.Object, e As System.EventArgs)
    '    'If Convert.ToInt16(textTelemetrieType.Text) > 0 Then textTelemetrieType.Text = Convert.ToString(Convert.ToInt16(textTelemetrieType.Text) - 1)
    '    ShowMsg("No Telemetry Feature Available !!!", ShowMsgImage.Info, "Error")
    'End Sub
    'Private Sub ButtonPlusTelemetrie_Click(sender As System.Object, e As System.EventArgs)
    '    'If Convert.ToInt16(textTelemetrieType.Text) < 6 Then textTelemetrieType.Text = Convert.ToString(Convert.ToInt16(textTelemetrieType.Text) + 1)
    '    ShowMsg("No Telemetry Feature Available !!!", ShowMsgImage.Info, "Error")
    'End Sub

    Private Sub ButtonMoinsNombrePales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMoinsNombrePales.Click
        If Convert.ToInt16(textNombrePales.Text) > 1 Then textNombrePales.Text = Convert.ToString(Convert.ToInt16(textNombrePales.Text) - 1)
    End Sub
    Private Sub ButtonPlusNombrePales_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlusNombrePales.Click
        If Convert.ToInt16(textNombrePales.Text) < 5 Then textNombrePales.Text = Convert.ToString(Convert.ToInt16(textNombrePales.Text) + 1)
    End Sub

    Private Sub SettingsHelpText()
        If My.Settings.Language = "French" Then
            RichTextBoxSettingsHelp.Text = "A gauche, clickez boutons 'Lire' (un click = lecture, un autre = stop):" & vbCrLf & _
                vbTab & "-Position Generale Mini/Maxi" & vbCrLf & _
                vbTab & " Bouger le manche moteurs plusieurs fois du minimum au maximum" & vbCrLf & _
                vbTab & " (trim inclus)." & vbCrLf & _
                vbTab & " Placer aussi le trim moteurs au mini pour lire minimum," & vbCrLf & _
                vbTab & " puis au maxi pour lire maximum." & vbCrLf & _
                vbTab & "-Centre Servo 1 (trim moteurs au centre)" & vbCrLf & _
                vbTab & " Placez le manche des moteurs au centre." & vbCrLf & _
                vbTab & "-Centre Servo 2 (trim moteurs au centre)" & vbCrLf & _
                vbTab & " Placez le manche des moteurs au centre." & vbCrLf & _
                vbTab & "-Position Sécurité Servo 1 (trim moteurs au centre)" & vbCrLf & _
                vbTab & " Placez le manche moteurs en position sécurité." & vbCrLf & _
                vbTab & "-Position Sécurité Servo 2 (trim moteurs au centre)" & vbCrLf & _
                vbTab & " Placez le manche moteurs en position sécurité." & vbCrLf & _
                vbTab & "-Vitesse de réponse du module" & vbCrLf & _
                vbTab & " Utilisez + or - buttons (défaut = 2)." & vbCrLf & _
                vbTab & "-Position Maxi Servos (pour vitesse moteur maxi, trim Y est au centre)" & vbCrLf & _
                vbTab & " Placez le manche moteurs en position maximale." & vbCrLf & _
                vbTab & "-Démarrage Synchro Servos (trim Y est au centre)" & vbCrLf & _
                vbTab & " La synchro moteur démarre au delà de cette position." & vbCrLf & _
                vbTab & "-Connexion canal Auxiliaire (utiliser '?' pour plus d'explication sur les" & vbCrLf & _
                vbTab & " différents modes)." & vbCrLf & _
                vbTab & " Utiliser + ou - boutons (défaut = 1)." & vbCrLf & vbCrLf & _
                "A droite, définir les autres réglages:" & vbCrLf & _
                vbTab & " Inversion servo moteur 1." & vbCrLf & _
                vbTab & " Inversion servo moteur 2." & vbCrLf & _
                vbTab & " LCD display adresse (non utilisé actuellement)." & vbCrLf & _
                vbTab & " Nombre of pâles ou d'aimants." & vbCrLf & _
                vbTab & " Température en Celcius ou Kelvin." & vbCrLf & _
                vbTab & " Vitesse maxi des moteurs (en tr/mn)." & vbCrLf & _
                vbTab & " Différence entre vitesses moteurs autorisée (0 à 100)" & vbCrLf & _
                vbTab & " Définir le module en 'maître' ou 'esclave' (option non prête)." & vbCrLf & vbCrLf & _
                "Quand la configuration est terminée, utiliser le bouton 'Sauver Configuration' et redémarrer le module !"

            MakeBold(RichTextBoxSettingsHelp, "A gauche, clickez boutons 'Lire' (un click = lecture, un autre = stop):")
            MakeBold(RichTextBoxSettingsHelp, "A droite, définir les autres réglages:")

        ElseIf My.Settings.Language = "English" Then
            RichTextBoxSettingsHelp.Text = "On left, click 'Read' buttons (one click run read, another save value):" & vbCrLf & _
                vbTab & "-Position Generale Mini/Maxi" & vbCrLf & _
                vbTab & " Move your throttle stick from minimum to maximum several times." & vbCrLf & _
                vbTab & " Set throttle cursor positions to the maximum/minimum" & vbCrLf & _
                vbTab & " (trimmers included)." & vbCrLf & _
                vbTab & "-Center Servo 1 (trim Y is on center)" & vbCrLf & _
                vbTab & " Move your throttle stick to the center." & vbCrLf & _
                vbTab & "-Center Servo 2 (trim Y is on center)" & vbCrLf & _
                vbTab & " Move your throttle stick to the center." & vbCrLf & _
                vbTab & "-Position Idle Servo 1 (trim Y is on center)" & vbCrLf & _
                vbTab & " Move your throttle stick to the idle position you want." & vbCrLf & _
                vbTab & "-Position Idle Servo 2 (trim Y is on center)" & vbCrLf & _
                vbTab & " Move your throttle stick to the idle position you want." & vbCrLf & _
                vbTab & "-Speed Module Answer" & vbCrLf & _
                vbTab & " Use + or - buttons (default is 2)." & vbCrLf & _
                vbTab & "-Position Maxi Servos (for maximum speed, trim Y is on center)" & vbCrLf & _
                vbTab & " Move your throttle stick to the maximum position you want." & vbCrLf & _
                vbTab & "-Start Synchro Servos (trim Y is on center)" & vbCrLf & _
                vbTab & " The synchro start when the throttle will be above this position." & vbCrLf & _
                vbTab & "-Connexion channel Auxiliary (use '?' button for each mode explain)." & vbCrLf & _
                vbTab & " Use + or - buttons (default is 1)." & vbCrLf & vbCrLf & _
                "On right, define other settings:" & vbCrLf & _
                vbTab & " Reverse servo moteur 1." & vbCrLf & _
                vbTab & " Reverse servo moteur 2." & vbCrLf & _
                vbTab & " LCD display address (not used actually)." & vbCrLf & _
                vbTab & " Number of blades or magnets." & vbCrLf & _
                vbTab & " Temperature in Celcius or Kelvin." & vbCrLf & _
                vbTab & " Your motor speed mini and maxi (in RPM)." & vbCrLf & _
                vbTab & " Difference between the two speeds (0 to 100)" & vbCrLf & _
                vbTab & " Define the module as 'master' or 'slave' (feature not ready)." & vbCrLf & vbCrLf & _
                "When all settings are done, use the 'Save Settings' button and restart the module !"

            MakeBold(RichTextBoxSettingsHelp, "On left, click 'Read' buttons (one click run read, another save value):")
            MakeBold(RichTextBoxSettingsHelp, "On right, define other settings:")

        End If
    End Sub

    Private Sub ButtonReadCenter1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReadCenter1.Click
        ModeType = 1
        If TimerRXAuxiliaire.Enabled = True Then
            TimerRXAuxiliaire.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
        End If
        If TimerSpeeds.Enabled = True Then
            TimerSpeeds.Enabled = False
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
        End If

        If TimerRXMotors.Enabled = True Then
            TimerRXMotors.Enabled = False
            If My.Settings.Language = "French" Then
                ButtonReadCenter1.Text = My.Resources.buttonRead_FR
            ElseIf My.Settings.Language = "English" Then
                ButtonReadCenter1.Text = My.Resources.buttonRead_EN
            End If
            BackgroundWorkerThrottle.CancelAsync()
        Else
            BackgroundWorkerThrottle.WorkerSupportsCancellation = True
            BackgroundWorkerThrottle.RunWorkerAsync()
            TimerRXMotors.Enabled = True
            ButtonReadCenter1.Text = "Stop"
        End If
    End Sub
    Private Sub ButtonReadCenter2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReadCenter2.Click
        ModeType = 2
        If TimerRXAuxiliaire.Enabled = True Then
            TimerRXAuxiliaire.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
        End If
        If TimerSpeeds.Enabled = True Then
            TimerSpeeds.Enabled = False
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
        End If

        If TimerRXMotors.Enabled = True Then
            TimerRXMotors.Enabled = False
            If My.Settings.Language = "French" Then
                ButtonReadCenter2.Text = My.Resources.buttonRead_FR
            ElseIf My.Settings.Language = "English" Then
                ButtonReadCenter2.Text = My.Resources.buttonRead_EN
            End If
            BackgroundWorkerThrottle.CancelAsync()
        Else
            BackgroundWorkerThrottle.WorkerSupportsCancellation = True
            BackgroundWorkerThrottle.RunWorkerAsync()
            TimerRXMotors.Enabled = True
            ButtonReadCenter2.Text = "Stop"
        End If
    End Sub


    Private Sub ButtonIdelMoteur1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonIdleMoteur1.Click
        ModeType = 31
        If TimerRXAuxiliaire.Enabled = True Then
            TimerRXAuxiliaire.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
        End If
        If TimerSpeeds.Enabled = True Then
            TimerSpeeds.Enabled = False
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
        End If

        If TimerRXMotors.Enabled = True Then
            TimerRXMotors.Enabled = False
            If My.Settings.Language = "French" Then
                ButtonIdleMoteur1.Text = My.Resources.buttonRead_FR
            ElseIf My.Settings.Language = "English" Then
                ButtonIdleMoteur1.Text = My.Resources.buttonRead_EN
            End If
            BackgroundWorkerThrottle.CancelAsync()
        Else
            BackgroundWorkerThrottle.WorkerSupportsCancellation = True
            BackgroundWorkerThrottle.RunWorkerAsync()
            TimerRXMotors.Enabled = True
            ButtonIdleMoteur1.Text = "Stop"
        End If
    End Sub
    Private Sub ButtonIdleMoteur2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonIdleMoteur2.Click
        ModeType = 32
        If TimerRXAuxiliaire.Enabled = True Then
            TimerRXAuxiliaire.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
        End If
        If TimerSpeeds.Enabled = True Then
            TimerSpeeds.Enabled = False
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
        End If

        If TimerRXMotors.Enabled = True Then
            TimerRXMotors.Enabled = False
            If My.Settings.Language = "French" Then
                ButtonIdleMoteur2.Text = My.Resources.buttonRead_FR
            ElseIf My.Settings.Language = "English" Then
                ButtonIdleMoteur2.Text = My.Resources.buttonRead_EN
            End If
            BackgroundWorkerThrottle.CancelAsync()
        Else
            BackgroundWorkerThrottle.WorkerSupportsCancellation = True
            BackgroundWorkerThrottle.RunWorkerAsync()
            TimerRXMotors.Enabled = True
            ButtonIdleMoteur2.Text = "Stop"
        End If
    End Sub

    Private Sub ButtonMaxiMoteurs_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMaxiMoteurs.Click
        ModeType = 51
        If TimerRXAuxiliaire.Enabled = True Then
            TimerRXAuxiliaire.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
        End If
        If TimerSpeeds.Enabled = True Then
            TimerSpeeds.Enabled = False
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
        End If

        If TimerRXMotors.Enabled = True Then
            TimerRXMotors.Enabled = False
            If My.Settings.Language = "French" Then
                ButtonMaxiMoteurs.Text = My.Resources.buttonRead_FR
            ElseIf My.Settings.Language = "English" Then
                ButtonMaxiMoteurs.Text = My.Resources.buttonRead_EN
            End If
            BackgroundWorkerThrottle.CancelAsync()
        Else
            BackgroundWorkerThrottle.WorkerSupportsCancellation = True
            BackgroundWorkerThrottle.RunWorkerAsync()
            TimerRXMotors.Enabled = True
            ButtonMaxiMoteurs.Text = "Stop"
        End If
    End Sub
    Private Sub ButtonDebutSynchro_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDebutSynchro.Click
        ModeType = 52
        If TimerRXAuxiliaire.Enabled = True Then
            TimerRXAuxiliaire.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
        End If
        If TimerSpeeds.Enabled = True Then
            TimerSpeeds.Enabled = False
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
        End If

        If TimerRXMotors.Enabled = True Then
            TimerRXMotors.Enabled = False
            If My.Settings.Language = "French" Then
                ButtonDebutSynchro.Text = My.Resources.buttonRead_FR
            ElseIf My.Settings.Language = "English" Then
                ButtonDebutSynchro.Text = My.Resources.buttonRead_EN
            End If
            BackgroundWorkerThrottle.CancelAsync()
        Else
            BackgroundWorkerThrottle.WorkerSupportsCancellation = True
            BackgroundWorkerThrottle.RunWorkerAsync()
            TimerRXMotors.Enabled = True
            ButtonDebutSynchro.Text = "Stop"
        End If
    End Sub

    Private Sub ButtonMiniMaxGeneral_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonMiniMaxGeneral.Click
        ModeType = 53
        If TimerRXAuxiliaire.Enabled = True Then
            TimerRXAuxiliaire.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
        End If
        If TimerSpeeds.Enabled = True Then
            TimerSpeeds.Enabled = False
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
        End If

        If TimerRXMotors.Enabled = True Then
            If My.Settings.Language = "French" Then
                ButtonMiniMaxGeneral.Text = My.Resources.buttonRead_FR
            ElseIf My.Settings.Language = "English" Then
                ButtonMiniMaxGeneral.Text = My.Resources.buttonRead_EN
            End If
            TimerRXMotors.Enabled = False
            If watch.IsRunning = True Then watch.Stop()
            textGeneralMinMaxStopWatch.ForeColor = Color.Green
            textGeneralMinMaxStopWatch.Text = "0"
            If textMiniGenerale.Text <> "1000" And textMaxiGenerale.Text <> "2000" Then
                EnableDisableButtonsRead(True)
            Else
                EnableDisableButtonsRead(False)
            End If
            BackgroundWorkerThrottle.CancelAsync()
        Else
            BackgroundWorkerThrottle.WorkerSupportsCancellation = True
            BackgroundWorkerThrottle.RunWorkerAsync()
            textMiniGenerale.Text = "1500"
            textMaxiGenerale.Text = "1500"
            ButtonMiniMaxGeneral.Text = "Stop"
            TimerRXMotors.Enabled = True
            ' Create new Stopwatch instance.
            watch = Stopwatch.StartNew()
        End If


    End Sub

    Private Sub ButtonReadSpeeds_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReadSpeeds.Click
        If TimerRXAuxiliaire.Enabled = True Then
            TimerRXAuxiliaire.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_vert
        End If
        If TimerRXMotors.Enabled = True Then
            TimerRXMotors.Enabled = False
        End If

        If TimerSpeeds.Enabled = True Then
            TimerSpeeds.Enabled = False
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
            AquaGaugeMoteur1.Value = 0
            AquaGaugeMoteur2.Value = 0
            AquaGaugeMoteur1.DigitalValue = 0
            AquaGaugeMoteur2.DigitalValue = 0
            'LabelVitesse1.Text = "Vitesse 1: 0"
            'LabelVitesse2.Text = "Vitesse 2: 0"
        Else
            TimerSpeeds.Enabled = True
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_vert
            Dim v As Integer
            If textMaxiMotorRPM.Text = "" Then SerialPort1.Write(Trim(Str(363)) & vbCr)
            If CheckBoxAnaDigi.Checked = True Then
                For v = 0 To Convert.ToInt16(textMaxiMotorRPM.Text) Step 1000
                    AquaGaugeMoteur1.Value = Convert.ToInt32(v)
                    AquaGaugeMoteur2.Value = Convert.ToInt32(v)
                    AquaGaugeMoteur1.ValueToDigital = Convert.ToInt32(v)
                    AquaGaugeMoteur2.ValueToDigital = Convert.ToInt32(v)
                    Thread.Sleep(5)
                Next
                For v = Convert.ToInt16(textMaxiMotorRPM.Text) To 0 Step -1000
                    AquaGaugeMoteur1.Value = Convert.ToInt32(v)
                    AquaGaugeMoteur2.Value = Convert.ToInt32(v)
                    AquaGaugeMoteur1.ValueToDigital = Convert.ToInt32(v)
                    AquaGaugeMoteur2.ValueToDigital = Convert.ToInt32(v)
                    Thread.Sleep(5)
                Next
            Else
                For v = 0 To Convert.ToInt16(textMaxiMotorRPM.Text) Step 1000
                    SevenSegmentArray1.Value = "-----"
                    SevenSegmentArray2.Value = "-----"
                    Thread.Sleep(5)
                Next
                For v = Convert.ToInt16(textMaxiMotorRPM.Text) To 0 Step -1000
                    SevenSegmentArray1.Value = "-----"
                    SevenSegmentArray2.Value = "-----"
                    Thread.Sleep(5)
                Next
            End If
        End If

    End Sub

    Private Sub ButtonReadAuxiliairePulse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReadAuxiliairePulse.Click
        If TimerRXMotors.Enabled = True Then
            TimerRXMotors.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
        End If
        If TimerSpeeds.Enabled = True Then
            TimerSpeeds.Enabled = False
            PictureBoxTimer1OnOff.Image = My.Resources.rectangle_rouge
        End If

        If TimerRXAuxiliaire.Enabled = True Then
            If My.Settings.Language = "French" Then
                ButtonReadAuxiliairePulse.Text = My.Resources.buttonRead_FR
            ElseIf My.Settings.Language = "English" Then
                ButtonReadAuxiliairePulse.Text = My.Resources.buttonRead_EN
            End If
            TimerRXAuxiliaire.Enabled = False
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_rouge
            BackgroundWorkerAuxiliary.CancelAsync()
        Else
            BackgroundWorkerAuxiliary.WorkerSupportsCancellation = True
            BackgroundWorkerAuxiliary.RunWorkerAsync()
            ButtonReadAuxiliairePulse.Text = "Stop"
            TimerRXAuxiliaire.Enabled = True
            PictureBoxTimer2OnOff.Image = My.Resources.rectangle_vert
        End If

    End Sub

    Private Sub ButtonReadTempVoltage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReadTempVoltage.Click
        If TimerHardwareInfos.Enabled = True Then
            TimerHardwareInfos.Enabled = False
            PictureBoxReadHardwareOnOff.Image = My.Resources.rectangle_rouge
        Else
            TimerHardwareInfos.Enabled = True
            PictureBoxReadHardwareOnOff.Image = My.Resources.rectangle_vert
        End If
    End Sub

    Private Sub ButtonModuleType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonModuleType.Click
        array(19) = IIf(array(19) = "0", "1", "0")
        Dim master As String = ""
        Dim slave As String = ""
        If My.Settings.Language = "French" Then
            master = "Maître"
            slave = "Esclave"
        ElseIf My.Settings.Language = "English" Then
            master = "Master"
            slave = "Slave"
        End If

        If My.Settings.Language = "French" Then
            Me.Text = "SynchTwinRcEngine Programmateur (module " & IIf(array(19) = "0", master & ")", slave & ") v" & InterfaceVersion) 'type de module ajouté au titre
        ElseIf My.Settings.Language = "English" Then
            Me.Text = "SynchTwinRcEngine Programer(module " & IIf(array(19) = "0", master & ")", slave & ") v" & InterfaceVersion) 'type de module ajouté au titre
        End If

        ButtonModuleType.Text = IIf(array(19) = "0", master, slave)
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
    End Sub

    Private Function ModeAuxiliaireTypeText(ByVal mode As Integer) As String
        Dim StringMode As String = ""
        Select Case mode
            Case 1 : If My.Settings.Language = "French" Then StringMode = "===> Auxiliaire non connecté" Else StringMode = "===> Auxiliary not connected"
                : ButtonAuxMini.Enabled = False : ButtonAuxMiddle.Enabled = False : ButtonAuxMaxi.Enabled = False
                : ButtonAuxMini.Visible = True : ButtonAuxMiddle.Visible = True : ButtonAuxMaxi.Visible = True
                : PictureBoxAuxMini.Visible = True : PictureBoxAuxMiddle.Visible = True : PictureBoxAuxMaxi.Visible = True
                : ProgressBarAuxiliary.Visible = False : LabelAux.Enabled = False : TrackBarRudder.Visible = False
                : PictureBoxAuxMini.Image = My.Resources.rectangle_gris : PictureBoxAuxMiddle.Image = My.Resources.rectangle_gris : PictureBoxAuxMaxi.Image = My.Resources.rectangle_gris

            Case 2 : If My.Settings.Language = "French" Then StringMode = "===> Auxiliaire connecté à inter 3 positions" Else StringMode = "===> Auxiliary connected to switch 3 positions"
                : ButtonAuxMini.Enabled = True : ButtonAuxMiddle.Enabled = True : ButtonAuxMaxi.Enabled = True
                : ButtonAuxMini.Visible = True : ButtonAuxMiddle.Visible = True : ButtonAuxMaxi.Visible = True
                : PictureBoxAuxMini.Visible = True : PictureBoxAuxMiddle.Visible = True : PictureBoxAuxMaxi.Visible = True
                : ProgressBarAuxiliary.Visible = True : LabelAux.Enabled = True : TrackBarRudder.Visible = False
                : PictureBoxAuxMini.Image = My.Resources.rectangle_rouge : PictureBoxAuxMiddle.Image = My.Resources.rectangle_vert : PictureBoxAuxMaxi.Image = My.Resources.rectangle_rouge

            Case 3, 5, 6 : If My.Settings.Language = "French" Then StringMode = "===> Auxiliaire connecté à inter 2 positions" Else StringMode = "===> Auxiliary connected to switch 2 positions"
                : ButtonAuxMini.Enabled = True : ButtonAuxMiddle.Enabled = False : ButtonAuxMaxi.Enabled = True
                : ButtonAuxMini.Visible = True : ButtonAuxMiddle.Visible = True : ButtonAuxMaxi.Visible = True
                : PictureBoxAuxMini.Visible = True : PictureBoxAuxMiddle.Visible = True : PictureBoxAuxMaxi.Visible = True
                : ProgressBarAuxiliary.Visible = True : LabelAux.Enabled = True : TrackBarRudder.Visible = False
                : PictureBoxAuxMini.Image = My.Resources.rectangle_vert : PictureBoxAuxMiddle.Image = My.Resources.rectangle_gris : PictureBoxAuxMaxi.Image = My.Resources.rectangle_rouge

            Case 4 : If My.Settings.Language = "French" Then StringMode = "===> Auxiliaire connecté à la direction" Else StringMode = "===> Auxiliary connected to rudder"
                : ButtonAuxMini.Enabled = False : ButtonAuxMiddle.Enabled = False : ButtonAuxMaxi.Enabled = False
                : ButtonAuxMini.Visible = False : ButtonAuxMiddle.Visible = False : ButtonAuxMaxi.Visible = False
                : PictureBoxAuxMini.Visible = False : PictureBoxAuxMiddle.Visible = False : PictureBoxAuxMaxi.Visible = False
                : TrackBarRudder.Visible = True : TrackBarRudder.Value = 90
                : ProgressBarAuxiliary.Visible = True 'LabelAux.Text = 0 ': ProgressBarAuxiliary.Value = 90
                : If My.Settings.Language = "French" Then labelAuxRudderSimulation.Text = "Canal Direction Simulation" Else labelAuxRudderSimulation.Text = "Channel Direction Simulation"

        End Select
        Return StringMode
    End Function

    Private Sub ButtonAuxiliaireHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAuxiliaireHelp.Click
        If My.Settings.Language = "French" Then
            Select Case Convert.ToInt32(textAuxiliaireMode.Text)
                Case 0 : ShowMsg("Selectionnez un mode Auxiliaire (utiliser les boutons + ou - !!!)", ShowMsgImage.Info, "Canal Auxiliaire Erreur !!!")
                Case 1 : ShowMsg("MODE1: Auxiliaire non utilisé" & vbCrLf & _
                                "C'est le mode dans lequel le module devra être is l'entrée AUX n'est pas conectée." & vbCrLf & _
                                "Le module laisse l'entrée gaz controler les servos jusqu'à 1/5eme de sa position." & vbCrLf & _
                                "Au dela des 1/5eme des gaz, le module contrôle la position des servos et synchronise les moteurs." & vbCrLf & _
                                "Si le manche des gaz bouge, le processus est repete.", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 1")
                Case 2 : ShowMsg("MODE2: Fonctionnement indépendent des moteurs" & vbCrLf & _
                                "Dans ce mode, l'entrée AUX est connectée à un switch 3 positions de l'émetteur." & vbCrLf & _
                                "Si la sortie de ce canal est en dessous de 1/3 de sa course, le moteur 1 est contrôllé par" & vbCrLf & _
                                "le manche des gaz pendant que le moteur 2 est maintenu dans la position 'idle' programée." & vbCrLf & _
                                "Si la sortie du canal AUX est entre 1/3 et 2/3 de sa course, le manche des gaz contrôle les" & vbCrLf & _
                                "deux moteurs et le module est comme dans le mode 1.Si la sortie AUX est au dela des 2/3" & vbCrLf & _
                                "de sa course, le manche des gaz contrôle le moteur 2 et le moteur 1 est maintenu dans la" & vbCrLf & _
                                "position 'idle' programmée.Ce mode est idéal pour l'ajustement du mélange de carburant.", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 2")
                Case 3 : ShowMsg("MODE3: Synchronisation On/Off" & vbCrLf & _
                                "Dans ce mode, le canal AUX est ossocié à un switch 2 positions de l'émetteur." & vbCrLf & _
                                "Dans une position,la synchronisation est active.Dans l'autre position, le module" & vbCrLf & _
                                "ne fait rien et se comporte comme un cable 'Y' (bien que les directions et centres" & vbCrLf & _
                                "servos soient toujours controles par le module). Il s'agit d'un mode utile pour" & vbCrLf & _
                                "comprendre comment votre avion va réagir alors que les moteurs sont contrôlés par le module.", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 3")
                Case 4 : ShowMsg("MODE4: AUX CH is for Rudder Steering(!!non utilisable pour l'instant!!)" & vbCrLf & _
                                "Dans ce mode, le module se comporte comme dans le mode1 (canal aux. non connecté) avant les 1/3 moteur." & vbCrLf & _
                                "Au delà des 1/3 moteur, le canal auxiliaire est supposé être relié à la sortie du récepteur de gouvernail" & vbCrLf & _
                                "Il y a une zone morte autour du centre (ainsi, le trim du gouvernail est sans effet sur les moteurs)." & vbCrLf & _
                                "Quand le manche du gouvernail dépasse cette zone morte, il augmente la vitesse d'un moteur." & vbCrLf & _
                                "Bouger le manche du gouvernail dans l'autre sens augmente la vitesse de l'autre moteur." & vbCrLf & _
                                "Le gouvernail en position maxi d'un côté, se traduira par environ la moitié des gaz sur le moteur pour ce même côté." & vbCrLf & _
                                "Cela permet, lors du roulage des avions, le contrôle aux moteurs plutôt que d'une roue orientable ." & vbCrLf & _
                                "Ce mode authorize aussi des manoeuvres accrobatiques impossibles avec un seul moteur." & vbCrLf & _
                                "Ce mode est déactivé si un des moteurs est à l'arrêt." & vbCrLf & _
                                "Il est actif si les deux moteurs ou aucun des moteurs fonctionnent." & vbCrLf & _
                                "Cela permet les essais au banc ainsi que l'exploitation si les deux moteurs sont en marche", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 4")
                    '"Les moteurs ne sont pas synchronisés tant que le manche du gouvernail n'est pas au-dessus" & vbCrLf & _
                    '"du point de gouvernail bouvillon de désengagement ( 1/3 de la puissance )", ShowMsgImage.Info)
                Case 5 : ShowMsg("Canal auxilaire contrôle les bougies (FACTORY PRESET MODE)" & vbCrLf & _
                                "Dans ce mode, la canal auxilaire active ou pas le contrôle des bougies." & vbCrLf & _
                                "Un inter 2 positions sera utilisé. Dans une position les bougies seront" & vbCrLf & _
                                "allimentées et pas dans l'autre.", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 5")
                Case 6 : ShowMsg("Canal auxilaire contrôle les bougies (sans zone morte)", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 6")
                    'ShowMsg(ByVal Text As String, ByVal Question As String, ByVal Buttons As ShowMsgButtons, ByVal DefaultButton As ShowMsgDefaultButton, ByVal Title As String) 
            End Select
        ElseIf My.Settings.Language = "English" Then
            Select Case Convert.ToInt16(textAuxiliaireMode.Text)
                Case 0 : ShowMsg("Select an Auxiliary mode (use buttons + or - !!!)", ShowMsgImage.Info, "Channel Auxiliary Error !!!")
                Case 1 : ShowMsg("MODE1: No AUX CH" & vbCrLf & _
                                "This is the mode the device should be set to if an AUX CH is not connected to anything." & vbCrLf & _
                                "The device lets the throttle stick control servos below 1/5th stick. Above 1/5th stick the" & vbCrLf & _
                                "devices moves the servos to that position and then synchronizes the engines." & vbCrLf & _
                                "If the stick is moved the process is repeated.", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 1")
                Case 2 : ShowMsg("MODE2: Independent Run Up Mode" & vbCrLf & _
                                "In this mode it is assumed that the AUX CH input is connected to a 3-position switch." & vbCrLf & _
                                "If the output of this channel is less than 1/3 deflection engine1 is controlled by the throttle" & vbCrLf & _
                                "stick while engine2 is held at the programmed idle position. If the AUX CH output is" & vbCrLf & _
                                "between 1/3 and 2/3rds full deflection then the throttle stick controls both engines and the" & vbCrLf & _
                                "devices operates just like it was in mode 1. If the AUX CH output is greater than 2/3rds" & vbCrLf & _
                                "deflection then the throttle stick controls engine2 and engine1 is held in the" & vbCrLf & _
                                "preprogrammed idle position. This mode is useful for carb adjustments and mixture fine tuning.", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 2")
                Case 3 : ShowMsg("MODE3: AUX CH Sync Defeat" & vbCrLf & _
                                "In this mode it is assumed that the aux channel input is connected to a channel with a 2-" & vbCrLf & _
                                "position switch on the transmitter. In one position the synchronization function is" & vbCrLf & _
                                "enabled. In the other position the device does nothing and is simply a 'Y' cable (although" & vbCrLf & _
                                "servo directions and center is still controlled by the device)." & vbCrLf & vbCrLf & _
                                "This is a useful mode for understanding how your plane will react while the engines are" & vbCrLf & _
                                "being controlled by the device.", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 3")
                Case 4 : ShowMsg("MODE4: AUX CH is for Rudder Steering(!!not usable for now !!)" & vbCrLf & _
                                "In this mode the device operates as in mode 1 (no AUX CH) above 1/3rd throttle." & vbCrLf & _
                                "Below 1/3rd throttle the AUX CH is assumed to be connected to the RUDDER receiver output." & vbCrLf & _
                                "There is a dead band around the rudder center (so that rudder trim does not affect engine RPM)." & vbCrLf & _
                                "When the rudder stick is moved far enough past center to get out of the dead band" & vbCrLf & _
                                "it starts increasing the throttle on one engine. Moving the rudder stick in the other" & vbCrLf & _
                                "direction will result in increasing the throttle on the other engine." & vbCrLf & _
                                "Full rudder on the stick at idle will result in about half throttle on the engine for that side." & vbCrLf & vbCrLf & _
                                "This allows for taxiing airplanes with engine control rather than a steerable wheel." & vbCrLf & vbCrLf & _
                                "This mode should also result in some interesting aerobatic maneuvers not possible with single engine aircraft." & vbCrLf & vbCrLf & _
                                "This mode is disabled if only one engine is running. It is operational if both engines or neither engine is running." & vbCrLf & _
                                "This allows for bench testing as well as operation only if both engines are running." & vbCrLf & vbCrLf & _
                                "Engines are not synchronized until the stick is above the rudder steer disengagement point (33% throttle).", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 4")
                Case 5 : ShowMsg("AUX CH Controls Glow plugs on and off (FACTORY PRESET MODE)" & vbCrLf & _
                                "In this mode the AUX CH turns the glow plug drivers on and off." & vbCrLf & _
                                "A two position switch should be used and in one position glow plugs will be off" & vbCrLf & _
                                "and in the other position the glow plugs will be on.", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 5")
                Case 6 : ShowMsg("NO DEADSTICK DETECTION with AUX CH Controls Glow plugs" & vbCrLf & _
                                "This mode is the same as MODE5 except that the engines are not idled in the event of a deadstick." & vbCrLf & _
                                "The engines are synchronized when the stick is above idle and both engines are running." & vbCrLf & _
                                "If both engines are not running then the throttle servos are just moved to the transmitter stick position." & vbCrLf & _
                                "This allows full throttle control of one running engine at all times." & vbCrLf & vbCrLf & _
                                "The reason for this mode is if you have a very stable twin engine plane that flies well on" & vbCrLf & _
                                "one engine you may not want to idle the running engine in the event of a dead stick on one engine." & vbCrLf & vbCrLf & _
                                "Plugs are controlled by the AUX Channel just like in Mode 5." & vbCrLf & vbCrLf & _
                                "In this mode the AUX CH turns the glow plug drivers on and off." & vbCrLf & _
                                "A two position switch should be used and in one position glow plugs will be" & vbCrLf & _
                                "off and in the other position the glow plugs will be on.", ShowMsgImage.Info, "Canal Auxiliaire <--> Mode 6")

            End Select
        End If
    End Sub

    Private Sub btnSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSend.Click
        If SerialPort1.IsOpen() Then
            SerialPort1.Write(Trim(Str(txtMessage.Text)) & vbCrLf)
            SerialPort1.WriteLine(term.TextBoxTerminalComPort.Text)
        End If
    End Sub
    Private Sub TextBoxSendToCom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMessage.KeyPress
        If e.KeyChar = ControlChars.Cr Then
            SerialPort1.WriteLine(txtMessage.Text)
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        term.TextBoxTerminalComPort.Clear()
    End Sub

    Private Sub ButtonAuxMini_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAuxMini.Click
        PictureBoxAuxMini.Image = My.Resources.rectangle_vert
        PictureBoxAuxMiddle.Image = My.Resources.rectangle_rouge
        PictureBoxAuxMaxi.Image = My.Resources.rectangle_rouge
        LabelAux.Text = 0
        ProgressBarAuxiliary.Value = 0
        SerialPort1.Write(Trim(Str(181)) & vbCr)

    End Sub

    Private Sub ButtonAuxMiddle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAuxMiddle.Click
        PictureBoxAuxMini.Image = My.Resources.rectangle_rouge
        PictureBoxAuxMiddle.Image = My.Resources.rectangle_vert
        PictureBoxAuxMaxi.Image = My.Resources.rectangle_rouge
        LabelAux.Text = 90
        ProgressBarAuxiliary.Value = 90
        SerialPort1.Write(Trim(Str(270)) & vbCr)
    End Sub

    Private Sub ButtonAuxMaxi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAuxMaxi.Click
        PictureBoxAuxMini.Image = My.Resources.rectangle_rouge
        PictureBoxAuxMiddle.Image = My.Resources.rectangle_rouge
        PictureBoxAuxMaxi.Image = My.Resources.rectangle_vert
        LabelAux.Text = 180
        ProgressBarAuxiliary.Value = 180
        'SerialPort1.Write(Trim(Str(360)) & vbCr)
    End Sub

    Private Sub ButtonSettingsHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSettingsHelp.Click
        If RichTextBoxSettingsHelp.Visible = False Then

            RichTextBoxSettingsHelp.Visible = True
            RichTextBoxSettingsHelp.Location = New Point(4, 21)
            RichTextBoxSettingsHelp.Width = 584 '584; 303
            RichTextBoxSettingsHelp.Height = 303
            SettingsHelpText()
            labelExtervalVoltageUsed.Visible = False
            ButtonReadTempVoltage.Visible = False
            PictureBoxReadHardwareOnOff.Visible = False
            TextVoltageExterne.Visible = False
            LabelDEBUG.Visible = False
            txtMessage.Visible = False
            btnSend.Visible = False
            ButtonClear.Visible = False
            labelConfigModule.Visible = False
        Else
            RichTextBoxSettingsHelp.Visible = False
            If labelExtervalVoltageUsed.Text = "Not Used" Then
                labelExtervalVoltageUsed.Visible = True
            Else
                ButtonReadTempVoltage.Visible = True
                PictureBoxReadHardwareOnOff.Visible = True
                TextVoltageExterne.Visible = True
            End If

            LabelDEBUG.Visible = True
            txtMessage.Visible = True
            btnSend.Visible = True
            ButtonClear.Visible = True
            labelConfigModule.Visible = True
        End If


    End Sub

#End Region

#Region "Save Reset Delete Settings"
    Private Sub ButtonAnnulerModif_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnnulerModif.Click
        TabPage1.Select()
    End Sub

    Private Sub buttonEraseEEprom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonEraseEEprom.Click
        SerialPort1.Write(Trim(Str(370)) & vbCr)
        ProgressBarSaveSettings.Visible = True
        For p As Integer = 0 To 100 Step 25
            ProgressBarSaveSettings.Value = p
        Next
        ShowMsg("!!! You must to restart your module !!!", ShowMsgImage.Info, "Info")
    End Sub

    Private Sub buttonResetArduino_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonResetArduino.Click
        ProgressBarSaveSettings.Visible = False
        Thread.Sleep(500)

        SerialPort1.RtsEnable = True

        Debug.WriteLine("DTR +")
        System.Threading.Thread.Sleep(1000)

        SerialPort1.DtrEnable = True 'DTR -
        Debug.WriteLine("DTR -")
        System.Threading.Thread.Sleep(1000)

        SerialPort1.DtrEnable = False 'DTR +
        Debug.WriteLine("DTR +")
        System.Threading.Thread.Sleep(1000)

        SerialPort1.RtsEnable = False


        If SerialPort1.IsOpen Then
            PictureBoxConnectedOK.Image = My.Resources.rectangle_rouge
            SerialPort1.Write(Trim(Str(0)) & vbCr) 'positionne les moteurs à la postion 0°
            Me.Text = ":-("
            Button_Connect.Enabled = True
            ButtonAuxMiddle.PerformClick()
            SerialPort1.Close()
        End If

    End Sub

    Private Sub ButtonConfigDefaut_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonConfigDefaut.Click
        ProgressBarSaveSettings.Visible = True
        LabelModifications.Enabled = True

        SerialPort1.Write(Trim(Str(369)) & vbCr)
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
        LabelModifications.ForeColor = Color.Green

        LabelModifications.Text = "--"
        ProgressBarSaveSettings.Value = 25
        Thread.Sleep(1000)
        LabelModifications.Text = "--  --"
        ProgressBarSaveSettings.Value = 50
        Thread.Sleep(1000)
        LabelModifications.Text = "--  --  --"
        ProgressBarSaveSettings.Value = 75
        Thread.Sleep(1000)
        ProgressBarSaveSettings.Value = 100
        ShowMsg("!!! You must to restart your module !!!", ShowMsgImage.Info, "Info")
    End Sub
    Private Sub ButtonSauvegardeConfig_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSauvegardeConfig.Click
        ProgressBarSaveSettings.Visible = True
        ProgressBarSaveSettings.Value = 0
        'format envoyé : 
        'S1, 1500, 1500, 1000, 1000, 2, 2000, 1250, 1200, 1900, 1,0
        'S2,0,99.00,2,0,0,0,1000,20000,0

        MessageToSend = "S1,"
        MessageToSend &= textCentreServo1.Text & ","     'centerposServo1
        MessageToSend &= textCentreServo2.Text & ","    'centerposServo2
        MessageToSend &= textIdleServo1.Text & ","      'idelposServos1
        MessageToSend &= textIdleServo2.Text & ","      'idelposServos2
        MessageToSend &= textTempsReponse.Text & ","    'responseTime
        MessageToSend &= textMaxiMoteurs.Text & ","     'fullThrottle
        MessageToSend &= textDebutSynchro.Text & ","    'beginSynchro
        MessageToSend &= textMiniGenerale.Text & ","    'minimumPulse_US
        MessageToSend &= textMaxiGenerale.Text & ","    'maximumPulse_US
        MessageToSend &= textAuxiliaireMode.Text & ","  'auxChannel
        If CheckBoxInversionServo1.Checked = True Then MessageToSend &= "1" Else MessageToSend &= "0" 'reverseServo1

        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText(MessageToSend & vbCrLf)
        End If
        SerialPort1.Write(Trim(MessageToSend) & vbCr)

        Thread.Sleep(1000)


        MessageToSend = "S2,"
        If CheckBoxInversionServo2.Checked = True Then MessageToSend &= "1," Else MessageToSend &= "0," 'reverseServo2
        MessageToSend &= TextBoxDiffSpeedSimuConsigne.Text & ","    'difference speeds accepted
        MessageToSend &= textNombrePales.Text & ","     'nbPales
        MessageToSend &= labelModeRcRadio.Text - 1 & "," 'radio mode (1 à 4)
        MessageToSend &= "0," 'array(15).ToString & "," 'type de module (0=maître, 1=esclave)
        If CheckBoxFahrenheitDegrees.Checked = True Then MessageToSend &= "1," Else MessageToSend &= "0," 'Celcius/Fahrenheit

        MessageToSend &= textMiniMotorRPM.Text & ","    'speed motor mini
        MessageToSend &= textMaxiMotorRPM.Text & ","    'speed motor maxi

        ' MessageToSend &= LabelSignalType.Text
        Select Case LabelSignalType.Text
            Case "CPPM"
                MessageToSend &= "0"
            Case "SBUS"
                MessageToSend &= "1"
            Case "SRXL"
                MessageToSend &= "2"
            Case "SUMD"
                MessageToSend &= "3"
            Case "IBUS"
                MessageToSend &= "4"
            Case "JETI"
                MessageToSend &= "5"
        End Select

        'MsgBox(MessageToSend)
        If TextBoxDiffSpeedSimuConsigne.Text = "0" Then
            ShowMsg("Vitesse Error don't accept the value '0' !!!", ShowMsgImage.Critical, "Erreur")
            Exit Sub
        End If

        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText(MessageToSend & vbCrLf)
        End If
        SerialPort1.Write(Trim(MessageToSend) & vbCr)

        'assure le temps nécessaire à la sauvegarde
        LabelModifications.Enabled = True
        LabelModifications.Text = "--"
        ProgressBarSaveSettings.Value = 25
        Thread.Sleep(1000)
        LabelModifications.Text = "--  --"
        ProgressBarSaveSettings.Value = 50
        Thread.Sleep(1000)
        LabelModifications.Text = "--  --  --"
        ProgressBarSaveSettings.Value = 75
        Thread.Sleep(1000)
        ProgressBarSaveSettings.Value = 100
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications sauvegardées !" Else LabelModifications.Text = "Changes saved !"
        LabelModifications.ForeColor = Color.Green

        Thread.Sleep(1000)
        ShowMsg("!!! You must to restart your module !!!", ShowMsgImage.Info, "Info")
    End Sub

    Private Sub ButtonRcRadioMode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRcRadioMode.Click
        If RcMode.Visible = False Then
            RcMode.Show()
            RcMode.Location = New Point(My.Settings.LocationX + Me.Width + 3, My.Settings.LocationY)
        Else
            RcMode.Hide()
        End If
    End Sub

    'Read buttons
    Private Sub EnableDisableButtonsRead(ByVal state As Boolean)
        ButtonReadCenter1.Enabled = state
        ButtonReadCenter2.Enabled = state
        ButtonIdleMoteur1.Enabled = state
        ButtonIdleMoteur2.Enabled = state
        ButtonMaxiMoteurs.Enabled = state
        ButtonDebutSynchro.Enabled = state
        ButtonMoinsVitesseReponse.Enabled = state
        ButtonPlusVitesseReponse.Enabled = state

    End Sub



#End Region

#Region "Motors"
    Private Sub CheckBoxAnaDigi_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxAnaDigi.CheckedChanged

        If CheckBoxAnaDigi.Checked = True Then
            CheckBoxAnaDigi.Text = "Analogic"
            AquaGaugeMoteur1.Visible = True
            AquaGaugeMoteur2.Visible = True
            'LabelVitesse1.Visible = True
            'LabelVitesse2.Visible = True

            SevenSegmentArray1.Visible = False
            SevenSegmentArray2.Visible = False
            labelMotor1.Location = New Point(115, 11)
            labelMotor2.Location = New Point(406, 11)

        Else
            CheckBoxAnaDigi.Text = "Digital"
            AquaGaugeMoteur1.Visible = False
            AquaGaugeMoteur2.Visible = False
            'LabelVitesse1.Visible = False
            'LabelVitesse2.Visible = False

            SevenSegmentArray1.Visible = True
            SevenSegmentArray2.Visible = True
            SevenSegmentArray1.Location = New Point(0, 45)
            SevenSegmentArray2.Location = New Point(0, 201)
            labelMotor1.Location = New Point(18, 50)
            labelMotor2.Location = New Point(18, 209)

        End If

    End Sub
#End Region

#Region "Simulation"
    Private Sub TrackBarMotors_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarMotors.Scroll

    End Sub

    Private Sub ButtonServoTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonServoTest.Click
        CheckBoxReadTracBarMotorOrMotorThrottle.Checked = False
        PictureBoxSimuThrottle.Image = My.Resources.TrackBar
        TrackBarMotors.Visible = True
        ProgressBarThrottle.Visible = True
        LabelMotors.Visible = True
        labelMotorsSimulation.Visible = True
        LabelOnSimuMoveThrottle.Visible = False
        If SerialPort1.IsOpen Then
            Dim t As New Threading.Thread(AddressOf TestServo)
            t.Start()
        Else
            ShowMsg("The module isn't connected !", ShowMsgImage.Info, "Error")
        End If

    End Sub
    Private Sub TestServo()
        For t As Integer = 0 To 180
            TrackBarMotors.Value = t
            LabelMotors.Text = t.ToString
            ProgressBarThrottle.Value = t
            SerialPort1.WriteLine(t.ToString)
            If term.Visible = True Then
                term.TextBoxTerminalComPort.AppendText(Str(t) & vbCrLf)
            End If
            Thread.Sleep(5)
        Next
        Thread.Sleep(1000)
        For t As Integer = 180 To 0 Step -1
            TrackBarMotors.Value = t
            LabelMotors.Text = t.ToString
            ProgressBarThrottle.Value = t
            SerialPort1.WriteLine(t.ToString)
            If term.Visible = True Then
                term.TextBoxTerminalComPort.AppendText(Str(t) & vbCrLf)
            End If
            Thread.Sleep(5)
        Next
    End Sub

    Private Sub TrackBarMotors_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TrackBarMotors.ValueChanged
        If CheckBoxReadTracBarMotorOrMotorThrottle.Checked = False Then
            Dim t As New Threading.Thread(AddressOf OnTrackBarMotorsChange)
            t.Start()
        End If
    End Sub
    Private Sub OnTrackBarMotorsChange()
        pos = TrackBarMotors.Value
        LabelMotors.Text = Str(pos)
        ProgressBarThrottle.Value = pos
        SerialPort1.WriteLine(Trim(Str(pos)))
        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText(LabelMotors.Text & vbCrLf)
        End If

        If RecorderIsOn = True Then
            AddRecord(svfd.FileName, LabelMotors.Text)
        End If
    End Sub

    'Utilisé en mode Auxiliaire 4
    Private Sub TrackBarRudder_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TrackBarRudder.ValueChanged
        Try
            posRudder = CInt(TrackBarRudder.Value)
            SerialPort1.Write(Trim(Str(posRudder + 180)) & vbCr) '180 to 360
            LabelAux.Text = posRudder
            ProgressBarAuxiliary.Value = posRudder
        Catch ex As Exception
            ShowMsg("Please,connect to the module!", ShowMsgImage.Info, "Error")
        End Try
    End Sub

    'Utilisé en simulation (force la vitesse des moteurs)
    Private Sub TrackBarSpeedSimu1_MouseUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarSpeedSimu1.MouseUp
        Try
            If CheckBoxSimuSynchroSpeeds.Checked = True Then TrackBarSpeedSimu2.Value = TrackBarSpeedSimu1.Value
            SerialPort1.Write(Trim(MessageToSend) & vbCr)
            MessageToSend = ""
        Catch ex As Exception
            ShowMsg("Please,connect to the module!", ShowMsgImage.Info, "Error")
        End Try
    End Sub
    Private Sub TrackBarSpeedSimu1_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TrackBarSpeedSimu1.ValueChanged
        Try
            TextBoxForceSpeedSimu1.Text = Str(TrackBarSpeedSimu1.Value)

            MessageToSend = ""
            MessageToSend = "888" & ","
            MessageToSend &= TextBoxForceSpeedSimu1.Text & ","
            MessageToSend &= TextBoxForceSpeedSimu2.Text & ","
            MessageToSend &= TextBoxDiffSpeedSimuConsigne.Text

            'MsgBox(MessageToSend)
            SerialPort1.Write(Trim(MessageToSend) & vbCr)
            MessageToSend = ""
        Catch ex As Exception
            ShowMsg(ex.Message, ShowMsgImage.Info, "Error")
        End Try
    End Sub
    Private Sub TrackBarSpeedSimu2_MouseUp(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TrackBarSpeedSimu2.MouseUp
        Try
            If CheckBoxSimuSynchroSpeeds.Checked = True Then TrackBarSpeedSimu1.Value = TrackBarSpeedSimu2.Value
            SerialPort1.Write(Trim(MessageToSend) & vbCr)
            MessageToSend = ""
        Catch ex As Exception
            ShowMsg("Please,connect to the module!", ShowMsgImage.Info, "Error")
        End Try
    End Sub
    Private Sub TrackBarSpeedSimu2_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TrackBarSpeedSimu2.ValueChanged
        Try
            TextBoxForceSpeedSimu2.Text = Str(TrackBarSpeedSimu2.Value)

            MessageToSend = ""
            MessageToSend = "888" & ","
            MessageToSend &= TextBoxForceSpeedSimu1.Text & ","
            MessageToSend &= TextBoxForceSpeedSimu2.Text & ","
            MessageToSend &= TextBoxDiffSpeedSimuConsigne.Text

            'MsgBox(MessageToSend)
            SerialPort1.Write(Trim(MessageToSend) & vbCr)
        Catch ex As Exception
            ShowMsg(ex.Message, ShowMsgImage.Info, "Error")
        End Try
    End Sub

    Private Sub CheckBoxSimuSynchroSpeeds_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxSimuSynchroSpeeds.Click
        LabelModifications.Enabled = True
        LabelModifications.ForeColor = Color.Red
        If My.Settings.Language = "French" Then LabelModifications.Text = "Modifications non sauvegardées !" Else LabelModifications.Text = "Changes not saved !"
        If (CheckBoxSimuSynchroSpeeds.Checked = True) Then
            TrackBarSpeedSimu2.Value = TrackBarSpeedSimu1.Value
            If My.Settings.Language = "French" Then CheckBoxSimuSynchroSpeeds.Text = "Oui" Else CheckBoxSimuSynchroSpeeds.Text = "Yes"
        Else
            If My.Settings.Language = "French" Then CheckBoxSimuSynchroSpeeds.Text = "Non" Else CheckBoxSimuSynchroSpeeds.Text = "No"
        End If

    End Sub

    Private Sub ButtonSpeedSimuOn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSpeedSimuOn.Click
        If SimualtionSpeed = False Then
            'format envoyé : 888,1656,1653,100
            SimualtionSpeed = True
            PictureBoxSimuOnOff.Image = My.Resources.rectangle_vert
            TrackBarSpeedSimu1.Value = CInt(TextBoxForceSpeedSimu1.Text)
            TrackBarSpeedSimu2.Value = CInt(TextBoxForceSpeedSimu2.Text)

            MessageToSend = ""
            MessageToSend = "501" 'Simu ON
            'MessageToSend &= TextBoxForceSpeedSimu1.Text & ","
            'MessageToSend &= TextBoxForceSpeedSimu2.Text & ","
            'MessageToSend &= TextBoxDiffSpeedSimuConsigne.Text

            'MsgBox(MessageToSend)
            SerialPort1.Write(Trim(MessageToSend) & vbCr)

        Else
            'format envoyé : 889,0,0,100
            SimualtionSpeed = False
            PictureBoxSimuOnOff.Image = My.Resources.rectangle_rouge

            'MessageToSend = ""
            MessageToSend = "500" 'Simu OFF

            'MsgBox(MessageToSend)
            SerialPort1.Write(Trim(MessageToSend) & vbCr)
        End If


    End Sub

    Private Sub ButtonGlowPlugOnOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonGlowPlugOnOff.Click
        GlowPlugIsOn = Not GlowPlugIsOn
        SerialPort1.Write(Trim(Str(404)) & vbCr)
        If GlowPlugIsOn = True Then
            PictureBoxGlowPlugOnOff.Image = My.Resources.rectangle_vert
        Else
            PictureBoxGlowPlugOnOff.Image = My.Resources.rectangle_rouge
        End If

    End Sub

    Private Sub ButtonSavePIDVariables_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSavePIDVariables.Click
        'MessageToSend = ""
        'MessageToSend = "887,"
        'MessageToSend &= textBoxKpControl.Text & ","
        'MessageToSend &= textBoxKiControl.Text & ","
        'MessageToSend &= textBoxKdControl.Text

        ''MsgBox(MessageToSend)
        'SerialPort1.Write(Trim(MessageToSend) & vbCr)
        'If term.Visible = True Then
        '    term.TextBoxTerminalComPort.AppendText(Trim(MessageToSend) & vbCrLf)
        'End If
    End Sub

    Private Sub ButtonRecorder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonRecorder.Click
        RecorderIsOn = Not RecorderIsOn
        If RecorderIsOn = True Then
            SerialPort1.Write("701" & vbCr)
            If My.Settings.Language = "French" Then
                ButtonRecorder.Text = "Arrêt Enregis."
            ElseIf My.Settings.Language = "English" Then
                ButtonRecorder.Text = "Stop Recorder"
            End If
            PictureBoxRecorder.Image = My.Resources.rectangle_vert

            If CheckBoxReadTracBarMotorOrMotorThrottle.Checked = False Then
                'TrackBarMotors.ValueChanged call 'OnTrackBarMotorsChange' on a new thread !
                TimerRXMotors.Interval = 300
                TimerRXMotors.Enabled = False
            Else
                svfd.FileName = Path.GetFileNameWithoutExtension(svfd.FileName) & "_Throttle.record"
                TimerRXMotors.Interval = 100
                TimerRXMotors.Enabled = True
            End If
        Else
            SerialPort1.Write("700" & vbCr)
            If My.Settings.Language = "French" Then
                ButtonRecorder.Text = "Départ Enregis."
            ElseIf My.Settings.Language = "English" Then
                ButtonRecorder.Text = "Start Recorder"
            End If
            PictureBoxRecorder.Image = My.Resources.rectangle_rouge
        End If

    End Sub

    Dim ChronoInMinutes As Integer
    Dim cntDown As Date
    Private Sub ButtonPlayer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonPlayer.Click
        PlayerIsOn = Not PlayerIsOn
        If PlayerIsOn = True Then
            SerialPort1.Write("703" & vbCr)
            'If CheckBoxReadTracBarMotorOrMotorThrottle.Checked = False Then
            '    fd.Filter = "Text files|*.record"
            'Else
            '    fd.Filter = "Text files|*_Throttle.record"
            'End If

            'If fd.ShowDialog() = DialogResult.OK Then
            '    If My.Settings.Language = "French" Then
            '        ButtonPlayer.Text = "Arrêt Lecture"
            '    ElseIf My.Settings.Language = "English" Then
            '        ButtonPlayer.Text = "Stop Player"
            '    End If
            PictureBoxPlayer.Image = My.Resources.rectangle_vert
            LabelChrono.Visible = True
            LabelTestNow.Visible = True
            LabelChrono.ForeColor = Color.Green
            LabelTestNow.ForeColor = Color.Red
            LabelChronoSS.Text = "00"
            If CheckBoxChronoOnOff.Checked = True Then
                TimerChrono.Interval = 1000
                TimerChrono.Enabled = True
                TimerChrono.Start()
                ChronoInMinutes = (CInt(TextBoxChronoHH.Text) * 60) + CInt(TextBoxChronoMM.Text)
                cntDown = Date.Now.AddMinutes(ChronoInMinutes)

                BackgroundWorker1.WorkerSupportsCancellation = True
                BackgroundWorker1.RunWorkerAsync()
            End If
            'End If

        Else
            SerialPort1.Write("702" & vbCr)
            If My.Settings.Language = "French" Then
                ButtonPlayer.Text = "Départ Lecture"
            ElseIf My.Settings.Language = "English" Then
                ButtonPlayer.Text = "Start Player"
            End If
            PictureBoxPlayer.Image = My.Resources.rectangle_rouge
            If CheckBoxChronoOnOff.Checked = True Then
                TimerChrono.Enabled = False
            End If
        End If

    End Sub

    Private Sub TextBoxChronoMM_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxChronoMM.TextChanged
        LabelChronoSS.Text = "00"
    End Sub

    Private Sub TextBoxChronoHH_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBoxChronoHH.TextChanged
        LabelChronoSS.Text = "00"
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        While PlayerIsOn
            Application.DoEvents()
            ReadRecord(fd.FileName)
            Thread.Sleep(1000)
        End While
    End Sub

    Private Sub bw_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
        'Me.tbProgress.Text = e.ProgressPercentage.ToString() & "%"
        'If cntDown = Date.Now.AddMinutes(ChronoInMinutes) Then
        '    BackgroundWorker1.CancelAsync()
        'End If
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        'If e.Cancelled = True Then
        'ElseIf e.Error IsNot Nothing Then
        '    'Me.tbProgress.Text = "Error: " & e.Error.Message
        'Else
        '    'Me.tbProgress.Text = "Done!"
        'End If
    End Sub

    ''' <summary>
    ''' Build a record list file
    ''' </summary>
    ''' <param name="recorderList"></param>
    ''' <remarks></remarks>
    Public Sub AddRecord(ByVal recorderList As String, ByVal Position As String)
        If File.Exists(recorderList) = False Then
            Using objStreamWriter As StreamWriter = New StreamWriter(recorderList, True, Encoding.Unicode)
                objStreamWriter.Write(" 0" & vbCrLf)
                objStreamWriter.Close()
            End Using
        End If

        If CheckBoxReadTracBarMotorOrMotorThrottle.Checked = True Then
            Position = Trim(Position)
            'Position = Str(mapValue(Position, CInt(textMiniGenerale.Text), CInt(textMaxiGenerale.Text), 0, 180))
            'If CInt(Position) < 0 Then Position = "0"
            'If CInt(Position) > 180 Then Position = "180"
            'Position = Str(mapValue(Position, CInt(textMiniGenerale.Text), CInt(textMaxiGenerale.Text), 0, 180))
            If CInt(Position) < CInt(textMiniGenerale.Text) Then Position = textMiniGenerale.Text
            If CInt(Position) > CInt(textMaxiGenerale.Text) Then Position = textMaxiGenerale.Text
        End If
        File.AppendAllText(recorderList, Position & vbCrLf, Encoding.Unicode)

    End Sub

    ''' <summary>
    ''' read a record list
    ''' </summary>
    ''' <param name="recorderList"></param>
    ''' <remarks></remarks>
    Public Sub ReadRecord(ByVal recorderList As String)
        If File.Exists(recorderList) = True Then
            servoPosition = IO.File.ReadAllLines(recorderList, Encoding.Unicode)
            For Each p In servoPosition
                If PlayerIsOn = False Then Exit Sub ' force la sortie du player
                TrackBarMotors.Value = CInt(p)
                LabelMotors.Text = p
                ProgressBarThrottle.Value = CInt(p)
                SerialPort1.WriteLine(Trim(Str(p)))
                If term.Visible = True Then
                    term.TextBoxTerminalComPort.AppendText(LabelMotors.Text & vbCrLf)
                End If
                'If CheckBoxReadTracBarMotorOrMotorThrottle.Checked = True Then
                Thread.Sleep(25)
                'End If
            Next
        End If
    End Sub

    Private Sub ButtonSimulationHelp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSimulationHelp.Click
        If RichTextSimulationHelp.Visible = False Then
            RichTextSimulationHelp.Visible = True
            RichTextSimulationHelp.Location = New Point(4, 21)
            RichTextSimulationHelp.Width = 584
            RichTextSimulationHelp.Height = 303
            SimulationHelpText()
        Else
            RichTextSimulationHelp.Visible = False
        End If
    End Sub


    Private Sub SimulationHelpText()

        If My.Settings.Language = "French" Then
            RichTextSimulationHelp.Text = "Utilisation du mode 'Simulation':" & vbCrLf & _
                vbTab & "-Mode Manuel:" & vbCrLf & _
                vbTab & " Utilisez le curseur " & labelMotorsSimulation.Text & "." & vbCrLf & vbCrLf & _
                vbTab & "-Mode Auto:" & vbCrLf & _
                vbTab & " Utilisez le bouton Test (déplacement des servos de leurs positions" & vbCrLf &
                vbTab & " mini à maxi, puis retour)." & vbCrLf & vbCrLf & _
                vbTab & "-Mode Programmé: (a) lecture curseur ou b) manche radio)" & vbCrLf & _
                vbTab & " Utilisez le bouton " & CheckBoxReadTracBarMotorOrMotorThrottle.Text & " pour choisir a) or b)" & vbCrLf & _
                vbTab & " solution." & vbCrLf & _
                vbTab & " Utilisez le bouton " & ButtonRecorder.Text & " pour enregistrer les mouvements" & vbCrLf &
                vbTab & " des servos." & vbCrLf & _
                vbTab & "   a) déplacez le curseur." & vbCrLf & _
                vbTab & "   b) déplacez le manche moteur de la radio." & vbCrLf & _
                vbTab & " Utilisez le bouton " & ButtonPlayer.Text & " pour relire les" & vbCrLf &
                vbTab & " mouvements des servos." & vbCrLf &
                vbTab & " Option Chrono:" & vbCrLf &
                vbTab & " Définissez le temps que durera la lecture." & vbCrLf &
                vbTab & " Si l'option Chrono est dévalidée, la lecture sera permanente."

            MakeBold(RichTextSimulationHelp, labelMotorsSimulation.Text)
            MakeBold(RichTextSimulationHelp, "Test")
            MakeBold(RichTextSimulationHelp, "Utilisation du mode 'Simulation':")
            MakeBold(RichTextSimulationHelp, CheckBoxReadTracBarMotorOrMotorThrottle.Text)
            MakeBold(RichTextSimulationHelp, ButtonRecorder.Text)
            MakeBold(RichTextSimulationHelp, ButtonPlayer.Text)
            MakeBold(RichTextSimulationHelp, "Chrono")

        ElseIf My.Settings.Language = "English" Then
            RichTextSimulationHelp.Text = "How to use the mode 'Simulation':" & vbCrLf & _
                vbTab & "-Manual Mode:" & vbCrLf & _
                vbTab & " Use the cursor " & labelMotorsSimulation.Text & "." & vbCrLf & vbCrLf & _
                vbTab & "-Automatic Mode:" & vbCrLf & _
                vbTab & " Use the button Test (déplacement des servos de leurs positions" & vbCrLf &
                vbTab & " mini to maxi, then return)." & vbCrLf & vbCrLf & _
                vbTab & "-Programmed Mode:  (a) read cursor or b) motor's throttle)" & vbCrLf & _
                vbTab & " Use button " & CheckBoxReadTracBarMotorOrMotorThrottle.Text & " for select a) or b)" & vbCrLf &
                vbTab & " solution." & vbCrLf &
                vbTab & " Use the button " & ButtonRecorder.Text & " for save the servo's movements." & vbCrLf &
                vbTab & "   a) move the cursor." & vbCrLf & _
                vbTab & "   b) move the motor's throttle." & vbCrLf & _
                vbTab & " Use the button " & ButtonPlayer.Text & " for read the" & vbCrLf &
                vbTab & " servo's movements." & vbCrLf &
                vbTab & " Chrono Option:" & vbCrLf &
                vbTab & " Define the time for read." & vbCrLf &
                vbTab & " If you uncheck the Chrono option, the player will play always."

            MakeBold(RichTextSimulationHelp, labelMotorsSimulation.Text)
            MakeBold(RichTextSimulationHelp, "Test")
            MakeBold(RichTextSimulationHelp, "How to use the mode 'Simulation':")
            MakeBold(RichTextSimulationHelp, CheckBoxReadTracBarMotorOrMotorThrottle.Text)
            MakeBold(RichTextSimulationHelp, ButtonRecorder.Text)
            MakeBold(RichTextSimulationHelp, ButtonPlayer.Text)
            MakeBold(RichTextSimulationHelp, "Chrono")
        End If
    End Sub

#End Region

#Region "FRAM"
    Private Sub ButtonWriteInFram_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonWriteInFram.Click
        Dim y As Double, y2 As Double

        y = 1000
        y2 = 2000
        MessageToSend = ""
        MessageToSend = "602" ' & ","
        'MessageToSend &= "x," 'x.ToString.Replace(",", ".") & ","
        'MessageToSend &= y.ToString.Replace(",", ".") & ","
        'MessageToSend &= y2.ToString.Replace(",", ".")
        SerialPort1.Write(MessageToSend & vbCr)
        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText(MessageToSend & vbCrLf)
        End If
        'Next i
    End Sub

    Private Sub buttonFramInfos_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buttonFramInfos.Click
        labelFRAMIsUsed.Text = "FRAM Used:"
        labelFRAMType.Text = "FRAM Type:"
        labelSDCardSize.Text = "FRAM size:"
        SerialPort1.Write("600" & vbCr)
        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText("600" & vbCrLf)
        End If
    End Sub

    Private Sub ButtonFramRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFramRead.Click
        ListBoxSDListFiles.Items.Clear()
        SerialPort1.Write("601" & vbCr)
        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText("601" & vbCrLf)
        End If
    End Sub

    Private Sub ButtonFramErase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonFramErase.Click
        SerialPort1.Write("603" & vbCr)
        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText("603" & vbCrLf)
        End If
    End Sub

    Private Sub ButtonDumpLogFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonDumpLogFile.Click
        If ListBoxSDListFiles.SelectedItem <> "" Then
            MessageToSend = ""
            MessageToSend = Replace(ListBoxSDListFiles.SelectedItem, "   ", "|")
            Dim SDL() As String
            SDL = MessageToSend.Split("|")
            MessageToSend = "604" ' & SDL(0) & ",0,0"
            SerialPort1.Write(MessageToSend & vbCr)
            If term.Visible = True Then
                term.TextBoxTerminalComPort.AppendText(MessageToSend & vbCrLf)
            End If
        Else
            ShowMsg("Please,select a file", ShowMsgImage.Info, "Info")
        End If

    End Sub


#End Region

#Region "TabPages"
    'TabPage1

    Private Sub LinkLabel1_LinkClicked(sender As System.Object, e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)

    End Sub
    Private Sub ButtonAbout_Click(sender As System.Object, e As System.EventArgs) Handles ButtonAbout.Click
        'ShowMsg("Version Interface: " & InterfaceVersion & vbCrLf &
        '        "Version Module: " & ModuleVersion & vbCrLf & vbCrLf &
        '        "Copyright @2017" & vbCrLf & vbCrLf & "mailto:pierrotm777@gmail.com", ShowMsgImage.Info, "Info")

        ShowMsg("Version Interface: " & InterfaceVersion & vbCrLf &
                "Version Module: " & ModuleVersion & vbCrLf & vbCrLf &
                "Copyright @2017", ShowMsgImage.Info, "Info")
    End Sub

    Private Sub ButtonSettings_Click(sender As System.Object, e As System.EventArgs) Handles TabPage2.Enter
        Me.Width = 620
        Me.Height = 430
        CheckBoxReadTracBarMotorOrMotorThrottle.Checked = False

        Try
            SerialPort1.Write("368" & vbCr) 'recupération de la configuration du module
            Thread.Sleep(500)
            If textMiniGenerale.Text = "1500" And textMaxiGenerale.Text = "1500" Then
                EnableDisableButtonsRead(False)
            Else
                EnableDisableButtonsRead(True)
            End If

        Catch ex As Exception
            ShowMsg("Please,connect to the module!", ShowMsgImage.Info, "Error")
        End Try

    End Sub

    Private Sub ButtonMoteurs_Click(sender As System.Object, e As System.EventArgs) Handles TabPage3.Enter
        AquaGaugeMoteur1.MaxValue = Convert.ToInt32(textMaxiMotorRPM.Text) '30000
        AquaGaugeMoteur1.Threshold1Stop = Convert.ToInt32(textMiniMotorRPM.Text) '2000 'end idle zone
        AquaGaugeMoteur1.Threshold2Stop = Convert.ToInt32(textMaxiMotorRPM.Text) '30000 'end red zone
        AquaGaugeMoteur1.Threshold2Start = AquaGaugeMoteur1.Threshold2Stop - 4000 'start red zone
        AquaGaugeMoteur2.MaxValue = Convert.ToInt32(textMaxiMotorRPM.Text) '30000
        AquaGaugeMoteur2.Threshold1Stop = Convert.ToInt32(textMiniMotorRPM.Text) '2000 'end idle zone
        AquaGaugeMoteur2.Threshold2Stop = Convert.ToInt32(textMaxiMotorRPM.Text) '30000
        AquaGaugeMoteur2.Threshold2Start = AquaGaugeMoteur2.Threshold2Stop - 4000
    End Sub

    Private Sub ButtonDataLogger_Click(sender As System.Object, e As System.EventArgs) Handles TabPage4.Enter
        zg1.GraphPane = New GraphPane()

        'http://www.codeproject.com/Articles/5431/A-flexible-charting-library-for-NET
        'ou
        'http://zedgraph.dariowiz.com/index8da4.html?title=Line_%26_Symbol_Charts

        'voir http://www.cnblogs.com/luxiaoxun/p/4161782.html

        myPane = zg1.GraphPane

        ' Set the titles and axis labels

        If My.Settings.Language = "French" Then
            myPane.Title.Text = "Vitesses Moteurs tr/mn"
            myPane.XAxis.Title.Text = "Date"
            myPane.YAxis.Title.Text = "Tours/Minutes Moteur 1"
            myPane.Y2Axis.Title.Text = "Tours/Minutes Moteur 2"
        ElseIf My.Settings.Language = "English" Then
            myPane.Title.Text = "Motor Speed RPM"
            myPane.XAxis.Title.Text = "Date"
            myPane.YAxis.Title.Text = "RPM Motor 1"
            myPane.Y2Axis.Title.Text = "RPM Motor 2"
        End If

        ' Change the color of the title
        myPane.Title.FontSpec.FontColor = Color.Green
        myPane.XAxis.Title.FontSpec.FontColor = Color.Green
        myPane.YAxis.Title.FontSpec.FontColor = Color.Green

        'myPane.XAxis.Title.FontSpec.IsBold = True
        'myPane.XAxis.Title.FontSpec.Size = 18
        'myPane.YAxis.Title.FontSpec.Size = 18
        'myPane.Title.FontSpec.FontColor = Color.Red
        'myPane.Title.FontSpec.Size = 22
        'myPane.XAxis.Scale.IsUseTenPower = True
        'myPane.YAxis.Scale.IsUseTenPower = True


        myPane.XAxis.Type = AxisType.Date
        myPane.XAxis.Scale.Format = "HH:mm:ss.fff"
        myPane.XAxis.Scale.Min = New XDate(DateTime.Now)
        myPane.XAxis.Scale.Max = New XDate(DateTime.Now.AddSeconds(10))
        myPane.XAxis.Scale.MinorUnit = DateUnit.Second
        myPane.XAxis.Scale.MajorUnit = DateUnit.Second
        'myPane.XAxis.Scale.MajorUnit = DateUnit.Minute



        ' Make up some data points based on the Sine function
        'Dim i As Integer, x As Double, y As Double, y2 As Double
        'For i = 0 To 35
        '    x = i * 5.0
        '    y = Math.Sin(i * Math.PI / 15.0) * 16.0
        '    y2 = y * 13.5
        '    list.Add(x, y)
        '    list2.Add(x, y2)
        'Next i

        ' Generate a red curve with diamond symbols, and "Alpha" in the legend
        If My.Settings.Language = "French" Then
            myCurve = myPane.AddCurve("Vitesse1", list, Color.Red, SymbolType.Diamond)
        ElseIf My.Settings.Language = "English" Then
            myCurve = myPane.AddCurve("Speed1", list, Color.Red, SymbolType.Diamond)
        End If
        ' Fill the symbols with white
        myCurve.Symbol.Fill = New Fill(Color.GreenYellow)
        ' Generate a blue curve with circle symbols, and "Beta" in the legend
        If My.Settings.Language = "French" Then
            myCurve = myPane.AddCurve("Vitesse2", list2, Color.Blue, SymbolType.Circle)
        ElseIf My.Settings.Language = "English" Then
            myCurve = myPane.AddCurve("Speed2", list2, Color.Blue, SymbolType.Circle)
        End If
        ' Fill the symbols with white
        myCurve.Symbol.Fill = New Fill(Color.LightPink)

        'Associate this curve with the Y2 axis (axe cental de y=0)
        'myCurve.IsY2Axis = True

        ' Show the x axis grid
        myPane.XAxis.MajorGrid.IsVisible = True

        ' Make the Y axis scale red
        myPane.YAxis.Scale.FontSpec.FontColor = Color.Red
        myPane.YAxis.Title.FontSpec.FontColor = Color.Red
        ' turn off the opposite tics so the Y tics don't show up on the Y2 axis
        myPane.YAxis.MajorTic.IsOpposite = True
        myPane.YAxis.MinorTic.IsOpposite = True
        ' Don't display the Y zero line
        myPane.YAxis.MajorGrid.IsZeroLine = True
        ' Align the Y axis labels so they are flush to the axis
        myPane.YAxis.Scale.Align = AlignP.Inside
        ' Manually set the axis range
        myPane.YAxis.Scale.Min = 0 '-30
        myPane.YAxis.Scale.Max = Convert.ToInt32(textMaxiMotorRPM.Text) '30


        'zoom auto on axes x et y
        myPane.XAxis.Scale.MinAuto = True
        myPane.XAxis.Scale.MaxAuto = True
        myPane.YAxis.Scale.MinAuto = True
        myPane.YAxis.Scale.MaxAuto = True
        myPane.Y2Axis.Scale.MinAuto = True
        myPane.Y2Axis.Scale.MaxAuto = True


        ' Enable the Y2 axis display
        myPane.Y2Axis.IsVisible = True
        ' Make the Y2 axis scale blue
        myPane.Y2Axis.Scale.FontSpec.FontColor = Color.Blue
        myPane.Y2Axis.Title.FontSpec.FontColor = Color.Blue
        ' turn off the opposite tics so the Y2 tics don't show up on the Y axis
        myPane.Y2Axis.MajorTic.IsOpposite = True
        myPane.Y2Axis.MinorTic.IsOpposite = True
        ' Display the Y2 axis grid lines
        myPane.Y2Axis.MajorGrid.IsVisible = True
        ' Align the Y2 axis labels so they are flush to the axis
        myPane.Y2Axis.Scale.Align = AlignP.Inside
        ' Manually set the axis range
        myPane.Y2Axis.Scale.Min = 0 '-30
        myPane.Y2Axis.Scale.Max = Convert.ToInt32(textMaxiMotorRPM.Text) '30

        ' Fill the axis background with a gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.LightGray, 45.0F)

        ' Add a text box with instructions
        Dim text As New TextObj("Zoom: left mouse & drag" & Chr(10) & _
                                "Pan: middle mouse & drag" & Chr(10) & _
                                "Context Menu: right mouse", 0.05F, 0.95F,
                                CoordType.ChartFraction, AlignH.Left, AlignV.Bottom)
        text.FontSpec.StringAlignment = StringAlignment.Near
        myPane.GraphObjList.Add(text)

        ' Enable scrollbars if needed
        zg1.IsShowHScrollBar = True
        zg1.IsShowVScrollBar = True
        zg1.IsAutoScrollRange = True
        zg1.IsScrollY2 = True

        zg1.IsShowPointValues = True

        ' Size the control to fit the window
        SetSize()

        ' Tell ZedGraph to calculate the axis ranges
        ' Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
        ' up the proper scrolling parameters
        zg1.AxisChange()
        ' Make sure the Graph gets redrawn
        zg1.Invalidate()

        'update datalogger
        TimerDataLogger.Enabled = True
    End Sub

    Private Sub ButtonSimulation_Click(sender As System.Object, e As System.EventArgs) Handles TabPage5.Enter
        MessageToSend &= textBoxKpControl.Text & ","
        MessageToSend &= textBoxKiControl.Text & ","
        MessageToSend &= textBoxKdControl.Text


        RichTextSimulationHelp.Visible = False
        LabelChrono.Visible = False
        LabelTestNow.Visible = False
        PictureBoxSimuThrottle.Enabled = False
    End Sub

    ' On resize action, resize the ZedGraphControl to fill most of the Form, with a small
    ' margin around the outside
    Private Sub Form1_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        SetSize()
    End Sub

    Private Sub SetSize()
        Dim loc As New Point(10, 15)
        zg1.Location = loc
        ' Leave a small margin around the outside of the control
        'Dim size As New Size(Me.ClientRectangle.Width - 40, Me.ClientRectangle.Height - 370)
        'zg1.Size = size
    End Sub

    'Display customized tooltips when the mouse hovers over a point
    Private Function MyPointValueEvent(ByVal control As ZedGraphControl, ByVal pane As GraphPane, ByVal curve As CurveItem, ByVal iPt As Integer) As String
        ' Get the PointPair that is under the mouse
        Dim pt As PointPair = curve(iPt)
        Return curve.Label.Text + " = " + pt.Y.ToString + " tours/mn  à " + pt.X.ToString '+ " seconde"
        'Dim myDate As Date = DateTime.ParseExact(pt.X.ToString("f0"), "yyMMddHHmm", CultureInfo.InvariantCulture)
        'Return curve.Label.Text + " = " + pt.Y.ToString + " tours/mn  à " + myDate '+ " seconde"
    End Function

    ' Customize the context menu by adding a new item to the end of the menu
    Private Sub MyContextMenuBuilder(ByVal control As ZedGraphControl, ByVal menu As ContextMenuStrip, ByVal mousePt As Point, ByVal objState As ZedGraphControl.ContextMenuObjectState)
        Dim item As New ToolStripMenuItem
        item.Name = "add-beta"
        item.Tag = "add-beta"
        item.Text = "Add a new Beta Point"
        AddHandler item.Click, AddressOf Me.AddBetaPoint

        menu.Items.Add(item)
    End Sub

    ' Handle the "Add New Beta Point" context menu item.  This finds the curve with
    ' the CurveItem.Label = "Beta", and adds a new point to it.
    Private Sub AddBetaPoint(ByVal sender As Object, ByVal args As EventArgs)
        ' Get a reference to the "Beta" curve PointPairList
        Dim x As Double, y As Double
        Dim ip As IPointListEdit = Nothing
        If My.Settings.Language = "French" Then
            ip = zg1.GraphPane.CurveList("Vitesse1").Points
        ElseIf My.Settings.Language = "English" Then
            ip = zg1.GraphPane.CurveList("Speed1").Points
        End If

        If (Not IsNothing(ip)) Then
            x = ip.Count * 5.0
            y = ip.Count
            ip.Add(x, y)
            zg1.AxisChange()
            zg1.Refresh()
        End If
    End Sub

    Private Sub zg1_ZoomEvent(ByVal control As ZedGraphControl, ByVal oldState As ZoomState, ByVal newState As ZoomState)
        'Here we get notification everytime the user zooms
    End Sub

#End Region

#Region "Programmer"
    'Tab programmer
    'https://skyduino.wordpress.com/2011/12/02/tutoriel-avrdude-en-ligne-de-commande/

    Private Sub Programmer_Click(sender As System.Object, e As System.EventArgs) Handles TabPage7.Enter
        If SerialPort1.IsOpen Then
            PictureBoxConnectedOK.Image = My.Resources.rectangle_rouge
            SerialPort1.Close()
            SerialPort1.Dispose()
            ShowMsg("Com Port " & My.Settings.COMPort & " is closed !!!", ShowMsgImage.Info, "Error")
            labelInfoNeedSaveCOM.Visible = False
        End If

        cmddata.Visible = False
        PictureBoxPinOut.Visible = False
        PictureBoxPinOut.Location = New Point(9, 103)
        PictureBoxPinOut.Size = New Size(580, 255)

        txtoutput.BringToFront()
        txtoutput.Location = New Point(9, 103)
        txtoutput.Size = New Size(580, 255)

        rdowrvflash.Checked = True
        TextBoxHexaEditor.Visible = False

        ComboPort.Text = My.Settings.COMPort

        chktoggledtr.Checked = My.Settings.tdtr
        avrdude.getboardnames(cboboardname)

        If My.Settings.boardname <> Nothing Then
            cboboardname.Text = cboboardname.Items.Item(My.Settings.boardname)
        Else
            cboboardname.Text = cboboardname.Items.Item(16)
        End If

        avrdude.getboarddetails(cboboardname.Text, baud, mcu, protocol)
        If protocol = "stk500v2" Then
            chktoggledtr.Checked = True
        End If
        ComboBaudRate.Text = baud
        brdsetok = True

        txtfilename.Size = New Size(254, 20)
        txtfilename.Location = New Point(6, 38)
        CheckBoxUseUSBAsp.Checked = False
        lblinstallhw.Visible = False
        ButtonUSBAspUpload.Visible = False
        ButtonSerialUpload.Visible = True
        chktoggledtr.Visible = True
        ButtonBootLoader.Visible = False
        ButtonCmdWindow.Visible = False


        lblavrchip.Visible = False
        cmdreadfuse.Visible = False
        cboprecon.Visible = False
        lblEfuse.Visible = False
        lblHfuse.Visible = False
        lblLfuse.Visible = False
        txtefuse.Visible = False
        txthfuse.Visible = False
        txtlfuse.Visible = False
        cmdreadfuse.Visible = False
        cmdwritefuse.Visible = False
    End Sub

    Private Sub cboboardname_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboboardname.DropDownClosed
        If brdsetok = True Then
            avrdude.getboarddetails(cboboardname.Text, baud, mcu, protocol)
            If protocol = "stk500v2" Then
                chktoggledtr.Checked = True
            End If
            ComboBaudRate.Text = baud
        End If
    End Sub

    Private Sub cboboardname_SelectedValueChanged(sender As System.Object, e As System.EventArgs) Handles cboboardname.SelectedValueChanged
        Dim index As Integer
        index = cboboardname.FindString(cboboardname.Text)
        My.Settings.boardname = index
        My.Settings.Save()
    End Sub

    Private Sub bttnbrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bttnbrowse.Click
        ofd.Filter = "Intel hex file|*.hex"
        ofd.ShowDialog()
        txtfilename.Text = ofd.FileName
    End Sub

    Private Sub bttnupload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonSerialUpload.Click
        Dim result As Integer = MessageBox.Show("Do you want realy upload a new firmware ?", "oo OO Question OO oo", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            'MessageBox.Show("No pressed")
            Exit Sub
            'ElseIf result = DialogResult.Yes Then
            '    MessageBox.Show("Yes pressed")
        End If
        txtoutput.Text = ""
        txtoutput.Text &= "**************************************" & vbCrLf & _
                          "** Upload firmware is running ! **" & vbCrLf & _
                          "**************************************"

        If cboboardname.Text = "" Then
            MsgBox("Select a board.")
            Exit Sub
        End If
        If FileIO.FileSystem.FileExists(txtfilename.Text) = False Then
            MsgBox("Specified file does not exist.")
            Exit Sub
        End If
        Dim hex_to_write As String = ""
        ButtonSerialUpload.Text = "Please Wait..."
        'ButtonSerialUpload.Enabled = False
        If avrdude.folder_fix() = 0 Then
            MsgBox("Fatal error!!!")
            Exit Sub
        End If
        If avrdude.copy_file_to_temp(txtfilename.Text, hex_to_write) <> 1 Then
            MsgBox("Fatal Error in copying!!!")
            Exit Sub
        End If
        Threading.Thread.Sleep(100)
        Try
            If chktoggledtr.Checked = True Then
                SerialPort1.PortName = ComboPort.Text
                SerialPort1.BaudRate = ComboBaudRate.Text
                'serialport1.Parity = IO.Ports.Parity.None
                SerialPort1.Open()
                If chktoggledtr.Checked = True Then
                    SerialPort1.DtrEnable = True
                    SerialPort1.DtrEnable = False
                    SerialPort1.DtrEnable = True
                    SerialPort1.DtrEnable = False
                    SerialPort1.DtrEnable = True
                    SerialPort1.DtrEnable = False
                    SerialPort1.DtrEnable = True
                    SerialPort1.DtrEnable = False
                End If
                'serialport1.DtrEnable = False
                SerialPort1.Close()
            End If
            'MsgBox(hex_to_write)
            'MsgBox(mcu)

            avrdude.avrdude_command("avrdude", "-c " & protocol & " -P " & ComboPort.Text & " -b " & ComboBaudRate.Text & " -p " & mcu & " -U flash:w:" & hex_to_write & ":i", txtoutput.Text)
            avrdude.process_output_string(txtoutput.Text, 1)
            'Threading.Thread.Sleep(1000)
            'If avrdude.delete_temp_file() <> 1 Then
            '    MsgBox("Fatal Error in deleting!!!")
            'End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ButtonSerialUpload.Text = "Upload to arduino"
        'ButtonSerialUpload.Enabled = True
        'My.Settings.Save()
    End Sub

    Private Sub ButtonUSBAspUpload_Click(sender As System.Object, e As System.EventArgs) Handles ButtonUSBAspUpload.Click
        Dim result As Integer = MessageBox.Show("Do you want realy upload a new firmware ?", "oo OO Question OO oo", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then
            'MessageBox.Show("No pressed")
            Exit Sub
            'ElseIf result = DialogResult.Yes Then
            '    MessageBox.Show("Yes pressed")
        End If
        txtoutput.Text = ""
        txtoutput.Text &= "**************************************" & vbCrLf & _
                          "** Upload firmware is running ! **" & vbCrLf & _
                          "**************************************"

        If cboboardname.Text = "" Then
            MsgBox("Select a board.")
            Exit Sub
        End If
        If FileIO.FileSystem.FileExists(txtfilename.Text) = False Then
            MsgBox("Specified file does not exist.")
            Exit Sub
        End If
        Dim hex_to_write As String = ""
        ButtonUSBAspUpload.Text = "Please Wait..."
        ButtonUSBAspUpload.Enabled = False
        If avrdude.folder_fix() = 0 Then
            MsgBox("Fatal error!!!")
            Exit Sub
        End If
        If avrdude.copy_file_to_temp(txtfilename.Text, hex_to_write) <> 1 Then
            MsgBox("Fatal Error in copying!!!")
            Exit Sub
        End If
        Threading.Thread.Sleep(100)

        Try
            'avrdude -C avrdude.conf -v -patmega328p -cusbasp -Pusb -Uflash:w:Blink.ino.hex:i
            avrdude.avrdude_command("avrdude", "-C avrdude.conf -v -p" & mcu & " -cusbasp -Pusb -Uflash:w:" & hex_to_write & ":i", txtoutput.Text)
            avrdude.process_output_string(txtoutput.Text, 4)
            Threading.Thread.Sleep(1000)
            'If avrdude.delete_temp_file() <> 1 Then
            '    MsgBox("Fatal Error in deleting!!!")
            'End If
            'My.Settings.Save()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ButtonUSBAspUpload.Text = "Upload to arduino"
        ButtonUSBAspUpload.Enabled = True
    End Sub

    Private Sub ButtonBootLoader_Click(sender As System.Object, e As System.EventArgs) Handles ButtonBootLoader.Click
        If cboboardname.Text = "" Then
            MsgBox("Select a board.")
            Exit Sub
        End If
        If FileIO.FileSystem.FileExists(txtfilename.Text) = False Then
            MsgBox("Specified file does not exist.")
            Exit Sub
        End If
        Dim hex_to_write As String = ""
        ButtonUSBAspUpload.Text = "Please Wait..."
        ButtonUSBAspUpload.Enabled = False
        If avrdude.folder_fix() = 0 Then
            MsgBox("Fatal error!!!")
            Exit Sub
        End If
        If avrdude.copy_file_to_temp(txtfilename.Text, hex_to_write) <> 1 Then
            MsgBox("Fatal Error in copying!!!")
            Exit Sub
        End If
        Threading.Thread.Sleep(100)
        Try
            'avrdude -Cavrdude.conf -v -patmega328p -cusbasp -Pusb -Uflash:w:ATmegaBOOT_168_atmega328.hex:i -Ulock:w:0x0F:m 
            avrdude.avrdude_command("avrdude", "-C avrdude.conf -v -p" & mcu & " -cusbasp -Pusb -Uflash:w:" & hex_to_write & ":i -Ulock:w:0x0F:m", txtoutput.Text)
            avrdude.process_output_string(txtoutput.Text, 1)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        ButtonUSBAspUpload.Text = "Upload to arduino"
        ButtonUSBAspUpload.Enabled = True
        'My.Settings.Save()
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxUseUSBAsp.CheckedChanged
        If CheckBoxUseUSBAsp.Checked = True Then
            chktoggledtr.Visible = False
            lblinstallhw.Visible = True
            ButtonUSBAspUpload.Visible = True
            ButtonSerialUpload.Visible = False
            ButtonBootLoader.Visible = True
            ButtonBootLoader.Location = New Point(143, 66)
            ButtonCmdWindow.Visible = True
            lblavrchip.Visible = False
            cmdreadfuse.Visible = True
            cmdwritefuse.Visible = True
            txtfilename.Visible = False
            lblfileselect.Text = "Fuses:"
            bttnbrowse.Visible = False
            lblEfuse.Visible = True
            lblHfuse.Visible = True
            lblLfuse.Visible = True
            txtefuse.Visible = True
            txthfuse.Visible = True
            txtlfuse.Visible = True
            cmddata.Visible = True
            'CheckUSBAsp()

        Else
            lblinstallhw.Visible = False
            ButtonUSBAspUpload.Visible = False
            ButtonSerialUpload.Visible = True
            chktoggledtr.Visible = True
            ButtonBootLoader.Visible = False
            ButtonCmdWindow.Visible = False
            lblavrchip.Visible = False
            cmdreadfuse.Visible = False
            cmdwritefuse.Visible = False
            txtfilename.Visible = True
            lblfileselect.Text = "Select Hex file:"
            bttnbrowse.Visible = True
            lblEfuse.Visible = False
            lblHfuse.Visible = False
            lblLfuse.Visible = False
            txtefuse.Visible = False
            txthfuse.Visible = False
            txtlfuse.Visible = False
            cmddata.Visible = False
        End If
    End Sub

    Private Sub ButtonPinOutHelp_Click(sender As System.Object, e As System.EventArgs) Handles ButtonPinOutHelp.Click
        If PictureBoxPinOut.Visible = False Then
            PictureBoxPinOut.Visible = True
            txtoutput.Visible = False
            PictureBoxPinOut.Location = New Point(9, 99)
            PictureBoxPinOut.Size = New Size(580, 267)
        Else
            txtoutput.Visible = True
            PictureBoxPinOut.Visible = False
        End If

    End Sub

    Private Sub cboports_DropDown(ByVal sender As Object, ByVal e As System.EventArgs)
        update_port_enabled = False
    End Sub

    Private Sub cboports_DropDownClosed(ByVal sender As Object, ByVal e As System.EventArgs)
        update_port_enabled = True
    End Sub

    Private Sub CheckUSBAsp()

        'If usbasp_class._check_if_dev_available(usbasp_class.usbaspfinder) = True Then
        '    'MessageBox.Show(usbasp_class._check_usbasp_ext)
        '    MessageBox.Show("OK")
        'Else
        '    MessageBox.Show("Nothing")
        'End If
        'If usbasp_class._check_if_dev_available(usbasp_class.avrusbbootfinder) = True Then
        '    MessageBox.Show("Boot OK")
        'Else
        '    MessageBox.Show("Boot not OK")
        'End If
        If ShowInfo.CheckUsb() = True Then
            txtoutput.Text = ""
            txtoutput.Text &= "******************************************" & vbCrLf & _
                              "** USBAsp programmer is ready ! **" & vbCrLf & _
                              "******************************************"
        Else
            txtoutput.Text = ""
            txtoutput.Text &= "************************************************************" & vbCrLf & _
                              "** USBAsp programmer is not found !                    **" & vbCrLf & _
                              "** see http://zadig.akeo.ie/ if bad libusb drivers ! **" & vbCrLf & _
                              "************************************************************"

        End If


    End Sub

    Public global_chipname As String = ""
    Dim usbasp_class As New usbasp
    ''Dim avrdude As New libavrdude
    Public set_avr_fuses As Integer = 0
    'Dim clicktoopen As Boolean
    'Dim showmsg As Boolean

    Private Function get_chip_name() As String
        Dim outpstring As String = ""
        Dim chipname As String = ""

        'If usbasp_class._check_if_dev_available(usbasp_class.usbaspfinder) = True Then
        bitclock = " -B " & My.Settings.bitclock_slow
        Thread.Sleep(10)
        avrdude.avrdude_command("avrdude", bitclock & " -c usbasp -p m16", outpstring)
        Thread.Sleep(10)
        bitclock = " -B " & My.Settings.bitclock_fast
        Thread.Sleep(10)
        If avrdude.get_avr_signature() <> "" Then
            'MessageBox.Show(avrdude.get_avr_signature)
            chipname = avrdude.get_avr_chip_name(avrdude.get_avr_signature)
            If chipname = "Unknown" Then
                'MsgBox("Invalid chip signature...check connections")
                chipname = ""
                Return chipname
            End If
            bitclock = " -B " & My.Settings.bitclock_slow
            Thread.Sleep(10)
            avrdude.avrdude_command("avrdude", bitclock & " -c usbasp -p " & chipname, outpstring)
            Thread.Sleep(10)
            If My.Settings.bitclock_fast <> "0" Then
                bitclock = " -B " & My.Settings.bitclock_fast
            Else
                bitclock = ""
            End If
            Thread.Sleep(10)
            txtoutput.Text = outpstring
            txtoutput.Focus()
            txtoutput.SelectionStart = txtoutput.TextLength
        End If
        'End If
        Return chipname

    End Function

    'Private Sub ButtonReadFuses_Click(sender As System.Object, e As System.EventArgs) Handles ButtonReadFuses.Click

    '    'Private Sub RectangleShape1()
    '    Dim getavrchip As String = ""
    '    'RectangleShape1.Enabled = False
    '    lblavrchip.Enabled = False
    '    'frmloading.Show()
    '    If avrdude.get_avr_signature <> "-1" Then
    '        'MessageBox.Show(avrdude.get_avr_signature)
    '        getavrchip = get_chip_name()
    '        lblavrchip.Text = getavrchip
    '        'MessageBox.Show(getavrchip)
    '        If getavrchip = "" Or getavrchip = "Unknown" Or getavrchip = "-1" Then
    '            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
    '            'If chkenfnsnd.Checked Then avrdude.playsnd()
    '            'frmloading.Dispose()
    '            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
    '            MsgBox("No known chip connected")
    '        Else
    '            global_chipname = getavrchip
    '            set_avr_fuses = avrdude.getfuses(getavrchip)
    '            'set_fuse_boxes(set_avr_fuses)
    '            'If txtfilename.Text <> "" Then
    '            '    enable_control(1, 1, 1, 1, , 1, 1, 1, , , , 1, 1, 1, 1)
    '            'Else
    '            '    enable_control(1, 1, 1, 1, , , 1, , , , , 1, 1, 1, 1)
    '            'End If
    '            avrdude.load_fuse_predefs(global_chipname, cboprecon)
    '            'frmloading.Dispose()
    '            'MessageBox.Show("MCU is: " & global_chipname)

    '            'Dim board As String = ""
    '            'avrdude.getboarddetailsfrommcu(global_chipname, board)
    '            'MessageBox.Show("MCU is: " & global_chipname & vbCrLf & "Board: " & board)
    '            Dim out() As String = txtoutput.Text.Split(vbCrLf)
    '            For Each l In out
    '                If l.Contains("avrdude: safemode: Fuses OK (") Then
    '                    l = l.Replace("avrdude: safemode: Fuses OK (", "").Replace(")", "").Trim
    '                    txtefuse.Text = Mid(l, 3, 2)
    '                    txthfuse.Text = Mid(l, 9, 2)
    '                    txtlfuse.Text = Mid(l, 15, 2)
    '                    'MessageBox.Show("MCU is: " & global_chipname & vbCrLf & "E:" & efuse & vbCrLf & "H:" & hfuse & vbCrLf & "L:" & lfuse)
    '                End If
    '            Next

    '        End If
    '    Else
    '        'frmloading.Dispose()
    '        'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
    '        MsgBox("chip read error...either no chip is connected or the chip is nonresponsive")

    '        global_chipname = ""
    '        lblavrchip.Text = ""
    '        'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
    '    End If
    '    'Try
    '    '    'frmloading.Dispose()
    '    'Catch ex As Exception
    '    'End Try
    '    'RectangleShape1.Enabled = True
    '    lblavrchip.Enabled = True
    'End Sub

    Private Sub lblinstallhw_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblinstallhw.Click
        Dim msg As MsgBoxResult = MsgBox("Are you sure you want to install these drivers,if you previously had these drivers installed,they will be overwritten" & vbCrLf & "and that may cause problems.", vbYesNo + vbInformation)
        If msg = vbNo Then Exit Sub

        If File.Exists(Application.StartupPath & "\Hardware drivers\usbasp.inf") Then
            Dim driverfolderpath As String = Application.StartupPath & "\Hardware drivers"
            Using prc As Process = New Process
                With prc.StartInfo
                    .FileName = "rundll32"
                    .UseShellExecute = True
                    .Arguments = "libusb0.dll,usb_install_driver_np_rundll " & driverfolderpath & "\usbasp.inf"
                End With
                prc.Start()
                prc.WaitForExit()
                MsgBox("Wait for around a second.Then plug in you USBasp or if its already plugged in,unplug and then re-plug it")
            End Using
        Else
            MessageBox.Show("The file '\Hardware drivers\usbasp.inf' not exist !", "Info", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End If

    End Sub


    Private Sub cmdreadfuse_Click(sender As System.Object, e As System.EventArgs) Handles cmdreadfuse.Click
        'Dim txtlfuse As String = ""
        'Dim txtefuse As String = ""
        'Dim txthfuse As String = ""
        global_chipname = get_chip_name()
        'Thread.Sleep(100)
        avrdude.read_fuses(global_chipname, txtefuse.Text, txthfuse.Text, txtlfuse.Text)
        'Thread.Sleep(100)
        'MessageBox.Show(txtefuse & vbCrLf & txthfuse & vbCrLf & txtlfuse)
    End Sub


    'Private Sub cmdwritefuse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdwritefuse.Click
    '    Dim command_string As String = ""
    '    Dim outputstr As String = ""
    '    Dim fuse_string As String = ""
    '    If global_chipname = "" Then
    '        MsgBox("No chip selected")
    '        Exit Sub
    '    End If
    '    If avrdude.get_avr_signature = "-1" Then
    '        If My.Settings.enablefunnysound = True Then avrdude.playsnd()
    '        MsgBox("chip read error...either no chip is connected or the chip is nonresponsive")
    '        global_chipname = ""
    '        lblavrchip.Text = ""
    '        enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
    '        Exit Sub
    '    End If
    '    If set_avr_fuses = 111 Then
    '        avrdude.fixfuse(txtefuse)
    '        avrdude.fixfuse(txthfuse)
    '        avrdude.fixfuse(txtlfuse)
    '        If (txtefuse.Text = "" Or txthfuse.Text = "" Or txtlfuse.Text = "") Then
    '            MsgBox("Fill in all the fuse info")
    '            Exit Sub
    '        End If
    '        If avrdude.verify_fuse_byte(txtefuse.Text) <> 1 Or avrdude.verify_fuse_byte(txthfuse.Text) <> 1 Or avrdude.verify_fuse_byte(txtlfuse.Text) <> 1 Then
    '            MsgBox("Invalid fuse byte entered in one of the boxes.Fuse byte should be in the format 0xXX " & vbCrLf & "where XX is replaced by the fuse byte and since Hexadecimal number are bounded by 0-9 and A(a) to F(f)...you should be concerned about that too." & vbCrLf & "there should be no space before after or between the fuse bytes.If you pressed space bar,then press backspace in the box where you did that.")
    '            Exit Sub
    '        End If
    '        fuse_string = "-U efuse:w:" & txtefuse.Text & ":m -U hfuse:w:" & txthfuse.Text & ":m -U lfuse:w:" & txtlfuse.Text & ":m"
    '    ElseIf set_avr_fuses = 11 Then
    '        avrdude.fixfuse(txthfuse)
    '        avrdude.fixfuse(txtlfuse)
    '        If avrdude.verify_fuse_byte(txthfuse.Text) <> 1 Or avrdude.verify_fuse_byte(txtlfuse.Text) <> 1 Then
    '            MsgBox("Invalid fuse byte entered in one of the boxes.Fuse byte should be in the format 0xXX " & vbCrLf & "where XX is replaced by the fuse byte and since Hexadecimal number are bounded by 0-9 and A(a) to F(f)...you should be concerned about that too." & vbCrLf & "there should be no space before after or between the fuse bytes.If you pressed space bar,then press backspace in the box where you did that.")
    '            Exit Sub
    '        End If
    '        If (txtlfuse.Text = "" Or txthfuse.Text = "") Then
    '            MsgBox("Fill in all the fuse info")
    '            Exit Sub
    '        End If
    '        fuse_string = "-U hfuse:w:" & txthfuse.Text & ":m -U lfuse:w:" & txtlfuse.Text & ":m"
    '    End If
    '    txtoutput.Text = "Please Wait..."
    '    set_sck_option()
    '    enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
    '    command_string = " -c usbasp -p " & global_chipname & " " & fuse_string
    '    avrdude.avrdude_command("avrdude", bitclock & command_string, outputstr)
    '    txtoutput.Text = outputstr
    '    Thread.Sleep(100)
    '    show_programpic(False)
    '    avrdude.process_output_string(outputstr, 8, set_avr_fuses)

    '    enable_control(1, 1, 1, 1, , 1, 1, 1, , , , 1, 1, 1, 1)
    '    set_fuse_boxes(set_avr_fuses)

    'End Sub

    Public Sub set_sck_option()
        'If chksck.Checked Then
        bitclock = " -B " & My.Settings.bitclock_slow
        '    Thread.Sleep(5)
        'Else
        '    If My.Settings.bitclock_fast <> "0" Then
        '        bitclock = " -B " & My.Settings.bitclock_fast
        '    Else
        '        bitclock = ""
        '    End If
        '    Thread.Sleep(5)
        'End If
    End Sub

    'Public Sub set_fuse_boxes(ByVal fusesint As Integer)
    '    If fusesint = 111 Then
    '        enable_control(, , , , , , , , 1, 1, 1, , , , )
    '    ElseIf fusesint = 11 Then
    '        enable_control(, , , , , , , , 1, 1, 0, , , , )
    '    End If
    'End Sub

    Private Sub cmdwrite_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdwrite.Click
        Dim fldr_fix As Integer = 0
        Dim file_copy As Integer = 0
        Dim file_delete As Integer = 0
        Dim copied_filename As String = ""
        Dim command_string As String = ""
        Dim outputstr As String = ""
        Dim destination As String = ""
        If global_chipname = "" Then
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("No chip selected, use 'Read' before !")
            Exit Sub
        End If
        If avrdude.get_avr_signature = "-1" Then
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("chip read error...either no chip is connected or the chip is nonresponsive")
            global_chipname = ""
            lblavrchip.Text = ""
            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            Exit Sub
        End If
        If rdowrvflash.Checked Then
            destination = "flash"
        ElseIf rdowrveeprom.Checked Then
            destination = "eeprom"
        End If
        fldr_fix = avrdude.folder_fix()
        If fldr_fix <> 0 Then
            If txtfilename.Text <> "" Then
                file_copy = avrdude.copy_file_to_temp(txtfilename.Text, copied_filename)
                If file_copy <> 1 Then
                    'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
                    MsgBox("Error copying file")
                Else
                    'show_programpic(True)
                    txtoutput.Text = "Please Wait..."
                    set_sck_option()
                    'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    command_string = " -c " & programmer_hw & " -p " & global_chipname & " -U " & destination & ":w:" & copied_filename & ":i"
                    avrdude.avrdude_command("avrdude", bitclock & command_string, outputstr)
                    txtoutput.Text = outputstr
                    Thread.Sleep(100)
                    'show_programpic(False)
                    If destination = "flash" Then
                        avrdude.process_output_string(outputstr, 1)
                    Else
                        avrdude.process_output_string(outputstr, 2)
                    End If
                    'enable_control(1, 1, 1, 1, , 1, 1, 1, , , , 1, 1, 1, 1)
                    file_delete = avrdude.delete_temp_file
                    If file_delete <> 1 Then
                        'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
                        MsgBox("Fatal error")
                    End If
                End If
            Else
                'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
                MsgBox("Select file first")
            End If
        Else
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("Error creating folder")
        End If
        'set_fuse_boxes(set_avr_fuses)
    End Sub

    Private Sub cmdread_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdread.Click
        Dim fldr_fix As Integer = 0
        Dim file_copy As Integer = 0
        Dim file_delete As Integer = 0
        Dim destination_file As String = ""
        Dim command_string As String = ""
        Dim outputstr As String = ""
        Dim destination As String = ""
        Dim copyfile As Integer = 0
        Dim tempfilenm As String = ""

        If global_chipname = "" Then
            MsgBox("No chip selected, use 'Read' before !")
            Exit Sub
        End If
        If avrdude.get_avr_signature = "-1" Then
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("chip read error...either no chip is connected or the chip is nonresponsive")
            global_chipname = ""
            lblavrchip.Text = ""
            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            Exit Sub
        End If
        If rdowrvflash.Checked Then
            destination = "flash"
        ElseIf rdowrveeprom.Checked Then
            destination = "eeprom"
        End If
        fldr_fix = avrdude.folder_fix()
        If fldr_fix <> 0 Then
            svfd.Filter = "Hex files|*.hex|EEP files|*.eep"
            svfd.ShowDialog()
            destination_file = svfd.FileName
            If destination_file = "" Then
                MsgBox("You didn't enter Any File Name...So Read Operation Cannot Continue")
                Exit Sub
            End If
            tempfilenm = libavrdude.folderpath & "\readfile" & Rnd() & ".hex"
            'show_programpic(True)
            txtoutput.Text = "Please Wait..."
            'set_sck_option()
            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            command_string = " -c " & programmer_hw & " -p " & global_chipname & " -U " & destination & ":r:" & tempfilenm & ":i"
            avrdude.avrdude_command("avrdude", bitclock & command_string, outputstr)
            txtoutput.Text = outputstr
            Thread.Sleep(100)
            'show_programpic(False)
            If destination = "flash" Then
                avrdude.process_output_string(outputstr, 3)
            Else
                avrdude.process_output_string(outputstr, 7)
            End If
            enable_control(1, 1, 1, 1, , , 1, , , , , 1, 1, 1, 1)
            Try
                FileIO.FileSystem.MoveFile(tempfilenm, destination_file, True)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try


        Else
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("Error creating folder")
        End If
        'set_fuse_boxes(set_avr_fuses)
    End Sub

    Private Sub cmdverify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdverify.Click
        Dim fldr_fix As Integer = 0
        Dim file_copy As Integer = 0
        Dim file_delete As Integer = 0
        Dim copied_filename As String = ""
        Dim command_string As String = ""
        Dim outputstr As String = ""
        Dim destination As String = ""
        If global_chipname = "" Then
            MsgBox("No chip selected, use 'Read' before !")
            Exit Sub
        End If
        If avrdude.get_avr_signature = "-1" Then
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("chip read error...either no chip is connected or the chip is nonresponsive")
            global_chipname = ""
            lblavrchip.Text = ""
            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            Exit Sub
        End If
        If rdowrvflash.Checked Then
            destination = "flash"
        ElseIf rdowrveeprom.Checked Then
            destination = "eeprom"
        End If
        fldr_fix = avrdude.folder_fix()
        If fldr_fix <> 0 Then
            If txtfilename.Text <> "" Then
                file_copy = avrdude.copy_file_to_temp(txtfilename.Text, copied_filename)
                If file_copy <> 1 Then
                    'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
                    MsgBox("Error copying file")
                Else
                    'show_programpic(True)
                    txtoutput.Text = "Please Wait..."
                    'set_sck_option()
                    enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
                    command_string = " -c " & programmer_hw & " -p " & global_chipname & " -U " & destination & ":v:" & copied_filename & ":i"
                    avrdude.avrdude_command("avrdude", bitclock & command_string, outputstr)
                    txtoutput.Text = outputstr
                    Thread.Sleep(100)
                    'show_programpic(False)
                    If destination = "flash" Then
                        avrdude.process_output_string(outputstr, 4)
                    Else
                        avrdude.process_output_string(outputstr, 6)
                    End If

                    'enable_control(1, 1, 1, 1, , 1, 1, 1, , , , 1, 1, 1, 1)
                    file_delete = avrdude.delete_temp_file
                    If file_delete <> 1 Then
                        'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
                        MsgBox("Fatal error")
                    End If
                End If
            Else
                'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
                MsgBox("Select file first")
            End If
        Else
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("Error creating folder")
        End If
        'set_fuse_boxes(set_avr_fuses)
    End Sub

    Private Sub cmderasechip_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmderasechip.Click
        Dim command_string As String = ""
        Dim outputstr As String = ""
        Dim msg As MsgBoxResult
        'global_chipname = Form1.global_chipname
        If global_chipname = "" Then
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("No chip selected, use 'Read' before !")
            Exit Sub
        End If
        'msg = MsgBox("Are you sure you want to erase the entire chip memory?This operation cannot be reversed", vbYesNo + vbCritical, "WARNING!!!ERASING OPERATION")
        msg = ShowMsg("Are you sure you want to erase the entire chip memory?" & vbCrLf & vbCrLf & "This operation cannot be reversed", "!!", ShowMsgDefaultButton.Button3, ShowMsgDefaultButton.Button1, "!!! WARNING !!!     ERASING OPERATION")

        'MessageBox.Show(msg.ToString)
        'If msg = vbNo Then Exit Sub
        If msg = "No" Then Exit Sub
        If avrdude.get_avr_signature = "-1" Then
            'If My.Settings.enablefunnysound = True Then avrdude.playsnd()
            MsgBox("chip read error...either no chip is connected or the chip is nonresponsive")
            global_chipname = ""
            lblavrchip.Text = ""
            'enable_control(0, 0, 0, 0, , 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)
            Exit Sub
        End If
        'delete_animation(True)
        bitclock = " -B " & My.Settings.bitclock_slow
        Thread.Sleep(10)
        command_string = " -c " & programmer_hw & " -p " & global_chipname & " -e"
        avrdude.avrdude_command("avrdude", bitclock & command_string, outputstr)
        txtoutput.Text = outputstr
        If My.Settings.bitclock_fast <> "0" Then
            bitclock = " -B " & My.Settings.bitclock_fast
        Else
            bitclock = ""
        End If
        'delete_animation(False)
        avrdude.process_output_string(outputstr, 5)
    End Sub

    Private Sub cmddataviewer_Click(sender As System.Object, e As System.EventArgs) Handles cmddataviewer.Click
        If global_chipname <> "" Then
            Dim msg As MsgBoxResult = MsgBox("Do you want to load the interface for the Microcontroller detected?" & vbCrLf & "Pressing No will provide you with a standalone interface independent of the microcontroller detected" & vbCrLf & "with an option to choose a microcontroller for your own work", vbInformation + vbYesNo, "Flash/EEPROM viewer/editor")
            If msg = vbYes Then
                Dim flash_mem As String = ""
                Dim eeprom_mem As String = ""
                Dim flash_mem_int As Integer = 0
                Dim eeprom_mem_int As Integer = 0
                Dim i As Integer = 0
                Dim value_of_row_header As Integer = 0
                dataeditor.lblmcu.Text = global_chipname
                flash_mem = avrdude.get_memory_details(global_chipname, "flash")
                eeprom_mem = avrdude.get_memory_details(global_chipname, "eeprom")
                dataeditor.lblflash.Text = flash_mem
                dataeditor.lbleeprom.Text = eeprom_mem
                eeprom_mem_int = Int(eeprom_mem)
                dataeditor.cbochiplist.Text = global_chipname
                For flash_mem_int = 1 To (eeprom_mem_int / 8)
                    dataeditor.DataGridView1.Rows.Add()
                    'dataeditor.DataGridView1.Rows.Item(flash_mem_int - 1).HeaderCell.Style.Alignment =DataGridViewContentAlignment.MiddleCenter
                    For i = 0 To 7
                        dataeditor.DataGridView1.Item(i, flash_mem_int - 1).Value = "FF"
                    Next
                    dataeditor.DataGridView1.Rows.Item(flash_mem_int - 1).HeaderCell.Value = Hex((flash_mem_int - 1) * 8).ToString
                Next
                'MsgBox(dataeditor.DataGridView1.RowCount)
            End If
        End If
        dataeditor.Show()
    End Sub

    Private Sub ButtonReadHexaEditor_Click(sender As System.Object, e As System.EventArgs) Handles ButtonReadHexaEditor.Click
        If TextBoxHexaEditor.Visible = False Then
            TextBoxHexaEditor.Visible = True
            txtoutput.Visible = False
            TextBoxHexaEditor.Location = New Point(9, 210)
            TextBoxHexaEditor.Size = New Size(580, 153)

            Dim ArrayHold() As Byte
            Dim Index As Integer = 0
            Dim Str As New StringBuilder
            Dim tStr As String = ""
            Dim tempStr As String = ""
            Dim IndexEnd As Integer = 0
            Dim InputString As String = ""

            fd.Filter = "All Files|*.*"
            If fd.ShowDialog = Windows.Forms.DialogResult.OK Then
                ' ArrayHold = My.Computer.FileSystem.ReadAllBytes(OpenDia.FileName)
                '==============================================================
                Dim myStreamReader As StreamReader = Nothing

                ' Ensure that the creation of the new StreamReader is wrapped in a 
                '   Try-Catch block, since an invalid filename could have been used.
                ' Create a StreamReader using a Shared (static) File class.
                myStreamReader = File.OpenText(fd.FileName)
                ' Read the entire file in one pass, and add the contents to 
                '   txtFileText text box.
                InputString = myStreamReader.ReadToEnd()
                'Convert string to byte and copy to byte array
                ArrayHold = Encoding.Default.GetBytes(InputString)

                '=================================================================
                'ArrayHold = FileSystem.ReadAllBytes(OpenDia.FileName)
                Do
                    IndexEnd = Index + 9
                    For x As Integer = Index To IndexEnd
                        If x > UBound(ArrayHold) Then
                            Str.Append("    ")
                            tempStr = tempStr & "  "
                        Else
                            tStr = UCase(Convert.ToString(ArrayHold(x), 16))
                            If tStr.Length < 2 Then tStr = "0" & tStr
                            Str.Append(tStr & "  ")
                            If ArrayHold(x) < 32 Then
                                tempStr = tempStr & ". "
                            Else
                                tempStr = tempStr & Chr(ArrayHold(x)) & " "
                            End If
                        End If
                    Next
                    Str.Append("    |    " & tempStr & vbCrLf)
                    tempStr = ""
                    Index = Index + 10
                Loop While IndexEnd < UBound(ArrayHold)
                TextBoxHexaEditor.Text = Str.ToString
            End If
        Else
            txtoutput.Visible = True
            TextBoxHexaEditor.Visible = False
        End If

    End Sub

#End Region

#Region "Gaming Hidden"
    Private Sub form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        Select Case e.KeyData
            Case (Keys.Control + Keys.Alt + Keys.G)
                'MsgBox("Control + Alt + G")
                If TabControl1.TabPages.Contains(TabPage8) = False Then
                    Me.TabControl1.TabPages.Add(TabPage8)
                    'TabControl1.TabPages.Insert(7, TabPage8) ' where 7 is the position
                Else
                    Me.TabControl1.TabPages.Remove(TabPage8)
                End If


            Case (Keys.Escape)
                Me.Close()
        End Select
    End Sub

    Private Sub ButtonPlayGame_Click(sender As System.Object, e As System.EventArgs) Handles ButtonPlayGame.Click
        frmGdiGaming.Show()
    End Sub

#End Region

#Region "Timers"

    Private Sub TimerProgressBars_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerProgressBars.Tick
        'convertion de Pulse (600,2400) en (valeur bar graph (0,100)
        NewPulse = mapValue(pulseValue,
                                    CInt(textMiniGenerale.Text), CInt(textMaxiGenerale.Text),
                                    Me.ProgressBarThrottleMotors._Mini, Me.ProgressBarThrottleMotors._Maxi)
        If NewPulse > Me.ProgressBarThrottleMotors._Maxi Then
            Me.ProgressBarThrottleMotors._Value = Me.ProgressBarThrottleMotors._Maxi
        ElseIf NewPulse < Me.ProgressBarThrottleMotors._Mini Then
            Me.ProgressBarThrottleMotors._Value = Me.ProgressBarThrottleMotors._Mini
        End If
        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText("NewPulse: " & NewPulse & vbCrLf)
        End If
        Me.ProgressBarThrottleMotors._Value = NewPulse



        'convertion de Pulse (600,2400) en (valeur bar graph (0,100)
        Dim NewPulseAux As Integer = mapValue(pulseValueAux,
                                    CInt(textMiniGenerale.Text), CInt(textMaxiGenerale.Text),
                                    Me.ProgressBarThrottleAuxiliary._Mini, Me.ProgressBarThrottleAuxiliary._Maxi)

        If NewPulseAux > Me.ProgressBarThrottleAuxiliary._Maxi Then
            Me.ProgressBarThrottleAuxiliary._Value = Me.ProgressBarThrottleAuxiliary._Maxi
        ElseIf NewPulse < Me.ProgressBarThrottleAuxiliary._Mini Then
            Me.ProgressBarThrottleAuxiliary._Value = Me.ProgressBarThrottleAuxiliary._Mini
        End If

        If term.Visible = True Then
            term.TextBoxTerminalComPort.AppendText("NewPulseAux: " & NewPulseAux & vbCrLf)
        End If
        Me.ProgressBarThrottleAuxiliary._Value = NewPulseAux

    End Sub



    Private Sub TimerMotors_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerRXMotors.Tick
        SerialPort1.Write("400" & vbCr)
    End Sub

    Private Sub TimerSpeeds_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerSpeeds.Tick
        SerialPort1.Write("401" & vbCr)
    End Sub

    Private Sub TimerAuxiliaire_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerRXAuxiliaire.Tick
        SerialPort1.Write("402" & vbCr)
    End Sub

    Private Sub TimerHardware_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerHardwareInfos.Tick
        SerialPort1.Write("403" & vbCr)
    End Sub

    Private Sub TimerChrono_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerChrono.Tick
        If My.Settings.Language = "French" Then
            LabelChrono.Text = "Heure Finale: " & cntDown.ToString("H:mm:ss")
            LabelTestNow.Text = "Heure Actuelle: " & Date.Now.ToString("H:mm:ss")
        ElseIf My.Settings.Language = "English" Then
            LabelChrono.Text = "Final Time: " & cntDown.ToString("H:mm:ss")
            LabelTestNow.Text = "Actual Time: " & Date.Now.ToString("H:mm:ss")
        End If
        If cntDown < Date.Now Then
            If BackgroundWorker1.WorkerSupportsCancellation Then
                BackgroundWorker1.CancelAsync()
            End If
            PlayerIsOn = False
            TimerChrono.Enabled = False
            If My.Settings.Language = "French" Then
                ButtonPlayer.Text = "Départ Lecture"
            ElseIf My.Settings.Language = "English" Then
                ButtonPlayer.Text = "Start Player"
            End If
            PictureBoxPlayer.Image = My.Resources.rectangle_rouge
            LabelChrono.ForeColor = Color.Blue
            LabelTestNow.ForeColor = Color.Blue
            LabelTestNow.Text = "Actual Time: " & cntDown.ToString("H:mm:ss")
            'LabelChrono.Visible = False
            'LabelTestNow.Visible = False
            'MessageBox.Show("Countdown a terminé!")
        Else
            LabelChrono.Visible = True
            LabelTestNow.Visible = True
            Dim timeRem As TimeSpan = Me.cntDown.Subtract(Date.Now)
            TextBoxChronoHH.Text = timeRem.Hours.ToString
            TextBoxChronoMM.Text = timeRem.Minutes.ToString
            LabelChronoSS.Text = timeRem.Seconds.ToString
        End If
    End Sub

    Private Sub TimerDataLogger_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerDataLogger.Tick
        zg1.IsAutoScrollRange = True
        ' Tell ZedGraph to calculate the axis ranges
        ' Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
        ' up the proper scrolling parameters
        zg1.AxisChange()
        'zg1.Refresh()
        ' Make sure the Graph gets redrawn
        zg1.Invalidate()
    End Sub

#End Region

#Region "Languages"
    Public Sub getRMValue()
        If My.Settings.Language = "French" Then
            If (CheckBoxInversionServo1.Checked = True) Then CheckBoxInversionServo1.Text = "Oui" Else CheckBoxInversionServo1.Text = "Non"
            If (CheckBoxInversionServo2.Checked = True) Then CheckBoxInversionServo2.Text = "Oui" Else CheckBoxInversionServo2.Text = "Non"
            optLangFR.Checked = True
        ElseIf My.Settings.Language = "English" Then
            If (CheckBoxInversionServo1.Checked = True) Then CheckBoxInversionServo1.Text = "Yes" Else CheckBoxInversionServo1.Text = "No"
            If (CheckBoxInversionServo2.Checked = True) Then CheckBoxInversionServo2.Text = "Yes" Else CheckBoxInversionServo2.Text = "No"
            optLangEN.Checked = True
        End If

        LabelInterType.Text = ModeAuxiliaireTypeText(Convert.ToInt16(textAuxiliaireMode.Text))
    End Sub

    Private Sub SetFrench()
        optLangFR.Text = My.Resources.optLangFR_FR
        optLangEN.Text = My.Resources.optLangEN_FR
        My.Settings.Language = "French"
        GroupBoxSerialPort.Text = My.Resources.groupBoxComPort_FR
        Button_Connect.Text = My.Resources.buttonConnect_FR
        ButtonSauvegardeCOM.Text = My.Resources.buttonSaveComPort_FR
        'ButtonSettings.Text = My.Resources.buttonConfig_FR
        labelMotorsSimulation.Text = My.Resources.labelMotorsSimu_FR
        labelAuxRudderSimulation.Text = My.Resources.labelAuxiRudderSimu_FR
        ButtonGlowPlugOnOff.Text = My.Resources.buttonGlowOnOff_FR
        ButtonAuxMiddle.Text = My.Resources.buttonAuxCenter_FR
        '
        GroupBoxSettings.Text = My.Resources.groupBoxSettings_FR
        labelConfigModule.Text = My.Resources.labelConfigModule_FR
        labelConfigModule.Location = New Point(148, 5)
        ButtonReadCenter1.Text = My.Resources.buttonRead_FR
        ButtonReadCenter2.Text = My.Resources.buttonRead_FR
        ButtonIdleMoteur1.Text = My.Resources.buttonRead_FR
        ButtonIdleMoteur2.Text = My.Resources.buttonRead_FR
        ButtonMaxiMoteurs.Text = My.Resources.buttonRead_FR
        ButtonDebutSynchro.Text = My.Resources.buttonRead_FR
        ButtonMiniMaxGeneral.Text = My.Resources.buttonRead_FR
        ButtonReadAuxiliairePulse.Text = My.Resources.buttonRead_FR
        ButtonReadTempVoltage.Text = My.Resources.buttonRead_FR
        ButtonModuleType.Text = My.Resources.buttonModuleType_FR
        '
        labelCenterServo1.Text = My.Resources.labelCenterServo1_FR & " µS"
        labelCenterServo2.Text = My.Resources.labelCenterServo2_FR & " µS"
        If (CheckBoxInversionServo1.Checked = True) Then CheckBoxInversionServo1.Text = "Oui" Else CheckBoxInversionServo1.Text = "Non"
        If (CheckBoxInversionServo2.Checked = True) Then CheckBoxInversionServo2.Text = "Oui" Else CheckBoxInversionServo2.Text = "Non"
        If (CheckBoxInversionAux.Checked = True) Then CheckBoxInversionAux.Text = "Oui" Else CheckBoxInversionAux.Text = "Non"
        labelIdleServo1.Text = My.Resources.labelIdleServo1_FR & " µS"
        labelIdleServo2.Text = My.Resources.labelIdleServo2_FR & " µS"
        labelSpeedModuleAnswer.Text = My.Resources.labelSpeedModuleAnswer_FR
        labelStartSynchroServos.Text = My.Resources.labelStartSynchroMotors_FR & " µS"
        labelPosMiniGene.Text = My.Resources.labelGeneralePositionMini_FR & " µS"
        labelPosMaxiGene.Text = My.Resources.labelGeneralePositionMaxi_FR & " µS"
        labelAuxiPosition.Text = My.Resources.labelAuxiPosition_FR & " µS"
        labelReverseServo1.Text = My.Resources.labelReverseServo1_FR
        labelReverseServo2.Text = My.Resources.labelReverseServo2_FR
        labelReverseAuxi.Text = My.Resources.labelReverseAuxi_FR
        labelMode.Text = My.Resources.labelRadioMode_FR
        labelNbrBlades.Text = My.Resources.labelNbrBlades_FR
        labelSpeedMinMaxRPM.Text = My.Resources.labelSpeedMotorMinMaxRPM_FR
        ButtonAnnulerModif.Text = My.Resources.buttonAbortChanges_FR
        ButtonConfigDefaut.Text = My.Resources.buttonResetSettings_FR
        ButtonSauvegardeConfig.Text = My.Resources.buttonSaveSettings_FR
        'ButtonMoteurs.Text = My.Resources.buttonMotors_FR & "s >>"
        btnSend.Text = My.Resources.buttonDebugSend_FR
        GroupBoxMoteurs.Text = My.Resources.buttonMotors_FR & "s"
        labelMotor1.Text = My.Resources.buttonMotors_FR & " 1"
        labelMotor2.Text = My.Resources.buttonMotors_FR & " 2"
        'LabelVitesse1.Text = "Vitesse 1:"
        'LabelVitesse2.Text = "Vitesse 2:"
        ButtonReadSpeeds.Text = "Vitesse"
        GroupSpeedSimu.Text = "Simulation Vitesse"
        ButtonSpeedSimuOn.Text = "Simu Vitesse"
        CheckBoxSimuSynchroSpeeds.Text = "Synchroniser Vitesses"
        buttonEraseEEprom.Text = "Effacer EEprom"
        buttonResetArduino.Text = "Reset Module"
        ButtonRecorder.Text = "Enregistrement"
        ButtonPlayer.Text = "Lecture"
        LabelOnSimuMoveThrottle.Text = "Déplacez manche moteurs !"
        If CheckBoxChronoOnOff.Checked = True Then
            CheckBoxChronoOnOff.Text = "Oui"
        Else
            CheckBoxChronoOnOff.Text = "Non"
        End If

        TabPage1.Text = "Connexion"
        TabPage2.Text = "Configuration"
        TabPage3.Text = "Moteurs"
        TabPage4.Text = "Graphique Vitesses"
        TabPage5.Text = "Simulation"
        TabPage6.Text = "FRAM"
        TabPage7.Text = "Programmation"
        TabPage8.Text = "Jeu"

        GroupBoxFRAMInfos.Text = "Infos FRAM"
        GroupBoxSDListFiles.Text = "FRAM"

        labelInternalVoltage.Text = My.Resources.labelIntVoltage_FR
        labelMode.Text = My.Resources.labelMode_FR
    End Sub

    Private Sub SetEnglish()
        optLangFR.Text = My.Resources.optLangFR_EN
        optLangEN.Text = My.Resources.optLangEN_EN
        My.Settings.Language = "English"
        GroupBoxSerialPort.Text = My.Resources.groupBoxComPort_EN
        Button_Connect.Text = My.Resources.buttonConnect_EN
        ButtonSauvegardeCOM.Text = My.Resources.buttonSaveComPort_EN
        'ButtonSettings.Text = My.Resources.buttonConfig_EN
        labelMotorsSimulation.Text = My.Resources.labelMotorsSimu_EN
        labelAuxRudderSimulation.Text = My.Resources.labelAuxiRudderSimu_EN
        ButtonGlowPlugOnOff.Text = My.Resources.buttonGlowOnOff_EN
        ButtonAuxMiddle.Text = My.Resources.buttonAuxCenter_EN
        '
        GroupBoxSettings.Text = My.Resources.groupBoxSettings_EN
        labelConfigModule.Text = My.Resources.labelConfigModule_EN
        labelConfigModule.Location = New Point(200, 5)
        ButtonReadCenter1.Text = My.Resources.buttonRead_EN
        ButtonReadCenter2.Text = My.Resources.buttonRead_EN
        ButtonIdleMoteur1.Text = My.Resources.buttonRead_EN
        ButtonIdleMoteur2.Text = My.Resources.buttonRead_EN
        ButtonMaxiMoteurs.Text = My.Resources.buttonRead_EN
        ButtonDebutSynchro.Text = My.Resources.buttonRead_EN
        ButtonMiniMaxGeneral.Text = My.Resources.buttonRead_EN
        ButtonReadAuxiliairePulse.Text = My.Resources.buttonRead_EN
        ButtonReadTempVoltage.Text = My.Resources.buttonRead_EN
        ButtonModuleType.Text = My.Resources.buttonModuleType_EN
        '
        labelCenterServo1.Text = My.Resources.labelCenterServo1_EN & " µS"
        labelCenterServo2.Text = My.Resources.labelCenterServo2_EN & " µS"
        If (CheckBoxInversionServo1.Checked = True) Then CheckBoxInversionServo1.Text = "Yes" Else CheckBoxInversionServo1.Text = "No"
        If (CheckBoxInversionServo2.Checked = True) Then CheckBoxInversionServo2.Text = "Yes" Else CheckBoxInversionServo2.Text = "No"
        If (CheckBoxInversionAux.Checked = True) Then CheckBoxInversionAux.Text = "Yes" Else CheckBoxInversionAux.Text = "No"
        labelIdleServo1.Text = My.Resources.labelIdleServo1_EN & " µS"
        labelIdleServo2.Text = My.Resources.labelIdleServo2_EN & " µS"
        labelSpeedModuleAnswer.Text = My.Resources.labelSpeedModuleAnswer_EN
        labelStartSynchroServos.Text = My.Resources.labelStartSynchroMotors_EN & " µS"
        labelPosMiniGene.Text = My.Resources.labelGeneralePositionMini_EN & " µS"
        labelPosMaxiGene.Text = My.Resources.labelGeneralePositionMaxi_EN & " µS"
        labelAuxiPosition.Text = My.Resources.labelAuxiPosition_EN & " µS"
        labelReverseServo1.Text = My.Resources.labelReverseServo1_EN
        labelReverseServo2.Text = My.Resources.labelReverseServo2_EN
        labelReverseAuxi.Text = My.Resources.labelReverseAuxi_EN
        labelMode.Text = My.Resources.labelRadioMode_EN
        labelNbrBlades.Text = My.Resources.labelNbrBlades_EN
        labelSpeedMinMaxRPM.Text = My.Resources.labelSpeedMotorMinMaxRPM_EN
        ButtonAnnulerModif.Text = My.Resources.buttonAbortChanges_EN
        ButtonConfigDefaut.Text = My.Resources.buttonResetSettings_EN
        ButtonSauvegardeConfig.Text = My.Resources.buttonSaveSettings_EN
        'ButtonMoteurs.Text = My.Resources.buttonMotors_EN & "s >>"
        btnSend.Text = My.Resources.buttonDebugSend_EN
        GroupBoxMoteurs.Text = My.Resources.buttonMotors_EN & "s"
        labelMotor1.Text = My.Resources.buttonMotors_EN & " 1"
        labelMotor2.Text = My.Resources.buttonMotors_EN & " 2"
        'LabelVitesse1.Text = "Speed 1:"
        'LabelVitesse2.Text = "Speed 2:"
        ButtonReadSpeeds.Text = "Speed"
        GroupSpeedSimu.Text = "Speed Simulation"
        ButtonSpeedSimuOn.Text = "Speed Simu"
        CheckBoxSimuSynchroSpeeds.Text = "Synchronize Speeds"
        buttonEraseEEprom.Text = "Erase EEprom"
        buttonResetArduino.Text = "Restart Module"
        ButtonRecorder.Text = "Start Recorder"
        ButtonPlayer.Text = "Start Player"
        LabelOnSimuMoveThrottle.Text = "Move motor's throttle !"
        If CheckBoxChronoOnOff.Checked = True Then
            CheckBoxChronoOnOff.Text = "Yes"
        Else
            CheckBoxChronoOnOff.Text = "No"
        End If

        TabPage1.Text = "Connection"
        TabPage2.Text = "Settings"
        TabPage3.Text = "Moteurs"
        TabPage4.Text = "Data Logger"
        TabPage5.Text = "Simulation"
        TabPage6.Text = "FRAM"
        TabPage7.Text = "Programmer"
        TabPage8.Text = "Game"

        GroupBoxFRAMInfos.Text = "FRAM Infos"
        GroupBoxSDListFiles.Text = "FRAM"

        labelInternalVoltage.Text = My.Resources.labelIntVoltage_EN
        labelMode.Text = My.Resources.labelMode_EN
    End Sub

    Private Sub optLangFR_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optLangFR.CheckedChanged
        If optLangFR.Checked Then
            SetFrench()
            SetLanguages()
        End If
        My.Settings.Save()
        LabelInterType.Text = ModeAuxiliaireTypeText(Convert.ToInt16(textAuxiliaireMode.Text))
    End Sub

    Private Sub optLangEN_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles optLangEN.CheckedChanged
        If optLangEN.Checked Then
            SetEnglish()
            SetLanguages()
        End If
        My.Settings.Save()
        LabelInterType.Text = ModeAuxiliaireTypeText(Convert.ToInt16(textAuxiliaireMode.Text))
    End Sub

    Private Sub CheckBoxChronoOnOff_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxChronoOnOff.CheckedChanged
        'If My.Settings.Language = "French" Then

        'ElseIf My.Settings.Language = "English" Then

        'End If
    End Sub

    Private Sub CheckBoxReadTracBarMotorOrMotorThrottle_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxReadTracBarMotorOrMotorThrottle.CheckedChanged
        If CheckBoxReadTracBarMotorOrMotorThrottle.Checked = True Then
            If SerialPort1.IsOpen = True Then
                SerialPort1.Write("704" & vbCr)
            Else
                ShowMsg("Please,connect to the module!", ShowMsgImage.Info, "Error")
            End If

            PictureBoxSimuThrottle.Image = My.Resources.joystick
            LabelOnSimuMoveThrottle.Visible = True
            labelMotorsSimulation.Visible = False
            TrackBarMotors.Visible = False
            ProgressBarThrottle.Visible = False
            LabelMotors.Visible = False

            If My.Settings.Language = "French" Then
                CheckBoxReadTracBarMotorOrMotorThrottle.Text = "Lit 'Manche Moteurs'"
            Else
                CheckBoxReadTracBarMotorOrMotorThrottle.Text = "Read 'Motors Throttle'"
            End If
        ElseIf CheckBoxReadTracBarMotorOrMotorThrottle.Checked = False Then

            PictureBoxSimuThrottle.Image = My.Resources.TrackBar
            LabelOnSimuMoveThrottle.Visible = False
            labelMotorsSimulation.Visible = True
            TrackBarMotors.Visible = True
            ProgressBarThrottle.Visible = True
            LabelMotors.Visible = True

            If My.Settings.Language = "French" Then
                CheckBoxReadTracBarMotorOrMotorThrottle.Text = "Lit '" & My.Resources.labelMotorsSimu_FR & "'"
            Else
                CheckBoxReadTracBarMotorOrMotorThrottle.Text = "Read '" & My.Resources.labelMotorsSimu_EN & "'"
            End If
        End If
    End Sub

#End Region

    Private Sub MakeBold(txtBox As RichTextBox, text As String)
        'Format the output.
        Dim loc As Integer
        loc = InStr(1, txtBox.Text, text, CompareMethod.Text)

        txtBox.SelectionStart = loc - 1
        txtBox.SelectionLength = text.Length
        txtBox.SelectionFont = New System.Drawing.Font(txtBox.Font, FontStyle.Bold)
        'Move the caret back to start.
        txtBox.Select(0, 0)
    End Sub

    Private Function mapValue(x As Long, in_min As Long, in_max As Long, out_min As Long, out_max As Long) As Long
        Return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min
    End Function

    Private Sub ButtonCmdWindow_Click(sender As System.Object, e As System.EventArgs) Handles ButtonCmdWindow.Click
        Shell("cmd.exe", AppWinStyle.NormalFocus)
    End Sub


    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        ' by making Generator static, we preserve the same instance '
        ' (i.e., do not create new instances with the same seed over and over) '
        ' between calls '
        Static Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    'Private Sub cboprecon_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboprecon.SelectedIndexChanged

    '    If global_chipname = "" Then Exit Sub
    '    Dim fusearray As String = ""
    '    Dim fusecount As Integer = avrdude.getfuses(global_chipname)
    '    avrdude.read_fuse_predefs(global_chipname, cboprecon.SelectedIndex, fusearray)
    '    'txtefuse.Text = ""
    '    'txthfuse.Text = ""
    '    'txtlfuse.Text = ""
    '    'If fusecount = 111 Then
    '    '    txtefuse.Text = Mid(fusearray, InStrRev(fusearray, ",") + 1, 4)
    '    '    txtlfuse.Text = Mid(fusearray, 1, 4)
    '    '    txthfuse.Text = Mid(fusearray, InStr(fusearray, ",") + 1, 4)
    '    'ElseIf fusecount = 11 Then
    '    '    txthfuse.Text = Mid(fusearray, 1, 4)
    '    '    txtlfuse.Text = Mid(fusearray, InStr(fusearray, ",") + 1, 4)
    '    'End If
    'End Sub


    Private Sub cmddata_Click(sender As System.Object, e As System.EventArgs) Handles cmddata.Click

        txtoutput.BringToFront()

        If txtoutput.Location.Y = 209 Then
            txtoutput.Location = New Point(9, 103)
            txtoutput.Size = New Size(580, 255)
        Else
            txtoutput.Location = New Point(9, 209)
            txtoutput.Size = New Size(580, 145)
        End If
        'Data.Show()
    End Sub



    Private Sub BackgroundWorkerThrottle_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BackgroundWorkerThrottle.DoWork
        While TimerRXMotors.Enabled = True
            Application.DoEvents()
            'convertion de Pulse (600,2400) en (valeur bar graph (0,100)
            NewPulse = mapValue(pulseValue,
                                        CInt(textMiniGenerale.Text), CInt(textMaxiGenerale.Text),
                                        Me.ProgressBarThrottleMotors._Mini, Me.ProgressBarThrottleMotors._Maxi)

            If NewPulse > Me.ProgressBarThrottleMotors._Maxi Then
                Me.ProgressBarThrottleMotors._Value = Me.ProgressBarThrottleMotors._Maxi
            ElseIf NewPulse < Me.ProgressBarThrottleMotors._Mini Then
                Me.ProgressBarThrottleMotors._Value = Me.ProgressBarThrottleMotors._Mini
            End If

            'If term.Visible = True Then
            '    term.TextBoxTerminalComPort.AppendText("NewPulse: " & NewPulse & vbCrLf)
            'End If

            Me.ProgressBarThrottleMotors._Value = NewPulse
            labelM.Text = NewPulse
        End While
    End Sub

    'Private Sub bw_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
    '    'Me.tbProgress.Text = e.ProgressPercentage.ToString() & "%"
    'End Sub

    Private Sub BackgroundWorkerThrottle_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles BackgroundWorkerThrottle.RunWorkerCompleted
        'ButtonPlayer.Text = "Start Player"
        'PictureBoxPlayer.Image = My.Resources.rectangle_rouge
    End Sub

    Private Sub BackgroundWorkerAuxiliary_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorkerAuxiliary.DoWork
        While TimerRXAuxiliaire.Enabled = True
            Application.DoEvents()
            'convertion de Pulse (600,2400) en (valeur bar graph (0,100)
            Dim NewPulseAux As Integer = mapValue(pulseValueAux,
                                        CInt(textIdleServo1.Text), CInt(textMaxiMoteurs.Text),
                                        Me.ProgressBarThrottleAuxiliary._Mini, Me.ProgressBarThrottleAuxiliary._Maxi)

            If NewPulseAux > Me.ProgressBarThrottleAuxiliary._Maxi Then
                Me.ProgressBarThrottleAuxiliary._Value = Me.ProgressBarThrottleAuxiliary._Maxi
            ElseIf NewPulse < Me.ProgressBarThrottleAuxiliary._Mini Then
                Me.ProgressBarThrottleAuxiliary._Value = Me.ProgressBarThrottleAuxiliary._Mini
            End If

            'If term.Visible = True Then
            '    term.TextBoxTerminalComPort.AppendText("NewPulseAux: " & NewPulseAux & vbCrLf)
            'End If

            'MsgBox(NewPulseAux)
            Me.ProgressBarThrottleAuxiliary._Value = NewPulseAux
            labelA.Text = NewPulseAux
        End While
    End Sub

    'Private Sub bw_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs)
    '    'Me.tbProgress.Text = e.ProgressPercentage.ToString() & "%"
    'End Sub

    Private Sub BackgroundWorkerAuxiliary_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles BackgroundWorkerAuxiliary.RunWorkerCompleted
        'ButtonPlayer.Text = "Start Player"
        'PictureBoxPlayer.Image = My.Resources.rectangle_rouge
    End Sub


End Class


