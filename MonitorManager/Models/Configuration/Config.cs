using System.Collections.Generic;
using System.Xml.Serialization;

namespace MonitorManager.Models.Configuration
{
    public class Config
    {
        [XmlElement("Profile")]
        public List<Profile> Profiles { get; set; } = new();

        public Settings Settings { get; set; } = new();
    }
}
