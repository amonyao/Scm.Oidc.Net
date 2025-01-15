using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Com.Scm.Utils
{
    public class HttpUtils
    {
        /// <summary>
        /// 默认用户代理字符串
        /// </summary>
        public static string DEFAULT_USER_AGENT = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36 Edg/87.0.664.66";

        /// <summary>
        /// 内部记录日志
        /// </summary>
        /// <param name="msg"></param>
        private static void DebugLog(string msg)
        {
            LogUtils.Info(msg);
        }

        public static string BuildUrl(Dictionary<string, string> paramList, string url = null)
        {
            if (paramList == null)
            {
                return url;
            }

            var builder = new StringBuilder();
            foreach (var key in paramList.Keys)
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    continue;
                }

                var val = paramList[key];
                if (string.IsNullOrEmpty(val))
                {
                    continue;
                }
                builder.Append('&').Append(key).Append('=').Append(val);
            }
            if (builder.Length > 0)
            {
                builder.Remove(0, 1);
            }

            if (url == null)
            {
                url = "";
            }

            url += url.IndexOf("?") < 0 ? "?" : "&";
            return url + builder.ToString();
        }

        /// <summary>
        /// 创建 HttpClient
        /// </summary>
        /// <returns></returns>
        public static HttpClient CreateHttpClient()
        {
            return new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                ClientCertificateOptions = ClientCertificateOption.Automatic,
                ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true
            });
        }

        /// <summary>
        /// 移除空值项
        /// </summary>
        /// <param name="dict"></param>
        public static void RemoveEmpty(Dictionary<string, string> dict)
        {
            dict.Where(item => string.IsNullOrEmpty(item.Value)).Select(item => item.Key).ToList().ForEach(key =>
            {
                dict.Remove(key);
            });
        }

        /// <summary>
        /// 转换为查询字符串
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="urlEncode"></param>
        /// <returns></returns>
        public static string ToQueryString(Dictionary<string, string> dict, bool urlEncode = true)
        {
            return string.Join("&", dict.Select(p => $"{(urlEncode ? UrlEncode(p.Key) : p.Key)}={(urlEncode ? UrlEncode(p.Value) : p.Value)}"));
        }

        public static Dictionary<string, string> ToDictionary(string text)
        {
            var dict = new Dictionary<string, string>();
            var kvItems = text.Split('&');
            foreach (var kvItem in kvItems)
            {
                var items = kvItem.Split('=');
                if (items.Length == 2)
                {
                    var key = items[0];
                    var value = items[1];
                    dict.Add(key, value);
                }
            }
            return dict;
        }

        public static T Query2Object<T>(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return default;
            }

            var type = typeof(T);
            var obj = (T)Assembly.GetAssembly(type).CreateInstance(type.FullName);
            if (obj == null)
            {
                return obj;
            }

            var kvItems = text.Split('&');
            foreach (var kvItem in kvItems)
            {
                var items = kvItem.Split('=');
                if (items.Length == 2)
                {
                    var key = items[0];
                    var value = items[1];

                    var p = type.GetProperty(key);
                    if (p != null)
                    {
                        var o = Convert.ChangeType(value, p.PropertyType);
                        p.SetValue(obj, o, null);
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// 异步 GET 请求API，返回字符串
        /// </summary>
        /// <param name="api"></param>
        /// <param name="query"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static async Task<string> GetStringAsync(string api, Dictionary<string, string> query = null, Dictionary<string, string> header = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                if (query == null)
                {
                    query = new Dictionary<string, string>();
                }
                RemoveEmpty(query);
                api = $"{api}{(api.Contains("?") ? "&" : "?")}{ToQueryString(query)}";
                DebugLog($"GET [{api}]");
                if (header == null)
                {
                    header = new Dictionary<string, string>();
                }
                if (!header.ContainsKey("accept"))
                {
                    header.Add("accept", "application/json");
                }
                if (!header.ContainsKey("User-Agent"))
                {
                    header.Add("User-Agent", DEFAULT_USER_AGENT);
                }
                var key = "Content-Type";
                if (header.ContainsKey(key))
                {
                    var contentType = header[key];
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, contentType);
                    header.Remove(key);
                }
                foreach (var headerItem in header)
                {
                    httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    DebugLog($"GET Header [{headerItem.Key}]=[{headerItem.Value}]");
                }
                var response = await httpClient.GetAsync(api);
                var responseText = await response.Content.ReadAsStringAsync();
                DebugLog($"GET [{api}], reponse=[{responseText}]");
                return responseText;
            }
        }

        /// <summary>
        /// 异步 GET 请求API，返回反序列化后的类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="api"></param>
        /// <param name="query"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(string api, Dictionary<string, string> query = null, Dictionary<string, string> header = null) where T : new()
        {
            return TextUtils.AsJsonObject<T>(await GetStringAsync(api, query, header));
        }

        /// <summary>
        /// 异步 POST 请求API，返回字符串
        /// </summary>
        /// <param name="api"></param>
        /// <param name="form"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static async Task<string> PostFormStringAsync(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                if (form == null)
                {
                    form = new Dictionary<string, string>();
                }
                RemoveEmpty(form);
                DebugLog($"POST [{api}]");
                foreach (var item in form)
                {
                    DebugLog($"POST [{item.Key}]=[{item.Value}]");
                }
                if (header == null)
                {
                    header = new Dictionary<string, string>();
                }
                if (!header.ContainsKey("accept"))
                {
                    header.Add("accept", "application/json");
                }
                if (!header.ContainsKey("User-Agent"))
                {
                    header.Add("User-Agent", DEFAULT_USER_AGENT);
                }
                var key = "Content-Type";
                if (header.ContainsKey(key))
                {
                    var contentType = header[key];
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, contentType);
                    header.Remove(key);
                }
                foreach (var headerItem in header)
                {
                    httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    DebugLog($"POST Header [{headerItem.Key}]=[{headerItem.Value}]");
                }
                var response = await httpClient.PostAsync(api, new FormUrlEncodedContent(form));
                var responseText = await response.Content.ReadAsStringAsync();
                DebugLog($"POST [{api}], reponse=[{responseText}]");
                return responseText;
            }
        }

        /// <summary>
        /// 异步 POST 请求API，并将 form 字段序列化为 json，放在请求体内。返回字符串
        /// </summary>
        /// <param name="api"></param>
        /// <param name="form"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        public static async Task<string> PostJsonStringAsync(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                if (form == null)
                {
                    form = new Dictionary<string, string>();
                }
                RemoveEmpty(form);
                DebugLog($"POST [{api}]");
                var jsonBody = TextUtils.ToJsonString(form);
                DebugLog($"POST Body=[{jsonBody}]");
                if (header == null)
                {
                    header = new Dictionary<string, string>();
                }
                if (!header.ContainsKey("accept"))
                {
                    header.Add("accept", "application/json");
                }
                if (!header.ContainsKey("User-Agent"))
                {
                    header.Add("User-Agent", DEFAULT_USER_AGENT);
                }
                var key = "Content-Type";
                if (header.ContainsKey(key))
                {
                    header.Remove(key);
                }
                foreach (var headerItem in header)
                {
                    httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    DebugLog($"POST Header [{headerItem.Key}]=[{headerItem.Value}]");
                }
                var response = await httpClient.PostAsync(api, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                var responseText = await response.Content.ReadAsStringAsync();
                DebugLog($"POST [{api}], reponse=[{responseText}]");
                return responseText;
            }
        }

        public static async Task<string> PostJsonBodyAsync(string api, string json = null, Dictionary<string, string> header = null)
        {
            using (var httpClient = CreateHttpClient())
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                DebugLog($"POST [{api}]");
                DebugLog($"POST Body=[{json}]");
                if (header == null)
                {
                    header = new Dictionary<string, string>();
                }
                if (!header.ContainsKey("accept"))
                {
                    header.Add("accept", "application/json");
                }
                if (!header.ContainsKey("User-Agent"))
                {
                    header.Add("User-Agent", DEFAULT_USER_AGENT);
                }
                var key = "Content-Type";
                if (header.ContainsKey(key))
                {
                    header.Remove(key);
                }
                foreach (var headerItem in header)
                {
                    httpClient.DefaultRequestHeaders.Add(headerItem.Key, headerItem.Value);
                    DebugLog($"POST Header [{headerItem.Key}]=[{headerItem.Value}]");
                }
                var response = await httpClient.PostAsync(api, new StringContent(json, Encoding.UTF8, "application/json"));
                var responseText = await response.Content.ReadAsStringAsync();
                DebugLog($"POST [{api}], reponse=[{responseText}]");
                return responseText;
            }
        }

        public static async Task<T> PostFormObjectAsync<T>(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null) where T : new()
        {
            return TextUtils.AsJsonObject<T>(await PostFormStringAsync(api, form, header));
        }

        public static async Task<T> PostJsonObjectAsync<T>(string api, Dictionary<string, string> form = null, Dictionary<string, string> header = null) where T : new()
        {
            return TextUtils.AsJsonObject<T>(await PostJsonStringAsync(api, form, header));
        }

        public static async Task<T> PostJsonObjectAsync<T>(string api, string json = null, Dictionary<string, string> header = null) where T : new()
        {
            return TextUtils.AsJsonObject<T>(await PostJsonBodyAsync(api, json, header));
        }

        #region 编码转换
        /// <summary>
        /// unescape
        /// </summary>
        /// <returns></returns>
        public static string Unescape(string s)
        {
            string str = s.Remove(0, 2);//删除最前面两个＂%u＂
            string[] strArr = str.Split(new string[] { "%u" }, StringSplitOptions.None);//以子字符串＂%u＂分隔
            byte[] byteArr = new byte[strArr.Length * 2];
            for (int i = 0, j = 0; i < strArr.Length; i++, j += 2)
            {
                byteArr[j + 1] = Convert.ToByte(strArr[i].Substring(0, 2), 16);  //把十六进制形式的字串符串转换为二进制字节
                byteArr[j] = Convert.ToByte(strArr[i].Substring(2, 2), 16);
            }
            str = System.Text.Encoding.Unicode.GetString(byteArr); //把字节转为unicode编码
            return str;
        }

        /// <summary>
        /// Escape
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Escape(string s)
        {
            var sb = new StringBuilder();
            foreach (char c in s)
            {
                sb.Append((char.IsLetterOrDigit(c)
                || c == '-' || c == '_' || c == '\\'
                || c == '/' || c == '.') ? c.ToString() : Uri.HexEscape(c));
            }
            return sb.ToString();
        }

        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return System.Web.HttpUtility.UrlEncode(str, Encoding.UTF8);
        }

        public static string ToBase64String(byte[] input)
        {
            return Convert.ToBase64String(input).Replace("+", "-").Replace("/", "_");
        }

        public static byte[] FromBase64String(string input)
        {
            return Convert.FromBase64String(input.Replace("-", "+").Replace("_", "/"));
        }
        #endregion
    }
}
