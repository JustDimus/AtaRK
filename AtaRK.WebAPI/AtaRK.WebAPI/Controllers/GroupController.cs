using AtaRK.BLL.Interfaces;
using AtaRK.BLL.Models.DTO;
using AtaRK.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AtaRK.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(
            IGroupService groupService)
        {
            this._groupService = groupService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody]CreateGroupVM group)
        {
            var groupInfo = new GroupCreationInfo()
            {
                GroupName = group.Name
            };

            var serviceResult = await this._groupService.CreateGroupAsync(groupInfo);

            if (serviceResult.IsSuccessful)
            {
                return Ok();
            }
            else
            {
                return Conflict();
            }
        }
    }
}
