using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IOutcomeTypeService : IIndexCURDService<OutComeType, OutComeTypeDto, CreateOutComeTypeDto, UpdateOutComeTypeDto>
    {
    }
}
