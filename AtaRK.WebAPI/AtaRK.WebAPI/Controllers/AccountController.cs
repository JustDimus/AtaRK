using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models;
using AtaRK.BLL.Models.DTO;
using AtaRK.WebAPI.Authentication;
using AtaRK.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NLog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private readonly BLL.Interfaces.IAuthorizationService _authorizationService;

        public AccountController(
            IAccountService accountService,
            BLL.Interfaces.IAuthorizationService authorizationServices)
        {
            this._accountService = accountService;

            this._authorizationService = authorizationServices;
        }

        [HttpGet]
        [Authorize]
        [Route("info")]
        public async Task<IActionResult> GetInformation()
        {
            return Ok();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationVM registrationModel)
        {
            var registrationData = new AccountRegistrationData()
            {
                Credentials = new AccountCredentials()
                {
                    Email = registrationModel.Email,
                    Password = registrationModel.Password
                },
                Information = new AccountInformation()
                {
                    FirstName = registrationModel.FirstName,
                    SecondName = registrationModel.SecondName
                }
            };

            var serviceResult = await this._accountService.RegisterAsync(registrationData);

            if (serviceResult.IsSuccessful)
            {
                return CreateToken(serviceResult.Result) ?? Problem();
            }
            else
            {
                return Conflict();
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody]LoginVM loginModel)
        {
            var credentials = new AccountCredentials()
            {
                Email = loginModel.Email,
                Password = loginModel.Password
            };

            var serviceResult = await this._accountService.LoginAsync(credentials);

            if (serviceResult.IsSuccessful)
            {
                return this.CreateToken(serviceResult.Result) ?? Problem();
            }
            else
            {
                return Conflict();
            }
        }

        private IActionResult CreateToken(AuthorizationInfo authorizationInfo)
        {
            if (authorizationInfo == null)
            {
                this._logger.Error($"{nameof(authorizationInfo)} is null");
                return null;
            }

            string authorizationData = this._authorizationService.GetAccountIdentifier(authorizationInfo);

            var identity = this.GetIdentity(authorizationData);

            if (identity == null)
            {
                this._logger.Error($"Couldn't create identity token");
                return null;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt
            };

            return new JsonResult(response);
        }

        private ClaimsIdentity GetIdentity(string authorizationData)
        {
            if (authorizationData == null)
            {
                this._logger.Error($"{nameof(authorizationData)} is null");
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(AuthOptions.USER_AUTHORIZATION_DATA, authorizationData)
            };

            ClaimsIdentity identity = new ClaimsIdentity(
                claims: claims,
                authenticationType: "Token",
                nameType: ClaimsIdentity.DefaultNameClaimType,
                roleType: ClaimsIdentity.DefaultRoleClaimType);

            return identity;
        }
    }
}
