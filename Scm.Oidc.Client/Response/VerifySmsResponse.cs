using Com.Scm.Oidc;
using Com.Scm.Oidc.Response;

namespace Com.Scm.Response
{
    /// <summary>
    /// 校验授权码响应
    /// </summary>
    public class VerifySmsResponse : OidcResponse
    {
        public OidcUserInfo User { get; set; }
    }
}
