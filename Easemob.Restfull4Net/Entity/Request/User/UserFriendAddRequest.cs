namespace Easemob.Restfull4Net.Entity.Request
{
    public class UserFriendAddRequest
    {
        /// <summary>
        /// 要添加好友的用户名
        /// </summary>
        public string owner_username { get; set; }
        /// <summary>
        /// 被添加的用户名
        /// </summary>
        public string friend_username { get; set; }
    }
}
