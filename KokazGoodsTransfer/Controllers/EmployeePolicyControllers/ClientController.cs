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
        private readonly IClientCahedRepository _clientCahedRepository;
        private readonly IClientCashedService _clientCashedService;
        public ClientController(KokazContext context, IMapper mapper, Logging logging, IClientCahedRepository clientCahedRepository, IClientCashedService clientCashedService) : base(context, mapper, logging)
        {
            _clientCahedRepository = clientCahedRepository;
            _clientCashedService = clientCashedService;
        }
        [HttpPost]
        [Authorize(Roles = "AddClient")]
        public async Task<IActionResult> CreateClient(CreateClientDto createClientDto)
        {
            try
            {
                createClientDto.UserId = (int)AuthoticateUserId();
                var result = await _clientCashedService.AddAsync(createClientDto);
                if (result.Errors.Any())
                    return Conflict();
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _clientCashedService.GetCashed());
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) => Ok(await _clientCahedRepository.GetById(id));
        [HttpPut("addPhone")]
        public async Task<IActionResult> AddPhone([FromBody] AddPhoneDto addPhoneDto)
        {
            try
            {
                var result = await _clientCashedService.AddPhone(addPhoneDto);
                if (result.Errors.Any())
                    return Conflict();
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpPut("deletePhone/{id}")]
        public async Task<IActionResult> DeletePhone(int id)
        {
            try
            {
                await _clientCashedService.DeletePhone(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpPatch]
        public async Task<IActionResult> UpdateClient([FromBody] UpdateClientDto updateClientDto)
        {
            try
            {
                var client = await this._context.Clients.Where(c => c.Id == updateClientDto.Id).FirstOrDefaultAsync();

                if (client == null)
                    return NotFound();

                var oldPassord = client.Password;

                _mapper.Map(updateClientDto, client);
                if (updateClientDto.Password == null)
                    client.Password = oldPassord;

                await this._context.SaveChangesAsync();
                client = await this._context.Clients
                .Include(c => c.Country)
                .Include(c => c.User)
                .SingleAsync(c => c.Id == client.Id);
                await _clientCahedRepository.RefreshCash();
                return Ok(_mapper.Map<ClientDto>(client));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                var result = await _clientCashedService.Delete(id);
                if (result.Errors.Any())
                    return Conflict();
                return Ok();
            }
            catch (Exception ex)
            {
                _logging.WriteExption(ex);
                return BadRequest();
            }
        }
        [HttpPost("Account")]
        public async Task<IActionResult> Account([FromBody] AccountDto accountDto)
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
            return Ok(receipt.Id);
        }
        [HttpPost("GiveOrDiscountPoints")]
        public async Task<IActionResult> GivePoint([FromBody] GiveOrDiscountPointsDto giveOrDiscountPointsDto)
        {
            var client = await this._context.Clients.FindAsync(giveOrDiscountPointsDto.ClientId);
            string sen = "";
            if (giveOrDiscountPointsDto.IsGive)
            {
                client.Points += giveOrDiscountPointsDto.Points;
                sen += $"تم إهدائك {giveOrDiscountPointsDto.Points} نقاط";
            }
            else
            {
                client.Points -= giveOrDiscountPointsDto.Points;
                sen += $"تم خصم {giveOrDiscountPointsDto.Points} نقاط منك";
            }
            Notfication notfication = new Notfication()
            {
                ClientId = client.Id,
                Note = sen,
            };
            await this._context.AddAsync(notfication);
            await this._context.SaveChangesAsync();
            return Ok();
        }
    }
}