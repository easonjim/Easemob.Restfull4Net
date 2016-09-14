using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easemob.Restfull4Net.Config;
using Easemob.Restfull4Net.Utility.HttpUtility;

namespace Easemob.Restfull4Net.Request
{
    /// <summary>
    /// 请求基类
    /// </summary>
    public abstract class BaseRequest
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        protected readonly IServerConfig ServerConfig = null;

        /// <summary>
        /// 自定义标记头
        /// </summary>
        protected Dictionary<string, string> HeaderDictionary{get{return new Dictionary<string, string> {{"Authorization", string.Format("Bearer {0}", GetToken())}};}}

        /// <summary>
        /// 通过IServerConfig对象进行构造连接
        /// </summary>
        protected BaseRequest(IServerConfig serverConfig)
        {
            this.ServerConfig = serverConfig;
        }

        #region Token
        
        /// <summary>
        /// 获取Token，子类必须实现
        /// </summary>
        public abstract string GetToken();

        /// <summary>
        /// token缓存key
        /// </summary>
        protected string TokenCacheKey { get { return string.Concat(this.ServerConfig.ClientId ,"#", this.ServerConfig.ClientSecret);} }

        #endregion

        #region URL

        /// <summary>
        /// 拼装带有org的http链接
        /// </summary>
        protected string UrlBase
        {
            get { return string.Format("{0}{1}/{2}/", this.ServerConfig.ServerUrl, this.ServerConfig.OrgName, this.ServerConfig.AppName); }
        }

        /// <summary>
        /// 获取Token的URL
        /// </summary>
        protected string UrlGetToken
        {
            get { return string.Format("{0}{1}", this.UrlBase, "token"); }
        }

        #region 用户体系

        #region IM 用户管理

        /// <summary>
        /// 注册 IM 用户的URL
        /// </summary>
        protected string UrlCreateUsers
        {
            get { return string.Format("{0}{1}", this.UrlBase, "users"); }
        }

        /// <summary>
        /// 获取 IM 用户[单个]的URL
        /// </summary>
        protected string UrlGetUser
        {
            get { return string.Concat(this.UrlBase, "users/{0}"); }
        }

        /// <summary>
        /// 获取 IM 用户[批量]的URL
        /// </summary>
        protected string UrlGetUsers
        {
            get { return string.Concat(this.UrlBase, "users?limit={0}&cursor={1}"); }
        }

        /// <summary>
        /// 删除 IM 用户[单个]的URL
        /// </summary>
        protected string UrlDeleteUser
        {
            get { return string.Concat(this.UrlBase, "users/{0}"); }
        }

        /// <summary>
        /// 删除 IM 用户[批量]的URL
        /// </summary>
        protected string UrlDeleteUsers
        {
            get { return string.Concat(this.UrlBase, "users?limit={0}"); }
        }

        /// <summary>
        /// 重置 IM 用户密码的URL
        /// </summary>
        protected string UrlRestPassword
        {
            get { return string.Concat(this.UrlBase, "users/{0}/password"); }
        }

        /// <summary>
        /// 修改用户昵称的URL
        /// </summary>
        protected string UrlRestNickName
        {
            get { return string.Concat(this.UrlBase, "users/{0}"); }
        }

        #endregion

        #region 好友与黑名单

        /// <summary>
        /// 给 IM 用户添加好友的URL
        /// </summary>
        protected string UrlAddFriend
        {
            get { return string.Concat(this.UrlBase, "users/{0}/contacts/users/{1}"); }
        }

        /// <summary>
        /// 解除 IM 用户的好友关系的URL
        /// </summary>
        protected string UrlDeleteFriend
        {
            get { return string.Concat(this.UrlBase, "users/{0}/contacts/users/{1}"); }
        }

        /// <summary>
        /// 查看好友的URL
        /// </summary>
        protected string UrlGetFriend
        {
            get { return string.Concat(this.UrlBase, "users/{0}/contacts/users"); }
        }

        /// <summary>
        /// 获取 IM 用户的黑名单的URL
        /// </summary>
        protected string UrlGetBlock
        {
            get { return string.Concat(this.UrlBase, "users/{0}/blocks/users"); }
        }

        /// <summary>
        /// 往 IM 用户的黑名单中加人的URL
        /// </summary>
        protected string UrlAddBlock
        {
            get { return string.Concat(this.UrlBase, "users/{0}/blocks/users"); }
        }

