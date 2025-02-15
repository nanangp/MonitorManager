﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace MonitorManager.Win32
{
    public class NativeMethods
    {
        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref System.Drawing.Rectangle lprcMonitor, IntPtr dwData);
        public delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, NativeStructures.RECT rect, IntPtr dwData);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        [DllImport("User32.dll")]
        public static extern bool RegisterHotKey([In] IntPtr hWnd, [In] int id, [In] uint fsModifiers, [In] uint vk);

        [DllImport("User32.dll")]
        public static extern bool UnregisterHotKey([In] IntPtr hWnd, [In] int id);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplayDevices(string lpDevice, uint iDevNum, ref NativeStructures.DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetMonitorInfo(IntPtr hmonitor, [In, Out] NativeStructures.MONITORINFO info);

        [DllImport("user32.dll", EntryPoint = "MonitorFromWindow", SetLastError = true)]
        public static extern IntPtr MonitorFromWindow([In] IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", EntryPoint = "MonitorFromPoint", SetLastError = true)]
        public static extern IntPtr MonitorFromPoint([In] Point pt, uint dwFlags);

        [DllImport("user32.dll", EntryPoint = "MonitorFromRect", SetLastError = true)]
        public static extern IntPtr MonitorFromRect(ref System.Drawing.Rectangle lprect, uint dwFlags);

        [DllImport("dxva2.dll", EntryPoint = "GetNumberOfPhysicalMonitorsFromHMONITOR", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetNumberOfPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, ref uint pdwNumberOfPhysicalMonitors);

        [DllImport("dxva2.dll", EntryPoint = "GetPhysicalMonitorsFromHMONITOR", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetPhysicalMonitorsFromHMONITOR(IntPtr hMonitor, uint dwPhysicalMonitorArraySize, [Out] NativeStructures.PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "DestroyPhysicalMonitors", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyPhysicalMonitors(uint dwPhysicalMonitorArraySize, [Out] NativeStructures.PHYSICAL_MONITOR[] pPhysicalMonitorArray);

        [DllImport("dxva2.dll", EntryPoint = "DestroyPhysicalMonitor", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyPhysicalMonitor(IntPtr hMonitor);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorTechnologyType", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorTechnologyType(IntPtr hMonitor, ref NativeStructures.MC_DISPLAY_TECHNOLOGY_TYPE pdtyDisplayTechnologyType);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorCapabilities", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorCapabilities(IntPtr hMonitor, ref uint pdwMonitorCapabilities, ref uint pdwSupportedColorTemperatures);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorBrightness", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorBrightness([In] IntPtr hMonitor, [In] uint pdwNewBrightness);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorBrightness", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorBrightness([In] IntPtr hMonitor, ref uint pdwMinimumBrightness, ref uint pdwCurrentBrightness, ref uint pdwMaximumBrightness);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorBrightness", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorBrightness(IntPtr hMonitor, ref short pdwMinimumBrightness, ref short pdwCurrentBrightness, ref short pdwMaximumBrightness);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorContrast", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorContrast([In] IntPtr hMonitor, [In] uint pdwNewContrast);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorContrast", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorContrast([In] IntPtr hMonitor, ref uint pdwMinimumContrast, ref uint pdwCurrentContrast, ref uint pdwMaximumContrast);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorRedGreenOrBlueDrive", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorRedGreenOrBlueDrive([In] IntPtr hMonitor, [In] NativeStructures.MC_DRIVE_TYPE dtDriveType, [In] uint pdwNewDrive);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorRedGreenOrBlueDrive", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorRedGreenOrBlueDrive([In] IntPtr hMonitor, [In] NativeStructures.MC_DRIVE_TYPE dtDriveType, ref uint pdwMinimumDrive, ref uint pdwCurrentDrive, ref uint pdwMaximumDrive);

        [DllImport("dxva2.dll", EntryPoint = "SetMonitorRedGreenOrBlueGain", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetMonitorRedGreenOrBlueGain([In] IntPtr hMonitor, [In] NativeStructures.MC_GAIN_TYPE dtDriveType, [In] uint pdwNewGain);

        [DllImport("dxva2.dll", EntryPoint = "GetMonitorRedGreenOrBlueGain", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorRedGreenOrBlueGain([In] IntPtr hMonitor, [In] NativeStructures.MC_GAIN_TYPE dtDriveType, ref uint pdwMinimumGain, ref uint pdwCurrentGain, ref uint pdwMaximumGain);

        [DllImport("Dxva2.dll", EntryPoint = "GetCapabilitiesStringLength", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCapabilitiesStringLength(IntPtr hMonitor, out uint numCharacters);

        [DllImport("dxva2.dll", EntryPoint = "CapabilitiesRequestAndCapabilitiesReply", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CapabilitiesRequestAndCapabilitiesReply(IntPtr hMonitor, StringBuilder capabilities, uint capabilitiesLength);

        [DllImport("dxva2.dll", EntryPoint = "SetVCPFeature", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetVCPFeature([In] IntPtr hMonitor, byte bVCPCode, uint dwNewValue);

        [DllImport("dxva2.dll", EntryPoint = "GetVCPFeatureAndVCPFeatureReply", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetVCPFeatureAndVCPFeatureReply([In] IntPtr hMonitor, byte bVCPCode, [Out] IntPtr pvct, ref uint pdwCurrentValue, ref uint pdwMaximumValue);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref NativeStructures.WindowCompositionAttributeData data);

        [DllImport("dwmapi.dll", EntryPoint = "#127")]
        internal static extern void DwmGetColorizationParameters(ref NativeStructures.DWMCOLORIZATIONPARAMS dp);
    }

    public class NativeConstants
    {
        public const int WM_SYSCOMMAND = 0x112;
        public const byte SC_MONITORSOURCES = 0x60;
        public const byte SC_MONITORVOLUME = 0x62;
        public const byte SC_MONITORPOWER = 0xD6;
        public const int SC_MONITORSHARPNESS = 0x87;
        public const int SC_MONITORSLEEP = 0xF170;

        public static readonly string modelPattern = @"model\((.*?)\)";
        // Operates on result of CapabilitiesRequestAndCapabilitiesReply(). Extracts vcp code 60 values into capture group 1.
        public static readonly string vcp60ValuesPattern = @"vcp\((?:.*?\(.*?\))*[^\(\)]*?60 ?\((.*?)\)";
        public static readonly string vcpD6ValuesPattern = @"vcp\((?:.*?\(.*?\))*[^\(\)]*?D6 ?\((.*?)\)";
        // Operates on result of CapabilitiesRequestAndCapabilitiesReply(). Extracts the MCCS version.
        public static readonly string mccsVersionPattern = @"mccs_ver\((.*?)\)";
        public static readonly Regex modelRegex = new Regex(modelPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex vcp60ValuesRegex = new Regex(vcp60ValuesPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex vcpD6ValuesRegex = new Regex(vcpD6ValuesPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex mccsVersionRegex = new Regex(mccsVersionPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // Sources in MCCS v2.0 == v2.1, and both are a subset of 2.2, so we use a single array to cover them all.
        // Note that the standards use one-based indexing, so we just add a dummy element at the start.
        public static readonly string[] sourceNamesMccsV2 = {
            "**undefined**",
            "VGA 1",
            "VGA 2",
            "DVI 1",
            "DVI 2",
            "Composite 1",
            "Composite 2",
            "S-video 1",
            "S-video 2",
            "Tuner 1",
            "Tuner 2",
            "Tuner 3",
            "Component 1",
            "Component 2",
            "Component 3",
            "DisplayPort 1",
            "DisplayPort 2",
            "HDMI 1",
            "HDMI 2"
        };

        // Note that MCCS v3.0 was not well adopted, so 2.2a has become the active standard.
        // Note that the standards use one-based indexing, so we just add a dummy element at the start.
        public static readonly string[] sourceNamesMccsV3 = {
            "**undefined**",
            "VGA 1",
            "VGA 2",
            "DVI 1",
            "DVI 2",
            "Composite 1",
            "Composite 2",
            "S-video 1",
            "S-video 2",
            "Tuner - Analog 1",
            "Tuner - Analog 2",
            "Tuner - Digital 1",
            "Tuner - Digital 2",
            "Component 1",
            "Component 2",
            "Component 3",
            "**Unrecognized**",
            "DisplayPort 1",
            "DisplayPort 2"
        };

        public static readonly string[] powerModes = {
            "**undefined**",
            "Power on",
            "Standby",
            "Suspend",
            "Reduced power off",
            "Power off"
        };

        public static readonly string[] factoryResets = {
            "**undefined**",
            "**Unrecognized**",
            "**Unrecognized**",
            "**Unrecognized**",
            "Reset factory defaults",
            "Reset brightness and contrast",
            "**Unrecognized**",
            "**Unrecognized**",
            "Reset colors"
        };
    }

    public class NativeStructures
    {
        public struct MonitorCap
        {
            public int code;
            public string name;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct PHYSICAL_MONITOR
        {
            public IntPtr hPhysicalMonitor;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string szPhysicalMonitorDescription;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MonitorInfoEx
        {
            public int cbSize;
            public RECT rcMonitor; // Total area  
            public RECT rcWork; // Working area  
            public int dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string szDeviceName;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
        public class MONITORINFO
        {
            internal int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            internal RECT rcMonitor = new RECT();
            internal RECT rcWork = new RECT();
            internal int dwFlags;
        }

        public enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public enum MC_DISPLAY_TECHNOLOGY_TYPE
        {
            MC_SHADOW_MASK_CATHODE_RAY_TUBE,
            MC_APERTURE_GRILL_CATHODE_RAY_TUBE,
            MC_THIN_FILM_TRANSISTOR,
            MC_LIQUID_CRYSTAL_ON_SILICON,
            MC_PLASMA,
            MC_ORGANIC_LIGHT_EMITTING_DIODE,
            MC_ELECTROLUMINESCENT,
            MC_MICROELECTROMECHANICAL,
            MC_FIELD_EMISSION_DEVICE,
        }

        public enum MC_VCP_CODE_TYPE
        {
            MC_MOMENTARY,
            MC_SET_PARAMETER,
        }

        public enum MC_MONITOR_CAPABILITIES
        {
            MC_CAPS_NONE = 0x00000000,
            MC_CAPS_MONITOR_TECHNOLOGY_TYPE = 0x00000001,
            MC_CAPS_BRIGHTNESS = 0x00000002,
            MC_CAPS_CONTRAST = 0x00000004,
            MC_CAPS_COLOR_TEMPERATURE = 0x00000008,
            MC_CAPS_RED_GREEN_BLUE_GAIN = 0x00000010,
            MC_CAPS_RED_GREEN_BLUE_DRIVE = 0x00000020,
            MC_CAPS_DEGAUSS = 0x00000040,
            MC_CAPS_DISPLAY_AREA_POSITION = 0x00000080,
            MC_CAPS_DISPLAY_AREA_SIZE = 0x00000100,
            MC_CAPS_RESTORE_FACTORY_DEFAULTS = 0x00000400,
            MC_CAPS_RESTORE_FACTORY_COLOR_DEFAULTS = 0x00000800,
            MC_RESTORE_FACTORY_DEFAULTS_ENABLES_MONITOR_SETTINGS = 0x00001000
        }

        public enum MC_DRIVE_TYPE
        {
            MC_RED_DRIVE = 0,
            MC_GREEN_DRIVE = 1,
            MC_BLUE_DRIVE = 2
        }

        public enum MC_GAIN_TYPE
        {
            MC_RED_GAIN = 0,
            MC_GREEN_GAIN = 1,
            MC_BLUE_GAIN = 2
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [Flags()]
        public enum DisplayDeviceStateFlags : int
        {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x16,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }

        internal enum WindowCompositionAttribute
        {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }

        internal enum AccentState
        {
            ACCENT_DISABLED = 0,
            ACCENT_ENABLE_GRADIENT = 1,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct DWMCOLORIZATIONPARAMS
        {
            public UInt32 ColorizationColor;
            public UInt32 ColorizationAfterglow;
            public UInt32 ColorizationColorBalance;
            public UInt32 ColorizationAfterglowBalance;
            public UInt32 ColorizationBlurBalance;
            public UInt32 ColorizationGlassReflectionIntensity;
            public UInt32 ColorizationOpaqueBlend;
        }
    }
}