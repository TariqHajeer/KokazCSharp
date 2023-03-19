using Quqaz.Web.Dtos.Statics;
using System.Threading.Tasks;
using System.Collections.Generic;
using Quqaz.Web.Dtos.Users;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.ClientDtos;

namespace Quqaz.Web.Services.Interfaces
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
