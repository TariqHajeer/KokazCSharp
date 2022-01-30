using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.PayemntRequestDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPaymentRequestController : AbstractClientPolicyController
    {
        private readonly NotificationHub _notificationHub;
        public CPaymentRequestController(KokazContext context, IMapper mapper, Logging logging, NotificationHub notificationHub) : base(context, mapper, logging)
        {
            this._notificationHub = notificationHub;
        }
        [HttpGet("CanRequest")]
        public IActionResult CanRequest()
        {
            return Ok(!this.Context.PaymentRequests.Any(c => c.ClientId == AuthoticateUserId() && c.Accept == null));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequestDto createPaymentRequestDto)
        {
            PaymentRequest paymentRequest = new PaymentRequest()
            {
                PaymentWayId = createPaymentRequestDto.PaymentWayId,
                Note = createPaymentRequestDto.Note,
                ClientId = AuthoticateUserId(),
                CreateDate = createPaymentRequestDto.Date,
                Accept = null
            };
            this.Context.Add(paymentRequest);
            await this.Context.SaveChangesAsync();
            var newPaymentRequetsCount = await this.Context.PaymentRequests
                .Where(c => c.Accept == null).CountAsync();
            var adminNotification = new AdminNotification()
            {
                NewPaymentRequetsCount = newPaymentRequetsCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
            return Ok(mapper.Map<PayemntRquestDto>(paymentRequest));
        }
        [HttpGet]
        public IActionResult Get([FromQuery] PagingDto pagingDto)
        {
            var paymentRequests = this.Context.PaymentRequests
                .Include(c => c.PaymentWay)
                .Where(c => c.ClientId == AuthoticateUserId());
            var total = paymentRequests.Count();
            paymentRequests = paymentRequests.OrderByDescending(c => c.Id).Skip(pagingDto.Page - 1).Take(pagingDto.RowCount);
            var temp = paymentRequests.ToList();
            var dto = mapper.Map<PayemntRquestDto[]>(temp);
            return Ok(new { total, dto });
        }
        [HttpGet("GetPaymentWay")]
        public IActionResult GetPaymentWay()
        {
            var paymentWay = this.Context.PaymentWays.ToList();
            return Ok(mapper.Map<NameAndIdDto[]>(paymentWay));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var payemntRquest = this.Context.PaymentRequests.Find(id);
            if (payemntRquest.Accept != null)
                return Conflict();
            this.Context.PaymentRequests.Remove(payemntRquest);
            this.Context.SaveChanges();
            return Ok();
        }

    }
}