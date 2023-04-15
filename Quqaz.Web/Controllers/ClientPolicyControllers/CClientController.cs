﻿using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.NotifcationDtos;
using Quqaz.Web.HubsConfig;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CClientController : AbstractClientPolicyController
    {
        private readonly NotificationHub _notificationHub;
        private readonly IClientCashedService _clientCashedService;
        public CClientController(NotificationHub notificationHub, IClientCashedService clientCashedService)
        {
            _notificationHub = notificationHub;
            _clientCashedService = clientCashedService;
        }
        [HttpGet("CheckUserName/{username}")]
        public async Task<IActionResult> CheckUserName(string username)
        {
            return Ok(await _clientCashedService.Any(c => c.UserName == username && c.Id != AuthoticateUserId()));
        }
        [HttpGet("CheckName/{name}")]
        public async Task<IActionResult> CheckName(string name)
        {
            return Ok(await _clientCashedService.Any(c => c.Name == name && c.Id != AuthoticateUserId()));
        }
        [HttpPut("updateInformation")]
        public async Task<IActionResult> Update([FromBody] CUpdateClientDto updateClientDto)
        {
            await _clientCashedService.Update(updateClientDto);
            return Ok();
        }
        [HttpGet("GetByToken")]
        public async Task<IActionResult> GetByToken()
        {

            var appVersion = AppVersion();
            if (appVersion == -1)
            {
                return Conflict(new MobileErrorLogin() { Message = "عليك التحديث", URL = "" });
            }

            var authClient = await _clientCashedService.GetAuthClient();
            authClient.CanAddOrder = appVersion >= 2;
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