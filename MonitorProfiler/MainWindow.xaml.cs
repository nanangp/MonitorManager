using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MonitorProfiler.GUI;
using MonitorProfiler.Win32;
using MonitorProfiler.Models.Display;
using MonitorProfiler.Models.Configuration;
using System.Windows.Interop;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace MonitorProfiler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IntPtr _windowHandle;
        private Monitor _currentMonitor;
        private readonly MonitorCollection _monitorCollection = new MonitorCollection();
        private Dictionary<Slider, TrackBarFeatures> _bars;
        private Config _config;
        DispatcherTimer _timerResetColors = new DispatcherTimer(); 
        DispatcherTimer _timerResetFactory = new DispatcherTimer();
        DispatcherTimer _timerResetLuminance = new DispatcherTimer();
        DispatcherTimer _timerRestart = new DispatcherTimer();

        private const int HOTKEY_ID = 9000;
        //Modifiers:
        private const uint MOD_NONE = 0x0000; //(none)
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
        private const uint VK_CAPITAL = 0x14; //CAPSLOCK
        private const uint NK_0 = 0x60; // NUM_KEY 0
        private const string profiles_xml = "profiles.xml";

        private HwndSource _source;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);
            
            NativeMethods.RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_WIN, NK_0); //WINDOWS + NUMKEY_0
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            int vkey = (((int)lParam >> 16) & 0xFFFF);
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            if (vkey == NK_0) WakeHotKey();
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        public MainWindow()
        {
            InitializeComponent();
            InitialiseProfiles();
            InitialiseTrackBars();
            InitialiseButtons();
            GetMonitors();

            foreach (Monitor monitor in _monitorCollection)
            {
                monitor.CheckCapabilities();
            };

            /*
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Debug.Write("Checking duration: " + ts.ToString() + "\n");
            */
            
            int m = 1;
            foreach (Monitor monitor in _monitorCollection)
            {
                cboMonitors.Items.Add(monitor.Name + " #" + m++);
            }

            if (cboMonitors.Items.Count > 0) cboMonitors.SelectedIndex = 0;

            _timerResetColors.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timerResetColors.Tick += new EventHandler(TimerResetColors);
            _timerResetFactory.Interval = new TimeSpan(0, 0, 0, 2, 0);
            _timerResetFactory.Tick += new EventHandler(TimerResetFactory);
            _timerResetLuminance.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timerResetLuminance.Tick += new EventHandler(TimerResetLuminance);
            _timerRestart.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _timerRestart.Tick += new EventHandler(Restart);

            return;
        }
        
        private void GetMonitors()
        {
            var handler = new NativeMethods.EnumMonitorsDelegate(MonitorEnum);
            NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, handler, IntPtr.Zero); // should be sequential
        }

        private bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, NativeStructures.RECT rect, IntPtr dwData)
        {
            NativeStructures.MONITORINFO mi = new NativeStructures.MONITORINFO();

            if (NativeMethods.GetMonitorInfo(hMonitor, mi)) _monitorCollection.Add(hMonitor);
            return true;
        }

        private void WakeHotKey()
        {
            for (int i = 0; i < cboMonitors.Items.Count; i++)
            {
                Debug.WriteLine("Waking monitor: " + i);
                bool teest = NativeMethods.SetVCPFeature(_monitorCollection[i].HPhysicalMonitor, NativeConstants.SC_MONITORPOWER, 1);
            }
        }

        private void ShowContextMemu(Button button)
        {
            button.ContextMenu.Placement = PlacementMode.Bottom;
            button.ContextMenu.PlacementTarget = button;
            button.ContextMenu.PlacementRectangle = new Rect(0, button.Height, 0, 0);
            button.ContextMenu.IsOpen = true;
        }

        private void btnFactoryReset_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ShowContextMemu(button);
        }

        public void contextMenuFactory_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to " + item.Header.ToString().ToLower() + " on " + cboMonitors.SelectedItem + " ?" + (Convert.ToInt32(item.Tag) == 4 ? "\nAll the monitor settings will be reset !" : ""), "Warning !", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.Yes || result == MessageBoxResult.OK)
            {
                Debug.WriteLine("Reset: " + Convert.ToByte(item.Tag));
                NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, Convert.ToByte(item.Tag), 1);

                if (Convert.ToUInt32(item.Tag) == 4)
                {
                    _timerResetFactory.IsEnabled = true;
                    _timerResetFactory.Start();
                }
                if (Convert.ToUInt32(item.Tag) == 5)
                {
                    _timerResetLuminance.IsEnabled = true;
                    _timerResetLuminance.Start();
                }
                if (Convert.ToUInt32(item.Tag) == 8)
                {
                    _timerResetColors.IsEnabled = true;
                    _timerResetColors.Start();
                }
            }
        }

        private void TimerResetColors(Object source, EventArgs e)
        {
            Debug.WriteLine("TimerResetColors");
            _timerResetColors.Stop();
            _timerResetColors.IsEnabled = false;
            _currentMonitor.GetRgbDrive();
            _currentMonitor.GetRgbGain();
            RefreshSliders(_currentMonitor);
        }

        private void TimerResetFactory(Object source, EventArgs e)
        {
            Debug.WriteLine("TimerResetFactory");
            _timerResetFactory.Stop();
            _timerResetFactory.IsEnabled = false;
            _currentMonitor.CheckCapabilities();
            RefreshSliders(_currentMonitor);
        }

        private void TimerResetLuminance(Object source, EventArgs e)
        {
            Debug.WriteLine("TimerResetLuminance");
            _timerResetLuminance.Stop();
            _timerResetLuminance.IsEnabled = false;
            _currentMonitor.GetBrightness();
            _currentMonitor.GetContrast();
            RefreshSliders(_currentMonitor);
        }

        private void btnSources_Click(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);
            _currentMonitor.GetSources();
            RefreshSourcesMenu();
            ShowContextMemu(button);
        }

        private void contextMenuSources_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORSOURCES, Convert.ToUInt32(item.Tag));
            _currentMonitor.Source.Current = Convert.ToUInt32(item.Tag);
        }

        private void btnPower_Click(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);
            ShowContextMemu(button);
        }

        private void contextMenuPower_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            Debug.WriteLine("Power : " + item.Tag);
            // No VCP just force windows monitor sleeping
            if (Convert.ToUInt32(item.Tag) == 61808)
            {
                NativeMethods.SendMessage(_windowHandle, NativeConstants.WM_SYSCOMMAND, (IntPtr)NativeConstants.SC_MONITORSLEEP, (IntPtr)2);
            }
            else NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORPOWER, Convert.ToUInt32(item.Tag));
        }

        private void btnProfiles_Click(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);
            ShowContextMemu(button);
        }

        ~MainWindow()
        {
            foreach (Monitor monitor in _monitorCollection)
            {
                NativeMethods.DestroyPhysicalMonitor(monitor.HPhysicalMonitor);
            }
        }

        #region Private Methods

        private void InitialiseTrackBars()
        {
            _bars = new Dictionary<Slider, TrackBarFeatures>{
                {barBrightness, new TrackBarFeatures(FeatureType.Brightness, lblBrightness)},
                {barContrast, new TrackBarFeatures(FeatureType.Contrast, lblContrast)},
                {barRedGain, new TrackBarFeatures(FeatureType.RedGain, lblRedGain)},
                {barGreenGain, new TrackBarFeatures(FeatureType.GreenGain, lblGreenGain)},
                {barBlueGain, new TrackBarFeatures(FeatureType.BlueGain, lblBlueGain)},
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

            foreach (KeyValuePair<string, string> entry in factoryreset)
            {
                MenuItem item = new MenuItem();
                item.Header = entry.Key;
                item.Tag = entry.Value;
                item.Click += new RoutedEventHandler(contextMenuFactory_Click);
                btnFactoryReset.ContextMenu.Items.Add(item);
            }
        }

        // To be called by a delegate
        private bool MonitorEnum(IntPtr hMonitor, IntPtr hdcMonitor, ref System.Drawing.Rectangle lprcMonitor, IntPtr dwData)
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
                barBrightness.IsEnabled = true;
            }
            else barBrightness.IsEnabled = false;

            Debug.WriteLine("RefreshSliders - Contrast.Max: " + m.Contrast.Max);
            if (m.Contrast.Max > 0)
            {
                barContrast.Minimum = (int)m.Contrast.Min;
                barContrast.Maximum = (int)m.Contrast.Max;
                barContrast.Value = (int)m.Contrast.Current;
                barContrast.IsEnabled = true;
            }
            else barContrast.IsEnabled = false;

            Debug.WriteLine("RefreshSliders - RedGain.Max: " + m.RedGain.Max);
            Debug.WriteLine("RefreshSliders - RedGain.Current: " + m.RedGain.Current);
            if (m.RedGain.Max > 0)
            {
                barRedGain.Minimum = (int)m.RedGain.Min;
                barRedGain.Maximum = (int)m.RedGain.Max;
                barRedGain.Value = (int)m.RedGain.Current;
                barRedGain.IsEnabled = true;
            }
            else barRedGain.IsEnabled = false;

            Debug.WriteLine("RefreshSliders - GreenGain.Max: " + m.GreenGain.Max);
            Debug.WriteLine("RefreshSliders - GreenGain.Current: " + m.GreenGain.Current);
            if (m.GreenGain.Max > 0)
            {
                barGreenGain.Minimum = (int)m.GreenGain.Min;
                barGreenGain.Maximum = (int)m.GreenGain.Max;
                barGreenGain.Value = (int)m.GreenGain.Current;
                barGreenGain.IsEnabled = true;
            }
            else barGreenGain.IsEnabled = false;

            Debug.WriteLine("RefreshSliders - BlueGain.Max: " + m.BlueGain.Max);
            Debug.WriteLine("RefreshSliders - BlueGain.Current: " + m.BlueGain.Current);
            if (m.BlueGain.Max > 0)
            {
                barBlueGain.Minimum = (int)m.BlueGain.Min;
                barBlueGain.Maximum = (int)m.BlueGain.Max;
                barBlueGain.Value = (int)m.BlueGain.Current;
                barBlueGain.IsEnabled = true;
            }
            else barBlueGain.IsEnabled = false;

            Debug.WriteLine("RefreshSliders - Sharpness.Max: " + m.Sharpness.Max);
            if (m.Sharpness.Max > 0)
            {
                barSharpness.TickFrequency = 5;
                if (m.Sharpness.Max < 20) barSharpness.TickFrequency = 1;
                barSharpness.LargeChange = barSharpness.TickFrequency;
                barSharpness.Minimum = 0;
                barSharpness.Maximum = (int)m.Sharpness.Max;
                barSharpness.Value = (int)m.Sharpness.Current;
                barSharpness.IsEnabled = true;
            }
            else barSharpness.IsEnabled = false;
            lblSharpness.IsEnabled = barSharpness.IsEnabled;

            Debug.WriteLine("RefreshSliders - Volume.Max: " + m.Volume.Max);
            if (m.Volume.Max > 0)
            {
                barVolume.Minimum = 0;
                barVolume.Maximum = (int)m.Volume.Max;
                barVolume.Value = (int)m.Volume.Current;
                barVolume.IsEnabled = true;
            }
            else barVolume.IsEnabled = false;
            lblVolume.IsEnabled = barVolume.IsEnabled;

            RefreshPowerMenu();
            RefreshSourcesMenu();
        }

        private void RefreshPowerMenu()
        {
            btnPower.ContextMenu.Items.Clear();
            if (_currentMonitor.PowerModes.Length > 0)
            {
                for (int i = 0; i < _currentMonitor.PowerModes.Length; i++)
                {
                    MenuItem item = new MenuItem();
                    item.Header = _currentMonitor.PowerModes[i].name;
                    item.Tag = _currentMonitor.PowerModes[i].code;
                    //if (_currentMonitor.PowerMode.Current == _currentMonitor.PowerModes[i].code) item.isChecked = true;
                    item.Click += new RoutedEventHandler(contextMenuPower_Click);
                    btnPower.ContextMenu.Items.Add(item);
                }

                MenuItem lastitem = new MenuItem();
                lastitem.Header = "Sleep";
                lastitem.Tag = "61808";
                lastitem.Click += new RoutedEventHandler(contextMenuPower_Click);
                btnPower.ContextMenu.Items.Add(lastitem);
            }
        }

        private void RefreshSourcesMenu()
        {
            btnSources.ContextMenu.Items.Clear();
            if (_currentMonitor.Sources.Length > 0)
            {
                for (int i = 0; i < _currentMonitor.Sources.Length; i++)
                {
                    MenuItem item = new MenuItem();
                    item.Header = _currentMonitor.Sources[i].name;
                    item.Tag = _currentMonitor.Sources[i].code;
                    if (_currentMonitor.Source.Current == _currentMonitor.Sources[i].code) item.IsChecked = true;
                    item.Click += new RoutedEventHandler(contextMenuSources_Click);
                    btnSources.ContextMenu.Items.Add(item);
                }
            }
        }

        private void InitialiseProfiles()
        {
            btnProfiles.ContextMenu.Items.Clear();
            MenuItem item;

            if (File.Exists(profiles_xml))
            {
                var deserializer = new XmlSerializer(typeof(Config));
                using (var reader = new StreamReader(profiles_xml))
                {
                    _config = (Config)deserializer.Deserialize(reader);
                }

                string link = _config.Settings[0].Link;
                if (link.ToLower() == "true")
                {
                    unlink_png.Source = new BitmapImage(new Uri("pack://application:,,,/MonitorProfiler;component/images/link.png"));
                    btnLinkMonitors.Tag = "link";
                }

                foreach (var cfg in _config.Profiles)
                {
                    item = new MenuItem();
                    item.Header = cfg.Name;
                    //if (_currentMonitor.Source.Current == _currentMonitor.Sources[i].code) item.IsChecked = true;
                    item.Click += new RoutedEventHandler(contextMenuProfiles_Click);
                    btnProfiles.ContextMenu.Items.Add(item);
                }
            }

            if (btnProfiles.ContextMenu.Items.Count > 0) btnProfiles.ContextMenu.Items.Add(new Separator());

            item = new MenuItem();
            item.Header = "Delete profile";
            item.Tag = "delete";
            item.Click += new RoutedEventHandler(contextMenuProfiles_Click);
            btnProfiles.ContextMenu.Items.Add(item);

            item = new MenuItem();
            item.Header = "Save profile";
            item.Tag = "save";
            item.Click += new RoutedEventHandler(SaveProfiles);
            btnProfiles.ContextMenu.Items.Add(item);
        }

        private void SaveProfiles(object sender, RoutedEventArgs e)
        {
            for (int i = _config.Profiles.Count - 1; i >= 0; i--)
            {
                if (_config.Profiles[i].Name == "Test") _config.Profiles.RemoveAt(i);
            }

            var serializer = new XmlSerializer(typeof(Config));
            using (var writer = new StreamWriter(profiles_xml))
            {
                serializer.Serialize(writer, _config);
            }

            InitialiseProfiles();
        }

        #endregion

        private void TrackBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                // Assumes sender is a Slider, and exists in the collection. Don't worry about errors :)
                if (barVolume.Value <= 0)
                {
                    picVolume.Source = new BitmapImage(new Uri("pack://application:,,,/MonitorProfiler;component/images/speaker_mute.png"));
                    picVolume.ToolTip = "Volume";
                    picVolume.Cursor = Cursors.Arrow;
                }
                else
                {
                    if (barVolume.Value >= 50) picVolume.Source = new BitmapImage(new Uri("pack://application:,,,/MonitorProfiler;component/images/speaker_high.png"));
                    else picVolume.Source = new BitmapImage(new Uri("pack://application:,,,/MonitorProfiler;component/images/speaker_low.png"));
                    picVolume.ToolTip = "Mute";
                    picVolume.Cursor = Cursors.Hand;
                }

                if ((string)btnLinkMonitors.Tag == "link")
                {
                    for (int i = 0; i < cboMonitors.Items.Count; i++)
                    {
                        _bars[sender as Slider].UpdateScreenWithBarValue(sender as Slider, _monitorCollection[i]);
                        Debug.WriteLine("TrackBar_ValueChanged monitor: " + i);
                    }
                }
                else
                {
                    _bars[sender as Slider].UpdateScreenWithBarValue(sender as Slider, _currentMonitor);
                    Debug.WriteLine("TrackBar_ValueChanged (no link)");
                }
            }
            catch { }
        }

        private void btnRevert_Click(object sender, RoutedEventArgs e)
        {
            barBrightness.Value = (int)_currentMonitor.Brightness.Original;
            barContrast.Value = (int)_currentMonitor.Contrast.Original;
            barRedGain.Value = (int)_currentMonitor.RedGain.Original;
            barGreenGain.Value = (int)_currentMonitor.GreenGain.Original;
            barBlueGain.Value = (int)_currentMonitor.BlueGain.Original;
            barSharpness.Value = (int)_currentMonitor.Sharpness.Original;
            barVolume.Value = (int)_currentMonitor.Volume.Original;
        }

        private void CboMonitors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("cboMonitors_SelectedIndexChanged: " + cboMonitors.SelectedIndex);
            _currentMonitor = _monitorCollection[cboMonitors.SelectedIndex];
            RefreshSliders(_currentMonitor);
        }

        private void btnIdentifyMonitor_Click(object sender, RoutedEventArgs e)
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

        private void contextMenuProfiles_Click(object sender, RoutedEventArgs e)
        {
            var selectedProfile = _config.Profiles.Where(p => p.Name == ((MenuItem)sender).Header.ToString()).FirstOrDefault();

            if (selectedProfile == null)
                return;

            foreach (var monitorCfg in selectedProfile.MonitorConfigs)
            {
                if (monitorCfg.Index >= cboMonitors.Items.Count)
                    continue;
                
                if (monitorCfg.Index == cboMonitors.SelectedIndex)
                {
                    //update bars on current selected monitor
                    if (monitorCfg.Brightness >= 0) barBrightness.Value = monitorCfg.Brightness;
                    if (monitorCfg.Contrast >= 0) barContrast.Value = monitorCfg.Contrast;
                    if (monitorCfg.RedGain >= 0) barRedGain.Value = monitorCfg.RedGain;
                    if (monitorCfg.GreenGain >= 0) barGreenGain.Value = monitorCfg.GreenGain;
                    if (monitorCfg.BlueGain >= 0) barBlueGain.Value = monitorCfg.BlueGain;
                    if (monitorCfg.Sharpness >= 0) barSharpness.Value = monitorCfg.Sharpness;
                    if (monitorCfg.Volume >= 0) barVolume.Value = monitorCfg.Volume;
                }
                else
                {
                    if (monitorCfg.Brightness >= 0) _monitorCollection[monitorCfg.Index].Brightness.Current = (uint)monitorCfg.Brightness;
                    if (monitorCfg.Contrast >= 0) _monitorCollection[monitorCfg.Index].Contrast.Current = (uint)monitorCfg.Contrast;
                    if (monitorCfg.RedGain >= 0) _monitorCollection[monitorCfg.Index].RedGain.Current = (uint)monitorCfg.RedGain;
                    if (monitorCfg.GreenGain >= 0) _monitorCollection[monitorCfg.Index].GreenGain.Current = (uint)monitorCfg.GreenGain;
                    if (monitorCfg.BlueGain >= 0) _monitorCollection[monitorCfg.Index].BlueGain.Current = (uint)monitorCfg.BlueGain;
                    if (monitorCfg.Sharpness >= 0) _monitorCollection[monitorCfg.Index].Sharpness.Current = (uint)monitorCfg.Sharpness;
                    if (monitorCfg.Volume >= 0) _monitorCollection[monitorCfg.Index].Volume.Current = (uint)monitorCfg.Volume;
                    if (monitorCfg.Brightness >= 0) _monitorCollection[monitorCfg.Index].SetBrightness((uint)monitorCfg.Brightness);
                    if (monitorCfg.Contrast >= 0) _monitorCollection[monitorCfg.Index].SetContrast((uint)monitorCfg.Contrast);
                    if (monitorCfg.RedGain >= 0) _monitorCollection[monitorCfg.Index].SetRedGain((uint)monitorCfg.RedGain);
                    if (monitorCfg.GreenGain >= 0) _monitorCollection[monitorCfg.Index].SetGreenGain((uint)monitorCfg.GreenGain);
                    if (monitorCfg.BlueGain >= 0) _monitorCollection[monitorCfg.Index].SetBlueGain((uint)monitorCfg.BlueGain);
                    if (monitorCfg.Sharpness >= 0) _monitorCollection[monitorCfg.Index].SetSharpness((uint)monitorCfg.Sharpness);
                    if (monitorCfg.Volume >= 0) _monitorCollection[monitorCfg.Index].SetVolume((uint)monitorCfg.Volume);
                }
            }
        }

        private void btnLinkMonitors_Click(object sender, RoutedEventArgs e)
        {
            if ((string)btnLinkMonitors.Tag == "unlink")
            {
                unlink_png.Source = new BitmapImage(new Uri("pack://application:,,,/MonitorProfiler;component/images/link.png"));
                btnLinkMonitors.Tag = "link";
                _config.Settings[0].Link = "True";
            }
            else
            {
                unlink_png.Source = new BitmapImage(new Uri("pack://application:,,,/MonitorProfiler;component/images/unlink.png"));
                btnLinkMonitors.Tag = "unlink";
                _config.Settings[0].Link = "False";
            }
        }

        private void picVolume_Click(object sender, MouseButtonEventArgs e)
        {
            if (barVolume.Value > 0) barVolume.Value = 0;
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            lblWait.Visibility = Visibility.Visible;
            Debug.WriteLine("show the thing");
            _timerRestart.IsEnabled = true;
            _timerRestart.Start();
        }

        private void Restart(object sender, EventArgs e)
        {
            _timerRestart.Stop();
            _timerRestart.IsEnabled = false;
            _monitorCollection.Clear();
            cboMonitors.SelectionChanged -= CboMonitors_SelectionChanged;
            cboMonitors.Items.Clear();
            cboMonitors.SelectionChanged += CboMonitors_SelectionChanged;
            InitialiseTrackBars();
            InitialiseButtons();
            GetMonitors();

            foreach (Monitor monitor in _monitorCollection)
            {
                monitor.CheckCapabilities();
            };

            int m = 1;
            foreach (Monitor monitor in _monitorCollection)
            {
                cboMonitors.Items.Add(monitor.Name + " #" + m++);
            }

            if (cboMonitors.Items.Count > 0) cboMonitors.SelectedIndex = 0;

            Debug.WriteLine("hide the thing");
            lblWait.Visibility = Visibility.Hidden;
        }

        private void btnDeleteProfile_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NativeMethods.UnregisterHotKey(_windowHandle, HOTKEY_ID); //WINDOWS + NUMKEY_0
            SaveProfiles(sender, null);
        }
    }
}
