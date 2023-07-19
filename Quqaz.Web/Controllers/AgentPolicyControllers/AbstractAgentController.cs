using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.AgentPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Agent")]
    public class AbstractAgentController : AbstractController
    {
    }
}
