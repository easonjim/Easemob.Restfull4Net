using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Entity.Request
{
    public class MsgRequest<T> where T : BaseMsg
    {
        /// <summary>
        /// users 给用户发消息。chatgroups: 给群发消息，chatrooms: 给聊天室发消息
        /// </summary>
        public string target_type { get; set; }

        /// <summary>
        /// 注意这里需要用数组，数组长度建议不大于20，即使只有一个用户，也要用数组 ['u1']，给用户发送时数组元素是用户名，给群组发送时数组元素是groupid
        /// </summary>
        public string[] target { get; set; }

        /// <summary>
        /// 消息类型及消息内容
        /// </summary>
        public T msg { get; set; }

        /// <summary>
        /// 表示消息发送者。无此字段Server会默认设置为"from":"admin"，有from字段但值为空串("")时请求失败
        /// </summary>
        public string from { get; set; }

        /// <summary>
        /// 扩展属性，由APP自己定义。可以没有这个字段，但是如果有，值不能是"ext:null"这种形式，否则出错
        /// </summary>
        public IDictionary<string, object> ext { get; set; }
    }

    /// <summary>
    /// 消息基类，确定类型
    /// </summary>
    public class BaseMsg
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string type { get; internal set; }
    }

    /// <summary>
    /// 文本消息
    /// </summary>
    public class MsgText : BaseMsg
    {
        public MsgText()
        {
            base.type = "txt";
        }

        /// <summary>
        /// 消息内容，参考[[start:100serverintegration:30chatlog|聊天记录]]里的bodies内容
        /// </summary>
        public string msg { get; set; }
    }

    /// <summary>
    /// 图片消息
    /// </summary>
    public class MsgImg : BaseMsg
    {
        public MsgImg()
        {
            type = "img";
        }

        /// <summary>
        /// "https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/55f12940-64af-11e4-8a5b-ff2336f03252",成功上传文件返回的UUI
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// "24849.jpg",指定一个文件名
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// "VfEpSmSvEeS7yU8dwa9rAQc-DIL2HhmpujTNfSTsrDt6eNb_",成功上传文件后返回的secret
        /// </summary>
        public string secret { get; set; }

        /// <summary>
        /// {"width" : 480,"height" : 720},图片大小
        /// </summary>
        public MsgImgSize size { get; set; }
    }
    /// <summary>
    /// 图片消息的图片大小
    /// </summary>
    public class MsgImgSize
    {
        public int width { get; set; }
        public int height { get; set; }
    }

    /// <summary>
    /// 语言消息
    /// </summary>
    public class MsgAudio : BaseMsg
    {
        public MsgAudio()
        {
            type = "audio";
        }

        /// <summary>
        /// "https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/1dfc7f50-55c6-11e4-8a07-7d75b8fb3d42",成功上传文件返回的UUID
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// "messages.amr",指定一个文件名
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// "Hfx_WlXGEeSdDW-SuX2EaZcXDC7ZEig3OgKZye9IzKOwoCjM",成功上传文件后返回的secret
        /// </summary>
        public string secret { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int length { get; set; }
    }

    /// <summary>
    /// 视频消息
    /// </summary>
    public class MsgVideo : BaseMsg
    {
        public MsgVideo()
        {
            type = "video";
        }

        /// <summary>
        ///  "https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/671dfe30-7f69-11e4-ba67-8fef0d502f46",成功上传视频文件返回的UUID
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// "1418105136313.mp4",视频文件名称
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// "VfEpSmSvEeS7yU8dwa9rAQc-DIL2HhmpujTNfSTsrDt6eNb_",成功上传视频文件后返回的secret
        /// </summary>
        public string secret { get; set; }

        /// <summary>
        /// "https://a1.easemob.com/easemob-demo/chatdemoui/chatfiles/67279b20-7f69-11e4-8eee-21d3334b3a97",成功上传视频缩略图返回的UUID
        /// </summary>
        public string thumb { get; set; }

        /// <summary>
        /// 视频播放长度
        /// </summary>
        public int length { get; set; }

        /// <summary>
        /// 视频文件大小
        /// </summary>
        public int file_length { get; set; }

        /// <summary>
        /// "ZyebKn9pEeSSfY03ROk7ND24zUf74s7HpPN1oMV-1JxN2O2I",成功上传视频缩略图后返回的secret
        /// </summary>
        public string thumb_secret { get; set; }
    }

    /// <summary>
    /// 透传消息
    /// </summary>
    public class MsgAction : BaseMsg
    {
        public MsgAction()
        {
            type = "cmd";
        }

        /// <summary>
        /// 内容，可以指定json字符串
        /// </summary>
        public string action { get; set; }
    }
}
