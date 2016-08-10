using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using Easemob.Restfull4Net.Config;
using Easemob.Restfull4Net.Config.Configuration;
using Easemob.Restfull4Net.Helper;
using Easemob.Restfull4Net.Request;

namespace Easemob.Restfull4Net
{
    /// <summary>
    /// 请求客户端
    /// 说明：实现快速创建实例
    /// </summary>
    public static class Client
    {
        /// <summary>
        /// 配置节点集合，EasemobServer和Custom的集合
        /// 说明，key为appname
        /// </summary>
        public static readonly Dictionary<string,IServerConfig> ServerConfigs = new Dictionary<string, IServerConfig>();

        /// <summary>
        /// 多个请求客户端的同步请求
        /// 说明：key为appname
        /// </summary>
        public static readonly Dictionary<string,SyncRequest> SyncRequests = new Dictionary<string, SyncRequest>();

        /// <summary>
        /// 默认读取第一个配置的SyncRequest
        /// 说明：读取顺序EasemobServer->Custom
        /// </summary>
        public static SyncRequest DefaultSyncRequest = null;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Client()
        {
            FillEasemobServerConfigSection();
            FillCustomerConfig();
            InstantiateSyncRequest();
        }

        #region 填充配置节点

        /// <summary>
        /// 填充EasemobServer配置节点
        /// </summary>
        private static void FillEasemobServerConfigSection()
        {
            try
            {
                ServerConfigSection serverConfigSection = ConfigurationManager.GetSection("EasemobServer") as ServerConfigSection;
                if (serverConfigSection != null)
                {
                    foreach (ServerConfigElement item in serverConfigSection.ServerConfigElementCollection)
                    {
                        if (!ServerConfigs.ContainsKey(item.AppName))
                        {
                            ServerConfigs.Add(item.AppName, item);
                        }
                    }
                }
                else
                {
                    throw new AggregateException("EasemobServer节点读取失败，请检查配置！");
                }
            }
            catch (Exception ex)
            {
                LogTraceHelper.SendLog(string.Format("Message:{0}\r\n\tStackTrace:{1}", ex.Message, ex.StackTrace), "FillEasemobServerConfigSection",isDebug:true);
            }
            
        }

        /// <summary>
        /// 填充Custom自定义配置节点
        /// </summary>
        private static void FillCustomerConfig()
        {
            try
            {
                var customServerConfig = new CustomServerConfig();
                if (!customServerConfig.AppName.IsNullOrEmpty())
                {
                    if (!ServerConfigs.ContainsKey(customServerConfig.AppName))
                    {
                        ServerConfigs.Add(customServerConfig.AppName, customServerConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                LogTraceHelper.SendLog(string.Format("Message:{0}\r\n\tStackTrace:{1}", ex.Message, ex.StackTrace), "FillCustomerConfig", isDebug: true);
            }
        }
        
        #endregion

        #region 实例化SyncRequest

        /// <summary>
        /// 实例化SyncRequest
        /// </summary>
        private static void InstantiateSyncRequest()
        {
            try
            {
                if (ServerConfigs.Count > 0)
                {
                    foreach (var serverConfig in ServerConfigs)
                    {
                        SyncRequests.Add(serverConfig.Key, new SyncRequest(serverConfig.Value));
                    }
                }
                if (SyncRequests.Count > 0)
                {
                    DefaultSyncRequest = SyncRequests.FirstOrDefault().Value;
                }
            }
            catch (Exception ex)
            {
                LogTraceHelper.SendLog(string.Format("Message:{0}\r\n\tStackTrace:{1}", ex.Message, ex.StackTrace), "InstantiateSyncRequest",isDebug:true);
            }
        }

        #endregion
    }
}