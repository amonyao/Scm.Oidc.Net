using Com.Scm.Oidc;
using Com.Scm.Oidc.Response;

namespace Com.Scm.Response
{
    /// <summary>
    /// 校验授权码响应
    /// </summary>
    public class VerifySmsResponse : OidcResponse
    {
        public string key { get; set; }

        public string redirect_url { get; set; }
        public string grant_type { get; set; }
        public string grant_data { get; set; }
        public string state { get; set; }

        public OidcUserInfo User { get; set; }
    }
}
