
using KokazGoodsTransfer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface ICountryRepository:IRepository<Country>
    {
         Task<List<Country>> GetAllCountryIncludeALl();
    }
}