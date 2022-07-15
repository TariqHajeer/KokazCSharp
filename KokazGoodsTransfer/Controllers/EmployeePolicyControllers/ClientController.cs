using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.ReceiptDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Services.Interfaces;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : AbstractEmployeePolicyController
    {
        private readonly IClientCashedService _clientCashedService;
        public ClientController(KokazContext context, IMapper mapper, IClientCashedService clientCashedService) : base(context, mapper)
        {
            _clientCashedService = clientCashedService;
        }
        [HttpPost]
        [Authorize(Roles = "AddClient")]
        public async Task<IActionResult> CreateClient(CreateClientDto createClientDto)
        {

            createClientDto.UserId = (int)AuthoticateUserId();
            var result = await _clientCashedService.AddAsync(createClientDto);
            if (result.Errors.Any())
                return Conflict();
            return Ok(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _clientCashedService.GetCashed());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _clientCashedService.GetById(id));
        [HttpPut("addPhone")]
        public async Task<IActionResult> AddPhone([FromBody] AddPhoneDto addPhoneDto)
        {
            var result = await _clientCashedService.AddPhone(addPhoneDto);
            if (result.Errors.Any())
                return Conflict();
            return Ok(result.Data);

        }
        [HttpPut("deletePhone/{id}")]
        public async Task<IActionResult> DeletePhone(int id)
        {
            await _clientCashedService.DeletePhone(id);
            return Ok();

        }
        [HttpPatch]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientDto updateClientDto)
        {
            var result = await _clientCashedService.Update(updateClientDto);
            if (result.Errors.Any())
                return Conflict();
            return Ok(result.Data);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var result = await _clientCashedService.Delete(id);
            if (result.Errors.Any())
                return Conflict();
            return Ok();

        }
        [HttpPost("Account")]
        public async Task<IActionResult> Account([FromBody] AccountDto accountDto)
        {
            var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                Receipt receipt = new Receipt()
                {
                    IsPay = accountDto.IsPay,
                    About = accountDto.About,
                    CreatedBy = AuthoticateUserName(),
                    ClientId = accountDto.ClinetId,
                    Date = DateTime.Now,
                    Amount = accountDto.Amount,
                    Manager = accountDto.Manager,
                    Note = accountDto.Note,
                };

                await this._context.AddAsync(receipt);
                await this._context.SaveChangesAsync();
                var treasuer = await _context.Treasuries.FindAsync(AuthoticateUserId());
                treasuer.Total += accountDto.Amount;
                _context.Update(treasuer);
                var history = new TreasuryHistory()
                {
                    Amount = accountDto.Amount,
                    ReceiptId = receipt.Id,
                    TreasuryId = treasuer.Id,
                    CreatedOnUtc = DateTime.Now,
                };
                await _context.AddAsync(history);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(receipt.Id);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw ex;
            }
        }
        [HttpPost("GiveOrDiscountPoints")]
        public async Task<IActionResult> GivePoint([FromBody] GiveOrDiscountPointsDto giveOrDiscountPointsDto)
        {
            await _clientCashedService.GivePoints(giveOrDiscountPointsDto);
            return Ok();
        }
    }
}