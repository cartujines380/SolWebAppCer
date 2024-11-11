namespace WCFEnvioCorreo.Model
{
    using Newtonsoft.Json;

    public partial class ErrorResponseApi
    {
        [JsonProperty("trace_id")]
        public string TraceId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
