using AutoMapper;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using Quqaz.Web.Dtos.BranchDtos;
using Quqaz.Web.Services.Interfaces;
using System.Linq;
using Quqaz.Web.Dtos.Common;

namespace Quqaz.Web.Controllers.EmployeePolicyControllers
{
    public class BranchesController : AbstractEmployeePolicyController
    {
        private readonly IBranchService _branchService;
        public BranchesController(IBranchService branchService)
        {
            _branchService = branchService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchDto>>> GetAll()
        {
            return Ok(await _branchService.GetAll(c => c.Country));
        }
        [HttpGet("Lite")]
        public async Task<ActionResult<IEnumerable<NameAndIdDto>>> GetLite()
        {
            return Ok(await _branchService.GetLite());
        }
    }
}
