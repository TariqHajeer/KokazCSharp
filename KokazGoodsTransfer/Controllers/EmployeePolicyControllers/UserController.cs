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
    public class UserController : OldAbstractEmployeePolicyController
    {

        private readonly IUserCashedService _userCashedService;
        private readonly ICountryCashedService _countryCashedService;
        public UserController(KokazContext context, IMapper mapper, IUserCashedService userCashedService, ICountryCashedService countryCashedService) : base(context, mapper)
        {
            _userCashedService = userCashedService;
            _countryCashedService = countryCashedService;
        }
        [HttpGet("ActiveAgent")]
        public async Task<IActionResult> GetEnalbedAgent() => Ok(await _userCashedService.GetCashed());
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _userCashedService.GetAll());
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {

            var reuslt = await _userCashedService.AddAsync(createUserDto);
            _countryCashedService.RemoveCash();
            if (reuslt.Errors.Any())
                return Conflict();
            return Ok(reuslt.Data);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _userCashedService.GetById(id));
        }
        [HttpPut("AddPhone")]
        public async Task<IActionResult> AddPhone([FromBody] AddPhoneDto addPhoneDto)
        {

            var data = await _userCashedService.AddPhone(addPhoneDto);
            _countryCashedService.RemoveCash();
            return Ok(data);

        }
        [HttpPut("deletePhone/{id}")]
        public async Task<IActionResult> DeletePhone(int id)
        {

            await _userCashedService.DeletePhone(id);
            _countryCashedService.RemoveCash();
            return Ok();

        }
        [HttpPut("deleteGroup/{userId}")]
        public async Task<IActionResult> DelteGroup(int userId, [FromForm] int groupId)
        {

            await _userCashedService.DeleteGroup(userId, groupId);
            return Ok();


        }
        [HttpPut("AddToGroup/{userId}")]
        public async Task<IActionResult> AddToGroup(int userId, [FromForm] int groupId)
        {
            await _userCashedService.AddToGroup(userId, groupId);
            return Ok();

        }
        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUserDto)
        {
            var result = await _userCashedService.Update(updateUserDto);
            _countryCashedService.RemoveCash();
            return Ok(result.Data);

        }
        [HttpGet("UsernameExist/{username}")]
        public async Task<IActionResult> UsernameExist(string username)
        {

            return Ok(await _userCashedService.Any(c => c.UserName == username));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {

            var result = await _userCashedService.Delete(id);
            _countryCashedService.RemoveCash();
            if (result.Errors.Any())
                return Conflict();
            return Ok();

        }

    }
}