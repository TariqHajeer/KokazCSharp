using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IRegionCashedService: ICashService<Region, RegionDto,CreateRegionDto,UpdateRegionDto>
    {

    }
}
