using KokazGoodsTransfer.Dtos.Statics;
using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.ClientDtos;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<MainStaticsDto> GetMainStatics();
        Task<IEnumerable<UserDto>> AgnetStatics();
        Task<AggregateDto> GetAggregate(DateFiter dateFiter);
        Task<IEnumerable<ClientBlanaceDto>> GetClientBalance();
        Task<StaticsDto> GetClientStatistic();
    }
}
