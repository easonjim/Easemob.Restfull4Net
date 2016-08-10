using System;
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
    /// POST请求处理
    /// </summary>
    public static class Post
    {
        #region 同步方法

        #region 上传文件

        /// <summary>
        /// 发起Post请求，可上传文件
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="refererUrl">RefererUrl</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="timeOut">默认null为10秒</param>
        /// <param name="fileDictionary">字典文件流，需要上传的文件，Key：对应要上传的Name，Value：本地文件名</param>
        /// <param name="postDataDictionary">字典参数数据流</param>
        /// <param name="maxJsonLength">允许最大JSON长度</param>
        /// <param name="isDebug">是否为debug，默认false</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="accept">Accept</param>
        public static T PostGetJson<T>(string url, CookieContainer cookieContainer = null, Dictionary<string, object> postDataDictionary = null, Dictionary<string, string> fileDictionary = null, string refererUrl = null, Encoding encoding = null, int? timeOut = null, int? maxJsonLength = null, bool isDebug = false, ContentType? contentType = null, Dictionary<string, string> headerDictionary = null, ContentType? accept = null) where T : BaseResponse, new()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    postDataDictionary.FillFormDataStream(ms); //填充formData
                    string returnText = RequestUtility.HttpPost(url, cookieContainer, ms, fileDictionary, refererUrl, encoding, timeOut, contentType, headerDictionary,HttpMethod.Post,accept);
                    LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", postDataDictionary.GetQueryJsonString(), returnText, 200), url, isDebug: isDebug);//记录日志
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
                        LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", postDataDictionary.GetQueryJsonString(), returnText, (int)httpWebResponse.StatusCode), url, isDebug: isDebug);//记录日志
                        return result;
                    }
                    else
                    {
                        T model = new T();
                        ((BaseResponse)model).StatusCode = HttpStatusCode.InternalServerError;
                        LogTraceHelper.SendLog(string.Format("Method:DELETE\r\n\tParames:{0}\r\n\tMessage:{1}\r\n\tStackTrace:{2}\r\n\tStatusCode:{3}", postDataDictionary.GetQueryJsonString(), ex.Message, ex.StackTrace, 0), url, isDebug: isDebug); //记录日志
                        return model;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 发起Post请求，可上传文件
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="postStream">参数数据流</param>
        /// <param name="fileDictionary">字典文件流，需要上传的文件，Key：对应要上传的Name，Value：本地文件名</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="timeOut">默认null为10秒</param>
        /// <param name="refererUrl">RefererUrl</param>
        /// <param name="maxJsonLength">允许最大JSON长度</param>
        /// <param name="isDebug">是否为debug，默认false</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="headerDictionary">HeaderDictionary</param>
        /// <param name="accept">Accept</param>
        public static T PostGetJson<T>(string url, CookieContainer cookieContainer = null, Stream postStream = null, Dictionary<string, string> fileDictionary = null, string refererUrl = null, Encoding encoding = null, int? timeOut = null, int? maxJsonLength = null, bool isDebug = false, ContentType? contentType = null, Dictionary<string, string> headerDictionary = null, ContentType? accept = null) where T : BaseResponse, new()
        {
            try
            {
                string returnText = RequestUtility.HttpPost(url, cookieContainer, postStream, fileDictionary, refererUrl, encoding, timeOut, contentType, headerDictionary,HttpMethod.Post,accept);
                LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", postStream.GetQueryJsonString(), returnText, 200), url, isDebug: isDebug);//记录日志
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
                    LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", postStream.GetQueryJsonString(), returnText, (int)httpWebResponse.StatusCode), url, isDebug: isDebug);//记录日志
                    return result;
                }
                else
                {
                    T model = new T();
                    ((BaseResponse)model).StatusCode = HttpStatusCode.InternalServerError;
                    LogTraceHelper.SendLog(string.Format("Method:DELETE\r\n\tParames:{0}\r\n\tMessage:{1}\r\n\tStackTrace:{2}\r\n\tStatusCode:{3}", postStream.GetQueryJsonString(), ex.Message, ex.StackTrace, 0), url, isDebug: isDebug); //记录日志
                    return model;
                }
            }
            return null;
        }
        
        #endregion

        #region 常规请求

        /// <summary>
        /// 发送Post请求，常规请求
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="formData">参数</param>
        /// <param name="refererUrl">RefererUrl</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="timeOut">默认null为10秒</param>
        /// <param name="maxJsonLength">允许最大JSON长度</param>
        /// <param name="isDebug">是否为debug，默认false</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="accept">Accept</param>
        public static T PostGetJson<T>(string url, CookieContainer cookieContainer = null, Dictionary<string, object> formData = null, string refererUrl = null, Encoding encoding = null, int? timeOut = null, int? maxJsonLength = null, bool isDebug = false, ContentType? contentType = null, Dictionary<string, string> headerDictionary = null, ContentType? accept = null) where T : BaseResponse, new()
        {
            try
            {
                string returnText = RequestUtility.HttpPost(url, cookieContainer, formData,refererUrl,encoding,timeOut,contentType,headerDictionary,HttpMethod.Post,accept);
                LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", formData.GetQueryJsonString(), returnText, 200), url, isDebug: isDebug);//记录日志
                var result = SerializerUtility.GetResult<T>(returnText,maxJsonLength:maxJsonLength);
                return result;
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    var returnText = new StreamReader(httpWebResponse.GetResponseStream()).ReadLine();
                    var result = SerializerUtility.GetResult<T>(null, returnText, httpWebResponse.StatusCode, maxJsonLength);
                    LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", formData.GetQueryJsonString(), returnText, (int)httpWebResponse.StatusCode), url, isDebug: isDebug);//记录日志
                    return result;
                }
                else
                {
                    T model = new T();
                    ((BaseResponse)model).StatusCode = HttpStatusCode.InternalServerError;
                    LogTraceHelper.SendLog(string.Format("Method:DELETE\r\n\tParames:{0}\r\n\tMessage:{1}\r\n\tStackTrace:{2}\r\n\tStatusCode:{3}", formData.GetQueryJsonString(), ex.Message, ex.StackTrace, 0), url, isDebug: isDebug); //记录日志
                    return model;
                }
            }
            return null;
        }

        /// <summary>
        /// 发送Post请求，常规请求
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <typeparam name="T2">formData的数据类型</typeparam>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="formData">参数</param>
        /// <param name="refererUrl">RefererUrl</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="timeOut">默认null为10秒</param>
        /// <param name="maxJsonLength">允许最大JSON长度</param>
        /// <param name="isDebug">是否为debug，默认false</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="accept">Accept</param>
        public static T PostGetJson<T, T2>(string url, CookieContainer cookieContainer = null, IList<T2> formData = null, string refererUrl = null, Encoding encoding = null, int? timeOut = null, int? maxJsonLength = null, bool isDebug = false, ContentType? contentType = null, Dictionary<string, string> headerDictionary = null, ContentType? accept = null) where T : BaseResponse, new()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    formData.FillFormDataStream(ms); //填充formData
                    string returnText = RequestUtility.HttpPost(url, cookieContainer, ms, null, refererUrl, encoding, timeOut, contentType, headerDictionary,HttpMethod.Post,accept);
                    LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", formData.GetQueryJsonString(), returnText, 200), url, isDebug: isDebug); //记录日志
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
                        LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", formData.GetQueryJsonString(), returnText, (int)httpWebResponse.StatusCode), url, isDebug: isDebug); //记录日志
                        return result;
                    }
                    else
                    {
                        T model = new T();
                        ((BaseResponse)model).StatusCode = HttpStatusCode.InternalServerError;
                        LogTraceHelper.SendLog(string.Format("Method:DELETE\r\n\tParames:{0}\r\n\tMessage:{1}\r\n\tStackTrace:{2}\r\n\tStatusCode:{3}", formData.GetQueryJsonString(), ex.Message, ex.StackTrace, 0), url, isDebug: isDebug); //记录日志
                        return model;
                    }
                }
                return null;
            }
        }

        #endregion

        #region 下载文件

        /// <summary>
        /// 使用Post方法上传数据并下载文件或结果
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="data">json数据</param>
        /// <param name="stream">填充的Stream</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="isDebug">是否为debug，默认false</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="accept">Accept</param>
        public static void FillDownload(string url, string data, Stream stream, ContentType? contentType = null, bool isDebug = false, Dictionary<string, string> headerDictionary = null, ContentType? accept = null)
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
                var file = wc.UploadData(url, "POST", Encoding.UTF8.GetBytes(string.IsNullOrEmpty(data) ? "" : data));
                foreach (var b in file)
                {
                    stream.WriteByte(b);
                }
                LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", data, "", 200), url, isDebug: isDebug); //记录日志
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    var returnText = new StreamReader(httpWebResponse.GetResponseStream()).ReadLine();
                    LogTraceHelper.SendLog(string.Format("Method:POST\r\n\tParames:{0}\r\n\tReturnText:{1}\r\n\tStatusCode:{2}", data, returnText, (int)httpWebResponse.StatusCode), url, isDebug: isDebug); //记录日志
                }
                else
                {
                    LogTraceHelper.SendLog(string.Format("Method:DELETE\r\n\tParames:{0}\r\n\tMessage:{1}\r\n\tStackTrace:{2}\r\n\tStatusCode:{3}", data, ex.Message, ex.StackTrace, 0), url, isDebug: isDebug); //记录日志
                }
            }
            
        }

        #endregion

        #endregion
    }
}
