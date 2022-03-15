using KokazGoodsTransfer.Dtos.IncomeTypes;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IIncomeTypeSerive:ICRUDService<IncomeType,IncomeTypeDto,CreateIncomeTypeDto,UpdateIncomeTypeDto>
    {
    }
}
