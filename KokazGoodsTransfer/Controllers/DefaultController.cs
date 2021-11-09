using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.SignalR;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.NotifcationDtos;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : AbstractController
    {
        NotificationHub notificationHub;
        public DefaultController(KokazContext context, IMapper mapper, NotificationHub notificationHub) : base(context, mapper)
        {
            this.notificationHub = notificationHub;
            
        }

        [HttpGet("GetNo")]
        public async Task<IActionResult> GetNo()
        {
            
            var uId = AuthoticateUserId();
            var nos = this.Context.Notfications.Where(c => c.ClientId == uId && c.IsSeen == false).ToList();
            var dto=  mapper.Map<NotficationDto[]>(nos );
            await notificationHub.AllNotification(AuthoticateUserId().ToString(),dto);
            return Ok();
        }

        //[Authorize]
        //[HttpGet("Check")]
        //public IActionResult ChekcLogin()
        //{

        //    return Ok();
        //}

        //[HttpGet]
        //public IActionResult Get()
        //{

        //    var clientPrints = this.Context.ClientPrints
        //        .Include(c => c.Print)
        //            .ThenInclude(c => c.OrderPrints)
        //                .ThenInclude(c => c.Order)
        //         .Where(c => c.Print.PrintNmber == 654 || c.Print.PrintNmber == 655)
        //         .ToList();
        //    clientPrints.ForEach(c =>
        //    {
        //        c.PayForClient = GetPayForClient(c);
        //    });
        //    return Ok("it's work");
        //}         
        //public decimal GetPayForClient(ClientPrint clientPrint)
        //{
        //    var order = clientPrint.Print.OrderPrints.Select(c => c.Order).Where(c => c.Code == clientPrint.Code).Single();

        //    if (clientPrint.OrderPlacedId == (int)OrderplacedEnum.Way)
        //    {
        //        var origlnalCost = order.OldCost == null ? order.Cost : order.OldCost;
        //        var orignalDeliverCost = order.OldDeliveryCost == null ? order.DeliveryCost : order.OldDeliveryCost;
        //        return (decimal)(origlnalCost - orignalDeliverCost);
        //    }
        //    if (clientPrint.OrderPlacedId == (int)OrderplacedEnum.Delivered)
        //    {
        //        return order.Cost - order.DeliveryCost;
        //    }
        //    if (clientPrint.OrderPlacedId == (int)OrderplacedEnum.CompletelyReturned)
        //    {

        //    }
        //    return 0;
        //}


        //[HttpGet("connection")]
        //public IActionResult GetConnectonString()
        //{
        //    return Ok(this.Context.Database.GetDbConnection().ConnectionString);
        //}
        //[HttpGet("status")]
        //public IActionResult GetStatus()
        //{
        //    try
        //    {
        //        return Ok(Context.Database.CanConnect().ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok("False");
        //    }
        //}
        //[HttpGet("Currencies")]
        //public IActionResult GetDepartmnetsName()
        //{
        //    try
        //    {
        //        return Ok(this.Context.Currencies.Select(c => c.Name));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }
        //}
    }
}