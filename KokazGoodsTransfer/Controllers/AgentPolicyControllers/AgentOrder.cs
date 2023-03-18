using KokazGoodsTransfer.Dtos.AgentDtos;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Services.Interfaces;

namespace KokazGoodsTransfer.Controllers.AgentPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentOrderController : AbstractAgentController
    {
        private readonly IOrderService _orderService;
        private readonly IAgentPrintService _agentPrintService;
        public AgentOrderController(IOrderService orderService,
            IAgentPrintService agentPrintService)
        {
            _orderService = orderService;
            _agentPrintService = agentPrintService;
        }
        [HttpGet("InStock")]
        public async Task<IActionResult> GetInStock()
        {
            var includes = new string[] { nameof(Order.Client), nameof(Order.Country), nameof(Order.Region), $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}" };
            var orders = await _orderService.GetAsync(c => c.OrderPlace == OrderPlace.Store && c.AgentId == AuthoticateUserId(), includes);
            return Ok(orders);
        }
        [HttpGet("InWay")]
        public async Task<IActionResult> GetInWay()
        {
            var includes = new string[] { nameof(Order.Client), nameof(Order.Country), nameof(Order.Region), $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}" };
            var orders = await _orderService.GetAsync(c => c.OrderPlace == OrderPlace.Way
            && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None
            || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove), includes);
            return Ok(orders);
        }
        [HttpGet("InWayByCode")]
        public async Task<IActionResult> InWayByCode([FromQuery] string code)
        {
            var includes = new string[] { nameof(Order.Client), nameof(Order.Country), nameof(Order.Region), $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}" };
            var orders = await _orderService.GetAsync(c => c.Code == code && c.AgentId == AuthoticateUserId() && c.OrderPlace == OrderPlace.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove), includes);
            orders = orders.OrderBy(c => c.Date);
            if (!orders.Any())
                return NotFound();
            return Ok(orders);
        }

        [HttpGet("OwedOrder")]
        public async Task<IActionResult> OwedOrder()
        {
            var includes = new string[] { nameof(Order.Client), nameof(Order.Country), nameof(Order.Region), $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}" };
            var orders = await _orderService.GetAsync(c => c.AgentId == AuthoticateUserId() &&
            (c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending ||
            c.MoneyPlace == MoneyPalced.WithAgent ||
            (c.OrderPlace == OrderPlace.Way &&
            (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove ||
            c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve))), includes);
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
            var includes = new string[] { nameof(Order.Client), nameof(Order.Country), nameof(Order.Region), $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}" };
            var orders = await _orderService.GetAsync(c => c.AgentId == AuthoticateUserId()
            && c.Date <= date && (c.MoneyPlace < MoneyPalced.InsideCompany), includes);
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
            await _agentPrintService.SetOrderState(agentOrderStateDtos);
            return Ok();
        }
        [HttpGet("GetAgentStatics")]
        public async Task<IActionResult> GetAgentStatics([FromQuery] DateTime dateTime)
        {
            var date = dateTime.AddDays(-4);
            AgentStaticsDto mainStatics = new AgentStaticsDto()
            {
                TotalOrderInSotre = await _orderService.Count(c => c.OrderPlace == OrderPlace.Store && c.AgentId == AuthoticateUserId()),
                TotalOrderInWay = await _orderService.Count(c => c.OrderPlace == OrderPlace.Way && c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.None || c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove)),
                TotlaOwedOrder = await _orderService.Count(c => c.AgentId == AuthoticateUserId() && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending || c.MoneyPlace == MoneyPalced.WithAgent || (c.OrderPlace == OrderPlace.Way && (c.AgentRequestStatus == (int)AgentRequestStatusEnum.DisApprove || c.AgentRequestStatus == (int)AgentRequestStatusEnum.Approve)))),
                TotlaPrintOrder = await _agentPrintService.Count(c => c.AgentOrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId())),
                TotalOrderSuspended = await _orderService.Count(c => c.AgentId == AuthoticateUserId() && c.Date <= date && (c.MoneyPlace < MoneyPalced.InsideCompany))
            };
            return Ok(mainStatics);
        }
    }
}
