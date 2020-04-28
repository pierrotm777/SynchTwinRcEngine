<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class dataeditor
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.editortab = New System.Windows.Forms.TabControl()
        Me.eepromtab = New System.Windows.Forms.TabPage()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cmdreadeeprom = New System.Windows.Forms.Button()
        Me.chksaveclose = New System.Windows.Forms.CheckBox()
        Me.cmdsave = New System.Windows.Forms.Button()
        Me.cmdopen = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.bit0 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bit1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bit2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bit3 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bit4 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bit5 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bit6 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.bit7 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cbochiplist = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lbleeprom = New System.Windows.Forms.Label()
        Me.lblflash = New System.Windows.Forms.Label()
        Me.lblmcu = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ofd = New System.Windows.Forms.OpenFileDialog()
        Me.sfd = New System.Windows.Forms.SaveFileDialog()
        Me.tmr = New System.Windows.Forms.Timer(Me.components)
        Me.editortab.SuspendLayout()
        Me.eepromtab.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'editortab
        '
        Me.editortab.Controls.Add(Me.eepromtab)
        Me.editortab.Location = New System.Drawing.Point(3, 129)
        Me.editortab.Name = "editortab"
        Me.editortab.SelectedIndex = 0
        Me.editortab.Size = New System.Drawing.Size(516, 356)
        Me.editortab.TabIndex = 0
        '
        'eepromtab
        '
        Me.eepromtab.Controls.Add(Me.Button1)
        Me.eepromtab.Controls.Add(Me.cmdreadeeprom)
        Me.eepromtab.Controls.Add(Me.chksaveclose)
        Me.eepromtab.Controls.Add(Me.cmdsave)
        Me.eepromtab.Controls.Add(Me.cmdopen)
        Me.eepromtab.Controls.Add(Me.DataGridView1)
        Me.eepromtab.Location = New System.Drawing.Point(4, 22)
        Me.eepromtab.Name = "eepromtab"
        Me.eepromtab.Padding = New System.Windows.Forms.Padding(3)
        Me.eepromtab.Size = New System.Drawing.Size(508, 330)
        Me.eepromtab.TabIndex = 1
        Me.eepromtab.Text = "EEPROM"
        Me.eepromtab.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(363, 6)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(104, 21)
        Me.Button1.TabIndex = 5
        Me.Button1.Text = "New datasheet"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'cmdreadeeprom
        '
        Me.cmdreadeeprom.Location = New System.Drawing.Point(242, 6)
        Me.cmdreadeeprom.Name = "cmdreadeeprom"
        Me.cmdreadeeprom.Size = New System.Drawing.Size(104, 21)
        Me.cmdreadeeprom.TabIndex = 4
        Me.cmdreadeeprom.Text = "Read EEPROM"
        Me.cmdreadeeprom.UseVisualStyleBackColor = True
        '
        'chksaveclose
        '
        Me.chksaveclose.AutoSize = True
        Me.chksaveclose.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chksaveclose.Location = New System.Drawing.Point(6, 33)
        Me.chksaveclose.Name = "chksaveclose"
        Me.chksaveclose.Size = New System.Drawing.Size(500, 17)
        Me.chksaveclose.TabIndex = 3
        Me.chksaveclose.Text = "Close dialog,divert saved file path to file selection box in main window when sav" & _
    "ed."
        Me.chksaveclose.UseVisualStyleBackColor = True
        '
        'cmdsave
        '
        Me.cmdsave.Location = New System.Drawing.Point(121, 6)
        Me.cmdsave.Name = "cmdsave"
        Me.cmdsave.Size = New System.Drawing.Size(104, 21)
        Me.cmdsave.TabIndex = 2
        Me.cmdsave.Text = "Save..."
        Me.cmdsave.UseVisualStyleBackColor = True
        '
        'cmdopen
        '
        Me.cmdopen.Location = New System.Drawing.Point(6, 6)
        Me.cmdopen.Name = "cmdopen"
        Me.cmdopen.Size = New System.Drawing.Size(104, 21)
        Me.cmdopen.TabIndex = 1
        Me.cmdopen.Text = "Open..."
        Me.cmdopen.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.bit0, Me.bit1, Me.bit2, Me.bit3, Me.bit4, Me.bit5, Me.bit6, Me.bit7})
        Me.DataGridView1.Location = New System.Drawing.Point(6, 56)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.RowHeadersWidth = 50
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DataGridView1.RowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.Size = New System.Drawing.Size(495, 263)
        Me.DataGridView1.TabIndex = 0
        '
        'bit0
        '
        Me.bit0.HeaderText = "00"
        Me.bit0.Name = "bit0"
        '
        'bit1
        '
        Me.bit1.HeaderText = "01"
        Me.bit1.Name = "bit1"
        '
        'bit2
        '
        Me.bit2.HeaderText = "02"
        Me.bit2.Name = "bit2"
        '
        'bit3
        '
        Me.bit3.HeaderText = "03"
        Me.bit3.Name = "bit3"
        '
        'bit4
        '
        Me.bit4.HeaderText = "04"
        Me.bit4.Name = "bit4"
        '
        'bit5
        '
        Me.bit5.HeaderText = "05"
        Me.bit5.Name = "bit5"
        '
        'bit6
        '
        Me.bit6.HeaderText = "06"
        Me.bit6.Name = "bit6"
        '
        'bit7
        '
        Me.bit7.HeaderText = "07"
        Me.bit7.Name = "bit7"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cbochiplist)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(194, 108)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Custom Microcontroller"
        '
        'cbochiplist
        '
        Me.cbochiplist.FormattingEnabled = True
        Me.cbochiplist.Location = New System.Drawing.Point(14, 48)
        Me.cbochiplist.Name = "cbochiplist"
        Me.cbochiplist.Size = New System.Drawing.Size(174, 21)
        Me.cbochiplist.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(62, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Chip Name:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lbleeprom)
        Me.GroupBox2.Controls.Add(Me.lblflash)
        Me.GroupBox2.Controls.Add(Me.lblmcu)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Location = New System.Drawing.Point(207, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(302, 108)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Microcontroller Information"
        '
        'lbleeprom
        '
        Me.lbleeprom.AutoSize = True
        Me.lbleeprom.Location = New System.Drawing.Point(91, 70)
        Me.lbleeprom.Name = "lbleeprom"
        Me.lbleeprom.Size = New System.Drawing.Size(0, 13)
        Me.lbleeprom.TabIndex = 5
        '
        'lblflash
        '
        Me.lblflash.AutoSize = True
        Me.lblflash.Location = New System.Drawing.Point(93, 48)
        Me.lblflash.Name = "lblflash"
        Me.lblflash.Size = New System.Drawing.Size(0, 13)
        Me.lblflash.TabIndex = 4
        '
        'lblmcu
        '
        Me.lblmcu.AutoSize = True
        Me.lblmcu.Location = New System.Drawing.Point(95, 24)
        Me.lblmcu.Name = "lblmcu"
        Me.lblmcu.Size = New System.Drawing.Size(0, 13)
        Me.lblmcu.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 68)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "EEPROM       :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(14, 47)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Flash              :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(79, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Microcontroller:"
        '
        'tmr
        '
        Me.tmr.Enabled = True
        '
        'dataeditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(521, 495)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.editortab)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "dataeditor"
        Me.Text = "Flash/EEPROM Data Editor and Viewer"
        Me.editortab.ResumeLayout(False)
        Me.eepromtab.ResumeLayout(False)
        Me.eepromtab.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents editortab As System.Windows.Forms.TabControl
    Friend WithEvents eepromtab As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lbleeprom As System.Windows.Forms.Label
    Friend WithEvents lblflash As System.Windows.Forms.Label
    Friend WithEvents lblmcu As System.Windows.Forms.Label
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents bit0 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bit1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bit2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bit3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bit4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bit5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bit6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents bit7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents cmdsave As System.Windows.Forms.Button
    Friend WithEvents cmdopen As System.Windows.Forms.Button
    Friend WithEvents ofd As System.Windows.Forms.OpenFileDialog
    Friend WithEvents sfd As System.Windows.Forms.SaveFileDialog
    Friend WithEvents cbochiplist As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents tmr As System.Windows.Forms.Timer
    Friend WithEvents chksaveclose As System.Windows.Forms.CheckBox
    Friend WithEvents cmdreadeeprom As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
End Class
