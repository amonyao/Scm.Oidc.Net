using Com.Scm.Oidc;

namespace Com.Scm.Response
{
    /// <summary>
    /// 握手响应
    /// </summary>
    public class HandshakeResponse : OidcResponse
    {
        public TicketInfo Ticket { get; set; }
    }

    public class TicketInfo
    {
        public string Code { get; set; }
        public long Time { get; set; }
        public string Salt { get; set; }

        public ListenHandle Handle { get; set; }
        public ListenResult Result { get; set; }
    }

    public enum ListenHandle : byte
    {
        None,
        Todo,
        Doing,
        Done
    }

    public enum ListenResult : byte
    {
        None,
        Failure,
        Success
    }
}
