﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ClientController : AbstractEmployeePolicyController
    {
        public ClientController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPost]
        [Authorize(Roles = "AddClient")]
        public IActionResult CreateClient(CreateClientDto createClientDto)
        {
            try
            {
                var isExist = Context.Clients.Any(c => c.UserName.ToLower() == createClientDto.UserName.ToLower() || c.Name.ToLower() == createClientDto.Name.ToLower());
                if (isExist)
                {
                    return Conflict();
                }
                var client = mapper.Map<Client>(createClientDto);
                client.UserId = (int)AuthoticateUserId();

                foreach (var item in createClientDto.Phones)
                {
                    client.ClientPhones.Add(new ClientPhone()
                    {
                        ClientId = client.Id,
                        Phone = item
                    });
                }
                client.Total = 0;
                this.Context.Set<Client>().Add(client);
                this.Context.SaveChanges();
                client = this.Context.Clients
                    .Include(c => c.Region)
                    .Include(c => c.User)
                    .Single(c => c.Id == client.Id);
                return Ok(mapper.Map<ClientDto>(client));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult Get()
        {
            var clients = this.Context.Clients
                .Include(c => c.Region)
                .Include(c => c.User)
                .Include(c => c.ClientPhones)
                .Include(c => c.Orders)
                .ToList();
            return Ok(mapper.Map<ClientDto[]>(clients));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var client = this.Context.Clients.Include(c => c.Region)
                .Include(c => c.User)
                .Include(c => c.ClientPhones)
                .Include(c => c.Orders)
                .Where(c => c.Id == id).FirstOrDefault();
            return Ok(mapper.Map<ClientDto>(client));
        }
        [HttpPut("addPhone")]
        public IActionResult AddPhone([FromBody]AddPhoneDto addPhoneDto)
        {
            try
            {
                var client = this.Context.Clients.Find(addPhoneDto.objectId);
                if (client == null)
                    return NotFound();
                this.Context.Entry(client).Collection(c => c.ClientPhones).Load();
                if (client.ClientPhones.Select(c => c.Phone).Contains(addPhoneDto.Phone))
                {
                    return Conflict();
                }
                var clientPhone = new ClientPhone()
                {
                    ClientId = client.Id,
                    Phone = addPhoneDto.Phone
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
        [HttpPatch]
        public IActionResult UpdateClient([FromBody] UpdateClientDto updateClientDto)
        {
            try
            {
                var client = this.Context.Clients.Where(c => c.Id == updateClientDto.Id).FirstOrDefault();
                if (client == null)
                    return NotFound();

                var oldPassord = client.Password;

                client = mapper.Map<UpdateClientDto, Client>(updateClientDto, client);
                if (updateClientDto.Password == null)
                    client.Password = oldPassord;

                this.Context.Update(client);
                this.Context.SaveChanges();
                client = this.Context.Clients
                .Include(c => c.Region)
                .Include(c => c.User)
                .Single(c => c.Id == client.Id);
                return Ok(mapper.Map<ClientDto>(client));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteClient(int id)
        {
            var client = this.Context.Clients
                .Find(id);
            if (client == null)
                return NotFound();
            this.Context.Entry(client).Collection(c => c.Orders).Load();
            if (client.Orders.Count() != 0)
                return Conflict();
            this.Context.Remove(client);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpPost("Account")]
        public IActionResult Account([FromBody] AccountDto accountDto)
        {
            var client = this.Context.Clients.Find(accountDto.ClinetId);
            client.Total += accountDto.Amount;
            this.Context.Update(client);
            this.Context.SaveChanges();
            return Ok();
        }
        
    }
}