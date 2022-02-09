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
using System.Threading.Tasks;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Services.Interfaces;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AbstractEmployeePolicyController
    {

        private readonly IUserCashedService _userCashedService;
        public UserController(KokazContext context, IMapper mapper, Logging logging, IUserCashedService userCashedService) : base(context, mapper, logging)
        {
            _userCashedService = userCashedService;
        }
        [HttpGet("ActiveAgent")]
        public async Task<IActionResult> GetEnalbedAgent() => Ok(await _userCashedService.GetCashed());
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            if (!createUserDto.CanWorkAsAgent)
            {
                var similerUser = this._context.Users.Where(c => c.UserName.ToLower() == createUserDto.UserName.ToLower()).Count();
                if (similerUser != 0)
                    return Conflict();
            }
            {
                var similerUser = this._context.Users.Where(c => c.Name.ToLower() == createUserDto.Name.ToLower()).Count();
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
                UserName = createUserDto.UserName,
                Password = MD5Hash.GetMd5Hash(createUserDto.Password)
            };
            _context.Add(user);
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
            await _context.SaveChangesAsync();
            this._context.Entry(user).Collection(c => c.AgentCountrs).Load();
            foreach (var item in user.AgentCountrs)
            {
                this._context.Entry(item).Reference(c => c.Country).Load();
            }
            await _agentCashRepository.RefreshCash();
            await _countryCashedRepository.RefreshCash();
            return Ok(_mapper.Map<UserDto>(user));
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = this._context.Users
                .Include(c => c.UserPhones)
                .Include(c => c.UserGroups)
                .Include(c => c.AgentCountrs)
                    .ThenInclude(c => c.Country)
                .ToList();
            var usersDto = _mapper.Map<UserDto[]>(users);
            foreach (var item in usersDto)
            {
                item.UserStatics = new UserStatics();
                if (item.CanWorkAsAgent)
                {
                    item.UserStatics.OrderInStore = this._context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Store).Count();
                    item.UserStatics.OrderInWay = this._context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Way).Count();
                }
            }
            return Ok(usersDto);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var dbuser = this._context.Users
                .Include(c => c.UserPhones)
                .Include(c => c.UserGroups)
                .Include(c => c.AgentCountrs)
                    .ThenInclude(c => c.Country)
                .FirstOrDefault(c => c.Id == id);
            var user = _mapper.Map<UserDto>(dbuser);
            user.UserStatics = new UserStatics();
            if (user.CanWorkAsAgent)
            {
                user.UserStatics.OrderInStore = this._context.Orders.Where(c => c.AgentId == id && c.OrderplacedId == (int)OrderplacedEnum.Store).Count();
                user.UserStatics.OrderInWay = this._context.Orders.Where(c => c.AgentId == id && c.OrderplacedId == (int)OrderplacedEnum.Way).Count();
            }
            return Ok(user);
        }
        [HttpPut("AddPhone")]
        public async Task<IActionResult> AddPhone([FromBody] AddPhoneDto addPhoneDto)
        {
            try
            {
                var user = this._context.Users.Find(addPhoneDto.objectId);
                if (user == null)
                    return NotFound();
                this._context.Entry(user).Collection(c => c.UserPhones).Load();
                if (user.UserPhones.Where(c => c.Phone == addPhoneDto.Phone).Any())
                    return Conflict();
                var userPhone = new UserPhone()
                {
                    UserId = user.Id,
                    Phone = addPhoneDto.Phone
                };
                this._context.Add(userPhone);
                this._context.SaveChanges();
                await _agentCashRepository.RefreshCash();
                await _countryCashedRepository.RefreshCash();
                return Ok(_mapper.Map<PhoneDto>(userPhone));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("deletePhone/{id}")]
        public async Task<IActionResult> DeletePhone(int id)
        {
            try
            {
                var userPhone = this._context.UserPhones.Find(id);
                if (userPhone == null)
                    return Conflict();
                this._context.Remove(userPhone);
                this._context.SaveChanges();
                await _agentCashRepository.RefreshCash();
                await _countryCashedRepository.RefreshCash();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("deleteGroup/{userId}")]
        public async Task<IActionResult> DelteGroup(int userId, [FromForm] int groupId)
        {
            try
            {
                var userGroup = this._context.UserGroups.Where(c => c.UserId == userId && c.GroupId == groupId).FirstOrDefault();
                if (userGroup == null)
                    return Conflict();
                this._context.Remove(userGroup);
                this._context.SaveChanges();
                await _agentCashRepository.RefreshCash();
                await _countryCashedRepository.RefreshCash();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("AddToGroup/{userId}")]
        public async Task<IActionResult> AddToGroup(int userId, [FromForm] int groupId)
        {
            try
            {
                var userGroup = new UserGroup()
                {
                    UserId = userId,
                    GroupId = groupId
                };
                this._context.Add(userGroup);
                this._context.SaveChanges();
                await _agentCashRepository.RefreshCash();
                await _countryCashedRepository.RefreshCash();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            var user = this._context.Users.Find(updateUserDto.Id);
            this._context.Entry(user).Collection(c => c.AgentCountrs).Load();
            user.Adress = updateUserDto.Address;
            user.Name = updateUserDto.Name;
            user.HireDate = updateUserDto.HireDate;
            user.Note = updateUserDto.Note;
            {
                var similerUserByname = this._context.Users.Where(c => c.Name.ToLower() == updateUserDto.Name.ToLower() && c.Id != updateUserDto.Id).Count();
                if (similerUserByname != 0)
                {
                    return Conflict();
                }
            }
            if (updateUserDto.CanWorkAsAgent)
            {
                user.UserGroups.Clear();
                user.CanWorkAsAgent = true;
                user.Salary = updateUserDto.Salary;
                user.AgentCountrs.Clear();
                foreach (var item in updateUserDto.Countries)
                {
                    user.AgentCountrs.Add(new AgentCountr()
                    {
                        AgentId = user.Id,
                        CountryId = item
                    });
                }
            }
            else
            {
                var similerUserByname = this._context.Users.Where(c => c.UserName.ToLower() == updateUserDto.UserName.ToLower() && c.Id != updateUserDto.Id).Count();
                if (similerUserByname != 0)
                {
                    return Conflict();
                }

                user.AgentCountrs = null;
                user.Salary = null;
            }
            user.UserName = updateUserDto.UserName;
            if (updateUserDto.UserName == "")
            {
                user.Password = null;
            }
            else
            {
                if (updateUserDto.Password != "")
                {
                    user.Password = MD5Hash.GetMd5Hash(updateUserDto.Password);
                }

            }
            this._context.Update(user);
            this._context.SaveChanges();
            await _agentCashRepository.RefreshCash();
            await _countryCashedRepository.RefreshCash();
            return Ok();
        }
        [HttpGet("UsernameExist/{username}")]
        public IActionResult UsernameExist(string username)
        {
            return Ok(this._context.Users.Where(c => c.UserName == username).Count() != 0);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {

                var result = await _userCashedService.Delete(id);
                if (result.Errors.Any())
                    return Conflict();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}