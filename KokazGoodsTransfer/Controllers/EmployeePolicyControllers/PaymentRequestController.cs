using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        [HttpGet("New")]
        public IActionResult New()
        {
            var newPaymentRequets = this.Context.PaymentRequests
                .Include(c => c.Client)
                .Include(c => c.PaymentWay)
                .Where(c => c.Accept != true).ToList();
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

            }
            this.Context.SaveChanges();

            return Ok();
        }
    }
}