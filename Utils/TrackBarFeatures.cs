using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MonitorProfiler.Win32;
using MonitorProfiler.Models.Display;
using System.Diagnostics;

namespace MonitorProfiler.GUI
{
    public class TrackBarFeatures
    {
        private Label _label;
        private FeatureType _type;

        // Constructor
        public TrackBarFeatures(FeatureType type, Label label)
        {
            _type = type;
            _label = label;
        }
        
        // Update objects with new value from trackbar
        public void UpdateScreenWithBarValue(TrackBar bar, Monitor currentMonitor)
        {
            uint newValue = (uint)bar.Value;
            if(bar.Maximum < 100) _label.Text = string.Format("{0}", newValue);
            else _label.Text = string.Format("{0}%", newValue);

            switch (_type)
            {
                case FeatureType.Brightness:
                    currentMonitor.Brightness.Current = newValue;
                    NativeMethods.SetMonitorBrightness(currentMonitor.HPhysicalMonitor, newValue);
                    break;
                case FeatureType.Contrast:
                    currentMonitor.Contrast.Current = newValue;
                    NativeMethods.SetMonitorContrast(currentMonitor.HPhysicalMonitor, newValue);
                    break;
                case FeatureType.RedGain:
                    currentMonitor.RedGain.Current = newValue;
                    NativeMethods.SetMonitorRedGreenOrBlueGain(currentMonitor.HPhysicalMonitor, NativeStructures.MC_GAIN_TYPE.MC_RED_GAIN, newValue);
                    break;
                case FeatureType.GreenGain:
                    currentMonitor.GreenGain.Current = newValue;
                    NativeMethods.SetMonitorRedGreenOrBlueGain(currentMonitor.HPhysicalMonitor, NativeStructures.MC_GAIN_TYPE.MC_GREEN_GAIN, newValue);
                    break;
                case FeatureType.BlueGain:
                    currentMonitor.BlueGain.Current = newValue;
                    NativeMethods.SetMonitorRedGreenOrBlueGain(currentMonitor.HPhysicalMonitor, NativeStructures.MC_GAIN_TYPE.MC_BLUE_GAIN, newValue);
                    break;
                case FeatureType.Sharpness:
                    currentMonitor.Sharpness.Current = newValue;
                    NativeMethods.SetVCPFeature(currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORSHARPNESS, newValue);
                    break;
                case FeatureType.Volume:
                    currentMonitor.Volume.Current = newValue;
                    NativeMethods.SetVCPFeature(currentMonitor.HPhysicalMonitor, NativeConstants.SC_MONITORVOLUME, newValue);
                    break;
            }
        }
    }
    /*
    public class TrackBarCollection: Dictionary <TrackBar, TrackBarFeatures>
    {
        public void TriggerChanges(object sender, Monitor currentMonitor)
        {
            TrackBar bar = sender as TrackBar;
            if (bar == null) return;

            TrackBarFeatures feature = this[bar];
            if (feature != null) feature.UpdateScreenWithBarValue(bar, currentMonitor);
        }
    }
    */
}
