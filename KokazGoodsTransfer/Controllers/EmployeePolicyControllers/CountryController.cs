using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : AbstractEmployeePolicyController
    {

        private readonly ICountryCashedService _countryCashedService;
        private readonly IRegionCashedService _regionCashedService;
        private readonly IUserCashedService _userCashedService;
        private readonly IClientCashedService _clientCashedService;
        private readonly IRepository<MediatorCountry> _mediatorCountry;
        private readonly IHttpContextAccessorService _httpContextAccessorService;
        private readonly IRepository<Branch> _branchRepository;
        private readonly int _currentBranchId;
        public CountryController(ICountryCashedService countryCashedService, IRegionCashedService regionCashedService, IUserCashedService userCashedService, IClientCashedService clientCashedService, IRepository<MediatorCountry> mediatorCountry, IHttpContextAccessorService httpContextAccessorService, IRepository<Branch> branchRepository)
        {
            _countryCashedService = countryCashedService;
            _regionCashedService = regionCashedService;
            _userCashedService = userCashedService;
            _clientCashedService = clientCashedService;
            _mediatorCountry = mediatorCountry;
            _httpContextAccessorService = httpContextAccessorService;
            _currentBranchId = _httpContextAccessorService.CurrentBranchId();
            _branchRepository = branchRepository;
        }
        private void RemoveRelatedCash()
        {
            _regionCashedService.RemoveCash();
            _userCashedService.RemoveCash();
            _clientCashedService.RemoveCash();
        }
        [HttpGet("RequiredAgent")]
        public async Task<ActionResult<bool>> RequiredAgent([FromQuery] int countryId)
        {
            var currentBranch = await _branchRepository.GetById(_currentBranchId); 
            return Ok(!(await _mediatorCountry.Any(c => c.FromCountryId == currentBranch.CountryId && c.ToCountryId == countryId)));

        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetAll() => Ok(await _countryCashedService.GetCashed());



        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCountryDto createCountryDto)
        {

            var result = await _countryCashedService.AddAsync(createCountryDto);
            if (result.Errors.Any())
                return Conflict();
            RemoveRelatedCash();
            return Ok(result.Data);

        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCountryDto updateCountryDto)
        {
            var result = await _countryCashedService.Update(updateCountryDto);
            if (result.Errors.Any())
                return Conflict();
            RemoveRelatedCash();
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _countryCashedService.Delete(id);
            if (result.Errors.Any())
                return Conflict();
            RemoveRelatedCash();
            return Ok();

        }
        [HttpPut("SetMain/{id}")]
        public async Task<IActionResult> SetIsMain(int id)
        {

            await _countryCashedService.SetMainCountry(id);
            RemoveRelatedCash();
            return Ok();

        }
    }
}