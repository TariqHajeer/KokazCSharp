using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Users;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quqaz.Web.Services.Interfaces;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
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
            try
            {
                var user = await this._context.Users
                    .Include(c => c.UserGroups)
                        .ThenInclude(c => c.Group)
                            .ThenInclude(c => c.GroupPrivileges)
                                .ThenInclude(c => c.Privileg)
                    .Include(c => c.Branches)
                        .ThenInclude(c => c.Branch)
                    .Where(c => c.UserName.ToLower() == loginDto.UserName.ToLower()).FirstOrDefaultAsync();
                if (user == null)
                    return Conflict();
                if (!MD5Hash.VerifyMd5Hash(loginDto.Password, user.Password))
                    return Conflict();
                var climes = new List<Claim>
                {
                    new Claim("UserID", user.Id.ToString())
                };
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
                if (user.BranchId != null)
                    climes.Add(new Claim("branchId", user.BranchId.ToString()));
                else
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message + "\n" + _context.Database.GetDbConnection().ConnectionString);
            }
        }

    }
}