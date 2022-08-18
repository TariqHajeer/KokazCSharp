using KokazGoodsTransfer.Dtos.Statics;
using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.Clients;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<MainStaticsDto> GetMainStatics();
        Task<IEnumerable<UserDto>> AgnetStatics();
        Task<AggregateDto> GetAggregate(DateFiter dateFiter);
        Task<AdminNotification> GetAdminNotification();
        Task<IEnumerable<ClientBlanaceDto>> GetClientBalance();
    }
}
