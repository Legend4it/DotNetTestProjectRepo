using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSettingDependOnConfigMode
{
    public class DevModeSetting:ConfigurationSection
    {
        public override bool IsReadOnly()
        {
            return false;
        }

        [ConfigurationProperty("testProp", IsRequired = false)]
        public string TestProp
        {
            get { return (string) this["testProp"]; }
            set { this["testProp"] = value; }
        }
    }

    public static class DevModeSettingHandler
    {
        public static DevModeSetting GetDevModeSetting()
        {
            return GetDevModeSetting("debug");
        }

        public static DevModeSetting GetDevModeSetting(string mode)
        {
            var section = "DevModeSettings/" + mode;
            DevModeSetting config = (DevModeSetting)ConfigurationManager.GetSection(section);
            return config;
        }
    }
}
