namespace Easemob.Restfull4Net.Entity.Request
{
    public class UserBlockDeleteRequest
    {
        /// <summary>
        /// 要删除黑名单的用户名
        /// </summary>
        public string owner_username { get; set; }
        /// <summary>
        /// 被删除黑名单的用户名
        /// </summary>
        public string blocked_username { get; set; }
    }
}
