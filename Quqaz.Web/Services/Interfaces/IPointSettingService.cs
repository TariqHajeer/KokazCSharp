using Quqaz.Web.Dtos.PointSettingsDtos;
using Quqaz.Web.Models;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IPointSettingService : ICRUDService<PointsSetting, PointSettingsDto, CreatePointSetting, UpdatePointSettingDto>
    {
    }
}
