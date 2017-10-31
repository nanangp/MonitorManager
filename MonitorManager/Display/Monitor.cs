using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonitorProfiler.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MonitorProfiler.Models.Display
{
    public class Monitor
    {
        #region Declarations

        public IntPtr HPhysicalMonitor { get; set; }
        public string Name { get; set; }

        public bool SupportsDDC { get; set; }
        public MonitorFeature Capabilities;
        public MonitorFeature Brightness;
        public MonitorFeature Contrast;
        public MonitorFeature RedDrive;
        public MonitorFeature GreenDrive;
        public MonitorFeature BlueDrive;
        public MonitorFeature RedGain;
        public MonitorFeature GreenGain;
        public MonitorFeature BlueGain;
        public MonitorFeature Source;
        public MonitorFeature PowerMode;
        public MonitorFeature Sharpness;
        public MonitorFeature Volume;
        public NativeStructures.MonitorCap[] Sources;
        public NativeStructures.MonitorCap[] PowerModes;

        private uint _monitorCapabilities = 0u;
        private uint _supportedColorTemperatures = 0u;

        #endregion

        public Monitor(NativeStructures.PHYSICAL_MONITOR physicalMonitor)
        {
            HPhysicalMonitor = physicalMonitor.hPhysicalMonitor;
            Name = physicalMonitor.szPhysicalMonitorDescription;
            //CheckCapabilities();
        }

        public void CheckCapabilities()
        {
            //NativeMethods.GetMonitorCapabilities(HPhysicalMonitor, ref _monitorCapabilities, ref _supportedColorTemperatures);
            CheckBrightness();
            CheckContrast();
            CheckRgbDrive();
            CheckRgbGain();
            CheckVolume();
            // not useful anymore, we check at each source menu click
            //CheckInput();
            CheckPower();
            CheckSharpness();
            GetCapabilities();
            //CheckALot();
        }

        public void CheckBrightness()
        {
            Debug.WriteLine("Start CheckBrightness");

            //Brightness.Supported = ((int)NativeStructures.MC_MONITOR_CAPABILITIES.MC_CAPS_BRIGHTNESS & _monitorCapabilities) > 0;
            //if (Brightness.Supported)
            //{
            //short Current = -1, Min = -1, Max = -1;
            NativeMethods.GetMonitorBrightness(HPhysicalMonitor, ref Brightness.Min, ref Brightness.Current, ref Brightness.Max);
            Brightness.Original = Brightness.Current;
            //}

            Debug.WriteLine("End CheckBrightness");
        }

        public void CheckContrast()
        {
            Debug.WriteLine("Start CheckContrast");

            //Contrast.Supported = ((int)NativeStructures.MC_MONITOR_CAPABILITIES.MC_CAPS_CONTRAST & _monitorCapabilities) > 0;
            //if (Contrast.Supported)
            //{
            NativeMethods.GetMonitorContrast(HPhysicalMonitor, ref Contrast.Min, ref Contrast.Current, ref Contrast.Max);
            Contrast.Original = Contrast.Current;

            Debug.WriteLine("End CheckContrast");
        }

        public void CheckRgbDrive()
        {
            Debug.WriteLine("Start CheckRgbDrive");

            NativeMethods.GetMonitorRedGreenOrBlueDrive(HPhysicalMonitor, NativeStructures.MC_DRIVE_TYPE.MC_RED_DRIVE, ref RedDrive.Min, ref RedDrive.Current, ref RedDrive.Max);
            NativeMethods.GetMonitorRedGreenOrBlueDrive(HPhysicalMonitor, NativeStructures.MC_DRIVE_TYPE.MC_GREEN_DRIVE, ref GreenDrive.Min, ref GreenDrive.Current, ref GreenDrive.Max);
            NativeMethods.GetMonitorRedGreenOrBlueDrive(HPhysicalMonitor, NativeStructures.MC_DRIVE_TYPE.MC_BLUE_DRIVE, ref BlueDrive.Min, ref BlueDrive.Current, ref BlueDrive.Max);
            RedDrive.Original = RedDrive.Current;
            GreenDrive.Original = GreenDrive.Current;
            BlueDrive.Original = BlueDrive.Current;

            Debug.WriteLine("RgbDrive.Min: " + RedDrive.Min);
            Debug.WriteLine("RgbDrive.Current: " + RedDrive.Current);
            Debug.WriteLine("RgbDrive.Max: " + RedDrive.Max);

            Debug.WriteLine("End CheckRgbDrive");
        }

        public void CheckRgbGain()
        {
            Debug.WriteLine("Start CheckRgbGain");

            NativeMethods.GetMonitorRedGreenOrBlueGain(HPhysicalMonitor, NativeStructures.MC_GAIN_TYPE.MC_RED_GAIN, ref RedGain.Min, ref RedGain.Current, ref RedGain.Max);
            NativeMethods.GetMonitorRedGreenOrBlueGain(HPhysicalMonitor, NativeStructures.MC_GAIN_TYPE.MC_GREEN_GAIN, ref GreenGain.Min, ref GreenGain.Current, ref GreenGain.Max);
            NativeMethods.GetMonitorRedGreenOrBlueGain(HPhysicalMonitor, NativeStructures.MC_GAIN_TYPE.MC_BLUE_GAIN, ref BlueGain.Min, ref BlueGain.Current, ref BlueGain.Max);
            RedGain.Original = RedGain.Current;
            GreenGain.Original = GreenGain.Current;
            BlueGain.Original = BlueGain.Current;

            Debug.WriteLine("End CheckRgbGain");
        }

        public void CheckInput()
        {
            Debug.WriteLine("Start CheckInput");

            NativeMethods.GetVCPFeatureAndVCPFeatureReply(HPhysicalMonitor, NativeConstants.SC_MONITORINPUT, IntPtr.Zero, ref Source.Current, ref Source.Max);
            Debug.WriteLine("Input: " + Source.Current);

            Debug.WriteLine("End CheckInput");
        }

        public void CheckPower()
        {
            Debug.WriteLine("Start CheckPower");

            NativeMethods.GetVCPFeatureAndVCPFeatureReply(HPhysicalMonitor, NativeConstants.SC_MONITORPOWER, IntPtr.Zero, ref PowerMode.Current, ref PowerMode.Max);
            Debug.WriteLine("Power Mode: " + PowerMode.Current);

            Debug.WriteLine("End CheckPower");
        }

        public void CheckSharpness()
        {
            Debug.WriteLine("Start CheckSharpness");
            
            NativeMethods.GetVCPFeatureAndVCPFeatureReply(HPhysicalMonitor, 135, IntPtr.Zero, ref Sharpness.Current, ref Sharpness.Max);
            Sharpness.Original = Sharpness.Current;
            Debug.WriteLine("Sharpness: " + Sharpness.Current + " " + Sharpness.Max);

            Debug.WriteLine("Start CheckSharpness");
        }

        public void CheckVolume()
        {
            Debug.WriteLine("Start CheckVolume");

            NativeMethods.GetVCPFeatureAndVCPFeatureReply(HPhysicalMonitor, NativeConstants.SC_MONITORVOLUME, IntPtr.Zero, ref Volume.Current, ref Volume.Max);
            Volume.Original = Volume.Current;

            Debug.WriteLine("End CheckVolume");
        }

        public void GetCapabilities()
        {
            Debug.WriteLine("Start GetVCPStuff");

            int[] values = new int[0];
            uint strSize;

            NativeMethods.GetCapabilitiesStringLength(HPhysicalMonitor, out strSize);
            StringBuilder capabilities = new StringBuilder((int)strSize);
            NativeMethods.CapabilitiesRequestAndCapabilitiesReply(HPhysicalMonitor, capabilities, strSize);
            string capabilitiesStr = capabilities.ToString();

            // Parse model version.
            Match match = NativeConstants.modelRegex.Match(capabilitiesStr);
            if (match.Success) Name = match.Groups[1].Value.Trim();

            //// Sources
            // Parse source codes.
            match = NativeConstants.vcp60ValuesRegex.Match(capabilitiesStr);
            if (match.Success)
            {
                string valuesStr = match.Groups[1].Value.Trim();
                string[] valueArray = valuesStr.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                values = Array.ConvertAll(valueArray, s => int.Parse(s, System.Globalization.NumberStyles.HexNumber));
            }

            // Parse MCCS version.
            string[] sourceNames = new string[0];
            match = NativeConstants.mccsVersionRegex.Match(capabilitiesStr);
            if (match.Success)
            {
                string versionStr = match.Groups[1].Value.Trim();
                string[] versionArray = versionStr.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                int majorVersion = int.Parse(versionArray[0]);

                if (majorVersion < 3) sourceNames = NativeConstants.sourceNamesMccsV2;
                else sourceNames = NativeConstants.sourceNamesMccsV3;
            }

            // Prepare output.
            Sources = new NativeStructures.MonitorCap[values.Length];
            for (int i = 0; i < values.Length; ++i)
            {
                Sources[i].code = values[i];
                if (0 <= values[i] && values[i] < sourceNames.Length) Sources[i].name = sourceNames[values[i]];
                else Sources[i].name = "**Unrecognized**";
            }

            //// Power
            // Parse power mode codes.
            match = NativeConstants.vcpD6ValuesRegex.Match(capabilitiesStr);
            if (match.Success)
            {
                string valuesStr = match.Groups[1].Value.Trim();
                string[] valueArray = valuesStr.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

                values = Array.ConvertAll(valueArray, s => int.Parse(s, System.Globalization.NumberStyles.HexNumber));
            }

            string[] powerModes = NativeConstants.powerStates;
            // Prepare output.
            PowerModes = new NativeStructures.MonitorCap[values.Length];
            for (int i = 0; i < values.Length; ++i)
            {
                PowerModes[i].code = values[i];
                if (0 <= values[i] && values[i] < powerModes.Length) PowerModes[i].name = powerModes[values[i]];
                else PowerModes[i].name = "**Unrecognized**";
            }

            Debug.WriteLine(capabilitiesStr);
            Debug.WriteLine("End GetVCPStuff");
        }

        // Loop all VCP
        public void CheckALot()
        {
            IntPtr output = new IntPtr();
            for (int i = 0; i <= 255; i++)
            {
                Sharpness.Max = 0;
                Sharpness.Current = 0;
                NativeMethods.GetVCPFeatureAndVCPFeatureReply(HPhysicalMonitor, (byte)i, output, ref Sharpness.Current, ref Sharpness.Max);
                Debug.WriteLine(i.ToString("X") + ": " + Sharpness.Current + " " + Sharpness.Max);
            }
        }
    }
}
