using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Services.Interfaces.Additional;
using System.Threading.Tasks;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientMessageController : ControllerBase
    {
        private readonly IClientMessageService _clientMessageService;
        public ClientMessageController(IClientMessageService clientMessageService)
        {
            _clientMessageService = clientMessageService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto, [FromQuery] bool? isPublished)
        {
            if (isPublished.HasValue)
            {
                return Ok(await _clientMessageService.GetAsync(paging: pagingDto, filter: c => c.IsPublished == isPublished));
            }
            return Ok(await _clientMessageService.GetAsync(paging: pagingDto));
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _clientMessageService.Delete(id));
        }
        [HttpPut("Publish")]
        public async Task<IActionResult> Publish([FromBody] int id)
        {
            await _clientMessageService.Publish(id);
            return Ok();
        }
        [HttpPut("UnPublish")]
        public async Task<IActionResult> UnPublish([FromBody] int id)
        {
            await _clientMessageService.UnPublish(id);
            return Ok();
        }
    }
}

