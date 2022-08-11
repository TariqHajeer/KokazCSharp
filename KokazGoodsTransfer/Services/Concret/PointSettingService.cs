using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.PointSettingsDtos;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class PointSettingService : CRUDService<PointsSetting, PointSettingsDto, CreatePointSetting, UpdatePointSettingDto>, IPointSettingService
    {
        public PointSettingService(IRepository<PointsSetting> repository, IMapper mapper, Logging logging, IHttpContextAccessorService httpContextAccessorService) : base(repository, mapper, logging, httpContextAccessorService)
        {
        }

        public override Task<ErrorRepsonse<PointSettingsDto>> Update(UpdatePointSettingDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
