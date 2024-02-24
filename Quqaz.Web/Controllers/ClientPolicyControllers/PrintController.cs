using System.Linq;
using System.Threading.Tasks;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Wkhtmltopdf.NetCore;
using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintController : AbstractClientPolicyController
    {
        private readonly IClientPaymentService _clientPaymentService;
        private readonly IGeneratePdf _generatePdf;
        public PrintController(IClientPaymentService clientPaymentService, IGeneratePdf generatePdf)
        {
            _clientPaymentService = clientPaymentService;
            _generatePdf = generatePdf;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] int? number, string code)
        {
            return Ok(await _clientPaymentService.GetClientprints(pagingDto, number, code));
        }

        [HttpGet("{printNumber}")]
        public async Task<IActionResult> Get(int printNumber)
        {
            var authId = AuthoticateUserId();
            return Ok(await _clientPaymentService.GetFirst(c => c.Id == printNumber && c.OrderClientPaymnets.All(c => c.Order.ClientId == authId)));
        }

        [HttpGet("Orders/{printId:int}")]
        public async Task<IActionResult> GetOrders(int printId, [FromQuery] PagingDto pagingDto)
        {
            return Ok(await _clientPaymentService.GetOrdersByPrintNumberId(printId, pagingDto));
        }
        [HttpGet("DownloadReceipt")]
        public async Task<IActionResult> DownloadReceipt([FromQuery] int printId)
        {
            var txt = await _clientPaymentService.GetReceiptAsHtml(printId);
            _generatePdf.SetConvertOptions(new ConvertOptions()
            {
                PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,

                PageMargins = new Wkhtmltopdf.NetCore.Options.Margins() { Bottom = 10, Left = 10, Right = 10, Top = 10 },

            });
            var pdfBytes = _generatePdf.GetPDF(txt);
            string fileName = "وصل.pdf";
            return File(pdfBytes, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
        }
    }
}