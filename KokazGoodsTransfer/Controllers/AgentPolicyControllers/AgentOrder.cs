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
using KokazGoodsTransfer.Helpers;
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
        public AgentOrderController(KokazContext context, IMapper mapper, NotificationHub notificationHub, IIndexService<OrderPlaced> indexService) : base(context, mapper)
        {
            _notificationHub = notificationHub;
            _indexService = indexService;
        }
        [HttpGet("InStock")]
        public async Task<IActionResult> GetInStock()
        {
            var orders = await this._context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId())
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                .Include(c => c.AgentOrderPrints)
                    .ThenInclude(c => c.AgentPrint)
                 .ToListAsync();
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("InWay")]
        public async Task<IActionResult> GetInWay()
        {
            var orders = await this._context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove))
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                 .Include(c => c.Orderplaced)
                .Include(c => c.AgentOrderPrints)
                    .ThenInclude(c => c.AgentPrint)
                 .ToListAsync();
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("InWayByCode")]
        public async Task<IActionResult> InWayByCode([FromQuery] string code)
        {
            var orders = await this._context.Orders.Where(c => c.Code == code && c.AgentId == AuthoticateUserId() && c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove)).OrderByDescending(c => c.Date)
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                 .Include(c => c.Orderplaced)
                .Include(c => c.AgentOrderPrints)
                    .ThenInclude(c => c.AgentPrint)
                .ToListAsync();
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }

        [HttpGet("OwedOrder")]
        public async Task<IActionResult> OwedOrder()
        {
            var orders = await this._context.Orders.Where(c => c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent || (c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove || c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve))))
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                 .Include(c => c.Orderplaced)
                .Include(c => c.AgentOrderPrints)
                    .ThenInclude(c => c.AgentPrint)
                    .ToListAsync();
            return Ok(_mapper.Map<OrderDto[]>(orders));

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
            var orders = await this._context.Orders.Where(c => c.AgentId == AuthoticateUserId() && c.Date <= date && (c.MoenyPlacedId < (int)MoneyPalcedEnum.InsideCompany))
                .Include(c => c.Client)
                .Include(c => c.Country)
                .Include(c => c.Client)
                .Include(c => c.Region)
                .Include(c => c.AgentOrderPrints)
                    .ThenInclude(c => c.AgentPrint)
                 .ToListAsync();
            return Ok(_mapper.Map<OrderDto[]>(orders));
        }
        [HttpGet("Prints")]
        public async Task<IActionResult> GetPrint([FromQuery] PagingDto pagingDto, [FromQuery] PrintFilterDto printFilterDto)
        {
            var prints = this._context.AgentPrints.AsQueryable();

            if (printFilterDto.Date != null)
            {
                prints = prints.Where(c => c.Date == printFilterDto.Date);
            }
            if (printFilterDto.Number != null)
            {
                prints = prints.Where(c => c.Id == printFilterDto.Number);
            }
            prints = prints.Where(c => c.AgentOrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId()));
            var total = await prints.CountAsync();
            var list = await prints.Skip(pagingDto.Page - 1).Take(pagingDto.RowCount * pagingDto.Page)
                .ToListAsync();
            return Ok(new { total, Data = _mapper.Map<PrintOrdersDto[]>(list) });
        }
        [HttpGet("Print")]
        public async Task<IActionResult> GetPrintById([FromQuery] int printNumber)
        {
            var printed = await this._context.AgentPrints.Where(c => c.Id == printNumber)
                .Include(c => c.AgentPrintDetails)
                .FirstOrDefaultAsync();
            if (printed == null)
            {
                return Conflict();
            }
            var x = _mapper.Map<PrintOrdersDto>(printed);
            return Ok(x);
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
                TotalOrderInSotre = await this._context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store && c.AgentId == AuthoticateUserId()).CountAsync(),
                TotalOrderInWay = await this._context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove)).CountAsync(),
                TotlaOwedOrder = await this._context.Orders.Where(c => c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent || (c.OrderplacedId == (int)OrderplacedEnum.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove || c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve)))).CountAsync(),
                TotlaPrintOrder = await this._context.AgentPrints.Where(c => c.AgentOrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId())).CountAsync(),
                TotalOrderSuspended = await this._context.Orders.Where(c => c.AgentId == AuthoticateUserId() && c.Date <= date && (c.MoenyPlacedId < (int)MoneyPalcedEnum.InsideCompany)).CountAsync()
            };
            return Ok(mainStatics);
        }


    }
}
