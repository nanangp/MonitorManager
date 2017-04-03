using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MonitorProfiler.Models.Configuration
{
    public class Config
    {
        [XmlElement("Profile")]
        public List<Profile> Profiles { get; set; }
    }
}
