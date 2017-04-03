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
            this.cboMonitors = new System.Windows.Forms.ComboBox();
            this.lblContrast = new System.Windows.Forms.Label();
            this.barContrast = new System.Windows.Forms.TrackBar();
            this.lblBrightness = new System.Windows.Forms.Label();
            this.barBrightness = new System.Windows.Forms.TrackBar();
            this.lblGainRed = new System.Windows.Forms.Label();
            this.barGainRed = new System.Windows.Forms.TrackBar();
            this.lblGainGreen = new System.Windows.Forms.Label();
            this.barGainGreen = new System.Windows.Forms.TrackBar();
            this.lblGainBlue = new System.Windows.Forms.Label();
            this.barGainBlue = new System.Windows.Forms.TrackBar();
            this.lblSharpness = new System.Windows.Forms.Label();
            this.barSharpness = new System.Windows.Forms.TrackBar();
            this.lblVolume = new System.Windows.Forms.Label();
            this.barVolume = new System.Windows.Forms.TrackBar();
            this.btnRevert = new System.Windows.Forms.Button();
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuFactory = new System.Windows.Forms.ContextMenu();
            this.contextMenuInput = new System.Windows.Forms.ContextMenu();
            this.contextMenuPower = new System.Windows.Forms.ContextMenu();
            this.contextMenuProfiles = new System.Windows.Forms.ContextMenu();
            this.btnFactoryReset = new System.Windows.Forms.Button();
            this.btnInput = new System.Windows.Forms.Button();
            this.btnPower = new System.Windows.Forms.Button();
            this.btnProfiles = new System.Windows.Forms.Button();
            this.picVolume = new System.Windows.Forms.PictureBox();
            this.picSharpness = new System.Windows.Forms.PictureBox();
            this.picGainBlue = new System.Windows.Forms.PictureBox();
            this.picGainGreen = new System.Windows.Forms.PictureBox();
            this.picGainRed = new System.Windows.Forms.PictureBox();
            this.picContrast = new System.Windows.Forms.PictureBox();
            this.picBrightness = new System.Windows.Forms.PictureBox();
            this.btnRestart = new System.Windows.Forms.Button();
            this.btnIdentifyMonitor = new System.Windows.Forms.Button();
            this.btnLinkMonitors = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.barContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barGainRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barGainGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barGainBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barSharpness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSharpness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGainBlue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGainGreen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGainRed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBrightness)).BeginInit();
            this.SuspendLayout();
            // 
            // cboMonitors
            // 
            this.cboMonitors.BackColor = System.Drawing.Color.White;
            this.cboMonitors.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboMonitors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonitors.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboMonitors.FormattingEnabled = true;
            this.cboMonitors.ItemHeight = 32;
            this.cboMonitors.Location = new System.Drawing.Point(8, 8);
            this.cboMonitors.Margin = new System.Windows.Forms.Padding(0);
            this.cboMonitors.Name = "cboMonitors";
            this.cboMonitors.Size = new System.Drawing.Size(141, 38);
            this.cboMonitors.TabIndex = 1;
            this.cboMonitors.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.OnDrawCbItem);
            this.cboMonitors.SelectedIndexChanged += new System.EventHandler(this.cboMonitors_SelectedIndexChanged);
            // 
            // lblContrast
            // 
            this.lblContrast.AutoSize = true;
            this.lblContrast.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContrast.Location = new System.Drawing.Point(241, 91);
            this.lblContrast.Name = "lblContrast";
            this.lblContrast.Size = new System.Drawing.Size(23, 15);
            this.lblContrast.TabIndex = 18;
            this.lblContrast.Text = "0%";
            // 
            // barContrast
            // 
            this.barContrast.Location = new System.Drawing.Point(39, 89);
            this.barContrast.Maximum = 100;
            this.barContrast.Name = "barContrast";
            this.barContrast.Size = new System.Drawing.Size(196, 45);
            this.barContrast.TabIndex = 6;
            this.barContrast.TickFrequency = 5;
            this.barContrast.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // lblBrightness
            // 
            this.lblBrightness.AutoSize = true;
            this.lblBrightness.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBrightness.Location = new System.Drawing.Point(241, 61);
            this.lblBrightness.Name = "lblBrightness";
            this.lblBrightness.Size = new System.Drawing.Size(23, 15);
            this.lblBrightness.TabIndex = 17;
            this.lblBrightness.Text = "0%";
            // 
            // barBrightness
            // 
            this.barBrightness.Location = new System.Drawing.Point(39, 59);
            this.barBrightness.Maximum = 100;
            this.barBrightness.Name = "barBrightness";
            this.barBrightness.Size = new System.Drawing.Size(196, 45);
            this.barBrightness.TabIndex = 5;
            this.barBrightness.TickFrequency = 5;
            this.barBrightness.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // lblGainRed
            // 
            this.lblGainRed.AutoSize = true;
            this.lblGainRed.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGainRed.Location = new System.Drawing.Point(241, 121);
            this.lblGainRed.Name = "lblGainRed";
            this.lblGainRed.Size = new System.Drawing.Size(23, 15);
            this.lblGainRed.TabIndex = 19;
            this.lblGainRed.Text = "0%";
            this.lblGainRed.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // barGainRed
            // 
            this.barGainRed.Location = new System.Drawing.Point(39, 119);
            this.barGainRed.Maximum = 100;
            this.barGainRed.Name = "barGainRed";
            this.barGainRed.Size = new System.Drawing.Size(196, 45);
            this.barGainRed.TabIndex = 7;
            this.barGainRed.TickFrequency = 5;
            this.barGainRed.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // lblGainGreen
            // 
            this.lblGainGreen.AutoSize = true;
            this.lblGainGreen.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGainGreen.Location = new System.Drawing.Point(241, 151);
            this.lblGainGreen.Name = "lblGainGreen";
            this.lblGainGreen.Size = new System.Drawing.Size(23, 15);
            this.lblGainGreen.TabIndex = 20;
            this.lblGainGreen.Text = "0%";
            this.lblGainGreen.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // barGainGreen
            // 
            this.barGainGreen.Location = new System.Drawing.Point(39, 149);
            this.barGainGreen.Maximum = 100;
            this.barGainGreen.Name = "barGainGreen";
            this.barGainGreen.Size = new System.Drawing.Size(196, 45);
            this.barGainGreen.TabIndex = 8;
            this.barGainGreen.TickFrequency = 5;
            this.barGainGreen.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // lblGainBlue
            // 
            this.lblGainBlue.AutoSize = true;
            this.lblGainBlue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGainBlue.Location = new System.Drawing.Point(241, 181);
            this.lblGainBlue.Name = "lblGainBlue";
            this.lblGainBlue.Size = new System.Drawing.Size(23, 15);
            this.lblGainBlue.TabIndex = 21;
            this.lblGainBlue.Text = "0%";
            this.lblGainBlue.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // barGainBlue
            // 
            this.barGainBlue.Location = new System.Drawing.Point(39, 179);
            this.barGainBlue.Maximum = 100;
            this.barGainBlue.Name = "barGainBlue";
            this.barGainBlue.Size = new System.Drawing.Size(196, 45);
            this.barGainBlue.TabIndex = 9;
            this.barGainBlue.TickFrequency = 5;
            this.barGainBlue.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // lblSharpness
            // 
            this.lblSharpness.AutoSize = true;
            this.lblSharpness.Enabled = false;
            this.lblSharpness.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSharpness.Location = new System.Drawing.Point(241, 211);
            this.lblSharpness.Name = "lblSharpness";
            this.lblSharpness.Size = new System.Drawing.Size(13, 15);
            this.lblSharpness.TabIndex = 22;
            this.lblSharpness.Text = "0";
            this.lblSharpness.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // barSharpness
            // 
            this.barSharpness.Enabled = false;
            this.barSharpness.Location = new System.Drawing.Point(39, 209);
            this.barSharpness.Maximum = 100;
            this.barSharpness.Name = "barSharpness";
            this.barSharpness.Size = new System.Drawing.Size(196, 45);
            this.barSharpness.TabIndex = 10;
            this.barSharpness.TickFrequency = 5;
            this.barSharpness.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Enabled = false;
            this.lblVolume.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolume.Location = new System.Drawing.Point(241, 241);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(23, 15);
            this.lblVolume.TabIndex = 23;
            this.lblVolume.Text = "0%";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // barVolume
            // 
            this.barVolume.Enabled = false;
            this.barVolume.Location = new System.Drawing.Point(39, 239);
            this.barVolume.Maximum = 100;
            this.barVolume.Name = "barVolume";
            this.barVolume.Size = new System.Drawing.Size(196, 45);
            this.barVolume.TabIndex = 11;
            this.barVolume.TickFrequency = 5;
            this.barVolume.ValueChanged += new System.EventHandler(this.TrackBar_ValueChanged);
            // 
            // btnRevert
            // 
            this.btnRevert.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRevert.Location = new System.Drawing.Point(197, 272);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(75, 40);
            this.btnRevert.TabIndex = 16;
            this.btnRevert.Text = "Revert";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // btnFactoryReset
            // 
            this.btnFactoryReset.BackgroundImage = global::MonitorProfiler.Properties.Resources.undo;
            this.btnFactoryReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFactoryReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFactoryReset.ForeColor = System.Drawing.Color.White;
            this.btnFactoryReset.Location = new System.Drawing.Point(128, 272);
            this.btnFactoryReset.Name = "btnFactoryReset";
            this.btnFactoryReset.Size = new System.Drawing.Size(40, 40);
            this.btnFactoryReset.TabIndex = 15;
            this.btnFactoryReset.UseVisualStyleBackColor = true;
            this.btnFactoryReset.Click += new System.EventHandler(this.btnFactoryReset_Click);
            // 
            // btnInput
            // 
            this.btnInput.BackgroundImage = global::MonitorProfiler.Properties.Resources.split;
            this.btnInput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnInput.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInput.ForeColor = System.Drawing.Color.White;
            this.btnInput.Location = new System.Drawing.Point(88, 272);
            this.btnInput.Margin = new System.Windows.Forms.Padding(0);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(40, 40);
            this.btnInput.TabIndex = 14;
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // btnPower
            // 
            this.btnPower.BackgroundImage = global::MonitorProfiler.Properties.Resources.power;
            this.btnPower.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPower.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPower.ForeColor = System.Drawing.Color.White;
            this.btnPower.Location = new System.Drawing.Point(48, 272);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(40, 40);
            this.btnPower.TabIndex = 13;
            this.btnPower.UseVisualStyleBackColor = true;
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // btnProfiles
            // 
            this.btnProfiles.BackColor = System.Drawing.Color.Transparent;
            this.btnProfiles.BackgroundImage = global::MonitorProfiler.Properties.Resources.profiles;
            this.btnProfiles.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnProfiles.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProfiles.ForeColor = System.Drawing.Color.White;
            this.btnProfiles.Location = new System.Drawing.Point(7, 272);
            this.btnProfiles.Name = "btnProfiles";
            this.btnProfiles.Size = new System.Drawing.Size(40, 40);
            this.btnProfiles.TabIndex = 12;
            this.toolTips.SetToolTip(this.btnProfiles, "Profiles");
            this.btnProfiles.UseVisualStyleBackColor = false;
            this.btnProfiles.Click += new System.EventHandler(this.btnProfiles_Click);
            // 
            // picVolume
            // 
            this.picVolume.BackgroundImage = global::MonitorProfiler.Properties.Resources.speaker_high;
            this.picVolume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picVolume.Location = new System.Drawing.Point(5, 233);
            this.picVolume.Name = "picVolume";
            this.picVolume.Size = new System.Drawing.Size(32, 32);
            this.picVolume.TabIndex = 34;
            this.picVolume.TabStop = false;
            this.toolTips.SetToolTip(this.picVolume, "Mute");
            this.picVolume.Click += new System.EventHandler(this.picVolume_Click);
            // 
            // picSharpness
            // 
            this.picSharpness.BackgroundImage = global::MonitorProfiler.Properties.Resources.sharpness;
            this.picSharpness.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picSharpness.Location = new System.Drawing.Point(5, 203);
            this.picSharpness.Name = "picSharpness";
            this.picSharpness.Size = new System.Drawing.Size(32, 32);
            this.picSharpness.TabIndex = 39;
            this.picSharpness.TabStop = false;
            this.toolTips.SetToolTip(this.picSharpness, "Sharpness");
            // 
            // picGainBlue
            // 
            this.picGainBlue.BackgroundImage = global::MonitorProfiler.Properties.Resources.rgbblue;
            this.picGainBlue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picGainBlue.Location = new System.Drawing.Point(5, 173);
            this.picGainBlue.Name = "picGainBlue";
            this.picGainBlue.Size = new System.Drawing.Size(32, 32);
            this.picGainBlue.TabIndex = 31;
            this.picGainBlue.TabStop = false;
            this.toolTips.SetToolTip(this.picGainBlue, "Blue gain");
            // 
            // picGainGreen
            // 
            this.picGainGreen.BackgroundImage = global::MonitorProfiler.Properties.Resources.rgbgreen;
            this.picGainGreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picGainGreen.Location = new System.Drawing.Point(5, 143);
            this.picGainGreen.Name = "picGainGreen";
            this.picGainGreen.Size = new System.Drawing.Size(32, 32);
            this.picGainGreen.TabIndex = 30;
            this.picGainGreen.TabStop = false;
            this.toolTips.SetToolTip(this.picGainGreen, "Green gain");
            // 
            // picGainRed
            // 
            this.picGainRed.BackgroundImage = global::MonitorProfiler.Properties.Resources.rgbred;
            this.picGainRed.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picGainRed.Location = new System.Drawing.Point(5, 113);
            this.picGainRed.Name = "picGainRed";
            this.picGainRed.Size = new System.Drawing.Size(32, 32);
            this.picGainRed.TabIndex = 29;
            this.picGainRed.TabStop = false;
            this.toolTips.SetToolTip(this.picGainRed, "Red gain");
            // 
            // picContrast
            // 
            this.picContrast.BackgroundImage = global::MonitorProfiler.Properties.Resources.contrast;
            this.picContrast.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picContrast.Location = new System.Drawing.Point(5, 83);
            this.picContrast.Name = "picContrast";
            this.picContrast.Size = new System.Drawing.Size(32, 32);
            this.picContrast.TabIndex = 28;
            this.picContrast.TabStop = false;
            this.toolTips.SetToolTip(this.picContrast, "Contrast");
            // 
            // picBrightness
            // 
            this.picBrightness.BackgroundImage = global::MonitorProfiler.Properties.Resources.brightness;
            this.picBrightness.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picBrightness.Location = new System.Drawing.Point(5, 53);
            this.picBrightness.Name = "picBrightness";
            this.picBrightness.Size = new System.Drawing.Size(32, 32);
            this.picBrightness.TabIndex = 27;
            this.picBrightness.TabStop = false;
            this.toolTips.SetToolTip(this.picBrightness, "Brightness");
            // 
            // btnRestart
            // 
            this.btnRestart.BackgroundImage = global::MonitorProfiler.Properties.Resources.refresh;
            this.btnRestart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestart.ForeColor = System.Drawing.Color.White;
            this.btnRestart.Location = new System.Drawing.Point(232, 7);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(40, 40);
            this.btnRestart.TabIndex = 4;
            this.toolTips.SetToolTip(this.btnRestart, "Identify");
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // btnIdentifyMonitor
            // 
            this.btnIdentifyMonitor.BackgroundImage = global::MonitorProfiler.Properties.Resources.light;
            this.btnIdentifyMonitor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnIdentifyMonitor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIdentifyMonitor.ForeColor = System.Drawing.Color.White;
            this.btnIdentifyMonitor.Location = new System.Drawing.Point(192, 7);
            this.btnIdentifyMonitor.Name = "btnIdentifyMonitor";
            this.btnIdentifyMonitor.Size = new System.Drawing.Size(40, 40);
            this.btnIdentifyMonitor.TabIndex = 3;
            this.toolTips.SetToolTip(this.btnIdentifyMonitor, "Identify");
            this.btnIdentifyMonitor.UseVisualStyleBackColor = true;
            this.btnIdentifyMonitor.Click += new System.EventHandler(this.btnIdentifyMonitor_Click);
            // 
            // btnLinkMonitors
            // 
            this.btnLinkMonitors.BackgroundImage = global::MonitorProfiler.Properties.Resources.unlink;
            this.btnLinkMonitors.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnLinkMonitors.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLinkMonitors.ForeColor = System.Drawing.Color.White;
            this.btnLinkMonitors.Location = new System.Drawing.Point(152, 7);
            this.btnLinkMonitors.Name = "btnLinkMonitors";
            this.btnLinkMonitors.Size = new System.Drawing.Size(40, 40);
            this.btnLinkMonitors.TabIndex = 2;
            this.btnLinkMonitors.Tag = "unlink";
            this.toolTips.SetToolTip(this.btnLinkMonitors, "Link all");
            this.btnLinkMonitors.UseVisualStyleBackColor = true;
            this.btnLinkMonitors.Click += new System.EventHandler(this.btnLinkMonitors_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(279, 319);
            this.Controls.Add(this.btnRevert);
            this.Controls.Add(this.btnFactoryReset);
            this.Controls.Add(this.btnInput);
            this.Controls.Add(this.btnPower);
            this.Controls.Add(this.btnProfiles);
            this.Controls.Add(this.lblVolume);
            this.Controls.Add(this.barVolume);
            this.Controls.Add(this.picVolume);
            this.Controls.Add(this.lblSharpness);
            this.Controls.Add(this.barSharpness);
            this.Controls.Add(this.picSharpness);
            this.Controls.Add(this.lblGainBlue);
            this.Controls.Add(this.barGainBlue);
            this.Controls.Add(this.picGainBlue);
            this.Controls.Add(this.lblGainGreen);
            this.Controls.Add(this.barGainGreen);
            this.Controls.Add(this.picGainGreen);
            this.Controls.Add(this.lblGainRed);
            this.Controls.Add(this.barGainRed);
            this.Controls.Add(this.picGainRed);
            this.Controls.Add(this.lblContrast);
            this.Controls.Add(this.barContrast);
            this.Controls.Add(this.picContrast);
            this.Controls.Add(this.lblBrightness);
            this.Controls.Add(this.barBrightness);
            this.Controls.Add(this.picBrightness);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnIdentifyMonitor);
            this.Controls.Add(this.btnLinkMonitors);
            this.Controls.Add(this.cboMonitors);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "MonitorProfiler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.barContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barGainRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barGainGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barGainBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barSharpness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSharpness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGainBlue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGainGreen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picGainRed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBrightness)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ComboBox cboMonitors;
        private TrackBar barBrightness;
        private TrackBar barContrast;
        private TrackBar barGainRed;
        private TrackBar barGainGreen;
        private TrackBar barGainBlue;
        private TrackBar barSharpness;
        private TrackBar barVolume;
        private Label lblBrightness;
        private Label lblContrast;
        private Label lblGainRed;
        private Label lblGainGreen;
        private Label lblGainBlue;
        private Label lblSharpness;
        private Label lblVolume;
        private PictureBox picBrightness;
        private PictureBox picContrast;
        private PictureBox picGainRed;
        private PictureBox picGainGreen;
        private PictureBox picGainBlue;
        private PictureBox picSharpness;
        private PictureBox picVolume;
        private Button btnFactoryReset;
        private Button btnIdentifyMonitor;
        private Button btnInput;
        private Button btnLinkMonitors;
        private Button btnPower;
        private Button btnProfiles;
        private Button btnRestart;
        private Button btnRevert;
        private ToolTip toolTips;
        private ContextMenu contextMenuPower;
        private ContextMenu contextMenuInput;
        private ContextMenu contextMenuFactory;
        private ContextMenu contextMenuProfiles;
    }
}

