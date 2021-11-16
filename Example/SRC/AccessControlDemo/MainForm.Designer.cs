namespace RFID_Sample
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.bt_StopScan = new System.Windows.Forms.Button();
            this.tb_LogOutput = new System.Windows.Forms.TextBox();
            this.bt_Clear = new System.Windows.Forms.Button();
            this.lb_Version = new System.Windows.Forms.Label();
            this.bt_LedColor = new System.Windows.Forms.Button();
            this.cb_ReverseUID = new System.Windows.Forms.CheckBox();
            this.RWData = new System.Windows.Forms.TextBox();
            this.ReadButton = new System.Windows.Forms.Button();
            this.WriteButton = new System.Windows.Forms.Button();
            this.RWAdresse = new System.Windows.Forms.TextBox();
            this.RWLänge = new System.Windows.Forms.TextBox();
            this.Adress_Label = new System.Windows.Forms.Label();
            this.Laenge_Label = new System.Windows.Forms.Label();
            this.VHL_nummer = new System.Windows.Forms.TextBox();
            this.VHL_Label = new System.Windows.Forms.Label();
            this.Date_Label = new System.Windows.Forms.Label();
            this.radioB_Read_UID = new System.Windows.Forms.RadioButton();
            this.radioB_autoread = new System.Windows.Forms.RadioButton();
            this.radioB_RW_manual = new System.Windows.Forms.RadioButton();
            this.radioB_Configcard = new System.Windows.Forms.RadioButton();
            this.bt_StartScan = new System.Windows.Forms.Button();
            this.radioButtonRS232 = new System.Windows.Forms.RadioButton();
            this.radioButtonUSB = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox_COM = new System.Windows.Forms.ComboBox();
            this.radioB_ResetFactory = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bt_StopScan
            // 
            this.bt_StopScan.Location = new System.Drawing.Point(12, 141);
            this.bt_StopScan.Name = "bt_StopScan";
            this.bt_StopScan.Size = new System.Drawing.Size(90, 23);
            this.bt_StopScan.TabIndex = 5;
            this.bt_StopScan.Text = "STOP";
            this.bt_StopScan.UseVisualStyleBackColor = true;
            this.bt_StopScan.Click += new System.EventHandler(this.bt_StopScan_Click);
            // 
            // tb_LogOutput
            // 
            this.tb_LogOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tb_LogOutput.BackColor = System.Drawing.SystemColors.Control;
            this.tb_LogOutput.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_LogOutput.Location = new System.Drawing.Point(127, 11);
            this.tb_LogOutput.Multiline = true;
            this.tb_LogOutput.Name = "tb_LogOutput";
            this.tb_LogOutput.ReadOnly = true;
            this.tb_LogOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_LogOutput.Size = new System.Drawing.Size(498, 428);
            this.tb_LogOutput.TabIndex = 100;
            this.tb_LogOutput.WordWrap = false;
            this.tb_LogOutput.TextChanged += new System.EventHandler(this.tb_LogOutput_TextChanged);
            // 
            // bt_Clear
            // 
            this.bt_Clear.Location = new System.Drawing.Point(13, 391);
            this.bt_Clear.Name = "bt_Clear";
            this.bt_Clear.Size = new System.Drawing.Size(91, 23);
            this.bt_Clear.TabIndex = 13;
            this.bt_Clear.Text = "Clear";
            this.bt_Clear.UseVisualStyleBackColor = true;
            this.bt_Clear.Click += new System.EventHandler(this.bt_Clear_Click);
            // 
            // lb_Version
            // 
            this.lb_Version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lb_Version.AutoSize = true;
            this.lb_Version.Location = new System.Drawing.Point(534, 596);
            this.lb_Version.Name = "lb_Version";
            this.lb_Version.Size = new System.Drawing.Size(45, 13);
            this.lb_Version.TabIndex = 105;
            this.lb_Version.Text = "Version:";
            // 
            // bt_LedColor
            // 
            this.bt_LedColor.Enabled = false;
            this.bt_LedColor.Location = new System.Drawing.Point(13, 351);
            this.bt_LedColor.Name = "bt_LedColor";
            this.bt_LedColor.Size = new System.Drawing.Size(91, 23);
            this.bt_LedColor.TabIndex = 12;
            this.bt_LedColor.Text = "LED Color";
            this.bt_LedColor.UseVisualStyleBackColor = true;
            this.bt_LedColor.Click += new System.EventHandler(this.bt_ChangeColLed_Click);
            // 
            // cb_ReverseUID
            // 
            this.cb_ReverseUID.AutoSize = true;
            this.cb_ReverseUID.Enabled = false;
            this.cb_ReverseUID.Location = new System.Drawing.Point(13, 317);
            this.cb_ReverseUID.Name = "cb_ReverseUID";
            this.cb_ReverseUID.Size = new System.Drawing.Size(88, 17);
            this.cb_ReverseUID.TabIndex = 11;
            this.cb_ReverseUID.Text = "Reverse UID";
            this.cb_ReverseUID.UseVisualStyleBackColor = true;
            this.cb_ReverseUID.CheckedChanged += new System.EventHandler(this.cb_ReverseUID_CheckedChanged);
            // 
            // RWData
            // 
            this.RWData.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RWData.Location = new System.Drawing.Point(127, 476);
            this.RWData.MaxLength = 5000;
            this.RWData.Multiline = true;
            this.RWData.Name = "RWData";
            this.RWData.ReadOnly = true;
            this.RWData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RWData.Size = new System.Drawing.Size(360, 108);
            this.RWData.TabIndex = 17;
            this.RWData.TextChanged += new System.EventHandler(this.RWData_TextChanged);
            this.RWData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RWData_KeyPress);
            // 
            // ReadButton
            // 
            this.ReadButton.Enabled = false;
            this.ReadButton.Location = new System.Drawing.Point(510, 494);
            this.ReadButton.Name = "ReadButton";
            this.ReadButton.Size = new System.Drawing.Size(84, 23);
            this.ReadButton.TabIndex = 18;
            this.ReadButton.Text = "ReadData";
            this.ReadButton.UseVisualStyleBackColor = true;
            this.ReadButton.Click += new System.EventHandler(this.bt_ReadButton_Click);
            // 
            // WriteButton
            // 
            this.WriteButton.Enabled = false;
            this.WriteButton.Location = new System.Drawing.Point(510, 543);
            this.WriteButton.Name = "WriteButton";
            this.WriteButton.Size = new System.Drawing.Size(84, 23);
            this.WriteButton.TabIndex = 19;
            this.WriteButton.Text = "WriteData";
            this.WriteButton.UseVisualStyleBackColor = true;
            this.WriteButton.Click += new System.EventHandler(this.bt_WriteButton_Click);
            // 
            // RWAdresse
            // 
            this.RWAdresse.AllowDrop = true;
            this.RWAdresse.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.RWAdresse.Location = new System.Drawing.Point(8, 520);
            this.RWAdresse.MaxLength = 4;
            this.RWAdresse.Name = "RWAdresse";
            this.RWAdresse.ReadOnly = true;
            this.RWAdresse.Size = new System.Drawing.Size(91, 20);
            this.RWAdresse.TabIndex = 15;
            this.RWAdresse.Text = "0";
            this.RWAdresse.TextChanged += new System.EventHandler(this.RWAdresse_TextChanged);
            // 
            // RWLänge
            // 
            this.RWLänge.Location = new System.Drawing.Point(9, 564);
            this.RWLänge.MaxLength = 4;
            this.RWLänge.Name = "RWLänge";
            this.RWLänge.ReadOnly = true;
            this.RWLänge.Size = new System.Drawing.Size(90, 20);
            this.RWLänge.TabIndex = 16;
            this.RWLänge.Text = "1";
            this.RWLänge.TextChanged += new System.EventHandler(this.RWLänge_TextChanged);
            // 
            // Adress_Label
            // 
            this.Adress_Label.AutoSize = true;
            this.Adress_Label.Location = new System.Drawing.Point(26, 504);
            this.Adress_Label.Name = "Adress_Label";
            this.Adress_Label.Size = new System.Drawing.Size(44, 13);
            this.Adress_Label.TabIndex = 102;
            this.Adress_Label.Text = "address";
            // 
            // Laenge_Label
            // 
            this.Laenge_Label.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.Laenge_Label.AutoSize = true;
            this.Laenge_Label.Location = new System.Drawing.Point(26, 548);
            this.Laenge_Label.Name = "Laenge_Label";
            this.Laenge_Label.Size = new System.Drawing.Size(36, 13);
            this.Laenge_Label.TabIndex = 103;
            this.Laenge_Label.Text = "length";
            // 
            // VHL_nummer
            // 
            this.VHL_nummer.AllowDrop = true;
            this.VHL_nummer.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.VHL_nummer.Location = new System.Drawing.Point(8, 475);
            this.VHL_nummer.MaxLength = 3;
            this.VHL_nummer.Name = "VHL_nummer";
            this.VHL_nummer.ReadOnly = true;
            this.VHL_nummer.Size = new System.Drawing.Size(91, 20);
            this.VHL_nummer.TabIndex = 14;
            this.VHL_nummer.Text = "255";
            this.VHL_nummer.TextChanged += new System.EventHandler(this.VHL_nummer_TextChanged);
            // 
            // VHL_Label
            // 
            this.VHL_Label.AutoSize = true;
            this.VHL_Label.Location = new System.Drawing.Point(17, 456);
            this.VHL_Label.Name = "VHL_Label";
            this.VHL_Label.Size = new System.Drawing.Size(82, 13);
            this.VHL_Label.TabIndex = 101;
            this.VHL_Label.Text = "VHL file number";
            // 
            // Date_Label
            // 
            this.Date_Label.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.Date_Label.AutoSize = true;
            this.Date_Label.Location = new System.Drawing.Point(133, 456);
            this.Date_Label.Name = "Date_Label";
            this.Date_Label.Size = new System.Drawing.Size(307, 13);
            this.Date_Label.TabIndex = 104;
            this.Date_Label.Text = "Data format: hexadecimal 2-digits with a space (55 EE 99 DD ..)";
            this.Date_Label.Click += new System.EventHandler(this.Date_Label_Click);
            // 
            // radioB_Read_UID
            // 
            this.radioB_Read_UID.AutoSize = true;
            this.radioB_Read_UID.Checked = true;
            this.radioB_Read_UID.Enabled = false;
            this.radioB_Read_UID.Location = new System.Drawing.Point(12, 181);
            this.radioB_Read_UID.Name = "radioB_Read_UID";
            this.radioB_Read_UID.Size = new System.Drawing.Size(81, 17);
            this.radioB_Read_UID.TabIndex = 6;
            this.radioB_Read_UID.TabStop = true;
            this.radioB_Read_UID.Text = "Read SerNr";
            this.radioB_Read_UID.UseVisualStyleBackColor = true;
            this.radioB_Read_UID.CheckedChanged += new System.EventHandler(this.rb_SerNr_CheckedChanged);
            // 
            // radioB_autoread
            // 
            this.radioB_autoread.AutoSize = true;
            this.radioB_autoread.Enabled = false;
            this.radioB_autoread.Location = new System.Drawing.Point(11, 205);
            this.radioB_autoread.Name = "radioB_autoread";
            this.radioB_autoread.Size = new System.Drawing.Size(107, 17);
            this.radioB_autoread.TabIndex = 7;
            this.radioB_autoread.Text = "Read Data (auto)";
            this.radioB_autoread.UseVisualStyleBackColor = true;
            this.radioB_autoread.CheckedChanged += new System.EventHandler(this.rb_autoread_CheckedChanged);
            // 
            // radioB_RW_manual
            // 
            this.radioB_RW_manual.AutoSize = true;
            this.radioB_RW_manual.Enabled = false;
            this.radioB_RW_manual.Location = new System.Drawing.Point(11, 229);
            this.radioB_RW_manual.Name = "radioB_RW_manual";
            this.radioB_RW_manual.Size = new System.Drawing.Size(113, 17);
            this.radioB_RW_manual.TabIndex = 8;
            this.radioB_RW_manual.Text = "Read/Write (man.)";
            this.radioB_RW_manual.UseVisualStyleBackColor = true;
            this.radioB_RW_manual.CheckedChanged += new System.EventHandler(this.rb_rw_manualCheckedChanged);
            // 
            // radioB_Configcard
            // 
            this.radioB_Configcard.AutoSize = true;
            this.radioB_Configcard.Enabled = false;
            this.radioB_Configcard.Location = new System.Drawing.Point(11, 252);
            this.radioB_Configcard.Name = "radioB_Configcard";
            this.radioB_Configcard.Size = new System.Drawing.Size(79, 17);
            this.radioB_Configcard.TabIndex = 9;
            this.radioB_Configcard.Text = "Configcard ";
            this.radioB_Configcard.UseVisualStyleBackColor = true;
            this.radioB_Configcard.CheckedChanged += new System.EventHandler(this.radioB_Configcard_CheckedChanged);
            // 
            // bt_StartScan
            // 
            this.bt_StartScan.Location = new System.Drawing.Point(13, 112);
            this.bt_StartScan.Name = "bt_StartScan";
            this.bt_StartScan.Size = new System.Drawing.Size(91, 23);
            this.bt_StartScan.TabIndex = 4;
            this.bt_StartScan.Text = "START";
            this.bt_StartScan.UseVisualStyleBackColor = true;
            this.bt_StartScan.Click += new System.EventHandler(this.button_StartScan_USB);
            // 
            // radioButtonRS232
            // 
            this.radioButtonRS232.AutoSize = true;
            this.radioButtonRS232.Location = new System.Drawing.Point(3, 26);
            this.radioButtonRS232.Name = "radioButtonRS232";
            this.radioButtonRS232.Size = new System.Drawing.Size(58, 17);
            this.radioButtonRS232.TabIndex = 2;
            this.radioButtonRS232.Text = "RS232";
            this.radioButtonRS232.UseVisualStyleBackColor = true;
            this.radioButtonRS232.CheckedChanged += new System.EventHandler(this.radioButtonRS232_CheckedChanged);
            // 
            // radioButtonUSB
            // 
            this.radioButtonUSB.AutoSize = true;
            this.radioButtonUSB.Checked = true;
            this.radioButtonUSB.Location = new System.Drawing.Point(3, 3);
            this.radioButtonUSB.Name = "radioButtonUSB";
            this.radioButtonUSB.Size = new System.Drawing.Size(47, 17);
            this.radioButtonUSB.TabIndex = 1;
            this.radioButtonUSB.TabStop = true;
            this.radioButtonUSB.Text = "USB";
            this.radioButtonUSB.UseVisualStyleBackColor = true;
            this.radioButtonUSB.CheckedChanged += new System.EventHandler(this.radioButtonUSB_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonUSB);
            this.panel1.Controls.Add(this.radioButtonRS232);
            this.panel1.Location = new System.Drawing.Point(13, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(105, 52);
            this.panel1.TabIndex = 21;
            // 
            // comboBox_COM
            // 
            this.comboBox_COM.Enabled = false;
            this.comboBox_COM.FormattingEnabled = true;
            this.comboBox_COM.Items.AddRange(new object[] {
            "COM 1",
            "COM 2",
            "COM 3",
            "COM 4",
            "COM 5",
            "COM 6",
            "COM 7",
            "COM 8",
            "COM 9",
            "COM 10",
            "COM 11",
            "COM 12",
            "COM 13",
            "COM 14",
            "COM 15",
            "COM 16",
            "COM 17",
            "COM 18",
            "COM 19",
            "COM 20"});
            this.comboBox_COM.Location = new System.Drawing.Point(13, 70);
            this.comboBox_COM.Name = "comboBox_COM";
            this.comboBox_COM.Size = new System.Drawing.Size(89, 21);
            this.comboBox_COM.TabIndex = 3;
            // 
            // radioB_ResetFactory
            // 
            this.radioB_ResetFactory.AutoSize = true;
            this.radioB_ResetFactory.Enabled = false;
            this.radioB_ResetFactory.Location = new System.Drawing.Point(11, 275);
            this.radioB_ResetFactory.Name = "radioB_ResetFactory";
            this.radioB_ResetFactory.Size = new System.Drawing.Size(94, 17);
            this.radioB_ResetFactory.TabIndex = 10;
            this.radioB_ResetFactory.Text = "Reset2Factory";
            this.radioB_ResetFactory.UseVisualStyleBackColor = true;
            this.radioB_ResetFactory.CheckedChanged += new System.EventHandler(this.radioB_ResetFactory_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 609);
            this.Controls.Add(this.radioB_ResetFactory);
            this.Controls.Add(this.comboBox_COM);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bt_StartScan);
            this.Controls.Add(this.radioB_Configcard);
            this.Controls.Add(this.radioB_RW_manual);
            this.Controls.Add(this.radioB_autoread);
            this.Controls.Add(this.radioB_Read_UID);
            this.Controls.Add(this.Date_Label);
            this.Controls.Add(this.VHL_Label);
            this.Controls.Add(this.VHL_nummer);
            this.Controls.Add(this.Laenge_Label);
            this.Controls.Add(this.Adress_Label);
            this.Controls.Add(this.RWLänge);
            this.Controls.Add(this.RWAdresse);
            this.Controls.Add(this.WriteButton);
            this.Controls.Add(this.ReadButton);
            this.Controls.Add(this.RWData);
            this.Controls.Add(this.cb_ReverseUID);
            this.Controls.Add(this.bt_LedColor);
            this.Controls.Add(this.lb_Version);
            this.Controls.Add(this.bt_Clear);
            this.Controls.Add(this.tb_LogOutput);
            this.Controls.Add(this.bt_StopScan);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "AccessControlDemo RF10x0R";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button bt_StopScan;
        private System.Windows.Forms.TextBox tb_LogOutput;
        private System.Windows.Forms.Button bt_Clear;
        private System.Windows.Forms.Label lb_Version;
		private System.Windows.Forms.Button bt_LedColor;
		private System.Windows.Forms.CheckBox cb_ReverseUID;
        private System.Windows.Forms.TextBox RWData;
        private System.Windows.Forms.Button ReadButton;
        private System.Windows.Forms.Button WriteButton;
        private System.Windows.Forms.TextBox RWAdresse;
        private System.Windows.Forms.TextBox RWLänge;
        private System.Windows.Forms.Label Adress_Label;
        private System.Windows.Forms.Label Laenge_Label;
        private System.Windows.Forms.TextBox VHL_nummer;
        private System.Windows.Forms.Label VHL_Label;
        private System.Windows.Forms.Label Date_Label;
        private System.Windows.Forms.RadioButton radioB_Read_UID;
        private System.Windows.Forms.RadioButton radioB_autoread;
        private System.Windows.Forms.RadioButton radioB_RW_manual;
        private System.Windows.Forms.RadioButton radioB_Configcard;
        private System.Windows.Forms.Button bt_StartScan;
        private System.Windows.Forms.RadioButton radioButtonRS232;
        private System.Windows.Forms.RadioButton radioButtonUSB;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox_COM;
        private System.Windows.Forms.RadioButton radioB_ResetFactory;
    }
}

