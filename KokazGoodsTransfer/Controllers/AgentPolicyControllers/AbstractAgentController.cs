using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.AgentPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Agent")]
    public class AbstractAgentController : AbstractController
    {
    }
}
