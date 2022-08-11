using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.EditRequestDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditRequestController : AbstractEmployeePolicyController
    {
        public EditRequestController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        [HttpGet("NewEditReuqet")]
        public async Task<IActionResult> NewEditRequest()
        {
            var newEditRquests = await this._context.EditRequests.Where(c => c.Accept == null)
                .Include(c => c.Client)
                .ToListAsync();
            return Ok(_mapper.Map<EditRequestDto[]>(newEditRquests));
        }
        [HttpPut("DisAccpet")]
        public async Task<IActionResult> DisAccpet([FromBody] int id)
        {
            var editRequest = await this._context.EditRequests.FindAsync(id);
            editRequest.Accept = false;
            editRequest.UserId = AuthoticateUserId();
            await this._context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("Accept")]
        public async Task<IActionResult> Accept([FromBody] int id)
        {
            var editRequest = await this._context.EditRequests.FindAsync(id);
            editRequest.Accept = true;
            editRequest.UserId = AuthoticateUserId();
            var client = this._context.Clients.Find(editRequest.ClientId);
            client.Name = editRequest.NewName;
            client.UserName = editRequest.NewUserName;
            this._context.Update(client);
            this._context.Update(editRequest);
            await this._context.SaveChangesAsync();
            return Ok();
        }
    }
}