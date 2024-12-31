namespace Com.Scm.Oidc.Response
{
    public class UserInfoResponse : OidcResponse
    {
        public string oidc_code { get; set; }
        public string user_name { get; set; }
        public string picture { get; set; }
    }
}
