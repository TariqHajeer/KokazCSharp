using AutoMapper;
using KokazGoodsTransfer.CustomException;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
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
                throw new ConfilectException("");

            }
            PaymentWay paymentWay = new PaymentWay();
            paymentWay.Name = createDto.Name;
            await _repository.AddAsync(paymentWay);
            return new ErrorRepsonse<NameAndIdDto>(_mapper.Map<NameAndIdDto>(paymentWay));
        }

        public override Task<ErrorRepsonse<NameAndIdDto>> Update(NameAndIdDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
