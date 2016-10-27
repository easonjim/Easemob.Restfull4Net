using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Entity.Response
{
    public class ChatRoomResponseCreate:BaseChatRoomResponse
    {
        public ChatRoomCreate data { get; set; }
    }

    public class ChatRoomResponseUpdate : BaseChatRoomResponse
    {
        public ChatRoomUpdate data { get; set; }
    }

    public class ChatRoomResponseDelete : BaseChatRoomResponse
    {
        public ChatRoomDelete data { get; set; }
    }

    public class ChatRoomResponse : BaseChatRoomResponse
    {
        public List<ChatRoom> data { get; set; }
    }

    public class ChatRoomResponseDetail : BaseChatRoomResponse
    {
        public List<ChatRoomDetail> data { get; set; }
    }

    public class ChatRoomResponseJoined : BaseChatRoomResponse
    {
        public List<ChatRoomJoined> data { get; set; }
    }

    public class ChatRoomResponseMemberAdd : BaseChatRoomResponse
    {
        public ChatRoomMemberAdd data { get; set; }
    }

    public class ChatRoomResponseMemberAddBatch : BaseChatRoomResponse
    {
        public List<ChatRoomMemberAddBatch> data { get; set; }
    }

    public class ChatRoomResponseMemberDelete : BaseChatRoomResponse
    {
        public ChatRoomMemberDelete data { get; set; }
    }

    public class ChatRoomResponseMemberDeleteBatch : BaseChatRoomResponse
    {
        public List<ChatRoomMemberDeleteBatch> data { get; set; }
    }
    
    public class BaseChatRoomResponse:BaseResponse
    {
        public string action { get; set; }
        public string application { get; set; }
        public ChatRoomParams @params { get; set; }
        public string path { get; set; }
        public string uri { get; set; }
        public ChatRoomEntity[] entities { get; set; }
        public long timestamp { get; set; }
        public int duration { get; set; }
        public string organization { get; set; }
        public string applicationName { get; set; }
        public string cursor { get; set; }
        public int count { get; set; }
    }

    public class ChatRoomParams
    {
        public string[] limit { get; set; }
        public string[] cursor { get; set; }
    }

    public class ChatRoomEntity
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

    public class ChatRoom
    {
        /// <summary>
        /// 天室 ID，聊天室唯一标识符，由环信服务器生成。
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 聊天室名称，任意字符串。
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 聊天室创建者的 username。例如：{“owner”: “13800138001”}。
        /// </summary>
        public string owner { get; set; }
        /// <summary>
        /// 现有成员总数。
        /// </summary>
        public int affiliations_count { get; set; }
    }
    
    public class ChatRoomDetail
    {
        public bool membersonly { get; set; }
        public bool allowinvites { get; set; }
        public bool @public { get; set; }
        /// <summary>
        /// 聊天室名称，任意字符串。
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 聊天室描述，任意字符串。
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 现有成员列表，包含了 owner 和 member。例如： “affiliations”:[{“owner”: “13800138001”},{“member”:”v3y0kf9arx”},{“member”:”xc6xrnbzci”}]。
        /// </summary>
        public List<Affiliation> affiliations { get; set; }
        /// <summary>
        /// 聊天室 ID，聊天室唯一标识符，由环信服务器生成。
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 聊天室成员上限，创建聊天室的时候设置，可修改。
        /// </summary>
        public int maxusers { get; set; }
        /// <summary>
        /// 现有成员总数。
        /// </summary>
        public int affiliations_count { get; set; }
    }

    public class Affiliation
    {
        /// <summary>
        /// 聊天室创建者的 username。例如：{“owner”: “13800138001”}。
        /// </summary>
        public string owner { get; set; }
        public string member { get; set; }
    }

    public class ChatRoomJoined
    {
        /// <summary>
        /// 天室 ID，聊天室唯一标识符，由环信服务器生成。
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 聊天室名称，任意字符串。
        /// </summary>
        public string name { get; set; }
    }

    public class ChatRoomMemberAdd
    {

        /// <summary>
        /// 天室 ID，聊天室唯一标识符，由环信服务器生成。
        /// </summary>
        public string id { get; set; }
        public string user { get; set; }
        public string action { get; set; }
        public bool result { get; set; }
    }

    public class ChatRoomMemberDelete
    {

        /// <summary>
        /// 天室 ID，聊天室唯一标识符，由环信服务器生成。
        /// </summary>
        public string id { get; set; }
        public string user { get; set; }
        public string action { get; set; }
        public bool result { get; set; }
    }

    public class ChatRoomMemberAddBatch
    {
        public List<string> newmembers { get; set; }
        public string action { get; set; }
        /// <summary>
        /// 天室 ID，聊天室唯一标识符，由环信服务器生成。
        /// </summary>
        public string id { get; set; }
    }

    public class ChatRoomMemberDeleteBatch
    {
        public string action { get; set; }
        /// <summary>
        /// 天室 ID，聊天室唯一标识符，由环信服务器生成。
        /// </summary>
        public string id { get; set; }
        public string result { get; set; }
        public string user { get; set; }
    }

    public class ChatRoomCreate
    {
        /// <summary>
        /// 天室 ID，聊天室唯一标识符，由环信服务器生成。
        /// </summary>
        public string id { get; set; }
    }

    public class ChatRoomUpdate
    {
        /// <summary>
        /// 聊天室成员上限，创建聊天室的时候设置，可修改。
        /// </summary>
        public bool maxusers { get; set; }
        /// <summary>
        /// 聊天室名称，任意字符串。
        /// </summary>
        public bool name { get; set; }
        /// <summary>
        /// 聊天室描述，任意字符串。
        /// </summary>
        public bool description { get; set; }
    }

    public class ChatRoomDelete
    {
        /// <summary>
        /// 天室 ID，聊天室唯一标识符，由环信服务器生成。
        /// </summary>
        public string id { get; set; }
        public bool success { get; set; }
    }
}
