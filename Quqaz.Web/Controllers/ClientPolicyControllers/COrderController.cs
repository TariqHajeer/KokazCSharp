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
        private readonly IReceiptService _receiptService;
        private readonly IGeneratePdf _generatePdf;
        public COrderController(IOrderClientSerivce orderClientSerivce,  IReceiptService receiptService, IGeneratePdf generatePdf)
        {
            _orderClientSerivce = orderClientSerivce;
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
        public async Task<IActionResult> Send([FromBody] IdCollection ids)
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
        [HttpPost("OrdersDontFinished")]
        public async Task<IActionResult> OrdersDontFinished([FromQuery] PagingDto paging, [FromBody] OrderDontFinishFilter orderDontFinishFilter)
        {
            return Ok(await _orderClientSerivce.OrdersDontFinished(orderDontFinishFilter, paging));
        }
        [HttpPost("DownloadOrdersDontFinished")]
        public async Task<IActionResult> DownloadOrdersDontFinished([FromBody] OrderDontFinishFilter orderDontFinishFilter)
        {
            var txt = await _orderClientSerivce.GetReceipt(10);
            _generatePdf.SetConvertOptions(new ConvertOptions()
            {
                PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,

                PageMargins = new Wkhtmltopdf.NetCore.Options.Margins() { Bottom = 10, Left = 10, Right = 10, Top = 10 },

            });
            var pdfBytes = _generatePdf.GetPDF(txt);
            string fileName = "وصل.pdf";
            return File(pdfBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        }
        [HttpGet("UnPaidRecipt")]
        public async Task<IActionResult> UnPaidRecipt()
        {
            return Ok(await _receiptService.UnPaidRecipt(AuthoticateUserId()));
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
        [HttpGet("Track")]
        public async Task<IActionResult> Track([FromQuery] int Id)
        {
            if (Id == 1)
                return NotFound();
            List<ClientTracShipmentDto> tracking = new List<ClientTracShipmentDto>() {
            new ClientTracShipmentDto()
            {
                Number = 1,
                Text = "في المخزن",
                ExtraText = "الأربعاء ",
                Checked = true
            }, new ClientTracShipmentDto()
            {
                Number = 2,
                Text = "متوجعة إلى بغداد",
                ExtraText = "الأربعاء ",
                Checked = false
            }, new ClientTracShipmentDto()
            {
                Number = 3,
                Text = "وصلت إلى محافظة",
                ExtraText = "الأربعاء ",
                Checked = false
            }, new ClientTracShipmentDto()
            {
                Number = 4,
                Text = "خرجت مع مندوب",
                ExtraText = "00000000000 ",
                Checked = false
            },
             new ClientTracShipmentDto()
            {
                Number = 5,
                Text = "في إننزظار التسليم",
                ExtraText = "الأربعاء ",
                Checked = false
                }
            };
            return Ok(tracking);
        }
    }
}