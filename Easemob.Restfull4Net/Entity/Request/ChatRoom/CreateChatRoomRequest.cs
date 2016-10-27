using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Entity.Request
{
    public class CreateChatRoomRequest
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
        /// <summary>
        /// 聊天室的管理员，此属性为必须的
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 聊天室成员，此属性为可选的，但是如果加了此项，数组元素至少一个（注：群主jma1不需要写入到members里面）
        /// </summary>
        public string[] members { get; set; }
    }
}
