namespace Easemob.Restfull4Net.Entity.Request
{
    public class UserBlockAddRequest
    {
        /// <summary>
        /// 要添加黑名单的用户名
        /// </summary>
        public string owner_username { get; set; }
        /// <summary>
        /// 被添加黑名单的用户名
        /// </summary>
        public string[] usernames { get; set; }
    }
}
