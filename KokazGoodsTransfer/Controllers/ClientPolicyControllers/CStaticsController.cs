using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.ClientDtos;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
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
        public IActionResult Get()
        {
            var orders = this.Context.Orders.Where(c => c.ClientId == AuthoticateUserId());
            var ordersInsiedCompany = orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany);
            StaticsDto staticsDto = new StaticsDto
            {
                TotalOrder = orders.Count(),
                //OrderMoneyInCompany = ordersInsiedCompany.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Delivered || c.OrderplacedId == (int)OrderplacedEnum.PartialReturned).Count(),
                OrderMoneyInCompany = ordersInsiedCompany.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Delivered || c.OrderplacedId == (int)OrderplacedEnum.PartialReturned || c.OrderStateId == (int)OrderStateEnum.ShortageOfCash).Count(),
                //OrderDeliverdToClient = orders.Where(c => c.OrderStateId != (int)OrderStateEnum.Finished && c.IsClientDiliverdMoney == true).Count(),
                OrderDeliverdToClient = orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent && (c.OrderplacedId == (int)OrderplacedEnum.PartialReturned || c.OrderplacedId == (int)OrderplacedEnum.Delivered)).Count(),
                OrderMoneyDelived = orders.Where(c => c.IsClientDiliverdMoney == true && (c.OrderplacedId == (int)OrderplacedEnum.Way || c.OrderplacedId == (int)OrderplacedEnum.Delivered)).Count(),
                OrderInWat = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way&&c.IsClientDiliverdMoney!=true).Count(),
                OrderInStore = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store).Count(),
                OrderWithClient = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Client).Count(),
                OrderComplitlyReutrndInCompany = ordersInsiedCompany.Where(c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable).Count(),
                OrderComplitlyReutrndDeliverd = orders.Where(c => (c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned|| c.OrderplacedId == (int)OrderplacedEnum.Unacceptable) && c.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered).Count(),
                DelayedOrder = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Delayed).Count(),
                OrderPartialReturned = orders.Where(c=>c.OrderplacedId==(int)OrderplacedEnum.PartialReturned).Count()
            };
            //var orderInCompany = orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany && c.OrderplacedId != (int)OrderplacedEnum.Delayed&&c.OrderplacedId!=(int)OrderplacedEnum.CompletelyReturned);
            //StaticsDto staticsDto = new StaticsDto
            //{
            //    TotalOrder = orders.Count(),
            //    OrderWithClient = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Client).Count(),
            //    OrderInWay = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way).Count(),
            //    OrderInCompany = orderInCompany.Count(),
            //    DeliveredOrder = orderInCompany.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Delivered).Count(),
            //    UnacceptableOrder = orderInCompany.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Unacceptable).Count(),
            //    CompletelyReturnedOrder = orderInCompany.Where(c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned).Count(),
            //    PartialReturnedOrder = orderInCompany.Where(c => c.OrderplacedId == (int)OrderplacedEnum.PartialReturned).Count(),
            //    DelayedOrder = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Delayed).Count(),
            //};
            return Ok(staticsDto);
        }

        [HttpGet("GetNo")]
        public async Task<IActionResult> GetNo()
        {

            var uId = AuthoticateUserId();
            var nos = this.Context.Notfications.Where(c => c.ClientId == uId && c.IsSeen == false)
                .Include(c=>c.MoneyPlaced)
                .Include(c=>c.OrderPlaced)
                .ToList();
            var dto = mapper.Map<NotficationDto[]>(nos);
            await notificationHub.AllNotification(AuthoticateUserId().ToString(), dto);
            return Ok();
        }
    }
}