using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COrderController : AbstractClientPolicyController
    {
        public COrderController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPost]
        public IActionResult Create(CreateOrderFromClient createOrderFromClient)
        {

            var order = mapper.Map<Order>(createOrderFromClient);
            order.ClientId = AuthoticateUserId();
            order.CreatedBy = AuthoticateUserName();
            return Ok();
        }
        [HttpGet("codeExist")]
        public IActionResult CodeExist(string code)
        {
            if(this.Context.Orders.Where(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase)).Any())
            {
                return Ok(true);
            }
            return Ok(false);
        }
    }
}