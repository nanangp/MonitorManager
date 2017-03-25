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
        public int Index { get; set; }
        public string Name { get; set; }

        public bool SupportsDDC { get; set; }
        public MonitorFeature Brightness;
        public MonitorFeature Contrast;
        public MonitorFeature RgbDrive;
        public MonitorFeature RgbGain;
        public MonitorFeature RedGain;
        public MonitorFeature GreenGain;
        public MonitorFeature BlueGain;
        public MonitorFeature Input;
        public MonitorFeature Sharpness;
        public MonitorFeature Volume;
        public MonitorFeature VCPFeature;
        //public Dictionary<string, string> Input;
        public Dictionary<string, string> Power;

        private uint _monitorCapabilities = 0u;
        private uint _supportedColorTemperatures = 0u;

        #endregion

        public Monitor(NativeStructures.PHYSICAL_MONITOR physicalMonitor)
        {
            HPhysicalMonitor = physicalMonitor.hPhysicalMonitor;
            Name = physicalMonitor.szPhysicalMonitorDescription;
            CheckCapabilities();
        }

        private void CheckCapabilities()
        {
            //NativeMethods.GetMonitorCapabilities(HPhysicalMonitor, ref _monitorCapabilities, ref _supportedColorTemperatures);
            CheckBrightness();
            CheckContrast();
            CheckRgbDrive();
            CheckRgbGain();
            CheckVolume();
            CheckInput();
            CheckPower();
            CheckSharpness();
            GetVCPStuff();
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

            NativeMethods.GetMonitorRedGreenOrBlueDrive(HPhysicalMonitor, NativeStructures.MC_DRIVE_TYPE.MC_BLUE_DRIVE, ref RgbDrive.Min, ref RgbDrive.Current, ref RgbDrive.Max);

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

        private void CheckInput()
        {
            Debug.WriteLine("Start CheckInput");

            NativeMethods.GetVCPFeatureAndVCPFeatureReply(HPhysicalMonitor, NativeConstants.SC_MONITORINPUT, IntPtr.Zero, ref Input.Current, ref Input.Max);
            Debug.WriteLine("Input: " + Input.Current);

            Debug.WriteLine("End CheckInput");
        }

        private void CheckPower()
        {
            Debug.WriteLine("Start CheckPower");

            // NativeMethods.GetVCPFeatureAndVCPFeatureReply(HPhysicalMonitor, NativeConstants.SC_MONITORPOWER, IntPtr.Zero, ref Power.Current, ref Power.Max);
            // Debug.WriteLine("Power: " + Power.Current);

            Debug.WriteLine("End CheckPower");
        }

        private void CheckSharpness()
        {
            Debug.WriteLine("Start CheckSharpness");
            
            NativeMethods.GetVCPFeatureAndVCPFeatureReply(HPhysicalMonitor, 135, IntPtr.Zero, ref Sharpness.Current, ref Sharpness.Max);
            Sharpness.Original = Sharpness.Current;
            Debug.WriteLine("Sharpness: " + Sharpness.Current + " " + Sharpness.Max);

            Debug.WriteLine("Start CheckSharpness");
        }

        private void CheckVolume()
        {
            Debug.WriteLine("Start CheckVolume");

            NativeMethods.GetVCPFeatureAndVCPFeatureReply(HPhysicalMonitor, NativeConstants.SC_MONITORVOLUME, IntPtr.Zero, ref Volume.Current, ref Volume.Max);
            Volume.Original = Volume.Current;

            Debug.WriteLine("End CheckVolume");
        }

        private void GetVCPStuff()
        {
            Debug.WriteLine("Start GetVCPStuff");

            int[] values = new int[0];

            uint strSize;
            NativeMethods.GetCapabilitiesStringLength(HPhysicalMonitor, out strSize);
            StringBuilder capabilities = new StringBuilder((int)strSize);
            NativeMethods.CapabilitiesRequestAndCapabilitiesReply(HPhysicalMonitor, capabilities, strSize);
            string capabilitiesStr = capabilities.ToString();

            Debug.WriteLine(capabilitiesStr);

            // Parse model version.
            Match match = NativeConstants.modelRegex.Match(capabilitiesStr);
            if (match.Success) Name = match.Groups[1].Value.Trim();

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

            // Parse source codes.
            match = NativeConstants.vcp60ValuesRegex.Match(capabilitiesStr);
            // if (match.Success) Input.Values = match.Groups[1].Value.Trim();

            Debug.WriteLine("End GetVCPStuff");
        }

        // Loop all VCP
        private void CheckALot()
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
