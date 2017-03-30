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
        private static System.Timers.Timer _timer;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                /* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */

                // The key of the hotkey that was pressed.
                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                // The modifier of the hotkey that was pressed.
                NativeStructures.KeyModifier modifier = (NativeStructures.KeyModifier)((int)m.LParam & 0xFFFF);
                // The id of the hotkey that was pressed.
                int id = m.WParam.ToInt32();

                //MessageBox.Show("Hotkey has been pressed!");
                WakeHotKey();
            }
        }
        
        public frmMain()
        {
            InitializeComponent();
            InitialiseProfiles();
            InitialiseTrackBars();
            InitialiseButtons();

            //Log("Started.");

            var @delegate = new NativeMethods.MonitorEnumDelegate(MonitorEnum);
            NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, @delegate, IntPtr.Zero);
            
            /*
            Debug.Write("\n\nStart Add");
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            */

            // Trying to get all monitors info in parrallel but for some reason it's not faster / working
            Parallel.ForEach(_monitorCollection, (monitor) =>
            {
                monitor.CheckCapabilities();
            });

            /*
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Debug.Write("Checking duration: " + ts.ToString() + "\n");
            */

            //("Number of physical monitors: {0}", _monitorCollection.Count);

            int m = 1;
            foreach (Monitor monitor in _monitorCollection)
            {
                cboMonitors.Items.Add(monitor.Name + " #" + m++);
            }

            if (cboMonitors.Items.Count > 0) cboMonitors.SelectedIndex = 0;
            
            // Register Winkey + 0 as global hotkey.
            NativeMethods.RegisterHotKey(this.Handle, 0, (int)NativeStructures.KeyModifier.WinKey, Keys.NumPad0.GetHashCode()); 

            //Log("");
            //Log("Ready...");
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
            Button button = (Button)sender;
            contextMenuFactory.Show(button, new Point(1, button.Height - 1));
        }

        public void contextMenuFactory_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;

            DialogResult result = MessageBox.Show("Are you sure you want to " + item.Text.ToLower() + " on " + cboMonitors.SelectedItem + " ?" + (Convert.ToInt32(item.Tag) == 4 ? "\nAll the monitor settings will be reset !" : ""), "Warning !", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                Debug.WriteLine("Reset: " + Convert.ToByte(item.Tag));
                NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, Convert.ToByte(item.Tag), 1);

                // Create a timer with a two second interval.
                if (Convert.ToUInt32(item.Tag) == 4) _timer = new System.Timers.Timer(2000);
                else _timer = new System.Timers.Timer(100);
                // Hook up the Elapsed event for the timer. 
                if (Convert.ToUInt32(item.Tag) == 5) _timer.Elapsed += TimerResetLuminance;
                if (Convert.ToUInt32(item.Tag) == 8) _timer.Elapsed += TimerResetColors;
                if (Convert.ToUInt32(item.Tag) == 4) _timer.Elapsed += TimerResetFactory;
                _timer.AutoReset = false;
                _timer.Enabled = true;
                _timer.SynchronizingObject = this;
            }
        }

        private void TimerResetColors(Object source, System.Timers.ElapsedEventArgs e)
        {
            _currentMonitor.CheckRgbDrive();
            _currentMonitor.CheckRgbGain();
            RefreshSliders(_currentMonitor);
        }

        private void TimerResetLuminance(Object source, System.Timers.ElapsedEventArgs e)
        {
            _currentMonitor.CheckBrightness();
            _currentMonitor.CheckContrast();
            RefreshSliders(_currentMonitor);
        }

        private void TimerResetFactory(Object source, System.Timers.ElapsedEventArgs e)
        {
            _currentMonitor.CheckCapabilities();
            RefreshSliders(_currentMonitor);
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            Button button = ((Button)sender);
            _currentMonitor.CheckInput();
            RefreshSourcesMenu();
            contextMenuInput.Show(button, new Point(1, button.Height - 1));
        }

        private void contextMenuInput_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;

            NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORINPUT, Convert.ToUInt32(item.Tag));
            _currentMonitor.Source.Current = Convert.ToUInt32(item.Tag);
        }

        private void btnPower_Click(object sender, EventArgs e)
        {
            Button button = ((Button)sender);
            _currentMonitor.CheckPower();
            RefreshPowerMenu();
            contextMenuPower.Show(button, new Point(1, button.Height - 1));
        }

        private void contextMenuPower_Click(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;

            Debug.WriteLine("Power : " + item.Tag);
            // No VCP just force windows monitor sleeping
            if (Convert.ToUInt32(item.Tag) == 61808)
            {
                NativeMethods.SendMessage(this.Handle, NativeConstants.WM_SYSCOMMAND, (IntPtr)NativeConstants.SC_MONITORSLEEP, (IntPtr)2);
            }
            else NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORPOWER, Convert.ToUInt32(item.Tag));
        }

        private void btnProfiles_Click(object sender, EventArgs e)
        {
            Button button = ((Button)sender);
            contextMenuProfiles.Show(button, new Point(1, button.Height - 1));
        }

        ~frmMain()
        {
            foreach (Monitor monitor in _monitorCollection)
            {
                NativeMethods.DestroyPhysicalMonitor(monitor.HPhysicalMonitor);
            }
        }

        #region Private Methods
