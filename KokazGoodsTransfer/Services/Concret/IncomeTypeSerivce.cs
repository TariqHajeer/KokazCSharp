using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
namespace KokazGoodsTransfer.Services.Concret
{
    public class IncomeTypeSerivce : CRUDService<IncomeType, IncomeTypeDto, CreateIncomeTypeDto, UpdateIncomeTypeDto>, IIncomeTypeSerive
    {
        public IncomeTypeSerivce(IRepository<IncomeType> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
