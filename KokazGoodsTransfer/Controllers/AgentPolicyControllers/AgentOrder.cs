using AutoMapper;
using KokazGoodsTransfer.Dtos.AgentDtos;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Services.Interfaces;

namespace KokazGoodsTransfer.Controllers.AgentPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentOrderController : AbstractAgentController
    {
        private readonly NotificationHub _notificationHub;
        private readonly IIndexService<OrderPlaced> _indexService;
        private readonly IOrderService _orderService;
        private readonly IAgentPrintService _agentPrintService;
        public AgentOrderController(KokazContext context, IMapper mapper, NotificationHub notificationHub, IIndexService<OrderPlaced> indexService,
            IOrderService orderService,
            IAgentPrintService agentPrintService) : base(context, mapper)
        {
            _notificationHub = notificationHub;
            _indexService = indexService;
            _orderService = orderService;
            _agentPrintService = agentPrintService;
        }
        [HttpGet("InStock")]
        public async Task<IActionResult> GetInStock()
        {
            var includes = new string[] { "Client", "Country", "Region", "AgentOrderPrints.AgentPrint" };
            var orders = await _orderService.GetAsync(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId(), includes);
            return Ok(orders);
        }
        [HttpGet("InWay")]
        public async Task<IActionResult> GetInWay()
        {
            var includes = new string[] { "Client", "Country", "Region", "AgentOrderPrints.AgentPrint", "Orderplaced" };
            var orders = await _orderService.GetAsync(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove), includes);
            return Ok(orders);
        }
        [HttpGet("InWayByCode")]
        public async Task<IActionResult> InWayByCode([FromQuery] string code)
        {
            var includes = new string[] { "Client", "Country", "Region", "AgentOrderPrints.AgentPrint", "Orderplaced" };
            var orders = await _orderService.GetAsync(c => c.Code == code && c.AgentId == AuthoticateUserId() && c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove), includes);
            orders = orders.OrderBy(c => c.Date);
            return Ok(orders);
        }

        [HttpGet("OwedOrder")]
        public async Task<IActionResult> OwedOrder()
        {
            var includes = new string[] { "Client", "Country", "Region", "AgentOrderPrints.AgentPrint", "Orderplaced" };
            var orders = await _orderService.GetAsync(c => c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent || (c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove || c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve))), includes);
            return Ok(orders);

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
            var includes = new string[] { "Client", "Country", "Region", "AgentOrderPrints.AgentPrint", "Orderplaced" };
            var orders = await _orderService.GetAsync(c => c.AgentId == AuthoticateUserId()
            && c.Date <= date && (c.MoenyPlacedId < (int)MoneyPalcedEnum.InsideCompany), includes);
            return Ok(orders);
        }
        [HttpGet("Prints")]
        public async Task<IActionResult> GetPrint([FromQuery] PagingDto pagingDto, [FromQuery] PrintFilterDto printFilterDto)
        {
            var reult = await _agentPrintService.GetPrint(pagingDto, printFilterDto);
            return Ok(reult);
        }
        [HttpGet("Print")]
        public async Task<IActionResult> GetPrintById([FromQuery] int printNumber)
        {
            return Ok(await _agentPrintService.GetPrintById(printNumber));
        }
        [HttpPost("SetOrderPlaced")]
        public async Task<IActionResult> SetOrderState([FromBody] List<AgentOrderStateDto> agentOrderStateDtos)
        {
            var orders = await this._context.Orders.Where(c => agentOrderStateDtos.Select(c => c.Id).ToList().Contains(c.Id)).ToListAsync();
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
                this._context.Add(temp);
            });
            orders.ForEach(c =>
            {
                c.AgentRequestStatus = (int)AgentRequestStatusEnum.Pending;
            });
            await this._context.SaveChangesAsync();
            var orderRequestEditStateCount = await this._context.ApproveAgentEditOrderRequests.Where(c => c.IsApprove == null).CountAsync();
            AdminNotification adminNotification = new AdminNotification()
            {

                OrderRequestEditStateCount = orderRequestEditStateCount,

            };
            await _notificationHub.AdminNotifcation(adminNotification);
            return Ok();
        }
        [HttpGet("GetOrderPlaced")]
        public async Task<ActionResult<IEnumerable<NameAndIdDto>>> GetOrderPlaced()
        {
            var orderPlaceds = await _indexService.GetAllLite();
            return Ok(orderPlaceds);
        }
        [HttpGet("GetAgentStatics")]
        public async Task<IActionResult> GetAgentStatics([FromQuery] DateTime dateTime)
        {
            var date = dateTime.AddDays(-4);
            AgentStaticsDto mainStatics = new AgentStaticsDto()
            {
                TotalOrderInSotre = await _orderService.Count(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId()),
                TotalOrderInWay = await _orderService.Count(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove)),
                TotlaOwedOrder = await _orderService.Count(c => c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent || (c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove || c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve)))),
                TotlaPrintOrder = await _agentPrintService.Count(c => c.AgentOrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId())),
                TotalOrderSuspended = await _orderService.Count(c => c.AgentId == AuthoticateUserId() && c.Date <= date && (c.MoenyPlacedId < (int)MoneyPalcedEnum.InsideCompany))
            };
            return Ok(mainStatics);
        }
    }
}
