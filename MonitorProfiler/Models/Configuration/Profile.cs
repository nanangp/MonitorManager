using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MonitorProfiler.Models.Configuration
{
    public class Profile
    {
        public string Name { get; set; }

        [XmlElement("MonitorConfig")]
        public List<MonitorConfig> MonitorConfigs { get; set; }
    }
}
