using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.ClientDtos;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CStaticsController : AbstractClientPolicyController
    {
        NotificationHub notificationHub;
        public CStaticsController(KokazContext context, IMapper mapper, NotificationHub notificationHub) : base(context, mapper)
        {
            this.notificationHub = notificationHub;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = this._context.Orders.Where(c => c.ClientId == AuthoticateUserId());
            var ordersInsiedCompany = orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany);
            StaticsDto staticsDto = new StaticsDto
            {
                TotalOrder = await orders.CountAsync(),
                OrderMoneyInCompany = await ordersInsiedCompany.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Delivered || c.OrderplacedId == (int)OrderplacedEnum.PartialReturned || c.OrderStateId == (int)OrderStateEnum.ShortageOfCash).CountAsync(),
                OrderDeliverdToClient = await orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent && (c.OrderplacedId == (int)OrderplacedEnum.PartialReturned || c.OrderplacedId == (int)OrderplacedEnum.Delivered)).CountAsync(),
                OrderMoneyDelived =await orders.Where(c => c.IsClientDiliverdMoney == true && (c.OrderplacedId == (int)OrderplacedEnum.Way || c.OrderplacedId == (int)OrderplacedEnum.Delivered)).CountAsync(),
                OrderInWat = await orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way && c.IsClientDiliverdMoney != true).CountAsync(),
                OrderInStore = await orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store).CountAsync(),
                OrderWithClient = await orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Client).CountAsync(),
                OrderComplitlyReutrndInCompany = await ordersInsiedCompany.Where(c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable).CountAsync(),
                OrderComplitlyReutrndDeliverd =await orders.Where(c => (c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable) && c.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered).CountAsync(),
                DelayedOrder =await orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Delayed).CountAsync(),
                OrderPartialReturned = await orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.PartialReturned).CountAsync()
            };
            return Ok(staticsDto);
        }

        [HttpGet("GetNo")]
        public async Task<IActionResult> GetNo()
        {

            var uId = AuthoticateUserId();
            var nos = await this._context.Notfications.Where(c => c.ClientId == uId && c.IsSeen == false)
                .Include(c => c.MoneyPlaced)
                .Include(c => c.OrderPlaced)
                .ToListAsync();
            var dto = _mapper.Map<NotificationDto[]>(nos);
            await notificationHub.AllNotification(AuthoticateUserId().ToString(), dto);
            return Ok();
        }
    }
}