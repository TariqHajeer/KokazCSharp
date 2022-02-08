using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface ICountryCashedService:ICashService<Country,CountryDto,CreateCountryDto,UpdateCountryDto>
    {
    }
}
