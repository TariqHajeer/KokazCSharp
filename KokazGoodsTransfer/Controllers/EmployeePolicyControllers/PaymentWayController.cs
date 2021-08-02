using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentWayController : AbstractEmployeePolicyController
    {
        public PaymentWayController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpPost]
        public IActionResult Add([FromBody] NameAndIdDto nameAndIdDto)
        {
            PaymentWay paymentWay = new PaymentWay();
            if (this.Context.PaymentWays.Any(c => c.Name == nameAndIdDto.Name))
            {
                return Conflict();
            }
            paymentWay.Name = nameAndIdDto.Name;
            this.Context.Add(paymentWay);
            this.Context.SaveChanges();
            return Ok(mapper.Map<NameAndIdDto[]>(paymentWay));
        }
        [HttpGet]
        public IActionResult Get()
        {
            var paymentWaies= this.Context.PaymentWays.ToList();
            return Ok(mapper.Map<NameAndIdDto[]>(paymentWaies));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var payment= this.Context.PaymentWays.Find(id);
            return Ok();
        }
    }
}