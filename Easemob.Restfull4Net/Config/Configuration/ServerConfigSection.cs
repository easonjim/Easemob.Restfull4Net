using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Config.Configuration
{
    public class ServerConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("",IsDefaultCollection = true)]
        public ServerConfigElementCollection ServerConfigElementCollection
        {
            get { return (ServerConfigElementCollection) this[""]; }
        }
    }
}
