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
            return Ok(!this.Context.PaymentRequests.Any(c => c.ClientId == AuthoticateUserId() && c.Accept == null));
        }
        [HttpPost]
        public IActionResult Create([FromBody] CreatePaymentRequestDto createPaymentRequestDto)
        {
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                PaymentWayId = createPaymentRequestDto.PaymentWayId,
                Note = createPaymentRequestDto.Note,
                ClientId = AuthoticateUserId(),
                CreateDate = createPaymentRequestDto.Date,
                Accept =null
            };
            this.Context.Add(paymentRequest);
            this.Context.SaveChanges();
            return Ok(mapper.Map<PayemntRquestDto>(paymentRequest));
        }
        [HttpGet]
        public IActionResult Get([FromQuery] PagingDto pagingDto)
        {
            var paymentRequests = this.Context.PaymentRequests
                .Include(c => c.PaymentWay)
                .Where(c => c.ClientId == AuthoticateUserId());
            var total = paymentRequests.Count();
            paymentRequests = paymentRequests.OrderByDescending(c => c.Id).Skip(pagingDto.Page - 1).Take(pagingDto.RowCount);
            var temp = paymentRequests.ToList();
            var dto = mapper.Map<PayemntRquestDto[]>(temp);
            return Ok(new { total, dto });
        }
        [HttpGet("GetPaymentWay")]
        public IActionResult GetPaymentWay()
        {
            var paymentWay = this.Context.PaymentWays.ToList();
            return Ok(mapper.Map<NameAndIdDto[]>(paymentWay));
        }

    }
}