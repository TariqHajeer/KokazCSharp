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
        public ClientAuthController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper, logging)
        {
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            List<string> errors = new List<string>();
            bool canLogin = true;
            bool canAddOrder = true;
            if (!Request.Headers.TryGetValue("app-v", out var app_vs))
            {
                canAddOrder = canLogin = false;
            }
            else
            {
                var app_v = app_vs.First();
                if (string.IsNullOrEmpty(app_v))
                {
                    canLogin = canAddOrder = false;
                }
                if (int.Parse(app_v) < 2)
                {
                    canAddOrder = canLogin = false;
                }
            }
            var client = this._context.Clients
                .Include(c => c.Country)
                .Include(c => c.ClientPhones)
                .Where(c => c.UserName.ToLower() == loginDto.UserName.ToLower()).FirstOrDefault();


            if (client == null || !MD5Hash.VerifyMd5Hash(loginDto.Password, client.Password))
            {
                errors.Add("خطأ بأسم المستخدم و كلمة المرور");
                return Conflict(new { messages = errors });
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
            var authClient = _mapper.Map<AuthClient>(client);
            authClient.Token = token;
            authClient.CanAddOrder = canAddOrder;
            authClient.CanLogin = canLogin;
            return Ok(authClient);
        }
    }
}