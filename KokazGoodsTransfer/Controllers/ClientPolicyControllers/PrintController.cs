using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintController : AbstractClientPolicyController
    {
        public PrintController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet("{printNumber}")]
        public IActionResult Get(int printNumber)
        {
            var print = this.Context.Printeds
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Order)
                .Include(c => c.Receipts)
                .Include(c => c.ClientPrints)
                .Where(c => c.Type == PrintType.Client && c.PrintNmber == printNumber&&c.OrderPrints.All(c=>c.Order.ClientId==AuthoticateUserId())).FirstOrDefault();
            return Ok(mapper.Map<PrintOrdersDto>(print));
        }
    }
}