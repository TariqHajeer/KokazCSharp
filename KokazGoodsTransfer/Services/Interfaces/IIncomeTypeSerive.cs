using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IIncomeTypeSerive : IIndexService<IncomeType, IncomeTypeDto, CreateIncomeTypeDto, UpdateIncomeTypeDto>
    {
    }
}
