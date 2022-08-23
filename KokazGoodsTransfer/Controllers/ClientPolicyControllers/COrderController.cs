using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COrderController : AbstractClientPolicyController
    {
        private readonly IOrderClientSerivce _orderClientSerivce;
        private readonly INotificationService _notificationService;
        private readonly IReceiptService _receiptService;
        public COrderController(IOrderClientSerivce orderClientSerivce, INotificationService notificationService, IReceiptService receiptService)
        {
            _orderClientSerivce = orderClientSerivce;
            _notificationService = notificationService;
            _receiptService = receiptService;
        }
        /// <summary>
        /// إضافة طلب
        /// </summary>
        /// <param name="createOrderFromClient"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderFromClient createOrderFromClient)
        {
            var validate = await _orderClientSerivce.Validate(createOrderFromClient);
            if (validate.Count != 0)
            {
                return Conflict(new { messages = validate });
            }
            return Ok(await _orderClientSerivce.Create(createOrderFromClient));
        }
        [HttpPost("UploadExcel")]
        public async Task<IActionResult> UploadExcel(IFormFile file, [FromForm] DateTime dateTime)
        {
            return Ok(await _orderClientSerivce.CreateFromExcel(file, dateTime));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("codeExist")]
        public async Task<IActionResult> CheckCodeExist([FromQuery] string code)
        {
            return Ok(await _orderClientSerivce.CodeExist(code));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _orderClientSerivce.GetById(id));
        }
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditOrder editOrder)
        {
            await _orderClientSerivce.Edit(editOrder);
            return Ok();

        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] COrderFilter orderFilter)
        {
            var paginResult = await _orderClientSerivce.Get(pagingDto, orderFilter);
            return Ok(new { data = paginResult.Data, total = paginResult.Total });
        }
        [HttpGet("NonSendOrder")]
        public async Task<IActionResult> NonSendOrder()
        {
            return Ok(await _orderClientSerivce.NonSendOrder());
        }
        [HttpPost("Sned")]
        public async Task<IActionResult> Send([FromBody] int[] ids)
        {
            await _orderClientSerivce.Send(ids);
            return Ok();
        }
        [HttpGet("OrdersDontFinished")]
        public async Task<IActionResult> OrdersDontFinished([FromQuery] OrderDontFinishFilter orderDontFinishFilter)
        {
            return Ok(await _orderClientSerivce.OrdersDontFinished(orderDontFinishFilter));
        }
        [HttpGet("UnPaidRecipt")]
        public async Task<IActionResult> UnPaidRecipt()
        {
            return Ok(await _receiptService.UnPaidRecipt(AuthoticateUserId()));
        }

        [HttpGet("NewNotfiaction")]
        public async Task<IActionResult> NewNotfiaction()
        {
            return Ok(await _notificationService.NewNotfiactionCount());
        }
        [HttpGet("Notifcation")]
        public async Task<IActionResult> Notifcation()
        {
            return Ok(await _notificationService.GetClientNotifcations());

        }
        [HttpPut("SeeNotifactions")]
        public async Task<IActionResult> SeeNotifactions([FromBody] int[] ids)
        {
            await _notificationService.SeeNotifactions(ids);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderClientSerivce.Delete(id);
            return Ok();
        }
        [HttpGet("OrdersNeedToRevision")]
        public async Task<IActionResult> OrdersNeedToRevision()
        {
            return Ok(await _orderClientSerivce.OrdersNeedToRevision());
        }
        [HttpPut("CorrectOrderCountry")]
        public async Task<IActionResult> CorrectOrderCountry(List<KeyValuePair<int, int>> pairs)
        {
            await _orderClientSerivce.CorrectOrderCountry(pairs);
            return Ok();
        }
    }
}