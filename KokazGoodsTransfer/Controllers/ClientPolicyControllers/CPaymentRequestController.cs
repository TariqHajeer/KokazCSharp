using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.PayemntRequestDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPaymentRequestController : AbstractClientPolicyController
    {
        public CPaymentRequestController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet("CanRequest")]
        public IActionResult CanRequest()
        {
            return Ok(!this.Context.PaymentRequests.Any(c => c.ClientId == AuthoticateUserId() && c.Accept != true));
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreatePaymentRequestDto createPaymentRequestDto)
        {
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                PaymentWayId = createPaymentRequestDto.PaymentWayId,
                Note = createPaymentRequestDto.Note,
                ClientId = AuthoticateUserId(),
            };
            this.Context.Add(paymentRequest);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public IActionResult Get([FromQuery] PagingDto pagingDto)
        {
            var paymentRequests = this.Context.PaymentRequests
                .Include(c => c.PaymentWay)
                .Where(c => c.ClientId == AuthoticateUserId());
            var total = paymentRequests.Count();
            paymentRequests = paymentRequests.Skip(pagingDto.RowCount - 1).Take(pagingDto.RowCount);
            return Ok(mapper.Map<PayemntRquestDto[]>(paymentRequests.ToList()));
        }

    }
}