using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Statics;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : AbstractEmployeePolicyController
    {
        public StatisticsController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet("MainStatics")]
        public IActionResult MainStatics()
        {
            MainStaticsDto mainStatics = new MainStaticsDto()
            {
                TotalAgent = this.Context.Users.Where(c => c.CanWorkAsAgent == true).Count(),
                TotalClient = this.Context.Clients.Count(),
                TotalOrderInSotre = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store).Count(),
                TotlaOrder = this.Context.Orders.Count(),
                TotalOrderOutStore = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way).Count(),
                //TotalOrderDiliverd = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store).Count(),
            };
            return Ok(mainStatics);
        }
        [HttpGet("GetAggregate")]
        public IActionResult GetAggregate()
        {
            AggregateDto aggregateDto = new AggregateDto()
            {
                ShipmentTotal = this.Context.Orders.Sum(c => c.DeliveryCost - c.AgentCost),
                TotalIncome = this.Context.Incomes.Sum(c => c.Amount),
                TotalOutCome = this.Context.OutComes.Sum(c => c.Amount)
            };
            return Ok(aggregateDto);
        }
    }
}