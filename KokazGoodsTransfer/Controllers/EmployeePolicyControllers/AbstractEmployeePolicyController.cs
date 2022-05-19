using System;
using System.Linq;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
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

        public AbstractEmployeePolicyController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper, logging)
        {
        }
        public ActionResult<GenaricErrorResponse<T1, T2, T3>> GetResult<T1, T2, T3>(GenaricErrorResponse<T1, T2, T3> response)
        {
            if (response.Success)
                return Ok(response);
            if (response.Conflict)
                return Conflict(response);

            return BadRequest(response);
        }

    }
}