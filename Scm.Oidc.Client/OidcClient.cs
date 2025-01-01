using Com.Scm.Oidc.Response;
using Com.Scm.Utils;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.Scm.Oidc
{
    public class OidcClient
    {
        public const string BASE_URL = "http://oidc.org.cn";
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
        public async Task<List<OspItem>> ListAllOsp()
        {
            var nonce = GenNonce();
            var url = "/OAuth/ListOsp/0";
            url += "?nonce=" + nonce;
            url += "&sign=" + GenSign(nonce);

            var response = await GetObjectAsync<ListOspResponse>(url);
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
        public async Task<List<OspItem>> ListAppOsp()
        {
            var nonce = GenNonce();
            var url = "/OAuth/ListOsp/" + _Config.AppKey;
            url += "?nonce=" + nonce;
            url += "&sign=" + GenSign(nonce);

            var response = await GetObjectAsync<ListOspResponse>(url);
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
            var url = OAUTH_URL + "/oauth/authorize";
            url += "?code=" + code;

            return url;
        }

        /// <summary>
        /// 换取Token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<OidcResponse> AccessToken(string code)
        {
            return null;
        }

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<OidcResponse> RefreshToken(string code)
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
        public async Task<OidcResponse> SendSms(OidcSmsEnums type, string code, string key)
        {
            var url = "User/SendSms";
            url += "?type=" + type;
            url += "&code=" + code;
            url += "&key=" + GenNonce();

            var response = await GetObjectAsync<SendSmsResponse>(url);
            if (response == null || !response.IsSuccess())
            {
                return null;
            }

            return null;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="key"></param>
        /// <param name="sms"></param>
        /// <returns></returns>
        public async Task<OidcResponse> SignIn(string code, string key, string sms)
        {
            var url = "User/SignIn";
            url += "?code=" + code;
            url += "&key=" + key;
            url += "&sms=" + sms;

            var response = await GetObjectAsync<SendSmsResponse>(url);
            if (response == null || !response.IsSuccess())
            {
                return null;
            }

            return null;
        }
        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserInfo> GetUserInfo(string code)
        {
            var url = "/OAuth/UserInfo";
            url = GenUrl(url);

            var response = await PostObjectAsync<UserInfoResponse>(url);
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
        public async Task<OidcResponse> HeartBeat(string code)
        {
            var url = "/OAuth/UserInfo";
            url = GenUrl(url);

            var response = await PostObjectAsync<OidcResponse>(url);
            if (response == null || !response.IsSuccess())
            {
                return null;
            }

            return response;
        }
        #endregion

        #region 网络请求
        public async Task<T> GetObjectAsync<T>(string url) where T : class, new()
        {
            var client = new HttpClient();
            url = GenUrl(url);
            var message = await client.GetAsync(url);
            if (!message.IsSuccessStatusCode)
            {
                return default(T);
            }

            var result = await message.Content.ReadAsStringAsync();
            return result.AsJsonObject<T>();
        }

        public async Task<string> GetStringAsync(string url)
        {
            var client = new HttpClient();
            url = GenUrl(url);
            var message = await client.GetAsync(url);
            if (!message.IsSuccessStatusCode)
            {
                return "";
            }

            return await message.Content.ReadAsStringAsync();
        }

        public async Task<T> PostObjectAsync<T>(string url) where T : class, new()
        {
            var client = new HttpClient();
            url = GenUrl(url);

            var content = new StringContent("", Encoding.UTF8);
            var message = await client.PostAsync(url, content);
            if (!message.IsSuccessStatusCode)
            {
                return default(T);
            }

            var result = await message.Content.ReadAsStringAsync();
            return result.AsJsonObject<T>();
        }

        public async Task<string> PostStringAsync(string url)
        {
            var client = new HttpClient();
            url = GenUrl(url);

            var content = new StringContent("", Encoding.UTF8);
            var message = await client.PostAsync(url, content);
            if (!message.IsSuccessStatusCode)
            {
                return "";
            }

            return await message.Content.ReadAsStringAsync();
        }
        #endregion

        #region 私有方法
        private string GenUrl(string url)
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
