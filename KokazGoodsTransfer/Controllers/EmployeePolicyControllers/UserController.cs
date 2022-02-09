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
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _userCashedService.GetALl());
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            try
            {
                var reuslt = await _userCashedService.AddAsync(createUserDto);
                if (reuslt.Errors.Any())
                    return Conflict();
                return Ok(reuslt.Data);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _userCashedService.GetById(id));
        }
        // [HttpPut("AddPhone")]
        // public async Task<IActionResult> AddPhone([FromBody] AddPhoneDto addPhoneDto)
        // {
        //     try
        //     {
        //         var user = this._context.Users.Find(addPhoneDto.objectId);
        //         if (user == null)
        //             return NotFound();
        //         this._context.Entry(user).Collection(c => c.UserPhones).Load();
        //         if (user.UserPhones.Where(c => c.Phone == addPhoneDto.Phone).Any())
        //             return Conflict();
        //         var userPhone = new UserPhone()
        //         {
        //             UserId = user.Id,
        //             Phone = addPhoneDto.Phone
        //         };
        //         this._context.Add(userPhone);
        //         this._context.SaveChanges();
        //         await _agentCashRepository.RefreshCash();
        //         await _countryCashedRepository.RefreshCash();
        //         return Ok(_mapper.Map<PhoneDto>(userPhone));
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest();
        //     }
        // }
        // [HttpPut("deletePhone/{id}")]
        // public async Task<IActionResult> DeletePhone(int id)
        // {
        //     try
        //     {
        //         var userPhone = this._context.UserPhones.Find(id);
        //         if (userPhone == null)
        //             return Conflict();
        //         this._context.Remove(userPhone);
        //         this._context.SaveChanges();
        //         await _agentCashRepository.RefreshCash();
        //         await _countryCashedRepository.RefreshCash();
        //         return Ok();
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest();
        //     }
        // }
        // [HttpPut("deleteGroup/{userId}")]
        // public async Task<IActionResult> DelteGroup(int userId, [FromForm] int groupId)
        // {
        //     try
        //     {
        //         var userGroup = this._context.UserGroups.Where(c => c.UserId == userId && c.GroupId == groupId).FirstOrDefault();
        //         if (userGroup == null)
        //             return Conflict();
        //         this._context.Remove(userGroup);
        //         this._context.SaveChanges();
        //         await _agentCashRepository.RefreshCash();
        //         await _countryCashedRepository.RefreshCash();
        //         return Ok();
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest();
        //     }
        // }
        // [HttpPut("AddToGroup/{userId}")]
        // public async Task<IActionResult> AddToGroup(int userId, [FromForm] int groupId)
        // {
        //     try
        //     {
        //         var userGroup = new UserGroup()
        //         {
        //             UserId = userId,
        //             GroupId = groupId
        //         };
        //         this._context.Add(userGroup);
        //         this._context.SaveChanges();
        //         await _agentCashRepository.RefreshCash();
        //         await _countryCashedRepository.RefreshCash();
        //         return Ok();
        //     }
        //     catch (Exception ex)
        //     {
        //         return BadRequest();
        //     }
        // }
        // [HttpPatch]
        // public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        // {
        //     var user = this._context.Users.Find(updateUserDto.Id);
        //     this._context.Entry(user).Collection(c => c.AgentCountrs).Load();
        //     user.Adress = updateUserDto.Address;
        //     user.Name = updateUserDto.Name;
        //     user.HireDate = updateUserDto.HireDate;
        //     user.Note = updateUserDto.Note;
        //     {
        //         var similerUserByname = this._context.Users.Where(c => c.Name.ToLower() == updateUserDto.Name.ToLower() && c.Id != updateUserDto.Id).Count();
        //         if (similerUserByname != 0)
        //         {
        //             return Conflict();
        //         }
        //     }
        //     if (updateUserDto.CanWorkAsAgent)
        //     {
        //         user.UserGroups.Clear();
        //         user.CanWorkAsAgent = true;
        //         user.Salary = updateUserDto.Salary;
        //         user.AgentCountrs.Clear();
        //         foreach (var item in updateUserDto.Countries)
        //         {
        //             user.AgentCountrs.Add(new AgentCountr()
        //             {
        //                 AgentId = user.Id,
        //                 CountryId = item
        //             });
        //         }
        //     }
        //     else
        //     {
        //         var similerUserByname = this._context.Users.Where(c => c.UserName.ToLower() == updateUserDto.UserName.ToLower() && c.Id != updateUserDto.Id).Count();
        //         if (similerUserByname != 0)
        //         {
        //             return Conflict();
        //         }

        //         user.AgentCountrs = null;
        //         user.Salary = null;
        //     }
        //     user.UserName = updateUserDto.UserName;
        //     if (updateUserDto.UserName == "")
        //     {
        //         user.Password = null;
        //     }
        //     else
        //     {
        //         if (updateUserDto.Password != "")
        //         {
        //             user.Password = MD5Hash.GetMd5Hash(updateUserDto.Password);
        //         }

        //     }
        //     this._context.Update(user);
        //     this._context.SaveChanges();
        //     await _agentCashRepository.RefreshCash();
        //     await _countryCashedRepository.RefreshCash();
        //     return Ok();
        // }
        [HttpGet("UsernameExist/{username}")]
        public async Task<IActionResult> UsernameExist(string username)
        {

            return Ok(await _userCashedService.Any(c => c.UserName == username));
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