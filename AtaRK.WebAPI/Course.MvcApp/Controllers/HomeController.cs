using Course.MvcApp.ApiServices;
using Course.MvcApp.Models.MvcModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Course.MvcApp.MvcContorllers
{
    public class HomeController : Controller
    {
        private readonly IAccountApiService _accountService;

        public HomeController(IAccountApiService accountService)
        {
            _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel loginModel)
        {
            var result = await this._accountService.LoginAsync(loginModel);

            if (result == null)
            {
                this.ModelState.AddModelError(
                    "Invalid data",
                    "email or password is incorrect");

                return this.View(loginModel);
            }

            await this.AuthenticateAsync(result.AccessToken);

            return this.RedirectToAction("List", "Organization");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return this.RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromForm] RegistrationModel registrationModel)
        {
            var result = await this._accountService.RegisterAsync(registrationModel);

            if (result == null)
            {
                this.ModelState.AddModelError(
                    "Invalid data",
                    "email or password is incorrect");

                return this.View(registrationModel);
            }

            await this.AuthenticateAsync(result.AccessToken);

            return this.RedirectToAction("List", "Organization");
        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        private async Task AuthenticateAsync(string token)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Thumbprint, token)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(principal);
        }
    }
}
