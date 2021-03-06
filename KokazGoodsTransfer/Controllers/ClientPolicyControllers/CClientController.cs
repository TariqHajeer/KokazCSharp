﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CClientController : AbstractClientPolicyController
    {
        public CClientController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPut("updateInformation")]
        public IActionResult Update([FromBody] CUpdateClientDto updateClientDto)
        {
            try
            {
                var client = this.Context.Clients.Find(AuthoticateUserId());
                var clientName = client.Name;
                var oldPassword = client.Password;
                client = mapper.Map<CUpdateClientDto, Client>(updateClientDto, client);
                client.Name = clientName;
                if (client.Password == "")
                    client.Password = oldPassword;
                this.Context.Update(client);
                this.Context.SaveChanges();
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(new {message="خطأ بالتعديل ",Ex=ex.Message});
            }
        }
        [HttpPut("deletePhone/{id}")]
        public IActionResult DeletePhone(int id)
        {
            try
            {
                var clientPhone = this.Context.ClientPhones.Find(id);
                this.Context.Remove(clientPhone);
                this.Context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("addPhone/{phone}")]
        public IActionResult AddPhone(string phone)
        {
            try
            {
                var clientId = AuthoticateUserId();
                this.Context.Clients.Find(clientId);

                ClientPhone clientPhone = new ClientPhone()
                {
                    ClientId = clientId,
                    Phone = phone
                };
                this.Context.Add(clientPhone);
                this.Context.SaveChanges();
                return Ok(mapper.Map<PhoneDto>(clientPhone));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet("GetByToken")]
        public IActionResult GetByToken()
        {
            var client = this.Context.Clients.Include(c=>c.ClientPhones).Include(c=>c.Country).Where(c=>c.Id==AuthoticateUserId()).First();
            var authClient = mapper.Map<AuthClient>(client);
            return Ok(authClient);
        }
    }
}