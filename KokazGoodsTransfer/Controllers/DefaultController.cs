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
using KokazGoodsTransfer.Helpers;

namespace KokazGoodsTransfer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultController : AbstractController
    {
        NotificationHub notificationHub;
        public DefaultController(KokazContext context, IMapper mapper, NotificationHub notificationHub, Logging logging) : base(context, mapper, logging)
        {
            this.notificationHub = notificationHub;
            
        }

        [HttpGet("GetNo")]
        public async Task<IActionResult> GetNo()
        {
            
            var uId = AuthoticateUserId();
            var nos = this.Context.Notfications.Where(c => c.ClientId == uId && c.IsSeen == false)
                .Include(c=>c.OrderPlaced)
                .Include(c=>c.MoneyPlaced)
                .ToList();
            var dto=  mapper.Map<NotficationDto[]>(nos );
            await notificationHub.AllNotification(AuthoticateUserId().ToString(),dto);
            return Ok();
        }
        [HttpGet("GetHash")]
        public string GetShash(string x)
        {
            return MD5Hash.GetMd5Hash(x);
        }
        
    }
}