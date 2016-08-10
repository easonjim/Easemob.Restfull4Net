namespace Easemob.Restfull4Net.Entity.Response
{
    public class ErrorResponse
    {
        public string error { get; set; }
        public long timestamp { get; set; }
        public int duration { get; set; }
        public string exception { get; set; }
        public string error_description { get; set; }
    }
}
