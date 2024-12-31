namespace Com.Scm.Oidc.Response
{
    public class OspItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public string GetAuthUrl()
        {
            return $"{OidcClient.OAUTH_URL}/{Code}";
        }

        public string GetIconUrl()
        {
            return $"{OidcClient.DATA_URL}/logo/{Icon}";
        }
    }
}
