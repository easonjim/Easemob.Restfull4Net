 using System;

namespace Easemob.Restfull4Net.Helper
{
    /// <summary>
    /// web.config������
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// �õ�AppSettings�е������ַ�����Ϣ
        /// ����web.config��app.config
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
                    if ( rootWebConfig.AppSettings.Settings.Count > 0 && rootWebConfig.AppSettings.Settings[key] != null)//�Ȼ�ȡweb.config
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
            if (string.IsNullOrEmpty(retObj))//���û�У����Ի�ȡapp.config
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
        /// �õ�AppSettings�е�����Bool��Ϣ
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
        /// �õ�AppSettings�е�����Decimal��Ϣ
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
        /// �õ�AppSettings�е�����int��Ϣ
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
