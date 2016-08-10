using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Easemob.Restfull4Net.Common;
using Easemob.Restfull4Net.Helper;

namespace Easemob.Restfull4Net.Utility.HttpUtility
{
    /// <summary>
    /// 请求工具
    /// </summary>
    public static class RequestUtility
    {
        #region 代理

        private static WebProxy _webproxy = null;

        /// <summary>
        /// 设置Web代理
        /// </summary>
        /// <param name="host">服务器地址</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="port">端口，默认null为80</param>
        public static void SetHttpProxy(string host, string username, string password, string port = null)
        {
            ICredentials cred = new NetworkCredential(username, password);
            if (!string.IsNullOrEmpty(host))
            {
                _webproxy = new WebProxy(host + ":" + port ?? "80", true, null, cred);
            }
        }

        /// <summary>
        /// 清除Web代理状态
        /// </summary>
        public static void RemoveHttpProxy()
        {
            _webproxy = null;
        }

        #endregion

        #region 同步方法

        #region Get

        /// <summary>
        /// 使用Get方法获取字符串结果（没有加入Cookie）(使用WebClient)
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="accept">Accept</param>
        public static string HttpGet(string url, Encoding encoding = null, Dictionary<string, string> headerDictionary = null,ContentType? contentType = null,ContentType? accept=null)
        {
            WebClient wc = new WebClient
            {
                Proxy = _webproxy,
                Encoding = encoding ?? Encoding.UTF8
            };
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
            return wc.DownloadString(url);
        }

        /// <summary>
        /// 使用Get方法获取字符串结果（加入Cookie）(使用HttpWebReqeust)
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="formData">默认null为使用url，如果此值不为null，将拼接url进行请求</param>
        /// <param name="refererUrl">RefererUrl</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="timeOut">默认null为10秒</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="accept">Accept</param>
        public static string HttpGet(string url, CookieContainer cookieContainer = null, Dictionary<string, object> formData = null, string refererUrl = null, Encoding encoding = null, int? timeOut = null, Dictionary<string, string> headerDictionary = null,ContentType? contentType=null,ContentType? accept = null)
        {
            if (formData!=null)
            {
                url = string.Concat(url, url.Contains("?") ? "" : "?", formData.GetQueryString());
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Timeout = timeOut ?? 10000;
            request.Proxy = _webproxy;

            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }

            if (!string.IsNullOrEmpty(refererUrl))
            {
                request.Referer = refererUrl;
            }

            if (contentType!=null)
            {
                request.ContentType = contentType.Value.ToValue();
            }

            if (accept!=null)
            {
                request.Accept = accept.Value.ToValue();
            }

            if (headerDictionary!=null)//自定义头增加
            {
                foreach (var header in headerDictionary)
                {
                    switch (header.Key.ToLower())
                    {
                        case "accept":
                            request.Accept = header.Value;
                            break;
                        case "contenttype":
                            request.ContentType = header.Value;
                            break;
                        default:
                            request.Headers.Add(header.Key,header.Value);
                            break;
                    }
                    
                }
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (cookieContainer != null)
            {
                response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            }
            using (Stream responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                    {
                        string retString = myStreamReader.ReadToEnd();
                        return retString;
                    }
                }
            }
            return null;
        }

        #endregion

        #region Post

        /// <summary>
        /// 使用Post方法获取字符串结果，常规提交
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="formData">参数</param>
        /// <param name="refererUrl">RefererUrl</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="timeOut">默认null为10秒</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="method">HtppMethod默认null为POST</param>
        /// <param name="accept">Accept</param>
        public static string HttpPost(string url, CookieContainer cookieContainer = null, Dictionary<string, object> formData = null, string refererUrl = null, Encoding encoding = null, int? timeOut = null, ContentType? contentType = null, Dictionary<string, string> headerDictionary = null, HttpMethod method = HttpMethod.Post,ContentType? accept=null)
        {
            MemoryStream ms = new MemoryStream();
            if (contentType!=null)
            {
                switch (contentType)
                {
                    case ContentType.JSON:
                    {
                        formData.FillFormDataJsonStream(ms);
                    }
                    break;
                    default:
                    {
                        formData.FillFormDataStream(ms);//填充formData
                    }
                    break;
                }
            }
            return HttpPost(url, cookieContainer, ms, null, refererUrl, encoding, timeOut,contentType,headerDictionary,method,accept);
        }

