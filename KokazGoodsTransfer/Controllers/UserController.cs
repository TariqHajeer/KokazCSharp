using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KokazGoodsTransfer.Dtos.Users;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        KokazContext Context;
        public UserController(KokazContext context)
        {
            this.Context = context;
        }
        [HttpPost]
        public IActionResult Create([FromBody]CreateUserDto createUserDto)
        {
            User user = new User()
            {
                Name = createUserDto.Name,
                Adress = createUserDto.Address,
                Experince = createUserDto.Experince,
                DepartmentId = createUserDto.DepartmentId,
                HireDate = createUserDto.HireDate,
                Note = createUserDto.Note,
                UserName = createUserDto.UserName,
                Password = createUserDto.Password,
                CanWorkAsAgent = createUserDto.CanWorkAsAgent,
                Salary = createUserDto.Salary,
                CountryId = createUserDto.CountryId,
            };
            Context.Add(user);

            foreach (var item in createUserDto.GroupsId)
            {
                user.UserGroups.Add(new UserGroup()
                {
                    UserId = user.Id, 
                    GroupId = item
                });
            }
            foreach (var item in createUserDto.Phones)
            {
                user.UserPhones.Add(new UserPhone()
                {
                    Phone = item,
                    UserId = user.Id
                });
            }
            Context.SaveChanges();
            return Ok();
        }
    }
}