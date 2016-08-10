using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Config.Configuration
{
    public class ServerConfigElementCollection:ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServerConfigElement) element).AppName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override string ElementName
        {
            get { return "server"; }
        }

        public ServerConfigElement this[int index]
        {
            get
            {
                return (ServerConfigElement) BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index,this);
            }
        }
    }
}
