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
        public async Task<IActionResult> Get(int printNumber)
        {
            var print = await this._context.ClientPayments
                .Include(c => c.OrderClientPaymnets)
                .ThenInclude(c => c.Order)
                .Include(c => c.Receipts)
                .Include(c => c.ClientPaymentDetails)
                .Where(c => c.Id == printNumber && c.OrderClientPaymnets.All(c => c.Order.ClientId == AuthoticateUserId())).FirstOrDefaultAsync();
            return Ok(_mapper.Map<PrintOrdersDto>(print));
        }
    }
}