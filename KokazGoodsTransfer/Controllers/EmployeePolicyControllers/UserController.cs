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
        [HttpGet("Agent")]
        public IActionResult GetAgent()
        {
            var users = this.Context.Users.Where(c => c.CanWorkAsAgent == true).ToList();
            return Ok(mapper.Map<UserDto[]>(users));
        }
        [HttpPost]

        public IActionResult Create([FromBody]CreateUserDto createUserDto)
        {
            if (!createUserDto.CanWorkAsAgent)
            {
                var similerUser = this.Context.Users.Where(c => c.UserName.ToLower() == createUserDto.UserName.ToLower()).Count();
                if (similerUser != 0)
                    return Conflict();
            }
            {
                var similerUser = this.Context.Users.Where(c => c.Name.ToLower() == createUserDto.Name.ToLower()).Count();
                if (similerUser != 0)
                    return Conflict();
            }

            User user = new User()
            {
                Name = createUserDto.Name,
                Adress = createUserDto.Address,
                Experince = createUserDto.Experince,
                HireDate = createUserDto.HireDate,
                Note = createUserDto.Note,
                CanWorkAsAgent = createUserDto.CanWorkAsAgent,
                Salary = createUserDto.Salary,
                CountryId = createUserDto.CountryId,
            };
            if (!createUserDto.CanWorkAsAgent)
            {
                user.UserName = createUserDto.UserName;
                user.Password = MD5Hash.GetMd5Hash(createUserDto.Password);
            }
            Context.Add(user);
            if (createUserDto.GroupsId != null)
                foreach (var item in createUserDto.GroupsId)
                {
                    user.UserGroups.Add(new UserGroup()
                    {
                        UserId = user.Id,
                        GroupId = item
                    });
                }
            if (createUserDto.Phones != null)
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
                .Include(c => c.UserGroups)
                .ToList();
            return Ok(mapper.Map<UserDto[]>(users));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = this.Context.Users.Include(c => c.UserPhones)
                .Include(c => c.UserGroups)
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
        [HttpPatch]
        public IActionResult UpdateUser([FromBody]UpdateUserDto updateUserDto)
        {
            var user = this.Context.Users.Find(updateUserDto.Id);
            user.Adress = updateUserDto.Address;
            user.Name = updateUserDto.Name;
            user.HireDate = updateUserDto.HireDate;
            user.Note = updateUserDto.Note;
            {
                var similerUserByname = this.Context.Users.Where(c => c.Name.ToLower() == updateUserDto.Name.ToLower()&&c.Id!=updateUserDto.Id).Count();
                if (similerUserByname != 0)
                {
                    return Conflict();
                }
            }
            if (updateUserDto.CanWorkAsAgent)
            {
                user.UserName = string.Empty;
                user.Password = string.Empty;
                user.UserGroups.Clear();
                user.CanWorkAsAgent = true;
                user.CountryId = updateUserDto.CountryId;
                user.Salary = updateUserDto.Salary;
            }
            else
            {
                var similerUserByname = this.Context.Users.Where(c => c.UserName.ToLower() == updateUserDto.UserName.ToLower()&&c.Id!=updateUserDto.Id).Count();
                if (similerUserByname != 0)
                {
                    return Conflict();
                }
                user.UserName = updateUserDto.UserName;
                user.Password = MD5Hash.GetMd5Hash(updateUserDto.Password);
                user.CountryId = null;
                user.Salary = null;
            }
            this.Context.Update(user);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpGet("UsernameExist/{username}")]
        public IActionResult UsernameExist(string username)
        {
            return Ok(this.Context.Users.Where(c => c.UserName == username).Count()!=0);
        }
        

    }
}