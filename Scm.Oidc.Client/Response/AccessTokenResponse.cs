namespace Com.Scm.Oidc.Response
{
    /// <summary>
    /// 获取访问令牌响应
    /// </summary>
    public class AccessTokenResponse : OidcResponse
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string access_token { get; set; }
        /// <summary>
        /// 刷新令牌
        /// </summary>
        public string refresh_token { get; set; }
        /// <summary>
        /// 过期时间（单位：毫秒）
        /// </summary>
        public long expires_in { get; set; }

        /// <summary>
        /// 基于授权的用户信息
        /// </summary>
        public OidcUserInfo User { get; set; }
    }
}
