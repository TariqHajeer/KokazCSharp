using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class ClientPaymentService : IClientPaymentService
    {
        private readonly IRepository<ClientPayment> _clientPaymentRepository;
        private readonly IMapper _mapper;
        public ClientPaymentService(IRepository<ClientPayment> repository, IMapper mapper)
        {
            _clientPaymentRepository = repository;
            _mapper = mapper;
        }
        public async Task<PrintOrdersDto> GetFirst(Expression<Func<ClientPayment, bool>> filter, string[] includes)
        {
            var payment = await _clientPaymentRepository.FirstOrDefualt(filter, includes);
            return _mapper.Map<PrintOrdersDto>(payment);
        }
    }
}
