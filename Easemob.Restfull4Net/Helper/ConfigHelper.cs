 using System;

namespace Easemob.Restfull4Net.Helper
{
    /// <summary>
    /// web.config操作类
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// 兼容web.config和app.config
        /// </summary>
        public static string GetConfigString(string key)
        {
            string cacheKey = "AppSettings-" + key;
            object objModel = CacheHelper.Get(cacheKey);
            if (objModel == null)
            {
                try
                {
                    System.Configuration.Configuration rootWebConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
                    if ( rootWebConfig.AppSettings.Settings.Count > 0 && rootWebConfig.AppSettings.Settings[key] != null)//先获取web.config
                    {
                        objModel = rootWebConfig.AppSettings.Settings[key].Value;
                        CacheHelper.Insert(cacheKey, objModel, DateTime.Now.AddMinutes(180).Second);
                    }
                }
                catch
                {
                    // exceptions.
                }
            }
            string retObj = objModel != null ? objModel.ToString() : null;
            if (string.IsNullOrEmpty(retObj))//如果没有，尝试获取app.config
            {
                try
                {
                    objModel = System.Configuration.ConfigurationManager.AppSettings[key];
                    if (objModel != null)
                    {
                        CacheHelper.Insert(cacheKey, objModel, DateTime.Now.AddMinutes(180).Second);
                    }
                }
                catch
                {
                    // exceptions.
                }
            }
            return objModel != null ? objModel.ToString() : null;
        }

        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        public static bool GetConfigBool(string key)
        {
            bool result = false;
            string cfgVal = GetConfigString(key);
            if (!string.IsNullOrWhiteSpace(cfgVal))
            {
                try
                {
                    result = bool.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    //exceptions.
                }
            }
            return result;
        }

        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        public static decimal GetConfigDecimal(string key)
        {
            decimal result = 0;
            string cfgVal = GetConfigString(key);
            if (!string.IsNullOrWhiteSpace(cfgVal))
            {
                try
                {
                    result = decimal.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    //exceptions.
                }
            }

            return result;
        }

        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        public static int GetConfigInt(string key)
        {
            int result = 0;
            string cfgVal = GetConfigString(key);
            if (!string.IsNullOrWhiteSpace(cfgVal))
            {
                try
                {
                    result = int.Parse(cfgVal);
                }
                catch (FormatException)
                {
                    //exceptions.
                }
            }

            return result;
        }
    }
}
