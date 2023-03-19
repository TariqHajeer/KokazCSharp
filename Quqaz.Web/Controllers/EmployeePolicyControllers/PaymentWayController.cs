using System.Threading.Tasks;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentWayController : AbstractEmployeePolicyController
    {
        private readonly IPaymentWayService _paymentWayService;
        public PaymentWayController(IPaymentWayService paymentWayService)
        {
            _paymentWayService = paymentWayService;
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] NameAndIdDto nameAndIdDto)
        {
            return Ok((await _paymentWayService.AddAsync(nameAndIdDto)).Data);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _paymentWayService.GetAll());
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _paymentWayService.Delete(id);
            return Ok();
        }
    }
}