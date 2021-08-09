using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.ReceiptDtos;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : AbstractEmployeePolicyController
    {
        public ReceiptController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }

        [HttpGet]
        public IActionResult Get([FromQuery] PagingDto pagingDto, [FromQuery]AccountFilterDto accountFilterDto)
        {
            var repiq = this.Context.Receipts.AsQueryable();
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
                .ToList();
            var data = mapper.Map<ReceiptDto[]>(replist);
            return Ok(new { data, total = totalRreq });
        }
        [HttpGet("UnPaidRecipt/{clientId}")]
        public IActionResult UnPaidRecipt(int clientId)
        {
            var repiq = this.Context.Receipts
                .Include(c => c.Print)
                .Where(c => c.ClientId == clientId && c.PrintId == null).ToList();
            return Ok(mapper.Map<ReceiptDto[]>(repiq));
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var recipe = this.Context.Receipts.Find(id);
            if (recipe.PrintId != null)
            {
                return Conflict();
            }
            this.Context.Receipts.Remove(recipe);
            this.Context.SaveChanges();
            return Ok();
        }
    }
}