using Com.Scm.Oidc;
using Com.Scm.Oidc.Response;

namespace Com.Scm.Response
{
    /// <summary>
    /// 侦听响应
    /// </summary>
    public class ListenResponse : OidcResponse
    {
        public TicketInfo Ticket { get; set; }

        public OidcUserInfo User { get; set; }
    }
}
