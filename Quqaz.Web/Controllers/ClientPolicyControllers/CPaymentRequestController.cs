using System.Threading.Tasks;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.PayemntRequestDtos;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CPaymentRequestController : AbstractClientPolicyController
    {
        private readonly IPaymentRequestSerivce _paymentRequestSerivce;
        private readonly IPaymentWayService _paymentWayService;
        public CPaymentRequestController(IPaymentRequestSerivce paymentRequestSerivce, IPaymentWayService paymentWayService)
        {
            _paymentRequestSerivce = paymentRequestSerivce;
            _paymentWayService = paymentWayService;
        }
        [HttpGet("CanRequest")]
        public async Task<ActionResult<bool>> CanRequest()
        {
            return Ok(!await _paymentRequestSerivce.CanClientRequest(AuthoticateUserId()));
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePaymentRequestDto createPaymentRequestDto)
        {
            return Ok(await _paymentRequestSerivce.Create(createPaymentRequestDto));
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto)
        {
            var pagingResult = await _paymentRequestSerivce.GetByClient(pagingDto, AuthoticateUserId());
            return Ok(new { total = pagingResult.Total, dto = pagingResult.Data });
        }
        [HttpGet("GetPaymentWay")]
        public async Task<IActionResult> GetPaymentWay()
        {
            return Ok(await _paymentWayService.GetAll());
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _paymentRequestSerivce.Delete(id);
            return Ok();
        }

    }
}