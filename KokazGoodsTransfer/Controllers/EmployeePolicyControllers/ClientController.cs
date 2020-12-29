﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
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

        public IActionResult CreateClient(CreateClientDto createClientDto)
        {
            var isExist = Context.Clients.Any(c => c.UserName.ToLower() == createClientDto.UserName.ToLower());
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
            this.Context.Set<Client>().Add(client);
            this.Context.SaveChanges();
            client = this.Context.Clients
                .Include(c => c.Region)
                .Include(c => c.User)
                .Single(c => c.Id == client.Id);
            return Ok(mapper.Map<ClientDto>(client));
        }
        [HttpGet]
        public IActionResult Get()
        {
            var clients = this.Context.Clients
                .Include(c => c.Region)
                .Include(c => c.User)
                .Include(c => c.ClientPhones)
                .ToList();
            return Ok(mapper.Map<ClientDto[]>(clients));
        }
        [HttpPut("addPhone")]
        public IActionResult AddPhone([FromBody]AddPhoneDto addPhoneDto)
        {
            try
            {
                var client = this.Context.Clients.Find(addPhoneDto.ClientId);
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
                return Ok(mapper.Map<ClientPhoneDto>(clientPhone));
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
    }
}