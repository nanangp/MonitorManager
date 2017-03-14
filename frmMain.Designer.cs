using System.Collections.Generic;
using System.Windows.Forms;

namespace MonitorProfiler
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.cboInput = new System.Windows.Forms.ComboBox();
            this.cboPower = new System.Windows.Forms.ComboBox();
            this.lblBlue = new System.Windows.Forms.Label();
            this.lblGreen = new System.Windows.Forms.Label();
            this.lblRed = new System.Windows.Forms.Label();
            this.barGreen = new System.Windows.Forms.TrackBar();
            this.barRed = new System.Windows.Forms.TrackBar();
            this.cboMonitors = new System.Windows.Forms.ComboBox();
            this.btnRevert = new System.Windows.Forms.Button();
            this.lblContrast = new System.Windows.Forms.Label();
            this.barContrast = new System.Windows.Forms.TrackBar();
            this.lblBrightness = new System.Windows.Forms.Label();
            this.barBrightness = new System.Windows.Forms.TrackBar();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lstProfiles = new System.Windows.Forms.ListBox();
            this.barBlue = new System.Windows.Forms.TrackBar();
            this.barVolume = new System.Windows.Forms.TrackBar();
            this.lblVolume = new System.Windows.Forms.Label();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.cboFactoryReset = new System.Windows.Forms.ComboBox();
            this.lblSharpness = new System.Windows.Forms.Label();
            this.barSharpness = new System.Windows.Forms.TrackBar();
            this.btnRestart = new System.Windows.Forms.Button();
            this.picSharpness = new System.Windows.Forms.PictureBox();
            this.btnFactoryReset = new System.Windows.Forms.Button();
            this.btnInput = new System.Windows.Forms.Button();
            this.btnPower = new System.Windows.Forms.Button();
            this.picVolume = new System.Windows.Forms.PictureBox();
            this.btnDeleteProfile = new System.Windows.Forms.Button();
            this.btnSaveProfile = new System.Windows.Forms.Button();
            this.btnNewProfile = new System.Windows.Forms.Button();
            this.picBlue = new System.Windows.Forms.PictureBox();
            this.picGreen = new System.Windows.Forms.PictureBox();
            this.picRed = new System.Windows.Forms.PictureBox();
            this.picContrast = new System.Windows.Forms.PictureBox();
            this.picBrightness = new System.Windows.Forms.PictureBox();
            this.btnLinkMonitors = new System.Windows.Forms.Button();
            this.btnIdentifyMonitor = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.barGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barSharpness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSharpness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBrightness)).BeginInit();
            this.SuspendLayout();
            // 
            // cboInput
            // 
            this.cboInput.DisplayMember = "Key";
            this.cboInput.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboInput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboInput.FormattingEnabled = true;
            this.cboInput.ItemHeight = 22;
            this.cboInput.Location = new System.Drawing.Point(8, 187);
            this.cboInput.Name = "cboInput";
            this.cboInput.Size = new System.Drawing.Size(90, 28);
            this.cboInput.TabIndex = 20;
            this.cboInput.ValueMember = "Value";
            this.cboInput.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawCbItem);
            // 
            // cboPower
            // 
            this.cboPower.DisplayMember = "Key";
            this.cboPower.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboPower.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPower.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboPower.FormattingEnabled = true;
            this.cboPower.ItemHeight = 22;
            this.cboPower.Location = new System.Drawing.Point(8, 156);
            this.cboPower.Name = "cboPower";
            this.cboPower.Size = new System.Drawing.Size(90, 28);
            this.cboPower.TabIndex = 19;
            this.cboPower.ValueMember = "Value";
            this.cboPower.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawCbItem);
            // 
            // lblBlue
            // 
            this.lblBlue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBlue.AutoSize = true;
            this.lblBlue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBlue.Location = new System.Drawing.Point(340, 164);
            this.lblBlue.Name = "lblBlue";
            this.lblBlue.Size = new System.Drawing.Size(23, 15);
            this.lblBlue.TabIndex = 16;
            this.lblBlue.Text = "0%";
            this.lblBlue.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblGreen
            // 
            this.lblGreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGreen.AutoSize = true;
            this.lblGreen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGreen.Location = new System.Drawing.Point(340, 134);
            this.lblGreen.Name = "lblGreen";
            this.lblGreen.Size = new System.Drawing.Size(23, 15);
            this.lblGreen.TabIndex = 15;
            this.lblGreen.Text = "0%";
            this.lblGreen.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblRed
            // 
            this.lblRed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRed.AutoSize = true;
            this.lblRed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRed.Location = new System.Drawing.Point(340, 104);
            this.lblRed.Name = "lblRed";
            this.lblRed.Size = new System.Drawing.Size(23, 15);
            this.lblRed.TabIndex = 14;
            this.lblRed.Text = "0%";
            this.lblRed.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // barGreen
            // 
            this.barGreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.barGreen.Location = new System.Drawing.Point(171, 132);
            this.barGreen.Maximum = 100;
            this.barGreen.Name = "barGreen";
            this.barGreen.Size = new System.Drawing.Size(167, 45);
            this.barGreen.TabIndex = 12;
            this.barGreen.TickFrequency = 5;
            this.barGreen.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // barRed
            // 
            this.barRed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.barRed.Location = new System.Drawing.Point(171, 102);
            this.barRed.Maximum = 100;
            this.barRed.Name = "barRed";
            this.barRed.Size = new System.Drawing.Size(167, 45);
            this.barRed.TabIndex = 11;
            this.barRed.TickFrequency = 5;
            this.barRed.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // cboMonitors
            // 
            this.cboMonitors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMonitors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonitors.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMonitors.FormattingEnabled = true;
            this.cboMonitors.ItemHeight = 22;
            this.cboMonitors.Location = new System.Drawing.Point(139, 8);
            this.cboMonitors.Name = "cboMonitors";
            this.cboMonitors.Size = new System.Drawing.Size(144, 28);
            this.cboMonitors.TabIndex = 9;
            this.cboMonitors.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawCbItem);
            this.cboMonitors.SelectedIndexChanged += new System.EventHandler(this.cboMonitors_SelectedIndexChanged);
            // 
            // btnRevert
            // 
            this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRevert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRevert.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRevert.Location = new System.Drawing.Point(-1, 254);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(75, 30);
            this.btnRevert.TabIndex = 7;
            this.btnRevert.Text = "Revert";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // lblContrast
            // 
            this.lblContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblContrast.AutoSize = true;
            this.lblContrast.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContrast.Location = new System.Drawing.Point(340, 74);
            this.lblContrast.Name = "lblContrast";
            this.lblContrast.Size = new System.Drawing.Size(23, 15);
            this.lblContrast.TabIndex = 4;
            this.lblContrast.Text = "0%";
            // 
            // barContrast
            // 
            this.barContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.barContrast.Location = new System.Drawing.Point(171, 72);
            this.barContrast.Maximum = 100;
            this.barContrast.Name = "barContrast";
            this.barContrast.Size = new System.Drawing.Size(167, 45);
            this.barContrast.TabIndex = 3;
            this.barContrast.TickFrequency = 5;
            this.barContrast.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // lblBrightness
            // 
            this.lblBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBrightness.AutoSize = true;
            this.lblBrightness.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrightness.Location = new System.Drawing.Point(340, 44);
            this.lblBrightness.Name = "lblBrightness";
            this.lblBrightness.Size = new System.Drawing.Size(23, 15);
            this.lblBrightness.TabIndex = 2;
            this.lblBrightness.Text = "0%";
            // 
            // barBrightness
            // 
            this.barBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.barBrightness.Location = new System.Drawing.Point(171, 42);
            this.barBrightness.Maximum = 100;
            this.barBrightness.Name = "barBrightness";
            this.barBrightness.Size = new System.Drawing.Size(167, 45);
            this.barBrightness.TabIndex = 1;
            this.barBrightness.TickFrequency = 5;
            this.barBrightness.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(-1, 254);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(386, 103);
            this.txtLog.TabIndex = 6;
            // 
            // lstProfiles
            // 
            this.lstProfiles.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstProfiles.FormattingEnabled = true;
            this.lstProfiles.ItemHeight = 15;
            this.lstProfiles.Location = new System.Drawing.Point(8, 8);
            this.lstProfiles.Name = "lstProfiles";
            this.lstProfiles.Size = new System.Drawing.Size(123, 109);
            this.lstProfiles.TabIndex = 7;
            this.lstProfiles.SelectedValueChanged += new System.EventHandler(this.lstProfiles_SelectedValueChanged);
            // 
            // barBlue
            // 
            this.barBlue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.barBlue.Location = new System.Drawing.Point(171, 162);
            this.barBlue.Maximum = 100;
            this.barBlue.Name = "barBlue";
            this.barBlue.Size = new System.Drawing.Size(167, 45);
            this.barBlue.TabIndex = 13;
            this.barBlue.TickFrequency = 5;
            this.barBlue.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // barVolume
            // 
            this.barVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.barVolume.Enabled = false;
            this.barVolume.Location = new System.Drawing.Point(171, 192);
            this.barVolume.Maximum = 100;
            this.barVolume.Name = "barVolume";
            this.barVolume.Size = new System.Drawing.Size(167, 45);
            this.barVolume.TabIndex = 32;
            this.barVolume.TickFrequency = 5;
            this.barVolume.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // lblVolume
            // 
            this.lblVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVolume.AutoSize = true;
            this.lblVolume.Enabled = false;
            this.lblVolume.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolume.Location = new System.Drawing.Point(340, 194);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(23, 15);
            this.lblVolume.TabIndex = 33;
            this.lblVolume.Text = "0%";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cboFactoryReset
            // 
            this.cboFactoryReset.DisplayMember = "Key";
            this.cboFactoryReset.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboFactoryReset.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFactoryReset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboFactoryReset.FormattingEnabled = true;
            this.cboFactoryReset.ItemHeight = 22;
            this.cboFactoryReset.Location = new System.Drawing.Point(8, 218);
            this.cboFactoryReset.Name = "cboFactoryReset";
            this.cboFactoryReset.Size = new System.Drawing.Size(90, 28);
            this.cboFactoryReset.TabIndex = 36;
            this.cboFactoryReset.ValueMember = "Value";
            this.cboFactoryReset.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawCbItem);
            // 
            // lblSharpness
            // 
            this.lblSharpness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSharpness.AutoSize = true;
            this.lblSharpness.Enabled = false;
            this.lblSharpness.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSharpness.Location = new System.Drawing.Point(340, 224);
            this.lblSharpness.Name = "lblSharpness";
            this.lblSharpness.Size = new System.Drawing.Size(23, 15);
            this.lblSharpness.TabIndex = 38;
            this.lblSharpness.Text = "0%";
            this.lblSharpness.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // barSharpness
            // 
            this.barSharpness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.barSharpness.Enabled = false;
            this.barSharpness.Location = new System.Drawing.Point(171, 222);
            this.barSharpness.Maximum = 100;
            this.barSharpness.Name = "barSharpness";
            this.barSharpness.Size = new System.Drawing.Size(167, 45);
            this.barSharpness.TabIndex = 37;
            this.barSharpness.TickFrequency = 5;
            this.barSharpness.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // btnRestart
            // 
            this.btnRestart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestart.BackgroundImage = global::MonitorProfiler.Properties.Resources.refresh;
            this.btnRestart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRestart.Location = new System.Drawing.Point(347, 7);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(30, 30);
            this.btnRestart.TabIndex = 40;
            this.toolTips.SetToolTip(this.btnRestart, "Identify");
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // picSharpness
            // 
            this.picSharpness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picSharpness.BackgroundImage = global::MonitorProfiler.Properties.Resources.sharpness;
            this.picSharpness.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picSharpness.Location = new System.Drawing.Point(135, 216);
            this.picSharpness.Name = "picSharpness";
            this.picSharpness.Size = new System.Drawing.Size(32, 32);
            this.picSharpness.TabIndex = 39;
            this.picSharpness.TabStop = false;
            this.toolTips.SetToolTip(this.picSharpness, "Sharpness");
            // 
            // btnFactoryReset
            // 
            this.btnFactoryReset.BackgroundImage = global::MonitorProfiler.Properties.Resources.undo;
            this.btnFactoryReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFactoryReset.Location = new System.Drawing.Point(100, 217);
            this.btnFactoryReset.Name = "btnFactoryReset";
            this.btnFactoryReset.Size = new System.Drawing.Size(30, 30);
            this.btnFactoryReset.TabIndex = 35;
            this.btnFactoryReset.UseVisualStyleBackColor = true;
            this.btnFactoryReset.Click += new System.EventHandler(this.btnFactoryReset_Click);
            // 
            // btnInput
            // 
            this.btnInput.BackgroundImage = global::MonitorProfiler.Properties.Resources.split;
            this.btnInput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnInput.Location = new System.Drawing.Point(100, 186);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(30, 30);
            this.btnInput.TabIndex = 21;
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // btnPower
            // 
            this.btnPower.BackgroundImage = global::MonitorProfiler.Properties.Resources.power;
            this.btnPower.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPower.Location = new System.Drawing.Point(100, 155);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(30, 30);
            this.btnPower.TabIndex = 18;
            this.btnPower.UseVisualStyleBackColor = true;
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // picVolume
            // 
            this.picVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picVolume.BackgroundImage = global::MonitorProfiler.Properties.Resources.speaker_high;
            this.picVolume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picVolume.Location = new System.Drawing.Point(135, 186);
            this.picVolume.Name = "picVolume";
            this.picVolume.Size = new System.Drawing.Size(32, 32);
            this.picVolume.TabIndex = 34;
            this.picVolume.TabStop = false;
            this.toolTips.SetToolTip(this.picVolume, "Mute");
            this.picVolume.Click += new System.EventHandler(this.picVolume_Click);
            // 
            // btnDeleteProfile
            // 
            this.btnDeleteProfile.BackgroundImage = global::MonitorProfiler.Properties.Resources.delete;
            this.btnDeleteProfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDeleteProfile.Location = new System.Drawing.Point(69, 119);
            this.btnDeleteProfile.Name = "btnDeleteProfile";
            this.btnDeleteProfile.Size = new System.Drawing.Size(30, 30);
            this.btnDeleteProfile.TabIndex = 2;
            this.btnDeleteProfile.UseVisualStyleBackColor = true;
            this.btnDeleteProfile.Click += new System.EventHandler(this.btnDeleteProfile_Click);
            // 
            // btnSaveProfile
            // 
            this.btnSaveProfile.BackgroundImage = global::MonitorProfiler.Properties.Resources.save;
            this.btnSaveProfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSaveProfile.Location = new System.Drawing.Point(7, 119);
            this.btnSaveProfile.Name = "btnSaveProfile";
            this.btnSaveProfile.Size = new System.Drawing.Size(30, 30);
            this.btnSaveProfile.TabIndex = 1;
            this.btnSaveProfile.UseVisualStyleBackColor = true;
            this.btnSaveProfile.Click += new System.EventHandler(this.btnSaveProfile_Click);
            // 
            // btnNewProfile
            // 
            this.btnNewProfile.BackgroundImage = global::MonitorProfiler.Properties.Resources.add;
            this.btnNewProfile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnNewProfile.Location = new System.Drawing.Point(38, 119);
            this.btnNewProfile.Name = "btnNewProfile";
            this.btnNewProfile.Size = new System.Drawing.Size(30, 30);
            this.btnNewProfile.TabIndex = 3;
            this.btnNewProfile.UseVisualStyleBackColor = true;
            // 
            // picBlue
            // 
            this.picBlue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picBlue.BackgroundImage = global::MonitorProfiler.Properties.Resources.rgbblue;
            this.picBlue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picBlue.Location = new System.Drawing.Point(135, 156);
            this.picBlue.Name = "picBlue";
            this.picBlue.Size = new System.Drawing.Size(32, 32);
            this.picBlue.TabIndex = 31;
            this.picBlue.TabStop = false;
            this.toolTips.SetToolTip(this.picBlue, "Blue gain");
            // 
            // picGreen
            // 
            this.picGreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picGreen.BackgroundImage = global::MonitorProfiler.Properties.Resources.rgbgreen;
            this.picGreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picGreen.Location = new System.Drawing.Point(135, 126);
            this.picGreen.Name = "picGreen";
            this.picGreen.Size = new System.Drawing.Size(32, 32);
            this.picGreen.TabIndex = 30;
            this.picGreen.TabStop = false;
            this.toolTips.SetToolTip(this.picGreen, "Green gain");
            // 
            // picRed
            // 
            this.picRed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picRed.BackgroundImage = global::MonitorProfiler.Properties.Resources.rgbred;
            this.picRed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picRed.Location = new System.Drawing.Point(135, 96);
            this.picRed.Name = "picRed";
            this.picRed.Size = new System.Drawing.Size(32, 32);
            this.picRed.TabIndex = 29;
            this.picRed.TabStop = false;
            this.toolTips.SetToolTip(this.picRed, "Red gain");
            // 
            // picContrast
            // 
            this.picContrast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picContrast.BackgroundImage = global::MonitorProfiler.Properties.Resources.contrast;
            this.picContrast.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picContrast.Location = new System.Drawing.Point(135, 66);
            this.picContrast.Name = "picContrast";
            this.picContrast.Size = new System.Drawing.Size(32, 32);
            this.picContrast.TabIndex = 28;
            this.picContrast.TabStop = false;
            this.toolTips.SetToolTip(this.picContrast, "Contrast");
            // 
            // picBrightness
            // 
            this.picBrightness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picBrightness.BackgroundImage = global::MonitorProfiler.Properties.Resources.brightness;
            this.picBrightness.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picBrightness.Location = new System.Drawing.Point(135, 36);
            this.picBrightness.Name = "picBrightness";
            this.picBrightness.Size = new System.Drawing.Size(32, 32);
            this.picBrightness.TabIndex = 27;
            this.picBrightness.TabStop = false;
            this.toolTips.SetToolTip(this.picBrightness, "Brightness");
            // 
            // btnLinkMonitors
            // 
            this.btnLinkMonitors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLinkMonitors.BackgroundImage = global::MonitorProfiler.Properties.Resources.unlink;
            this.btnLinkMonitors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLinkMonitors.Location = new System.Drawing.Point(285, 7);
            this.btnLinkMonitors.Name = "btnLinkMonitors";
            this.btnLinkMonitors.Size = new System.Drawing.Size(30, 30);
            this.btnLinkMonitors.TabIndex = 22;
            this.toolTips.SetToolTip(this.btnLinkMonitors, "Link all");
            this.btnLinkMonitors.UseVisualStyleBackColor = true;
            this.btnLinkMonitors.Click += new System.EventHandler(this.btnLinkMonitors_Click);
            // 
            // btnIdentifyMonitor
            // 
            this.btnIdentifyMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnIdentifyMonitor.BackgroundImage = global::MonitorProfiler.Properties.Resources.light;
            this.btnIdentifyMonitor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnIdentifyMonitor.Location = new System.Drawing.Point(316, 7);
            this.btnIdentifyMonitor.Name = "btnIdentifyMonitor";
            this.btnIdentifyMonitor.Size = new System.Drawing.Size(30, 30);
            this.btnIdentifyMonitor.TabIndex = 17;
            this.toolTips.SetToolTip(this.btnIdentifyMonitor, "Identify");
            this.btnIdentifyMonitor.UseVisualStyleBackColor = true;
            this.btnIdentifyMonitor.Click += new System.EventHandler(this.btnIdentifyMonitor_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 356);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.picSharpness);
            this.Controls.Add(this.lblSharpness);
            this.Controls.Add(this.barSharpness);
            this.Controls.Add(this.cboFactoryReset);
            this.Controls.Add(this.btnFactoryReset);
            this.Controls.Add(this.btnInput);
            this.Controls.Add(this.cboInput);
            this.Controls.Add(this.cboPower);
            this.Controls.Add(this.btnPower);
            this.Controls.Add(this.picVolume);
            this.Controls.Add(this.lblVolume);
            this.Controls.Add(this.btnDeleteProfile);
            this.Controls.Add(this.btnSaveProfile);
            this.Controls.Add(this.btnNewProfile);
            this.Controls.Add(this.barVolume);
            this.Controls.Add(this.picBlue);
            this.Controls.Add(this.picGreen);
            this.Controls.Add(this.picRed);
            this.Controls.Add(this.picContrast);
            this.Controls.Add(this.picBrightness);
            this.Controls.Add(this.barBlue);
            this.Controls.Add(this.barGreen);
            this.Controls.Add(this.barRed);
            this.Controls.Add(this.barContrast);
            this.Controls.Add(this.lstProfiles);
            this.Controls.Add(this.btnLinkMonitors);
            this.Controls.Add(this.btnIdentifyMonitor);
            this.Controls.Add(this.lblBlue);
            this.Controls.Add(this.barBrightness);
            this.Controls.Add(this.lblGreen);
            this.Controls.Add(this.lblBrightness);
            this.Controls.Add(this.lblRed);
            this.Controls.Add(this.lblContrast);
            this.Controls.Add(this.cboMonitors);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "MonitorProfiler";
            ((System.ComponentModel.ISupportInitialize)(this.barGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barSharpness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSharpness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBrightness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnNewProfile;
        private System.Windows.Forms.Button btnSaveProfile;
        private System.Windows.Forms.Button btnDeleteProfile;
        private System.Windows.Forms.TrackBar barBrightness;
        private System.Windows.Forms.Label lblBrightness;
        private System.Windows.Forms.Label lblContrast;
        private System.Windows.Forms.TrackBar barContrast;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.ComboBox cboMonitors;
        private System.Windows.Forms.TrackBar barRed;
        private System.Windows.Forms.TrackBar barGreen;
        private System.Windows.Forms.Label lblBlue;
        private System.Windows.Forms.Label lblGreen;
        private System.Windows.Forms.Label lblRed;
        private System.Windows.Forms.ListBox lstProfiles;
        private System.Windows.Forms.Button btnIdentifyMonitor;
        private System.Windows.Forms.Button btnPower;
        private System.Windows.Forms.ComboBox cboPower;
        private ComboBox cboInput;
        private Button btnInput;
        private Button btnLinkMonitors;
        private PictureBox picBrightness;
        private PictureBox picContrast;
        private PictureBox picRed;
        private PictureBox picGreen;
        private PictureBox picBlue;
        private TrackBar barBlue;
        private TrackBar barVolume;
        private Label lblVolume;
        private PictureBox picVolume;
        private ToolTip toolTips;
        private Button btnFactoryReset;
        private ComboBox cboFactoryReset;
        private PictureBox picSharpness;
        private Label lblSharpness;
        private TrackBar barSharpness;
        private Button btnRestart;
    }
}

