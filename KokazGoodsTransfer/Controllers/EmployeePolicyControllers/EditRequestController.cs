using System.Threading.Tasks;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditRequestController : AbstractEmployeePolicyController
    {
        private readonly IEditRequestService _editRequestService;
        public EditRequestController(IEditRequestService editRequestService)
        {
            _editRequestService = editRequestService;
        }
        [HttpGet("NewEditReuqet")]
        public async Task<IActionResult> NewEditRequest()
        {
            return Ok(await _editRequestService.NewEditRequest());
        }
        [HttpPut("DisAccpet")]
        public async Task<IActionResult> DisAccpet([FromBody] int id)
        {
            await _editRequestService.DisAccpet(id);
            return Ok();

        }
        [HttpPut("Accept")]
        public async Task<IActionResult> Accept([FromBody] int id)
        {

            await _editRequestService.Accept(id);
            return Ok();
        }
    }
}