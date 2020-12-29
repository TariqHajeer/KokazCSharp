using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CClientController : AbstractClientPolicyController
    {
        public CClientController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        //[HttpPatch]
        //public IActionResult Update([FromBody] Up)
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
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}