using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Entity.Response
{
    public class ChatMsgResponse:BaseResponse
    {
        public string action { get; set; }
        public string application { get; set; }
        public ChatMsgParams @params { get; set; }
        public string path { get; set; }
        public string uri { get; set; }
        public ChatMsgEntity[] entities { get; set; }
        public long timestamp { get; set; }
        public int duration { get; set; }
        public string organization { get; set; }
        public string applicationName { get; set; }
        public string cursor { get; set; }
        public int count { get; set; }
    }

    public class ChatMsgParams
    {
        public string[] limit { get; set; }
        public string[] cursor { get; set; }
        public string[] ql { get; set; }
    }

    public class ChatMsgEntity
    {
        public string uuid { get; set; }
        public string type { get; set; }
        public string from { get; set; }
        public long created { get; set; }
        public long modified { get; set; }
        public string username { get; set; }
        public string msg_id { get; set; }
        public string to { get; set; }
        public string chat_type { get; set; }
        public string groupid { get; set; }
        public ChatMsgPayload payload { get; set; }
        public long timestamp { get; set; }
    }

    public class ChatMsgPayload
    {
        public ChatMsg[] bodies { get; set; }
        /// <summary>
        /// 扩展消息
        /// </summary>
        public IDictionary<string, object> ext { get; set; }
    }

    public class ChatMsg
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// txt: 文本消息；img: 图片；loc: 位置；audio: 语音
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 语音时长，单位为秒，这个属性只有语音消息有
        /// </summary>
        public int length { get; set; }
        /// <summary>
        /// 图片语音等文件的网络URL，图片和语音消息有这个属性
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 获取文件的secret，图片和语音消息有这个属性
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        /// 发送的位置的纬度，只有位置消息有这个属性
        /// </summary>
        public float lat { get; set; }
        /// <summary>
        /// 位置经度，只有位置消息有这个属性
        /// </summary>
        public float lng { get; set; }
        /// <summary>
        /// 位置消息详细地址，只有位置消息有这个属性
        /// </summary>
        public string addr { get; set; }
        /// <summary>
        /// 文件名字，图片和语音消息有这个属性
        /// </summary>
        public string filename { get; set; }
        /// <summary>
        /// 上传缩略图地址
        /// </summary>
        public string thumb { get; set; }
        /// <summary>
        /// thumb_secret在上传缩略图后会返回
        /// </summary>
        public string thumb_secret { get; set; }
    }
}
