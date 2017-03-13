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

namespace MonitorProfiler
{
    public partial class frmMain : Form
    {
        private readonly MonitorCollection _monitorCollection = new MonitorCollection();
        private Monitor _currentMonitor;
        private Dictionary<TrackBar, TrackBarFeatures> _bars;
        private Config _config;

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
            cboInput.DataSource = new BindingSource(input, null);

            Dictionary<string, string> power = new Dictionary<string, string>();
            power.Add("Power on", "1");
            power.Add("Standby", "2");
            power.Add("Suspend", "3");
            power.Add("Reduced power off ", "4");
            power.Add("Power off", "5");
            power.Add("Sleep", "61808");
            cboPower.DataSource = new BindingSource(power, null);

            Dictionary<string, string> factoryreset = new Dictionary<string, string>();
            factoryreset.Add("Reset luminance", "5");
            factoryreset.Add("Reset colors", "8");
            factoryreset.Add("Reset factory defaults", "4");
            cboFactoryReset.DataSource = new BindingSource(factoryreset, null);

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

            if (m.RedGain.Max > 0)
            {
                Debug.WriteLine("RefreshSliders - RedGain.Max: " + m.RedGain.Max);
                barRed.Minimum = (int)m.RedGain.Min;
                barRed.Maximum = (int)m.RedGain.Max;
                barRed.Value = (int)m.RedGain.Current;
                barRed.Enabled = true;
            }
            else barRed.Enabled = false;

            Debug.WriteLine("RefreshSliders - GreenGain.Max: " + m.GreenGain.Max);
            if (m.GreenGain.Max > 0)
            {
                barGreen.Minimum = (int)m.GreenGain.Min;
                barGreen.Maximum = (int)m.GreenGain.Max;
                barGreen.Value = (int)m.GreenGain.Current;
                barGreen.Enabled = true;
            }
            else barGreen.Enabled = false;

            Debug.WriteLine("RefreshSliders - BlueGain.Max: " + m.BlueGain.Max);
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
                else if (barVolume.Value >= 14) picVolume.BackgroundImage = MonitorProfiler.Properties.Resources.speaker_high;
                else if (barVolume.Value >= 7) picVolume.BackgroundImage = MonitorProfiler.Properties.Resources.speaker_medium;
                else picVolume.BackgroundImage = MonitorProfiler.Properties.Resources.speaker_low;
                
                if (cboMonitors.Enabled == true)
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

        private void btnPower_Click(object sender, EventArgs e)
        {
            string value = ((KeyValuePair<string, string>)cboPower.SelectedItem).Value;
            // No VCP just force windows monitor sleeping
            if (value == "61808") NativeMethods.SendMessage(this.Handle, NativeConstants.WM_SYSCOMMAND, (IntPtr)NativeConstants.SC_MONITORSLEEP, (IntPtr)2);
            else
            {
                bool teest = NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORPOWER, Convert.ToUInt32(value));
                if (teest == false)
                {
                    Debug.WriteLine(Marshal.GetLastWin32Error());
                }
                else Debug.WriteLine(teest);
            }
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            string value = ((KeyValuePair<string, string>)cboInput.SelectedItem).Value;
            NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORINPUT, Convert.ToUInt32(value));
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
            if (cboMonitors.Enabled == true)
            {
                if(cboMonitors.Items.Count > 0) cboMonitors.SelectedIndex = 0;
                btnLinkMonitors.BackgroundImage = MonitorProfiler.Properties.Resources.link;
                cboMonitors.Enabled = false;
            } else
            {
                btnLinkMonitors.BackgroundImage = MonitorProfiler.Properties.Resources.unlink;
                cboMonitors.Enabled = true;
            }
        }

        private void picVolume_Click(object sender, EventArgs e)
        {
            if (barVolume.Value > 0) barVolume.Value = 0;
        }

        private void btnFactoryReset_Click(object sender, MouseEventArgs e)
        {

        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void btnFactoryReset_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to " + ((KeyValuePair<string, string>)cboFactoryReset.SelectedItem).Key.ToLower() + " on " + cboMonitors.SelectedItem + " ?" + (cboFactoryReset.SelectedIndex == 2 ? "\nAll the monitor settings will be reset !" : ""), "Warning !", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.OK)
            {
                string value = ((KeyValuePair<string, string>)cboFactoryReset.SelectedItem).Value;
                NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, Convert.ToByte(value), 1);
            }
        }
    }
}