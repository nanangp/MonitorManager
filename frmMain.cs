using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using MonitorProfiler.GUI;
using MonitorProfiler.Win32;
using MonitorProfiler.Models.Display;
using MonitorProfiler.Models.Configuration;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace MonitorProfiler
{
    public partial class frmMain : Form
    {
        private readonly MonitorCollection _monitorCollection = new MonitorCollection();
        private Monitor _currentMonitor;
        private Dictionary<TrackBar, TrackBarFeatures> _bars;
        private Config _config;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                /* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */

                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                NativeStructures.KeyModifier modifier = (NativeStructures.KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
                int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.


                //MessageBox.Show("Hotkey has been pressed!");
                WakeHotKey();
            }
        }
        
        public frmMain()
        {
            InitializeComponent();
            InitialiseProfiles();
            InitialiseTrackBars();

            Log("Started.");

            var @delegate = new NativeMethods.MonitorEnumDelegate(MonitorEnum);
            NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, @delegate, IntPtr.Zero);
            Log("Number of physical monitors: {0}", _monitorCollection.Count);

            int m = 1;
            foreach (Monitor monitor in _monitorCollection)
            {
 
                //NativeMethods.G GetCapabilitiesStringLength(hMonitor, out strSize);
                // IntPtr nullVal = IntPtr.Zero;
                // int currentValue;
                //int maxValue;
                //NativeMethods.GetVCPFeatureAndVCPFeatureReply(monitor.HPhysicalMonitor, 0x60, IntPtr.Zero, out currentValue, out maxValue);

                //Log("-----");
                //Log(monitor.Name);
                //Log(monitor.Index);
                //Log(monitor.HPhysicalMonitor);
                //Log("DDC : {0}", monitor.SupportsDDC);
                //Log("Brightness : {0}", monitor.Brightness.Supported);
                //Log("Contrast : {0}", monitor.Contrast.Supported);
                //Log("RGB Drive : {0}", monitor.RgbDrive.Supported);
                //Log("RGB Gain : {0}", monitor.RgbGain.Supported);

                cboMonitors.Items.Add(monitor.Name + " #" + m++);
            }

            if (cboMonitors.Items.Count > 0) cboMonitors.SelectedIndex = 0;


            Dictionary<string, string> input = new Dictionary<string, string>();
            input.Add("VGA-1", "1");
            input.Add("DVI-1", "2");
            input.Add("DVI-2", "3");
            input.Add("DisplayPort-1", "15");
            input.Add("DisplayPort-2", "16");
            input.Add("HDMI-1", "17");
            input.Add("HDMI-2", "18");
            //cboInput.DataSource = new BindingSource(input, null);

            Dictionary <string, string> power = new Dictionary<string, string>();
            power.Add("Power on", "1");
            power.Add("Standby", "2");
            power.Add("Suspend", "3");
            power.Add("Reduced power off ", "4");
            power.Add("Power off", "5");
            power.Add("Sleep", "61808");
            //cboPower.DataSource = new BindingSource(power, null);

            Dictionary<string, string> factoryreset = new Dictionary<string, string>();
            factoryreset.Add("Reset luminance", "5");
            factoryreset.Add("Reset colors", "8");
            factoryreset.Add("Reset factory defaults", "4");
            //cboFactoryReset.DataSource = new BindingSource(factoryreset, null);

            foreach (KeyValuePair<string, string> entry in input)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = entry.Key;
                item.Tag = entry.Value;
                if (_currentMonitor.Input.Current == Convert.ToInt32(entry.Value)) item.Checked = true;
                contextMenuInput.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { item });
            }
            //((ToolStripMenuItem)contextMenuInput.Items[1]).Checked = true;
            contextMenuInput.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenuInput_Click);

            foreach (KeyValuePair<string, string> entry in power)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = entry.Key;
                item.Tag = entry.Value;
                contextMenuPower.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { item });
            }
            //((ToolStripMenuItem)contextMenuPower.Items[1]).Checked = true;
            contextMenuPower.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenuPower_Click);

            foreach (KeyValuePair<string, string> entry in factoryreset)
            {
                ToolStripItem item = new ToolStripMenuItem();
                item.Text = entry.Key;
                item.Tag = entry.Value;
                contextMenuFactory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { item });
            }
            //((ToolStripMenuItem)contextMenuFactory.Items[1]).Checked = true;
            contextMenuFactory.ItemClicked += new ToolStripItemClickedEventHandler(this.contextMenuFactory_Click);

            // Register Winkey + 0 as global hotkey. 
            NativeMethods.RegisterHotKey(this.Handle, 0, (int)NativeStructures.KeyModifier.WinKey, Keys.NumPad0.GetHashCode()); 

            Log("");
            Log("Ready...");
            return;
        }
        
        private void OnDrawCbItem(object sender, DrawItemEventArgs e)
        {
            object item = ((ComboBox)sender).Items[e.Index];
            string s = "";
            Brush brush = Brushes.Black;
            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.FormatFlags = StringFormatFlags.NoWrap;
            format.Trimming = StringTrimming.None;

            if ((e.State & DrawItemState.ComboBoxEdit) != DrawItemState.ComboBoxEdit)
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) brush = Brushes.White;
                e.DrawBackground();
            }
            
            if (item.GetType() == typeof(KeyValuePair<string, string>)) s = ((KeyValuePair<string, string>)item).Key.ToString();
            else s = item.ToString();
            
            e.Graphics.DrawString(s, e.Font, brush, e.Bounds, format);
        }

        private void WakeHotKey() {
            for (int i = 0; i < cboMonitors.Items.Count; i++)
            {
                Debug.WriteLine("Waking monitor: " + i);
                bool teest = NativeMethods.SetVCPFeature(_monitorCollection[i].HPhysicalMonitor, NativeConstants.SC_MONITORPOWER, 1);
            }
        }

        private void btnFactoryReset_Click(object sender, EventArgs e)
        {
            Button button = ((Button)sender);
            contextMenuFactory.Show(PointToScreen(new Point(button.Location.X + 1, button.Location.Y + button.Height - 1)));
        }

        public void contextMenuFactory_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)e.ClickedItem;
            //item.Checked = !item.Checked;

            DialogResult result = MessageBox.Show("Are you sure you want to " + item.Text.ToLower() + " on " + cboMonitors.SelectedItem + " ?" + (Convert.ToInt32(item.Tag) == 4 ? "\nAll the monitor settings will be reset !" : ""), "Warning !", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                Debug.WriteLine("Reset: " + Convert.ToByte(item.Tag));
                NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, Convert.ToByte(item.Tag), 1);
            }
            /*
            NativeMethods.GetMonitorRedGreenOrBlueGain(_currentMonitor.HPhysicalMonitor, NativeStructures.MC_GAIN_TYPE.MC_RED_GAIN, ref _currentMonitor.RedGain.Min, ref _currentMonitor.RedGain.Current, ref _currentMonitor.RedGain.Max);
            NativeMethods.GetMonitorRedGreenOrBlueGain(_currentMonitor.HPhysicalMonitor, NativeStructures.MC_GAIN_TYPE.MC_GREEN_GAIN, ref _currentMonitor.GreenGain.Min, ref _currentMonitor.GreenGain.Current, ref _currentMonitor.GreenGain.Max);
            NativeMethods.GetMonitorRedGreenOrBlueGain(_currentMonitor.HPhysicalMonitor, NativeStructures.MC_GAIN_TYPE.MC_BLUE_GAIN, ref _currentMonitor.BlueGain.Min, ref _currentMonitor.BlueGain.Current, ref _currentMonitor.BlueGain.Max);
            */
            


            Task.Delay(2000).ContinueWith(t => testage());


            Debug.WriteLine("Refreshing");
            RefreshSliders(_currentMonitor);
        }

        private void testage()
        {
            NativeMethods.GetVCPFeatureAndVCPFeatureReply(_currentMonitor.HPhysicalMonitor, 0x16, IntPtr.Zero, ref _currentMonitor.RedGain.Current, ref _currentMonitor.RedGain.Max);
            NativeMethods.GetVCPFeatureAndVCPFeatureReply(_currentMonitor.HPhysicalMonitor, 0x18, IntPtr.Zero, ref _currentMonitor.GreenGain.Current, ref _currentMonitor.GreenGain.Max);
            NativeMethods.GetVCPFeatureAndVCPFeatureReply(_currentMonitor.HPhysicalMonitor, 0x1a, IntPtr.Zero, ref _currentMonitor.BlueGain.Current, ref _currentMonitor.BlueGain.Max);

            Debug.WriteLine("_currentMonitor.RedGain.Current " + _currentMonitor.RedGain.Current);
            Debug.WriteLine("_currentMonitor.GreenGain.Current " + _currentMonitor.GreenGain.Current);
            Debug.WriteLine("_currentMonitor.BlueGain.Current " + _currentMonitor.BlueGain.Current);
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            Button button = ((Button)sender);
            contextMenuInput.Show(PointToScreen(new Point(button.Location.X + 1, button.Location.Y + button.Height - 1)));
        }

        private void contextMenuInput_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)e.ClickedItem;
            //item.Checked = !item.Checked;

            NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORINPUT, Convert.ToUInt32(item.Tag));
            _currentMonitor.Input.Current = Convert.ToUInt32(item.Tag);
        }

        private void btnPower_Click(object sender, EventArgs e)
        {
            Button button = ((Button)sender);
            contextMenuPower.Show(PointToScreen(new Point(button.Location.X + 1, button.Location.Y + button.Height - 1)));
        }

        private void contextMenuPower_Click(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)e.ClickedItem;
            //item.Checked = !item.Checked;

            // No VCP just force windows monitor sleeping
            if (Convert.ToUInt32(item.Tag) == 61808) NativeMethods.SendMessage(this.Handle, NativeConstants.WM_SYSCOMMAND, (IntPtr)NativeConstants.SC_MONITORSLEEP, (IntPtr)2);
            NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORPOWER, Convert.ToUInt32(item.Tag));
        }
        private void ParseVCPStuff()
        {
          /*
            if(values)
                string[] valueArray = valuesStr.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

        values = Array.ConvertAll(valueArray, s => int.Parse(s, System.Globalization.NumberStyles.HexNumber));
            }

    // Prepare output.
    NativeStructures.MonitorSource[] sources = new NativeStructures.MonitorSource[values.Length];
            for (int i = 0; i<values.Length; ++i)
            {
                sources[i].code = values[i];
                if (0 <= values[i] && values[i] < sourceNames.Length) sources[i].name = sourceNames[values[i]];
                else sources[i].name = "**Unrecognized**";
            }*/
        }

        private void InitialiseProfiles()
        {
            lstProfiles.Items.Clear();
            var deserializer = new XmlSerializer(typeof(Config));
            using (var reader = new StreamReader("profiles.xml"))
            {
                _config = (Config)deserializer.Deserialize(reader);
            }
            foreach (var cfg in _config.Profiles)
                lstProfiles.Items.Add(cfg.Name);
        }

        private void SaveProfiles()
        {
            for (int i = _config.Profiles.Count - 1; i >= 0; i--)
            {
                if (_config.Profiles[i].Name == "Test") _config.Profiles.RemoveAt(i);
            }

            var serializer = new XmlSerializer(typeof(Config));
            using (var writer = new StreamWriter("profiles.xml"))
            {
               serializer.Serialize(writer, _config);
            }

            InitialiseProfiles();
        }

        ~frmMain()
        {
            foreach (Monitor monitor in _monitorCollection)
            {
                NativeMethods.DestroyPhysicalMonitor(monitor.HPhysicalMonitor);
            }
        }

        #region Private Methods

        // Writes message to a textbox and scroll it to bottom
        private void Log(object message, params object[] args)
        {
            txtLog.Text += Environment.NewLine + string.Format(message.ToString(), args);
            NativeMethods.ScrollToBottom(txtLog);
        }

        private void InitialiseTrackBars()
        {
            _bars = new Dictionary<TrackBar, TrackBarFeatures>{
                {barBrightness, new TrackBarFeatures(FeatureType.Brightness, lblBrightness)},
                {barContrast, new TrackBarFeatures(FeatureType.Contrast, lblContrast)},
                {barRed, new TrackBarFeatures(FeatureType.RedGain, lblRed)},
                {barGreen, new TrackBarFeatures(FeatureType.GreenGain, lblGreen)},
                {barBlue, new TrackBarFeatures(FeatureType.BlueGain, lblBlue)},
                {barSharpness, new TrackBarFeatures(FeatureType.Sharpness, lblSharpness)},
                {barVolume, new TrackBarFeatures(FeatureType.Volume, lblVolume)},
            };
        }

        // To be called by a delegate
        private bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref Rectangle lprcMonitor, IntPtr dwData)
        {
            _monitorCollection.Add(hMonitor);
            return true;
        }

        private void RefreshSliders(Monitor m)
        {
            Debug.WriteLine("RefreshSliders - Brightness.Max: " + m.Brightness.Max);
            if (m.Brightness.Max > 0)
            {
                barBrightness.Minimum = (int)m.Brightness.Min;
                barBrightness.Maximum = (int)m.Brightness.Max;
                barBrightness.Value = (int)m.Brightness.Current;
                barBrightness.Enabled = true;
            }
            else barBrightness.Enabled = false;

            Debug.WriteLine("RefreshSliders - Contrast.Max: " + m.Contrast.Max);
            if (m.Contrast.Max > 0)
            {
                barContrast.Minimum = (int)m.Contrast.Min;
                barContrast.Maximum = (int)m.Contrast.Max;
                barContrast.Value = (int)m.Contrast.Current;
                barContrast.Enabled = true;
            }
            else barContrast.Enabled = false;

            Debug.WriteLine("RefreshSliders - RedGain.Max: " + m.RedGain.Max);
            Debug.WriteLine("RefreshSliders - RedGain.Current: " + m.RedGain.Current);
            if (m.RedGain.Max > 0)
            {
                barRed.Minimum = (int)m.RedGain.Min;
                barRed.Maximum = (int)m.RedGain.Max;
                barRed.Value = (int)m.RedGain.Current;
                barRed.Enabled = true;
            }
            else barRed.Enabled = false;

            Debug.WriteLine("RefreshSliders - GreenGain.Max: " + m.GreenGain.Max);
            Debug.WriteLine("RefreshSliders - GreenGain.Current: " + m.GreenGain.Current);
            if (m.GreenGain.Max > 0)
            {
                barGreen.Minimum = (int)m.GreenGain.Min;
                barGreen.Maximum = (int)m.GreenGain.Max;
                barGreen.Value = (int)m.GreenGain.Current;
                barGreen.Enabled = true;
            }
            else barGreen.Enabled = false;

            Debug.WriteLine("RefreshSliders - BlueGain.Max: " + m.BlueGain.Max);
            Debug.WriteLine("RefreshSliders - BlueGain.Current: " + m.BlueGain.Current);
            if (m.BlueGain.Max > 0)
            {
                barBlue.Minimum = (int)m.BlueGain.Min;
                barBlue.Maximum = (int)m.BlueGain.Max;
                barBlue.Value = (int)m.BlueGain.Current;
                barBlue.Enabled = true;
            }
            else barBlue.Enabled = false;

            Debug.WriteLine("RefreshSliders - Sharpness.Max: " + m.Sharpness.Max);
            if (m.Sharpness.Max > 0)
            {
                barSharpness.TickFrequency = 5;
                if (m.Sharpness.Max < 20) barSharpness.TickFrequency = 1;
                barSharpness.LargeChange = barSharpness.TickFrequency;
                barSharpness.Minimum = 0;
                barSharpness.Maximum = (int)m.Sharpness.Max;
                barSharpness.Value = (int)m.Sharpness.Current;
                barSharpness.Enabled = true;
            }
            else barSharpness.Enabled = false;
            lblSharpness.Enabled = barSharpness.Enabled;

            Debug.WriteLine("RefreshSliders - Volume.Max: " + m.Volume.Max);
            if (m.Volume.Max > 0)
            {
                barVolume.Minimum = 0;
                barVolume.Maximum = (int)m.Volume.Max;
                barVolume.Value = (int)m.Volume.Current;
                barVolume.Enabled = true;
            }
            else barVolume.Enabled = false;
            lblVolume.Enabled = barVolume.Enabled;

            /*Debug.WriteLine("RefreshSliders - Volume.Max: " + m.Volume.Max);
            cboInput.SelectedIndex = monitorCfg.Input;
            cboPower.SelectedIndex = monitorCfg.Power;*/
        }

        #endregion

        private void TrackBar_ValueChanged(object sender, EventArgs e)
        {
            try
            {   // Assumes sender is a trackbar, and exists in the collection. Don't worry about errors :)
                if (barVolume.Value <= 0) picVolume.BackgroundImage = MonitorProfiler.Properties.Resources.speaker_mute;
                else if (barVolume.Value >= 50) picVolume.BackgroundImage = MonitorProfiler.Properties.Resources.speaker_high;
                else picVolume.BackgroundImage = MonitorProfiler.Properties.Resources.speaker_low;
                
                if ((string)btnLinkMonitors.Tag == "unlink")
                {
                    Debug.WriteLine("TrackBar_ValueChanged (no link)");
                    _bars[sender as TrackBar].UpdateScreenWithBarValue(sender as TrackBar, _currentMonitor);
                } else
                {
                    Debug.WriteLine("TrackBar_ValueChanged (linked)");
                    for (int i = 0; i < cboMonitors.Items.Count; i++)
                    {
                        Debug.WriteLine("TrackBar_ValueChanged monitor: " + i);
                        _bars[sender as TrackBar].UpdateScreenWithBarValue(sender as TrackBar, _monitorCollection[i]);
                    }
                }
            }
            catch{}
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            barBrightness.Value = (int) _currentMonitor.Brightness.Original;
            barContrast.Value = (int) _currentMonitor.Contrast.Original;
            barRed.Value = (int)_currentMonitor.RedGain.Original;
            barGreen.Value = (int)_currentMonitor.GreenGain.Original;
            barBlue.Value = (int)_currentMonitor.BlueGain.Original;
            barSharpness.Value = (int)_currentMonitor.Sharpness.Original;
            barVolume.Value = (int)_currentMonitor.Volume.Original;
        }

        private void cboMonitors_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentMonitor = _monitorCollection[cboMonitors.SelectedIndex];
            RefreshSliders(_currentMonitor);
        }

        private void btnIdentifyMonitor_Click(object sender, EventArgs e)
        {
            uint _currentBrightness = _currentMonitor.Brightness.Current;
            uint _currentContrast = _currentMonitor.Contrast.Current;

            // Dim the monitor
            if (_currentBrightness < 30 && _currentContrast < 30)
            {
                NativeMethods.SetMonitorBrightness(_currentMonitor.HPhysicalMonitor, _currentBrightness + 30);
                NativeMethods.SetMonitorContrast(_currentMonitor.HPhysicalMonitor, _currentContrast + 30);
            }
            else
            {
                uint minBrightness = (uint)((int)_currentBrightness - 30 >= 0 ? (int)_currentBrightness - 30 : 0);
                uint minContrast = (uint)((int)_currentContrast - 30 >= 0 ? (int)_currentContrast - 30 : 0);
                NativeMethods.SetMonitorBrightness(_currentMonitor.HPhysicalMonitor, minBrightness);
                NativeMethods.SetMonitorContrast(_currentMonitor.HPhysicalMonitor, minContrast);
            }
            System.Threading.Thread.Sleep(100);
            // Revert
            NativeMethods.SetMonitorBrightness(_currentMonitor.HPhysicalMonitor, _currentBrightness); 
            NativeMethods.SetMonitorContrast(_currentMonitor.HPhysicalMonitor, _currentContrast); 
        }

        private void lstProfiles_SelectedValueChanged(object sender, EventArgs e)
        {
            // Exception occured when clicking white space of the list (index = -1)
            if (lstProfiles.SelectedIndex >= 0)
            {
                var selectedProfile = _config.Profiles.Where(p => p.Name == lstProfiles.SelectedItem.ToString()).FirstOrDefault();

                if (selectedProfile == null)
                    return;

                foreach (var monitorCfg in selectedProfile.MonitorConfigs)
                {
                    if (monitorCfg.Index >= cboMonitors.Items.Count)
                        continue;

                    cboMonitors.SelectedIndex = monitorCfg.Index;
                    barBrightness.Value = monitorCfg.Brightness;
                    barContrast.Value = monitorCfg.Contrast;
                    barRed.Value = monitorCfg.Red;
                    barGreen.Value = monitorCfg.Green;
                    barBlue.Value = monitorCfg.Blue;
                    barVolume.Value = monitorCfg.Volume;
                }
            }
        }

        private void btnSaveProfile_Click(object sender, EventArgs e)
        {
            SaveProfiles();
        }

        private void btnDeleteProfile_Click(object sender, EventArgs e)
        {

        }

        private void btnLinkMonitors_Click(object sender, EventArgs e)
        {
            if ((string)btnLinkMonitors.Tag == "unlink")
            {
                btnLinkMonitors.BackgroundImage = Properties.Resources.link;
                btnLinkMonitors.Tag = "link";
            }
            else
            {
                btnLinkMonitors.BackgroundImage = MonitorProfiler.Properties.Resources.unlink;
                btnLinkMonitors.Tag = "unlink";
            }
        }

        private void picVolume_Click(object sender, EventArgs e)
        {
            if (barVolume.Value > 0) barVolume.Value = 0;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Unregister hotkey with id 0 before closing the form. You might want to call this more than once with different id values if you are planning to register more than one hotkey.
            NativeMethods.UnregisterHotKey(this.Handle, 0);
        }
    }
}