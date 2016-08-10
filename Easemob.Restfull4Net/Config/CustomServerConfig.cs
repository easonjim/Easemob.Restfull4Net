using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Easemob.Restfull4Net.Helper;

namespace Easemob.Restfull4Net.Config
{
    /// <summary>
    /// 自定义配置文件，可直接实例
    /// 说明：后续可参照此写法自定义实现
    /// 备注：此节点也可实现多个server，每个值使用|进行切割
    /// 在appsettings节点下添加自定义key
    /// </summary>
    /*
     * <add key="HX_EaseServerUrl" value=""/>
     * <add key="HX_EaseAppClientID" value=""/>
     * <add key="HX_EaseAppClientSecret" value=""/>
     * <add key="HX_EaseAppName" value=""/>
     * <add key="HX_EaseAppOrgName" value=""/>
     * <add key="HX_EaseHttpTimeOut" value=""/>
     * <add key="HX_EaseIsDebug" value=""/>
     * <add key="HX_EaseMaxJsonLength" value=""/>
     */
    public partial class CustomServerConfig:IServerConfig
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

        public CustomServerConfig()
        {
            this.ServerUrl = ConfigHelper.GetConfigString("HX_EaseServerUrl")?? DefaultServerUrl;

            this.HttpTimeOut = ConfigHelper.GetConfigInt("HX_EaseHttpTimeOut");
            if (this.HttpTimeOut == 0) this.HttpTimeOut = DefaultHttpTimeOut;

            this.IsDebug = ConfigHelper.GetConfigBool("HX_EaseIsDebug");

            this.MaxJsonLength = ConfigHelper.GetConfigInt("HX_EaseMaxJsonLength");
            if (this.MaxJsonLength == 0) this.MaxJsonLength = DefaultHttpTimeOut;

            this.OrgName = ConfigHelper.GetConfigString("HX_EaseAppOrgName");
            this.AppName = ConfigHelper.GetConfigString("HX_EaseAppName");
            this.ClientId = ConfigHelper.GetConfigString("HX_EaseAppClientID");
            this.ClientSecret = ConfigHelper.GetConfigString("HX_EaseAppClientSecret");
        }

        /// <summary>
        /// 环信服务器地址
        /// 如：https://a1.easemob.com/
        /// </summary>
        public string ServerUrl { get; private set; }

        /// <summary>
        /// 组织名
        /// 对应#前面部分
        /// </summary>
        public string OrgName { get; private set; }

        /// <summary>
        /// 应用名
        /// 对应#后面部分
        /// </summary>
        public string AppName { get; private set; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        public string ClientSecret { get; private set; }

        /// <summary>
        /// 请求超时设置（以毫秒为单位）
        /// </summary>
        public int HttpTimeOut { get; private set; }

        /// <summary>
        /// 是否为调试模式
        /// 说明：如果为调试模式，将在程序主目录输出日志文件
        /// </summary>
        public bool IsDebug { get; private set; }

        /// <summary>
        /// JavaScriptSerializer类接受的JSON字符串的最大长度
        /// </summary>
        public int MaxJsonLength { get; private set; }
    }
}