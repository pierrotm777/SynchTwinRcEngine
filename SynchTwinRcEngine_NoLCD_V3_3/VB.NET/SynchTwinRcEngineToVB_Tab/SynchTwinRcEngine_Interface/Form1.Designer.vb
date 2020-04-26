<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.ComboPort = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ComboBaudRate = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.ButtonSauvegardeCOM = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.ButtonAbout = New System.Windows.Forms.Button()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.btnSend = New System.Windows.Forms.Button()
        Me.GroupBoxSerialPort = New System.Windows.Forms.GroupBox()
        Me.PictureBoxConnectedOK = New System.Windows.Forms.PictureBox()
        Me.Button_Connect = New System.Windows.Forms.Button()
        Me.GroupBoxSettings = New System.Windows.Forms.GroupBox()
        Me.ButtonRcRadioMode = New System.Windows.Forms.Button()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.labelModeRcRadio = New System.Windows.Forms.Label()
        Me.LabelSignalType = New System.Windows.Forms.Label()
        Me.ButtonClear = New System.Windows.Forms.Button()
        Me.ProgressBarSaveSettings = New System.Windows.Forms.ProgressBar()
        Me.labelExtervalVoltageUsed = New System.Windows.Forms.Label()
        Me.PictureBoxReadHardwareOnOff = New System.Windows.Forms.PictureBox()
        Me.RichTextBoxSettingsHelp = New System.Windows.Forms.RichTextBox()
        Me.labelCenterServo1 = New System.Windows.Forms.Label()
        Me.ButtonSettingsHelp = New System.Windows.Forms.Button()
        Me.labelCenterServo2 = New System.Windows.Forms.Label()
        Me.ButtonReadCenter2 = New System.Windows.Forms.Button()
        Me.buttonResetArduino = New System.Windows.Forms.Button()
        Me.ButtonReadCenter1 = New System.Windows.Forms.Button()
        Me.textCentreServo1 = New System.Windows.Forms.TextBox()
        Me.buttonEraseEEprom = New System.Windows.Forms.Button()
        Me.textCentreServo2 = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.ButtonDiffSpeedSimuConsigne = New System.Windows.Forms.Label()
        Me.TextBoxDiffSpeedSimuConsigne = New System.Windows.Forms.TextBox()
        Me.textMaxiMotorRPM = New System.Windows.Forms.TextBox()
        Me.textMiniMotorRPM = New System.Windows.Forms.TextBox()
        Me.labelSpeedMinMaxRPM = New System.Windows.Forms.Label()
        Me.textGeneralMinMaxStopWatch = New System.Windows.Forms.Label()
        Me.labelReverseAuxi = New System.Windows.Forms.Label()
        Me.CheckBoxInversionAux = New System.Windows.Forms.CheckBox()
        Me.LabelDEBUG = New System.Windows.Forms.Label()
        Me.CheckBoxFahrenheitDegrees = New System.Windows.Forms.CheckBox()
        Me.ButtonModuleType = New System.Windows.Forms.Button()
        Me.ButtonReadTempVoltage = New System.Windows.Forms.Button()
        Me.PictureBoxTimer2OnOff = New System.Windows.Forms.PictureBox()
        Me.labelAuxiPosition = New System.Windows.Forms.Label()
        Me.ButtonReadAuxiliairePulse = New System.Windows.Forms.Button()
        Me.TextBoxAuxiliairePulse = New System.Windows.Forms.TextBox()
        Me.ButtonAuxiliaireHelp = New System.Windows.Forms.Button()
        Me.LabelInterType = New System.Windows.Forms.Label()
        Me.ButtonMiniMaxGeneral = New System.Windows.Forms.Button()
        Me.ButtonDebutSynchro = New System.Windows.Forms.Button()
        Me.ButtonMaxiMoteurs = New System.Windows.Forms.Button()
        Me.ButtonIdleMoteur2 = New System.Windows.Forms.Button()
        Me.ButtonIdleMoteur1 = New System.Windows.Forms.Button()
        Me.ButtonConfigDefaut = New System.Windows.Forms.Button()
        Me.ButtonPlusNombrePales = New System.Windows.Forms.Button()
        Me.ButtonMoinsNombrePales = New System.Windows.Forms.Button()
        Me.ButtonPlusModeAuxiliaire = New System.Windows.Forms.Button()
        Me.ButtonMoinsModeAuxiliaire = New System.Windows.Forms.Button()
        Me.ButtonPlusVitesseReponse = New System.Windows.Forms.Button()
        Me.ButtonMoinsVitesseReponse = New System.Windows.Forms.Button()
        Me.LabelModifications = New System.Windows.Forms.Label()
        Me.ButtonAnnulerModif = New System.Windows.Forms.Button()
        Me.CheckBoxInversionServo2 = New System.Windows.Forms.CheckBox()
        Me.CheckBoxInversionServo1 = New System.Windows.Forms.CheckBox()
        Me.TextVoltageExterne = New System.Windows.Forms.TextBox()
        Me.TextTempInterne = New System.Windows.Forms.TextBox()
        Me.TextVoltageInterne = New System.Windows.Forms.TextBox()
        Me.textNombrePales = New System.Windows.Forms.TextBox()
        Me.textAuxiliaireMode = New System.Windows.Forms.TextBox()
        Me.textMaxiGenerale = New System.Windows.Forms.TextBox()
        Me.textMiniGenerale = New System.Windows.Forms.TextBox()
        Me.textDebutSynchro = New System.Windows.Forms.TextBox()
        Me.textMaxiMoteurs = New System.Windows.Forms.TextBox()
        Me.textTempsReponse = New System.Windows.Forms.TextBox()
        Me.textIdleServo2 = New System.Windows.Forms.TextBox()
        Me.textIdleServo1 = New System.Windows.Forms.TextBox()
        Me.LabelExternalVoltage = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.labelInternalVoltage = New System.Windows.Forms.Label()
        Me.labelNbrBlades = New System.Windows.Forms.Label()
        Me.labelMode = New System.Windows.Forms.Label()
        Me.labelReverseServo2 = New System.Windows.Forms.Label()
        Me.labelReverseServo1 = New System.Windows.Forms.Label()
        Me.labelAuxiMode = New System.Windows.Forms.Label()
        Me.labelPosMaxiGene = New System.Windows.Forms.Label()
        Me.labelPosMiniGene = New System.Windows.Forms.Label()
        Me.labelStartSynchroServos = New System.Windows.Forms.Label()
        Me.labelMaxiServos = New System.Windows.Forms.Label()
        Me.labelSpeedModuleAnswer = New System.Windows.Forms.Label()
        Me.labelIdleServo2 = New System.Windows.Forms.Label()
        Me.labelIdleServo1 = New System.Windows.Forms.Label()
        Me.ButtonSauvegardeConfig = New System.Windows.Forms.Button()
        Me.labelConfigModule = New System.Windows.Forms.Label()
        Me.TimerRXAuxiliaire = New System.Windows.Forms.Timer(Me.components)
        Me.TimerRXMotors = New System.Windows.Forms.Timer(Me.components)
        Me.TimerHardwareInfos = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBoxMoteurs = New System.Windows.Forms.GroupBox()
        Me.SevenSegmentArray2 = New SevenSegment.SevenSegmentArray()
        Me.SevenSegmentArray1 = New SevenSegment.SevenSegmentArray()
        Me.AquaGaugeMoteur2 = New AquaControls.AquaGauge()
        Me.AquaGaugeMoteur1 = New AquaControls.AquaGauge()
        Me.LabelVitesse2 = New System.Windows.Forms.Label()
        Me.LabelVitesse1 = New System.Windows.Forms.Label()
        Me.zg1 = New ZedGraph.ZedGraphControl()
        Me.TimerDataLogger = New System.Windows.Forms.Timer(Me.components)
        Me.SerialPort1 = New System.IO.Ports.SerialPort(Me.components)
        Me.optLangFR = New System.Windows.Forms.RadioButton()
        Me.optLangEN = New System.Windows.Forms.RadioButton()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.ButtonTerminal = New System.Windows.Forms.Button()
        Me.labelInfoNeedSaveCOM = New System.Windows.Forms.TextBox()
        Me.GroupBoxLang = New System.Windows.Forms.GroupBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.labelMotor2 = New System.Windows.Forms.Label()
        Me.CheckBoxAnaDigi = New System.Windows.Forms.CheckBox()
        Me.ButtonReadSpeeds = New System.Windows.Forms.Button()
        Me.labelMotor1 = New System.Windows.Forms.Label()
        Me.PictureBoxTimer1OnOff = New System.Windows.Forms.PictureBox()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.DataLogger = New System.Windows.Forms.GroupBox()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.LabelTestNow = New System.Windows.Forms.Label()
        Me.LabelChrono = New System.Windows.Forms.Label()
        Me.RichTextSimulationHelp = New System.Windows.Forms.RichTextBox()
        Me.BoxRecorder = New System.Windows.Forms.GroupBox()
        Me.CheckBoxReadTracBarMotorOrMotorThrottle = New System.Windows.Forms.CheckBox()
        Me.CheckBoxChronoOnOff = New System.Windows.Forms.CheckBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.LabelChronoSS = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.TextBoxChronoMM = New System.Windows.Forms.TextBox()
        Me.LabelChronoSimu = New System.Windows.Forms.Label()
        Me.TextBoxChronoHH = New System.Windows.Forms.TextBox()
        Me.PictureBox6 = New System.Windows.Forms.PictureBox()
        Me.PictureBoxPlayer = New System.Windows.Forms.PictureBox()
        Me.ButtonPlayer = New System.Windows.Forms.Button()
        Me.PictureBoxRecorder = New System.Windows.Forms.PictureBox()
        Me.ButtonRecorder = New System.Windows.Forms.Button()
        Me.ButtonSimulationHelp = New System.Windows.Forms.Button()
        Me.GroupSpeedPID = New System.Windows.Forms.GroupBox()
        Me.ButtonSavePIDVariables = New System.Windows.Forms.Button()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.textBoxKdControl = New System.Windows.Forms.TextBox()
        Me.textBoxKiControl = New System.Windows.Forms.TextBox()
        Me.textBoxKpControl = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LabelOnSimuMoveThrottle = New System.Windows.Forms.Label()
        Me.TrackBarRudder = New System.Windows.Forms.TrackBar()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.labelAuxRudderSimulation = New System.Windows.Forms.Label()
        Me.PictureBoxGlowPlugOnOff = New System.Windows.Forms.PictureBox()
        Me.ButtonServoTest = New System.Windows.Forms.Button()
        Me.ButtonGlowPlugOnOff = New System.Windows.Forms.Button()
        Me.PictureBoxGlowPlug = New System.Windows.Forms.PictureBox()
        Me.ProgressBarAuxiliary = New System.Windows.Forms.ProgressBar()
        Me.PictureBoxAuxMaxi = New System.Windows.Forms.PictureBox()
        Me.PictureBoxAuxMiddle = New System.Windows.Forms.PictureBox()
        Me.PictureBoxAuxMini = New System.Windows.Forms.PictureBox()
        Me.ButtonAuxMaxi = New System.Windows.Forms.Button()
        Me.ButtonAuxMiddle = New System.Windows.Forms.Button()
        Me.ButtonAuxMini = New System.Windows.Forms.Button()
        Me.ProgressBarThrottle = New System.Windows.Forms.ProgressBar()
        Me.LabelAux = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.TrackBarMotors = New System.Windows.Forms.TrackBar()
        Me.PictureBoxSimuThrottle = New System.Windows.Forms.PictureBox()
        Me.labelMotorsSimulation = New System.Windows.Forms.Label()
        Me.LabelMotors = New System.Windows.Forms.Label()
        Me.GroupSpeedSimu = New System.Windows.Forms.GroupBox()
        Me.PictureBoxSimuOnOff = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TextBoxForceSpeedSimu2 = New System.Windows.Forms.TextBox()
        Me.CheckBoxSimuSynchroSpeeds = New System.Windows.Forms.CheckBox()
        Me.TrackBarSpeedSimu2 = New System.Windows.Forms.TrackBar()
        Me.TrackBarSpeedSimu1 = New System.Windows.Forms.TrackBar()
        Me.TextBoxForceSpeedSimu1 = New System.Windows.Forms.TextBox()
        Me.ButtonSpeedSimuOn = New System.Windows.Forms.Button()
        Me.TabPage6 = New System.Windows.Forms.TabPage()
        Me.GroupBoxSDListFiles = New System.Windows.Forms.GroupBox()
        Me.ButtonDumpLogFile = New System.Windows.Forms.Button()
        Me.ListBoxSDListFiles = New System.Windows.Forms.ListBox()
        Me.ButtonSDCardErase = New System.Windows.Forms.Button()
        Me.GroupBoxSDCardInfos = New System.Windows.Forms.GroupBox()
        Me.ButtonSDCardListFiles = New System.Windows.Forms.Button()
        Me.ButtonWriteDataLogger = New System.Windows.Forms.Button()
        Me.buttonSDCardInfos = New System.Windows.Forms.Button()
        Me.labelSDCardIsUsed = New System.Windows.Forms.Label()
        Me.labelSDCardFAT = New System.Windows.Forms.Label()
        Me.labelSDCardType = New System.Windows.Forms.Label()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.TextBoxHexaEditor = New System.Windows.Forms.TextBox()
        Me.PictureBoxPinOut = New System.Windows.Forms.PictureBox()
        Me.GroupBoxOperations = New System.Windows.Forms.GroupBox()
        Me.ButtonReadHexaEditor = New System.Windows.Forms.Button()
        Me.cmddataviewer = New System.Windows.Forms.Button()
        Me.cmderasechip = New System.Windows.Forms.Button()
        Me.cmdverify = New System.Windows.Forms.Button()
        Me.cmdread = New System.Windows.Forms.Button()
        Me.cmdwrite = New System.Windows.Forms.Button()
        Me.rdowrveeprom = New System.Windows.Forms.RadioButton()
        Me.rdowrvflash = New System.Windows.Forms.RadioButton()
        Me.txtoutput = New System.Windows.Forms.TextBox()
        Me.grpfile = New System.Windows.Forms.GroupBox()
        Me.ButtonPinOutHelp = New System.Windows.Forms.Button()
        Me.cmddata = New System.Windows.Forms.Button()
        Me.cmdwritefuse = New System.Windows.Forms.Button()
        Me.lblLfuse = New System.Windows.Forms.Label()
        Me.lblHfuse = New System.Windows.Forms.Label()
        Me.lblEfuse = New System.Windows.Forms.Label()
        Me.txtlfuse = New System.Windows.Forms.TextBox()
        Me.txthfuse = New System.Windows.Forms.TextBox()
        Me.txtefuse = New System.Windows.Forms.TextBox()
        Me.cmdreadfuse = New System.Windows.Forms.Button()
        Me.lblavrchip = New System.Windows.Forms.Label()
        Me.ButtonCmdWindow = New System.Windows.Forms.Button()
        Me.cboprecon = New System.Windows.Forms.ComboBox()
        Me.ButtonBootLoader = New System.Windows.Forms.Button()
        Me.ButtonUSBAspUpload = New System.Windows.Forms.Button()
        Me.CheckBoxUseUSBAsp = New System.Windows.Forms.CheckBox()
        Me.lblinstallhw = New System.Windows.Forms.Label()
        Me.chktoggledtr = New System.Windows.Forms.CheckBox()
        Me.lblbrdname = New System.Windows.Forms.Label()
        Me.ButtonSerialUpload = New System.Windows.Forms.Button()
        Me.cboboardname = New System.Windows.Forms.ComboBox()
        Me.bttnbrowse = New System.Windows.Forms.Button()
        Me.txtfilename = New System.Windows.Forms.TextBox()
        Me.lblfileselect = New System.Windows.Forms.Label()
        Me.TabPage8 = New System.Windows.Forms.TabPage()
        Me.ButtonPlayGame = New System.Windows.Forms.Button()
        Me.ofd = New System.Windows.Forms.OpenFileDialog()
        Me.svfd = New System.Windows.Forms.SaveFileDialog()
        Me.TimerSpeeds = New System.Windows.Forms.Timer(Me.components)
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.fd = New System.Windows.Forms.OpenFileDialog()
        Me.TimerChrono = New System.Windows.Forms.Timer(Me.components)
        Me.BackgroundWorkerThrottle = New System.ComponentModel.BackgroundWorker()
        Me.BackgroundWorkerAuxiliary = New System.ComponentModel.BackgroundWorker()
        Me.ProgressBarThrottleAuxiliary = New SynchTwinRcEngine_Interface.UcV_ProgressBar()
        Me.ProgressBarThrottleMotors = New SynchTwinRcEngine_Interface.UcV_ProgressBar()
        Me.UcV_ProgressBar1 = New SynchTwinRcEngine_Interface.UcV_ProgressBar()
        Me.UcV_ProgressBar2 = New SynchTwinRcEngine_Interface.UcV_ProgressBar()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxSerialPort.SuspendLayout()
        CType(Me.PictureBoxConnectedOK, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxSettings.SuspendLayout()
        CType(Me.PictureBoxReadHardwareOnOff, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxTimer2OnOff, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxMoteurs.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBoxLang.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        CType(Me.PictureBoxTimer1OnOff, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage4.SuspendLayout()
        Me.DataLogger.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.BoxRecorder.SuspendLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxPlayer, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxRecorder, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupSpeedPID.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.TrackBarRudder, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxGlowPlugOnOff, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxGlowPlug, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxAuxMaxi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxAuxMiddle, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxAuxMini, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBarMotors, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBoxSimuThrottle, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupSpeedSimu.SuspendLayout()
        CType(Me.PictureBoxSimuOnOff, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBarSpeedSimu2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBarSpeedSimu1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage6.SuspendLayout()
        Me.GroupBoxSDListFiles.SuspendLayout()
        Me.GroupBoxSDCardInfos.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        CType(Me.PictureBoxPinOut, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBoxOperations.SuspendLayout()
        Me.grpfile.SuspendLayout()
        Me.TabPage8.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboPort
        '
        Me.ComboPort.FormattingEnabled = True
        Me.ComboPort.Location = New System.Drawing.Point(46, 19)
        Me.ComboPort.Name = "ComboPort"
        Me.ComboPort.Size = New System.Drawing.Size(79, 21)
        Me.ComboPort.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(3, 20)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 16)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "Com  :"
        '
        'ComboBaudRate
        '
        Me.ComboBaudRate.FormattingEnabled = True
        Me.ComboBaudRate.Location = New System.Drawing.Point(46, 46)
        Me.ComboBaudRate.Name = "ComboBaudRate"
        Me.ComboBaudRate.Size = New System.Drawing.Size(79, 21)
        Me.ComboBaudRate.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(3, 47)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 16)
        Me.Label3.TabIndex = 8
        Me.Label3.Text = "Baud :"
        '
        'ButtonSauvegardeCOM
        '
        Me.ButtonSauvegardeCOM.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonSauvegardeCOM.Location = New System.Drawing.Point(169, 47)
        Me.ButtonSauvegardeCOM.Name = "ButtonSauvegardeCOM"
        Me.ButtonSauvegardeCOM.Size = New System.Drawing.Size(105, 23)
        Me.ButtonSauvegardeCOM.TabIndex = 10
        Me.ButtonSauvegardeCOM.Text = "Sauvegarde"
        Me.ButtonSauvegardeCOM.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox1.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources.Mosquito
        Me.PictureBox1.Location = New System.Drawing.Point(6, 6)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(87, 51)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 0
        Me.PictureBox1.TabStop = False
        '
        'ButtonAbout
        '
        Me.ButtonAbout.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonAbout.Location = New System.Drawing.Point(566, 6)
        Me.ButtonAbout.Name = "ButtonAbout"
        Me.ButtonAbout.Size = New System.Drawing.Size(25, 23)
        Me.ButtonAbout.TabIndex = 118
        Me.ButtonAbout.Text = "?"
        Me.ButtonAbout.UseVisualStyleBackColor = True
        '
        'txtMessage
        '
        Me.txtMessage.Location = New System.Drawing.Point(61, 334)
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(286, 20)
        Me.txtMessage.TabIndex = 120
        '
        'btnSend
        '
        Me.btnSend.Location = New System.Drawing.Point(353, 332)
        Me.btnSend.Name = "btnSend"
        Me.btnSend.Size = New System.Drawing.Size(44, 23)
        Me.btnSend.TabIndex = 119
        Me.btnSend.Text = "Envoi"
        Me.btnSend.UseVisualStyleBackColor = True
        '
        'GroupBoxSerialPort
        '
        Me.GroupBoxSerialPort.Controls.Add(Me.PictureBoxConnectedOK)
        Me.GroupBoxSerialPort.Controls.Add(Me.Button_Connect)
        Me.GroupBoxSerialPort.Controls.Add(Me.ButtonSauvegardeCOM)
        Me.GroupBoxSerialPort.Controls.Add(Me.ComboBaudRate)
        Me.GroupBoxSerialPort.Controls.Add(Me.ComboPort)
        Me.GroupBoxSerialPort.Controls.Add(Me.Label3)
        Me.GroupBoxSerialPort.Controls.Add(Me.Label2)
        Me.GroupBoxSerialPort.Location = New System.Drawing.Point(97, 3)
        Me.GroupBoxSerialPort.Name = "GroupBoxSerialPort"
        Me.GroupBoxSerialPort.Size = New System.Drawing.Size(281, 76)
        Me.GroupBoxSerialPort.TabIndex = 23
        Me.GroupBoxSerialPort.TabStop = False
        Me.GroupBoxSerialPort.Text = "Port Série"
        '
        'PictureBoxConnectedOK
        '
        Me.PictureBoxConnectedOK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxConnectedOK.Location = New System.Drawing.Point(155, 17)
        Me.PictureBoxConnectedOK.Name = "PictureBoxConnectedOK"
        Me.PictureBoxConnectedOK.Size = New System.Drawing.Size(13, 16)
        Me.PictureBoxConnectedOK.TabIndex = 124
        Me.PictureBoxConnectedOK.TabStop = False
        '
        'Button_Connect
        '
        Me.Button_Connect.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Button_Connect.Location = New System.Drawing.Point(169, 14)
        Me.Button_Connect.Name = "Button_Connect"
        Me.Button_Connect.Size = New System.Drawing.Size(105, 23)
        Me.Button_Connect.TabIndex = 11
        Me.Button_Connect.Text = "Connection"
        Me.Button_Connect.UseVisualStyleBackColor = True
        '
        'GroupBoxSettings
        '
        Me.GroupBoxSettings.Controls.Add(Me.ButtonRcRadioMode)
        Me.GroupBoxSettings.Controls.Add(Me.Label17)
        Me.GroupBoxSettings.Controls.Add(Me.labelModeRcRadio)
        Me.GroupBoxSettings.Controls.Add(Me.LabelSignalType)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonClear)
        Me.GroupBoxSettings.Controls.Add(Me.ProgressBarSaveSettings)
        Me.GroupBoxSettings.Controls.Add(Me.labelExtervalVoltageUsed)
        Me.GroupBoxSettings.Controls.Add(Me.PictureBoxReadHardwareOnOff)
        Me.GroupBoxSettings.Controls.Add(Me.RichTextBoxSettingsHelp)
        Me.GroupBoxSettings.Controls.Add(Me.labelCenterServo1)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonSettingsHelp)
        Me.GroupBoxSettings.Controls.Add(Me.labelCenterServo2)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonReadCenter2)
        Me.GroupBoxSettings.Controls.Add(Me.buttonResetArduino)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonReadCenter1)
        Me.GroupBoxSettings.Controls.Add(Me.textCentreServo1)
        Me.GroupBoxSettings.Controls.Add(Me.buttonEraseEEprom)
        Me.GroupBoxSettings.Controls.Add(Me.textCentreServo2)
        Me.GroupBoxSettings.Controls.Add(Me.Label12)
        Me.GroupBoxSettings.Controls.Add(Me.Label11)
        Me.GroupBoxSettings.Controls.Add(Me.ProgressBarThrottleAuxiliary)
        Me.GroupBoxSettings.Controls.Add(Me.ProgressBarThrottleMotors)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonDiffSpeedSimuConsigne)
        Me.GroupBoxSettings.Controls.Add(Me.TextBoxDiffSpeedSimuConsigne)
        Me.GroupBoxSettings.Controls.Add(Me.textMaxiMotorRPM)
        Me.GroupBoxSettings.Controls.Add(Me.textMiniMotorRPM)
        Me.GroupBoxSettings.Controls.Add(Me.labelSpeedMinMaxRPM)
        Me.GroupBoxSettings.Controls.Add(Me.textGeneralMinMaxStopWatch)
        Me.GroupBoxSettings.Controls.Add(Me.labelReverseAuxi)
        Me.GroupBoxSettings.Controls.Add(Me.CheckBoxInversionAux)
        Me.GroupBoxSettings.Controls.Add(Me.LabelDEBUG)
        Me.GroupBoxSettings.Controls.Add(Me.CheckBoxFahrenheitDegrees)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonModuleType)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonReadTempVoltage)
        Me.GroupBoxSettings.Controls.Add(Me.PictureBoxTimer2OnOff)
        Me.GroupBoxSettings.Controls.Add(Me.labelAuxiPosition)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonReadAuxiliairePulse)
        Me.GroupBoxSettings.Controls.Add(Me.TextBoxAuxiliairePulse)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonAuxiliaireHelp)
        Me.GroupBoxSettings.Controls.Add(Me.LabelInterType)
        Me.GroupBoxSettings.Controls.Add(Me.txtMessage)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonMiniMaxGeneral)
        Me.GroupBoxSettings.Controls.Add(Me.btnSend)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonDebutSynchro)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonMaxiMoteurs)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonIdleMoteur2)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonIdleMoteur1)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonConfigDefaut)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonPlusNombrePales)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonMoinsNombrePales)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonPlusModeAuxiliaire)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonMoinsModeAuxiliaire)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonPlusVitesseReponse)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonMoinsVitesseReponse)
        Me.GroupBoxSettings.Controls.Add(Me.LabelModifications)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonAnnulerModif)
        Me.GroupBoxSettings.Controls.Add(Me.CheckBoxInversionServo2)
        Me.GroupBoxSettings.Controls.Add(Me.CheckBoxInversionServo1)
        Me.GroupBoxSettings.Controls.Add(Me.TextVoltageExterne)
        Me.GroupBoxSettings.Controls.Add(Me.TextTempInterne)
        Me.GroupBoxSettings.Controls.Add(Me.TextVoltageInterne)
        Me.GroupBoxSettings.Controls.Add(Me.textNombrePales)
        Me.GroupBoxSettings.Controls.Add(Me.textAuxiliaireMode)
        Me.GroupBoxSettings.Controls.Add(Me.textMaxiGenerale)
        Me.GroupBoxSettings.Controls.Add(Me.textMiniGenerale)
        Me.GroupBoxSettings.Controls.Add(Me.textDebutSynchro)
        Me.GroupBoxSettings.Controls.Add(Me.textMaxiMoteurs)
        Me.GroupBoxSettings.Controls.Add(Me.textTempsReponse)
        Me.GroupBoxSettings.Controls.Add(Me.textIdleServo2)
        Me.GroupBoxSettings.Controls.Add(Me.textIdleServo1)
        Me.GroupBoxSettings.Controls.Add(Me.LabelExternalVoltage)
        Me.GroupBoxSettings.Controls.Add(Me.Label19)
        Me.GroupBoxSettings.Controls.Add(Me.labelInternalVoltage)
        Me.GroupBoxSettings.Controls.Add(Me.labelNbrBlades)
        Me.GroupBoxSettings.Controls.Add(Me.labelMode)
        Me.GroupBoxSettings.Controls.Add(Me.labelReverseServo2)
        Me.GroupBoxSettings.Controls.Add(Me.labelReverseServo1)
        Me.GroupBoxSettings.Controls.Add(Me.labelAuxiMode)
        Me.GroupBoxSettings.Controls.Add(Me.labelPosMaxiGene)
        Me.GroupBoxSettings.Controls.Add(Me.labelPosMiniGene)
        Me.GroupBoxSettings.Controls.Add(Me.labelStartSynchroServos)
        Me.GroupBoxSettings.Controls.Add(Me.labelMaxiServos)
        Me.GroupBoxSettings.Controls.Add(Me.labelSpeedModuleAnswer)
        Me.GroupBoxSettings.Controls.Add(Me.labelIdleServo2)
        Me.GroupBoxSettings.Controls.Add(Me.labelIdleServo1)
        Me.GroupBoxSettings.Controls.Add(Me.ButtonSauvegardeConfig)
        Me.GroupBoxSettings.Controls.Add(Me.labelConfigModule)
        Me.GroupBoxSettings.Location = New System.Drawing.Point(3, 3)
        Me.GroupBoxSettings.Name = "GroupBoxSettings"
        Me.GroupBoxSettings.Size = New System.Drawing.Size(591, 360)
        Me.GroupBoxSettings.TabIndex = 26
        Me.GroupBoxSettings.TabStop = False
        Me.GroupBoxSettings.Text = "Configuration"
        '
        'ButtonRcRadioMode
        '
        Me.ButtonRcRadioMode.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonRcRadioMode.Location = New System.Drawing.Point(385, 77)
        Me.ButtonRcRadioMode.Name = "ButtonRcRadioMode"
        Me.ButtonRcRadioMode.Size = New System.Drawing.Size(58, 20)
        Me.ButtonRcRadioMode.TabIndex = 151
        Me.ButtonRcRadioMode.Text = "Mode"
        Me.ButtonRcRadioMode.UseVisualStyleBackColor = True
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(331, 81)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(12, 13)
        Me.Label17.TabIndex = 150
        Me.Label17.Text = "/"
        '
        'labelModeRcRadio
        '
        Me.labelModeRcRadio.AutoSize = True
        Me.labelModeRcRadio.Location = New System.Drawing.Point(319, 81)
        Me.labelModeRcRadio.Name = "labelModeRcRadio"
        Me.labelModeRcRadio.Size = New System.Drawing.Size(13, 13)
        Me.labelModeRcRadio.TabIndex = 149
        Me.labelModeRcRadio.Text = "0"
        '
        'LabelSignalType
        '
        Me.LabelSignalType.AutoSize = True
        Me.LabelSignalType.Location = New System.Drawing.Point(342, 81)
        Me.LabelSignalType.Name = "LabelSignalType"
        Me.LabelSignalType.Size = New System.Drawing.Size(37, 13)
        Me.LabelSignalType.TabIndex = 148
        Me.LabelSignalType.Text = "CPPM"
        '
        'ButtonClear
        '
        Me.ButtonClear.Location = New System.Drawing.Point(403, 332)
        Me.ButtonClear.Name = "ButtonClear"
        Me.ButtonClear.Size = New System.Drawing.Size(44, 23)
        Me.ButtonClear.TabIndex = 147
        Me.ButtonClear.Text = "Clear"
        Me.ButtonClear.UseVisualStyleBackColor = True
        '
        'ProgressBarSaveSettings
        '
        Me.ProgressBarSaveSettings.Location = New System.Drawing.Point(120, 278)
        Me.ProgressBarSaveSettings.Name = "ProgressBarSaveSettings"
        Me.ProgressBarSaveSettings.Size = New System.Drawing.Size(300, 18)
        Me.ProgressBarSaveSettings.Step = 5
        Me.ProgressBarSaveSettings.TabIndex = 146
        '
        'labelExtervalVoltageUsed
        '
        Me.labelExtervalVoltageUsed.AutoSize = True
        Me.labelExtervalVoltageUsed.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelExtervalVoltageUsed.Location = New System.Drawing.Point(424, 126)
        Me.labelExtervalVoltageUsed.Name = "labelExtervalVoltageUsed"
        Me.labelExtervalVoltageUsed.Size = New System.Drawing.Size(60, 13)
        Me.labelExtervalVoltageUsed.TabIndex = 145
        Me.labelExtervalVoltageUsed.Text = "Not Used"
        '
        'PictureBoxReadHardwareOnOff
        '
        Me.PictureBoxReadHardwareOnOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxReadHardwareOnOff.Location = New System.Drawing.Point(520, 126)
        Me.PictureBoxReadHardwareOnOff.Name = "PictureBoxReadHardwareOnOff"
        Me.PictureBoxReadHardwareOnOff.Size = New System.Drawing.Size(13, 16)
        Me.PictureBoxReadHardwareOnOff.TabIndex = 144
        Me.PictureBoxReadHardwareOnOff.TabStop = False
        '
        'RichTextBoxSettingsHelp
        '
        Me.RichTextBoxSettingsHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.RichTextBoxSettingsHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextBoxSettingsHelp.Location = New System.Drawing.Point(4, 21)
        Me.RichTextBoxSettingsHelp.Name = "RichTextBoxSettingsHelp"
        Me.RichTextBoxSettingsHelp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.RichTextBoxSettingsHelp.Size = New System.Drawing.Size(19, 29)
        Me.RichTextBoxSettingsHelp.TabIndex = 143
        Me.RichTextBoxSettingsHelp.Text = "Help"
        '
        'labelCenterServo1
        '
        Me.labelCenterServo1.AutoSize = True
        Me.labelCenterServo1.Location = New System.Drawing.Point(24, 81)
        Me.labelCenterServo1.Name = "labelCenterServo1"
        Me.labelCenterServo1.Size = New System.Drawing.Size(79, 13)
        Me.labelCenterServo1.TabIndex = 56
        Me.labelCenterServo1.Text = "Centre servo1: "
        '
        'ButtonSettingsHelp
        '
        Me.ButtonSettingsHelp.Location = New System.Drawing.Point(541, 332)
        Me.ButtonSettingsHelp.Name = "ButtonSettingsHelp"
        Me.ButtonSettingsHelp.Size = New System.Drawing.Size(44, 23)
        Me.ButtonSettingsHelp.TabIndex = 142
        Me.ButtonSettingsHelp.Text = "Help"
        Me.ButtonSettingsHelp.UseVisualStyleBackColor = True
        '
        'labelCenterServo2
        '
        Me.labelCenterServo2.AutoSize = True
        Me.labelCenterServo2.Location = New System.Drawing.Point(24, 103)
        Me.labelCenterServo2.Name = "labelCenterServo2"
        Me.labelCenterServo2.Size = New System.Drawing.Size(79, 13)
        Me.labelCenterServo2.TabIndex = 57
        Me.labelCenterServo2.Text = "Centre servo2: "
        '
        'ButtonReadCenter2
        '
        Me.ButtonReadCenter2.Location = New System.Drawing.Point(200, 100)
        Me.ButtonReadCenter2.Name = "ButtonReadCenter2"
        Me.ButtonReadCenter2.Size = New System.Drawing.Size(44, 20)
        Me.ButtonReadCenter2.TabIndex = 105
        Me.ButtonReadCenter2.Text = "Lire"
        Me.ButtonReadCenter2.UseVisualStyleBackColor = True
        '
        'buttonResetArduino
        '
        Me.buttonResetArduino.Location = New System.Drawing.Point(474, 301)
        Me.buttonResetArduino.Name = "buttonResetArduino"
        Me.buttonResetArduino.Size = New System.Drawing.Size(111, 23)
        Me.buttonResetArduino.TabIndex = 140
        Me.buttonResetArduino.Text = "Reset"
        Me.buttonResetArduino.UseVisualStyleBackColor = True
        '
        'ButtonReadCenter1
        '
        Me.ButtonReadCenter1.Location = New System.Drawing.Point(200, 78)
        Me.ButtonReadCenter1.Name = "ButtonReadCenter1"
        Me.ButtonReadCenter1.Size = New System.Drawing.Size(44, 20)
        Me.ButtonReadCenter1.TabIndex = 19
        Me.ButtonReadCenter1.Text = "Lire"
        Me.ButtonReadCenter1.UseVisualStyleBackColor = True
        '
        'textCentreServo1
        '
        Me.textCentreServo1.Location = New System.Drawing.Point(158, 78)
        Me.textCentreServo1.Name = "textCentreServo1"
        Me.textCentreServo1.Size = New System.Drawing.Size(39, 20)
        Me.textCentreServo1.TabIndex = 75
        Me.textCentreServo1.Text = "0"
        '
        'buttonEraseEEprom
        '
        Me.buttonEraseEEprom.Location = New System.Drawing.Point(6, 301)
        Me.buttonEraseEEprom.Name = "buttonEraseEEprom"
        Me.buttonEraseEEprom.Size = New System.Drawing.Size(96, 23)
        Me.buttonEraseEEprom.TabIndex = 139
        Me.buttonEraseEEprom.Text = "Effacer EEprom"
        Me.buttonEraseEEprom.UseVisualStyleBackColor = True
        '
        'textCentreServo2
        '
        Me.textCentreServo2.Location = New System.Drawing.Point(158, 100)
        Me.textCentreServo2.Name = "textCentreServo2"
        Me.textCentreServo2.Size = New System.Drawing.Size(39, 20)
        Me.textCentreServo2.TabIndex = 76
        Me.textCentreServo2.Text = "0"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(559, 15)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(34, 17)
        Me.Label12.TabIndex = 138
        Me.Label12.Text = "Aux"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(528, 15)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(33, 17)
        Me.Label11.TabIndex = 137
        Me.Label11.Text = "Thr"
        '
        'ButtonDiffSpeedSimuConsigne
        '
        Me.ButtonDiffSpeedSimuConsigne.AutoSize = True
        Me.ButtonDiffSpeedSimuConsigne.Location = New System.Drawing.Point(249, 214)
        Me.ButtonDiffSpeedSimuConsigne.Name = "ButtonDiffSpeedSimuConsigne"
        Me.ButtonDiffSpeedSimuConsigne.Size = New System.Drawing.Size(85, 13)
        Me.ButtonDiffSpeedSimuConsigne.TabIndex = 134
        Me.ButtonDiffSpeedSimuConsigne.Text = "Vitesse Error µS:"
        '
        'TextBoxDiffSpeedSimuConsigne
        '
        Me.TextBoxDiffSpeedSimuConsigne.Location = New System.Drawing.Point(375, 212)
        Me.TextBoxDiffSpeedSimuConsigne.Name = "TextBoxDiffSpeedSimuConsigne"
        Me.TextBoxDiffSpeedSimuConsigne.Size = New System.Drawing.Size(35, 20)
        Me.TextBoxDiffSpeedSimuConsigne.TabIndex = 133
        Me.TextBoxDiffSpeedSimuConsigne.Text = "10"
        '
        'textMaxiMotorRPM
        '
        Me.textMaxiMotorRPM.Location = New System.Drawing.Point(463, 190)
        Me.textMaxiMotorRPM.Name = "textMaxiMotorRPM"
        Me.textMaxiMotorRPM.Size = New System.Drawing.Size(42, 20)
        Me.textMaxiMotorRPM.TabIndex = 131
        Me.textMaxiMotorRPM.Text = "20000"
        '
        'textMiniMotorRPM
        '
        Me.textMiniMotorRPM.Location = New System.Drawing.Point(422, 190)
        Me.textMiniMotorRPM.Name = "textMiniMotorRPM"
        Me.textMiniMotorRPM.Size = New System.Drawing.Size(35, 20)
        Me.textMiniMotorRPM.TabIndex = 130
        Me.textMiniMotorRPM.Text = "1000"
        '
        'labelSpeedMinMaxRPM
        '
        Me.labelSpeedMinMaxRPM.AutoSize = True
        Me.labelSpeedMinMaxRPM.Location = New System.Drawing.Point(249, 192)
        Me.labelSpeedMinMaxRPM.Name = "labelSpeedMinMaxRPM"
        Me.labelSpeedMinMaxRPM.Size = New System.Drawing.Size(123, 13)
        Me.labelSpeedMinMaxRPM.TabIndex = 129
        Me.labelSpeedMinMaxRPM.Text = "Vitesse Motor Mini/Maxi:"
        '
        'textGeneralMinMaxStopWatch
        '
        Me.textGeneralMinMaxStopWatch.AutoSize = True
        Me.textGeneralMinMaxStopWatch.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.textGeneralMinMaxStopWatch.ForeColor = System.Drawing.Color.Red
        Me.textGeneralMinMaxStopWatch.Location = New System.Drawing.Point(200, 59)
        Me.textGeneralMinMaxStopWatch.Name = "textGeneralMinMaxStopWatch"
        Me.textGeneralMinMaxStopWatch.Size = New System.Drawing.Size(14, 13)
        Me.textGeneralMinMaxStopWatch.TabIndex = 128
        Me.textGeneralMinMaxStopWatch.Text = "0"
        '
        'labelReverseAuxi
        '
        Me.labelReverseAuxi.AutoSize = True
        Me.labelReverseAuxi.Location = New System.Drawing.Point(266, 259)
        Me.labelReverseAuxi.Name = "labelReverseAuxi"
        Me.labelReverseAuxi.Size = New System.Drawing.Size(74, 13)
        Me.labelReverseAuxi.TabIndex = 127
        Me.labelReverseAuxi.Text = "Inversion Aux:"
        '
        'CheckBoxInversionAux
        '
        Me.CheckBoxInversionAux.AutoSize = True
        Me.CheckBoxInversionAux.Location = New System.Drawing.Point(356, 258)
        Me.CheckBoxInversionAux.Name = "CheckBoxInversionAux"
        Me.CheckBoxInversionAux.Size = New System.Drawing.Size(46, 17)
        Me.CheckBoxInversionAux.TabIndex = 126
        Me.CheckBoxInversionAux.Text = "Non"
        Me.CheckBoxInversionAux.UseVisualStyleBackColor = True
        '
        'LabelDEBUG
        '
        Me.LabelDEBUG.AutoSize = True
        Me.LabelDEBUG.Location = New System.Drawing.Point(6, 337)
        Me.LabelDEBUG.Name = "LabelDEBUG"
        Me.LabelDEBUG.Size = New System.Drawing.Size(53, 13)
        Me.LabelDEBUG.TabIndex = 125
        Me.LabelDEBUG.Text = "Terminal :"
        '
        'CheckBoxFahrenheitDegrees
        '
        Me.CheckBoxFahrenheitDegrees.AutoSize = True
        Me.CheckBoxFahrenheitDegrees.Location = New System.Drawing.Point(426, 149)
        Me.CheckBoxFahrenheitDegrees.Name = "CheckBoxFahrenheitDegrees"
        Me.CheckBoxFahrenheitDegrees.Size = New System.Drawing.Size(36, 17)
        Me.CheckBoxFahrenheitDegrees.TabIndex = 123
        Me.CheckBoxFahrenheitDegrees.Text = "°F"
        Me.CheckBoxFahrenheitDegrees.UseVisualStyleBackColor = True
        '
        'ButtonModuleType
        '
        Me.ButtonModuleType.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonModuleType.Location = New System.Drawing.Point(421, 255)
        Me.ButtonModuleType.Name = "ButtonModuleType"
        Me.ButtonModuleType.Size = New System.Drawing.Size(74, 20)
        Me.ButtonModuleType.TabIndex = 120
        Me.ButtonModuleType.Text = "Maître"
        Me.ButtonModuleType.UseVisualStyleBackColor = True
        '
        'ButtonReadTempVoltage
        '
        Me.ButtonReadTempVoltage.Location = New System.Drawing.Point(475, 124)
        Me.ButtonReadTempVoltage.Name = "ButtonReadTempVoltage"
        Me.ButtonReadTempVoltage.Size = New System.Drawing.Size(44, 20)
        Me.ButtonReadTempVoltage.TabIndex = 122
        Me.ButtonReadTempVoltage.Text = "Lire"
        Me.ButtonReadTempVoltage.UseVisualStyleBackColor = True
        '
        'PictureBoxTimer2OnOff
        '
        Me.PictureBoxTimer2OnOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxTimer2OnOff.Location = New System.Drawing.Point(252, 257)
        Me.PictureBoxTimer2OnOff.Name = "PictureBoxTimer2OnOff"
        Me.PictureBoxTimer2OnOff.Size = New System.Drawing.Size(13, 16)
        Me.PictureBoxTimer2OnOff.TabIndex = 121
        Me.PictureBoxTimer2OnOff.TabStop = False
        '
        'labelAuxiPosition
        '
        Me.labelAuxiPosition.AutoSize = True
        Me.labelAuxiPosition.Location = New System.Drawing.Point(24, 257)
        Me.labelAuxiPosition.Name = "labelAuxiPosition"
        Me.labelAuxiPosition.Size = New System.Drawing.Size(91, 13)
        Me.labelAuxiPosition.TabIndex = 120
        Me.labelAuxiPosition.Text = "Position Auxiliaire:"
        '
        'ButtonReadAuxiliairePulse
        '
        Me.ButtonReadAuxiliairePulse.Location = New System.Drawing.Point(200, 255)
        Me.ButtonReadAuxiliairePulse.Name = "ButtonReadAuxiliairePulse"
        Me.ButtonReadAuxiliairePulse.Size = New System.Drawing.Size(44, 20)
        Me.ButtonReadAuxiliairePulse.TabIndex = 119
        Me.ButtonReadAuxiliairePulse.Text = "Lire"
        Me.ButtonReadAuxiliairePulse.UseVisualStyleBackColor = True
        '
        'TextBoxAuxiliairePulse
        '
        Me.TextBoxAuxiliairePulse.Location = New System.Drawing.Point(158, 255)
        Me.TextBoxAuxiliairePulse.Name = "TextBoxAuxiliairePulse"
        Me.TextBoxAuxiliairePulse.Size = New System.Drawing.Size(39, 20)
        Me.TextBoxAuxiliairePulse.TabIndex = 118
        Me.TextBoxAuxiliairePulse.Text = "0"
        '
        'ButtonAuxiliaireHelp
        '
        Me.ButtonAuxiliaireHelp.Location = New System.Drawing.Point(4, 233)
        Me.ButtonAuxiliaireHelp.Name = "ButtonAuxiliaireHelp"
        Me.ButtonAuxiliaireHelp.Size = New System.Drawing.Size(19, 20)
        Me.ButtonAuxiliaireHelp.TabIndex = 117
        Me.ButtonAuxiliaireHelp.Text = "?"
        Me.ButtonAuxiliaireHelp.UseVisualStyleBackColor = True
        '
        'LabelInterType
        '
        Me.LabelInterType.AutoSize = True
        Me.LabelInterType.Location = New System.Drawing.Point(248, 237)
        Me.LabelInterType.Name = "LabelInterType"
        Me.LabelInterType.Size = New System.Drawing.Size(88, 13)
        Me.LabelInterType.TabIndex = 112
        Me.LabelInterType.Text = " ===> Non Utilisé"
        '
        'ButtonMiniMaxGeneral
        '
        Me.ButtonMiniMaxGeneral.Location = New System.Drawing.Point(200, 34)
        Me.ButtonMiniMaxGeneral.Name = "ButtonMiniMaxGeneral"
        Me.ButtonMiniMaxGeneral.Size = New System.Drawing.Size(44, 20)
        Me.ButtonMiniMaxGeneral.TabIndex = 110
        Me.ButtonMiniMaxGeneral.Text = "Lire"
        Me.ButtonMiniMaxGeneral.UseVisualStyleBackColor = True
        '
        'ButtonDebutSynchro
        '
        Me.ButtonDebutSynchro.Location = New System.Drawing.Point(200, 210)
        Me.ButtonDebutSynchro.Name = "ButtonDebutSynchro"
        Me.ButtonDebutSynchro.Size = New System.Drawing.Size(44, 20)
        Me.ButtonDebutSynchro.TabIndex = 109
        Me.ButtonDebutSynchro.Text = "Lire"
        Me.ButtonDebutSynchro.UseVisualStyleBackColor = True
        '
        'ButtonMaxiMoteurs
        '
        Me.ButtonMaxiMoteurs.Location = New System.Drawing.Point(200, 188)
        Me.ButtonMaxiMoteurs.Name = "ButtonMaxiMoteurs"
        Me.ButtonMaxiMoteurs.Size = New System.Drawing.Size(44, 20)
        Me.ButtonMaxiMoteurs.TabIndex = 108
        Me.ButtonMaxiMoteurs.Text = "Lire"
        Me.ButtonMaxiMoteurs.UseVisualStyleBackColor = True
        '
        'ButtonIdleMoteur2
        '
        Me.ButtonIdleMoteur2.Location = New System.Drawing.Point(200, 144)
        Me.ButtonIdleMoteur2.Name = "ButtonIdleMoteur2"
        Me.ButtonIdleMoteur2.Size = New System.Drawing.Size(44, 20)
        Me.ButtonIdleMoteur2.TabIndex = 107
        Me.ButtonIdleMoteur2.Text = "Lire"
        Me.ButtonIdleMoteur2.UseVisualStyleBackColor = True
        '
        'ButtonIdleMoteur1
        '
        Me.ButtonIdleMoteur1.Location = New System.Drawing.Point(200, 122)
        Me.ButtonIdleMoteur1.Name = "ButtonIdleMoteur1"
        Me.ButtonIdleMoteur1.Size = New System.Drawing.Size(44, 20)
        Me.ButtonIdleMoteur1.TabIndex = 106
        Me.ButtonIdleMoteur1.Text = "Lire"
        Me.ButtonIdleMoteur1.UseVisualStyleBackColor = True
        '
        'ButtonConfigDefaut
        '
        Me.ButtonConfigDefaut.Location = New System.Drawing.Point(230, 301)
        Me.ButtonConfigDefaut.Name = "ButtonConfigDefaut"
        Me.ButtonConfigDefaut.Size = New System.Drawing.Size(117, 23)
        Me.ButtonConfigDefaut.TabIndex = 104
        Me.ButtonConfigDefaut.Text = "RAZ Configuration"
        Me.ButtonConfigDefaut.UseVisualStyleBackColor = True
        '
        'ButtonPlusNombrePales
        '
        Me.ButtonPlusNombrePales.Location = New System.Drawing.Point(479, 99)
        Me.ButtonPlusNombrePales.Name = "ButtonPlusNombrePales"
        Me.ButtonPlusNombrePales.Size = New System.Drawing.Size(19, 20)
        Me.ButtonPlusNombrePales.TabIndex = 103
        Me.ButtonPlusNombrePales.Text = "+"
        Me.ButtonPlusNombrePales.UseVisualStyleBackColor = True
        '
        'ButtonMoinsNombrePales
        '
        Me.ButtonMoinsNombrePales.Location = New System.Drawing.Point(453, 99)
        Me.ButtonMoinsNombrePales.Name = "ButtonMoinsNombrePales"
        Me.ButtonMoinsNombrePales.Size = New System.Drawing.Size(19, 20)
        Me.ButtonMoinsNombrePales.TabIndex = 100
        Me.ButtonMoinsNombrePales.Text = "-"
        Me.ButtonMoinsNombrePales.UseVisualStyleBackColor = True
        '
        'ButtonPlusModeAuxiliaire
        '
        Me.ButtonPlusModeAuxiliaire.Location = New System.Drawing.Point(225, 232)
        Me.ButtonPlusModeAuxiliaire.Name = "ButtonPlusModeAuxiliaire"
        Me.ButtonPlusModeAuxiliaire.Size = New System.Drawing.Size(19, 20)
        Me.ButtonPlusModeAuxiliaire.TabIndex = 99
        Me.ButtonPlusModeAuxiliaire.Text = "+"
        Me.ButtonPlusModeAuxiliaire.UseVisualStyleBackColor = True
        '
        'ButtonMoinsModeAuxiliaire
        '
        Me.ButtonMoinsModeAuxiliaire.Location = New System.Drawing.Point(200, 232)
        Me.ButtonMoinsModeAuxiliaire.Name = "ButtonMoinsModeAuxiliaire"
        Me.ButtonMoinsModeAuxiliaire.Size = New System.Drawing.Size(19, 20)
        Me.ButtonMoinsModeAuxiliaire.TabIndex = 98
        Me.ButtonMoinsModeAuxiliaire.Text = "-"
        Me.ButtonMoinsModeAuxiliaire.UseVisualStyleBackColor = True
        '
        'ButtonPlusVitesseReponse
        '
        Me.ButtonPlusVitesseReponse.Location = New System.Drawing.Point(225, 165)
        Me.ButtonPlusVitesseReponse.Name = "ButtonPlusVitesseReponse"
        Me.ButtonPlusVitesseReponse.Size = New System.Drawing.Size(19, 20)
        Me.ButtonPlusVitesseReponse.TabIndex = 97
        Me.ButtonPlusVitesseReponse.Text = "+"
        Me.ButtonPlusVitesseReponse.UseVisualStyleBackColor = True
        '
        'ButtonMoinsVitesseReponse
        '
        Me.ButtonMoinsVitesseReponse.Location = New System.Drawing.Point(200, 165)
        Me.ButtonMoinsVitesseReponse.Name = "ButtonMoinsVitesseReponse"
        Me.ButtonMoinsVitesseReponse.Size = New System.Drawing.Size(19, 20)
        Me.ButtonMoinsVitesseReponse.TabIndex = 96
        Me.ButtonMoinsVitesseReponse.Text = "-"
        Me.ButtonMoinsVitesseReponse.UseVisualStyleBackColor = True
        '
        'LabelModifications
        '
        Me.LabelModifications.AutoSize = True
        Me.LabelModifications.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.LabelModifications.Enabled = False
        Me.LabelModifications.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelModifications.ForeColor = System.Drawing.Color.Red
        Me.LabelModifications.Location = New System.Drawing.Point(120, 278)
        Me.LabelModifications.MaximumSize = New System.Drawing.Size(300, 0)
        Me.LabelModifications.MinimumSize = New System.Drawing.Size(300, 0)
        Me.LabelModifications.Name = "LabelModifications"
        Me.LabelModifications.Size = New System.Drawing.Size(300, 19)
        Me.LabelModifications.TabIndex = 95
        Me.LabelModifications.Text = "..."
        Me.LabelModifications.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'ButtonAnnulerModif
        '
        Me.ButtonAnnulerModif.Location = New System.Drawing.Point(107, 301)
        Me.ButtonAnnulerModif.Name = "ButtonAnnulerModif"
        Me.ButtonAnnulerModif.Size = New System.Drawing.Size(117, 23)
        Me.ButtonAnnulerModif.TabIndex = 94
        Me.ButtonAnnulerModif.Text = "Annuler modifications"
        Me.ButtonAnnulerModif.UseVisualStyleBackColor = True
        '
        'CheckBoxInversionServo2
        '
        Me.CheckBoxInversionServo2.AutoSize = True
        Me.CheckBoxInversionServo2.Location = New System.Drawing.Point(346, 58)
        Me.CheckBoxInversionServo2.Name = "CheckBoxInversionServo2"
        Me.CheckBoxInversionServo2.Size = New System.Drawing.Size(46, 17)
        Me.CheckBoxInversionServo2.TabIndex = 93
        Me.CheckBoxInversionServo2.Text = "Non"
        Me.CheckBoxInversionServo2.UseVisualStyleBackColor = True
        '
        'CheckBoxInversionServo1
        '
        Me.CheckBoxInversionServo1.AutoSize = True
        Me.CheckBoxInversionServo1.Location = New System.Drawing.Point(346, 36)
        Me.CheckBoxInversionServo1.Name = "CheckBoxInversionServo1"
        Me.CheckBoxInversionServo1.Size = New System.Drawing.Size(46, 17)
        Me.CheckBoxInversionServo1.TabIndex = 92
        Me.CheckBoxInversionServo1.Text = "Non"
        Me.CheckBoxInversionServo1.UseVisualStyleBackColor = True
        '
        'TextVoltageExterne
        '
        Me.TextVoltageExterne.Location = New System.Drawing.Point(426, 124)
        Me.TextVoltageExterne.Name = "TextVoltageExterne"
        Me.TextVoltageExterne.Size = New System.Drawing.Size(46, 20)
        Me.TextVoltageExterne.TabIndex = 91
        Me.TextVoltageExterne.Text = "0 v"
        '
        'TextTempInterne
        '
        Me.TextTempInterne.Location = New System.Drawing.Point(361, 146)
        Me.TextTempInterne.Name = "TextTempInterne"
        Me.TextTempInterne.Size = New System.Drawing.Size(46, 20)
        Me.TextTempInterne.TabIndex = 90
        Me.TextTempInterne.Text = "0 °C"
        '
        'TextVoltageInterne
        '
        Me.TextVoltageInterne.Location = New System.Drawing.Point(331, 123)
        Me.TextVoltageInterne.Name = "TextVoltageInterne"
        Me.TextVoltageInterne.Size = New System.Drawing.Size(42, 20)
        Me.TextVoltageInterne.TabIndex = 89
        Me.TextVoltageInterne.Text = "0 v"
        '
        'textNombrePales
        '
        Me.textNombrePales.Location = New System.Drawing.Point(411, 100)
        Me.textNombrePales.Name = "textNombrePales"
        Me.textNombrePales.Size = New System.Drawing.Size(32, 20)
        Me.textNombrePales.TabIndex = 87
        Me.textNombrePales.Text = "2"
        '
        'textAuxiliaireMode
        '
        Me.textAuxiliaireMode.Location = New System.Drawing.Point(158, 233)
        Me.textAuxiliaireMode.Name = "textAuxiliaireMode"
        Me.textAuxiliaireMode.Size = New System.Drawing.Size(39, 20)
        Me.textAuxiliaireMode.TabIndex = 84
        Me.textAuxiliaireMode.Text = "0"
        '
        'textMaxiGenerale
        '
        Me.textMaxiGenerale.Location = New System.Drawing.Point(158, 56)
        Me.textMaxiGenerale.Name = "textMaxiGenerale"
        Me.textMaxiGenerale.Size = New System.Drawing.Size(39, 20)
        Me.textMaxiGenerale.TabIndex = 83
        Me.textMaxiGenerale.Text = "0"
        '
        'textMiniGenerale
        '
        Me.textMiniGenerale.Location = New System.Drawing.Point(158, 34)
        Me.textMiniGenerale.Name = "textMiniGenerale"
        Me.textMiniGenerale.Size = New System.Drawing.Size(39, 20)
        Me.textMiniGenerale.TabIndex = 82
        Me.textMiniGenerale.Text = "0"
        '
        'textDebutSynchro
        '
        Me.textDebutSynchro.Location = New System.Drawing.Point(158, 210)
        Me.textDebutSynchro.Name = "textDebutSynchro"
        Me.textDebutSynchro.Size = New System.Drawing.Size(39, 20)
        Me.textDebutSynchro.TabIndex = 81
        Me.textDebutSynchro.Text = "0"
        '
        'textMaxiMoteurs
        '
        Me.textMaxiMoteurs.Location = New System.Drawing.Point(158, 188)
        Me.textMaxiMoteurs.Name = "textMaxiMoteurs"
        Me.textMaxiMoteurs.Size = New System.Drawing.Size(39, 20)
        Me.textMaxiMoteurs.TabIndex = 80
        Me.textMaxiMoteurs.Text = "0"
        '
        'textTempsReponse
        '
        Me.textTempsReponse.Location = New System.Drawing.Point(158, 166)
        Me.textTempsReponse.Name = "textTempsReponse"
        Me.textTempsReponse.Size = New System.Drawing.Size(39, 20)
        Me.textTempsReponse.TabIndex = 79
        Me.textTempsReponse.Text = "0"
        '
        'textIdleServo2
        '
        Me.textIdleServo2.Location = New System.Drawing.Point(158, 144)
        Me.textIdleServo2.Name = "textIdleServo2"
        Me.textIdleServo2.Size = New System.Drawing.Size(39, 20)
        Me.textIdleServo2.TabIndex = 78
        Me.textIdleServo2.Text = "0"
        '
        'textIdleServo1
        '
        Me.textIdleServo1.Location = New System.Drawing.Point(158, 122)
        Me.textIdleServo1.Name = "textIdleServo1"
        Me.textIdleServo1.Size = New System.Drawing.Size(39, 20)
        Me.textIdleServo1.TabIndex = 77
        Me.textIdleServo1.Text = "0"
        '
        'LabelExternalVoltage
        '
        Me.LabelExternalVoltage.AutoSize = True
        Me.LabelExternalVoltage.Location = New System.Drawing.Point(379, 126)
        Me.LabelExternalVoltage.Name = "LabelExternalVoltage"
        Me.LabelExternalVoltage.Size = New System.Drawing.Size(46, 13)
        Me.LabelExternalVoltage.TabIndex = 74
        Me.LabelExternalVoltage.Text = "Externe:"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(249, 149)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(108, 13)
        Me.Label19.TabIndex = 73
        Me.Label19.Text = "Temperature interne :"
        '
        'labelInternalVoltage
        '
        Me.labelInternalVoltage.AutoSize = True
        Me.labelInternalVoltage.Location = New System.Drawing.Point(249, 126)
        Me.labelInternalVoltage.Name = "labelInternalVoltage"
        Me.labelInternalVoltage.Size = New System.Drawing.Size(82, 13)
        Me.labelInternalVoltage.TabIndex = 72
        Me.labelInternalVoltage.Text = "Voltage Interne:"
        '
        'labelNbrBlades
        '
        Me.labelNbrBlades.AutoSize = True
        Me.labelNbrBlades.Location = New System.Drawing.Point(249, 104)
        Me.labelNbrBlades.Name = "labelNbrBlades"
        Me.labelNbrBlades.Size = New System.Drawing.Size(152, 13)
        Me.labelNbrBlades.TabIndex = 70
        Me.labelNbrBlades.Text = "Nombre de pales ou d'aimants:"
        '
        'labelMode
        '
        Me.labelMode.AutoSize = True
        Me.labelMode.Location = New System.Drawing.Point(249, 81)
        Me.labelMode.Name = "labelMode"
        Me.labelMode.Size = New System.Drawing.Size(68, 13)
        Me.labelMode.TabIndex = 69
        Me.labelMode.Text = "Mode Radio:"
        '
        'labelReverseServo2
        '
        Me.labelReverseServo2.AutoSize = True
        Me.labelReverseServo2.Location = New System.Drawing.Point(249, 59)
        Me.labelReverseServo2.Name = "labelReverseServo2"
        Me.labelReverseServo2.Size = New System.Drawing.Size(88, 13)
        Me.labelReverseServo2.TabIndex = 67
        Me.labelReverseServo2.Text = "Inversion servo2:"
        '
        'labelReverseServo1
        '
        Me.labelReverseServo1.AutoSize = True
        Me.labelReverseServo1.Location = New System.Drawing.Point(249, 37)
        Me.labelReverseServo1.Name = "labelReverseServo1"
        Me.labelReverseServo1.Size = New System.Drawing.Size(88, 13)
        Me.labelReverseServo1.TabIndex = 66
        Me.labelReverseServo1.Text = "Inversion servo1:"
        '
        'labelAuxiMode
        '
        Me.labelAuxiMode.AutoSize = True
        Me.labelAuxiMode.Location = New System.Drawing.Point(23, 236)
        Me.labelAuxiMode.Name = "labelAuxiMode"
        Me.labelAuxiMode.Size = New System.Drawing.Size(130, 13)
        Me.labelAuxiMode.TabIndex = 65
        Me.labelAuxiMode.Text = "Connexion Ch AUX: mode"
        '
        'labelPosMaxiGene
        '
        Me.labelPosMaxiGene.AutoSize = True
        Me.labelPosMaxiGene.Location = New System.Drawing.Point(24, 59)
        Me.labelPosMaxiGene.Name = "labelPosMaxiGene"
        Me.labelPosMaxiGene.Size = New System.Drawing.Size(115, 13)
        Me.labelPosMaxiGene.TabIndex = 64
        Me.labelPosMaxiGene.Text = "Position maxi generale:"
        '
        'labelPosMiniGene
        '
        Me.labelPosMiniGene.AutoSize = True
        Me.labelPosMiniGene.Location = New System.Drawing.Point(24, 37)
        Me.labelPosMiniGene.Name = "labelPosMiniGene"
        Me.labelPosMiniGene.Size = New System.Drawing.Size(112, 13)
        Me.labelPosMiniGene.TabIndex = 63
        Me.labelPosMiniGene.Text = "Position mini generale:"
        '
        'labelStartSynchroServos
        '
        Me.labelStartSynchroServos.AutoSize = True
        Me.labelStartSynchroServos.Location = New System.Drawing.Point(23, 213)
        Me.labelStartSynchroServos.Name = "labelStartSynchroServos"
        Me.labelStartSynchroServos.Size = New System.Drawing.Size(113, 13)
        Me.labelStartSynchroServos.TabIndex = 62
        Me.labelStartSynchroServos.Text = "Début synchro servos:"
        '
        'labelMaxiServos
        '
        Me.labelMaxiServos.AutoSize = True
        Me.labelMaxiServos.Location = New System.Drawing.Point(23, 191)
        Me.labelMaxiServos.Name = "labelMaxiServos"
        Me.labelMaxiServos.Size = New System.Drawing.Size(124, 13)
        Me.labelMaxiServos.TabIndex = 61
        Me.labelMaxiServos.Text = "Position Maxi Servos: µS"
        '
        'labelSpeedModuleAnswer
        '
        Me.labelSpeedModuleAnswer.AutoSize = True
        Me.labelSpeedModuleAnswer.Location = New System.Drawing.Point(23, 169)
        Me.labelSpeedModuleAnswer.Name = "labelSpeedModuleAnswer"
        Me.labelSpeedModuleAnswer.Size = New System.Drawing.Size(100, 13)
        Me.labelSpeedModuleAnswer.TabIndex = 60
        Me.labelSpeedModuleAnswer.Text = "Vitesse de reponse:"
        '
        'labelIdleServo2
        '
        Me.labelIdleServo2.AutoSize = True
        Me.labelIdleServo2.Location = New System.Drawing.Point(23, 147)
        Me.labelIdleServo2.Name = "labelIdleServo2"
        Me.labelIdleServo2.Size = New System.Drawing.Size(110, 13)
        Me.labelIdleServo2.TabIndex = 59
        Me.labelIdleServo2.Text = "Position Idle servos 2:"
        '
        'labelIdleServo1
        '
        Me.labelIdleServo1.AutoSize = True
        Me.labelIdleServo1.Location = New System.Drawing.Point(23, 125)
        Me.labelIdleServo1.Name = "labelIdleServo1"
        Me.labelIdleServo1.Size = New System.Drawing.Size(110, 13)
        Me.labelIdleServo1.TabIndex = 58
        Me.labelIdleServo1.Text = "Position Idle servos 1:"
        '
        'ButtonSauvegardeConfig
        '
        Me.ButtonSauvegardeConfig.Location = New System.Drawing.Point(352, 301)
        Me.ButtonSauvegardeConfig.Name = "ButtonSauvegardeConfig"
        Me.ButtonSauvegardeConfig.Size = New System.Drawing.Size(117, 23)
        Me.ButtonSauvegardeConfig.TabIndex = 55
        Me.ButtonSauvegardeConfig.Text = "Sauver Configuration"
        Me.ButtonSauvegardeConfig.UseVisualStyleBackColor = True
        '
        'labelConfigModule
        '
        Me.labelConfigModule.AutoSize = True
        Me.labelConfigModule.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelConfigModule.Location = New System.Drawing.Point(148, 5)
        Me.labelConfigModule.Name = "labelConfigModule"
        Me.labelConfigModule.Size = New System.Drawing.Size(286, 17)
        Me.labelConfigModule.TabIndex = 54
        Me.labelConfigModule.Text = "Editeur de la configuration du module:"
        '
        'TimerRXAuxiliaire
        '
        Me.TimerRXAuxiliaire.Interval = 300
        '
        'TimerRXMotors
        '
        Me.TimerRXMotors.Interval = 300
        '
        'TimerHardwareInfos
        '
        Me.TimerHardwareInfos.Interval = 1000
        '
        'GroupBoxMoteurs
        '
        Me.GroupBoxMoteurs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBoxMoteurs.Controls.Add(Me.SevenSegmentArray2)
        Me.GroupBoxMoteurs.Controls.Add(Me.SevenSegmentArray1)
        Me.GroupBoxMoteurs.Controls.Add(Me.AquaGaugeMoteur2)
        Me.GroupBoxMoteurs.Controls.Add(Me.AquaGaugeMoteur1)
        Me.GroupBoxMoteurs.Controls.Add(Me.LabelVitesse2)
        Me.GroupBoxMoteurs.Controls.Add(Me.LabelVitesse1)
        Me.GroupBoxMoteurs.Location = New System.Drawing.Point(0, 27)
        Me.GroupBoxMoteurs.Name = "GroupBoxMoteurs"
        Me.GroupBoxMoteurs.Size = New System.Drawing.Size(597, 336)
        Me.GroupBoxMoteurs.TabIndex = 117
        Me.GroupBoxMoteurs.TabStop = False
        Me.GroupBoxMoteurs.Text = "Moteurs"
        '
        'SevenSegmentArray2
        '
        Me.SevenSegmentArray2.ArrayCount = 5
        Me.SevenSegmentArray2.ColorBackground = System.Drawing.Color.Black
        Me.SevenSegmentArray2.ColorDark = System.Drawing.Color.DimGray
        Me.SevenSegmentArray2.ColorLight = System.Drawing.Color.Red
        Me.SevenSegmentArray2.DecimalShow = True
        Me.SevenSegmentArray2.ElementPadding = New System.Windows.Forms.Padding(4)
        Me.SevenSegmentArray2.ElementWidth = 10
        Me.SevenSegmentArray2.ItalicFactor = 0.0!
        Me.SevenSegmentArray2.Location = New System.Drawing.Point(1, 212)
        Me.SevenSegmentArray2.Name = "SevenSegmentArray2"
        Me.SevenSegmentArray2.Size = New System.Drawing.Size(594, 120)
        Me.SevenSegmentArray2.TabIndex = 128
        Me.SevenSegmentArray2.TabStop = False
        Me.SevenSegmentArray2.Value = Nothing
        '
        'SevenSegmentArray1
        '
        Me.SevenSegmentArray1.ArrayCount = 5
        Me.SevenSegmentArray1.ColorBackground = System.Drawing.Color.Black
        Me.SevenSegmentArray1.ColorDark = System.Drawing.Color.DimGray
        Me.SevenSegmentArray1.ColorLight = System.Drawing.Color.Red
        Me.SevenSegmentArray1.DecimalShow = True
        Me.SevenSegmentArray1.ElementPadding = New System.Windows.Forms.Padding(4)
        Me.SevenSegmentArray1.ElementWidth = 10
        Me.SevenSegmentArray1.ItalicFactor = 0.0!
        Me.SevenSegmentArray1.Location = New System.Drawing.Point(0, 61)
        Me.SevenSegmentArray1.Name = "SevenSegmentArray1"
        Me.SevenSegmentArray1.Size = New System.Drawing.Size(594, 120)
        Me.SevenSegmentArray1.TabIndex = 127
        Me.SevenSegmentArray1.TabStop = False
        Me.SevenSegmentArray1.Value = Nothing
        '
        'AquaGaugeMoteur2
        '
        Me.AquaGaugeMoteur2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AquaGaugeMoteur2.AutoSize = True
        Me.AquaGaugeMoteur2.BackColor = System.Drawing.Color.Transparent
        Me.AquaGaugeMoteur2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.AquaGaugeMoteur2.DecimalPlaces = 0
        Me.AquaGaugeMoteur2.DialAlpha = 255
        Me.AquaGaugeMoteur2.DialBorderColor = System.Drawing.Color.Gray
        Me.AquaGaugeMoteur2.DialColor = System.Drawing.Color.Black
        Me.AquaGaugeMoteur2.DialText = Nothing
        Me.AquaGaugeMoteur2.DialTextColor = System.Drawing.Color.Gold
        Me.AquaGaugeMoteur2.DialTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AquaGaugeMoteur2.DialTextVOffset = 0
        Me.AquaGaugeMoteur2.DigitalValue = 0.0!
        Me.AquaGaugeMoteur2.DigitalValueBackAlpha = 1
        Me.AquaGaugeMoteur2.DigitalValueBackColor = System.Drawing.Color.White
        Me.AquaGaugeMoteur2.DigitalValueColor = System.Drawing.Color.Orange
        Me.AquaGaugeMoteur2.DigitalValueDecimalPlaces = 0
        Me.AquaGaugeMoteur2.Glossiness = 40.0!
        Me.AquaGaugeMoteur2.Location = New System.Drawing.Point(302, 11)
        Me.AquaGaugeMoteur2.MaxValue = 20000.0!
        Me.AquaGaugeMoteur2.MinValue = 0.0!
        Me.AquaGaugeMoteur2.Name = "AquaGaugeMoteur2"
        Me.AquaGaugeMoteur2.NoOfSubDivisions = 1
        Me.AquaGaugeMoteur2.PointerColor = System.Drawing.Color.Black
        Me.AquaGaugeMoteur2.RimAlpha = 255
        Me.AquaGaugeMoteur2.RimColor = System.Drawing.Color.Gold
        Me.AquaGaugeMoteur2.ScaleColor = System.Drawing.Color.Gold
        Me.AquaGaugeMoteur2.ScaleFontSizeDivider = 25
        Me.AquaGaugeMoteur2.Size = New System.Drawing.Size(287, 287)
        Me.AquaGaugeMoteur2.TabIndex = 118
        Me.AquaGaugeMoteur2.Threshold1Color = System.Drawing.Color.LawnGreen
        Me.AquaGaugeMoteur2.Threshold1Start = 0.0!
        Me.AquaGaugeMoteur2.Threshold1Stop = 2000.0!
        Me.AquaGaugeMoteur2.Threshold2Color = System.Drawing.Color.Red
        Me.AquaGaugeMoteur2.Threshold2Start = 16000.0!
        Me.AquaGaugeMoteur2.Threshold2Stop = 20000.0!
        Me.AquaGaugeMoteur2.Value = 0.0!
        Me.AquaGaugeMoteur2.ValueToDigital = True
        '
        'AquaGaugeMoteur1
        '
        Me.AquaGaugeMoteur1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AquaGaugeMoteur1.AutoSize = True
        Me.AquaGaugeMoteur1.BackColor = System.Drawing.Color.Transparent
        Me.AquaGaugeMoteur1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.AquaGaugeMoteur1.DecimalPlaces = 0
        Me.AquaGaugeMoteur1.DialAlpha = 255
        Me.AquaGaugeMoteur1.DialBorderColor = System.Drawing.Color.Gray
        Me.AquaGaugeMoteur1.DialColor = System.Drawing.Color.Black
        Me.AquaGaugeMoteur1.DialText = Nothing
        Me.AquaGaugeMoteur1.DialTextColor = System.Drawing.Color.Gold
        Me.AquaGaugeMoteur1.DialTextFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.AquaGaugeMoteur1.DialTextVOffset = 0
        Me.AquaGaugeMoteur1.DigitalValue = 0.0!
        Me.AquaGaugeMoteur1.DigitalValueBackAlpha = 1
        Me.AquaGaugeMoteur1.DigitalValueBackColor = System.Drawing.Color.White
        Me.AquaGaugeMoteur1.DigitalValueColor = System.Drawing.Color.Orange
        Me.AquaGaugeMoteur1.DigitalValueDecimalPlaces = 0
        Me.AquaGaugeMoteur1.Glossiness = 40.0!
        Me.AquaGaugeMoteur1.Location = New System.Drawing.Point(6, 11)
        Me.AquaGaugeMoteur1.MaxValue = 20000.0!
        Me.AquaGaugeMoteur1.MinValue = 0.0!
        Me.AquaGaugeMoteur1.Name = "AquaGaugeMoteur1"
        Me.AquaGaugeMoteur1.NoOfSubDivisions = 1
        Me.AquaGaugeMoteur1.PointerColor = System.Drawing.Color.Black
        Me.AquaGaugeMoteur1.RimAlpha = 255
        Me.AquaGaugeMoteur1.RimColor = System.Drawing.Color.Gold
        Me.AquaGaugeMoteur1.ScaleColor = System.Drawing.Color.Gold
        Me.AquaGaugeMoteur1.ScaleFontSizeDivider = 25
        Me.AquaGaugeMoteur1.Size = New System.Drawing.Size(287, 287)
        Me.AquaGaugeMoteur1.TabIndex = 117
        Me.AquaGaugeMoteur1.Threshold1Color = System.Drawing.Color.LawnGreen
        Me.AquaGaugeMoteur1.Threshold1Start = 0.0!
        Me.AquaGaugeMoteur1.Threshold1Stop = 2000.0!
        Me.AquaGaugeMoteur1.Threshold2Color = System.Drawing.Color.Red
        Me.AquaGaugeMoteur1.Threshold2Start = 16000.0!
        Me.AquaGaugeMoteur1.Threshold2Stop = 20000.0!
        Me.AquaGaugeMoteur1.Value = 0.0!
        Me.AquaGaugeMoteur1.ValueToDigital = True
        '
        'LabelVitesse2
        '
        Me.LabelVitesse2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.LabelVitesse2.AutoSize = True
        Me.LabelVitesse2.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelVitesse2.ForeColor = System.Drawing.Color.Blue
        Me.LabelVitesse2.Location = New System.Drawing.Point(299, 301)
        Me.LabelVitesse2.Name = "LabelVitesse2"
        Me.LabelVitesse2.Size = New System.Drawing.Size(117, 25)
        Me.LabelVitesse2.TabIndex = 126
        Me.LabelVitesse2.Text = "Vitesse 2:"
        '
        'LabelVitesse1
        '
        Me.LabelVitesse1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.LabelVitesse1.AutoSize = True
        Me.LabelVitesse1.Font = New System.Drawing.Font("Microsoft Sans Serif", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelVitesse1.ForeColor = System.Drawing.Color.Red
        Me.LabelVitesse1.Location = New System.Drawing.Point(6, 301)
        Me.LabelVitesse1.Name = "LabelVitesse1"
        Me.LabelVitesse1.Size = New System.Drawing.Size(117, 25)
        Me.LabelVitesse1.TabIndex = 125
        Me.LabelVitesse1.Text = "Vitesse 1:"
        '
        'zg1
        '
        Me.zg1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.zg1.Location = New System.Drawing.Point(7, 19)
        Me.zg1.Name = "zg1"
        Me.zg1.ScrollGrace = 0.0R
        Me.zg1.ScrollMaxX = 0.0R
        Me.zg1.ScrollMaxY = 0.0R
        Me.zg1.ScrollMaxY2 = 0.0R
        Me.zg1.ScrollMinX = 0.0R
        Me.zg1.ScrollMinY = 0.0R
        Me.zg1.ScrollMinY2 = 0.0R
        Me.zg1.Size = New System.Drawing.Size(578, 335)
        Me.zg1.TabIndex = 2
        '
        'TimerDataLogger
        '
        Me.TimerDataLogger.Interval = 1000
        '
        'SerialPort1
        '
        '
        'optLangFR
        '
        Me.optLangFR.AutoSize = True
        Me.optLangFR.Location = New System.Drawing.Point(46, 19)
        Me.optLangFR.Name = "optLangFR"
        Me.optLangFR.Size = New System.Drawing.Size(58, 17)
        Me.optLangFR.TabIndex = 127
        Me.optLangFR.TabStop = True
        Me.optLangFR.Text = "French"
        Me.optLangFR.UseVisualStyleBackColor = True
        '
        'optLangEN
        '
        Me.optLangEN.AutoSize = True
        Me.optLangEN.Location = New System.Drawing.Point(46, 46)
        Me.optLangEN.Name = "optLangEN"
        Me.optLangEN.Size = New System.Drawing.Size(59, 17)
        Me.optLangEN.TabIndex = 128
        Me.optLangEN.TabStop = True
        Me.optLangEN.Text = "English"
        Me.optLangEN.UseVisualStyleBackColor = True
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(6, 16)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(34, 20)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 129
        Me.PictureBox4.TabStop = False
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(6, 43)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(34, 20)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox5.TabIndex = 130
        Me.PictureBox5.TabStop = False
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Controls.Add(Me.TabPage5)
        Me.TabControl1.Controls.Add(Me.TabPage6)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Controls.Add(Me.TabPage8)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(605, 392)
        Me.TabControl1.TabIndex = 131
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.ButtonTerminal)
        Me.TabPage1.Controls.Add(Me.labelInfoNeedSaveCOM)
        Me.TabPage1.Controls.Add(Me.GroupBoxLang)
        Me.TabPage1.Controls.Add(Me.GroupBoxSerialPort)
        Me.TabPage1.Controls.Add(Me.ButtonAbout)
        Me.TabPage1.Controls.Add(Me.PictureBox1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(597, 366)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Connection"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'ButtonTerminal
        '
        Me.ButtonTerminal.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonTerminal.Location = New System.Drawing.Point(527, 337)
        Me.ButtonTerminal.Name = "ButtonTerminal"
        Me.ButtonTerminal.Size = New System.Drawing.Size(64, 23)
        Me.ButtonTerminal.TabIndex = 135
        Me.ButtonTerminal.Text = "Terminal"
        Me.ButtonTerminal.UseVisualStyleBackColor = True
        '
        'labelInfoNeedSaveCOM
        '
        Me.labelInfoNeedSaveCOM.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.labelInfoNeedSaveCOM.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelInfoNeedSaveCOM.Location = New System.Drawing.Point(4, 87)
        Me.labelInfoNeedSaveCOM.Name = "labelInfoNeedSaveCOM"
        Me.labelInfoNeedSaveCOM.Size = New System.Drawing.Size(590, 26)
        Me.labelInfoNeedSaveCOM.TabIndex = 134
        Me.labelInfoNeedSaveCOM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'GroupBoxLang
        '
        Me.GroupBoxLang.Controls.Add(Me.PictureBox4)
        Me.GroupBoxLang.Controls.Add(Me.optLangFR)
        Me.GroupBoxLang.Controls.Add(Me.PictureBox5)
        Me.GroupBoxLang.Controls.Add(Me.optLangEN)
        Me.GroupBoxLang.Location = New System.Drawing.Point(385, 3)
        Me.GroupBoxLang.Name = "GroupBoxLang"
        Me.GroupBoxLang.Size = New System.Drawing.Size(119, 76)
        Me.GroupBoxLang.TabIndex = 131
        Me.GroupBoxLang.TabStop = False
        Me.GroupBoxLang.Text = "Language"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.GroupBoxSettings)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(597, 366)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Settings"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.labelMotor2)
        Me.TabPage3.Controls.Add(Me.CheckBoxAnaDigi)
        Me.TabPage3.Controls.Add(Me.ButtonReadSpeeds)
        Me.TabPage3.Controls.Add(Me.labelMotor1)
        Me.TabPage3.Controls.Add(Me.PictureBoxTimer1OnOff)
        Me.TabPage3.Controls.Add(Me.GroupBoxMoteurs)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(597, 366)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Motors"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'labelMotor2
        '
        Me.labelMotor2.AutoSize = True
        Me.labelMotor2.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelMotor2.Location = New System.Drawing.Point(406, 11)
        Me.labelMotor2.Name = "labelMotor2"
        Me.labelMotor2.Size = New System.Drawing.Size(72, 17)
        Me.labelMotor2.TabIndex = 130
        Me.labelMotor2.Text = "Moteur 2"
        '
        'CheckBoxAnaDigi
        '
        Me.CheckBoxAnaDigi.AutoSize = True
        Me.CheckBoxAnaDigi.Location = New System.Drawing.Point(11, 10)
        Me.CheckBoxAnaDigi.Name = "CheckBoxAnaDigi"
        Me.CheckBoxAnaDigi.Size = New System.Drawing.Size(63, 17)
        Me.CheckBoxAnaDigi.TabIndex = 129
        Me.CheckBoxAnaDigi.Text = "AnaDigi"
        Me.CheckBoxAnaDigi.UseVisualStyleBackColor = True
        '
        'ButtonReadSpeeds
        '
        Me.ButtonReadSpeeds.Location = New System.Drawing.Point(270, 7)
        Me.ButtonReadSpeeds.Name = "ButtonReadSpeeds"
        Me.ButtonReadSpeeds.Size = New System.Drawing.Size(54, 23)
        Me.ButtonReadSpeeds.TabIndex = 127
        Me.ButtonReadSpeeds.Text = "Vitesse"
        Me.ButtonReadSpeeds.UseVisualStyleBackColor = True
        '
        'labelMotor1
        '
        Me.labelMotor1.AutoSize = True
        Me.labelMotor1.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelMotor1.Location = New System.Drawing.Point(115, 11)
        Me.labelMotor1.Name = "labelMotor1"
        Me.labelMotor1.Size = New System.Drawing.Size(72, 17)
        Me.labelMotor1.TabIndex = 129
        Me.labelMotor1.Text = "Moteur 1"
        '
        'PictureBoxTimer1OnOff
        '
        Me.PictureBoxTimer1OnOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxTimer1OnOff.Location = New System.Drawing.Point(325, 11)
        Me.PictureBoxTimer1OnOff.Name = "PictureBoxTimer1OnOff"
        Me.PictureBoxTimer1OnOff.Size = New System.Drawing.Size(13, 16)
        Me.PictureBoxTimer1OnOff.TabIndex = 128
        Me.PictureBoxTimer1OnOff.TabStop = False
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.DataLogger)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Size = New System.Drawing.Size(597, 366)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "Data Log"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'DataLogger
        '
        Me.DataLogger.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataLogger.Controls.Add(Me.zg1)
        Me.DataLogger.Location = New System.Drawing.Point(3, 3)
        Me.DataLogger.Name = "DataLogger"
        Me.DataLogger.Size = New System.Drawing.Size(591, 360)
        Me.DataLogger.TabIndex = 119
        Me.DataLogger.TabStop = False
        Me.DataLogger.Text = "Data Log"
        '
        'TabPage5
        '
        Me.TabPage5.Controls.Add(Me.LabelTestNow)
        Me.TabPage5.Controls.Add(Me.LabelChrono)
        Me.TabPage5.Controls.Add(Me.RichTextSimulationHelp)
        Me.TabPage5.Controls.Add(Me.BoxRecorder)
        Me.TabPage5.Controls.Add(Me.ButtonSimulationHelp)
        Me.TabPage5.Controls.Add(Me.GroupSpeedPID)
        Me.TabPage5.Controls.Add(Me.GroupBox1)
        Me.TabPage5.Controls.Add(Me.GroupSpeedSimu)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Size = New System.Drawing.Size(597, 366)
        Me.TabPage5.TabIndex = 4
        Me.TabPage5.Text = "Simulation"
        Me.TabPage5.UseVisualStyleBackColor = True
        '
        'LabelTestNow
        '
        Me.LabelTestNow.AutoSize = True
        Me.LabelTestNow.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelTestNow.Location = New System.Drawing.Point(296, 338)
        Me.LabelTestNow.Name = "LabelTestNow"
        Me.LabelTestNow.Size = New System.Drawing.Size(21, 22)
        Me.LabelTestNow.TabIndex = 144
        Me.LabelTestNow.Text = "0"
        Me.LabelTestNow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LabelChrono
        '
        Me.LabelChrono.AutoSize = True
        Me.LabelChrono.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelChrono.Location = New System.Drawing.Point(69, 338)
        Me.LabelChrono.Name = "LabelChrono"
        Me.LabelChrono.Size = New System.Drawing.Size(21, 22)
        Me.LabelChrono.TabIndex = 144
        Me.LabelChrono.Text = "0"
        Me.LabelChrono.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'RichTextSimulationHelp
        '
        Me.RichTextSimulationHelp.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.RichTextSimulationHelp.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RichTextSimulationHelp.Location = New System.Drawing.Point(4, 334)
        Me.RichTextSimulationHelp.Name = "RichTextSimulationHelp"
        Me.RichTextSimulationHelp.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.RichTextSimulationHelp.Size = New System.Drawing.Size(19, 29)
        Me.RichTextSimulationHelp.TabIndex = 159
        Me.RichTextSimulationHelp.Text = "Simu"
        '
        'BoxRecorder
        '
        Me.BoxRecorder.Controls.Add(Me.CheckBoxReadTracBarMotorOrMotorThrottle)
        Me.BoxRecorder.Controls.Add(Me.CheckBoxChronoOnOff)
        Me.BoxRecorder.Controls.Add(Me.Label16)
        Me.BoxRecorder.Controls.Add(Me.LabelChronoSS)
        Me.BoxRecorder.Controls.Add(Me.Label14)
        Me.BoxRecorder.Controls.Add(Me.TextBoxChronoMM)
        Me.BoxRecorder.Controls.Add(Me.LabelChronoSimu)
        Me.BoxRecorder.Controls.Add(Me.TextBoxChronoHH)
        Me.BoxRecorder.Controls.Add(Me.PictureBox6)
        Me.BoxRecorder.Controls.Add(Me.PictureBoxPlayer)
        Me.BoxRecorder.Controls.Add(Me.ButtonPlayer)
        Me.BoxRecorder.Controls.Add(Me.PictureBoxRecorder)
        Me.BoxRecorder.Controls.Add(Me.ButtonRecorder)
        Me.BoxRecorder.Location = New System.Drawing.Point(394, 187)
        Me.BoxRecorder.Name = "BoxRecorder"
        Me.BoxRecorder.Size = New System.Drawing.Size(200, 148)
        Me.BoxRecorder.TabIndex = 158
        Me.BoxRecorder.TabStop = False
        Me.BoxRecorder.Text = "Recorder / Player Movements"
        '
        'CheckBoxReadTracBarMotorOrMotorThrottle
        '
        Me.CheckBoxReadTracBarMotorOrMotorThrottle.AutoSize = True
        Me.CheckBoxReadTracBarMotorOrMotorThrottle.Location = New System.Drawing.Point(10, 21)
        Me.CheckBoxReadTracBarMotorOrMotorThrottle.Name = "CheckBoxReadTracBarMotorOrMotorThrottle"
        Me.CheckBoxReadTracBarMotorOrMotorThrottle.Size = New System.Drawing.Size(130, 17)
        Me.CheckBoxReadTracBarMotorOrMotorThrottle.TabIndex = 164
        Me.CheckBoxReadTracBarMotorOrMotorThrottle.Text = "Read 'Motors Throttle'"
        Me.CheckBoxReadTracBarMotorOrMotorThrottle.UseVisualStyleBackColor = True
        '
        'CheckBoxChronoOnOff
        '
        Me.CheckBoxChronoOnOff.AutoSize = True
        Me.CheckBoxChronoOnOff.Location = New System.Drawing.Point(157, 121)
        Me.CheckBoxChronoOnOff.Name = "CheckBoxChronoOnOff"
        Me.CheckBoxChronoOnOff.Size = New System.Drawing.Size(40, 17)
        Me.CheckBoxChronoOnOff.TabIndex = 153
        Me.CheckBoxChronoOnOff.Text = "On"
        Me.CheckBoxChronoOnOff.UseVisualStyleBackColor = True
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(122, 119)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(11, 15)
        Me.Label16.TabIndex = 163
        Me.Label16.Text = ":"
        '
        'LabelChronoSS
        '
        Me.LabelChronoSS.AutoSize = True
        Me.LabelChronoSS.Location = New System.Drawing.Point(130, 121)
        Me.LabelChronoSS.Name = "LabelChronoSS"
        Me.LabelChronoSS.Size = New System.Drawing.Size(21, 13)
        Me.LabelChronoSS.TabIndex = 162
        Me.LabelChronoSS.Text = "SS"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(87, 119)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(11, 15)
        Me.Label14.TabIndex = 161
        Me.Label14.Text = ":"
        '
        'TextBoxChronoMM
        '
        Me.TextBoxChronoMM.Location = New System.Drawing.Point(98, 118)
        Me.TextBoxChronoMM.Name = "TextBoxChronoMM"
        Me.TextBoxChronoMM.Size = New System.Drawing.Size(24, 20)
        Me.TextBoxChronoMM.TabIndex = 160
        Me.TextBoxChronoMM.Text = "30"
        Me.TextBoxChronoMM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelChronoSimu
        '
        Me.LabelChronoSimu.AutoSize = True
        Me.LabelChronoSimu.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelChronoSimu.Location = New System.Drawing.Point(6, 119)
        Me.LabelChronoSimu.Name = "LabelChronoSimu"
        Me.LabelChronoSimu.Size = New System.Drawing.Size(57, 15)
        Me.LabelChronoSimu.TabIndex = 157
        Me.LabelChronoSimu.Text = "Chrono:"
        '
        'TextBoxChronoHH
        '
        Me.TextBoxChronoHH.Location = New System.Drawing.Point(64, 118)
        Me.TextBoxChronoHH.Name = "TextBoxChronoHH"
        Me.TextBoxChronoHH.Size = New System.Drawing.Size(23, 20)
        Me.TextBoxChronoHH.TabIndex = 157
        Me.TextBoxChronoHH.Text = "5"
        Me.TextBoxChronoHH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'PictureBox6
        '
        Me.PictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox6.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources.Servo
        Me.PictureBox6.Location = New System.Drawing.Point(143, 46)
        Me.PictureBox6.Name = "PictureBox6"
        Me.PictureBox6.Size = New System.Drawing.Size(51, 65)
        Me.PictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox6.TabIndex = 144
        Me.PictureBox6.TabStop = False
        '
        'PictureBoxPlayer
        '
        Me.PictureBoxPlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxPlayer.Location = New System.Drawing.Point(10, 81)
        Me.PictureBoxPlayer.Name = "PictureBoxPlayer"
        Me.PictureBoxPlayer.Size = New System.Drawing.Size(13, 16)
        Me.PictureBoxPlayer.TabIndex = 158
        Me.PictureBoxPlayer.TabStop = False
        '
        'ButtonPlayer
        '
        Me.ButtonPlayer.Location = New System.Drawing.Point(29, 79)
        Me.ButtonPlayer.Name = "ButtonPlayer"
        Me.ButtonPlayer.Size = New System.Drawing.Size(93, 22)
        Me.ButtonPlayer.TabIndex = 159
        Me.ButtonPlayer.Text = "Start Player"
        Me.ButtonPlayer.UseVisualStyleBackColor = True
        '
        'PictureBoxRecorder
        '
        Me.PictureBoxRecorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxRecorder.Location = New System.Drawing.Point(10, 48)
        Me.PictureBoxRecorder.Name = "PictureBoxRecorder"
        Me.PictureBoxRecorder.Size = New System.Drawing.Size(13, 16)
        Me.PictureBoxRecorder.TabIndex = 144
        Me.PictureBoxRecorder.TabStop = False
        '
        'ButtonRecorder
        '
        Me.ButtonRecorder.Location = New System.Drawing.Point(29, 46)
        Me.ButtonRecorder.Name = "ButtonRecorder"
        Me.ButtonRecorder.Size = New System.Drawing.Size(93, 22)
        Me.ButtonRecorder.TabIndex = 157
        Me.ButtonRecorder.Text = "Start Recoder"
        Me.ButtonRecorder.UseVisualStyleBackColor = True
        '
        'ButtonSimulationHelp
        '
        Me.ButtonSimulationHelp.Location = New System.Drawing.Point(566, 341)
        Me.ButtonSimulationHelp.Name = "ButtonSimulationHelp"
        Me.ButtonSimulationHelp.Size = New System.Drawing.Size(27, 22)
        Me.ButtonSimulationHelp.TabIndex = 157
        Me.ButtonSimulationHelp.Text = "?"
        Me.ButtonSimulationHelp.UseVisualStyleBackColor = True
        '
        'GroupSpeedPID
        '
        Me.GroupSpeedPID.Controls.Add(Me.ButtonSavePIDVariables)
        Me.GroupSpeedPID.Controls.Add(Me.Label10)
        Me.GroupSpeedPID.Controls.Add(Me.Label7)
        Me.GroupSpeedPID.Controls.Add(Me.Label5)
        Me.GroupSpeedPID.Controls.Add(Me.textBoxKdControl)
        Me.GroupSpeedPID.Controls.Add(Me.textBoxKiControl)
        Me.GroupSpeedPID.Controls.Add(Me.textBoxKpControl)
        Me.GroupSpeedPID.Location = New System.Drawing.Point(394, 102)
        Me.GroupSpeedPID.Name = "GroupSpeedPID"
        Me.GroupSpeedPID.Size = New System.Drawing.Size(200, 79)
        Me.GroupSpeedPID.TabIndex = 24
        Me.GroupSpeedPID.TabStop = False
        Me.GroupSpeedPID.Text = "PID Control"
        '
        'ButtonSavePIDVariables
        '
        Me.ButtonSavePIDVariables.Location = New System.Drawing.Point(39, 46)
        Me.ButtonSavePIDVariables.Name = "ButtonSavePIDVariables"
        Me.ButtonSavePIDVariables.Size = New System.Drawing.Size(124, 22)
        Me.ButtonSavePIDVariables.TabIndex = 152
        Me.ButtonSavePIDVariables.Text = "Send Control Variables"
        Me.ButtonSavePIDVariables.UseVisualStyleBackColor = True
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(135, 20)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(28, 15)
        Me.Label10.TabIndex = 156
        Me.Label10.Text = "Kd:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(72, 20)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(24, 15)
        Me.Label7.TabIndex = 155
        Me.Label7.Text = "Ki:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 20)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(28, 15)
        Me.Label5.TabIndex = 152
        Me.Label5.Text = "Kp:"
        '
        'textBoxKdControl
        '
        Me.textBoxKdControl.Location = New System.Drawing.Point(163, 19)
        Me.textBoxKdControl.Name = "textBoxKdControl"
        Me.textBoxKdControl.Size = New System.Drawing.Size(32, 20)
        Me.textBoxKdControl.TabIndex = 154
        Me.textBoxKdControl.Text = "0"
        Me.textBoxKdControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'textBoxKiControl
        '
        Me.textBoxKiControl.Location = New System.Drawing.Point(100, 20)
        Me.textBoxKiControl.Name = "textBoxKiControl"
        Me.textBoxKiControl.Size = New System.Drawing.Size(32, 20)
        Me.textBoxKiControl.TabIndex = 153
        Me.textBoxKiControl.Text = "0"
        Me.textBoxKiControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'textBoxKpControl
        '
        Me.textBoxKpControl.Location = New System.Drawing.Point(34, 19)
        Me.textBoxKpControl.Name = "textBoxKpControl"
        Me.textBoxKpControl.Size = New System.Drawing.Size(32, 20)
        Me.textBoxKpControl.TabIndex = 152
        Me.textBoxKpControl.Text = "0"
        Me.textBoxKpControl.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.LabelOnSimuMoveThrottle)
        Me.GroupBox1.Controls.Add(Me.TrackBarRudder)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.UcV_ProgressBar1)
        Me.GroupBox1.Controls.Add(Me.UcV_ProgressBar2)
        Me.GroupBox1.Controls.Add(Me.labelAuxRudderSimulation)
        Me.GroupBox1.Controls.Add(Me.PictureBoxGlowPlugOnOff)
        Me.GroupBox1.Controls.Add(Me.ButtonServoTest)
        Me.GroupBox1.Controls.Add(Me.ButtonGlowPlugOnOff)
        Me.GroupBox1.Controls.Add(Me.PictureBoxGlowPlug)
        Me.GroupBox1.Controls.Add(Me.ProgressBarAuxiliary)
        Me.GroupBox1.Controls.Add(Me.PictureBoxAuxMaxi)
        Me.GroupBox1.Controls.Add(Me.PictureBoxAuxMiddle)
        Me.GroupBox1.Controls.Add(Me.PictureBoxAuxMini)
        Me.GroupBox1.Controls.Add(Me.ButtonAuxMaxi)
        Me.GroupBox1.Controls.Add(Me.ButtonAuxMiddle)
        Me.GroupBox1.Controls.Add(Me.ButtonAuxMini)
        Me.GroupBox1.Controls.Add(Me.ProgressBarThrottle)
        Me.GroupBox1.Controls.Add(Me.LabelAux)
        Me.GroupBox1.Controls.Add(Me.PictureBox3)
        Me.GroupBox1.Controls.Add(Me.TrackBarMotors)
        Me.GroupBox1.Controls.Add(Me.PictureBoxSimuThrottle)
        Me.GroupBox1.Controls.Add(Me.labelMotorsSimulation)
        Me.GroupBox1.Controls.Add(Me.LabelMotors)
        Me.GroupBox1.Location = New System.Drawing.Point(4, 102)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(384, 233)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Test Servos"
        '
        'LabelOnSimuMoveThrottle
        '
        Me.LabelOnSimuMoveThrottle.AutoSize = True
        Me.LabelOnSimuMoveThrottle.Font = New System.Drawing.Font("Arial", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelOnSimuMoveThrottle.Location = New System.Drawing.Point(66, 206)
        Me.LabelOnSimuMoveThrottle.Name = "LabelOnSimuMoveThrottle"
        Me.LabelOnSimuMoveThrottle.Size = New System.Drawing.Size(169, 16)
        Me.LabelOnSimuMoveThrottle.TabIndex = 144
        Me.LabelOnSimuMoveThrottle.Text = "Canal Moteurs Simulation"
        '
        'TrackBarRudder
        '
        Me.TrackBarRudder.LargeChange = 10
        Me.TrackBarRudder.Location = New System.Drawing.Point(58, 107)
        Me.TrackBarRudder.Maximum = 180
        Me.TrackBarRudder.Name = "TrackBarRudder"
        Me.TrackBarRudder.Size = New System.Drawing.Size(213, 45)
        Me.TrackBarRudder.SmallChange = 10
        Me.TrackBarRudder.TabIndex = 143
        Me.TrackBarRudder.TickFrequency = 10
        Me.TrackBarRudder.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(349, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(34, 17)
        Me.Label4.TabIndex = 142
        Me.Label4.Text = "Aux"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(318, 8)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(33, 17)
        Me.Label13.TabIndex = 141
        Me.Label13.Text = "Thr"
        '
        'labelAuxRudderSimulation
        '
        Me.labelAuxRudderSimulation.AutoSize = True
        Me.labelAuxRudderSimulation.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelAuxRudderSimulation.Location = New System.Drawing.Point(65, 87)
        Me.labelAuxRudderSimulation.Name = "labelAuxRudderSimulation"
        Me.labelAuxRudderSimulation.Size = New System.Drawing.Size(43, 19)
        Me.labelAuxRudderSimulation.TabIndex = 138
        Me.labelAuxRudderSimulation.Text = "Auxi"
        '
        'PictureBoxGlowPlugOnOff
        '
        Me.PictureBoxGlowPlugOnOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxGlowPlugOnOff.Location = New System.Drawing.Point(173, 180)
        Me.PictureBoxGlowPlugOnOff.Name = "PictureBoxGlowPlugOnOff"
        Me.PictureBoxGlowPlugOnOff.Size = New System.Drawing.Size(13, 16)
        Me.PictureBoxGlowPlugOnOff.TabIndex = 125
        Me.PictureBoxGlowPlugOnOff.TabStop = False
        '
        'ButtonServoTest
        '
        Me.ButtonServoTest.Location = New System.Drawing.Point(277, 36)
        Me.ButtonServoTest.Name = "ButtonServoTest"
        Me.ButtonServoTest.Size = New System.Drawing.Size(44, 23)
        Me.ButtonServoTest.TabIndex = 18
        Me.ButtonServoTest.Text = "Test"
        Me.ButtonServoTest.UseVisualStyleBackColor = True
        '
        'ButtonGlowPlugOnOff
        '
        Me.ButtonGlowPlugOnOff.Location = New System.Drawing.Point(69, 176)
        Me.ButtonGlowPlugOnOff.Name = "ButtonGlowPlugOnOff"
        Me.ButtonGlowPlugOnOff.Size = New System.Drawing.Size(99, 23)
        Me.ButtonGlowPlugOnOff.TabIndex = 137
        Me.ButtonGlowPlugOnOff.Text = "Bougies On/Off"
        Me.ButtonGlowPlugOnOff.UseVisualStyleBackColor = True
        '
        'PictureBoxGlowPlug
        '
        Me.PictureBoxGlowPlug.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBoxGlowPlug.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources.GlowPlug
        Me.PictureBoxGlowPlug.Location = New System.Drawing.Point(8, 156)
        Me.PictureBoxGlowPlug.Name = "PictureBoxGlowPlug"
        Me.PictureBoxGlowPlug.Size = New System.Drawing.Size(51, 65)
        Me.PictureBoxGlowPlug.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxGlowPlug.TabIndex = 136
        Me.PictureBoxGlowPlug.TabStop = False
        '
        'ProgressBarAuxiliary
        '
        Me.ProgressBarAuxiliary.Location = New System.Drawing.Point(69, 133)
        Me.ProgressBarAuxiliary.Maximum = 180
        Me.ProgressBarAuxiliary.Name = "ProgressBarAuxiliary"
        Me.ProgressBarAuxiliary.Size = New System.Drawing.Size(192, 19)
        Me.ProgressBarAuxiliary.TabIndex = 134
        '
        'PictureBoxAuxMaxi
        '
        Me.PictureBoxAuxMaxi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxAuxMaxi.Location = New System.Drawing.Point(263, 109)
        Me.PictureBoxAuxMaxi.Name = "PictureBoxAuxMaxi"
        Me.PictureBoxAuxMaxi.Size = New System.Drawing.Size(10, 16)
        Me.PictureBoxAuxMaxi.TabIndex = 133
        Me.PictureBoxAuxMaxi.TabStop = False
        '
        'PictureBoxAuxMiddle
        '
        Me.PictureBoxAuxMiddle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxAuxMiddle.Location = New System.Drawing.Point(196, 110)
        Me.PictureBoxAuxMiddle.Name = "PictureBoxAuxMiddle"
        Me.PictureBoxAuxMiddle.Size = New System.Drawing.Size(10, 16)
        Me.PictureBoxAuxMiddle.TabIndex = 132
        Me.PictureBoxAuxMiddle.TabStop = False
        '
        'PictureBoxAuxMini
        '
        Me.PictureBoxAuxMini.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxAuxMini.Location = New System.Drawing.Point(115, 110)
        Me.PictureBoxAuxMini.Name = "PictureBoxAuxMini"
        Me.PictureBoxAuxMini.Size = New System.Drawing.Size(10, 16)
        Me.PictureBoxAuxMini.TabIndex = 125
        Me.PictureBoxAuxMini.TabStop = False
        '
        'ButtonAuxMaxi
        '
        Me.ButtonAuxMaxi.Location = New System.Drawing.Point(217, 106)
        Me.ButtonAuxMaxi.Name = "ButtonAuxMaxi"
        Me.ButtonAuxMaxi.Size = New System.Drawing.Size(44, 23)
        Me.ButtonAuxMaxi.TabIndex = 131
        Me.ButtonAuxMaxi.Text = "On"
        Me.ButtonAuxMaxi.UseVisualStyleBackColor = True
        '
        'ButtonAuxMiddle
        '
        Me.ButtonAuxMiddle.Location = New System.Drawing.Point(140, 106)
        Me.ButtonAuxMiddle.Name = "ButtonAuxMiddle"
        Me.ButtonAuxMiddle.Size = New System.Drawing.Size(53, 23)
        Me.ButtonAuxMiddle.TabIndex = 130
        Me.ButtonAuxMiddle.Text = "Milieu"
        Me.ButtonAuxMiddle.UseVisualStyleBackColor = True
        '
        'ButtonAuxMini
        '
        Me.ButtonAuxMini.Location = New System.Drawing.Point(69, 106)
        Me.ButtonAuxMini.Name = "ButtonAuxMini"
        Me.ButtonAuxMini.Size = New System.Drawing.Size(44, 23)
        Me.ButtonAuxMini.TabIndex = 129
        Me.ButtonAuxMini.Text = "Off"
        Me.ButtonAuxMini.UseVisualStyleBackColor = True
        '
        'ProgressBarThrottle
        '
        Me.ProgressBarThrottle.Location = New System.Drawing.Point(69, 67)
        Me.ProgressBarThrottle.Maximum = 180
        Me.ProgressBarThrottle.Name = "ProgressBarThrottle"
        Me.ProgressBarThrottle.Size = New System.Drawing.Size(192, 19)
        Me.ProgressBarThrottle.TabIndex = 126
        '
        'LabelAux
        '
        Me.LabelAux.AutoSize = True
        Me.LabelAux.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelAux.Location = New System.Drawing.Point(277, 131)
        Me.LabelAux.Name = "LabelAux"
        Me.LabelAux.Size = New System.Drawing.Size(21, 22)
        Me.LabelAux.TabIndex = 125
        Me.LabelAux.Text = "0"
        Me.LabelAux.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox3
        '
        Me.PictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBox3.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources.inter
        Me.PictureBox3.Location = New System.Drawing.Point(8, 83)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(51, 65)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 121
        Me.PictureBox3.TabStop = False
        '
        'TrackBarMotors
        '
        Me.TrackBarMotors.LargeChange = 10
        Me.TrackBarMotors.Location = New System.Drawing.Point(58, 34)
        Me.TrackBarMotors.Maximum = 180
        Me.TrackBarMotors.Name = "TrackBarMotors"
        Me.TrackBarMotors.Size = New System.Drawing.Size(213, 45)
        Me.TrackBarMotors.SmallChange = 10
        Me.TrackBarMotors.TabIndex = 2
        Me.TrackBarMotors.TickFrequency = 10
        Me.TrackBarMotors.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'PictureBoxSimuThrottle
        '
        Me.PictureBoxSimuThrottle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBoxSimuThrottle.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources.TrackBar
        Me.PictureBoxSimuThrottle.Location = New System.Drawing.Point(8, 32)
        Me.PictureBoxSimuThrottle.Name = "PictureBoxSimuThrottle"
        Me.PictureBoxSimuThrottle.Size = New System.Drawing.Size(51, 45)
        Me.PictureBoxSimuThrottle.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxSimuThrottle.TabIndex = 14
        Me.PictureBoxSimuThrottle.TabStop = False
        '
        'labelMotorsSimulation
        '
        Me.labelMotorsSimulation.AutoSize = True
        Me.labelMotorsSimulation.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.labelMotorsSimulation.Location = New System.Drawing.Point(65, 14)
        Me.labelMotorsSimulation.Name = "labelMotorsSimulation"
        Me.labelMotorsSimulation.Size = New System.Drawing.Size(204, 19)
        Me.labelMotorsSimulation.TabIndex = 1
        Me.labelMotorsSimulation.Text = "Canal Moteurs Simulation"
        '
        'LabelMotors
        '
        Me.LabelMotors.AutoSize = True
        Me.LabelMotors.Font = New System.Drawing.Font("Arial", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelMotors.Location = New System.Drawing.Point(277, 67)
        Me.LabelMotors.Name = "LabelMotors"
        Me.LabelMotors.Size = New System.Drawing.Size(21, 22)
        Me.LabelMotors.TabIndex = 3
        Me.LabelMotors.Text = "0"
        Me.LabelMotors.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupSpeedSimu
        '
        Me.GroupSpeedSimu.Controls.Add(Me.PictureBoxSimuOnOff)
        Me.GroupSpeedSimu.Controls.Add(Me.Label1)
        Me.GroupSpeedSimu.Controls.Add(Me.Label6)
        Me.GroupSpeedSimu.Controls.Add(Me.Label8)
        Me.GroupSpeedSimu.Controls.Add(Me.Label9)
        Me.GroupSpeedSimu.Controls.Add(Me.TextBoxForceSpeedSimu2)
        Me.GroupSpeedSimu.Controls.Add(Me.CheckBoxSimuSynchroSpeeds)
        Me.GroupSpeedSimu.Controls.Add(Me.TrackBarSpeedSimu2)
        Me.GroupSpeedSimu.Controls.Add(Me.TrackBarSpeedSimu1)
        Me.GroupSpeedSimu.Controls.Add(Me.TextBoxForceSpeedSimu1)
        Me.GroupSpeedSimu.Controls.Add(Me.ButtonSpeedSimuOn)
        Me.GroupSpeedSimu.Location = New System.Drawing.Point(4, 4)
        Me.GroupSpeedSimu.Name = "GroupSpeedSimu"
        Me.GroupSpeedSimu.Size = New System.Drawing.Size(585, 97)
        Me.GroupSpeedSimu.TabIndex = 0
        Me.GroupSpeedSimu.TabStop = False
        Me.GroupSpeedSimu.Text = "Speed Simulation"
        '
        'PictureBoxSimuOnOff
        '
        Me.PictureBoxSimuOnOff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PictureBoxSimuOnOff.Location = New System.Drawing.Point(99, 21)
        Me.PictureBoxSimuOnOff.Name = "PictureBoxSimuOnOff"
        Me.PictureBoxSimuOnOff.Size = New System.Drawing.Size(13, 16)
        Me.PictureBoxSimuOnOff.TabIndex = 152
        Me.PictureBoxSimuOnOff.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(181, 56)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(32, 13)
        Me.Label1.TabIndex = 151
        Me.Label1.Text = "tr/mn"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(181, 23)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(32, 13)
        Me.Label6.TabIndex = 150
        Me.Label6.Text = "tr/mn"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(218, 56)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(24, 16)
        Me.Label8.TabIndex = 149
        Me.Label8.Text = "V2"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(218, 21)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(24, 16)
        Me.Label9.TabIndex = 146
        Me.Label9.Text = "V1"
        '
        'TextBoxForceSpeedSimu2
        '
        Me.TextBoxForceSpeedSimu2.Location = New System.Drawing.Point(138, 52)
        Me.TextBoxForceSpeedSimu2.Name = "TextBoxForceSpeedSimu2"
        Me.TextBoxForceSpeedSimu2.Size = New System.Drawing.Size(40, 20)
        Me.TextBoxForceSpeedSimu2.TabIndex = 148
        Me.TextBoxForceSpeedSimu2.Text = "5000"
        Me.TextBoxForceSpeedSimu2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'CheckBoxSimuSynchroSpeeds
        '
        Me.CheckBoxSimuSynchroSpeeds.AutoSize = True
        Me.CheckBoxSimuSynchroSpeeds.Location = New System.Drawing.Point(8, 54)
        Me.CheckBoxSimuSynchroSpeeds.Name = "CheckBoxSimuSynchroSpeeds"
        Me.CheckBoxSimuSynchroSpeeds.Size = New System.Drawing.Size(129, 17)
        Me.CheckBoxSimuSynchroSpeeds.TabIndex = 142
        Me.CheckBoxSimuSynchroSpeeds.Text = "Synchroniser Vitesses"
        Me.CheckBoxSimuSynchroSpeeds.UseVisualStyleBackColor = True
        '
        'TrackBarSpeedSimu2
        '
        Me.TrackBarSpeedSimu2.LargeChange = 10
        Me.TrackBarSpeedSimu2.Location = New System.Drawing.Point(241, 44)
        Me.TrackBarSpeedSimu2.Maximum = 20000
        Me.TrackBarSpeedSimu2.Minimum = 500
        Me.TrackBarSpeedSimu2.Name = "TrackBarSpeedSimu2"
        Me.TrackBarSpeedSimu2.Size = New System.Drawing.Size(331, 45)
        Me.TrackBarSpeedSimu2.SmallChange = 10
        Me.TrackBarSpeedSimu2.TabIndex = 147
        Me.TrackBarSpeedSimu2.TickFrequency = 1000
        Me.TrackBarSpeedSimu2.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.TrackBarSpeedSimu2.Value = 500
        '
        'TrackBarSpeedSimu1
        '
        Me.TrackBarSpeedSimu1.LargeChange = 10
        Me.TrackBarSpeedSimu1.Location = New System.Drawing.Point(241, 11)
        Me.TrackBarSpeedSimu1.Maximum = 20000
        Me.TrackBarSpeedSimu1.Minimum = 500
        Me.TrackBarSpeedSimu1.Name = "TrackBarSpeedSimu1"
        Me.TrackBarSpeedSimu1.Size = New System.Drawing.Size(331, 45)
        Me.TrackBarSpeedSimu1.SmallChange = 10
        Me.TrackBarSpeedSimu1.TabIndex = 145
        Me.TrackBarSpeedSimu1.TickFrequency = 1000
        Me.TrackBarSpeedSimu1.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.TrackBarSpeedSimu1.Value = 500
        '
        'TextBoxForceSpeedSimu1
        '
        Me.TextBoxForceSpeedSimu1.Location = New System.Drawing.Point(138, 20)
        Me.TextBoxForceSpeedSimu1.Name = "TextBoxForceSpeedSimu1"
        Me.TextBoxForceSpeedSimu1.Size = New System.Drawing.Size(40, 20)
        Me.TextBoxForceSpeedSimu1.TabIndex = 141
        Me.TextBoxForceSpeedSimu1.Text = "5000"
        Me.TextBoxForceSpeedSimu1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ButtonSpeedSimuOn
        '
        Me.ButtonSpeedSimuOn.Location = New System.Drawing.Point(6, 19)
        Me.ButtonSpeedSimuOn.Name = "ButtonSpeedSimuOn"
        Me.ButtonSpeedSimuOn.Size = New System.Drawing.Size(87, 22)
        Me.ButtonSpeedSimuOn.TabIndex = 143
        Me.ButtonSpeedSimuOn.Text = "Simu Vitesse"
        Me.ButtonSpeedSimuOn.UseVisualStyleBackColor = True
        '
        'TabPage6
        '
        Me.TabPage6.Controls.Add(Me.GroupBoxSDListFiles)
        Me.TabPage6.Controls.Add(Me.GroupBoxSDCardInfos)
        Me.TabPage6.Location = New System.Drawing.Point(4, 22)
        Me.TabPage6.Name = "TabPage6"
        Me.TabPage6.Size = New System.Drawing.Size(597, 366)
        Me.TabPage6.TabIndex = 5
        Me.TabPage6.Text = "SD Card"
        Me.TabPage6.UseVisualStyleBackColor = True
        '
        'GroupBoxSDListFiles
        '
        Me.GroupBoxSDListFiles.Controls.Add(Me.ButtonDumpLogFile)
        Me.GroupBoxSDListFiles.Controls.Add(Me.ListBoxSDListFiles)
        Me.GroupBoxSDListFiles.Controls.Add(Me.ButtonSDCardErase)
        Me.GroupBoxSDListFiles.Location = New System.Drawing.Point(9, 103)
        Me.GroupBoxSDListFiles.Name = "GroupBoxSDListFiles"
        Me.GroupBoxSDListFiles.Size = New System.Drawing.Size(580, 182)
        Me.GroupBoxSDListFiles.TabIndex = 5
        Me.GroupBoxSDListFiles.TabStop = False
        Me.GroupBoxSDListFiles.Text = "SD Card Files List"
        '
        'ButtonDumpLogFile
        '
        Me.ButtonDumpLogFile.Location = New System.Drawing.Point(442, 153)
        Me.ButtonDumpLogFile.Name = "ButtonDumpLogFile"
        Me.ButtonDumpLogFile.Size = New System.Drawing.Size(108, 23)
        Me.ButtonDumpLogFile.TabIndex = 9
        Me.ButtonDumpLogFile.Text = "SD Card Dump Log"
        Me.ButtonDumpLogFile.UseVisualStyleBackColor = True
        '
        'ListBoxSDListFiles
        '
        Me.ListBoxSDListFiles.FormattingEnabled = True
        Me.ListBoxSDListFiles.Location = New System.Drawing.Point(12, 20)
        Me.ListBoxSDListFiles.Name = "ListBoxSDListFiles"
        Me.ListBoxSDListFiles.Size = New System.Drawing.Size(562, 121)
        Me.ListBoxSDListFiles.TabIndex = 8
        '
        'ButtonSDCardErase
        '
        Me.ButtonSDCardErase.Location = New System.Drawing.Point(174, 153)
        Me.ButtonSDCardErase.Name = "ButtonSDCardErase"
        Me.ButtonSDCardErase.Size = New System.Drawing.Size(108, 23)
        Me.ButtonSDCardErase.TabIndex = 7
        Me.ButtonSDCardErase.Text = "Delete File"
        Me.ButtonSDCardErase.UseVisualStyleBackColor = True
        '
        'GroupBoxSDCardInfos
        '
        Me.GroupBoxSDCardInfos.Controls.Add(Me.ButtonSDCardListFiles)
        Me.GroupBoxSDCardInfos.Controls.Add(Me.ButtonWriteDataLogger)
        Me.GroupBoxSDCardInfos.Controls.Add(Me.buttonSDCardInfos)
        Me.GroupBoxSDCardInfos.Controls.Add(Me.labelSDCardIsUsed)
        Me.GroupBoxSDCardInfos.Controls.Add(Me.labelSDCardFAT)
        Me.GroupBoxSDCardInfos.Controls.Add(Me.labelSDCardType)
        Me.GroupBoxSDCardInfos.Location = New System.Drawing.Point(9, 13)
        Me.GroupBoxSDCardInfos.Name = "GroupBoxSDCardInfos"
        Me.GroupBoxSDCardInfos.Size = New System.Drawing.Size(580, 84)
        Me.GroupBoxSDCardInfos.TabIndex = 4
        Me.GroupBoxSDCardInfos.TabStop = False
        Me.GroupBoxSDCardInfos.Text = "SD Card Infos"
        '
        'ButtonSDCardListFiles
        '
        Me.ButtonSDCardListFiles.Location = New System.Drawing.Point(174, 51)
        Me.ButtonSDCardListFiles.Name = "ButtonSDCardListFiles"
        Me.ButtonSDCardListFiles.Size = New System.Drawing.Size(108, 23)
        Me.ButtonSDCardListFiles.TabIndex = 8
        Me.ButtonSDCardListFiles.Text = "SD Card List Files"
        Me.ButtonSDCardListFiles.UseVisualStyleBackColor = True
        '
        'ButtonWriteDataLogger
        '
        Me.ButtonWriteDataLogger.Location = New System.Drawing.Point(12, 51)
        Me.ButtonWriteDataLogger.Name = "ButtonWriteDataLogger"
        Me.ButtonWriteDataLogger.Size = New System.Drawing.Size(108, 23)
        Me.ButtonWriteDataLogger.TabIndex = 5
        Me.ButtonWriteDataLogger.Text = "SD Card Write Test"
        Me.ButtonWriteDataLogger.UseVisualStyleBackColor = True
        '
        'buttonSDCardInfos
        '
        Me.buttonSDCardInfos.Location = New System.Drawing.Point(475, 20)
        Me.buttonSDCardInfos.Name = "buttonSDCardInfos"
        Me.buttonSDCardInfos.Size = New System.Drawing.Size(99, 23)
        Me.buttonSDCardInfos.TabIndex = 4
        Me.buttonSDCardInfos.Text = "SD Card Infos"
        Me.buttonSDCardInfos.UseVisualStyleBackColor = True
        '
        'labelSDCardIsUsed
        '
        Me.labelSDCardIsUsed.AutoSize = True
        Me.labelSDCardIsUsed.Location = New System.Drawing.Point(9, 25)
        Me.labelSDCardIsUsed.Name = "labelSDCardIsUsed"
        Me.labelSDCardIsUsed.Size = New System.Drawing.Size(78, 13)
        Me.labelSDCardIsUsed.TabIndex = 0
        Me.labelSDCardIsUsed.Text = "SD Card Used:"
        '
        'labelSDCardFAT
        '
        Me.labelSDCardFAT.AutoSize = True
        Me.labelSDCardFAT.Location = New System.Drawing.Point(269, 25)
        Me.labelSDCardFAT.Name = "labelSDCardFAT"
        Me.labelSDCardFAT.Size = New System.Drawing.Size(73, 13)
        Me.labelSDCardFAT.TabIndex = 3
        Me.labelSDCardFAT.Text = "SD Card FAT:"
        '
        'labelSDCardType
        '
        Me.labelSDCardType.AutoSize = True
        Me.labelSDCardType.Location = New System.Drawing.Point(137, 25)
        Me.labelSDCardType.Name = "labelSDCardType"
        Me.labelSDCardType.Size = New System.Drawing.Size(77, 13)
        Me.labelSDCardType.TabIndex = 1
        Me.labelSDCardType.Text = "SD Card Type:"
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.TextBoxHexaEditor)
        Me.TabPage7.Controls.Add(Me.PictureBoxPinOut)
        Me.TabPage7.Controls.Add(Me.GroupBoxOperations)
        Me.TabPage7.Controls.Add(Me.txtoutput)
        Me.TabPage7.Controls.Add(Me.grpfile)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Size = New System.Drawing.Size(597, 366)
        Me.TabPage7.TabIndex = 6
        Me.TabPage7.Text = "Programmer"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'TextBoxHexaEditor
        '
        Me.TextBoxHexaEditor.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBoxHexaEditor.BackColor = System.Drawing.SystemColors.InactiveCaption
        Me.TextBoxHexaEditor.Font = New System.Drawing.Font("Courier New", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBoxHexaEditor.Location = New System.Drawing.Point(9, 211)
        Me.TextBoxHexaEditor.Multiline = True
        Me.TextBoxHexaEditor.Name = "TextBoxHexaEditor"
        Me.TextBoxHexaEditor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.TextBoxHexaEditor.Size = New System.Drawing.Size(580, 20)
        Me.TextBoxHexaEditor.TabIndex = 0
        '
        'PictureBoxPinOut
        '
        Me.PictureBoxPinOut.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PictureBoxPinOut.Image = CType(resources.GetObject("PictureBoxPinOut.Image"), System.Drawing.Image)
        Me.PictureBoxPinOut.Location = New System.Drawing.Point(9, 252)
        Me.PictureBoxPinOut.Name = "PictureBoxPinOut"
        Me.PictureBoxPinOut.Size = New System.Drawing.Size(580, 114)
        Me.PictureBoxPinOut.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBoxPinOut.TabIndex = 6
        Me.PictureBoxPinOut.TabStop = False
        '
        'GroupBoxOperations
        '
        Me.GroupBoxOperations.Controls.Add(Me.ButtonReadHexaEditor)
        Me.GroupBoxOperations.Controls.Add(Me.cmddataviewer)
        Me.GroupBoxOperations.Controls.Add(Me.cmderasechip)
        Me.GroupBoxOperations.Controls.Add(Me.cmdverify)
        Me.GroupBoxOperations.Controls.Add(Me.cmdread)
        Me.GroupBoxOperations.Controls.Add(Me.cmdwrite)
        Me.GroupBoxOperations.Controls.Add(Me.rdowrveeprom)
        Me.GroupBoxOperations.Controls.Add(Me.rdowrvflash)
        Me.GroupBoxOperations.Location = New System.Drawing.Point(8, 103)
        Me.GroupBoxOperations.Name = "GroupBoxOperations"
        Me.GroupBoxOperations.Size = New System.Drawing.Size(581, 104)
        Me.GroupBoxOperations.TabIndex = 6
        Me.GroupBoxOperations.TabStop = False
        Me.GroupBoxOperations.Text = "Opérations"
        '
        'ButtonReadHexaEditor
        '
        Me.ButtonReadHexaEditor.Image = CType(resources.GetObject("ButtonReadHexaEditor.Image"), System.Drawing.Image)
        Me.ButtonReadHexaEditor.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ButtonReadHexaEditor.Location = New System.Drawing.Point(373, 42)
        Me.ButtonReadHexaEditor.Name = "ButtonReadHexaEditor"
        Me.ButtonReadHexaEditor.Size = New System.Drawing.Size(81, 55)
        Me.ButtonReadHexaEditor.TabIndex = 8
        Me.ButtonReadHexaEditor.Text = "Read"
        Me.ButtonReadHexaEditor.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ButtonReadHexaEditor.UseVisualStyleBackColor = True
        '
        'cmddataviewer
        '
        Me.cmddataviewer.Location = New System.Drawing.Point(0, 0)
        Me.cmddataviewer.Name = "cmddataviewer"
        Me.cmddataviewer.Size = New System.Drawing.Size(75, 23)
        Me.cmddataviewer.TabIndex = 0
        '
        'cmderasechip
        '
        Me.cmderasechip.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources.delete
        Me.cmderasechip.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmderasechip.Location = New System.Drawing.Point(271, 42)
        Me.cmderasechip.Name = "cmderasechip"
        Me.cmderasechip.Size = New System.Drawing.Size(81, 55)
        Me.cmderasechip.TabIndex = 5
        Me.cmderasechip.Text = "Delete"
        Me.cmderasechip.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmderasechip.UseVisualStyleBackColor = True
        '
        'cmdverify
        '
        Me.cmdverify.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources._005_Task_32x42_72
        Me.cmdverify.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdverify.Location = New System.Drawing.Point(184, 42)
        Me.cmdverify.Name = "cmdverify"
        Me.cmdverify.Size = New System.Drawing.Size(81, 55)
        Me.cmdverify.TabIndex = 4
        Me.cmdverify.Text = "Verify"
        Me.cmdverify.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdverify.UseVisualStyleBackColor = True
        '
        'cmdread
        '
        Me.cmdread.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources._112_DownArrowLong_Green_32x32_72
        Me.cmdread.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdread.Location = New System.Drawing.Point(97, 42)
        Me.cmdread.Name = "cmdread"
        Me.cmdread.Size = New System.Drawing.Size(81, 55)
        Me.cmdread.TabIndex = 3
        Me.cmdread.Text = "Read"
        Me.cmdread.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdread.UseVisualStyleBackColor = True
        '
        'cmdwrite
        '
        Me.cmdwrite.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources._010_LowPriority_32x42_72
        Me.cmdwrite.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdwrite.Location = New System.Drawing.Point(10, 42)
        Me.cmdwrite.Name = "cmdwrite"
        Me.cmdwrite.Size = New System.Drawing.Size(81, 55)
        Me.cmdwrite.TabIndex = 2
        Me.cmdwrite.Text = "Write"
        Me.cmdwrite.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdwrite.UseVisualStyleBackColor = True
        '
        'rdowrveeprom
        '
        Me.rdowrveeprom.AutoSize = True
        Me.rdowrveeprom.Location = New System.Drawing.Point(278, 19)
        Me.rdowrveeprom.Name = "rdowrveeprom"
        Me.rdowrveeprom.Size = New System.Drawing.Size(161, 17)
        Me.rdowrveeprom.TabIndex = 1
        Me.rdowrveeprom.TabStop = True
        Me.rdowrveeprom.Text = "Write/Read/Verify EEPROM"
        Me.rdowrveeprom.UseVisualStyleBackColor = True
        '
        'rdowrvflash
        '
        Me.rdowrvflash.AutoSize = True
        Me.rdowrvflash.Location = New System.Drawing.Point(10, 19)
        Me.rdowrvflash.Name = "rdowrvflash"
        Me.rdowrvflash.Size = New System.Drawing.Size(140, 17)
        Me.rdowrvflash.TabIndex = 0
        Me.rdowrvflash.TabStop = True
        Me.rdowrvflash.Text = "Write/Read/Verify Flash"
        Me.rdowrvflash.UseVisualStyleBackColor = True
        '
        'txtoutput
        '
        Me.txtoutput.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtoutput.BackColor = System.Drawing.Color.Black
        Me.txtoutput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtoutput.ForeColor = System.Drawing.Color.DarkOrange
        Me.txtoutput.Location = New System.Drawing.Point(9, 237)
        Me.txtoutput.Multiline = True
        Me.txtoutput.Name = "txtoutput"
        Me.txtoutput.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtoutput.Size = New System.Drawing.Size(580, 121)
        Me.txtoutput.TabIndex = 5
        '
        'grpfile
        '
        Me.grpfile.Controls.Add(Me.ButtonPinOutHelp)
        Me.grpfile.Controls.Add(Me.cmddata)
        Me.grpfile.Controls.Add(Me.cmdwritefuse)
        Me.grpfile.Controls.Add(Me.lblLfuse)
        Me.grpfile.Controls.Add(Me.lblHfuse)
        Me.grpfile.Controls.Add(Me.lblEfuse)
        Me.grpfile.Controls.Add(Me.txtlfuse)
        Me.grpfile.Controls.Add(Me.txthfuse)
        Me.grpfile.Controls.Add(Me.txtefuse)
        Me.grpfile.Controls.Add(Me.cmdreadfuse)
        Me.grpfile.Controls.Add(Me.lblavrchip)
        Me.grpfile.Controls.Add(Me.ButtonCmdWindow)
        Me.grpfile.Controls.Add(Me.cboprecon)
        Me.grpfile.Controls.Add(Me.ButtonBootLoader)
        Me.grpfile.Controls.Add(Me.ButtonUSBAspUpload)
        Me.grpfile.Controls.Add(Me.CheckBoxUseUSBAsp)
        Me.grpfile.Controls.Add(Me.lblinstallhw)
        Me.grpfile.Controls.Add(Me.chktoggledtr)
        Me.grpfile.Controls.Add(Me.lblbrdname)
        Me.grpfile.Controls.Add(Me.ButtonSerialUpload)
        Me.grpfile.Controls.Add(Me.cboboardname)
        Me.grpfile.Controls.Add(Me.bttnbrowse)
        Me.grpfile.Controls.Add(Me.txtfilename)
        Me.grpfile.Controls.Add(Me.lblfileselect)
        Me.grpfile.Location = New System.Drawing.Point(8, 3)
        Me.grpfile.Name = "grpfile"
        Me.grpfile.Size = New System.Drawing.Size(581, 97)
        Me.grpfile.TabIndex = 4
        Me.grpfile.TabStop = False
        Me.grpfile.Text = "Upload Hex File"
        '
        'ButtonPinOutHelp
        '
        Me.ButtonPinOutHelp.Location = New System.Drawing.Point(518, 12)
        Me.ButtonPinOutHelp.Name = "ButtonPinOutHelp"
        Me.ButtonPinOutHelp.Size = New System.Drawing.Size(57, 23)
        Me.ButtonPinOutHelp.TabIndex = 23
        Me.ButtonPinOutHelp.Text = "PinOut"
        Me.ButtonPinOutHelp.UseVisualStyleBackColor = True
        '
        'cmddata
        '
        Me.cmddata.Location = New System.Drawing.Point(10, 67)
        Me.cmddata.Name = "cmddata"
        Me.cmddata.Size = New System.Drawing.Size(127, 23)
        Me.cmddata.TabIndex = 22
        Me.cmddata.Text = "Data Read/Write"
        Me.cmddata.UseVisualStyleBackColor = True
        '
        'cmdwritefuse
        '
        Me.cmdwritefuse.Location = New System.Drawing.Point(224, 36)
        Me.cmdwritefuse.Name = "cmdwritefuse"
        Me.cmdwritefuse.Size = New System.Drawing.Size(45, 24)
        Me.cmdwritefuse.TabIndex = 21
        Me.cmdwritefuse.Text = "Write"
        Me.cmdwritefuse.UseVisualStyleBackColor = True
        '
        'lblLfuse
        '
        Me.lblLfuse.AutoSize = True
        Me.lblLfuse.Location = New System.Drawing.Point(117, 41)
        Me.lblLfuse.Name = "lblLfuse"
        Me.lblLfuse.Size = New System.Drawing.Size(16, 13)
        Me.lblLfuse.TabIndex = 20
        Me.lblLfuse.Text = "L:"
        '
        'lblHfuse
        '
        Me.lblHfuse.AutoSize = True
        Me.lblHfuse.Location = New System.Drawing.Point(61, 41)
        Me.lblHfuse.Name = "lblHfuse"
        Me.lblHfuse.Size = New System.Drawing.Size(18, 13)
        Me.lblHfuse.TabIndex = 19
        Me.lblHfuse.Text = "H:"
        '
        'lblEfuse
        '
        Me.lblEfuse.AutoSize = True
        Me.lblEfuse.Location = New System.Drawing.Point(3, 42)
        Me.lblEfuse.Name = "lblEfuse"
        Me.lblEfuse.Size = New System.Drawing.Size(17, 13)
        Me.lblEfuse.TabIndex = 18
        Me.lblEfuse.Text = "E:"
        '
        'txtlfuse
        '
        Me.txtlfuse.Location = New System.Drawing.Point(135, 38)
        Me.txtlfuse.Name = "txtlfuse"
        Me.txtlfuse.Size = New System.Drawing.Size(32, 20)
        Me.txtlfuse.TabIndex = 17
        '
        'txthfuse
        '
        Me.txthfuse.Location = New System.Drawing.Point(79, 38)
        Me.txthfuse.Name = "txthfuse"
        Me.txthfuse.Size = New System.Drawing.Size(32, 20)
        Me.txthfuse.TabIndex = 16
        '
        'txtefuse
        '
        Me.txtefuse.Location = New System.Drawing.Point(23, 38)
        Me.txtefuse.Name = "txtefuse"
        Me.txtefuse.Size = New System.Drawing.Size(32, 20)
        Me.txtefuse.TabIndex = 15
        '
        'cmdreadfuse
        '
        Me.cmdreadfuse.Location = New System.Drawing.Point(173, 36)
        Me.cmdreadfuse.Name = "cmdreadfuse"
        Me.cmdreadfuse.Size = New System.Drawing.Size(45, 24)
        Me.cmdreadfuse.TabIndex = 14
        Me.cmdreadfuse.Text = "Read"
        Me.cmdreadfuse.UseVisualStyleBackColor = True
        '
        'lblavrchip
        '
        Me.lblavrchip.AutoSize = True
        Me.lblavrchip.Location = New System.Drawing.Point(96, 16)
        Me.lblavrchip.Name = "lblavrchip"
        Me.lblavrchip.Size = New System.Drawing.Size(52, 13)
        Me.lblavrchip.TabIndex = 8
        Me.lblavrchip.Text = "lblavrchip"
        '
        'ButtonCmdWindow
        '
        Me.ButtonCmdWindow.Image = Global.SynchTwinRcEngine_Interface.My.Resources.Resources.cmd
        Me.ButtonCmdWindow.Location = New System.Drawing.Point(428, 65)
        Me.ButtonCmdWindow.Name = "ButtonCmdWindow"
        Me.ButtonCmdWindow.Size = New System.Drawing.Size(32, 25)
        Me.ButtonCmdWindow.TabIndex = 11
        Me.ButtonCmdWindow.UseVisualStyleBackColor = True
        '
        'cboprecon
        '
        Me.cboprecon.FormattingEnabled = True
        Me.cboprecon.Location = New System.Drawing.Point(266, 68)
        Me.cboprecon.Name = "cboprecon"
        Me.cboprecon.Size = New System.Drawing.Size(31, 21)
        Me.cboprecon.TabIndex = 9
        '
        'ButtonBootLoader
        '
        Me.ButtonBootLoader.Location = New System.Drawing.Point(143, 11)
        Me.ButtonBootLoader.Name = "ButtonBootLoader"
        Me.ButtonBootLoader.Size = New System.Drawing.Size(117, 24)
        Me.ButtonBootLoader.TabIndex = 10
        Me.ButtonBootLoader.Text = "Upload Bootloader"
        Me.ButtonBootLoader.UseVisualStyleBackColor = True
        '
        'ButtonUSBAspUpload
        '
        Me.ButtonUSBAspUpload.Location = New System.Drawing.Point(305, 66)
        Me.ButtonUSBAspUpload.Name = "ButtonUSBAspUpload"
        Me.ButtonUSBAspUpload.Size = New System.Drawing.Size(117, 24)
        Me.ButtonUSBAspUpload.TabIndex = 9
        Me.ButtonUSBAspUpload.Text = "Upload to arduino"
        Me.ButtonUSBAspUpload.UseVisualStyleBackColor = True
        '
        'CheckBoxUseUSBAsp
        '
        Me.CheckBoxUseUSBAsp.AutoSize = True
        Me.CheckBoxUseUSBAsp.Location = New System.Drawing.Point(466, 71)
        Me.CheckBoxUseUSBAsp.Name = "CheckBoxUseUSBAsp"
        Me.CheckBoxUseUSBAsp.Size = New System.Drawing.Size(107, 17)
        Me.CheckBoxUseUSBAsp.TabIndex = 8
        Me.CheckBoxUseUSBAsp.Text = "Serial or USBAsp"
        Me.CheckBoxUseUSBAsp.UseVisualStyleBackColor = True
        '
        'lblinstallhw
        '
        Me.lblinstallhw.AutoSize = True
        Me.lblinstallhw.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblinstallhw.ForeColor = System.Drawing.Color.Blue
        Me.lblinstallhw.Location = New System.Drawing.Point(383, 16)
        Me.lblinstallhw.Name = "lblinstallhw"
        Me.lblinstallhw.Size = New System.Drawing.Size(129, 13)
        Me.lblinstallhw.TabIndex = 7
        Me.lblinstallhw.Text = "Install USBAsp Driver"
        '
        'chktoggledtr
        '
        Me.chktoggledtr.AutoSize = True
        Me.chktoggledtr.Location = New System.Drawing.Point(8, 73)
        Me.chktoggledtr.Name = "chktoggledtr"
        Me.chktoggledtr.Size = New System.Drawing.Size(128, 17)
        Me.chktoggledtr.TabIndex = 5
        Me.chktoggledtr.Text = "Toggle DTR to Reset"
        Me.chktoggledtr.UseVisualStyleBackColor = True
        '
        'lblbrdname
        '
        Me.lblbrdname.AutoSize = True
        Me.lblbrdname.Location = New System.Drawing.Point(305, 22)
        Me.lblbrdname.Name = "lblbrdname"
        Me.lblbrdname.Size = New System.Drawing.Size(69, 13)
        Me.lblbrdname.TabIndex = 5
        Me.lblbrdname.Text = "Board Name:"
        '
        'ButtonSerialUpload
        '
        Me.ButtonSerialUpload.Location = New System.Drawing.Point(143, 66)
        Me.ButtonSerialUpload.Name = "ButtonSerialUpload"
        Me.ButtonSerialUpload.Size = New System.Drawing.Size(117, 24)
        Me.ButtonSerialUpload.TabIndex = 3
        Me.ButtonSerialUpload.Text = "Upload to arduino"
        Me.ButtonSerialUpload.UseVisualStyleBackColor = True
        '
        'cboboardname
        '
        Me.cboboardname.FormattingEnabled = True
        Me.cboboardname.Location = New System.Drawing.Point(308, 38)
        Me.cboboardname.Name = "cboboardname"
        Me.cboboardname.Size = New System.Drawing.Size(259, 21)
        Me.cboboardname.TabIndex = 4
        '
        'bttnbrowse
        '
        Me.bttnbrowse.Location = New System.Drawing.Point(266, 37)
        Me.bttnbrowse.Name = "bttnbrowse"
        Me.bttnbrowse.Size = New System.Drawing.Size(32, 23)
        Me.bttnbrowse.TabIndex = 2
        Me.bttnbrowse.Text = "..."
        Me.bttnbrowse.UseVisualStyleBackColor = True
        '
        'txtfilename
        '
        Me.txtfilename.Location = New System.Drawing.Point(183, 0)
        Me.txtfilename.Name = "txtfilename"
        Me.txtfilename.Size = New System.Drawing.Size(254, 20)
        Me.txtfilename.TabIndex = 1
        '
        'lblfileselect
        '
        Me.lblfileselect.AutoSize = True
        Me.lblfileselect.Location = New System.Drawing.Point(3, 22)
        Me.lblfileselect.Name = "lblfileselect"
        Me.lblfileselect.Size = New System.Drawing.Size(78, 13)
        Me.lblfileselect.TabIndex = 0
        Me.lblfileselect.Text = "Select Hex file:"
        '
        'TabPage8
        '
        Me.TabPage8.Controls.Add(Me.ButtonPlayGame)
        Me.TabPage8.Location = New System.Drawing.Point(4, 22)
        Me.TabPage8.Name = "TabPage8"
        Me.TabPage8.Size = New System.Drawing.Size(597, 366)
        Me.TabPage8.TabIndex = 7
        Me.TabPage8.Text = "Hidden Game"
        Me.TabPage8.UseVisualStyleBackColor = True
        '
        'ButtonPlayGame
        '
        Me.ButtonPlayGame.Font = New System.Drawing.Font("Microsoft Sans Serif", 30.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonPlayGame.Location = New System.Drawing.Point(21, 18)
        Me.ButtonPlayGame.Name = "ButtonPlayGame"
        Me.ButtonPlayGame.Size = New System.Drawing.Size(554, 325)
        Me.ButtonPlayGame.TabIndex = 0
        Me.ButtonPlayGame.Text = "Play"
        Me.ButtonPlayGame.UseVisualStyleBackColor = True
        '
        'TimerSpeeds
        '
        Me.TimerSpeeds.Interval = 1000
        '
        'BackgroundWorker1
        '
        Me.BackgroundWorker1.WorkerReportsProgress = True
        Me.BackgroundWorker1.WorkerSupportsCancellation = True
        '
        'fd
        '
        Me.fd.FileName = "OpenFileDialog1"
        '
        'TimerChrono
        '
        Me.TimerChrono.Interval = 1000
        '
        'BackgroundWorkerThrottle
        '
        Me.BackgroundWorkerThrottle.WorkerReportsProgress = True
        Me.BackgroundWorkerThrottle.WorkerSupportsCancellation = True
        '
        'BackgroundWorkerAuxiliary
        '
        Me.BackgroundWorkerAuxiliary.WorkerReportsProgress = True
        Me.BackgroundWorkerAuxiliary.WorkerSupportsCancellation = True
        '
        'ProgressBarThrottleAuxiliary
        '
        Me.ProgressBarThrottleAuxiliary._Dessin = SynchTwinRcEngine_Interface.UcV_ProgressBar.Look.LookSmooth
        Me.ProgressBarThrottleAuxiliary._Maxi = 100
        Me.ProgressBarThrottleAuxiliary._Mini = 0
        Me.ProgressBarThrottleAuxiliary._Value = 50
        Me.ProgressBarThrottleAuxiliary.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.ProgressBarThrottleAuxiliary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ProgressBarThrottleAuxiliary.Location = New System.Drawing.Point(567, 37)
        Me.ProgressBarThrottleAuxiliary.Name = "ProgressBarThrottleAuxiliary"
        Me.ProgressBarThrottleAuxiliary.Size = New System.Drawing.Size(19, 241)
        Me.ProgressBarThrottleAuxiliary.TabIndex = 136
        '
        'ProgressBarThrottleMotors
        '
        Me.ProgressBarThrottleMotors._Dessin = SynchTwinRcEngine_Interface.UcV_ProgressBar.Look.LookSmooth
        Me.ProgressBarThrottleMotors._Maxi = 100
        Me.ProgressBarThrottleMotors._Mini = 0
        Me.ProgressBarThrottleMotors._Value = 50
        Me.ProgressBarThrottleMotors.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.ProgressBarThrottleMotors.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ProgressBarThrottleMotors.Location = New System.Drawing.Point(536, 37)
        Me.ProgressBarThrottleMotors.Name = "ProgressBarThrottleMotors"
        Me.ProgressBarThrottleMotors.Size = New System.Drawing.Size(19, 241)
        Me.ProgressBarThrottleMotors.TabIndex = 135
        '
        'UcV_ProgressBar1
        '
        Me.UcV_ProgressBar1._Dessin = SynchTwinRcEngine_Interface.UcV_ProgressBar.Look.LookSmooth
        Me.UcV_ProgressBar1._Maxi = 100
        Me.UcV_ProgressBar1._Mini = 0
        Me.UcV_ProgressBar1._Value = 50
        Me.UcV_ProgressBar1.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.UcV_ProgressBar1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.UcV_ProgressBar1.Location = New System.Drawing.Point(357, 30)
        Me.UcV_ProgressBar1.Name = "UcV_ProgressBar1"
        Me.UcV_ProgressBar1.Size = New System.Drawing.Size(19, 191)
        Me.UcV_ProgressBar1.TabIndex = 140
        '
        'UcV_ProgressBar2
        '
        Me.UcV_ProgressBar2._Dessin = SynchTwinRcEngine_Interface.UcV_ProgressBar.Look.LookSmooth
        Me.UcV_ProgressBar2._Maxi = 100
        Me.UcV_ProgressBar2._Mini = 0
        Me.UcV_ProgressBar2._Value = 50
        Me.UcV_ProgressBar2.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.UcV_ProgressBar2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.UcV_ProgressBar2.Location = New System.Drawing.Point(326, 30)
        Me.UcV_ProgressBar2.Name = "UcV_ProgressBar2"
        Me.UcV_ProgressBar2.Size = New System.Drawing.Size(19, 191)
        Me.UcV_ProgressBar2.TabIndex = 139
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(605, 392)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form1"
        Me.Text = "SynchTwinRcEngine Programmateur (module maître ou esclave)"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxSerialPort.ResumeLayout(False)
        Me.GroupBoxSerialPort.PerformLayout()
        CType(Me.PictureBoxConnectedOK, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxSettings.ResumeLayout(False)
        Me.GroupBoxSettings.PerformLayout()
        CType(Me.PictureBoxReadHardwareOnOff, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxTimer2OnOff, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxMoteurs.ResumeLayout(False)
        Me.GroupBoxMoteurs.PerformLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBoxLang.ResumeLayout(False)
        Me.GroupBoxLang.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        Me.TabPage3.PerformLayout()
        CType(Me.PictureBoxTimer1OnOff, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage4.ResumeLayout(False)
        Me.DataLogger.ResumeLayout(False)
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.BoxRecorder.ResumeLayout(False)
        Me.BoxRecorder.PerformLayout()
        CType(Me.PictureBox6, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxPlayer, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxRecorder, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupSpeedPID.ResumeLayout(False)
        Me.GroupSpeedPID.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.TrackBarRudder, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxGlowPlugOnOff, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxGlowPlug, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxAuxMaxi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxAuxMiddle, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxAuxMini, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBarMotors, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBoxSimuThrottle, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupSpeedSimu.ResumeLayout(False)
        Me.GroupSpeedSimu.PerformLayout()
        CType(Me.PictureBoxSimuOnOff, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBarSpeedSimu2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBarSpeedSimu1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage6.ResumeLayout(False)
        Me.GroupBoxSDListFiles.ResumeLayout(False)
        Me.GroupBoxSDCardInfos.ResumeLayout(False)
        Me.GroupBoxSDCardInfos.PerformLayout()
        Me.TabPage7.ResumeLayout(False)
        Me.TabPage7.PerformLayout()
        CType(Me.PictureBoxPinOut, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBoxOperations.ResumeLayout(False)
        Me.GroupBoxOperations.PerformLayout()
        Me.grpfile.ResumeLayout(False)
        Me.grpfile.PerformLayout()
        Me.TabPage8.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents ComboPort As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ComboBaudRate As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ButtonSauvegardeCOM As System.Windows.Forms.Button
    Friend WithEvents GroupBoxSerialPort As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBoxSettings As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonConfigDefaut As System.Windows.Forms.Button
    Friend WithEvents ButtonPlusNombrePales As System.Windows.Forms.Button
    Friend WithEvents ButtonMoinsNombrePales As System.Windows.Forms.Button
    Friend WithEvents ButtonPlusModeAuxiliaire As System.Windows.Forms.Button
    Friend WithEvents ButtonMoinsModeAuxiliaire As System.Windows.Forms.Button
    Friend WithEvents ButtonPlusVitesseReponse As System.Windows.Forms.Button
    Friend WithEvents ButtonMoinsVitesseReponse As System.Windows.Forms.Button
    Friend WithEvents LabelModifications As System.Windows.Forms.Label
    Friend WithEvents ButtonAnnulerModif As System.Windows.Forms.Button
    Friend WithEvents CheckBoxInversionServo2 As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxInversionServo1 As System.Windows.Forms.CheckBox
    Friend WithEvents TextVoltageExterne As System.Windows.Forms.TextBox
    Friend WithEvents TextTempInterne As System.Windows.Forms.TextBox
    Friend WithEvents TextVoltageInterne As System.Windows.Forms.TextBox
    Friend WithEvents textNombrePales As System.Windows.Forms.TextBox
    Friend WithEvents textAuxiliaireMode As System.Windows.Forms.TextBox
    Friend WithEvents textMaxiGenerale As System.Windows.Forms.TextBox
    Friend WithEvents textMiniGenerale As System.Windows.Forms.TextBox
    Friend WithEvents textDebutSynchro As System.Windows.Forms.TextBox
    Friend WithEvents textMaxiMoteurs As System.Windows.Forms.TextBox
    Friend WithEvents textTempsReponse As System.Windows.Forms.TextBox
    Friend WithEvents textIdleServo2 As System.Windows.Forms.TextBox
    Friend WithEvents textIdleServo1 As System.Windows.Forms.TextBox
    Friend WithEvents textCentreServo2 As System.Windows.Forms.TextBox
    Friend WithEvents textCentreServo1 As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents labelInternalVoltage As System.Windows.Forms.Label
    Friend WithEvents labelNbrBlades As System.Windows.Forms.Label
    Friend WithEvents labelMode As System.Windows.Forms.Label
    Friend WithEvents labelReverseServo2 As System.Windows.Forms.Label
    Friend WithEvents labelReverseServo1 As System.Windows.Forms.Label
    Friend WithEvents labelAuxiMode As System.Windows.Forms.Label
    Friend WithEvents labelPosMaxiGene As System.Windows.Forms.Label
    Friend WithEvents labelPosMiniGene As System.Windows.Forms.Label
    Friend WithEvents labelStartSynchroServos As System.Windows.Forms.Label
    Friend WithEvents labelMaxiServos As System.Windows.Forms.Label
    Friend WithEvents labelSpeedModuleAnswer As System.Windows.Forms.Label
    Friend WithEvents labelIdleServo2 As System.Windows.Forms.Label
    Friend WithEvents labelIdleServo1 As System.Windows.Forms.Label
    Friend WithEvents labelCenterServo2 As System.Windows.Forms.Label
    Friend WithEvents labelCenterServo1 As System.Windows.Forms.Label
    Friend WithEvents ButtonSauvegardeConfig As System.Windows.Forms.Button
    Friend WithEvents labelConfigModule As System.Windows.Forms.Label
    Friend WithEvents ButtonReadCenter2 As System.Windows.Forms.Button
    Friend WithEvents ButtonReadCenter1 As System.Windows.Forms.Button
    Friend WithEvents ButtonIdleMoteur1 As System.Windows.Forms.Button
    Friend WithEvents ButtonIdleMoteur2 As System.Windows.Forms.Button
    Friend WithEvents ButtonMiniMaxGeneral As System.Windows.Forms.Button
    Friend WithEvents ButtonDebutSynchro As System.Windows.Forms.Button
    Friend WithEvents ButtonMaxiMoteurs As System.Windows.Forms.Button
    Friend WithEvents LabelInterType As System.Windows.Forms.Label
    Friend WithEvents ButtonAuxiliaireHelp As System.Windows.Forms.Button
    Friend WithEvents TextBoxAuxiliairePulse As System.Windows.Forms.TextBox
    Friend WithEvents ButtonReadAuxiliairePulse As System.Windows.Forms.Button
    Friend WithEvents labelAuxiPosition As System.Windows.Forms.Label
    Friend WithEvents ButtonReadTempVoltage As System.Windows.Forms.Button
    Friend WithEvents GroupBoxMoteurs As System.Windows.Forms.GroupBox
    Friend WithEvents AquaGaugeMoteur1 As AquaControls.AquaGauge
    Friend WithEvents AquaGaugeMoteur2 As AquaControls.AquaGauge
    Friend WithEvents ButtonAbout As System.Windows.Forms.Button
    Friend WithEvents ButtonModuleType As System.Windows.Forms.Button
    Friend WithEvents CheckBoxFahrenheitDegrees As System.Windows.Forms.CheckBox
    Friend WithEvents Button_Connect As System.Windows.Forms.Button
    Friend WithEvents PictureBoxConnectedOK As System.Windows.Forms.PictureBox
    Friend WithEvents btnSend As System.Windows.Forms.Button
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Public WithEvents TimerRXAuxiliaire As System.Windows.Forms.Timer
    Public WithEvents TimerHardwareInfos As System.Windows.Forms.Timer
    Public WithEvents TimerDataLogger As System.Windows.Forms.Timer
    Friend WithEvents SerialPort1 As System.IO.Ports.SerialPort
    Friend WithEvents LabelDEBUG As System.Windows.Forms.Label
    Friend WithEvents labelReverseAuxi As System.Windows.Forms.Label
    Friend WithEvents CheckBoxInversionAux As System.Windows.Forms.CheckBox
    Friend WithEvents textGeneralMinMaxStopWatch As System.Windows.Forms.Label
    Friend WithEvents textMiniMotorRPM As System.Windows.Forms.TextBox
    Friend WithEvents labelSpeedMinMaxRPM As System.Windows.Forms.Label
    Friend WithEvents textMaxiMotorRPM As System.Windows.Forms.TextBox
    Friend WithEvents optLangFR As System.Windows.Forms.RadioButton
    Friend WithEvents optLangEN As System.Windows.Forms.RadioButton
    Friend WithEvents PictureBox4 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox5 As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonDiffSpeedSimuConsigne As System.Windows.Forms.Label
    Friend WithEvents TextBoxDiffSpeedSimuConsigne As System.Windows.Forms.TextBox
    Friend WithEvents GroupSpeedSimu As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents TextBoxForceSpeedSimu2 As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxSimuSynchroSpeeds As System.Windows.Forms.CheckBox
    Friend WithEvents TrackBarSpeedSimu2 As System.Windows.Forms.TrackBar
    Friend WithEvents TrackBarSpeedSimu1 As System.Windows.Forms.TrackBar
    Friend WithEvents TextBoxForceSpeedSimu1 As System.Windows.Forms.TextBox
    Friend WithEvents ButtonSpeedSimuOn As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents zg1 As ZedGraph.ZedGraphControl
    Friend WithEvents DataLogger As System.Windows.Forms.GroupBox
    Friend WithEvents ProgressBarThrottleMotors As SynchTwinRcEngine_Interface.UcV_ProgressBar
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents ProgressBarThrottleAuxiliary As SynchTwinRcEngine_Interface.UcV_ProgressBar
    Friend WithEvents GroupBoxLang As System.Windows.Forms.GroupBox
    Friend WithEvents buttonEraseEEprom As System.Windows.Forms.Button
    Friend WithEvents buttonResetArduino As System.Windows.Forms.Button
    Friend WithEvents labelMotor2 As System.Windows.Forms.Label
    Friend WithEvents labelMotor1 As System.Windows.Forms.Label
    Friend WithEvents LabelVitesse1 As System.Windows.Forms.Label
    Friend WithEvents ButtonReadSpeeds As System.Windows.Forms.Button
    Friend WithEvents PictureBoxTimer1OnOff As System.Windows.Forms.PictureBox
    Friend WithEvents labelSDCardIsUsed As System.Windows.Forms.Label
    Friend WithEvents LabelVitesse2 As System.Windows.Forms.Label
    Friend WithEvents CheckBoxAnaDigi As System.Windows.Forms.CheckBox
    Friend WithEvents labelSDCardType As System.Windows.Forms.Label
    Friend WithEvents labelSDCardFAT As System.Windows.Forms.Label
    Friend WithEvents GroupBoxSDCardInfos As System.Windows.Forms.GroupBox
    Friend WithEvents buttonSDCardInfos As System.Windows.Forms.Button
    Public WithEvents TabControl1 As System.Windows.Forms.TabControl
    Public WithEvents TabPage1 As System.Windows.Forms.TabPage
    Public WithEvents TabPage2 As System.Windows.Forms.TabPage
    Public WithEvents TabPage3 As System.Windows.Forms.TabPage
    Public WithEvents TabPage4 As System.Windows.Forms.TabPage
    Public WithEvents TabPage5 As System.Windows.Forms.TabPage
    Public WithEvents TabPage6 As System.Windows.Forms.TabPage
    Public WithEvents TimerRXMotors As System.Windows.Forms.Timer
    Friend WithEvents PictureBoxSimuOnOff As System.Windows.Forms.PictureBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents labelAuxRudderSimulation As System.Windows.Forms.Label
    Friend WithEvents PictureBoxGlowPlugOnOff As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonServoTest As System.Windows.Forms.Button
    Friend WithEvents ButtonGlowPlugOnOff As System.Windows.Forms.Button
    Friend WithEvents PictureBoxGlowPlug As System.Windows.Forms.PictureBox
    Friend WithEvents ProgressBarAuxiliary As System.Windows.Forms.ProgressBar
    Friend WithEvents PictureBoxAuxMaxi As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBoxAuxMiddle As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBoxAuxMini As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonAuxMaxi As System.Windows.Forms.Button
    Friend WithEvents ButtonAuxMiddle As System.Windows.Forms.Button
    Friend WithEvents ButtonAuxMini As System.Windows.Forms.Button
    Friend WithEvents ProgressBarThrottle As System.Windows.Forms.ProgressBar
    Friend WithEvents LabelAux As System.Windows.Forms.Label
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents TrackBarMotors As System.Windows.Forms.TrackBar
    Friend WithEvents PictureBoxSimuThrottle As System.Windows.Forms.PictureBox
    Friend WithEvents labelMotorsSimulation As System.Windows.Forms.Label
    Friend WithEvents LabelMotors As System.Windows.Forms.Label
    Friend WithEvents GroupSpeedPID As System.Windows.Forms.GroupBox
    Friend WithEvents ButtonSavePIDVariables As System.Windows.Forms.Button
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents textBoxKdControl As System.Windows.Forms.TextBox
    Friend WithEvents textBoxKiControl As System.Windows.Forms.TextBox
    Friend WithEvents textBoxKpControl As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents UcV_ProgressBar1 As SynchTwinRcEngine_Interface.UcV_ProgressBar
    Friend WithEvents UcV_ProgressBar2 As SynchTwinRcEngine_Interface.UcV_ProgressBar
    Friend WithEvents ButtonSimulationHelp As System.Windows.Forms.Button
    Friend WithEvents TrackBarRudder As System.Windows.Forms.TrackBar
    Friend WithEvents labelInfoNeedSaveCOM As System.Windows.Forms.TextBox
    Friend WithEvents ButtonSettingsHelp As System.Windows.Forms.Button
    Friend WithEvents RichTextBoxSettingsHelp As System.Windows.Forms.RichTextBox
    Friend WithEvents PictureBoxReadHardwareOnOff As System.Windows.Forms.PictureBox
    Friend WithEvents labelExtervalVoltageUsed As System.Windows.Forms.Label
    Friend WithEvents ProgressBarSaveSettings As System.Windows.Forms.ProgressBar
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents txtoutput As System.Windows.Forms.TextBox
    Friend WithEvents grpfile As System.Windows.Forms.GroupBox
    Friend WithEvents lblbrdname As System.Windows.Forms.Label
    Friend WithEvents cboboardname As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonSerialUpload As System.Windows.Forms.Button
    Friend WithEvents bttnbrowse As System.Windows.Forms.Button
    Friend WithEvents txtfilename As System.Windows.Forms.TextBox
    Friend WithEvents lblfileselect As System.Windows.Forms.Label
    Friend WithEvents chktoggledtr As System.Windows.Forms.CheckBox
    Friend WithEvents ofd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents lblinstallhw As System.Windows.Forms.Label
    Friend WithEvents lblavrchip As System.Windows.Forms.Label
    Friend WithEvents CheckBoxUseUSBAsp As System.Windows.Forms.CheckBox
    Friend WithEvents ButtonUSBAspUpload As System.Windows.Forms.Button
    Friend WithEvents cboprecon As System.Windows.Forms.ComboBox
    Friend WithEvents ButtonBootLoader As System.Windows.Forms.Button
    Friend WithEvents ButtonCmdWindow As System.Windows.Forms.Button
    Friend WithEvents TabPage8 As System.Windows.Forms.TabPage
    Friend WithEvents ButtonPlayGame As System.Windows.Forms.Button
    Friend WithEvents SevenSegmentArray2 As SevenSegment.SevenSegmentArray
    Friend WithEvents SevenSegmentArray1 As SevenSegment.SevenSegmentArray
    Friend WithEvents cmdreadfuse As System.Windows.Forms.Button
    Friend WithEvents txtlfuse As System.Windows.Forms.TextBox
    Friend WithEvents txthfuse As System.Windows.Forms.TextBox
    Friend WithEvents txtefuse As System.Windows.Forms.TextBox
    Friend WithEvents lblLfuse As System.Windows.Forms.Label
    Friend WithEvents lblHfuse As System.Windows.Forms.Label
    Friend WithEvents lblEfuse As System.Windows.Forms.Label
    Friend WithEvents cmdwritefuse As System.Windows.Forms.Button
    Friend WithEvents cmddata As System.Windows.Forms.Button
    Friend WithEvents GroupBoxOperations As System.Windows.Forms.GroupBox
    Friend WithEvents cmddataviewer As System.Windows.Forms.Button
    Friend WithEvents cmderasechip As System.Windows.Forms.Button
    Friend WithEvents cmdverify As System.Windows.Forms.Button
    Friend WithEvents cmdread As System.Windows.Forms.Button
    Friend WithEvents cmdwrite As System.Windows.Forms.Button
    Friend WithEvents rdowrveeprom As System.Windows.Forms.RadioButton
    Friend WithEvents rdowrvflash As System.Windows.Forms.RadioButton
    Friend WithEvents svfd As System.Windows.Forms.SaveFileDialog
    Friend WithEvents ButtonWriteDataLogger As System.Windows.Forms.Button
    Friend WithEvents ButtonSDCardErase As System.Windows.Forms.Button
    Friend WithEvents BoxRecorder As System.Windows.Forms.GroupBox
    Friend WithEvents PictureBoxRecorder As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonRecorder As System.Windows.Forms.Button
    Friend WithEvents PictureBoxPlayer As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonPlayer As System.Windows.Forms.Button
    Friend WithEvents PictureBox6 As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonTerminal As System.Windows.Forms.Button
    Public WithEvents TimerSpeeds As System.Windows.Forms.Timer
    Friend WithEvents ButtonClear As System.Windows.Forms.Button
    Friend WithEvents RichTextSimulationHelp As System.Windows.Forms.RichTextBox
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents fd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents TextBoxChronoMM As System.Windows.Forms.TextBox
    Friend WithEvents LabelChronoSimu As System.Windows.Forms.Label
    Friend WithEvents TextBoxChronoHH As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Public WithEvents TimerChrono As System.Windows.Forms.Timer
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents LabelChronoSS As System.Windows.Forms.Label
    Friend WithEvents BackgroundWorkerThrottle As System.ComponentModel.BackgroundWorker
    Friend WithEvents ButtonPinOutHelp As System.Windows.Forms.Button
    Friend WithEvents PictureBoxPinOut As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonReadHexaEditor As System.Windows.Forms.Button
    Friend WithEvents TextBoxHexaEditor As System.Windows.Forms.TextBox
    Friend WithEvents CheckBoxChronoOnOff As System.Windows.Forms.CheckBox
    Friend WithEvents LabelChrono As System.Windows.Forms.Label
    Friend WithEvents LabelTestNow As System.Windows.Forms.Label
    Friend WithEvents ButtonSDCardListFiles As System.Windows.Forms.Button
    Friend WithEvents CheckBoxReadTracBarMotorOrMotorThrottle As System.Windows.Forms.CheckBox
    Friend WithEvents LabelOnSimuMoveThrottle As System.Windows.Forms.Label
    Friend WithEvents BackgroundWorkerAuxiliary As System.ComponentModel.BackgroundWorker
    Friend WithEvents PictureBoxTimer2OnOff As System.Windows.Forms.PictureBox
    Friend WithEvents ButtonDumpLogFile As System.Windows.Forms.Button
    Friend WithEvents GroupBoxSDListFiles As System.Windows.Forms.GroupBox
    Friend WithEvents ListBoxSDListFiles As System.Windows.Forms.ListBox
    Friend WithEvents LabelSignalType As System.Windows.Forms.Label
    Friend WithEvents ButtonRcRadioMode As System.Windows.Forms.Button
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents labelModeRcRadio As System.Windows.Forms.Label
    Friend WithEvents LabelExternalVoltage As System.Windows.Forms.Label


End Class
