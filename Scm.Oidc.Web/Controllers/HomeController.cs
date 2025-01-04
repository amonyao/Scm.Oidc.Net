using Com.Scm.Oidc.Response;
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
        /// 首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Javascript示例
        /// </summary>
        /// <returns></returns>
        public IActionResult Js()
        {
            return View();
        }

        /// <summary>
        /// HTML示例
        /// </summary>
        /// <returns></returns>
        public IActionResult Html()
        {
            ViewBag.LoginUrl = _OidcClient.GetLoginUrl();

            return View();
        }

        /// <summary>
        /// 定制化示例
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Custom()
        {
            ViewBag.OspList = await _OidcClient.ListAppOspAsync();

            return View();
        }

        [HttpGet("home/sendSms")]
        public async Task<SendSmsResponse> SendSmsAsync(OidcSmsEnums type, string code, string key)
        {
            return await _OidcClient.SendSmsAsync(type, code, key);
        }

        [HttpGet("home/checkSms")]
        public async Task<SendSmsResponse> CheckSmsAsync(string code, string key, string sms)
        {
            return await _OidcClient.VerifySmsAsync(code, key, sms);
        }

        /// <summary>
        /// 授权回调
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
    }
}
