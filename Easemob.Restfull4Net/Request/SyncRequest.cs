using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Easemob.Restfull4Net.Common;
using Easemob.Restfull4Net.Config;
using Easemob.Restfull4Net.Entity.Request;
using Easemob.Restfull4Net.Entity.Response;
using Easemob.Restfull4Net.Helper;
using Easemob.Restfull4Net.Utility.HttpUtility;

namespace Easemob.Restfull4Net.Request
{
    /// <summary>
    /// 同步请求
    /// </summary>
    public class SyncRequest:BaseRequest
    {
        public SyncRequest(IServerConfig serverConfig) : base(serverConfig)
        {
        }

        #region 获取Token

        /// <summary>
        /// 获取授权token
        /// </summary>
        public override string GetToken()
        {
            if (string.IsNullOrEmpty(base.ServerConfig.ClientId) || string.IsNullOrEmpty(base.ServerConfig.ClientSecret))
            {
                return string.Empty;
            }
            
            if (CacheHelper.Get(base.TokenCacheKey) != null)
            {
                return CacheHelper.Get(base.TokenCacheKey).ToString();
            }

            var tokenRequest = new TokenRequest()
            {
                client_id = base.ServerConfig.ClientId,
                client_secret = base.ServerConfig.ClientSecret,
                grant_type = "client_credentials"
            };
            var tokenResponse = Post.PostGetJson<TokenResponse>(base.UrlGetToken, null, (Dictionary<string,object>)tokenRequest.ToDictionary<object>(), null, null, base.ServerConfig.HttpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON);
            if (tokenResponse.StatusCode == HttpStatusCode.OK)
            {
                string token;
                try
                {
                    token = tokenResponse.access_token;
                    var expireSeconds = tokenResponse.expires_in;
                    //设置缓存
                    if (!string.IsNullOrEmpty(token) && token.Length > 0 && expireSeconds > 0)
                    {
                        CacheHelper.Insert(base.TokenCacheKey, token, DateTime.Now.AddSeconds(expireSeconds).Second);
                    }
                }
                catch
                {
                    return string.Empty;
                }
                return token;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 刷新token的缓存，再重新获取token
        /// </summary>
        /// <returns></returns>
        public string TryToken()
        {
            LogTraceHelper.SendLog("Token失效，重新获取","TryToken",isDebug:base.ServerConfig.IsDebug);
            CacheHelper.Remove(base.TokenCacheKey);
            return this.GetToken();
        }

        #endregion

        #region 用户体系

        #region IM 用户管理

        /// <summary>
        /// 注册 IM 用户[单个]
        /// </summary>
        /// <param name="userCreateReqeust">UserCreateReqeust</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserCreate(UserCreateReqeust userCreateReqeust, int? timeOut = null)
        {
            var url = base.UrlCreateUsers;
            var formData = (Dictionary<string, object>)userCreateReqeust.ToDictionary<object>();
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 注册 IM 用户[单个]
        /// </summary>
        /// <param name="userCreateReqeusts">UserCreateReqeust</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserCreate(List<UserCreateReqeust> userCreateReqeusts, int? timeOut = null)
        {
            var url = base.UrlCreateUsers;
            var formData = userCreateReqeusts;
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse, UserCreateReqeust>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 获取 IM 用户[单个]
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserGet(string userName, int? timeOut = null)
        {
            var url = string.Format(base.UrlGetUser, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 获取 IM 用户[批量]
        /// </summary>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="limit">获取数量</param>
        /// <param name="cursor">游标</param>
        public UserResponse UserGetByLimit(int limit, string cursor, int? timeOut = null)
        {
            var url = string.Format(base.UrlGetUsers, limit, cursor);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 删除 IM 用户[单个]
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserDelete(string userName, int? timeOut = null)
        {
            var url = string.Format(base.UrlDeleteUser, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        ///  删除 IM 用户[批量]
        /// </summary>
        /// <param name="limit">数量</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserDeleteByLimit(int limit, int? timeOut = null)
        {
            var url = string.Format(base.UrlDeleteUsers, limit);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 重置 IM 用户密码
        /// </summary>
        /// <param name="request">UserResetPasswordRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserRestPassword(UserPasswordRestRequest request, int? timeOut = null)
        {
            var url = string.Format(base.UrlRestPassword,request.username);
            var formData = ObjectJsonHelper.CreateDictionary<UserPasswordRestRequest, Dictionary<string, object>>(new ObjectJsonItem<UserPasswordRestRequest>() { SetKey = (obj => obj.newpassword), Value = request.newpassword });
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Put.PutGetJson<UserResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <param name="request">UserResetNickNameRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserRestNickName(UserNickNameRestRequest request, int? timeOut = null)
        {
            var url = string.Format(base.UrlRestNickName, request.username);
            var formData = ObjectJsonHelper.CreateDictionary<UserNickNameRestRequest, Dictionary<string, object>>(new ObjectJsonItem<UserNickNameRestRequest>() { SetKey = (obj => obj.nickname), Value = request.nickname });
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Put.PutGetJson<UserResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }
        
        #endregion

        #region 好友与黑名单

        /// <summary>
        /// 给 IM 用户添加好友
        /// </summary>
        /// <param name="userFriendAddRequest">UserFriendAddRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserFriendAdd(UserFriendAddRequest userFriendAddRequest, int? timeOut = null)
        {
            var url = string.Format(base.UrlAddFriend,userFriendAddRequest.owner_username,userFriendAddRequest.friend_username);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 解除 IM 用户的好友关系
        /// </summary>
        /// <param name="userFriendDeleteRequest">UserFriendDeleteRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserFriendDelete(UserFriendDeleteRequest userFriendDeleteRequest, int? timeOut = null)
        {
            var url = string.Format(base.UrlDeleteFriend, userFriendDeleteRequest.owner_username, userFriendDeleteRequest.friend_username);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 查看好友
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserFriendResponse UserFriendGet(string userName, int? timeOut = null)
        {
            var url = string.Format(base.UrlGetFriend, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserFriendResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 获取 IM 用户的黑名单
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserFriendResponse UserBlockGet(string userName, int? timeOut = null)
        {
            var url = string.Format(base.UrlGetBlock, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserFriendResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 往 IM 用户的黑名单中加人
        /// </summary>
        /// <param name="userBlockAddRequest">UserBlockAddRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserFriendResponse UserBlockAdd(UserBlockAddRequest userBlockAddRequest, int? timeOut = null)
        {   
            var url = string.Format(base.UrlAddBlock, userBlockAddRequest.owner_username);
            var formData = ObjectJsonHelper.CreateDictionary<UserBlockAddRequest, Dictionary<string, object>>(new ObjectJsonItem<UserBlockAddRequest>() { SetKey = (obj => obj.usernames), Value = userBlockAddRequest.usernames });
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserFriendResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 从 IM 用户的黑名单中减人
        /// </summary>
        /// <param name="userBlockDeleteRequest">UserBlockDeleteRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserFriendResponse UserBlockDelete(UserBlockDeleteRequest userBlockDeleteRequest, int? timeOut = null)
        {
            var url = string.Format(base.UrlDeleteBlock, userBlockDeleteRequest.owner_username, userBlockDeleteRequest.blocked_username);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<UserFriendResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }
        #endregion

        #region 在线与离线

        /// <summary>
        /// 查看用户在线状态
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserStatusGet(string userName, int? timeOut = null)
        {
            var url = string.Format(base.UrlGetStatus, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 查询离线消息数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserOfflineMsgCountGet(string userName, int? timeOut = null)
        {
            var url = string.Format(base.UrlGetOfflineMsgCount, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 查询某条离线消息状态
        /// </summary>
        /// <param name="request">UserOfflineMsgStatusGetRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserOfflineMsgStatust(UserOfflineMsgStatusGetRequest request, int? timeOut = null)
        {
            var url = string.Format(base.UrlGetOfflineMsgStatus, request.username,request.msg_id);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }
        #endregion

        #region 账号禁用与解禁

        /// <summary>
        /// 用户账号禁用
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserSetDeactivate(string userName, int? timeOut = null)
        {
            var url = string.Format(base.UrlSetUserDeactivate,userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 账号禁用与解禁
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserSetActivate(string userName, int? timeOut = null)
        {
            var url = string.Format(base.UrlSetUserActivate, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        #endregion

        #region 强制用户下线

        /// <summary>
        /// 强制用户下线
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public UserResponse UserSetDisconnect(string userName, int? timeOut = null)
        {
            var url = string.Format(base.UrlSetUserDeactivate,userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        #endregion

        #endregion

        #region 聊天记录

        #region 导出聊天记录

        /// <summary>
        /// 获取聊天记录
        /// </summary>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="ql">条件，已对空格做了+号处理，详见：http://docs.easemob.com/start/100serverintegration/30chatlog</param>
        /// <param name="limit">获取数量,max:1000</param>
        /// <param name="cursor">游标</param>
        public ChatMsgResponse ChatMsgExport(string ql,int? limit = null, string cursor = null, int? timeOut = null)
        {
            var url = string.Format(base.UrlExportChatMsg,ql.Replace(" ","+"), limit, cursor);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatMsgResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        #endregion

        #endregion

        #region 发送消息

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgRequest">MsgRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public MsgResponse MsgSend<T>(MsgRequest<T> msgRequest, int? timeOut = null) where T:BaseMsg 
        {
            var url = base.UrlSendMsg;
            var formData = (Dictionary<string, object>)msgRequest.ToDictionary<object>();
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<MsgResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        #endregion

        #region 文件上传下载

        /// <summary>
        /// 上传语音/图片文件
        /// </summary>
        /// <param name="filePath">文件本地绝对路径</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public ChatFileResponse ChatFileUpload(string filePath, int? timeOut = null)
        {
            var headerDictionary = new Dictionary<string, string>(base.HeaderDictionary) { { "restrict-access","true" } };//解决并发问题
            var url = base.UrlUploadFiles;
            var fileData = new Dictionary<string,string>(){{"file",filePath}};
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatFileResponse>(url, null, new Dictionary<string, object>(),fileData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, null, headerDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized)/*token失效*/TryToken();/*只做重新获取token，不做重新请求*/
            return result;
        }

        /// <summary>
        /// 下载语音/图片文件
        /// </summary>
        /// <param name="shareSecret">上传资源时返回的share-secret</param>
        /// <param name="uuid">上传资源时返回的uuid</param>
        /// <param name="stream">Stream</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public ChatFileResponse ChatFileDownload(string shareSecret, string uuid,Stream stream, int? timeOut = null)
        {
            var headerDictionary = new Dictionary<string, string>(base.HeaderDictionary) { { "share-secret", shareSecret } };//解决并发问题
            var url = string.Format(base.UrlDownloadFiles, uuid);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            try
            {
                Get.FillDownload(url, stream, base.ServerConfig.IsDebug, headerDictionary,null,ContentType.DEFAULT);
                return new ChatFileResponse(){ StatusCode =  HttpStatusCode.OK};
            }
            catch (Exception ex)
            {
                LogTraceHelper.SendLog(string.Format("文件下载失败\r\n\tMessage:{0}\r\n\tStackTrace:{1}",ex.Message,ex.StackTrace), "ChatFileDownload", isDebug: base.ServerConfig.IsDebug);
                return new ChatFileResponse() { StatusCode = HttpStatusCode.BadRequest,ErrorMessage = new ErrorResponse(){ exception = ex.Message,error_description = ex.StackTrace}};
            }
        }
        
        /// <summary>
        /// 下载缩略图
        /// </summary>
        /// <param name="shareSecret">上传资源时返回的share-secret</param>
        /// <param name="uuid">上传资源时返回的uuid</param>
        /// <param name="stream">Stream</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public ChatFileResponse ChatFileDownloadThumbnail(string shareSecret, string uuid, Stream stream, int? timeOut = null)
        {
            var headerDictionary = new Dictionary<string, string>(base.HeaderDictionary) { { "share-secret", shareSecret }, { "thumbnail", "true" } };//解决并发问题
            var url = string.Format(base.UrlDownloadFiles, uuid);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            try
            {
                Get.FillDownload(url, stream, base.ServerConfig.IsDebug, headerDictionary,null,ContentType.DEFAULT);
                return new ChatFileResponse(){ StatusCode =  HttpStatusCode.OK};
            }
            catch (Exception ex)
            {
                LogTraceHelper.SendLog(string.Format("缩略图下载失败\r\n\tMessage:{0}\r\n\tStackTrace:{1}",ex.Message,ex.StackTrace), "ChatFileDownload", isDebug: base.ServerConfig.IsDebug);
                return new ChatFileResponse() { StatusCode = HttpStatusCode.BadRequest,ErrorMessage = new ErrorResponse(){ exception = ex.Message,error_description = ex.StackTrace}};
            }
        }
        #endregion
    }
}