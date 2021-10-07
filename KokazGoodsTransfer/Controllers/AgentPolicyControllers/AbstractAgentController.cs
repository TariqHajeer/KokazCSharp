using AutoMapper;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Controllers.AgentPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Agent")]
    public class AbstractAgentController : AbstractController
    {
        public AbstractAgentController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}
