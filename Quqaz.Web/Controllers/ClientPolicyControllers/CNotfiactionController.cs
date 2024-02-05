using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Services.Interfaces;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CNotfiactionController : AbstractClientPolicyController
    {
        private readonly IClientCashedService _clientCashedService;
        private readonly INotificationService _notificationService;
        public CNotfiactionController(IClientCashedService clientCashedService, INotificationService notificationService)
        {
            _clientCashedService = clientCashedService;
            _notificationService = notificationService;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PagingDto pagingDto)
        {
            return Ok(await _notificationService.GetNotifications(pagingDto));
        }

        [HttpPost("SetFirebaseToken")]
        public async Task<IActionResult> SetFirebaseToken([FromBody] SetFCMTokenDto setFCMToken)
        {
            await _clientCashedService.SetToken(setFCMToken);
            return Ok();
        }
        [HttpGet("NewNotfiactionCount")]
        public async Task<IActionResult> NewNotfiaction()
        {
            return Ok(await _notificationService.NewNotfiactionCount());
        }
        [HttpPut("SeeNotifactions")]
        public async Task<IActionResult> SeeNotifactions([FromBody] IdCollection ids)
        {
            await _notificationService.SeeNotifactions(ids.Ids);
            return Ok();
        }


    }
}