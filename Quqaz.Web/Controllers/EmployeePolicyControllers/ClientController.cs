using System.Linq;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Services.Interfaces;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : AbstractEmployeePolicyController
    {
        private readonly IClientCashedService _clientCashedService;
        public ClientController(IClientCashedService clientCashedService)
        {
            _clientCashedService = clientCashedService;
        }
        [HttpPost]
        [Authorize(Roles = "AddClient")]
        public async Task<IActionResult> CreateClient(CreateClientDto createClientDto)
        {

            createClientDto.UserId = (int)AuthoticateUserId();
            var result = await _clientCashedService.AddAsync(createClientDto);
            if (result.Errors.Any())
                return Conflict();
            return Ok(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _clientCashedService.GetCashed());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _clientCashedService.GetById(id));
        [HttpPut("addPhone")]
        public async Task<IActionResult> AddPhone([FromBody] AddPhoneDto addPhoneDto)
        {
            var result = await _clientCashedService.AddPhone(addPhoneDto);
            if (result.Errors.Any())
                return Conflict();
            return Ok(result.Data);

        }
        [HttpPut("deletePhone/{id}")]
        public async Task<IActionResult> DeletePhone(int id)
        {
            await _clientCashedService.DeletePhone(id);
            return Ok();

        }
        [HttpPatch]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientDto updateClientDto)
        {
            var result = await _clientCashedService.Update(updateClientDto);
            if (result.Errors.Any())
                return Conflict();
            return Ok(result.Data);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _clientCashedService.Delete(id);
            if (result.Errors.Any())
                return Conflict();
            return Ok();

        }
        [HttpPost("Account")]
        public async Task<IActionResult> Account([FromBody] AccountDto accountDto)
        {
            return Ok(await _clientCashedService.Account(accountDto));
        }
        [HttpPost("GiveOrDiscountPoints")]
        public async Task<IActionResult> GivePoint([FromBody] GiveOrDiscountPointsDto giveOrDiscountPointsDto)
        {
            await _clientCashedService.GivePoints(giveOrDiscountPointsDto);
            return Ok();
        }
    }
}