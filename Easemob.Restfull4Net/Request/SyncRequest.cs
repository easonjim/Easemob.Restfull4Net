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
                        CacheHelper.Insert(base.TokenCacheKey, token, expireSeconds);
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

        /// <summary>
        /// 自定义设置token
        /// </summary>
        public void SetToken(string token)
        {
            CacheHelper.Insert(base.TokenCacheKey, token);
        }

        #endregion

        #region 用户体系

        #region IM 用户管理

        /// <summary>
        /// 注册 IM 用户[单个]
        /// </summary>
        /// <param name="userCreateReqeust">UserCreateReqeust</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserCreate(UserCreateReqeust userCreateReqeust, int? timeOut = null,int tryCount = 0)
        {
            var url = base.UrlCreateUsers;
            var formData = (Dictionary<string, object>)userCreateReqeust.ToDictionary<object>();
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount>0)
            {
                return this.UserCreate(userCreateReqeust, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 注册 IM 用户[单个]
        /// </summary>
        /// <param name="userCreateReqeusts">UserCreateReqeust</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserCreate(List<UserCreateReqeust> userCreateReqeusts, int? timeOut = null, int tryCount = 0)
        {
            var url = base.UrlCreateUsers;
            var formData = userCreateReqeusts;
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse, UserCreateReqeust>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserCreate(userCreateReqeusts, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 获取 IM 用户[单个]
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserGet(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlGetUser, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserGet(userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 获取 IM 用户[批量]
        /// </summary>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="limit">获取数量</param>
        /// <param name="cursor">游标</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserGetByLimit(int limit, string cursor, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlGetUsers, limit, cursor);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserGetByLimit(limit,cursor, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 删除 IM 用户[单个]
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserDelete(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlDeleteUser, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserDelete(userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        ///  删除 IM 用户[批量]
        /// </summary>
        /// <param name="limit">数量</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserDeleteByLimit(int limit, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlDeleteUsers, limit);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserDeleteByLimit(limit, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 重置 IM 用户密码
        /// </summary>
        /// <param name="request">UserResetPasswordRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserRestPassword(UserPasswordRestRequest request, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlRestPassword,request.username);
            var formData = ObjectJsonHelper.CreateDictionary<UserPasswordRestRequest, Dictionary<string, object>>(new ObjectJsonItem<UserPasswordRestRequest>() { SetKey = (obj => obj.newpassword), Value = request.newpassword });
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Put.PutGetJson<UserResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserRestPassword(request, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <param name="request">UserResetNickNameRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserRestNickName(UserNickNameRestRequest request, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlRestNickName, request.username);
            var formData = ObjectJsonHelper.CreateDictionary<UserNickNameRestRequest, Dictionary<string, object>>(new ObjectJsonItem<UserNickNameRestRequest>() { SetKey = (obj => obj.nickname), Value = request.nickname });
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Put.PutGetJson<UserResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserRestNickName(request, timeOut, --tryCount);
            }
            return result;
        }
        
        #endregion

        #region 好友与黑名单

        /// <summary>
        /// 给 IM 用户添加好友
        /// </summary>
        /// <param name="userFriendAddRequest">UserFriendAddRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserFriendAdd(UserFriendAddRequest userFriendAddRequest, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlAddFriend,userFriendAddRequest.owner_username,userFriendAddRequest.friend_username);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserFriendAdd(userFriendAddRequest, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 解除 IM 用户的好友关系
        /// </summary>
        /// <param name="userFriendDeleteRequest">UserFriendDeleteRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserFriendDelete(UserFriendDeleteRequest userFriendDeleteRequest, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlDeleteFriend, userFriendDeleteRequest.owner_username, userFriendDeleteRequest.friend_username);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserFriendDelete(userFriendDeleteRequest, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 查看好友
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserFriendResponse UserFriendGet(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlGetFriend, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserFriendResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserFriendGet(userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 获取 IM 用户的黑名单
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserFriendResponse UserBlockGet(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlGetBlock, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserFriendResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserBlockGet(userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 往 IM 用户的黑名单中加人
        /// </summary>
        /// <param name="userBlockAddRequest">UserBlockAddRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserFriendResponse UserBlockAdd(UserBlockAddRequest userBlockAddRequest, int? timeOut = null, int tryCount = 0)
        {   
            var url = string.Format(base.UrlAddBlock, userBlockAddRequest.owner_username);
            var formData = ObjectJsonHelper.CreateDictionary<UserBlockAddRequest, Dictionary<string, object>>(new ObjectJsonItem<UserBlockAddRequest>() { SetKey = (obj => obj.usernames), Value = userBlockAddRequest.usernames });
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserFriendResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserBlockAdd(userBlockAddRequest, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 从 IM 用户的黑名单中减人
        /// </summary>
        /// <param name="userBlockDeleteRequest">UserBlockDeleteRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserFriendResponse UserBlockDelete(UserBlockDeleteRequest userBlockDeleteRequest, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlDeleteBlock, userBlockDeleteRequest.owner_username, userBlockDeleteRequest.blocked_username);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<UserFriendResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserBlockDelete(userBlockDeleteRequest, timeOut, --tryCount);
            }
            return result;
        }
        #endregion

        #region 在线与离线

        /// <summary>
        /// 查看用户在线状态
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserStatusGet(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlGetStatus, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserStatusGet(userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 查询离线消息数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserOfflineMsgCountGet(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlGetOfflineMsgCount, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserOfflineMsgCountGet(userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 查询某条离线消息状态
        /// </summary>
        /// <param name="request">UserOfflineMsgStatusGetRequest</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserOfflineMsgStatust(UserOfflineMsgStatusGetRequest request, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlGetOfflineMsgStatus, request.username,request.msg_id);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserOfflineMsgStatust(request, timeOut, --tryCount);
            }
            return result;
        }
        #endregion

        #region 账号禁用与解禁

        /// <summary>
        /// 用户账号禁用
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserSetDeactivate(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlSetUserDeactivate,userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserSetDeactivate(userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 账号禁用与解禁
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserSetActivate(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlSetUserActivate, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<UserResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserSetActivate(userName, timeOut, --tryCount);
            }
            return result;
        }

        #endregion

        #region 强制用户下线

        /// <summary>
        /// 强制用户下线
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public UserResponse UserSetDisconnect(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlSetUserDeactivate,userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<UserResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.UserSetDisconnect(userName, timeOut, --tryCount);
            }
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
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatMsgResponse ChatMsgExport(string ql, int? limit = null, string cursor = null, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlExportChatMsg,ql.Replace(" ","+"), limit, cursor);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatMsgResponse>(url, null, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatMsgExport(ql,limit,cursor, timeOut, --tryCount);
            }
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
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public MsgResponse MsgSend<T>(MsgRequest<T> msgRequest, int? timeOut = null, int tryCount = 0) where T : BaseMsg 
        {
            var url = base.UrlSendMsg;
            var formData = (Dictionary<string, object>)msgRequest.ToDictionary<object>();
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<MsgResponse>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.MsgSend(msgRequest, timeOut, --tryCount);
            }
            return result;
        }

        #endregion

        #region 文件上传下载

        /// <summary>
        /// 上传语音/图片文件
        /// </summary>
        /// <param name="filePath">文件本地绝对路径</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatFileResponse ChatFileUpload(string filePath, int? timeOut = null, int tryCount = 0)
        {
            var headerDictionary = new Dictionary<string, string>(base.HeaderDictionary) { { "restrict-access","true" } };//解决并发问题
            var url = base.UrlUploadFiles;
            var fileData = new Dictionary<string,string>(){{"file",filePath}};
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatFileResponse>(url, null, new Dictionary<string, object>(),fileData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, null, headerDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatFileUpload(filePath, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 下载语音/图片文件
        /// </summary>
        /// <param name="shareSecret">上传资源时返回的share-secret</param>
        /// <param name="uuid">上传资源时返回的uuid</param>
        /// <param name="stream">Stream</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        public ChatFileResponse ChatFileDownload(string shareSecret, string uuid, Stream stream, int? timeOut = null)
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

        #region 群组管理

        #region 获取群组

        /// <summary>
        /// 获取 APP 中所有的群组
        /// </summary>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseList ChatGroupGet(int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlChatGroupsGet);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatGroupResponseList>(url, null,null,null,null,httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupGet(timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 分页获取 APP 下的群组
        /// </summary>
        /// <param name="limit">预期获取的记录数</param>
        /// <param name="cursor">游标，如果数据还有下一页，API 返回值会包含此字段</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseList ChatGroupGetByLimit(int limit,string cursor = "",int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlChatGroupsGetByLimit,limit,cursor);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatGroupResponseList>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupGetByLimit(limit, cursor, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 获取群组详情
        /// </summary>
        /// <param name="groupIDs">群组ID数组</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponse ChatGroupDetails(string[] groupIDs, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlChatGroupsDetails, string.Join(",",groupIDs));
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatGroupResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupDetails(groupIDs, timeOut, --tryCount);
            }
            return result;
        }
        #endregion

        #region 管理群组

        /// <summary>
        /// 创建一个群组
        /// </summary>
        /// <param name="createChatGroupRequest">请求实体</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseCreate ChatGroupCreate(CreateChatGroupRequest createChatGroupRequest, int? timeOut = null, int tryCount = 0)
        {
            var url = base.UrlChatGroupsCreate;
            var formData = (Dictionary<string, object>)createChatGroupRequest.ToDictionary<object>();
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatGroupResponseCreate>(url, null, formData,null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupCreate(createChatGroupRequest, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 修改群组信息
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="updateChatGroupRequest">请求实体</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseUpdate ChatGroupUpdate(string groupID,UpdateChatGroupRequest updateChatGroupRequest, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsUpdate,groupID);
            var formData = (Dictionary<string, object>)updateChatGroupRequest.ToDictionary<object>(); 
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Put.PutGetJson<ChatGroupResponseUpdate>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupUpdate(groupID,updateChatGroupRequest, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 删除群组
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseDelete ChatGroupDelete(string groupID, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsDelete, groupID);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<ChatGroupResponseDelete>(url, null,null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupDelete(groupID, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 获取群组所有成员
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseMemberAll ChatGroupMemberAll(string groupID, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsMemberAll, groupID);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatGroupResponseMemberAll>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupMemberAll(groupID, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 添加群组成员[单个]
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="userName">环信ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseMemberAdd ChatGroupMemberAdd(string groupID,string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsMemberAdd, groupID,userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatGroupResponseMemberAdd>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupMemberAdd(groupID,userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 添加群组成员[批量]添加群组成员[单个]
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="userNames">环信IDs</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseMemberAddBatch ChatGroupMemberAddBatch(string groupID, string[] userNames, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsMemberAddBatch, groupID);
            var formData = new Dictionary<string, object>() { {"usernames",userNames} }; 
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatGroupResponseMemberAddBatch>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupMemberAddBatch(groupID, userNames, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 移除群组成员[单个]
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="userName">环信ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseMemberDelete ChatGroupMemberDelete(string groupID, string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsMemberDelete, groupID,userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<ChatGroupResponseMemberDelete>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupMemberDelete(groupID, userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 移除群组成员[批量]
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="userNames">环信IDs</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseMemberDeleteBatch ChatGroupMemberDeleteBatch(string groupID, string[] userNames, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsMemberDeleteBatch, groupID,string.Join(",",userNames));
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<ChatGroupResponseMemberDeleteBatch>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupMemberDeleteBatch(groupID, userNames, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 获取一个用户参与的所有群组
        /// </summary>
        /// <param name="userName">环信ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseUser ChatGroupUser(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsUser, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatGroupResponseUser>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupUser( userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 转让群组
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="newOwner">新群主环信ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseChange ChatGroupChange(string groupID,string newOwner, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsChange, groupID);
            var formData = new Dictionary<string, object>() { { "newowner", newOwner } }; 
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Put.PutGetJson<ChatGroupResponseChange>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupChange( groupID,newOwner, timeOut, --tryCount);
            }
            return result;
        }
        #endregion

        #region 黑名单管理

        /// <summary>
        /// 查询群组黑名单
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseBlock ChatGroupBlock(string groupID, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsBlock, groupID);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatGroupResponseBlock>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupBlock(groupID, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 添加用户至群组黑名单[单个]
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="userName">环信ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseBlockAdd ChatGroupBlockAdd(string groupID, string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsBlockAdd, groupID, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatGroupResponseBlockAdd>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupBlockAdd(groupID, userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 添加用户至群组黑名单[批量]
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="userNames">环信IDs</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseBlockAddBatch ChatGroupBlockAddBatch(string groupID, string[] userNames, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsBlockAddBatch, groupID);
            var formData = new Dictionary<string, object>() { { "usernames", userNames } };
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatGroupResponseBlockAddBatch>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupBlockAddBatch(groupID, userNames, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 从群组黑名单移除用户[单个]
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="userName">环信ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseBlockDelete ChatGroupBlockDelete(string groupID, string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsBlockDelete, groupID, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<ChatGroupResponseBlockDelete>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupBlockDelete(groupID, userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 从群组黑名单移除用户[批量]
        /// </summary>
        /// <param name="groupID">群组ID</param>
        /// <param name="userNames">环信IDs</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatGroupResponseBlockDeleteBatch ChatGroupBlockDeleteBatch(string groupID, string[] userNames, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatGroupsBlockDeleteBatch, groupID, string.Join(",", userNames));
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<ChatGroupResponseBlockDeleteBatch>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatGroupBlockDeleteBatch(groupID, userNames, timeOut, --tryCount);
            }
            return result;
        }


        #endregion

        #endregion

        #region 聊天室管理

        #region 管理聊天室


        /// <summary>
        /// 创建聊天室
        /// </summary>
        /// <param name="createChatRoomRequest">请求实体</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponseCreate ChatRoomCreate(CreateChatRoomRequest createChatRoomRequest, int? timeOut = null, int tryCount = 0)
        {
            var url = base.UrlChatRoomsCreate;
            var formData = (Dictionary<string, object>)createChatRoomRequest.ToDictionary<object>();
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatRoomResponseCreate>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomCreate(createChatRoomRequest, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 修改聊天室信息
        /// </summary>
        /// <param name="roomID">聊天室ID</param>
        /// <param name="updateChatRoomRequest">请求实体</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponseUpdate ChatRoomUpdate(string roomID, UpdateChatRoomRequest updateChatRoomRequest, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatRoomsUpdate, roomID);
            var formData = (Dictionary<string, object>)updateChatRoomRequest.ToDictionary<object>();
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Put.PutGetJson<ChatRoomResponseUpdate>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomUpdate(roomID, updateChatRoomRequest, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 删除聊天室
        /// </summary>
        /// <param name="roomID">聊天室ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponseDelete ChatRoomDelete(string roomID, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatRoomsDelete, roomID);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<ChatRoomResponseDelete>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomDelete(roomID, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 获取 APP 中所有的聊天室
        /// </summary>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponse ChatRoomGet(int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlChatRoomsGet);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatRoomResponse>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomGet(timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 获取一个聊天室详情
        /// </summary>
        /// <param name="roomID">聊天室ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponseDetail ChatRoomDetails(string roomID, int? timeOut = null, int tryCount = 0)
        {
            var url = string.Format(base.UrlChatRoomsDetails, roomID);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatRoomResponseDetail>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomDetails(roomID, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 获取用户加入的聊天室
        /// </summary>
        /// <param name="userName">环信ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponseJoined ChatRoomUser(string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatRoomsUserJoin, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Get.GetJson<ChatRoomResponseJoined>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomUser(userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 添加聊天室成员[单个]
        /// </summary>
        /// <param name="roomID">聊天室ID</param>
        /// <param name="userName">环信ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponseMemberAdd ChatRoomMemberAdd(string roomID, string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatRoomsUserAdd, roomID, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatRoomResponseMemberAdd>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomMemberAdd(roomID, userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 添加聊天室成员[批量]
        /// </summary>
        /// <param name="roomID">聊天室ID</param>
        /// <param name="userNames">环信IDs</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponseMemberAddBatch ChatRoomMemberAddBatch(string roomID, string[] userNames, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatRoomsUserBatchAdd, roomID);
            var formData = new Dictionary<string, object>() { { "usernames", userNames } };
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Post.PostGetJson<ChatRoomResponseMemberAddBatch>(url, null, formData, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomMemberAddBatch(roomID, userNames, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 删除聊天室成员[单个]
        /// </summary>
        /// <param name="roomID">聊天室ID</param>
        /// <param name="userName">环信ID</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponseMemberDelete ChatRoomMemberDelete(string roomID, string userName, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatRoomsUserDelete, roomID, userName);
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<ChatRoomResponseMemberDelete>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomMemberDelete(roomID, userName, timeOut, --tryCount);
            }
            return result;
        }

        /// <summary>
        /// 删除聊天室成员[批量]
        /// </summary>
        /// <param name="roomID">聊天室ID</param>
        /// <param name="userNames">环信IDs</param>
        /// <param name="timeOut">默认null为ServerConfig的值</param>
        /// <param name="tryCount">失败重试次数，默认为0</param>
        public ChatRoomResponseMemberDeleteBatch ChatRoomMemberDeleteBatch(string roomID, string[] userNames, int? timeOut = null, int tryCount = 0)
        {
            var url = String.Format(base.UrlChatRoomsUserBatchDelete, roomID, string.Join(",", userNames));
            var httpTimeOut = timeOut ?? base.ServerConfig.HttpTimeOut;
            var result = Delete.DeleteGetJson<ChatRoomResponseMemberDeleteBatch>(url, null, null, null, null, httpTimeOut, base.ServerConfig.MaxJsonLength, base.ServerConfig.IsDebug, ContentType.JSON, base.HeaderDictionary);
            if (result.StatusCode == HttpStatusCode.Unauthorized) /*token失效*/
            {
                TryToken();
                tryCount = 2;//设置重置次数，并往下进行重试
            }
            if (result.StatusCode != HttpStatusCode.OK && tryCount > 0)
            {
                return this.ChatRoomMemberDeleteBatch(roomID, userNames, timeOut, --tryCount);
            }
            return result;
        }

        #endregion

        #endregion
    }
}