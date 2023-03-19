using System.Threading.Tasks;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.ReceiptDtos;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : AbstractEmployeePolicyController
    {
        private readonly IReceiptService _receiptService;
        public ReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] AccountFilterDto accountFilterDto)
        {
            return Ok(await _receiptService.Get(pagingDto, accountFilterDto));
        }
        [HttpGet("UnPaidRecipt/{clientId}")]
        public async Task<IActionResult> UnPaidRecipt(int clientId)
        {
            return Ok(await _receiptService.UnPaidRecipt(clientId));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptDto>> GetById(int id)
        {
            return Ok(await _receiptService.GetById(id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _receiptService.Delete(id);
            return Ok();
        }
    }
}