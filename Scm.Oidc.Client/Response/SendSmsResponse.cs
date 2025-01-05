namespace Com.Scm.Oidc.Response
{
    /// <summary>
    /// 发送授权码响应
    /// </summary>
    public class SendSmsResponse : OidcResponse
    {
        public string Key { get; set; }
    }
}
