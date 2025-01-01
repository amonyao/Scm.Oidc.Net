namespace Com.Scm.Oidc.Response
{
    public class UserInfoResponse : OidcDataResponse<UserInfo>
    {
    }

    public class UserInfo
    {
        public string code { get; set; }
        public string name { get; set; }
        public string avatar { get; set; }
    }
}
