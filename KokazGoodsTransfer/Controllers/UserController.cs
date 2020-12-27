using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AbstractController
    {
        public UserController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpPost]
        public IActionResult Create([FromBody]CreateUserDto createUserDto)
        {
            var similerUser = this.Context.Users.Where(c => c.UserName.ToLower() == createUserDto.UserName.ToLower()).Count();
            if (similerUser != 0)
                return Conflict();
            User user = new User()
            {
                Name = createUserDto.Name,
                Adress = createUserDto.Address,
                Experince = createUserDto.Experince,
                DepartmentId = createUserDto.DepartmentId,
                HireDate = createUserDto.HireDate,
                Note = createUserDto.Note,
                UserName = createUserDto.UserName,
                Password = MD5Hash.GetMd5Hash(createUserDto.Password),
                CanWorkAsAgent = createUserDto.CanWorkAsAgent,
                Salary = createUserDto.Salary,
                CountryId = createUserDto.CountryId,
            };
            Context.Add(user);

            foreach (var item in createUserDto.GroupsId)
            {
                user.UserGroups.Add(new UserGroup()
                {
                    UserId = user. Id,
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

            return Ok(mapper.Map<UserDto>(user));
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = this.Context.Users
                .Include(c=>c.UserPhones)
                .Include(c=>c.Department)
                .ToList();
            return Ok(mapper.Map<UserDto[]>(users));
        }
    }
}