using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Employee")]
    public class AbstractEmployeePolicyController : AbstractController
    {

    }
}
