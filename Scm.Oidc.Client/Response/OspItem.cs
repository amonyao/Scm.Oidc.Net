namespace Com.Scm.Oidc.Response
{
    public class OspItem
    {
        public OidcOspEnums Type { get; set; }

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

        /// <summary>
        /// 是否邮件登录
        /// </summary>
        /// <returns></returns>
        public bool IsEmail()
        {
            return Type == OidcOspEnums.Email;
        }

        /// <summary>
        /// 是否手机登录
        /// </summary>
        /// <returns></returns>
        public bool IsPhone()
        {
            return Type == OidcOspEnums.Phone;
        }

        /// <summary>
        /// 是否授权登录
        /// </summary>
        /// <returns></returns>
        public bool IsOAuth()
        {
            return Type == OidcOspEnums.OAuth;
        }

        /// <summary>
        /// 是否更多
        /// </summary>
        /// <returns></returns>
        public bool IsMore()
        {
            return Type == OidcOspEnums.More;
        }
    }
}
