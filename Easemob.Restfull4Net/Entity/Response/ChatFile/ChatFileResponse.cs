using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Easemob.Restfull4Net.Entity.Response
{
    public class ChatFileResponse:BaseResponse
    {
        public string action { get; set; }
        public string application { get; set; }
        public string path { get; set; }
        public string uri { get; set; }
        public ChatFileEntity[] entities { get; set; }
        public long timestamp { get; set; }
        public int duration { get; set; }
        public string organization { get; set; }
        public string applicationName { get; set; }
        public string cursor { get; set; }
        public int count { get; set; }
    }

    public class ChatFileEntity
    {
        public string uuid { get; set; }
        public string type { get; set; }
        public string share_secret { get; set; }
    }
}
