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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Js()
        {
            return View();
        }

        public IActionResult Html()
        {
            ViewBag.LoginUrl = _OidcClient.GetLoginUrl();

            return View();
        }

        public IActionResult Custom()
        {
            ViewBag.Client = _OidcClient;

            return View();
        }

        public async Task<IActionResult> Result(string code)
        {
            var userInfo = await _OidcClient.AccessToken(code);

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
