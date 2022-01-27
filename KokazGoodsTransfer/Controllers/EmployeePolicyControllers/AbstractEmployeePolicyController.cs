using System;
using System.Linq;
using AutoMapper;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Employee")]
    public class AbstractEmployeePolicyController : AbstractController
    {
        public AbstractEmployeePolicyController(KokazContext context, IMapper mapper,Logging logging) : base(context, mapper,logging)
        {
        }

    }
}