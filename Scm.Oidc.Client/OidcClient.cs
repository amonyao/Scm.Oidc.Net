using Com.Scm.Oidc.Response;
using Com.Scm.Response;
using Com.Scm.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.Scm.Oidc
{
    /// <summary>
    /// OIDC客户端
    /// </summary>
    public class OidcClient
    {
#if DEBUG
        /// <summary>
        /// 服务端路径
        /// </summary>
        public const string BASE_URL = "http://localhost:7201";
#else
        /// <summary>
        /// 服务端路径
        /// </summary>
        public const string BASE_URL = "http://www.oidc.org.cn";
#endif
        /// <summary>
        /// 数据路径
        /// </summary>
        public const string DATA_URL = BASE_URL + "/data";
        /// <summary>
        /// 授权路径
        /// </summary>
        public const string OAUTH_URL = BASE_URL + "/oauth";

        /// <summary>
        /// OIDC配置
        /// </summary>
        private OidcConfig _Config;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="config"></param>
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
        public async Task<List<OidcOspInfo>> ListAllOspAsync()
        {
            var nonce = GenNonce();

            var url = GenAuthUrl("/ListOsp/0");
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
        public async Task<List<OidcOspInfo>> ListAppOspAsync()
        {
            var nonce = GenNonce();

            var url = GenAuthUrl("/ListOsp/" + _Config.AppKey);
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
        public string GetWebUrl(string state = null)
        {
            var url = GenBaseUrl("/Web/Login");
            url += "?client_id=" + _Config.AppKey;
            if (state != null)
            {
                url += "&state=" + state;
            }
            return url;
        }
        #endregion

        #region OAuth登录
        #region 服务端模式
        /// <summary>
        /// 登录授权
        /// </summary>
        /// <param name="state">发起方自定义参数，此参数在回调时进行回传</param>
        /// <returns></returns>
        public string Authorize(string state = null)
        {
            var url = GenAuthUrl("/Authorize");
            url += "?response_type=code";
            url += "&redirect_uri=" + _Config.RedirectUrl;
            url += "&state=" + state;
            //url += "&scope=";

            return url;
        }

        /// <summary>
        /// 换取令牌
        /// </summary>
        /// <param name="code">服务端回调时传递的参数</param>
        /// <returns></returns>
        public async Task<AccessTokenResponse> AccessTokenAsync(string code)
        {
            var url = GenAuthUrl("/Token");

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
        #endregion

        #region 客户端模式
        /// <summary>
        /// 握手
        /// </summary>
        /// <returns></returns>
        public async Task<HandshakeResponse> HandshakeAsync(string request_id)
        {
            var url = GenAuthUrl("/Handshake");

            var body = new Dictionary<string, string>()
            {
                ["response_type"] = "token",
                ["client_id"] = _Config.AppKey,
                ["redirect_uri"] = "",
                ["state"] = "login",
                ["scope"] = "",
                ["request_id"] = request_id
            };

            return await HttpUtils.GetObjectAsync<HandshakeResponse>(url, body, null);
        }

        /// <summary>
        /// 侦听
        /// </summary>
        /// <returns></returns>
        public async Task<ListenResponse> GetListen(TicketInfo ticket)
        {
            var url = GenAuthUrl("/Listen");

            var body = new Dictionary<string, string>()
            {
                ["client_id"] = _Config.AppKey,
                ["ticket"] = ticket.Code,
                ["salt"] = ticket.Salt
            };

            return await HttpUtils.GetObjectAsync<ListenResponse>(url, body, null);
        }
        #endregion

        /// <summary>
        /// 刷新令牌
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="refreshToken">刷新令牌</param>
        /// <returns></returns>
        public async Task<RefreshTokenResponse> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var url = GenAuthUrl("/Token");

            var header = new Dictionary<string, string>()
            {
                ["Authorization"] = accessToken,
            };

            var body = new Dictionary<string, string>()
            {
                ["grant_type"] = "refresh_token",
                ["refresh_token"] = refreshToken,
                ["client_id"] = _Config.AppKey,
                ["client_secret"] = _Config.AppSecret,
                ["redirect_uri"] = _Config.RedirectUrl
            };

            return await HttpUtils.PostFormObjectAsync<RefreshTokenResponse>(url, body, header);
        }
        #endregion

        #region 代码调用登录
        /// <summary>
        /// 执行登录（适用于服务端）
        /// </summary>
        /// <param name="ospCode">服务商代码</param>
        /// <param name="state">发起方自定义参数，此参数在回调时进行回传</param>
        /// <returns></returns>
        public string GetLoginUrl(string ospCode, string state = null)
        {
            var url = GenAuthUrl("/Login");
            url += "/" + ospCode;
            url += "?client_id=" + _Config.AppKey;
            url += "&state=" + state;

            return url;
        }

        /// <summary>
        /// 执行登录（适用于客户端）
        /// </summary>
        /// <param name="ospCode"></param>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public string GetTicketUrl(string ospCode, string ticket)
        {
            var url = GenAuthUrl("/Ticket");
            url += "/" + ospCode;
            url += "?ticket=" + ticket;

            return url;
        }
        #endregion

        #region 授权码登录
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="type">验证码类型</param>
        /// <param name="code">验证码接收地址（邮件或手机）</param>
        /// <param name="seq">自定义识别码，用于防重复提交</param>
        /// <returns></returns>
        public async Task<SendSmsResponse> SendSmsAsync(OidcSmsEnums type, string code, string seq = null)
        {
            var url = GenAuthUrl("/SendSms");
            url += "?type=" + type;
            url += "&code=" + code;
            url += "&seq=" + seq;

            return await HttpUtils.GetObjectAsync<SendSmsResponse>(url);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="client_id">应用ID</param>
        /// <param name="key">发送验证码时，服务端返回的Key</param>
        /// <param name="sms">验证码</param>
        /// <returns></returns>
        public async Task<VerifySmsResponse> VerifySmsAsync(string key, string sms)
        {
            var url = GenAuthUrl("/VerifySms");
            url += "?client_id=" + _Config.AppKey;
            url += "&key=" + key;
            url += "&sms=" + sms;

            return await HttpUtils.PostFormObjectAsync<VerifySmsResponse>(url);
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <returns></returns>
        public async Task<UserInfoResponse> GetUserInfoAsync(string accessToken)
        {
            var url = GenAuthUrl("/UserInfo");
            url += "?token=" + accessToken;

            return await HttpUtils.GetObjectAsync<UserInfoResponse>(url);
        }
        #endregion

        #region 心跳
        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="accessToken">访问令牌</param>
        /// <param name="type">心跳类型</param>
        /// <param name="data">心跳数据</param>
        /// <returns></returns>
        public async Task<HeartBeatResponse> HeartBeatAsync(string accessToken, int type, string data = null)
        {
            var url = GenAuthUrl("/HeartBeat");

            var body = new Dictionary<string, string>()
            {
                ["token"] = accessToken,
                ["device"] = "",
                ["type"] = type.ToString(),
                ["data"] = data
            };

            return await HttpUtils.PostFormObjectAsync<HeartBeatResponse>(url, body);
        }
        #endregion

        #region 私有方法
        public string GenBaseUrl(string url)
        {
            return BASE_URL + url;
        }

        public string GenAuthUrl(string url)
        {
            return OAUTH_URL + url;
        }

        public string GenDataUrl(string url)
        {
            return DATA_URL + url;
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

    public delegate int ListCallback(AccessTokenResponse response);
}
