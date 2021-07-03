using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MonitorManager.GUI;
using MonitorManager.Win32;
using MonitorManager.Models.Display;
using MonitorManager.Models.Configuration;
using System.Windows.Interop;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Navigation;
using System.Security.Principal;
using System.Globalization;

namespace MonitorManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Monitor _currentMonitor;
        private readonly MonitorCollection _monitorCollection = new MonitorCollection();
        private Dictionary<Slider, TrackBarFeatures> _bars;
        private Config _config;
        DispatcherTimer _timerResetColors = new DispatcherTimer();
        DispatcherTimer _timerResetFactory = new DispatcherTimer();
        DispatcherTimer _timerResetLuminance = new DispatcherTimer();
        DispatcherTimer _timerStart = new DispatcherTimer();

        private string svgLink = "M12.7 17l.3.3c.5.5 1 .8 1.5 1 .3-.5.4-1 .5-1.5-.4-.2-.8-.4-1.1-.8-1.6-1.6-1.6-4.1 0-5.7L16 8.2c1.6-1.6 4.1-1.6 5.7 0s1.6 4.1 0 5.7l-1 1c.2.6.2 1.3.2 2l2.1-2.1c2.1-2.1 2.1-5.4 0-7.5l-.3-.3c-2.1-2.1-5.4-2.1-7.5 0l-2.4 2.4c-2.1 2.1-2.1 5.5-.1 7.6z M7.1 22.6l.3.3c2.1 2.1 5.4 2.1 7.5 0l2.4-2.4c2.1-2.1 2.1-5.4 0-7.5l-.3-.3c-.5-.5-1-.8-1.5-1-.3.5-.4 1-.5 1.5.4.2.8.4 1.1.8 1.6 1.6 1.6 4.1 0 5.7L14 21.8c-1.6 1.6-4.1 1.6-5.7 0s-1.6-4.1 0-5.7l1-1c-.2-.6-.2-1.3-.2-2L7 15.2c-2 2-2 5.4.1 7.4z";
        private string svgUnlink = "M7.1 22.6l.3.3c2.1 2.1 5.4 2.1 7.5 0l1.9-1.9-1.1-1.1-1.8 1.8c-1.6 1.6-4.1 1.6-5.7 0s-1.6-4.1 0-5.7l1.8-1.8-1-1-1.9 1.9c-2.1 2.1-2.1 5.5 0 7.5zM16.1 8.3c1.6-1.6 4.1-1.6 5.7 0s1.6 4.1 0 5.7L20 15.7l1 1 1.9-1.9c2.1-2.1 2.1-5.4 0-7.5l-.3-.3c-2.1-2.1-5.4-2.1-7.5 0l-1.9 2 1.1 1.1 1.8-1.8zM19.435 20.515l1.06-1.06 2.83 2.828-1.062 1.06zM17.6 21.9h1.5v3.2h-1.5zM21.9 17.6h3.2v1.5h-3.2zM6.676 7.717l1.06-1.06 2.83 2.828-1.062 1.06zM4.9 10.9h3.2v1.5H4.9zM10.9 4.9h1.5v3.2h-1.5z";
        private string svgVolumeMute = "M26 11.5l-1-1-3.5 3.4-3.5-3.4-1 1 3.4 3.5-3.4 3.5 1 1 3.5-3.4 3.5 3.4 1-1-3.4-3.5M5.3 10.3v9.5h3.9l4.6 5.2V5l-4.6 5.2H5.3zm7-1.3v12l-2.4-2.8H6.8v-6.5h3.1L12.3 9z";
        private string svgVolumeLow = "M5.3 10.2v9.6h3.9l4.6 5.2V5l-4.6 5.2H5.3zm7-1.2v12l-2.4-2.8H6.8v-6.5h3.1L12.3 9zM18.5 10.1l-1 1.1c.9 1 1.5 2.3 1.5 3.8s-.6 2.8-1.5 3.8l1 1.1c1.2-1.3 2-3 2-4.9s-.8-3.6-2-4.9z";
        private string svgVolumeMedium = "M5.3 10.2v9.6h3.9l4.6 5.2V5l-4.6 5.2H5.3zm7-1.2v12l-2.4-2.8H6.8v-6.5h3.1L12.3 9zM18.5 10.1l-1 1.1c.9 1 1.5 2.3 1.5 3.8s-.6 2.8-1.5 3.8l1 1.1c1.2-1.3 2-3 2-4.9s-.8-3.6-2-4.9z M21.2 7.2l-1 1.1C21.9 10 23 12.4 23 15s-1.1 5-2.8 6.7l1 1.1c2-2 3.3-4.8 3.3-7.8s-1.3-5.9-3.3-7.8z";
        private string svgVolumeHigh = "M5.3 10.2v9.6h3.9l4.6 5.2V5l-4.6 5.2H5.3zm7-1.2v12l-2.4-2.8H6.8v-6.5h3.1L12.3 9zM18.5 10.1l-1 1.1c.9 1 1.5 2.3 1.5 3.8s-.6 2.8-1.5 3.8l1 1.1c1.2-1.3 2-3 2-4.9s-.8-3.6-2-4.9z M21.2 7.2l-1 1.1C21.9 10 23 12.4 23 15s-1.1 5-2.8 6.7l1 1.1c2-2 3.3-4.8 3.3-7.8s-1.3-5.9-3.3-7.8z M23.9 4.2l-1 1.1c2.6 2.5 4.2 6 4.1 9.9-.1 3.7-1.6 7.1-4.1 9.5l1 1.1c2.8-2.7 4.5-6.4 4.6-10.5.1-4.4-1.7-8.3-4.6-11.1z";
        private bool active;

        //Modifiers:
        private const uint MOD_NONE = 0x0000; //(none)
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
        private const uint VK_CAPITAL = 0x14; //CAPSLOCK
        private const uint NK_0 = 0x60; // NUM_KEY 0
        private const uint NK_1 = 0x61; // NUM_KEY 1
        private const uint NK_2 = 0x62; // NUM_KEY 2
        private const uint NK_3 = 0x63; // NUM_KEY 3
        private const uint NK_4 = 0x64; // NUM_KEY 4
        private const uint NK_5 = 0x65; // NUM_KEY 5
        private const uint NK_6 = 0x66; // NUM_KEY 6
        private const uint NK_7 = 0x67; // NUM_KEY 7
        private const uint NK_8 = 0x68; // NUM_KEY 8
        private const uint NK_9 = 0x69; // NUM_KEY 9
        private const int HOTKEY_ID = 9000;
        private const int WM_HOTKEY = 0x0312;
        private const int WM_DWMCOMPOSITIONCHANGED = 0x31A;
        private const int WM_THEMECHANGED = 0x31E;

        private const string profiles_xml = "profiles.xml";
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string RegistrySystemLight = "SystemUsesLightTheme";
        private static Brush WindowGlassBrush = GetWindowGlassBrush();
        private bool showintray;
        private string lastFocus = "";

        private enum WindowsTheme
        {
            Light,
            Dark
        }

        private static DoubleAnimation WindowAnimShow = new DoubleAnimation();
        private static DoubleAnimation WindowAnimHide = new DoubleAnimation();
        private static DoubleAnimation gridBlurAnimShow = new DoubleAnimation();
        private static DoubleAnimation gridBlurAnimHide = new DoubleAnimation();

        private IntPtr hwnd;
        private HwndSource hsource;
        private System.Windows.Forms.NotifyIcon trayIcon = new System.Windows.Forms.NotifyIcon();
        private System.Windows.Forms.ContextMenu trayContextMenu = new System.Windows.Forms.ContextMenu();

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            if ((hwnd = new WindowInteropHelper(this).Handle) == IntPtr.Zero)
            {
                throw new InvalidOperationException("Could not get window handle.");
            }

            hsource = HwndSource.FromHwnd(hwnd);
            hsource.AddHook(WndProc);
            
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_0); //WINDOWS + NUMKEY_0
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_1); //WINDOWS + NUMKEY_1
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_2); //WINDOWS + NUMKEY_2
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_3); //WINDOWS + NUMKEY_3
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_4); //WINDOWS + NUMKEY_4
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_5); //WINDOWS + NUMKEY_5
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_6); //WINDOWS + NUMKEY_6
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_7); //WINDOWS + NUMKEY_7
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_8); //WINDOWS + NUMKEY_8
            NativeMethods.RegisterHotKey(hwnd, HOTKEY_ID, MOD_WIN, NK_9); //WINDOWS + NUMKEY_9
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            int vkey = (int)(((long)lParam >> 16) & 0xFFFF);
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            if (vkey == NK_0) WakeHotKey();
                            if (vkey == NK_1) { barContrast.Value -= 5; barBrightness.Value -= 5; }
                            if (vkey == NK_2) { barContrast.Value = 50; barBrightness.Value = 50; }
                            if (vkey == NK_3) { barContrast.Value += 5; barBrightness.Value += 5; }
                            if (vkey == NK_4) barContrast.Value -= 5;
                            if (vkey == NK_5) barContrast.Value = 50;
                            if (vkey == NK_6) barContrast.Value += 5;
                            if (vkey == NK_7) barBrightness.Value -= 5;
                            if (vkey == NK_8) barBrightness.Value = 50;
                            if (vkey == NK_9) barBrightness.Value += 5;
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        public MainWindow()
        {
            RoutedCommand keyCommand = new RoutedCommand();
            keyCommand.InputGestures.Add(new KeyGesture(Key.Escape));
            CommandBindings.Add(new CommandBinding(keyCommand, btnBlurCancel_Click));

            Resources["GlassBrush"] = WindowGlassBrush;
            Resources["GlassBrush60"] = ConvertOpacity(GetWindowGlassColor(), 60);
            Resources["GlassBrush80"] = ConvertOpacity(GetWindowGlassColor(), 80);
            Resources["MinitorComboVertical"] = Convert.ToDouble(-4);
            WindowsTheme SysTheme = GetWindowsTheme();

            InitializeComponent();
            InitialiseProfiles();
            /*
            InitialiseTrackBars();
            GetMonitors();

            foreach (Monitor monitor in _monitorCollection)
            {
                monitor.CheckCapabilities();
            };
            */

            /*
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            Debug.Write("Checking duration: " + ts.ToString() + "\n");
            */

            System.Windows.Forms.MenuItem trayItem = new System.Windows.Forms.MenuItem();
            trayItem.Click += traySettings_Click;
            trayItem.Text = "Settings";
            trayContextMenu.MenuItems.Add(trayItem);

            trayItem = new System.Windows.Forms.MenuItem();
            trayItem.Click += trayAbout_Click;
            trayItem.Text = "About";
            trayContextMenu.MenuItems.Add(trayItem);

            trayItem = new System.Windows.Forms.MenuItem();
            trayItem.Click += CloseButton_Click;
            trayItem.Text = "Exit";
            trayContextMenu.MenuItems.Add(trayItem);

            if (SysTheme == WindowsTheme.Light) trayIcon.Icon = Properties.Resources.tray;
            else trayIcon.Icon = Properties.Resources.trayDark;
            trayIcon.Visible = true;
            trayIcon.Click += Trayicon_Click;
            trayIcon.MouseMove += Trayicon_MouseMove;
            trayIcon.ContextMenu = trayContextMenu;

            /*
             * int m = 1;
            foreach (Monitor monitor in _monitorCollection)
            {
                cboMonitors.Items.Add(monitor.Name + " #" + m++);
            }

            if (cboMonitors.Items.Count > 0) cboMonitors.SelectedIndex = 0;
            */

            _timerResetColors.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timerResetColors.Tick += new EventHandler(TimerResetColors);
            _timerResetFactory.Interval = new TimeSpan(0, 0, 0, 2, 0);
            _timerResetFactory.Tick += new EventHandler(TimerResetFactory);
            _timerResetLuminance.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timerResetLuminance.Tick += new EventHandler(TimerResetLuminance);

            SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler(SystemEvents_UserPreferenceChanged);

            barBrightness.PreviewMouseWheel += (sender, e) => barBrightness.Value += barBrightness.SmallChange * e.Delta / 60;
            barContrast.PreviewMouseWheel += (sender, e) => barContrast.Value += barContrast.SmallChange * e.Delta / 60;
            barRedGain.PreviewMouseWheel += (sender, e) => barRedGain.Value += barRedGain.SmallChange * e.Delta / 60;
            barGreenGain.PreviewMouseWheel += (sender, e) => barGreenGain.Value += barGreenGain.SmallChange * e.Delta / 60;
            barBlueGain.PreviewMouseWheel += (sender, e) => barBlueGain.Value += barBlueGain.SmallChange * e.Delta / 60;
            barSharpness.PreviewMouseWheel += (sender, e) => barSharpness.Value += barSharpness.SmallChange * e.Delta / 120;
            barVolume.PreviewMouseWheel += (sender, e) => barVolume.Value += barVolume.SmallChange * e.Delta / 60;

            Duration hideDur = new Duration(TimeSpan.FromSeconds(0.2));

            WindowAnimShow.From = 1;
            WindowAnimShow.To = 0.8;
            WindowAnimShow.Duration = hideDur;
       
            WindowAnimHide.From = 0.8;
            WindowAnimHide.To = 1;
            WindowAnimHide.Duration = hideDur;
            
            gridBlurAnimShow.From = 0;
            gridBlurAnimShow.To = 1;
            gridBlurAnimShow.Duration = hideDur;

            gridBlurAnimHide.From = 1;
            gridBlurAnimHide.To = 0;
            gridBlurAnimHide.Duration = hideDur;
            gridBlurAnimHide.Completed += new EventHandler(WaitMessage_Hide);

            int m = 1;
            foreach (Monitor monitor in _monitorCollection)
            {
                cboMonitors.Items.Add(monitor.Name + " #" + m++);
            }

            if (cboMonitors.Items.Count > 0) cboMonitors.SelectedIndex = 0;

            ShowMessage("Loading, please wait...", false);
            // window won't show before return so we delay the monitor scanning cause it's slow
            _timerStart.Interval = new TimeSpan(0, 0, 0, 0, 100);
            _timerStart.Tick += new EventHandler(StartUp);
            _timerStart.IsEnabled = true;
            _timerStart.Start();

            return;
        }

        private void traySettings_Click(object sender, EventArgs e)
        {
            ShowWindow();
            btnMenuSettings.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void trayAbout_Click(object sender, EventArgs e)
        {
            ShowWindow();
            btnMenuAbout.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            WindowGlassBrush = GetWindowGlassBrush();
            Resources["GlassBrush"] = WindowGlassBrush;
            Resources["GlassBrush60"] = ConvertOpacity(GetWindowGlassColor(), 60);
            Resources["GlassBrush80"] = ConvertOpacity(GetWindowGlassColor(), 80);
        }

        internal void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);

            var accent = new NativeStructures.AccentPolicy();
            accent.AccentState = NativeStructures.AccentState.ACCENT_ENABLE_BLURBEHIND;

            var accentStructSize = Marshal.SizeOf(accent);

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new NativeStructures.WindowCompositionAttributeData();
            data.Attribute = NativeStructures.WindowCompositionAttribute.WCA_ACCENT_POLICY;
            data.SizeOfData = accentStructSize;
            data.Data = accentPtr;

            NativeMethods.SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
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

        private void StartUp(object sender, EventArgs e)
        {
            _timerStart.Stop();
            _timerStart.IsEnabled = false;
            Restart(new object(), new EventArgs());
        }

        private bool ThisScreenSelected()
        {
            var thisScreen = System.Windows.Forms.Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            string thisDeviceName = thisScreen.DeviceName;
            int thisScreenId = 0, i = 0;
            var allScreens = System.Windows.Forms.Screen.AllScreens;//.FromHandle(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            foreach (System.Windows.Forms.Screen _screen in allScreens)
            {
                if (_screen.DeviceName == thisDeviceName)
                {
                    thisScreenId = i;
                    break;
                }
                i++;
            }

            if (cboMonitors.SelectedIndex == thisScreenId) return true;
            return false;
        }

        private void ShowContextMemu(Button button)
        {
            //if (ShowInTray == "bottom") button.ContextMenu.Placement = PlacementMode.Top;
            //else button.ContextMenu.Placement = PlacementMode.Bottom;
            button.ContextMenu.Placement = PlacementMode.Top;
            button.ContextMenu.PlacementTarget = button;
            //if (ShowInTray == "bottom") button.ContextMenu.PlacementRectangle = new Rect(0, 0, 0, 0);
            //else button.ContextMenu.PlacementRectangle = new Rect(0, button.Height, 0, 0);
            button.ContextMenu.PlacementRectangle = new Rect(0, 0, 0, 0);
            button.ContextMenu.IsOpen = true;
        }

        private void btnFactoryReset_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            lastFocus = button.Name;
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
            lastFocus = button.Name;
            _currentMonitor.GetSources();
            RefreshSourcesMenu();
            ShowContextMemu(button);
        }

        private void contextMenuSources_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            // Confirm power action to app screen
            if (ThisScreenSelected())
            {
                btnThisSceenConfirmationOk.Tag = item.Tag;
                btnThisSceenConfirmationOk.Click += btnScreenSource_Click;
                ShowMessage("This will apply to this monitor, are you sure?", true);
            } else
            {
                setScreenSource(Convert.ToUInt32(item.Tag));
            }
        }

        private void setScreenSource(UInt32 source)
        {
            NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORSOURCES, source);
           _currentMonitor.Source.Current = source;
        }
        
        private void btnScreenSource_Click(object sender, RoutedEventArgs e)
        {
            setScreenSource(Convert.ToUInt32(((Button)sender).Tag));
            btnThisSceenConfirmationOk.Click -= btnScreenSource_Click;
            HideMessage();
        }

        private void btnPower_Click(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);
            lastFocus = button.Name;
            ShowContextMemu(button);
        }

        private void contextMenuPower_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            Debug.WriteLine("Power : " + item.Tag);
            // No VCP just force windows monitor sleeping
            if (Convert.ToUInt32(item.Tag) == 61808)
            {
                btnThisSceenConfirmationOk.Click += btnScreenSleep_Click;
                ShowMessage("This will apply to all monitors, are you sure?", true);
            } else
            {
                // Confirm power action to app screen
                if (ThisScreenSelected() && Convert.ToString(item.Header) != "Power on")
                {
                    btnThisSceenConfirmationOk.Tag = item.Tag;
                    btnThisSceenConfirmationOk.Click += btnScreenPower_Click;
                    ShowMessage("This will apply to this monitor, are you sure?", true);
                } else
                {
                    setScreenPower(Convert.ToUInt32(item.Tag));
                }
            }
        }

        private void setScreenPower(UInt32 power)
        {
            NativeMethods.SetVCPFeature(_currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORPOWER, power);
        }

        private void btnScreenPower_Click(object sender, RoutedEventArgs e)
        {
            setScreenPower(Convert.ToUInt32(((Button)sender).Tag));
            btnThisSceenConfirmationOk.Click -= btnScreenPower_Click;
            HideMessage();
        }

        private void setScreenSleep()
        {
            NativeMethods.SendMessage(hwnd, NativeConstants.WM_SYSCOMMAND, (IntPtr)NativeConstants.SC_MONITORSLEEP, (IntPtr)2);
        }

        private void btnScreenSleep_Click(object sender, RoutedEventArgs e)
        {
            btnThisSceenConfirmationOk.Click -= btnScreenSleep_Click;
            setScreenSleep();
            HideMessage();
        }

        private void btnProfiles_Click(object sender, RoutedEventArgs e)
        {
            Button button = ((Button)sender);
            lastFocus = button.Name;
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
                pathBrightness.Opacity = 1;
            }
            else
            {
                barBrightness.IsEnabled = false;
                pathBrightness.Opacity = 0.15;
            }
            picBrightness.IsEnabled = barBrightness.IsEnabled;
            lblBrightness.Opacity = pathBrightness.Opacity;

            Debug.WriteLine("RefreshSliders - Contrast.Max: " + m.Contrast.Max);
            if (m.Contrast.Max > 0)
            {
                barContrast.Minimum = (int)m.Contrast.Min;
                barContrast.Maximum = (int)m.Contrast.Max;
                barContrast.Value = (int)m.Contrast.Current;
                barContrast.IsEnabled = true;
                pathContrast.Opacity = 1;
                lblContrast.Opacity = pathContrast.Opacity;
            }
            else
            {
                barContrast.IsEnabled = false;
                pathContrast.Opacity = 0.15;
            }
            picContrast.IsEnabled = barContrast.IsEnabled;
            lblContrast.Opacity = lblContrast.Opacity;

            Debug.WriteLine("RefreshSliders - RedGain.Max: " + m.RedGain.Max);
            Debug.WriteLine("RefreshSliders - RedGain.Current: " + m.RedGain.Current);
            if (m.RedGain.Max > 0)
            {
                barRedGain.Minimum = (int)m.RedGain.Min;
                barRedGain.Maximum = (int)m.RedGain.Max;
                barRedGain.Value = (int)m.RedGain.Current;
                barRedGain.IsEnabled = true;
                pathRedGain.Opacity = 1;
            }
            else
            {
                barRedGain.IsEnabled = false;
                pathRedGain.Opacity = 0.15;
            }
            picRedGain.IsEnabled = barRedGain.IsEnabled;
            lblRedGain.Opacity = pathRedGain.Opacity;

            Debug.WriteLine("RefreshSliders - GreenGain.Max: " + m.GreenGain.Max);
            Debug.WriteLine("RefreshSliders - GreenGain.Current: " + m.GreenGain.Current);
            if (m.GreenGain.Max > 0)
            {
                barGreenGain.Minimum = (int)m.GreenGain.Min;
                barGreenGain.Maximum = (int)m.GreenGain.Max;
                barGreenGain.Value = (int)m.GreenGain.Current;
                barGreenGain.IsEnabled = true;
                pathGreenGain.Opacity = 1;
            }
            else
            {
                barGreenGain.IsEnabled = false;
                pathGreenGain.Opacity = 0.15;
            }
            picGreenGain.IsEnabled = barGreenGain.IsEnabled;
            lblGreenGain.Opacity = pathGreenGain.Opacity;

            Debug.WriteLine("RefreshSliders - BlueGain.Max: " + m.BlueGain.Max);
            Debug.WriteLine("RefreshSliders - BlueGain.Current: " + m.BlueGain.Current);
            if (m.BlueGain.Max > 0)
            {
                barBlueGain.Minimum = (int)m.BlueGain.Min;
                barBlueGain.Maximum = (int)m.BlueGain.Max;
                barBlueGain.Value = (int)m.BlueGain.Current;
                barBlueGain.IsEnabled = true;
                pathBlueGain.Opacity = 1;
            }
            else
            {
                barBlueGain.IsEnabled = false;
                pathBlueGain.Opacity = 0.15;
            }
            picBlueGain.IsEnabled = barBlueGain.IsEnabled;
            lblBlueGain.Opacity = pathBlueGain.Opacity;

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
                pathSharpness.Opacity = 1;
            }
            else
            {
                barSharpness.IsEnabled = false;
                pathSharpness.Opacity = 0.15;
            }
            picSharpness.IsEnabled = barSharpness.IsEnabled;
            lblSharpness.Opacity = pathSharpness.Opacity;

            Debug.WriteLine("RefreshSliders - Volume.Max: " + m.Volume.Max);
            if (m.Volume.Max > 0)
            {
                barVolume.Minimum = 0;
                barVolume.Maximum = (int)m.Volume.Max;
                barVolume.Value = (int)m.Volume.Current;
                barVolume.IsEnabled = true;
                pathVolume.Opacity = 1;
            }
            else
            {
                barVolume.IsEnabled = false;
                pathVolume.Opacity = 0.15;
            }
            picVolume.IsEnabled = barVolume.IsEnabled;
            lblVolume.Opacity = pathVolume.Opacity;

            RefreshPowerMenu();
            RefreshSourcesMenu();
            RefreshFactoryResetMenu();
        }

        private void RefreshPowerMenu()
        {
            btnPower.ContextMenu.Items.Clear();
            if (_currentMonitor.PowerModes == null) return;
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

                if (btnPower.ContextMenu.Items.Count > 0) btnPower.ContextMenu.Items.Add(new Separator());

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
            if (_currentMonitor.Sources == null) return;
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

        private void RefreshFactoryResetMenu()
        {
            bool resetall = false;
            btnFactoryReset.ContextMenu.Items.Clear();

            for (int i = 0; i < NativeConstants.factoryResets.Length; ++i)
            {
                string _reset = NativeConstants.factoryResets[i];
                if (_reset != "**undefined**" && _reset != "**Unrecognized**")
                {
                    if (i == 4) {
                        resetall = true;
                        continue;
                    }
                   
                    MenuItem item = new MenuItem();
                    item.Header = _reset;
                    item.Tag = i;
                    item.Click += new RoutedEventHandler(contextMenuFactory_Click);
                    btnFactoryReset.ContextMenu.Items.Add(item);
                }
            }

            if (resetall)
            {
                btnFactoryReset.ContextMenu.Items.Add(new Separator());
                string _reset = NativeConstants.factoryResets[4];
                MenuItem item = new MenuItem();
                item.Header = _reset;
                item.Tag = 4;
                item.Click += new RoutedEventHandler(contextMenuFactory_Click);
                btnFactoryReset.ContextMenu.Items.Add(item);
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

                string link = _config.Settings[0].Link.ToLower();
                if (link == "true")
                {
                    pathlink.Data = Geometry.Parse(svgLink);
                    btnLinkMonitors.Tag = "link";
                }

                showintray = Convert.ToBoolean(_config.Settings[0].Tray);
                checkTray.Checked -= checkTray_Checked;
                if (showintray) checkTray.IsChecked = true;
                checkTray.Checked += checkTray_Checked;

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
            item.Header = "Manage profiles";
            item.Tag = "delete";
            item.Click += new RoutedEventHandler(ManageProfiles);
            btnProfiles.ContextMenu.Items.Add(item);
        }

        private void ManageProfiles(object sender, RoutedEventArgs e)
        {
            btnMenuProfiles.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
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
                    pathVolume.Data = Geometry.Parse(svgVolumeMute);
                    picVolume.ToolTip = "Volume";
                }
                else
                {
                    if (barVolume.Value >= 66) pathVolume.Data = Geometry.Parse(svgVolumeHigh);
                    else if (barVolume.Value >= 33) pathVolume.Data = Geometry.Parse(svgVolumeMedium);
                    else pathVolume.Data = Geometry.Parse(svgVolumeLow);
                    picVolume.ToolTip = "Mute";
                }

                switch ((string)btnLinkMonitors.Tag)
                {
                    case "link":
                        for (int i = 0; i < cboMonitors.Items.Count; i++)
                        {
                            _bars[sender as Slider].UpdateScreenWithBarValue(sender as Slider, _monitorCollection[i]);
                            Debug.WriteLine("TrackBar_ValueChanged (link) monitor: " + i);
                        }
                        break;
                    case "unlink":
                        _bars[sender as Slider].UpdateScreenWithBarValue(sender as Slider, _currentMonitor);
                        Debug.WriteLine("TrackBar_ValueChanged (no link)");
                        break;
                    default:
                        // hack, no value means we just switched the monitor combo, no need to re-set values
                        _bars[sender as Slider].UpdateScreenWithBarValue(sender as Slider, _currentMonitor, false);
                        Debug.WriteLine("TrackBar_ValueChanged (false)");
                        break;
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
            bool link = false;
            if ((string)btnLinkMonitors.Tag == "link") link = true;
            // hack, set value to false will prevent changeing values when combo selection changes
            btnLinkMonitors.Tag = "false";
            Debug.WriteLine("cboMonitors_SelectedIndexChanged: " + cboMonitors.SelectedIndex);
            _currentMonitor = _monitorCollection[cboMonitors.SelectedIndex];
            RefreshSliders(_currentMonitor);
            // re set link value
            if (link) btnLinkMonitors.Tag = "link";
            else btnLinkMonitors.Tag = "unlink";
            cboMonitors.IsDropDownOpen = false;
            Style style = Application.Current.FindResource("ComboBoxItem") as Style;
            Resources["MinitorComboVertical"] = Convert.ToDouble(-4 - (cboMonitors.SelectedIndex * 32));
        }

        private void btnIdentifyMonitor_Click(object sender, RoutedEventArgs e)
        {
            btnIdentifyMonitor.IsEnabled = false;
            
            // Revert
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
                    
            NativeMethods.SetMonitorBrightness(_currentMonitor.HPhysicalMonitor, _currentBrightness);
            NativeMethods.SetMonitorContrast(_currentMonitor.HPhysicalMonitor, _currentContrast);

            btnIdentifyMonitor.IsEnabled = true;
        }

        private static WindowsTheme GetWindowsTheme()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
            {
                object registryValueObject = key?.GetValue(RegistrySystemLight);
                if (registryValueObject == null)
                {
                    return WindowsTheme.Light;
                }

                int registryValue = (int)registryValueObject;

                return registryValue > 0 ? WindowsTheme.Light : WindowsTheme.Dark;
            }
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
                pathlink.Data = Geometry.Parse(svgLink);
                btnLinkMonitors.Tag = "link";
                _config.Settings[0].Link = "True";
            }
            else
            {
                pathlink.Data = Geometry.Parse(svgUnlink);
                btnLinkMonitors.Tag = "unlink";
                _config.Settings[0].Link = "False";
            }
        }

        private void picBrightness_Click(object sender, RoutedEventArgs e)
        {
            if (barBrightness.Value > 0) barBrightness.Value = 0;
        }

        private void picContrast_Click(object sender, RoutedEventArgs e)
        {
            if (barContrast.Value > 0) barContrast.Value = 0;
        }

        private void picRedGain_Click(object sender, RoutedEventArgs e)
        {
            if (barRedGain.Value > 0) barRedGain.Value = 0;
        }

        private void picGreenGain_Click(object sender, RoutedEventArgs e)
        {
            if (barGreenGain.Value > 0) barGreenGain.Value = 0;
        }

        private void picBlueGain_Click(object sender, RoutedEventArgs e)
        {
            if (barBlueGain.Value > 0) barBlueGain.Value = 0;
        }

        private void picSharpness_Click(object sender, RoutedEventArgs e)
        {
            if (barSharpness.Value > 0) barSharpness.Value = 0;
        }

        private void picVolume_Click(object sender, RoutedEventArgs e)
        {
            if (barVolume.Value > 0) barVolume.Value = 0;
        }

        private void btnMenuRestart_Click(object sender, RoutedEventArgs e)
        {
            btnMenuClose.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            gridBlurAnimShow.Completed += Restart;
            ShowMessage("Refreshing informations...", false);
            Debug.WriteLine("show the thing");
        }

        private async void Restart(object sender, EventArgs e)
        {
            await Task.Run(() =>
                this.Dispatcher.Invoke(() =>
                {
                    _monitorCollection.Clear();
                    cboMonitors.SelectionChanged -= CboMonitors_SelectionChanged;
                    cboMonitors.Items.Clear();
                    cboMonitors.SelectionChanged += CboMonitors_SelectionChanged;
                    InitialiseTrackBars();
                    RefreshFactoryResetMenu();
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

                    gridBlurAnimShow.Completed -= Restart;
                    HideMessage();
                }
            ));
        }

        private void WaitMessage_Hide(object sender, EventArgs e)
        {
            Message.Visibility = Visibility.Hidden;
            WindowBlur.Radius = 0;
        }

        private void btnDeleteProfile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MinButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            CloseButton_Click(sender, new RoutedEventArgs());
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Menu.Width >= 350) CloseMenus();
            if (e.ChangedButton == MouseButton.Left) Application.Current.MainWindow.DragMove();
        }

        private Brush ConvertOpacity(Color color, int alpha)
        {
            return new SolidColorBrush(Color.FromArgb((byte)(alpha * 2.55), color.R, color.G, color.B));
        }

        private static Color GetWindowGlassColor()
        {
            var colorizationParams = new NativeStructures.DWMCOLORIZATIONPARAMS();
            NativeMethods.DwmGetColorizationParameters(ref colorizationParams);
            var frameColor = ToColor(colorizationParams.ColorizationColor);

            return frameColor;
        }

        private static Brush GetWindowGlassBrush()
        {
            Color color = GetWindowGlassColor();
            return new SolidColorBrush(color);
        }

        private static Color ToColor(UInt32 value)
        {
            return Color.FromArgb(255,
                (byte)(value >> 16),
                (byte)(value >> 8),
                (byte)value
                );
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void cboMonitors_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter || e.Key == Key.Space) cboMonitors.IsDropDownOpen = true;
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            btnMenuClose.Focusable = true;
            btnMenuClose.Focus();
            btnMenu.Focusable = false;
        }

        private void btnMenuClose_Click(object sender, RoutedEventArgs e)
        {
            btnMenuClose.Focusable = false;
            btnMenuProfilesClose.Focusable = false;
            btnMenuSettingsClose.Focusable = false;
            btnMenuAboutClose.Focusable = false;
            btnMenu.Focusable = true;
            btnMenu.Focus();
        }

        private void btnMenuProfiles_Click(object sender, RoutedEventArgs e)
        {
            btnMenuProfilesClose.Focusable = true;
            btnMenuProfilesClose.Focus();
            btnMenuClose.Focusable = false;
        }

        private void btnMenuProfilesClose_Click(object sender, RoutedEventArgs e)
        {
            btnMenuClose.Focusable = true;
            btnMenuClose.Focus();
            btnMenuProfilesClose.Focusable = false;
        }

        private void btnMenuSettings_Click(object sender, RoutedEventArgs e)
        {
            btnMenuSettingsClose.Focusable = true;
            btnMenuSettingsClose.Focus();
            btnMenuClose.Focusable = false;
        }

        private void btnMenuSettingsClose_Click(object sender, RoutedEventArgs e)
        {
            btnMenuClose.Focusable = true;
            btnMenuClose.Focus();
            btnMenuSettingsClose.Focusable = false;
        }

        private void btnMenuAbout_Click(object sender, RoutedEventArgs e)
        {
            btnMenuAboutClose.Focusable = true;
            btnMenuAboutClose.Focus();
            btnMenuClose.Focusable = false;
        }

        private void btnMenuAboutClose_Click(object sender, RoutedEventArgs e)
        {
            btnMenuClose.Focusable = true;
            btnMenuClose.Focus();
            btnMenuAboutClose.Focusable = false;
        }

        private void CloseMenus()
        {
            Debug.WriteLine("close menu");
            if (Menu.Width == 360) btnMenuClose.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            if (MenuProfiles.Width == 360) btnMenuProfilesClose.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            if (MenuSettings.Width == 360) btnMenuSettingsClose.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            if (MenuAbout.Width == 360) btnMenuAboutClose.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            btnMenu.Focusable = true;
            btnMenu.Focus();
            btnMenuClose.Focusable = false;
            btnMenuProfilesClose.Focusable = false;
            btnMenuSettingsClose.Focusable = false;
            btnMenuAboutClose.Focusable = false;
        }

        private void CloseMenus(object sender, MouseButtonEventArgs e)
        {
            CloseMenus();
        }

        private void checkTray_Checked(object sender, RoutedEventArgs e)
        {
            _config.Settings[0].Tray = "True";
            CloseMenus();
            MoveToTray();
        }

        private void checkTray_Unchecked(object sender, RoutedEventArgs e)
        {
            _config.Settings[0].Tray = "False";
            CloseMenus();
            RemoveFromTray();
        }

        private void MoveToTray()
        {
            Topmost = true;
            showintray = true;
            ShowInTaskbar = false;
            TitleBar.Visibility = Visibility.Collapsed;
            MainGrid.Margin = new Thickness(0, 9, 0, 0);
            this.Height -= 22;
            int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - (int)this.Width + 1;
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - (int)this.Height + 1;
            this.Left = screenWidth;
            this.Top = screenHeight;
        }

        private void RemoveFromTray()
        {
            Topmost = false;
            showintray = false;
            ShowInTaskbar = true;
            TitleBar.Visibility = Visibility.Visible;
            MainGrid.Margin = new Thickness(0, 31, 0, 0);
            Height += 22;
            int screenWidth = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 2 - (int)this.Width / 2;
            int screenHeight = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height / 2 - (int)this.Height / 2;
            Left = screenWidth;
            Top = screenHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EnableBlur();
            //set position to tray bar
            if (showintray)
            {
                MoveToTray();
            }
        }

        private void ShowWindow()
        {
            WindowState = WindowState.Normal;
            Show();
            Activate();
        }

        private void Trayicon_Click(object sender, EventArgs e)
        {
            if (!active)
            {
                ShowWindow();
            }
        }

        private void Trayicon_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            active = IsActive;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            NativeMethods.UnregisterHotKey(hwnd, HOTKEY_ID); //WINDOWS + NUMKEY_0
            trayIcon.Visible = false;
            SaveProfiles(sender, null);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            //if (showintray == "true") this.WindowState = WindowState.Minimized;
            CloseMenus();
            Debug.WriteLine("deactivating");
            if (showintray) Hide();
            Debug.WriteLine("deactivated");
        }

        private void ShowMessage(String text, Boolean buttons)
        {
            lblBlurText.Text = text;
            if(buttons) MessageButtons.Visibility = Visibility.Visible;
            else MessageButtons.Visibility = Visibility.Hidden;
            Message.Visibility = Visibility.Visible;
            Window.BeginAnimation(OpacityProperty, WindowAnimShow);
            Message.BeginAnimation(OpacityProperty, gridBlurAnimShow);
            WindowBlur.Radius = 5;
            btnThisSceenConfirmationCancel.Focusable = true;
            btnThisSceenConfirmationCancel.Focus();
            btnMenu.Focusable = false;
        }

        private void HideMessage()
        {
            btnThisSceenConfirmationCancel.Focusable = false;
            btnMenu.Focusable = true;
            Button _lastFocus = FindName(lastFocus) as Button;
            if(_lastFocus != null) _lastFocus.Focus();
            Debug.WriteLine("esc");
            Window.BeginAnimation(OpacityProperty, WindowAnimHide);
            Message.BeginAnimation(OpacityProperty, gridBlurAnimHide);
        }

        private void btnBlurCancel_Click(object sender, RoutedEventArgs e)
        {
            if(Message.Opacity == 1) HideMessage();
        }
    }
}
