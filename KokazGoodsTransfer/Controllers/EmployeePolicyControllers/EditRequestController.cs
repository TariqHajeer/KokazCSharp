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
        public EditRequestController(KokazContext context, IMapper mapper, Logging logging) : base(context, mapper, logging)
        {
        }
        [HttpGet("NewEditReuqet")]
        public async Task<IActionResult> NewEditRequest()
        {
            var newEditRquests = await this.Context.EditRequests.Where(c => c.Accept == null)
                .Include(c => c.Client)
                .ToListAsync();
            return Ok(mapper.Map<EditRequestDto[]>(newEditRquests));
        }
        [HttpPut("DisAccpet")]
        public async Task<IActionResult> DisAccpet([FromBody] int id)
        {
            var editRequest = await this.Context.EditRequests.FindAsync(id);
            editRequest.Accept = false;
            editRequest.UserId = AuthoticateUserId();
            await this.Context.SaveChangesAsync();
            return Ok();
        }
        [HttpPut("Accept")]
        public async Task<IActionResult> Accept([FromBody] int id)
        {
            var editRequest = await this.Context.EditRequests.FindAsync(id);
            editRequest.Accept = true;
            editRequest.UserId = AuthoticateUserId();
            var client = this.Context.Clients.Find(editRequest.ClientId);
            client.Name = editRequest.NewName;
            client.UserName = editRequest.NewUserName;
            this.Context.Update(client);
            this.Context.Update(editRequest);
            await this.Context.SaveChangesAsync();
            return Ok();
        }
    }
}