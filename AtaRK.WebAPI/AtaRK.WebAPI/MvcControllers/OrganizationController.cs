using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.MvcControllers
{
    public class OrganizationController : Controller
    {
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            throw new NotImplementedException();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
