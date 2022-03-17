using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Groups;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class GroupController : AbstractEmployeePolicyController
    {

        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService, KokazContext context, IMapper mapper, Logging logging) : base(context, mapper, logging)
        {
            _groupService = groupService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDto>>> GetAll()
        {
            var groups = await this._context.Groups
                .Include(c => c.GroupPrivileges)
                .Include(c => c.UserGroups)
                .ThenInclude(c => c.User)
                .ToListAsync();
            var groupsDto = _mapper.Map<GroupDto[]>(groups);
            return Ok(groupsDto);
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
        public IActionResult Update([FromBody] UpdateGroupDto updateGroupDto)
        {
            try
            {
                var group = this._context.Groups.Find(updateGroupDto.Id);
                if (group == null)
                    return NotFound();
                var similer = this._context.Groups.Where(c => c.Id != updateGroupDto.Id && c.Name == updateGroupDto.Name).FirstOrDefault();
                if (similer != null)
                    return Conflict();
                group.Name = group.Name;
                this._context.Update(group);
                var groupPrivilges = this._context.GroupPrivileges.Where(c => c.GroupId == updateGroupDto.Id).ToList();
                foreach (var item in groupPrivilges)
                {
                    if (!updateGroupDto.Privileges.Contains(item.PrivilegId))
                    {
                        this._context.Remove(item);
                    }
                }
                foreach (var item in updateGroupDto.Privileges)
                {
                    if (!groupPrivilges.Select(c => c.PrivilegId).Contains(item))
                    {
                        this._context.Add(new GroupPrivilege()
                        {
                            GroupId = updateGroupDto.Id,
                            PrivilegId = item

                        });
                    }
                }
                this._context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }


    }
}