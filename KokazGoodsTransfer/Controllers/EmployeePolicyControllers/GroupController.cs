using System.Collections.Generic;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Groups;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GroupController : AbstractEmployeePolicyController
    {

        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetAll()
        {
            var groups = await this._groupService.GetAll(new string[] { "GroupPrivileges", "UserGroups.User" });
            return Ok(groups);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _groupService.Delete(id);
            if (!response.Sucess)
                return Conflict();
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Creat(CreateGroupDto createGroupDto)
        {
            var res = await _groupService.AddAsync(createGroupDto);
            if (!res.Sucess)
                return Conflict();
            return Ok(res.Data);
        }


        [HttpGet("Privileges")]
        public async Task<ActionResult<IEnumerable<PrivilegeDto>>> GetPrivileges()
        {
            return Ok(await _groupService.GetPrivileges());
        }
        [HttpPatch]
        public async Task<ActionResult<GroupDto>> Update([FromBody] UpdateGroupDto updateGroupDto)
        {
            var result = await _groupService.Update(updateGroupDto);
            if (result.Sucess)
                return Ok(result.Data);
            if (result.NotFound)
                return NotFound();
            return Conflict(result);
        }


    }
}