using Quqaz.Web.Dtos.Countries;
using Quqaz.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface ICountryCashedService : ICashService<Country, CountryDto, CreateCountryDto, UpdateCountryDto>
    {
    }
}
