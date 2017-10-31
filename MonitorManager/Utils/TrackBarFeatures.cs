using MonitorManager.Models.Display;
using System.Windows.Controls;

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
            uint newValue = (uint)bar.Value;
            if(bar.Maximum < 100) _label.Content = string.Format("{0}", newValue);
            else _label.Content = string.Format("{0}", newValue);

            switch (_type)
            {
                case FeatureType.Brightness:
                    currentMonitor.Brightness.Current = newValue;
                    currentMonitor.SetBrightness(newValue);
                    break;
                case FeatureType.Contrast:
                    currentMonitor.Contrast.Current = newValue;
                    currentMonitor.SetContrast(newValue);
                    break;
                case FeatureType.RedGain:
                    currentMonitor.RedGain.Current = newValue;
                    currentMonitor.SetRedGain(newValue);
                    break;
                case FeatureType.GreenGain:
                    currentMonitor.GreenGain.Current = newValue;
                    currentMonitor.SetGreenGain(newValue);
                    break;
                case FeatureType.BlueGain:
                    currentMonitor.BlueGain.Current = newValue;
                    currentMonitor.SetBlueGain(newValue);
                    break;
                case FeatureType.Sharpness:
                    currentMonitor.Sharpness.Current = newValue;
                    currentMonitor.SetSharpness(newValue);
                    break;
                case FeatureType.Volume:
                    currentMonitor.Volume.Current = newValue;
                    currentMonitor.SetVolume(newValue);
                    break;
            }
        }
    }
}
