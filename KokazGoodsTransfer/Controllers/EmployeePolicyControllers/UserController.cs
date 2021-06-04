using System;
using System.Linq;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models.Static;

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
            var users = this.Context.Users.Where(c => c.CanWorkAsAgent == true)
                .Include(c => c.AgentCountrs)
                    .ThenInclude(c => c.Country)
                .ToList();
            return Ok(mapper.Map<UserDto[]>(users));
        }
        [HttpGet("ActiveAgent")]
        public IActionResult GetEnalbedAgent()
        {
            var users = this.Context.Users.Where(c => c.CanWorkAsAgent == true && c.IsActive == true)
                .Include(c=>c.UserPhones)
                .Include(c=>c.AgentCountrs)
                    .ThenInclude(c=>c.Country)
                .ToList();
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
                IsActive = true,
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
            if (createUserDto.CanWorkAsAgent)
            {
                foreach (var item in createUserDto.Countries)
                {
                    user.AgentCountrs.Add(new AgentCountr()
                    {
                        CountryId = item,
                        AgentId = user.Id
                    });
                }
            }
            Context.SaveChanges();
            this.Context.Entry(user).Collection(c => c.AgentCountrs).Load();
            foreach (var item in user.AgentCountrs)
            {
                this.Context.Entry(item).Reference(c => c.Country).Load();
            }
            return Ok(mapper.Map<UserDto>(user));
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = this.Context.Users
                .Include(c => c.UserPhones)
                .Include(c => c.UserGroups)
                .Include(c=>c.AgentCountrs)
                    .ThenInclude(c=>c.Country)
                .ToList();
            var usersDto = mapper.Map<UserDto[]>(users);
            foreach (var item in usersDto)
            {
                item.UserStatics = new UserStatics();
                if (item.CanWorkAsAgent)
                {
                    item.UserStatics.OrderInStore = this.Context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Store).Count();
                    item.UserStatics.OrderInWay = this.Context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Way).Count();
                }
            }
            return Ok(usersDto);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dbuser = this.Context.Users
                .Include(c => c.UserPhones)
                .Include(c => c.UserGroups)
                .Include(c => c.AgentCountrs)
                    .ThenInclude(c => c.Country)
                .FirstOrDefault(c => c.Id == id);
            var user = mapper.Map<UserDto>(dbuser);
            user.UserStatics = new UserStatics();
            if (user.CanWorkAsAgent)
            {
                user.UserStatics.OrderInStore = this.Context.Orders.Where(c => c.AgentId == id && c.OrderplacedId == (int)OrderplacedEnum.Store).Count();
                user.UserStatics.OrderInWay = this.Context.Orders.Where(c => c.AgentId == id && c.OrderplacedId == (int)OrderplacedEnum.Way).Count();
            }
            return Ok(user);
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
                var userGroup = new UserGroup()
                {
                    UserId = userId,
                    GroupId = groupId
                };
                this.Context.Add(userGroup);
                this.Context.SaveChanges();
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
            this.Context.Entry(user).Collection(c => c.AgentCountrs).Load();
            user.Adress = updateUserDto.Address;
            user.Name = updateUserDto.Name;
            user.HireDate = updateUserDto.HireDate;
            user.Note = updateUserDto.Note;
            {
                var similerUserByname = this.Context.Users.Where(c => c.Name.ToLower() == updateUserDto.Name.ToLower() && c.Id != updateUserDto.Id).Count();
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
                user.Salary = updateUserDto.Salary;
                user.AgentCountrs = null;
                foreach (var item in updateUserDto.Countries)
                {
                    user.AgentCountrs.Add(new AgentCountr()
                    {
                        AgentId = user.Id,
                        CountryId =item
                    });
                } 
            }
            else
            {
                var similerUserByname = this.Context.Users.Where(c => c.UserName.ToLower() == updateUserDto.UserName.ToLower() && c.Id != updateUserDto.Id).Count();
                if (similerUserByname != 0)
                {
                    return Conflict();
                }
                user.UserName = updateUserDto.UserName;
                user.Password = MD5Hash.GetMd5Hash(updateUserDto.Password);
                user.AgentCountrs= null;
                user.Salary = null;
            }
            this.Context.Update(user);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpGet("UsernameExist/{username}")]
        public IActionResult UsernameExist(string username)
        {
            return Ok(this.Context.Users.Where(c => c.UserName == username).Count() != 0);
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = this.Context.Users.Find(id);
            if (user == null)
                return NotFound();
            this.Context.Entry(user).Collection(c => c.Orders).Load();
            this.Context.Entry(user).Collection(c => c.OutComes).Load();
            this.Context.Entry(user).Collection(c => c.Incomes).Load();
            this.Context.Entry(user).Collection(c => c.Clients).Load();
            int x = user.Orders.Count() + user.OutComes.Count() + user.Incomes.Count() + user.Clients.Count();

            if (x > 0)
            {
                return Conflict();
            }
            try
            {
                //   user.UserPhones.Clear();
                user.UserGroups.Clear();
                this.Context.Remove(user);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}