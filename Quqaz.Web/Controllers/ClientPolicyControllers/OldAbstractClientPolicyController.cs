using AutoMapper;
using Quqaz.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Quqaz.Web.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Client")]
    public class OldAbstractClientPolicyController : OldAbstractController
    {
        public OldAbstractClientPolicyController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}