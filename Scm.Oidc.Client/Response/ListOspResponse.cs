using System.Collections.Generic;

namespace Com.Scm.Oidc.Response
{
    public class ListOspResponse : OidcResponse
    {
        public List<OspItem> Data { get; set; }
    }
}
