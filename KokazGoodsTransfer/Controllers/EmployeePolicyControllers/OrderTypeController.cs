using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersTypes;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderTypeController : AbstractEmployeePolicyController
    {
        private readonly IOrderTypeCashService _orderTypeService;
        public OrderTypeController(IOrderTypeCashService orderTypeService)
        {
            _orderTypeService = orderTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _orderTypeService.GetAll());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderType orderTypeDto)
        {
            var dto = await _orderTypeService.AddAsync(orderTypeDto);
            return Ok(dto.Data);
        }
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateOrderTypeDto updateOrderTypeDto)
        {
            await _orderTypeService.Update(updateOrderTypeDto);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _orderTypeService.Delete(id);
            return Ok();
        }

    }
}