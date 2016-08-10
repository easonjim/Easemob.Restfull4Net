using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using Easemob.Restfull4Net.Entity.Response;
using Easemob.Restfull4Net.Helper;

namespace Easemob.Restfull4Net.Utility.HttpUtility
{
    /// <summary>
    /// 特殊的序列化工具
    /// </summary>
    public static class SerializerUtility
    {
        /// <summary>
        /// 获取Post结果
        /// </summary>
        /// <typeparam name="T">返回数据类型（Json对应的实体）</typeparam>
        /// <param name="returnText">要序列化的JSON</param>
        /// <param name="errorText">错误信息的JSON，默认为null</param>
        /// <param name="statusCode">错误返回的请求码</param>
        /// <param name="maxJsonLength">允许最大JSON长度</param>
        public static T GetResult<T>(string returnText, string errorText = null, HttpStatusCode? statusCode = null,int? maxJsonLength = null) where T : BaseResponse, new()
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                if (maxJsonLength != null && maxJsonLength.Value > 0)
                {
                    js.MaxJsonLength = maxJsonLength.Value;
                }
                if (!errorText.IsNullOrEmpty())
                {
                    T model = new T();
                    ErrorResponse result = js.Deserialize<ErrorResponse>(errorText);
                    ((BaseResponse) model).ErrorMessage = result;
                    ((BaseResponse) model).StatusCode = statusCode ?? HttpStatusCode.InternalServerError;
                    return model;
                }
                else
                {
                    //特殊返回字符处理
                    var replace = returnText.Replace("share-secret", "share_secret"); //处理share-secret

                    T result = js.Deserialize<T>(replace);
                    ((BaseResponse) result).StatusCode = HttpStatusCode.OK; //默认为200
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogTraceHelper.SendLog(string.Format("序列化失败！\r\n\tMessage:{0}\r\n\tStackTrace:{1}", ex.Message, ex.StackTrace), "GetResult", isDebug: true);
                T model = new T();
                ((BaseResponse) model).StatusCode = statusCode ?? HttpStatusCode.BadRequest;
                ((BaseResponse) model).ErrorMessage = new ErrorResponse() {exception = ex.Message, error_description = ex.StackTrace};
                return model;
            }
        }
    }
}
