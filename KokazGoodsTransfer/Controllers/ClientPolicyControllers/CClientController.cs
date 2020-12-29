using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
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
        [HttpPatch]
        public IActionResult Update([FromBody] CUpdateClientDto updateClientDto)
        {
            var client = this.Context.Clients.Find(AuthoticateUserId());
            client = mapper.Map<CUpdateClientDto, Client>(updateClientDto, client);
            this.Context.Update(client);
            this.Context.SaveChanges();
            client = this.Context.Clients
                .Include(c => c.Region)
                .Include(c => c.User)
                .Single(c => c.Id == client.Id);
            return Ok();
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
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}