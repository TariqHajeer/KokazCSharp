
using System.Collections.Generic;
using System.Threading.Tasks;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(KokazContext kokazContext) : base(kokazContext)
        {
        }

        public async Task<List<Country>> GetAllCountryIncludeALl()
        {
            return await this._kokazContext.Countries.Include(c => c.Regions).Include(c => c.Clients).Include(c => c.AgentCountrs).ThenInclude(c => c.Agent).ToListAsync();
        }
    }
}