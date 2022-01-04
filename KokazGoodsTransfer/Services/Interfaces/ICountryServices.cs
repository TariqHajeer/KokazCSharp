using KokazGoodsTransfer.Dtos.Countries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface ICountryServices
    {
        List<CountryDto> GetAll();

    }
}
