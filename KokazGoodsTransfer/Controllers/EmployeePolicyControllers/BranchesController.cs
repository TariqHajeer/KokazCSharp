using AutoMapper;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.BranchDtos;
using KokazGoodsTransfer.Services.Interfaces;
using System.Linq;
using KokazGoodsTransfer.Dtos.Common;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
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

        //[HttpPost]
        //public async Task<ActionResult<BranchDto>> Create(CreateBranchDto branchDto)
        //{
        //    var response = await _branchService.AddAsync(branchDto);
        //    if (response.Errors.Any())
        //        return BadRequest(response.Errors);
        //    return Ok(response.Data);
        //}
        //[HttpDelete]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    return Ok(await _branchService.Delete(id));
        //}
    }
}
