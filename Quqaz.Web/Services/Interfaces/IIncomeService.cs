using Quqaz.Web.Dtos.IncomesDtos;
using Quqaz.Web.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IIncomeService : ICRUDService<Income, IncomeDto, CreateIncomeDto, UpdateIncomeDto>
    {
    }
}
