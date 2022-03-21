using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IIncomeTypeSerive : IIndexCURDService<IncomeType, IncomeTypeDto, CreateIncomeTypeDto, UpdateIncomeTypeDto>
    {
    }
}
