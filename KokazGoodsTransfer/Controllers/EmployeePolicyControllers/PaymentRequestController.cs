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

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentRequestController : AbstractEmployeePolicyController
    {
        public PaymentRequestController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet]
        public IActionResult Get([FromQuery]PagingDto pagingDto, [FromQuery] PaymentFilterDto Filter)
        {
            var paymentRquest = this.Context.PaymentRequests
                .Include(c => c.Client)
                .Include(c => c.PaymentWay)
                .AsQueryable();
            if (Filter.Id != null)
            {
                paymentRquest = paymentRquest.Where(c => c.Id.ToString().StartsWith(Filter.Id.ToString()));
            }
            if (Filter.ClientId != null)
            {
                paymentRquest = paymentRquest.Where(c => c.ClientId == Filter.ClientId);
            }
            if (Filter.PaymentWayId != null)
            {
                paymentRquest = paymentRquest.Where(c => c.PaymentWayId == Filter.PaymentWayId);
            }
            if (Filter.Accept != null)
            {
                paymentRquest = paymentRquest.Where(c => c.Accept == Filter.Accept);
            }
            if (Filter.CreateDate != null)
            {
                paymentRquest = paymentRquest.Where(c => c.CreateDate == Filter.CreateDate);
            }
            var total = paymentRquest.Count();
            var list = paymentRquest.Skip(pagingDto.Page - 1).Take(pagingDto.RowCount);
            return Ok(new { total, data = mapper.Map<PayemntRquestDto[]>(list) });
        }
        [HttpGet("New")]
        public IActionResult New()
        {
            var newPaymentRequets = this.Context.PaymentRequests
                .Include(c => c.Client)
                .Include(c => c.PaymentWay)
                .Where(c => c.Accept ==null).ToList();
            return Ok(mapper.Map<PayemntRquestDto[]>(newPaymentRequets));
        }
        [HttpPut("{id}")]
        public IActionResult Accept(int id)
        {
            var paymentRquest = this.Context.PaymentRequests.Find(id);
            paymentRquest.Accept = true;
            this.Context.Update(paymentRquest);
            Notfication notfication = new Notfication()
            {

            };
            this.Context.SaveChanges();

            return Ok();
        }


    }
}