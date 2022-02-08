using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Helpers;
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
        public PrintController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper, logging)
        {
        }
        [HttpGet("{printNumber}")]
        public IActionResult Get(int printNumber)
        {
            var print = this._context.Printeds
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Order)
                .Include(c => c.Receipts)
                .Include(c => c.ClientPrints)
                .Where(c => c.Type == PrintType.Client && c.PrintNmber == printNumber&&c.OrderPrints.All(c=>c.Order.ClientId==AuthoticateUserId())).FirstOrDefault();
            return Ok(_mapper.Map<PrintOrdersDto>(print));
        }
    }
}