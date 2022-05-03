using KokazGoodsTransfer.Models;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.TreasuryDtos;
using KokazGoodsTransfer.Services.Helper;
using System.Collections.Generic;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface ITreasuryService
    {
        Task<ErrorRepsonse<TreasuryDto>> Create(CreateTreasuryDto createTreasuryDto);
        Task<TreasuryDto> GetById(int id);
    }
}
