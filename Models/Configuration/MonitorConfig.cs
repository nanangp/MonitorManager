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
        public int GainRed { get; set; }
        public int GainGreen { get; set; }
        public int GainBlue { get; set; }
        public int Sharpness { get; set; }
        public int Volume { get; set; }
    }
}
