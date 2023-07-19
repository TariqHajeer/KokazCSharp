using AutoMapper;
using Quqaz.Web.Dtos.PointSettingsDtos;
using Quqaz.Web.Models;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class PointProfile:Profile
    {
        public PointProfile()
        {
            CreateMap<CreatePointSetting, PointsSetting>();
            CreateMap<PointsSetting, PointSettingsDto>();
        }
    }
}
