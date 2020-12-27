using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : AbstractController
    {

        public AuthController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            var user = this.Context.Users.Where(c => c.UserName == loginDto.UserName).FirstOrDefault();
            if (user == null)
                return Conflict();
            if (!MD5Hash.VerifyMd5Hash(loginDto.Password, user.Password))
                return Conflict();
            //security key
            string securityKey = "this_is_our_supper_long_security_key_for_token_validation_project_2018_09_07$smesk.in";
            //symmetric security key
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));

            //signing credentials
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            //add claims
            var claims = new List<Claim>();
            claims.Add(new Claim("userId", user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, user.UserName));
            foreach (var Role in user.UserGroups.SelectMany(c => c.Group.GroupPrivileges.Select(r => r.Privileg)))
            {
                claims.Add(new Claim(ClaimTypes.Role, Role.Name));
            }
            //create token
            var token = new JwtSecurityToken(
                    issuer: "smesk.in",
                    audience: "readers",
                    expires: DateTime.Now.AddMinutes(7 * 60),
                    signingCredentials: signingCredentials
                    , claims: claims
                );
            return Ok(token);
        }
    }
}