using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Easemob.Restfull4Net.Config
{
    /// <summary>
    /// 默认配置文件，可直接实例
    /// </summary>
    public partial class ServerConfig : IServerConfig
    {
        /// <summary>
        /// 默认环信服务器地址https://a1.easemob.com/
        /// </summary>
        public const string DefaultServerUrl = "https://a1.easemob.com/";

        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public const int DefaultHttpTimeOut = 10000;
        
        /// <summary>
        /// 默认为调试模式，直接输出日志
        /// </summary>
        public const bool DefaultIsDebug = true;

        /// <summary>
        /// 默认JSON转化为最大值
        /// </summary>
        public const int DefaultMaxJsonLength = int.MaxValue;

        public ServerConfig()
        {
            this.ServerUrl = DefaultServerUrl;
            this.HttpTimeOut = DefaultHttpTimeOut;
            this.IsDebug = DefaultIsDebug;
            this.MaxJsonLength = DefaultHttpTimeOut;
        }

        /// <summary>
        /// 环信服务器地址
        /// 如：https://a1.easemob.com/
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// 组织名
        /// 对应#前面部分
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// 应用名
        /// 对应#后面部分
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 请求超时设置（以毫秒为单位）
        /// </summary>
        public int HttpTimeOut { get; set; }

        /// <summary>
        /// 是否为调试模式
        /// 说明：如果为调试模式，将在程序主目录输出日志文件
        /// </summary>
        public bool IsDebug { get; set; }

        /// <summary>
        /// JavaScriptSerializer类接受的JSON字符串的最大长度
        /// </summary>
        public int MaxJsonLength { get; set; }
    }
}