using FullStackApplication.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace FullStackApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApiService _apiService;

        public LoginController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var response = await _apiService.PostAsync<LoginResponse, LoginRequest>("api/auth/login", request);
            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                HttpContext.Session.SetString("JWTToken", response.Token);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View("Index");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JWTToken");
            return RedirectToAction("Index", "Home");
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
    }
}