/*
        // Writes message to a textbox and scroll it to bottom
        private void Log(object message, params object[] args)
        {
            txtLog.Text += Environment.NewLine + string.Format(message.ToString(), args);
            NativeMethods.ScrollToBottom(txtLog);
        }
*/
        private void InitialiseTrackBars()
        {
            _bars = new Dictionary<TrackBar, TrackBarFeatures>{
                {barBrightness, new TrackBarFeatures(FeatureType.Brightness, lblBrightness)},
                {barContrast, new TrackBarFeatures(FeatureType.Contrast, lblContrast)},
                {barGainRed, new TrackBarFeatures(FeatureType.GainRed, lblGainRed)},
                {barGainGreen, new TrackBarFeatures(FeatureType.GainGreen, lblGainGreen)},
                {barGainBlue, new TrackBarFeatures(FeatureType.GainBlue, lblGainBlue)},
                {barSharpness, new TrackBarFeatures(FeatureType.Sharpness, lblSharpness)},
                {barVolume, new TrackBarFeatures(FeatureType.Volume, lblVolume)},
            };
        }

        private void InitialiseButtons()
        {
            Dictionary<string, string> factoryreset = new Dictionary<string, string>();
            factoryreset.Add("Reset luminance", "5");
            factoryreset.Add("Reset colors", "8");
            factoryreset.Add("Reset factory defaults", "4");
            //cboFactoryReset.DataSource = new BindingSource(factoryreset, null);

            foreach (KeyValuePair<string, string> entry in factoryreset)
            {
                MenuItem item = new MenuItem();
                item.RadioCheck = true;
                item.Text = entry.Key;
                item.Tag = entry.Value;
                item.Click += new EventHandler(contextMenuFactory_Click);
                contextMenuFactory.MenuItems.Add(item);
            }
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

            Debug.WriteLine("RefreshSliders - GainRed.Max: " + m.GainRed.Max);
            Debug.WriteLine("RefreshSliders - GainRed.Current: " + m.GainRed.Current);
            if (m.GainRed.Max > 0)
            {
                barGainRed.Minimum = (int)m.GainRed.Min;
                barGainRed.Maximum = (int)m.GainRed.Max;
                barGainRed.Value = (int)m.GainRed.Current;
                barGainRed.Enabled = true;
            }
            else barGainRed.Enabled = false;

            Debug.WriteLine("RefreshSliders - GainGreen.Max: " + m.GainGreen.Max);
            Debug.WriteLine("RefreshSliders - GainGreen.Current: " + m.GainGreen.Current);
            if (m.GainGreen.Max > 0)
            {
                barGainGreen.Minimum = (int)m.GainGreen.Min;
                barGainGreen.Maximum = (int)m.GainGreen.Max;
                barGainGreen.Value = (int)m.GainGreen.Current;
                barGainGreen.Enabled = true;
            }
            else barGainGreen.Enabled = false;

            Debug.WriteLine("RefreshSliders - GainBlue.Max: " + m.GainBlue.Max);
            Debug.WriteLine("RefreshSliders - GainBlue.Current: " + m.GainBlue.Current);
            if (m.GainBlue.Max > 0)
            {
                barGainBlue.Minimum = (int)m.GainBlue.Min;
                barGainBlue.Maximum = (int)m.GainBlue.Max;
                barGainBlue.Value = (int)m.GainBlue.Current;
                barGainBlue.Enabled = true;
            }
            else barGainBlue.Enabled = false;

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

            RefreshPowerMenu();
            RefreshSourcesMenu();
        }

        private void RefreshPowerMenu()
        {
            contextMenuPower.MenuItems.Clear();
            if (_currentMonitor.PowerModes.Length > 0)
            {
                for (int i = 0; i < _currentMonitor.PowerModes.Length; i++)
                {
                    MenuItem item = new MenuItem();
                    item.RadioCheck = true;
                    item.Text = _currentMonitor.PowerModes[i].name;
                    item.Tag = _currentMonitor.PowerModes[i].code;
                    //if (_currentMonitor.PowerMode.Current == _currentMonitor.PowerModes[i].code) item.Checked = true;
                    item.Click += new EventHandler(contextMenuPower_Click);
                    contextMenuPower.MenuItems.Add(item);
                }

                MenuItem lastitem = new MenuItem();
                lastitem.RadioCheck = true;
                lastitem.Text = "Sleep";
                lastitem.Tag = "61808";
                lastitem.Click += new EventHandler(contextMenuPower_Click);
                contextMenuPower.MenuItems.Add(lastitem);
            }
        }

        private void RefreshSourcesMenu()
        {
            contextMenuInput.MenuItems.Clear();
            if (_currentMonitor.Sources.Length > 0)
            {
                for (int i = 0; i < _currentMonitor.Sources.Length; i++)
                {
                    MenuItem item = new MenuItem();
                    item.RadioCheck = true;
                    item.Text = _currentMonitor.Sources[i].name;
                    item.Tag = _currentMonitor.Sources[i].code;
                    if (_currentMonitor.Source.Current == _currentMonitor.Sources[i].code) item.Checked = true;
                    item.Click += new EventHandler(contextMenuInput_Click);
                    contextMenuInput.MenuItems.Add(item);
                }
            }
        }

        private void InitialiseProfiles()
        {
            contextMenuProfiles.MenuItems.Clear();
            var deserializer = new XmlSerializer(typeof(Config));
            using (var reader = new StreamReader("profiles.xml"))
            {
                _config = (Config)deserializer.Deserialize(reader);
            }
            foreach (var cfg in _config.Profiles)
            {
                MenuItem item = new MenuItem();
                item.RadioCheck = true;
                item.Text = cfg.Name;
                //if (_currentMonitor.Source.Current == _currentMonitor.Sources[i].code) item.Checked = true;
                item.Click += new EventHandler(contextMenuProfiles_Click);
                contextMenuProfiles.MenuItems.Add(item);
            }
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
            barGainRed.Value = (int)_currentMonitor.GainRed.Original;
            barGainGreen.Value = (int)_currentMonitor.GainGreen.Original;
            barGainBlue.Value = (int)_currentMonitor.GainBlue.Original;
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

        private void contextMenuProfiles_Click(object sender, EventArgs e)
        {
            var selectedProfile = _config.Profiles.Where(p => p.Name == ((MenuItem)sender).Text).FirstOrDefault();

            if (selectedProfile == null)
                return;

            foreach (var monitorCfg in selectedProfile.MonitorConfigs)
            {
                if (monitorCfg.Index >= cboMonitors.Items.Count)
                    continue;

                cboMonitors.SelectedIndex = monitorCfg.Index;
                barBrightness.Value = monitorCfg.Brightness;
                barContrast.Value = monitorCfg.Contrast;
                barGainRed.Value = monitorCfg.GainRed;
                barGainGreen.Value = monitorCfg.GainGreen;
                barGainBlue.Value = monitorCfg.GainBlue;
                barVolume.Value = monitorCfg.Volume;
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
            }
            */
        }
    }
}