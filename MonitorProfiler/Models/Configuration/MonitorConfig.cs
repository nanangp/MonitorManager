using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonitorProfiler.Models.Configuration
{
    public class MonitorConfig
    {
        public int Index { get; set; }
        public int Brightness { get; set; }
        public int Contrast { get; set; }
        public int RedDrive { get; set; }
        public int GreenDrive { get; set; }
        public int BlueDrive { get; set; }
        public int RedGain { get; set; }
        public int GreenGain { get; set; }
        public int BlueGain { get; set; }
        public int Sharpness { get; set; }
        public int Volume { get; set; }
    }
}
