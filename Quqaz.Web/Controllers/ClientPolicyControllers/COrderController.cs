using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wkhtmltopdf.NetCore;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COrderController : AbstractClientPolicyController
    {
        private readonly IOrderClientSerivce _orderClientSerivce;
        private readonly INotificationService _notificationService;
        private readonly IReceiptService _receiptService;
        private readonly IGeneratePdf _generatePdf;
        public COrderController(IOrderClientSerivce orderClientSerivce, INotificationService notificationService, IReceiptService receiptService, IGeneratePdf generatePdf)
        {
            _orderClientSerivce = orderClientSerivce;
            _notificationService = notificationService;
            _receiptService = receiptService;
            _generatePdf = generatePdf;
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
        [HttpGet("DownloadReceipt/{id:int}")]
        public async Task<IActionResult> DownloadReceipt(int id)
        {
            var txt = await _orderClientSerivce.GetReceipt(id);
            _generatePdf.SetConvertOptions(new ConvertOptions()
            {
                PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,

                PageMargins = new Wkhtmltopdf.NetCore.Options.Margins() { Bottom = 10, Left = 10, Right = 10, Top = 10 },

            });
            var pdfBytes = _generatePdf.GetPDF(txt);
            string fileName = "وصل.pdf";
            return File(pdfBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
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
        public async Task<IActionResult> NonSendOrder([FromQuery] PagingDto pagingDto)
        {
            return Ok(await _orderClientSerivce.NonSendOrder(pagingDto));
        }
        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromBody] IdCollection ids )
        {
            await _orderClientSerivce.Send(ids.Ids);
            return Ok();
        }
        [HttpPost("Sned")]
        public async Task<IActionResult> Sned([FromBody] int[] ids)
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
        [HttpGet("GetNumberOrdersStatus")]
        public async Task<IActionResult> GetNumberOrdersStatus([FromQuery] string number)
        {
            return Ok(Task.FromResult(new List<NumberOrderStatusCount>()
            {
                new NumberOrderStatusCount()
                {
                    Number=10,
                    Title= "مرتجع"
                },
                 new NumberOrderStatusCount()
                {
                    Number=5,
                    Title= "تم التسليم"
                },
                  new NumberOrderStatusCount()
                {
                    Number=7,
                    Title= "مرفوض"
                }
            }));
        }
    }
}