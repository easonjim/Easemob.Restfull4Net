namespace Easemob.Restfull4Net.Entity.Response
{
    public class TokenResponse:BaseResponse
    {
        public string access_token { get; set; }
        public int expires_in { get; set; }
        public string application { get; set; }
    }
}
