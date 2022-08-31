using System.Windows.Controls;

using MonitorManager.Models.Display;

namespace MonitorManager.GUI
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
        public void UpdateScreenWithBarValue(Slider bar, Monitor currentMonitor)
        {
            UpdateScreenWithBarValue(bar, currentMonitor, true);
        }

        // Update objects with new value from trackbar
        public void UpdateScreenWithBarValue(Slider bar, Monitor currentMonitor, bool setMonitor)
        {
            System.Diagnostics.Debug.WriteLine("UpdateScreenWithBarValue setMonitor: " + setMonitor);
            uint newValue = (uint)bar.Value;
            if (bar.Maximum < 100) _label.Content = string.Format("{0}", newValue);
            else _label.Content = string.Format("{0}", newValue);

            switch (_type)
            {
                case FeatureType.Brightness:
                    currentMonitor.Brightness.Current = newValue;
                    if (setMonitor) currentMonitor.SetBrightness(newValue);
                    break;
                case FeatureType.Contrast:
                    currentMonitor.Contrast.Current = newValue;
                    if (setMonitor) currentMonitor.SetContrast(newValue);
                    break;
                case FeatureType.RedGain:
                    currentMonitor.RedGain.Current = newValue;
                    if (setMonitor) currentMonitor.SetRedGain(newValue);
                    break;
                case FeatureType.GreenGain:
                    currentMonitor.GreenGain.Current = newValue;
                    if (setMonitor) currentMonitor.SetGreenGain(newValue);
                    break;
                case FeatureType.BlueGain:
                    currentMonitor.BlueGain.Current = newValue;
                    if (setMonitor) currentMonitor.SetBlueGain(newValue);
                    break;
                case FeatureType.Sharpness:
                    currentMonitor.Sharpness.Current = newValue;
                    if (setMonitor) currentMonitor.SetSharpness(newValue);
                    break;
                case FeatureType.Volume:
                    currentMonitor.Volume.Current = newValue;
                    if (setMonitor) currentMonitor.SetVolume(newValue);
                    break;
            }
        }
    }
}
