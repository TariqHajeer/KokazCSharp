using AutoMapper;
using KokazGoodsTransfer.Dtos.AgentDtos;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.Swagger.Annotations;

namespace KokazGoodsTransfer.Controllers.AgentPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentOrderController : AbstractAgentController
    {
        public AgentOrderController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        //[HttpGet("Order")]
        //public IActionResult GetOrder()
        //{
        //    var orders = this.Context.Orders.Where(c => c.AgentId == AuthoticateUserId()).ToList();
        //    return Ok(mapper.Map<OrderDto[]>(orders));
        //}
        [HttpGet("InStock")]
        public IActionResult GetInStock()
        {
            var orders = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId())
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                 .ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("InWay")]
        public IActionResult GetInWay()
        {
            var orders = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove))
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                 .Include(c => c.Orderplaced)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                 .ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }

        [HttpGet("OwedOrder")]
        public IActionResult OwedOrder()
        {
            var orders = this.Context.Orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent || (c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove || c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve)) && c.AgentId == AuthoticateUserId())
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                 .Include(c => c.Orderplaced)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print);
            return Ok(mapper.Map<OrderDto[]>(orders));

        }
        [HttpGet("OrderSuspended")]
        public IActionResult OrderSuspended()
        {
            var orders = this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Processing && (c.OrderplacedId >= (int)OrderplacedEnum.Way || c.OrderplacedId == (int)OrderplacedEnum.Delayed) && c.AgentId == AuthoticateUserId())
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                 .ToList();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("Prints")]
        public IActionResult GetPrint([FromQuery] PagingDto pagingDto, [FromQuery] PrintFilterDto printFilterDto)
        {
            var printeds = this.Context.Printeds.Where(c => c.Type == PrintType.Agent);
            if (printFilterDto.Date != null)
            {
                printeds = printeds.Where(c => c.Date == printFilterDto.Date);
            }
            if (printFilterDto.Number != null)
            {
                printeds = printeds.Where(c => c.PrintNmber == printFilterDto.Number);
            }
            printeds = printeds.Where(c => c.OrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId()));
            var total = printeds.Count();
            var list = printeds.Skip(pagingDto.Page - 1).Take(pagingDto.RowCount * pagingDto.Page)
                .ToList();
            return Ok(new { total, Data = mapper.Map<PrintOrdersDto[]>(list) });
        }
        [HttpGet("Print")]
        public IActionResult GetPrintById([FromQuery] int printNumber)
        {
            var printed = this.Context.Printeds.Where(c => c.PrintNmber == printNumber && c.Type == PrintType.Agent)
                .Include(c => c.AgnetPrints)
                .FirstOrDefault();
            if (printed == null)
            {
                return Conflict();
                //this.err.Messges.Add($"رقم الطباعة غير موجود");
                //return Conflict(this.err);
            }
            var x = mapper.Map<PrintOrdersDto>(printed);
            return Ok(x);
        }
        [HttpPost("SetOrderPlaced")]
        public IActionResult SetOrderState([FromBody] List<AgentOrderStateDto> agentOrderStateDtos)
        {
            var orders = this.Context.Orders.Where(c => agentOrderStateDtos.Select(c => c.Id).ToList().Contains(c.Id)).ToList();
            agentOrderStateDtos.ForEach(c =>
            {
                ApproveAgentEditOrderRequest temp = new ApproveAgentEditOrderRequest()
                {
                    AgentId = AuthoticateUserId(),
                    IsApprove = null,
                    NewAmount = c.Cost,
                    OrderId = c.Id,
                    OrderPlacedId = c.OrderplacedId,
                };
                this.Context.Add(temp);
            });
            orders.ForEach(c =>
            {
                c.AgentRequestStatus = (int)AgentRequestStatusEnum.Pending;
            });
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpGet("GetOrderPlaced")]
        public IActionResult GetOrderPlaced()
        {
            return Ok(mapper.Map<NameAndIdDto[]>(this.Context.OrderPlaceds.ToList()));
        }
        [HttpGet("GetAgentStatics")]
        public IActionResult GetAgentStatics()
        {
            AgentStaticsDto mainStatics = new AgentStaticsDto()
            {
                TotalOrderInSotre = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId()).Count(),
                TotalOrderInWay = this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove)).Count(),
                TotlaOwedOrder = this.Context.Orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent || (c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove || c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve)) && c.AgentId == AuthoticateUserId()).Count(),
                TotlaPrintOrder=this.Context.Printeds.Where(c => c.Type == PrintType.Agent)
                .Where(c => c.OrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId())).Count(),
                TotalOrderSuspended=this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Processing && (c.OrderplacedId >= (int)OrderplacedEnum.Way || c.OrderplacedId == (int)OrderplacedEnum.Delayed) && c.AgentId == AuthoticateUserId()).Count()
            };
            return Ok(mainStatics);
        }
        private List<string> Validation()
        {
            return new List<string>();
        }

    }
}
