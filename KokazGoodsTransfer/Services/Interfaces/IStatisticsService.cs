using KokazGoodsTransfer.Dtos.Statics;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<MainStaticsDto> GetMainStatics();
    }
}
