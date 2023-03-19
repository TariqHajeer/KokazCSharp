using AutoMapper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.PointSettingsDtos;
using Quqaz.Web.Helpers;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Helper;
using Quqaz.Web.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Concret
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
