using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Dyspozytornia.Data;
using Dyspozytornia.Models;
using Dyspozytornia.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dyspozytornia.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController()
        {
            _loginService = new LoginService(new LoginRepository());
        }

        [Route("/login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/login")]
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User loginUser)
        {
            if (!loginUser.isValidLogin()) return View();

            if (await _loginService.Authenticate(loginUser.UserName, loginUser.UserPassword))
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, "MyUserNameOrID"),
                    new Claim(ClaimTypes.Role, "SomeRoleName")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [Route("/register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [Route("/register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(User newUser)
        {
            if (ModelState.IsValid)
                if (await _loginService.Create(newUser))
                    return RedirectToAction("Index", "Home");
            return RedirectToAction("Register", "Login");
        }

        [Route("/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("/account")]
        [Authorize]
        public IActionResult Account()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}