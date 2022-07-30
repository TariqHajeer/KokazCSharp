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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : OldAbstractEmployeePolicyController
    {
        public ReceiptController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult Get([FromQuery] PagingDto pagingDto, [FromQuery] AccountFilterDto accountFilterDto)
        {
            var repiq = this._context.Receipts.AsQueryable();
            if (accountFilterDto.ClientId != null)
            {
                repiq = repiq.Where(c => c.ClientId == accountFilterDto.ClientId);
            }
            if (accountFilterDto.IsPay != null)
            {
                repiq = repiq.Where(c => c.IsPay == (bool)accountFilterDto.IsPay);
            }
            if (accountFilterDto.Date != null)
            {
                repiq = repiq.Where(c => c.Date == accountFilterDto.Date);
            }
            var totalRreq = repiq.Count();
            var replist = repiq.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount)
                .Include(c => c.Client)
                .Include(c => c.ClientPayment)
                .ToList();
            var data = _mapper.Map<ReceiptDto[]>(replist);
            return Ok(new { data, total = totalRreq });
        }
        [HttpGet("UnPaidRecipt/{clientId}")]
        public IActionResult UnPaidRecipt(int clientId)
        {
            var repiq = this._context.Receipts
                .Include(c => c.ClientPayment)
                .Where(c => c.ClientId == clientId && c.ClientPaymentId == null).ToList();
            return Ok(_mapper.Map<ReceiptDto[]>(repiq));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptDto>> GetById(int id)
        {
            var receipt = await _context.Receipts.FindAsync(id);
            _context.Entry(receipt).Reference(c => c.Client).Load();
            return Ok(_mapper.Map<ReceiptDto>(receipt));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var recipe = this._context.Receipts.Find(id);
            if (recipe.ClientPaymentId != null)
            {
                return Conflict();
            }
            this._context.Receipts.Remove(recipe);
            this._context.SaveChanges();
            return Ok();
        }
    }
}