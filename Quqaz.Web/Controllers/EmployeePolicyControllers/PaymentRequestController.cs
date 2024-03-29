﻿using System.Threading.Tasks;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.PayemntRequestDtos;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentRequestController : AbstractEmployeePolicyController
    {
        private readonly IPaymentRequestSerivce _paymentRequestSerivce;
        public PaymentRequestController(IPaymentRequestSerivce paymentRequestSerivce)
        {
            _paymentRequestSerivce = paymentRequestSerivce;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] PaymentFilterDto Filter)
        {
            return Ok(await _paymentRequestSerivce.Get(pagingDto, Filter));
        }
        [HttpGet("New")]
        public async Task<IActionResult> New()
        {
            return Ok(await _paymentRequestSerivce.New());
        }
        [HttpPut("Accept/{id}")]
        public async Task<IActionResult> Accept(int id)
        {

            await _paymentRequestSerivce.Accept(id);
            return Ok();
        }
        [HttpPut("DisAccept/{id}")]
        public async Task<IActionResult> DisAccept(int id)
        {
            await _paymentRequestSerivce.DisAccept(id);
            return Ok();
        }

    }
}