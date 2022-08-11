using KokazGoodsTransfer.Dtos.PointSettingsDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IPointSettingService : ICRUDService<PointsSetting, PointSettingsDto, CreatePointSetting, UpdatePointSettingDto>
    {
    }
}
