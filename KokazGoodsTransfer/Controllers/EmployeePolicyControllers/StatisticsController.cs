using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.Statics;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                TotalOrderInWay = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way).Count(),
                TotalOrderCountInProcess = this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Processing).Count(),
                TotalOrderCountInProcessAmount = this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Processing).Sum(c => c.Cost - c.DeliveryCost),

            };
            return Ok(mainStatics);
        }
        [HttpGet("GetAggregate")]
        public IActionResult GetAggregate([FromQuery] DateFiter dateFiter)
        {
            //            AggregateDto aggregateDto = new AggregateDto();

            //var ShipmentTotal = this.Context.Orders.Sum(c => c.DeliveryCost - c.AgentCost);
            //var TotalIncome = this.Context.Incomes.Sum(c => c.Amount);
            //var TotalOutCome = this.Context.OutComes.Sum(c => c.Amount);
            var ShipmentTotal = this.Context.Orders.AsQueryable();
            var TotalIncome = this.Context.Incomes.AsQueryable();
            var TotalOutCome = this.Context.OutComes.AsQueryable();
            if (dateFiter.FromDate != null)
            {
                ShipmentTotal = ShipmentTotal.Where(c => c.Date >= dateFiter.FromDate);
                TotalIncome = TotalIncome.Where(c => c.Date >= dateFiter.FromDate);
                TotalOutCome = TotalOutCome.Where(c => c.Date >= dateFiter.FromDate);
            }
            if (dateFiter.ToDate != null)
            {
                ShipmentTotal = ShipmentTotal.Where(c => c.Date <= dateFiter.ToDate);
                TotalIncome = TotalIncome.Where(c => c.Date <= dateFiter.ToDate);
                TotalOutCome = TotalOutCome.Where(c => c.Date <= dateFiter.ToDate);
            }
            AggregateDto aggregateDto = new AggregateDto()
            {
                ShipmentTotal = ShipmentTotal.Sum(c => c.DeliveryCost - c.AgentCost),
                TotalIncome = TotalIncome.Sum(c => c.Amount),
                TotalOutCome = TotalOutCome.Sum(c => c.Amount)
            };
            return Ok(aggregateDto);
        }
        [HttpGet("AgnetStatics")]
        public IActionResult AgnetStatics()
        {
            var agent = this.Context.Users.Where(c => c.CanWorkAsAgent == true).ToList();
            List<UserDto> userDtos = new List<UserDto>();
            foreach (var item in agent)
            {
                var user = mapper.Map<UserDto>(item);
                user.UserStatics = new UserStatics();
                user.UserStatics.OrderInStore = this.Context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Store).Count();
                user.UserStatics.OrderInWay = this.Context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Way).Count();
                userDtos.Add(user);
            }
            return Ok(userDtos);
        }
        [HttpGet("ClientBalance")]
        public IActionResult ClientBalance()
        {
            var clients = this.Context.Clients.ToList();
            List<ClientBlanaceDto> clientBlanaceDtos = new List<ClientBlanaceDto>();
            foreach (var item in clients)
            {
                var totalOrder = this.Context.Orders.Where(c => c.ClientId == item.Id && ((c.OrderStateId != (int)OrderStateEnum.Processing && c.IsClientDiliverdMoney == true) || c.OrderStateId == (int)OrderStateEnum.ShortageOfCash)).Sum(c => c.Cost - c.DeliveryCost);
                if (totalOrder + item.Total == 0)
                    continue;
                clientBlanaceDtos.Add(new ClientBlanaceDto()
                {
                    ClientName = item.Name,
                    Account = item.Total,
                    TotalOrder = totalOrder
                });
            }
            return Ok(clientBlanaceDtos);
        }
        //[HttpGet("ClientClearing")]
        //public IActionResult ClientClearing()
        //{

        //    return Ok();
        //}
    }
}