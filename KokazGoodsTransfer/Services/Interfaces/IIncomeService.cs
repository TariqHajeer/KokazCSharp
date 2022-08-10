using KokazGoodsTransfer.Dtos.IncomesDtos;
using KokazGoodsTransfer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IIncomeService : ICRUDService<Income, IncomeDto, CreateIncomeDto, UpdateIncomeDto>
    {
    }
}
