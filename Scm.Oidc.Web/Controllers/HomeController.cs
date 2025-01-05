using Com.Scm.Oidc.Response;
using Com.Scm.Response;
using Microsoft.AspNetCore.Mvc;
using Scm.Oidc.Web.Models;
using System.Diagnostics;

namespace Com.Scm.Oidc.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private OidcClient _OidcClient;

        public HomeController(ILogger<HomeController> logger, OidcClient client)
        {
            _logger = logger;
            _OidcClient = client;
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        #region Html示例
        /// <summary>
        /// HTML示例视图
        /// </summary>
        /// <returns></returns>
        public IActionResult Html()
        {
            ViewBag.LoginUrl = _OidcClient.GetWebUrl();

            return View();
        }
        #endregion

        #region Javascript示例
        /// <summary>
        /// Javascript示例视图
        /// </summary>
        /// <returns></returns>
        public IActionResult Js()
        {
            return View();
        }
        #endregion

        #region 定制化示例
        /// <summary>
        /// 定制化示例视图
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Custom()
        {
            ViewBag.OspList = await _OidcClient.ListAppOspAsync();

            return View();
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="type"></param>
        /// <param name="code"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("home/sendSms")]
        public async Task<SendSmsResponse> SendSmsAsync(OidcSmsEnums type, string code, string key)
        {
            return await _OidcClient.SendSmsAsync(type, code, key);
        }

        /// <summary>
        /// 校验验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="key"></param>
        /// <param name="sms"></param>
        /// <returns></returns>
        [HttpGet("home/verifySms")]
        public async Task<VerifySmsResponse> VerifySmsAsync(string code, string key, string sms)
        {
            return await _OidcClient.VerifySmsAsync(code, key, sms);
        }
        #endregion

        #region OAuth登录
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ospCode">服务商代码</param>
        /// <param name="state">附加参数，可选</param>
        /// <returns></returns>
        [HttpGet("home/login")]
        public LoginResponse LoginAsync(string ospCode, string state = null)
        {
            return null;// _OidcClient.GetLoginUrl(ospCode, state);
        }
        #endregion

        #region 授权回调
        /// <summary>
        /// 授权回调视图
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IActionResult> Result(string code)
        {
            var response = await _OidcClient.AccessTokenAsync(code);
            if (!response.IsSuccess())
            {
                return ReturnToError(response.GetMessage());
            }

            ViewBag.User = response.User;
            return View();
        }
        #endregion

        #region 错误相关
        /// <summary>
        /// 错误页面
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string error)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, Error = error });
        }

        /// <summary>
        /// 转向错误页面
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        protected IActionResult ReturnToError(string error)
        {
            return RedirectToAction("Error", "Home", new { error });
        }
        #endregion
    }
}
