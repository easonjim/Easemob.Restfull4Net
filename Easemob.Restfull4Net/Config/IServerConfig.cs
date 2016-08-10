using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Easemob.Restfull4Net.Config
{
    /// <summary>
    /// 配置文件接口
    /// 继承实现可选择配置：1实例、2配置section、3自定义配置config
    /// </summary>
    public partial interface IServerConfig
    {
        /// <summary>
        /// 环信服务器地址
        /// 如：https://a1.easemob.com/
        /// </summary>
        string ServerUrl { get; }

        /// <summary>
        /// 组织名
        /// 对应#前面部分
        /// </summary>
        string OrgName { get; }

        /// <summary>
        /// 应用名
        /// 对应#后面部分
        /// </summary>
        string AppName { get; }

        /// <summary>
        /// 客户端ID
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// 客户端密钥
        /// </summary>
        string ClientSecret { get; }

        /// <summary>
        /// 请求超时设置（以毫秒为单位）
        /// </summary>
        int HttpTimeOut { get; }

        /// <summary>
        /// 是否为调试模式
        /// 说明：如果为调试模式，将在程序主目录输出日志文件
        /// </summary>
        bool IsDebug { get; }

        /// <summary>
        /// JavaScriptSerializer类接受的JSON字符串的最大长度
        /// </summary>
        int MaxJsonLength { get; }
    }
}
