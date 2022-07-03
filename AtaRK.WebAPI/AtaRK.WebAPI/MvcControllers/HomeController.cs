using AtaRK.WebAPI.Models.MvcModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.MvcContorllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel loginModel)
        {

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration([FromForm] RegistrationModel registrationModel)
        {

        }

        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
