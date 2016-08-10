using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Easemob.Restfull4Net.Common;
using Easemob.Restfull4Net.Entity.Response;
using Easemob.Restfull4Net.Helper;

namespace Easemob.Restfull4Net.Utility.HttpUtility
{
    /// <summary>
    /// Get请求处理
    /// </summary>
    public static class Get
    {
        #region 同步方法

        #region 常规请求

        /// <summary>
        /// 发起Get请求，（简单请求）
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">地址</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="maxJsonLength">允许最大JSON长度</param>
        /// <param name="isDebug">是否为debug，默认false</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="accept">Accept</param>
        public static T GetJson<T>(string url, Encoding encoding = null, int? maxJsonLength = null, bool isDebug = false, Dictionary<string, string> headerDictionary = null, ContentType? contentType = null, ContentType? accept = null) where T : BaseResponse, new()
        {
            try
            {
                string returnText = RequestUtility.HttpGet(url, encoding, headerDictionary,contentType,accept);
                LogTraceHelper.SendLog(string.Format("Method:GET\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", "", returnText, 200), url, isDebug: isDebug);//记录日志
                var result = SerializerUtility.GetResult<T>(returnText, maxJsonLength: maxJsonLength);
                return result;
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    var returnText = new StreamReader(httpWebResponse.GetResponseStream()).ReadLine();
                    var result = SerializerUtility.GetResult<T>(null, returnText, httpWebResponse.StatusCode, maxJsonLength);
                    LogTraceHelper.SendLog(string.Format("Method:GET\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", "", returnText, (int)httpWebResponse.StatusCode), url, isDebug: isDebug);//记录日志
                    return result;
                }
                else
                {
                    T model = new T();
                    ((BaseResponse)model).StatusCode = HttpStatusCode.InternalServerError;
                    LogTraceHelper.SendLog(string.Format("Method:DELETE\r\n\tParames:{0}\r\n\tMessage:{1}\r\n\tStackTrace:{2}\r\n\tStatusCode:{3}", "", ex.Message, ex.StackTrace, 0), url, isDebug: isDebug); //记录日志
                    return model;
                }
            }
            return null;
        }

        /// <summary>
        /// 发起Get请求，（加入cookie）
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="refererUrl">RefererUrl</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="timeOut">默认null为10秒</param>
        /// <param name="postDataDictionary">字典参数数据流</param>
        /// <param name="maxJsonLength">允许最大JSON长度</param>
        /// <param name="isDebug">是否为debug，默认false</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="accept">Accept</param>
        public static T GetJson<T>(string url, CookieContainer cookieContainer = null, Dictionary<string, object> postDataDictionary = null, string refererUrl = null, Encoding encoding = null, int? timeOut = null, int? maxJsonLength = null, bool isDebug = false, Dictionary<string, string> headerDictionary = null, ContentType? contentType = null, ContentType? accept = null) where T : BaseResponse, new()
        {
            try
            {
                string returnText = RequestUtility.HttpGet(url, cookieContainer, postDataDictionary, refererUrl, encoding, timeOut, headerDictionary,contentType,accept);
                LogTraceHelper.SendLog(string.Format("Method:GET\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", postDataDictionary.GetQueryJsonString(), returnText, 200), url, isDebug: isDebug);//记录日志
                var result = SerializerUtility.GetResult<T>(returnText, maxJsonLength: maxJsonLength);
                return result;
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    var returnText = new StreamReader(httpWebResponse.GetResponseStream()).ReadLine();
                    var result = SerializerUtility.GetResult<T>(null, returnText, httpWebResponse.StatusCode, maxJsonLength);
                    LogTraceHelper.SendLog(string.Format("Method:GET\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", postDataDictionary.GetQueryJsonString(), returnText, (int)httpWebResponse.StatusCode), url, isDebug: isDebug);//记录日志
                    return result;
                }
                else
                {
                    T model = new T();
                    ((BaseResponse)model).StatusCode = HttpStatusCode.InternalServerError;
                    LogTraceHelper.SendLog(string.Format("Method:DELETE\r\n\tParames:{0}\r\n\tMessage:{1}\r\n\tStackTrace:{2}\r\n\tStatusCode:{3}","", ex.Message, ex.StackTrace, 0), url, isDebug: isDebug); //记录日志
                    return model;
                }
            }
            return null;
        }
        
        #endregion

        #region 下载文件

        /// <summary>
        /// 从url下载
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="stream">填充的Stream</param>
        /// <param name="isDebug">是否为debug，默认false</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="accept">Accept</param>
        public static void FillDownload(string url, Stream stream, bool isDebug = false, Dictionary<string, string> headerDictionary = null, ContentType? contentType = null, ContentType? accept = null)
        {
            try
            {
                WebClient wc = new WebClient();
                if (contentType != null)
                {
                    wc.Headers[HttpRequestHeader.ContentType] = contentType.Value.ToValue();
                }

                if (accept != null)
                {
                    wc.Headers[HttpRequestHeader.Accept] = accept.Value.ToValue();
                }
                if (headerDictionary != null)//自定义头增加
                {
                    foreach (var header in headerDictionary)
                    {
                        switch (header.Key.ToLower())
                        {
                            case "accept":
                                wc.Headers[HttpRequestHeader.Accept] = header.Value;
                                break;
                            case "contenttype":
                                wc.Headers[HttpRequestHeader.ContentType] = header.Value;
                                break;
                            default:
                                wc.Headers.Add(header.Key, header.Value);
                                break;
                        }

                    }
                }
                var data = wc.DownloadData(url);
                foreach (var b in data)
                {
                    stream.WriteByte(b);
                }
                LogTraceHelper.SendLog(string.Format("Method:GET\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", data, "", 200), url, isDebug: isDebug); //记录日志
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    var returnText = new StreamReader(httpWebResponse.GetResponseStream()).ReadLine();
                    LogTraceHelper.SendLog(string.Format("Method:GET\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", "", returnText, (int)httpWebResponse.StatusCode), url, isDebug: isDebug); //记录日志
                }
                else
                {
                    LogTraceHelper.SendLog(string.Format("Method:DELETE\r\n\tParames:{0}\r\n\tMessage:{1}\r\n\tStackTrace:{2}\r\n\tStatusCode:{3}", "", ex.Message, ex.StackTrace, 0), url, isDebug: isDebug); //记录日志
                }
            }
        } 

        #endregion

        #endregion
    }
}