        /// <summary>
        /// 从 IM 用户的黑名单中减人的URL
        /// </summary>
        protected string UrlDeleteBlock
        {
            get { return string.Concat(this.UrlBase, "users/{0}/blocks/users/{1}"); }
        }

        #endregion

        #region 在线与离线

        /// <summary>
        /// 查看用户在线状态的URL
        /// </summary>
        protected string UrlGetStatus
        {
            get { return string.Concat(this.UrlBase, "users/{0}/status"); }
        }

        /// <summary>
        /// 查询离线消息数的URL
        /// </summary>
        protected string UrlGetOfflineMsgCount
        {
            get { return string.Concat(this.UrlBase, "users/{0}/offline_msg_count"); }
        }

        /// <summary>
        /// 查询某条离线消息状态的URL
        /// </summary>
        protected string UrlGetOfflineMsgStatus
        {
            get { return string.Concat(this.UrlBase, "users/{0}/offline_msg_status/{1}"); }
        }

        #endregion

        #region 账号禁用与解禁

        /// <summary>
        /// 用户账号禁用的URL
        /// </summary>
        protected string UrlSetUserDeactivate
        {
            get { return string.Concat(this.UrlBase, "users/{0}/deactivate"); }
        }

        /// <summary>
        /// 用户账号解禁的URL
        /// </summary>
        protected string UrlSetUserActivate
        {
            get { return string.Concat(this.UrlBase, "users/{0}/activate"); }
        }
        #endregion

        #region 强制用户下线

        /// <summary>
        /// 强制用户下线的URL
        /// </summary>
        protected string UrlSetUserDisconnect
        {
            get { return string.Concat(this.UrlBase, "users/{0}/disconnect"); }
        }

        #endregion
        
        #endregion

        #region 聊天记录

        #region 导出聊天记录

        /// <summary>
        /// 导出聊天记录的URL
        /// </summary>
        protected string UrlExportChatMsg
        {
            get { return string.Concat(this.UrlBase, "chatmessages?ql={0}&limit={1}&cursor={2}"); }
        }

        #endregion
        
        #endregion

        #region 发送消息

        /// <summary>
        /// 发送消息
        /// </summary>
        protected string UrlSendMsg
        {
            get { return string.Concat(this.UrlBase, "messages"); }
        }

        #endregion

        #region 文件上传下载

        /// <summary>
        /// 上传语音/图片文件
        /// </summary>
        protected string UrlUploadFiles
        {
            get { return string.Concat(this.UrlBase, "chatfiles"); }
        }

        /// <summary>
        /// 下载语音/图片文件
        /// </summary>
        protected string UrlDownloadFiles
        {
            get { return string.Concat(this.UrlBase, "chatfiles/{0}"); }
        }

        #endregion

        #region 群组管理

        #region 获取群组

        /// <summary>
        /// 获取 APP 中所有的群组
        /// 分页获取 APP 下的群组
        /// </summary>
        protected string UrlChatGroupsGet
        {
            get { return string.Concat(this.UrlBase, "chatgroups"); }
        }

        /// <summary>
        /// 分页获取 APP 下的群组
        /// </summary>
        protected string UrlChatGroupsGetByLimit
        {
            get { return string.Concat(this.UrlBase, "chatgroups"); }
        }

