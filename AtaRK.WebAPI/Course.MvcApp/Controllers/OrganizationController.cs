using Course.MvcApp.ApiServices;
using Course.MvcApp.Models.MvcModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Course.MvcApp.MvcControllers
{
    public class OrganizationController : Controller
    {
        private readonly IOrganizationApiService _organizationApiService;

        public OrganizationController(
            IOrganizationApiService organizationApiService)
        {
            this._organizationApiService = organizationApiService;
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List()
        {
            var organizationsResult = await this._organizationApiService.GetCurrentUserOrganizations();

            return this.View(organizationsResult);
        }

        [HttpGet]
        [Route("Invites")]
        public async Task<IActionResult> Invites()
        {
            var operationResult = await this._organizationApiService.GetCurrentUserInvitations();

            return View(operationResult);
        }

        [HttpGet]
        [Route("Invite")]
        public IActionResult Invite([FromQuery] string groupId)
        {
            var createGroupModel = new CreateInviteModel()
            {
                GroupId = groupId,
                Email = string.Empty
            };

            return View(createGroupModel);
        }

        [HttpPost]
        [Route("Invite")]
        public async Task<IActionResult> Invite([FromForm] CreateInviteModel createInviteModel)
        {
            var result = await this._organizationApiService.InviteUser(createInviteModel.GroupId, createInviteModel.Email);

            if (!result)
            {
                return View(createInviteModel);
            }

            return RedirectToAction("View", new { organizationId = createInviteModel.GroupId });
        }

        [HttpGet]
        [Route("OperateInvite")]
        public async Task<IActionResult> OperateInvite(
            [FromQuery] bool doAccept,
            [FromQuery] string inviteId)
        {
            var operationResult = await this._organizationApiService.OperateInvite(inviteId, doAccept);

            return this.RedirectToAction("Invites");
        }

        [HttpGet]
        [Route("View")]
        public async Task<IActionResult> View([FromQuery] string organizationId)
        {
            var organizationInfo = await this._organizationApiService.GetOrganizationInfo(organizationId);

            return View(organizationInfo);
        }

        [HttpGet]
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromForm] OrganizationModel organizationModel)
        {
            var result = await this._organizationApiService.CreateNewOrganization(organizationModel);

            if (!result)
            {
                this.ModelState.AddModelError(
                    "Invalid operation",
                    "Organization with the same name already exists");

                return this.View(organizationModel);
            }

            return this.RedirectToAction("List");
        }
    }
}
