using Com.Scm.Oidc;
using Com.Scm.Oidc.Response;

namespace Com.Scm.Response
{
    public class ListenResponse : OidcResponse
    {
        public TicketInfo Ticket { get; set; }

        public OidcUserInfo User { get; set; }
    }
}
