namespace Easemob.Restfull4Net.Entity.Request
{
    public class UserFriendDeleteRequest
    {
        /// <summary>
        /// 要删除好友的用户名
        /// </summary>
        public string owner_username { get; set; }
        /// <summary>
        /// 被删除的用户名
        /// </summary>
        public string friend_username { get; set; }
    }
}
