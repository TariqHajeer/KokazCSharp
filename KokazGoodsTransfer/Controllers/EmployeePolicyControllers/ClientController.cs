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

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : AbstractEmployeePolicyController
    {
        private readonly IClientCahedRepository _clientCahedRepository;
        public ClientController(KokazContext context, IMapper mapper, Logging logging, IClientCahedRepository clientCahedRepository) : base(context, mapper, logging)
        {
            _clientCahedRepository = clientCahedRepository;
        }
        [HttpPost]
        [Authorize(Roles = "AddClient")]
        public async Task<IActionResult> CreateClient(CreateClientDto createClientDto)
        {
            try
            {
                var isExist = await _context.Clients.AnyAsync(c => c.UserName.ToLower() == createClientDto.UserName.ToLower() || c.Name.ToLower() == createClientDto.Name.ToLower());
                if (isExist)
                {
                    return Conflict();
                }
                var client = _mapper.Map<Client>(createClientDto);
                client.Points = 0;
                client.UserId = (int)AuthoticateUserId();

                foreach (var item in createClientDto.Phones)
                {
                    client.ClientPhones.Add(new ClientPhone()
                    {
                        ClientId = client.Id,
                        Phone = item
                    });
                }
                this._context.Set<Client>().Add(client);
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clients = await _clientCahedRepository.GetAll();
            return Ok(_mapper.Map<ClientDto[]>(clients));
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var client = this._context.Clients.Include(c => c.Country)
                .Include(c => c.User)
                .Include(c => c.ClientPhones)
                .Include(c => c.Orders)
                .Where(c => c.Id == id).FirstOrDefault();
            return Ok(_mapper.Map<ClientDto>(client));
        }
        [HttpPut("addPhone")]
        public async Task<IActionResult> AddPhone([FromBody] AddPhoneDto addPhoneDto)
        {
            try
            {
                var client = await this._context.Clients.FindAsync(addPhoneDto.objectId);
                if (client == null)
                    return NotFound();
                await this._context.Entry(client).Collection(c => c.ClientPhones).LoadAsync();
                if (client.ClientPhones.Select(c => c.Phone).Contains(addPhoneDto.Phone))
                {
                    return Conflict();
                }
                var clientPhone = new ClientPhone()
                {
                    ClientId = client.Id,
                    Phone = addPhoneDto.Phone
                };
                await this._context.AddAsync(clientPhone);
                await this._context.SaveChangesAsync();
                await _clientCahedRepository.RefreshCash();
                return Ok(_mapper.Map<PhoneDto>(clientPhone));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        [HttpPut("deletePhone/{id}")]
        public async Task<IActionResult> DeletePhone(int id)
        {
            try
            {
                var clientPhone = await this._context.ClientPhones.FindAsync(id);
                this._context.Remove(clientPhone);
                await this._context.SaveChangesAsync();
                await _clientCahedRepository.RefreshCash();
                return Ok();
            }
            catch (Exception ex)
            {
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
            var client = await this._context.Clients
                .FindAsync(id);
            if (client == null)
                return NotFound();
            await this._context.Entry(client).Collection(c => c.Orders).LoadAsync();
            if (client.Orders.Count() != 0)
                return Conflict();
            this._context.Remove(client);
            await this._context.SaveChangesAsync();
            await this._clientCahedRepository.RefreshCash();
            return Ok();
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