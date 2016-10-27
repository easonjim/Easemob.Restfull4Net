using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Entity.Request
{
    public class UpdateChatRoomRequest
    {
        /// <summary>
        /// 聊天室名称，此属性为必须的
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 聊天室描述，此属性为必须的
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 聊天室成员最大数（包括群主），值为数值类型，默认值200，最大值5000，此属性为可选的
        /// </summary>
        public int maxusers { get; set; }
    }
}
