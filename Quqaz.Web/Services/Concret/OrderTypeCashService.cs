using AutoMapper;
using Quqaz.Web.CustomException;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.OrdersTypes;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
{
    public class OrderTypeCashService : CashService<OrderType, OrderTypeDto, CreateOrderType, UpdateOrderTypeDto>, IOrderTypeCashService
    {
        public OrderTypeCashService(IRepository<OrderType> repository, IMapper mapper, IMemoryCache cache, Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, cache, logging, httpContextAccessorService)
        {
        }
        public override async Task<ErrorRepsonse<OrderTypeDto>> AddAsync(CreateOrderType createDto)
        {
            if (await _repository.Any(c => c.Name.Equals(createDto.Name)))
            {
                throw new ConflictException("");
            }
            return await base.AddAsync(createDto);
        }
        public override async Task<ErrorRepsonse<OrderTypeDto>> Update(UpdateOrderTypeDto updateDto)
        {
            if (await _repository.Any(c => c.Id != updateDto.Id && c.Name.Equals(updateDto.Name)))
            {
                throw new ConflictException("");
            }
            return await base.Update(updateDto);
        }

    }
}
