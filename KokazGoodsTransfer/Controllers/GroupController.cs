using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Groups;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("EnableCORS")]
    public class GroupController : Controller
    {
        KokazContext Context;
        public GroupController(KokazContext context)
        {
            this.Context = context;
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var group = this.Context.Groups
                .Find(id);
            if (group == null)
                return Conflict();
            var totalPrivilegesCount = this.Context.Privileges.Count();
            if (group.GroupPrivileges.Count() == totalPrivilegesCount)
            {
                var anotherGroups = this.Context.Groups
                    .Include(c=>c.GroupPrivileges)
                    .Where(c => c.Id != id).ToList();
                anotherGroups = anotherGroups.Where(c => c.GroupPrivileges.Count() == totalPrivilegesCount).ToList();
                if (anotherGroups.Count == 0)
                    return Conflict();
            }
            this.Context.Groups.Remove(group);
            this.Context.Remove(group);
            this.Context.SaveChanges();
            return Ok();
        }
        
        [HttpPost]
        public IActionResult Creat(CreateGroupDto createGroupDto)
        {
            var similerGroup = this.Context.Groups.Where(c => c.Name == createGroupDto.Name).FirstOrDefault();
            if (similerGroup != null)
                return Conflict();
            Group group = new Group();
            group.Name = createGroupDto.Name;
            Context.Add(group);
            foreach (var item in createGroupDto.PrivilegesId)
            {
                group.GroupPrivileges.Add(new GroupPrivilege()
                {
                    GroupId = group.Id,
                    PrivilegId = item
                });
            }
            Context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            
            var groups = this.Context.Groups
                .Include(c=>c.GroupPrivileges)
                .ToList();
            var groupsDto = new List<GroupDto>();
            foreach (var item in groups)
            {
                GroupDto groupDto = new GroupDto()
                {
                    Id = item.Id,
                    Name = item.Name,
                    PrivilegesId = item.GroupPrivileges.Select(c => c.PrivilegId).ToList()
                };
                groupsDto.Add(groupDto);
            }
            return Ok(groupsDto);
        }
       
        [HttpGet("Privileges")]
        public IActionResult GetAlL()
        {
            var privileges= this.Context.Privileges.ToList();
            List<PrivilegeDto> privilegeDtos = new List<PrivilegeDto>();
            foreach (var item in privileges)
            {
                privilegeDtos.Add(new PrivilegeDto()
                {
                    Id= item.Id,
                    Name=  item.Name
                });
            }
            return Ok(privilegeDtos);
        }
        [HttpPatch]
        public IActionResult Update([FromBody] UpdateGroupDto updateGroupDto)
        {
            var group= this.Context.Groups.Find(updateGroupDto.Id);
            if (group == null)
                return NotFound();
            var similer = this.Context.Groups.Where(c => c.Id != updateGroupDto.Id && c.Name == updateGroupDto.Name).FirstOrDefault();
            if (similer != null)
                return Conflict();
            group.Name = group.Name;
            group.GroupPrivileges.Clear();
            foreach (var item in updateGroupDto.Privileges)
            {
                group.GroupPrivileges.Add(new GroupPrivilege()
                {
                    GroupId = group.Id,
                    PrivilegId = item
                });
            }
            this.Context.SaveChanges();
            return Ok();
        }

    }
}