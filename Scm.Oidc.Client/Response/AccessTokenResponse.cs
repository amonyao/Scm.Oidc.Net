namespace Com.Scm.Oidc.Response
{
    public class AccessTokenResponse : OidcResponse
    {
        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public long expires_in { get; set; }
    }
}
