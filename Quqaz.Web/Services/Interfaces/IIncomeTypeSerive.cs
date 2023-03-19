using Quqaz.Web.Dtos.IncomeTypes;
using Quqaz.Web.Models;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IIncomeTypeSerive : IIndexCURDService<IncomeType, IncomeTypeDto, CreateIncomeTypeDto, UpdateIncomeTypeDto>
    {
    }
}
