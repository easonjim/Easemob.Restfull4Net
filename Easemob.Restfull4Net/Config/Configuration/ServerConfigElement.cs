using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Config.Configuration
{
    public class ServerConfigElement : ConfigurationElement,IServerConfig
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

        /// <summary>
        /// 环信服务器地址
        /// 如：https://a1.easemob.com/
        /// </summary>
        [ConfigurationProperty("serverUrl", DefaultValue = DefaultServerUrl)]
        public string ServerUrl { get { return (string)this["serverUrl"]; } }

        /// <summary>
        /// 组织名
        /// 对应#前面部分
        /// </summary>
        [ConfigurationProperty("orgName",IsRequired = true)]
        public string OrgName { get { return (string)this["orgName"]; } }

        /// <summary>
        /// 应用名
        /// 对应#后面部分
        /// </summary>
        [ConfigurationProperty("appName",IsRequired = true)]
        public string AppName { get { return (string)this["appName"]; } }

        /// <summary>
        /// 客户端ID
        /// </summary>
        [ConfigurationProperty("clientId",IsRequired = true)]
        public string ClientId { get { return (string)this["clientId"]; } }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        [ConfigurationProperty("clientSecret",IsRequired = true)]
        public string ClientSecret { get { return (string)this["clientSecret"]; } }

        /// <summary>
        /// 请求超时设置（以毫秒为单位）
        /// </summary>
        [ConfigurationProperty("httpTimeOut", DefaultValue = DefaultHttpTimeOut)]
        public int HttpTimeOut { get { return (int)this["httpTimeOut"]; } }

        /// <summary>
        /// 是否为调试模式
        /// 说明：如果为调试模式，将在程序主目录输出日志文件
        /// </summary>
        [ConfigurationProperty("isDebug", DefaultValue = DefaultIsDebug)]
        public bool IsDebug { get { return (bool)this["isDebug"]; } }

        /// <summary>
        /// JavaScriptSerializer类接受的JSON字符串的最大长度
        /// </summary>
        [ConfigurationProperty("maxJsonLength",DefaultValue = DefaultMaxJsonLength)]
        public int MaxJsonLength { get { return (int)this["maxJsonLength"]; } }
    }
}
