using Microsoft.AspNetCore.Mvc;
using Quqaz.Web.Services.Interfaces;
using System.Threading.Tasks;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CBranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        public CBranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetALL()
        {

            return Ok(await _branchService.GetAll());

        }
    }
}
