using System;
using System.Linq;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KokazGoodsTransfer.Dtos.Common;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AbstractEmployeePolicyController
    {
        public UserController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpPost]

        public IActionResult Create([FromBody]CreateUserDto createUserDto)
        {
            var similerUser = this.Context.Users.Where(c => c.UserName.ToLower() == createUserDto.UserName.ToLower() || c.Name.ToLower() == createUserDto.Name).Count();
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

            return Ok(mapper.Map<UserDto>(user));
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = this.Context.Users
                .Include(c => c.UserPhones)
                .Include(c => c.Department)
                .ToList();
            return Ok(mapper.Map<UserDto[]>(users));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = this.Context.Users.Include(c => c.UserPhones)
                .Include(c => c.Department)
                .FirstOrDefault(c => c.Id == id);
            return Ok(mapper.Map<UserDto>(user));
        }
        [HttpPut("AddPhone")]
        public IActionResult AddPhone([FromBody]AddPhoneDto addPhoneDto)
        {
            try
            {
                var user = this.Context.Users.Find(addPhoneDto.objectId);
                if (user == null)
                    return NotFound();
                this.Context.Entry(user).Collection(c => c.UserPhones).Load();
                if (user.UserPhones.Where(c => c.Phone == addPhoneDto.Phone).Any())
                    return Conflict();
                var userPhone = new UserPhone()
                {
                    UserId = user.Id,
                    Phone = addPhoneDto.Phone
                };
                this.Context.Add(userPhone);
                this.Context.SaveChanges();
                return Ok(mapper.Map<PhoneDto>(userPhone));
                //return Ok(mapper.Map<UserPhone>);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("deletePhone/{id}")]
        public IActionResult DeletePhone(int id)
        {
            try
            {
                var userPhone = this.Context.UserPhones.Find(id);
                if (userPhone == null)
                    return Conflict();
                this.Context.Remove(userPhone);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("deleteGroup/{userId}")]
        public IActionResult DelteGroup(int userId, [FromForm] int groupId)
        {
            try
            {
                var userGroup = this.Context.UserGroups.Where(c => c.UserId == userId && c.GroupId == groupId).FirstOrDefault();
                if (userGroup == null)
                    return Conflict();
                this.Context.Remove(userGroup);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("AddToGroup/{userId}")]
        public IActionResult AddToGroup(int userId, [FromForm] int groupId)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        //[HttpPatch]
        //public IActionResult UpdateUser([FromBody] )

    }
}