using AutoMapper;
using KokazGoodsTransfer.Dtos.PointSettingsDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
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
