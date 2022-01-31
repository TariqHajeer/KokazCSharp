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
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.HubsConfig;

namespace KokazGoodsTransfer.Controllers.AgentPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentOrderController : AbstractAgentController
    {
        private readonly NotificationHub _notificationHub;
        public AgentOrderController(KokazContext context, IMapper mapper, Logging logging, NotificationHub notificationHub) : base(context, mapper, logging)
        {
            _notificationHub = notificationHub;
        }
        [HttpGet("InStock")]
        public async Task<IActionResult> GetInStock()
        {
            var orders = await this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId())
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                 .ToListAsync();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("InWay")]
        public async Task<IActionResult> GetInWay()
        {
            var orders = await this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove))
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                 .Include(c => c.Orderplaced)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                 .ToListAsync();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("InWayByCode")]
        public async Task<IActionResult> InWayByCode([FromQuery] string code)
        {
            var orders = await this.Context.Orders.Where(c => c.Code == code && c.AgentId == AuthoticateUserId() && c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove)).OrderByDescending(c => c.Date)
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                 .Include(c => c.Orderplaced)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                .ToListAsync();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }

        [HttpGet("OwedOrder")]
        public async Task<IActionResult> OwedOrder()
        {
            var orders = await this.Context.Orders.Where(c => c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent || (c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove || c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve))))
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                 .Include(c => c.Orderplaced)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print).ToListAsync();
            return Ok(mapper.Map<OrderDto[]>(orders));

        }
        /// <summary>
        /// الطلبات المعلقة
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        [HttpGet("OrderSuspended")]
        public async Task<IActionResult> OrderSuspended([FromQuery] DateTime dateTime)
        {
            var date = dateTime.AddDays(-4);
            var orders = await this.Context.Orders.Where(c => c.AgentId == AuthoticateUserId() && c.Date <= date && (c.MoenyPlacedId < (int)MoneyPalcedEnum.InsideCompany))
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                .Include(c => c.OrderPrints)
                    .ThenInclude(c => c.Print)
                 .ToListAsync();
            return Ok(mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("Prints")]
        public async Task<IActionResult> GetPrint([FromQuery] PagingDto pagingDto, [FromQuery] PrintFilterDto printFilterDto)
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
            var total = await printeds.CountAsync();
            var list = await printeds.Skip(pagingDto.Page - 1).Take(pagingDto.RowCount * pagingDto.Page)
                .ToListAsync();
            return Ok(new { total, Data = mapper.Map<PrintOrdersDto[]>(list) });
        }
        [HttpGet("Print")]
        public async Task<IActionResult> GetPrintById([FromQuery] int printNumber)
        {
            var printed = await this.Context.Printeds.Where(c => c.PrintNmber == printNumber && c.Type == PrintType.Agent)
                .Include(c => c.AgnetPrints)
                .FirstOrDefaultAsync();
            if (printed == null)
            {
                return Conflict();
            }
            var x = mapper.Map<PrintOrdersDto>(printed);
            return Ok(x);
        }
        [HttpPost("SetOrderPlaced")]
        public async Task<IActionResult> SetOrderState([FromBody] List<AgentOrderStateDto> agentOrderStateDtos)
        {
            var orders = await this.Context.Orders.Where(c => agentOrderStateDtos.Select(c => c.Id).ToList().Contains(c.Id)).ToListAsync();
            agentOrderStateDtos.ForEach(c =>
            {
                var temp = new ApproveAgentEditOrderRequest()
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
            await this.Context.SaveChangesAsync();
            var orderRequestEditStateCount = await this.Context.ApproveAgentEditOrderRequests.Where(c => c.IsApprove == null).CountAsync();
            AdminNotification adminNotification = new AdminNotification()
            {

                OrderRequestEditStateCount = orderRequestEditStateCount,

            };
            await _notificationHub.AdminNotifcation(adminNotification);
            return Ok();
        }
        [HttpGet("GetOrderPlaced")]
        public async Task<IActionResult> GetOrderPlaced()
        {
            var orderPlaceds = await this.Context.OrderPlaceds.ToListAsync();
            return Ok(mapper.Map<NameAndIdDto[]>(orderPlaceds));
        }
        [HttpGet("GetAgentStatics")]
        public async Task<IActionResult> GetAgentStatics([FromQuery] DateTime dateTime)
        {
            var date = dateTime.AddDays(-4);
            AgentStaticsDto mainStatics = new AgentStaticsDto()
            {
                TotalOrderInSotre =await this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId()).CountAsync(),
                TotalOrderInWay =await this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove)).CountAsync(),
                TotlaOwedOrder =await this.Context.Orders.Where(c => c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent || (c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove || c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve)))).CountAsync(),
                TotlaPrintOrder =await this.Context.Printeds.Where(c => c.Type == PrintType.Agent)
                .Where(c => c.OrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId())).CountAsync(),
                TotalOrderSuspended = await this.Context.Orders.Where(c => c.AgentId == AuthoticateUserId() && c.Date <= date && (c.MoenyPlacedId < (int)MoneyPalcedEnum.InsideCompany)).CountAsync()
            };
            return Ok(mainStatics);
        }


    }
}