        /// <summary>
        /// 使用Post方法获取字符串结果，上传文件
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="cookieContainer">CookieContainer</param>
        /// <param name="postStream">表单参数数据流</param>
        /// <param name="fileDictionary">字典文件流，需要上传的文件，Key：对应要上传的Name，Value：本地文件名</param>
        /// <param name="encoding">默认null为UTF8</param>
        /// <param name="timeOut">默认null为10秒</param>
        /// <param name="refererUrl">RefererUrl</param>
        /// <param name="contentType">ContentType</param>
        /// <param name="headerDictionary">自定义请求头参数</param>
        /// <param name="method">HtppMethod默认null为POST</param>
        /// <param name="accept">Accept</param>
        public static string HttpPost(string url, CookieContainer cookieContainer = null, Stream postStream = null, Dictionary<string, string> fileDictionary = null, string refererUrl = null, Encoding encoding = null, int? timeOut = null, ContentType? contentType = null, Dictionary<string, string> headerDictionary = null,HttpMethod method = HttpMethod.Post,ContentType? accept = null)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.ToString().ToUpper();
            request.Timeout = timeOut ?? 10000;
            request.Proxy = _webproxy;

            #region 处理Form表单文件上传
            var formUploadFile = fileDictionary != null && fileDictionary.Count > 0;//是否用Form上传文件
            if (formUploadFile)
            {
                //通过表单上传文件
                postStream = postStream ?? new MemoryStream();

                string boundary = "----" + DateTime.Now.Ticks.ToString("x");
                string fileFormdataTemplate = "\r\n--" + boundary + "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";
                string dataFormdataTemplate = "\r\n--" + boundary +
                                                "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
                foreach (var file in fileDictionary)
                {
                    try
                    {
                        var fileName = file.Value;
                        //准备文件流
                        using (var fileStream = FileHelper.GetFileStream(fileName))
                        {
                            string formdata = null;
                            if (fileStream != null)
                            {
                                //存在文件
                                formdata = string.Format(fileFormdataTemplate, file.Key, /*fileName*/ Path.GetFileName(fileName));
                            }
                            else
                            {
                                //不存在文件或只是注释
                                formdata = string.Format(dataFormdataTemplate, file.Key, file.Value);
                            }

                            //统一处理
                            var formdataBytes = Encoding.UTF8.GetBytes(postStream.Length == 0 ? formdata.Substring(2, formdata.Length - 2) : formdata);//第一行不需要换行
                            postStream.Write(formdataBytes, 0, formdataBytes.Length);

                            //写入文件
                            if (fileStream != null)
                            {
                                byte[] buffer = new byte[1024];
                                int bytesRead = 0;
                                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    postStream.Write(buffer, 0, bytesRead);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                //结尾
                var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                postStream.Write(footer, 0, footer.Length);

                request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            }
            else
            {
                request.ContentType = contentType==null ? "application/x-www-form-urlencoded":contentType.Value.ToValue();
            }
            #endregion
            
            request.ContentLength = postStream != null ? postStream.Length : 0;
            request.Accept = accept ==null?"text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8":accept.Value.ToValue();
            request.KeepAlive = true;

            if (!string.IsNullOrEmpty(refererUrl))
            {
                request.Referer = refererUrl;
            }
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";

            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }

            if (headerDictionary != null)//自定义头增加
            {
                foreach (var header in headerDictionary)
                {
                    switch (header.Key.ToLower())
                    {
                        case "accept":
                            request.Accept = header.Value;
                            break;
                        case "contenttype":
                            request.ContentType = header.Value;
                            break;
                        default:
                            request.Headers.Add(header.Key, header.Value);
                            break;
                    }

                }
            }

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;

                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                postStream.Close();//关闭访问
            }
            #endregion
            

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (cookieContainer != null)
            {
                response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            }

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.GetEncoding("utf-8")))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }

        #endregion

        #endregion
    }
}
