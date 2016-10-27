using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Entity.Request
{
    public class CreateChatGroupRequest
    {
        /// <summary>
        /// 群组名称，此属性为必须的
        /// </summary>
        public string groupname { get; set; }
        /// <summary>
        /// 群组描述，此属性为必须的
        /// </summary>
        public string desc { get; set; }
        /// <summary>
        /// 是否是公开群，此属性为必须的
        /// </summary>
        public bool @public { get; set; }
        /// <summary>
        /// 群组成员最大数（包括群主），值为数值类型，默认值200，最大值2000，此属性为可选的
        /// </summary>
        public int maxusers { get; set; }
        /// <summary>
        /// 加入公开群是否需要批准，默认值是false（加入公开群不需要群主批准），此属性为必选的，私有群必须为true
        /// </summary>
        public bool approval { get; set; }
        /// <summary>
        /// 群组的管理员，此属性为必须的
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 群组成员，此属性为可选的，但是如果加了此项，数组元素至少一个（注：群主jma1不需要写入到members里面）
        /// </summary>
        public string[] members { get; set; }
    }
}
