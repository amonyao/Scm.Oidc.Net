using Com.Scm.Oidc.Response;
using Com.Scm.Response;
using Com.Scm.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Scm.Oidc
{
    public class OidcClient
    {
#if DEBUG
        public const string BASE_URL = "http://localhost:7201";
#else
        public const string BASE_URL = "http://oidc.org.cn";
#endif
        public const string DATA_URL = BASE_URL + "/data";
        public const string OAUTH_URL = BASE_URL + "/oauth";

        private OidcConfig _Config;

        public OidcClient(OidcConfig config)
        {
            if (config == null)
            {
                config = new OidcConfig();
            }

            _Config = config;
        }

        #region 公共方法
        /// <summary>
        /// 获取所有服务商
        /// </summary>
        /// <returns></returns>
        public async Task<List<OspItem>> ListAllOspAsync()
        {
            var nonce = GenNonce();
            var url = "/OAuth/ListOsp/0";
            url += "?nonce=" + nonce;
            url += "&sign=" + GenSign(nonce);

            var response = await HttpUtils.GetObjectAsync<ListOspResponse>(url);
            if (response == null || !response.IsSuccess())
            {
                return null;
            }

            return response.Data;
        }

        /// <summary>
        /// 获取应用服务商
        /// </summary>
        /// <returns></returns>
        public async Task<List<OspItem>> ListAppOspAsync()
        {
            var nonce = GenNonce();
            var url = OAUTH_URL + "/ListOsp/" + _Config.AppKey;
            url += "?nonce=" + nonce;
            url += "&sign=" + GenSign(nonce);

            var response = await HttpUtils.GetObjectAsync<ListOspResponse>(url);
            if (response == null || !response.IsSuccess())
            {
                return null;
            }

            return response.Data;
        }

        /// <summary>
        /// 登录地址
        /// </summary>
        /// <param name="state">发起方自定义参数，此参数在回调时进行回传</param>
        /// <returns></returns>
        public string GetLoginUrl(string state = null)
        {
            var url = $"{BASE_URL}/Web/Login?key=" + _Config.AppKey;
            if (state != null)
            {
                url += "&state=" + state;
            }
            return url;
        }
        #endregion

        #region OAuth登录
        /// <summary>
        /// 登录授权
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string Authorize(string code)
        {
            var url = OAUTH_URL + "/Authorize";
            url += "?code=" + code;

            return url;
        }

        /// <summary>
        /// 换取Token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<AccessTokenResponse> AccessTokenAsync(string code)
        {
            var url = OAUTH_URL + "/Token";

            var body = new Dictionary<string, string>()
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["client_id"] = _Config.AppKey,
                ["client_secret"] = _Config.AppSecret,
                ["redirect_uri"] = _Config.RedirectUrl
            };

            return await HttpUtils.PostFormObjectAsync<AccessTokenResponse>(url, body, null);
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<OidcResponse> RefreshTokenAsync(string code)
        {
            return null;
        }
        #endregion

        #region 授权码登录
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<SendSmsResponse> SendSmsAsync(OidcSmsEnums type, string code, string key)
        {
            var url = OAUTH_URL + "/SendSms";
            url += "?type=" + type;
            url += "&code=" + code;
            url += "&key=" + GenNonce();

            return await HttpUtils.GetObjectAsync<SendSmsResponse>(url);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="key"></param>
        /// <param name="sms"></param>
        /// <returns></returns>
        public async Task<VerifySmsResponse> VerifySmsAsync(string code, string key, string sms)
        {
            var url = "User/SignIn";
            url += "?code=" + code;
            url += "&key=" + key;
            url += "&sms=" + sms;

            return await HttpUtils.GetObjectAsync<VerifySmsResponse>(url);
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<OidcUserInfo> GetUserInfoAsync(string code)
        {
            var url = "/OAuth/UserInfo";
            url = GenUrl(url);

            var response = await HttpUtils.PostFormObjectAsync<UserInfoResponse>(url);
            if (response == null || !response.IsSuccess())
            {
                return null;
            }

            return response.Data;
        }
        #endregion

        #region 心跳
        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<OidcResponse> HeartBeatAsync(string code)
        {
            var url = "/OAuth/UserInfo";
            url = GenUrl(url);

            var response = await HttpUtils.PostFormObjectAsync<OidcResponse>(url);
            if (response == null || !response.IsSuccess())
            {
                return null;
            }

            return response;
        }
        #endregion

        #region 私有方法
        public string GenUrl(string url)
        {
            return BASE_URL + url;
        }

        private string GenNonce()
        {
            return TextUtils.RandomString(8);
        }

        private string GenSign(string nonce)
        {
            var text = _Config.AppSecret + "@" + nonce + "@" + _Config.AppSecret;
            return TextUtils.Md5(text);
        }
        #endregion
    }
}
