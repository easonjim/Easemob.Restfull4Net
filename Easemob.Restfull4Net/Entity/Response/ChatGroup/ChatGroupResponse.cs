using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Entity.Response
{
    public class ChatGroupResponse : BaseChatGroupResponse
    {
        public List<ChatGroup> data { get; set; }
    }

    public class ChatGroupResponseList : BaseChatGroupResponse
    {
        public List<ChatGroupList> data { get; set; }
    }

    public class ChatGroupResponseCreate : BaseChatGroupResponse
    {
        public ChatGroupCreate data { get; set; }
    }

    public class ChatGroupResponseUpdate : BaseChatGroupResponse
    {
        public ChatGroupUpdate data { get; set; }
    }

    public class ChatGroupResponseDelete : BaseChatGroupResponse
    {
        public ChatGroupDelete data { get; set; }
    }

    public class ChatGroupResponseMemberAll : BaseChatGroupResponse
    {
        public List<ChatGroupMemberAll> data { get; set; }
    }

    public class ChatGroupResponseMemberAdd : BaseChatGroupResponse
    {
        public ChatGroupMemberAdd data { get; set; }
    }

    public class ChatGroupResponseMemberAddBatch : BaseChatGroupResponse
    {
        public List<ChatGroupMemberAddBatch> data { get; set; }
    }

    public class ChatGroupResponseMemberDelete : BaseChatGroupResponse
    {
        public ChatGroupMemberDelete data { get; set; }
    }

    public class ChatGroupResponseMemberDeleteBatch : BaseChatGroupResponse
    {
        public List<ChatGroupMemberDeleteBatch> data { get; set; }
    }

    public class ChatGroupResponseUser : BaseChatGroupResponse
    {
        public List<ChatGroupUser> data { get; set; }
    }

    public class ChatGroupResponseChange : BaseChatGroupResponse
    {
        public ChatGroupChange data { get; set; }
    }

    public class ChatGroupResponseBlock : BaseChatGroupResponse
    {
        public List<string> data { get; set; }
    }

    public class ChatGroupResponseBlockAdd : BaseChatGroupResponse
    {
        public ChatGroupBlock data { get; set; }
    }

    public class ChatGroupResponseBlockAddBatch : BaseChatGroupResponse
    {
        public List<ChatGroupBlock> data { get; set; }
    }

    public class ChatGroupResponseBlockDelete : BaseChatGroupResponse
    {
        public ChatGroupBlock data { get; set; }
    }

    public class ChatGroupResponseBlockDeleteBatch : BaseChatGroupResponse
    {
        public List<ChatGroupBlock> data { get; set; }
    }

    public class BaseChatGroupResponse:BaseResponse
    {
        public string action { get; set; }
        public string application { get; set; }
        public ChatGroupParams @params { get; set; }
        public string path { get; set; }
        public string uri { get; set; }
        public ChatGroupEntity[] entities { get; set; }
        public long timestamp { get; set; }
        public int duration { get; set; }
        public string organization { get; set; }
        public string applicationName { get; set; }
        public string cursor { get; set; }
        public int count { get; set; }
    }

    public class ChatGroupParams
    {
        public string[] limit { get; set; }
        public string[] cursor { get; set; }
    }

    public class ChatGroupEntity
    {
        public string uuid { get; set; }
        public string type { get; set; }
        public long created { get; set; }
        public long modified { get; set; }
        public string username { get; set; }
        public bool activated { get; set; }
        public string device_token { get; set; }
        public string nickname { get; set; }
    }

    public class ChatGroup
    {
        /// <summary>
        /// 群组 ID，群组唯一标识符，由环信服务器生成，等同于单个用户的环信 ID。
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 群组名称，根据用户输入创建，字符串类型。
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 群组描述，根据用户输入创建，字符串类型。
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 群组类型：true：公开群，false：私有群。
        /// </summary>
        public bool @public { get; set; }
        /// <summary>
        /// 是否只有群成员可以进来发言。true：是，false：否。该字段只能在群组详情中查看。
        /// </summary>
        public bool membersonly { get; set; }
        /// <summary>
        /// 是否允许群成员邀请别人加入此群。 true：允许群成员邀请人加入此群，false：只有群主才可以往群里加人。REST创建群组不支持该字段设置，只能在群组详情中查看或通过SDK创建群组时设置。
        /// </summary>
        public bool allowinvites { get; set; }
        /// <summary>
        /// 群成员上限，创建群组的时候设置，可修改。
        /// </summary>
        public int maxusers { get; set; }
        /// <summary>
        /// 现有成员总数。
        /// </summary>
        public int affiliations_count { get; set; }
        /// <summary>
        /// 现有成员列表，包含了 owner 和 member。例如：“affiliations”:[{“owner”: “13800138001”},{“member”:“v3y0kf9arx”},{“member”:“xc6xrnbzci”}]。
        /// </summary>
        public ChatGroupAffiliation[] affiliations { get; set; }
    }

    public class ChatGroupAffiliation
    {
        /// <summary>
        /// 群主的环信 ID。例如：{“owner”: “13800138001”}。
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 群成员的环信 ID。例如：{“member”:“xc6xrnbzci”}。
        /// </summary>
        public string member { get; set; }
    }

    public class ChatGroupList
    {
        /// <summary>
        /// 【列表特有】群组ID
        /// </summary>
        public string groupid { get; set; }
        /// <summary>
        /// 【列表特有】群组名称
        /// </summary>
        public string groupname { get; set; }
        /// <summary>
        /// 【列表特有】群主环信ID
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 【列表特有】成员数量
        /// </summary>
        public int affiliations { get; set; }
        /// <summary>
        /// 【列表特有】类型
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 【列表特有】最后修改时间
        /// </summary>
        public string last_modified { get; set; }
    }

    public class ChatGroupCreate
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        public string groupid { get; set; }
    }

    public class ChatGroupUpdate
    {
        /// <summary>
        /// 群组成员最大数（包括群主）
        /// </summary>
        public bool maxusers { get; set; }
        /// <summary>
        /// 群组名称
        /// </summary>
        public bool groupname { get; set; }
        /// <summary>
        /// 群组描述
        /// </summary>
        public bool description { get; set; }
    }

    public class ChatGroupDelete
    {
        public bool success { get; set; }
        /// <summary>
        /// 群组ID
        /// </summary>
        public string groupid { get; set; }
    }

    public class ChatGroupMemberAll
    {
        /// <summary>
        /// 成员
        /// </summary>
        public string member { get; set; }
        /// <summary>
        /// 群主
        /// </summary>
        public string owner { get; set; }
    }

    public class ChatGroupMemberAdd
    {
        public string action { get; set; }
        public bool result { get; set; }
        public string groupid { get; set; }
        public string user { get; set; }
    }

    public class ChatGroupMemberAddBatch
    {
        public List<string> newmembers { get; set; }
        public string action { get; set; }
        public string groupid { get; set; }
    }

    public class ChatGroupMemberDelete
    {
        public string action { get; set; }
        public string result { get; set; }
        public string groupid { get; set; }
        public string user { get; set; }
    }

    public class ChatGroupMemberDeleteBatch
    {
        public string action { get; set; }
        public string result { get; set; }
        public string groupid { get; set; }
        public string user { get; set; }
    }

    public class ChatGroupUser
    {
        public string groupid { get; set; }
        public string groupname { get; set; }
    }

    public class ChatGroupChange
    {
        public string newowner { get; set; }
    }

    public class ChatGroupBlock
    {
        public string action { get; set; }
        public string result { get; set; }
        public string groupid { get; set; }
        public string user { get; set; }
    }
}
