using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentWayController : AbstractEmployeePolicyController
    {
        public PaymentWayController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper,logging)
        {
        }
        [HttpPost]
        public IActionResult Add([FromBody] NameAndIdDto nameAndIdDto)
        {
            PaymentWay paymentWay = new PaymentWay();
            if (this._context.PaymentWays.Any(c => c.Name == nameAndIdDto.Name))
            {
                return Conflict();
            }
            paymentWay.Name = nameAndIdDto.Name;
            this._context.Add(paymentWay);
            this._context.SaveChanges();
            
            return Ok(_mapper.Map<NameAndIdDto>(paymentWay));
        }
        [HttpGet]
        public IActionResult Get()
        {
            var paymentWaies= this._context.PaymentWays.ToList();
            return Ok(_mapper.Map<NameAndIdDto[]>(paymentWaies));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var payment= this._context.PaymentWays.Find(id);
            return Ok();
        }
    }
}