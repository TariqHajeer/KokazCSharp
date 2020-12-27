using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult CreateClient(CreateClientDto createClientDto)
        {
            var isExist = Context.Clients.Any(c => c.UserName.ToLower() == createClientDto.UserName.ToLower());
            if (isExist)
            {
                return Conflict();
            }
            var client = mapper.Map<Client>(createClientDto);
            this.Context.Add(client);

            return Ok();
        }
    }
}