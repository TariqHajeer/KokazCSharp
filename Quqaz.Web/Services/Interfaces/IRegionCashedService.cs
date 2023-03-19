using Quqaz.Web.Dtos.Regions;
using Quqaz.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IRegionCashedService: ICashService<Region, RegionDto,CreateRegionDto,UpdateRegionDto>
    {

    }
}
