using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Entity.Request
{
    public class UpdateChatGroupRequest
    {
        /// <summary>
        /// 群组名称，此属性为必须的
        /// </summary>
        public string groupname { get; set; }
        /// <summary>
        /// 群组描述，此属性为必须的
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 群组成员最大数（包括群主），值为数值类型，默认值200，最大值2000，此属性为可选的
        /// </summary>
        public int maxusers { get; set; }
    }
}
