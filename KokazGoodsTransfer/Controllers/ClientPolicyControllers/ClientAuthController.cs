using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAuthController : AbstractController
    {
        public ClientAuthController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPost]
        public IActionResult Login([FromBody]LoginDto loginDto)
        {
            var client = this.Context.Clients
                .Include(c => c.Region)
                .Where(c => c.UserName == loginDto.UserName).FirstOrDefault();
            if (client == null)
                return Conflict();
            if (!MD5Hash.VerifyMd5Hash(loginDto.Password, client.Password))
            {
                return Conflict();
            }
            var climes = new List<Claim>();
            climes.Add(new Claim("UserID", client.Id.ToString()));
            climes.Add(new Claim("Type", "Client"));
            climes.Add(new Claim(ClaimTypes.Name, client.Name));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authnetication")), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(climes),
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            var authClient = mapper.Map<AuthClient>(client);
            authClient.Token = token;
            return Ok(authClient);
        }
    }
}