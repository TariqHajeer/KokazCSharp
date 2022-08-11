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
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using KokazGoodsTransfer.Services.Interfaces;
using KokazGoodsTransfer.Dtos.BranchDtos;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAuthController : OldAbstractController
    {
        private readonly ITreasuryService _treasuryService;
        public EmployeeAuthController(KokazContext context, IMapper mapper, ITreasuryService treasuryService) : base(context, mapper)
        {
            _treasuryService = treasuryService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await this._context.Users
                .Include(c => c.UserGroups)
                    .ThenInclude(c => c.Group)
                        .ThenInclude(c => c.GroupPrivileges)
                            .ThenInclude(c => c.Privileg)
                .Include(c => c.Branches)
                    .ThenInclude(c => c.Branch)
                .Where(c => c.UserName == loginDto.UserName).FirstOrDefaultAsync();
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
            var haveTreasury = await _treasuryService.Any(c => c.Id == user.Id && c.IsActive);
            if (haveTreasury)
            {
                climes.Add(new Claim("treasury", true.ToString()));
            }
            else
            {
                climes.Add(new Claim("treasury", false.ToString()));
            }
            climes.Add(new Claim(ClaimTypes.Name, user.Name));
            foreach (var item in user.Branches)
            {
                climes.Add(new Claim("branchId", item.BranchId.ToString()));
            }
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(climes),
                Expires = DateTime.UtcNow.AddHours(14),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication")), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            AuthenticatedUserDto authenticatedUserDto = _mapper.Map<AuthenticatedUserDto>(user);
            authenticatedUserDto.Token = token;
            authenticatedUserDto.Policy = user.CanWorkAsAgent ? "Agent" : "Employee";
            authenticatedUserDto.HaveTreasury = haveTreasury;
            return Ok(authenticatedUserDto);
        }

    }
}