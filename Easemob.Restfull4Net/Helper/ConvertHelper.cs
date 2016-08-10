using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace Easemob.Restfull4Net.Helper
{
    public static class ConvertHelper
    {
        #region SafeParse

        public static bool SafeBool(this object target, bool defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeBool(tmp, defaultValue);
        }
        public static bool SafeBool(this string text, bool defaultValue)
        {
            bool flag;
            if (bool.TryParse(text, out flag))
            {
                defaultValue = flag;
            }
            return defaultValue;
        }

        public static DateTime SafeDateTime(this object target, DateTime defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeDateTime(tmp, defaultValue);
        }
        public static DateTime SafeDateTime(this string text, DateTime defaultValue)
        {
            DateTime time;
            if (DateTime.TryParse(text, out time))
            {
                defaultValue = time;
            }
            return defaultValue;
        }

        public static decimal SafeDecimal(this object target, decimal defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeDecimal(tmp, defaultValue);
        }
        public static decimal SafeDecimal(this string text, decimal defaultValue)
        {
            decimal num;
            if (decimal.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static short SafeShort(this object target, short defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeShort(tmp, defaultValue);
        }
        public static short SafeShort(this string text, short defaultValue)
        {
            short num;
            if (short.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static int SafeInt(this object target, int defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeInt(tmp, defaultValue);
        }
        public static int SafeInt(this string text, int defaultValue)
        {
            int num;
            if (int.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }
        public static int[] SafeInt(this string[] text, int defaultValue)
        {
            if (text == null || text.Length < 1) return new[] { defaultValue };

            int[] nums = new int[text.Length];
            int tmp;
            for (int i = 0; i < text.Length; i++)
            {
                if (int.TryParse(text[i], out tmp)) nums[i] = tmp;
                else nums[i] = defaultValue;
            }
            return nums;
        }
        public static string SafeIntFilter(this string text, int defaultValue, char split = ',')
        {
            if (string.IsNullOrWhiteSpace(text)) return defaultValue.ToString(CultureInfo.InvariantCulture);
            string[] tmpSplit = text.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
            if (tmpSplit.Length < 1) return defaultValue.ToString(CultureInfo.InvariantCulture);

            int tmp;
            for (int i = 0; i < tmpSplit.Length; i++)
            {
                if (int.TryParse(tmpSplit[i], out tmp))
                    tmpSplit[i] = tmp.ToString(CultureInfo.InvariantCulture);
                else
                    tmpSplit[i] = defaultValue.ToString(CultureInfo.InvariantCulture);
            }
            return string.Join(split.ToString(CultureInfo.InvariantCulture), tmpSplit);
        }

        public static long SafeLong(this object target, long defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString(); if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeLong(tmp, defaultValue);
        }
        public static long SafeLong(string text, long defaultValue)
        {
            long num;
            if (long.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static long[] SafeLong(this string[] text, long defaultValue)
        {
            if (text == null || text.Length < 1) return new[] { defaultValue };

            long[] nums = new long[text.Length];
            long tmp;
            for (int i = 0; i < text.Length; i++)
            {
                if (long.TryParse(text[i], out tmp)) nums[i] = tmp;
                else nums[i] = defaultValue;
            }
            return nums;
        }

        public static string SafeLongFilter(this string text, long defaultValue, char split = ',')
        {
            if (string.IsNullOrWhiteSpace(text)) return defaultValue.ToString(CultureInfo.InvariantCulture);
            string[] tmpSplit = text.Split(new[] { split }, StringSplitOptions.RemoveEmptyEntries);
            if (tmpSplit.Length < 1) return defaultValue.ToString(CultureInfo.InvariantCulture);

            long tmp;
            for (int i = 0; i < tmpSplit.Length; i++)
            {
                if (long.TryParse(tmpSplit[i], out tmp))
                    tmpSplit[i] = tmp.ToString(CultureInfo.InvariantCulture);
                else
                    tmpSplit[i] = defaultValue.ToString(CultureInfo.InvariantCulture);
            }
            return string.Join(split.ToString(CultureInfo.InvariantCulture), tmpSplit);
        }

        public static string SafeString(this object target, string defaultValue)
        {
            if (null != target && "" != target.ToString())
            {
                return target.ToString();
            }
            return defaultValue;
        }

        #region SafeNullParse
        public static bool? SafeBool(this object target, bool? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeBool(tmp, defaultValue);
        }
        public static bool? SafeBool(this string text, bool? defaultValue)
        {
            bool flag;
            if (bool.TryParse(text, out flag))
            {
                defaultValue = flag;
            }
            return defaultValue;
        }

        public static DateTime? SafeDateTime(this object target, DateTime? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeDateTime(tmp, defaultValue);
        }
        public static DateTime? SafeDateTime(this string text, DateTime? defaultValue)
        {
            DateTime time;
            if (DateTime.TryParse(text, out time))
            {
                defaultValue = time;
            }
            return defaultValue;
        }

        public static decimal? SafeDecimal(this object target, decimal? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeDecimal(tmp, defaultValue);
        }
        public static decimal? SafeDecimal(this string text, decimal? defaultValue)
        {
            decimal num;
            if (decimal.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static short? SafeShort(this object target, short? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeShort(tmp, defaultValue);
        }
        public static short? SafeShort(this string text, short? defaultValue)
        {
            short num;
            if (short.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static int? SafeInt(this object target, int? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeInt(tmp, defaultValue);
        }
        public static int? SafeInt(this string text, int? defaultValue)
        {
            int num;
            if (int.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }

        public static long? SafeLong(this object target, long? defaultValue)
        {
            if (target == null) return defaultValue;
            string tmp = target.ToString();
            if (string.IsNullOrWhiteSpace(tmp)) return defaultValue;
            return SafeLong(tmp, defaultValue);
        }
        public static long? SafeLong(this string text, long? defaultValue)
        {
            long num;
            if (long.TryParse(text, out num))
            {
                defaultValue = num;
            }
            return defaultValue;
        }
        #endregion

        #region SafeEnum
        /// <summary>
        /// 将枚举数值or枚举名称 安全转换为枚举对象
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">数值or名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <remarks>转换区分大小写</remarks>
        /// <returns></returns>
        public static T SafeEnum<T>(this string value, T defaultValue) where T : struct
        {
            return SafeEnum<T>(value, defaultValue, false);
        }

        /// <summary>
        /// 将枚举数值or枚举名称 安全转换为枚举对象
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">数值or名称</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="ignoreCase">是否忽略大小写 true 不区分大小写 | false 区分大小写</param>
        /// <returns></returns>
        public static T SafeEnum<T>(this string value, T defaultValue, bool ignoreCase) where T : struct
        {
            T result;
            if (Enum.TryParse<T>(value, ignoreCase, out result))
            {
                if (Enum.IsDefined(typeof(T), result))
                {
                    defaultValue = result;
                }
            }
            return defaultValue;
        }
        #endregion
        
        #endregion

        #region 字符串
        /// <summary>
        /// 检查字符串是否是 null 或者空白字符,不同于.net自带的string.IsNullOrEmpty，多个空格在这里也返回true。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string target)
        {
            if (null != target && "" != target.ToString())
            {
                return target.ToString().Trim().Length == 0;
            }
            return true;
        }
        #endregion

        #region Html

        /// <summary>
        /// 组装QueryString的方法
        /// 参数之间用&连接，首位没有符号，如：a=1&b=2&c=3
        /// </summary>
        public static string GetQueryString(this Dictionary<string, object> formData)
        {
            if (formData == null || formData.Count == 0)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();

            var i = 0;
            foreach (var kv in formData)
            {
                i++;
                sb.AppendFormat("{0}={1}", kv.Key, kv.Value);
                if (i < formData.Count)
                {
                    sb.Append("&");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 组装QueryJsonString的方法
        /// JSON形式的提交参数
        /// </summary>
        public static string GetQueryJsonString(this Dictionary<string, object> formData)
        {
            return new JavaScriptSerializer().Serialize(formData);
        }

        /// <summary>
        /// 组装QueryJsonString的方法
        /// JSON形式的提交参数
        /// </summary>
        public static string GetQueryJsonString(this Stream formData)
        {
            return new JavaScriptSerializer().Serialize(formData);
        }

        /// <summary>
        /// 组装QueryJsonString的方法
        /// JSON形式的提交参数
        /// </summary>
        public static string GetQueryJsonString<T>(this IList<T> formData)
        {
            return new JavaScriptSerializer().Serialize(formData);
        }

        /// <summary>
        /// 填充表单信息的Stream
        /// </summary>
        public static void FillFormDataStream(this Dictionary<string, object> formData, Stream stream)
        {
            string dataString = GetQueryString(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }

        /// <summary>
        /// 填充表单信息的Stream，JSON的形式
        /// </summary>
        public static void FillFormDataStream<T>(this IList<T> formData, Stream stream)
        {
            string dataString = GetQueryJsonString<T>(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }

        /// <summary>
        /// 填充表单信息的Stream，JSON的形式
        /// </summary>
        public static void FillFormDataJsonStream(this Dictionary<string, object> formData, Stream stream)
        {
            string dataString = GetQueryJsonString(formData);
            var formDataBytes = formData == null ? new byte[0] : Encoding.UTF8.GetBytes(dataString);
            stream.Write(formDataBytes, 0, formDataBytes.Length);
            stream.Seek(0, SeekOrigin.Begin);//设置指针读取位置
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.HtmlEncode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string html)
        {
            return System.Web.HttpUtility.HtmlEncode(html);
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.HtmlDecode
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string html)
        {
            return System.Web.HttpUtility.HtmlDecode(html);
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.UrlEncode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlEncode(this string url)
        {
            return System.Web.HttpUtility.UrlEncode(url);
        }

        /// <summary>
        /// 封装System.Web.HttpUtility.UrlDecode
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UrlDecode(this string url)
        {
            return System.Web.HttpUtility.UrlDecode(url);
        }

        /// <summary>
        /// <para>将 URL 中的参数名称/值编码为合法的格式。</para>
        /// <para>可以解决类似这样的问题：假设参数名为 tvshow, 参数值为 Tom&Jerry，如果不编码，可能得到的网址： http://a.com/?tvshow=Tom&Jerry&year=1965 编码后则为：http://a.com/?tvshow=Tom%26Jerry&year=1965 </para>
        /// <para>实践中经常导致问题的字符有：'&', '?', '=' 等</para>
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string AsUrlData(this string data)
        {
            return Uri.EscapeDataString(data);
        }

        #endregion
    }
}
