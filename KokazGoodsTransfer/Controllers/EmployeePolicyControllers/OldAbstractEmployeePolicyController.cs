﻿using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Employee")]
    public class OldAbstractEmployeePolicyController : OldAbstractController
    {

        public OldAbstractEmployeePolicyController(KokazContext context, IMapper mapper) : base(context, mapper)
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