using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAuthController : AbstractController
    {

        public EmployeeAuthController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet("HetHash/{str}")]
        public IActionResult GetHash(string str)
        {
            return Ok(MD5Hash.GetMd5Hash(str));
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var expireDate = new DateTime(2022, 2,15);
            if (DateTime.Now > expireDate)
            {
                return Conflict("You should  to pay");
            }
            var user = this.Context.Users
                .Include(c => c.UserGroups)
                .ThenInclude(c => c.Group)
                .ThenInclude(c => c.GroupPrivileges)
                .ThenInclude(c => c.Privileg)
                .Where(c => c.UserName == loginDto.UserName).FirstOrDefault();
            if (user == null)
                return Conflict();
            if (!MD5Hash.VerifyMd5Hash(loginDto.Password, user.Password))
                return Conflict();

            var climes = new List<Claim>();
            var privlileges = user.UserGroups.SelectMany(c => c.Group.GroupPrivileges.Select(c => c.Privileg)).Distinct();
            foreach (var item in privlileges)
            {
                climes.Add(new Claim(ClaimTypes.Role, item.SysName));
            }

            climes.Add(new Claim("UserID", user.Id.ToString()));
            if (!user.CanWorkAsAgent)
                climes.Add(new Claim("Type", "Employee"));
            else
                climes.Add(new Claim("Type", "Agent"));

            climes.Add(new Claim(ClaimTypes.Name, user.Name));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(climes),
                Expires = DateTime.UtcNow.AddHours(14),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication")), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            AuthenticatedUserDto authenticatedUserDto = mapper.Map<AuthenticatedUserDto>(user);
            authenticatedUserDto.Token = token;
            authenticatedUserDto.Policy = user.CanWorkAsAgent ? "Agent" : "Employee";
            return Ok(authenticatedUserDto);
        }

    }
}