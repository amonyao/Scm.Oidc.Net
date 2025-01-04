namespace Com.Scm.Oidc.Response
{
    public class OidcUserInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }

        public string GetAvatarUrl()
        {
            return OidcClient.BASE_URL + Avatar;
        }
    }
}
