using AutoMapper;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Client")]
    public class AbstractClientPolicyController : AbstractController
    {
        public AbstractClientPolicyController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}