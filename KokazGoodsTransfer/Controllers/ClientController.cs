using System;
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

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ClientController : AbstractController
    {
        public ClientController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPost]
        [Authorize]
        public IActionResult CreateClient(CreateClientDto createClientDto)
        {
            var isExist = Context.Clients.Any(c => c.UserName.ToLower() == createClientDto.UserName.ToLower());
            if (isExist)
            {
                return Conflict();
            }
            var client = mapper.Map<Client>(createClientDto);
            client.UserId = (int)AuthoticateUserId();
            this.Context.Set<Client>().Add(client);
            this.Context.SaveChanges();
            client = this.Context.Clients
                .Include(c => c.Region)
                .Include(c=>c.User)
                .Single(c => c.Id == client.Id);
            return Ok(mapper.Map<ClientDto>(client));
        }
        [HttpGet]
        public IActionResult Get()
        {
            var clients = this.Context.Clients
                .Include(c=>c.Region)
                .Include(c=>c.User)
                .ToList();
            return Ok(mapper.Map<ClientDto[]>(clients));
        }
    }
}