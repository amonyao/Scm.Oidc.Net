using System.Collections.Generic;

namespace Com.Scm.OAuth.Response
{
    public class ListOspResponse : OidcResponse
    {
        public List<OspItem> Data { get; set; }
    }
}
