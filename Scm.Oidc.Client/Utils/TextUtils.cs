using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Com.Scm.Utils
{
    public class TextUtils
    {
        private static JsonSerializerSettings _Settings = new JsonSerializerSettings();

        public static string ToJsonString(object obj, bool indented = false)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj is string)
            {
                return obj as string;
            }
            _Settings.Formatting = indented ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(obj, _Settings);
        }

        public static T AsJsonObject<T>(string text) where T : new()
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(text);
        }

        /// <summary>
        /// 是否为中国的手机号码，格式如下：
        /// 130XXXXXXXX
        /// 199XXXXXXXX
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsCellphone(string s)
        {
            return Regex.IsMatch(s, "^1[3-9]\\d{9}$");
        }

        /// <summary>
        /// 是否为中国的固话号码，格式如下：
        /// XXXX-XXXXXXXX
        /// XXX-XXXXXXXX
        /// XXXX-XXXXXXX
        /// (XXXX)XXXXXXXX
        /// (XXXX)XXXXXXX
        /// (XXX)XXXXXXXX
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsTelephone(string s)
        {
            return Regex.IsMatch(s, @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$");
        }

        /// <summary>
        /// 是否为中的电话号码（含手机和固话）
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsPhone(string s)
        {
            return IsTelephone(s) || IsCellphone(s);
        }

        /// <summary>
        /// 判断是否为合法的电子邮件，支持格式如下：
        /// someone@host.com
        /// some.one@host.com.cn
        /// some_one@host.com.cn
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsMail(string s)
        {
            return Regex.IsMatch(s, @"^\w+[-.\w]*@\w+[-\w]*(\.\w+[-\w])+$");
        }

        /// <summary>
        /// 基于时间的字符串
        /// </summary>
        /// <param name="upperCase">是否大写</param>
        /// <returns></returns>
        public static string TimeString(bool upperCase = false)
        {
            var format = upperCase ? "X" : "x";
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(format + "16");
        }

        /// <summary>
        /// 随机数字串
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomNumber(int length = 10)
        {
            string chars = "0123456789";
            var content = new char[length];
            var random = new Random();
            for (var i = 0; i < length; i++)
            {
                content[i] = chars[random.Next(chars.Length)];
            }

            return new string(content);
        }

        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <param name="length"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static string RandomString(int length = 10, bool upper = true)
        {
            string chars = upper ? "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ" : "0123456789abcdefghijklmnopqrstuvwxyz";
            var content = new char[length];
            var random = new Random();
            for (var i = 0; i < length; i++)
            {
                content[i] = chars[random.Next(chars.Length)];
            }

            return new string(content);
        }

        /// <summary>
        /// 转换为16进度字符串
        /// </summary>
        /// <param name="bytes">待转换数组</param>
        /// <param name="upperCase">是否为大写</param>
        /// <returns></returns>
        public static string ToHexString(byte[] bytes, bool upperCase = false)
        {
            var format = upperCase ? "X2" : "x2";
            var builder = new StringBuilder();
            foreach (var b in bytes)
            {
                builder.Append(b.ToString(format));
            }
            return builder.ToString();
        }

        public static string Md5(string text, bool upperCase = false)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return "";
            }

            var alg = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var result = alg.ComputeHash(bytes);

            return ToHexString(result, upperCase);
        }
    }
}
