﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAuthController : AbstractController
    {
        private readonly KokazContext _context;
        private readonly IMapper _mapper;
        public ClientAuthController(KokazContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            List<string> errors = new List<string>();
            var appVersion = AppVersion();
            if (appVersion == -1)
            {
               return Conflict(new MobileErrorLogin() { Message = "عليك التحديث", URL = "" });
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
            var climes = new List<Claim>
            {
                new Claim("UserID", client.Id.ToString()),
                new Claim("Type", "Client"),
                new Claim(ClaimTypes.Name, client.Name),
                new Claim("branchId", client.BranchId.ToString())
            };
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
            authClient.CanAddOrder = appVersion>=2;
            return Ok(authClient);
        }
        private int AppVersion()
        {
            if (!Request.Headers.TryGetValue("app-v", out var app_vs))
            {
                return -1;
            }
            else
            {
                var app_v = app_vs.First();
                if (string.IsNullOrEmpty(app_v))
                {
                    return -1;
                }
                if (!int.TryParse(app_v, out int version))
                {

                    return -1;
                }
                else
                {
                    return version;
                }
            }
        }
    }
}