using System.Linq;
using System.Threading.Tasks;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrintController : AbstractClientPolicyController
    {
        private readonly IClientPaymentService _clientPaymentService;
        public PrintController(IClientPaymentService clientPaymentService)
        {
            _clientPaymentService = clientPaymentService;
        }
        [HttpGet("{printNumber}")]
        public async Task<IActionResult> Get(int printNumber)
        {
            var includes = new string[] { "OrderClientPaymnets.Order", "Receipts", "ClientPaymentDetails" };
            return Ok(await _clientPaymentService.GetFirst(c => c.Id == printNumber && c.OrderClientPaymnets.All(c => c.Order.ClientId == AuthoticateUserId()), includes));
        }
    }
}