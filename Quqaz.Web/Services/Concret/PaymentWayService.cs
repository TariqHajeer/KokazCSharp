using AutoMapper;
using Quqaz.Web.CustomException;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
{
    public class PaymentWayService : CRUDService<PaymentWay, NameAndIdDto, NameAndIdDto, NameAndIdDto>, IPaymentWayService
    {
        public PaymentWayService(IRepository<PaymentWay> repository, IMapper mapper, Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, logging, httpContextAccessorService)
        {
        }
        public override async Task<ErrorRepsonse<NameAndIdDto>> AddAsync(NameAndIdDto createDto)
        {
            if (await _repository.Any(c => c.Name == createDto.Name))
            {
                throw new ConflictException("");

            }
            PaymentWay paymentWay = new PaymentWay
            {
                Name = createDto.Name
            };
            await _repository.AddAsync(paymentWay);
            return new ErrorRepsonse<NameAndIdDto>(_mapper.Map<NameAndIdDto>(paymentWay));
        }

        public override Task<ErrorRepsonse<NameAndIdDto>> Update(NameAndIdDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
