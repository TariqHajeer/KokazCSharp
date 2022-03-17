using KokazGoodsTransfer.Dtos.OutComeTypeDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IOutcomeTypeService : IIndexService<OutComeType, OutComeTypeDto, CreateOutComeTypeDto, UpdateOutComeTypeDto>
    {
    }
}
