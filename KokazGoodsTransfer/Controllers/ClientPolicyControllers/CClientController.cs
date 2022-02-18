using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CClientController : AbstractClientPolicyController
    {
        private readonly NotificationHub _notificationHub;
        public CClientController(KokazContext context, IMapper mapper, Logging logging, NotificationHub notificationHub) : base(context, mapper, logging)
        {
            _notificationHub = notificationHub;
        }
        [HttpGet("CheckUserName/{username}")]
        public async Task<IActionResult> CheckUserName(string username)
        {
            return Ok(await this._context.Clients.AnyAsync(c => c.UserName == username && c.Id != AuthoticateUserId()));
        }
        [HttpGet("CheckName/{name}")]
        public async Task<IActionResult> CheckName(string name)
        {
            return Ok(await this._context.Clients.AnyAsync(c => c.Name == name && c.Id != AuthoticateUserId()));
        }
        [HttpPut("updateInformation")]
        public async Task<IActionResult> Update([FromBody] CUpdateClientDto updateClientDto)
        {
            try
            {
                var client = await this._context.Clients.FindAsync(AuthoticateUserId());
                var clientName = client.Name;
                var clientUserName = client.UserName;
                var oldPassword = client.Password;
                client = _mapper.Map<CUpdateClientDto, Client>(updateClientDto, client);
                client.Name = clientName;
                client.UserName = clientUserName;

                if (client.Password == "")
                    client.Password = oldPassword;
                this._context.Update(client);
                this._context.Entry(client).Collection(c => c.ClientPhones).Load();
                client.ClientPhones.Clear();
                if (updateClientDto.Phones != null)
                {
                    foreach (var item in updateClientDto.Phones)
                    {
                        var clientPhone = new ClientPhone()
                        {
                            ClientId = AuthoticateUserId(),
                            Phone = item,
                        };
                        this._context.Add(clientPhone);
                    }
                }

                bool isEditRequest = clientName != updateClientDto.Name || clientUserName != updateClientDto.UserName;
                if (isEditRequest)
                {
                    EditRequest editRequest = new EditRequest();
                    if (clientName != updateClientDto.Name)
                    {
                        editRequest.OldName = clientName;
                        editRequest.NewName = updateClientDto.Name;
                    }
                    if (clientUserName != updateClientDto.UserName)
                    {
                        editRequest.OldUserName = clientUserName;
                        editRequest.NewUserName = updateClientDto.UserName;
                    }
                    editRequest.Accept = null;
                    editRequest.ClientId = AuthoticateUserId();
                    editRequest.UserId = null;
                    this._context.Add(editRequest);
                }
                await this._context.SaveChangesAsync();
                if (isEditRequest)
                {
                    var newEditRquests = await this._context.EditRequests.Where(c => c.Accept == null).CountAsync();

                    var adminNotification = new AdminNotification()
                    {
                        NewEditRquests = newEditRquests,
                    };
                    await _notificationHub.AdminNotifcation(adminNotification);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "خطأ بالتعديل ", Ex = ex.Message });
            }
        }
        [HttpGet("GetByToken")]
        public async Task<IActionResult> GetByToken()
        {

            var appVersion = AppVersion();
            if (appVersion == -1)
            {
                return Conflict(new MobileErrorLogin() { Message = "عليك التحديث", URL = "" });
            }
            var client = await this._context.Clients
                .Include(c => c.ClientPhones)
                .Include(c => c.Country)
                .Where(c => c.Id == AuthoticateUserId()).FirstAsync();
            var authClient = _mapper.Map<AuthClient>(client);
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