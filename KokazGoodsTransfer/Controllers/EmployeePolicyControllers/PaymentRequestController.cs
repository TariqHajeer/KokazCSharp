using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.PayemntRequestDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentRequestController : OldAbstractEmployeePolicyController
    {
        public PaymentRequestController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet]
        public IActionResult Get([FromQuery]PagingDto pagingDto, [FromQuery] PaymentFilterDto Filter)
        {
            var paymentRquest = this._context.PaymentRequests
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
            return Ok(new { total, data = _mapper.Map<PayemntRquestDto[]>(list) });
        }
        [HttpGet("New")]
        public IActionResult New()
        {
            var newPaymentRequets = this._context.PaymentRequests
                .Include(c => c.Client)
                .Include(c => c.PaymentWay)
                .Where(c => c.Accept ==null).ToList();
            return Ok(_mapper.Map<PayemntRquestDto[]>(newPaymentRequets));
        }
        [HttpPut("Accept/{id}")]
        public IActionResult Accept(int id)
        {
            var paymentRquest = this._context.PaymentRequests.Find(id);
            paymentRquest.Accept = true;
            this._context.Update(paymentRquest);
            this._context.SaveChanges();

            return Ok();
        }
        [HttpPut("DisAccept/{id}")]
        public IActionResult DisAccept(int id)
        {
            var paymentRquest = this._context.PaymentRequests.Find(id);
            paymentRquest.Accept = false;
            this._context.Update(paymentRquest);
            this._context.SaveChanges();
            return Ok();
        }

    }
}