        /// <summary>
        /// 获取群组详情
        /// 可以获取一个或多个群组的详情
        /// </summary>
        protected string UrlChatGroupsDetails
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}"); }
        }

        #endregion

        #region 管理群组
        
        /// <summary>
        /// 创建一个群组
        /// </summary>
        protected string UrlChatGroupsCreate
        {
            get { return string.Concat(this.UrlBase, "chatgroups"); }
        }

        /// <summary>
        /// 修改群组信息
        /// </summary>
        protected string UrlChatGroupsUpdate
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}"); }
        }

        /// <summary>
        /// 删除群组
        /// </summary>
        protected string UrlChatGroupsDelete
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}"); }
        }

        /// <summary>
        /// 添加群组成员[单个]
        /// </summary>
        protected string UrlChatGroupsUserAdd
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}/users/{1}"); }
        }

        /// <summary>
        /// 添加群组成员[批量]
        /// 为群组添加多个成员，一次最多可以添加60位成员。
        /// </summary>
        protected string UrlChatGroupsUserBatchAdd
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}/users"); }
        }

        /// <summary>
        /// 移除群组成员[单个]
        /// </summary>
        protected string UrlChatGroupsUserDelete
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}/users/{1}"); }
        }

        /// <summary>
        /// 移除群组成员[批量]
        /// </summary>
        protected string UrlChatGroupsUserBatchDelete
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}/users/{1}"); }
        }

        /// <summary>
        /// 获取一个用户参与的所有群组
        /// </summary>
        protected string UrlChatGroupsUserJoin
        {
            get { return string.Concat(this.UrlBase, "users/{0}/joined_chatgroups"); }
        }

        /// <summary>
        /// 转让群组
        /// </summary>
        protected string UrlChatGroupsTran
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}"); }
        }

        #endregion

        #region 黑名单管理
        
        /// <summary>
        /// 查询群组黑名单
        /// </summary>
        protected string UrlChatGroupsBlockUserGet
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}/blocks/users"); }
        }

        /// <summary>
        /// 添加用户至群组黑名单[单个]
        /// </summary>
        protected string UrlChatGroupsBlockUserAdd
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}/blocks/users/{1}"); }
        }

        /// <summary>
        /// 添加用户至群组黑名单[批量]
        /// 添加多个用户进入一个群组的黑名单，一次性最多可以添加60个用户
        /// </summary>
        protected string UrlChatGroupsBlockUserBatchAdd
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{group_id}/blocks/users"); }
        }

        /// <summary>
        /// 从群组黑名单移除用户[单个]
        /// </summary>
        protected string UrlChatGroupsBlockUserDelete
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}/blocks/users/{1}"); }
        }

        /// <summary>
        /// 从群组黑名单移除用户[批量]
        /// </summary>
        protected string UrlChatGroupsBlockUserBatchDelete
        {
            get { return string.Concat(this.UrlBase, "chatgroups/{0}/blocks/users/{1}"); }
        }
        #endregion
            
        #endregion

        #region 聊天室管理

        #region 管理聊天室
        
        /// <summary>
        /// 创建聊天室
        /// </summary>
        protected string UrlChatRoomsCreate
        {
            get { return string.Concat(this.UrlBase, "chatrooms"); }
        }

        /// <summary>
        /// 修改聊天室信息
        /// </summary>
        protected string UrlChatRoomsUpdate
        {
            get { return string.Concat(this.UrlBase, "chatrooms/{0}"); }
        }

        /// <summary>
        /// 删除聊天室
        /// </summary>
        protected string UrlChatRoomsDelete
        {
            get { return string.Concat(this.UrlBase, "chatrooms/{0}"); }
        }

        /// <summary>
        /// 获取 APP 中所有的聊天室
        /// </summary>
        protected string UrlChatRoomsGet
        {
            get { return string.Concat(this.UrlBase, "chatrooms"); }
        }

        /// <summary>
        /// 获取一个聊天室详情
        /// </summary>
        protected string UrlChatRoomsDetails
        {
            get { return string.Concat(this.UrlBase, "chatrooms/{0}"); }
        }

        /// <summary>
        /// 获取用户加入的聊天室
        /// </summary>
        protected string UrlChatRoomsUserJoin
        {
            get { return string.Concat(this.UrlBase, "users/{0}/joined_chatrooms"); }
        }

        /// <summary>
        /// 添加聊天室成员[单个]
        /// </summary>
        protected string UrlChatRoomsUserAdd
        {
            get { return string.Concat(this.UrlBase, "chatrooms/{0}/users/{1}"); }
        }

        /// <summary>
        /// 添加聊天室成员[批量]
        /// </summary>
        protected string UrlChatRoomsUserBatchAdd
        {
            get { return string.Concat(this.UrlBase, "chatrooms/{0}/users"); }
        }

        /// <summary>
        /// 删除聊天室成员[单个]
        /// </summary>
        protected string UrlChatRoomsUserDelete
        {
            get { return string.Concat(this.UrlBase, "chatrooms/{0}/users/{1}"); }
        }
        
        /// <summary>
        /// 删除聊天室成员[批量]
        /// </summary>
        protected string UrlChatRoomsUserBatchDelete
        {
            get { return string.Concat(this.UrlBase, "chatrooms/{0}/users/{1}"); }
        }
        #endregion

        #endregion

        #endregion


    }
}
