using Quqaz.Web.Dtos.OutComeTypeDtos;
using Quqaz.Web.Models;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IOutcomeTypeService : IIndexCURDService<OutComeType, OutComeTypeDto, CreateOutComeTypeDto, UpdateOutComeTypeDto>
    {
    }
}
