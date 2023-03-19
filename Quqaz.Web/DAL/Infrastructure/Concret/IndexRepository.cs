using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Models;
using Quqaz.Web.Models.Infrastrcuter;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quqaz.Web.Services.Interfaces;

namespace Quqaz.Web.DAL.Infrastructure.Concret
{
    public class IndexRepository<T> : Repository<T>, IIndexRepository<T> where T : class, IIndex
    {
        public IndexRepository(KokazContext kokazContext, IHttpContextAccessorService httpContextAccessorService) : base(kokazContext, httpContextAccessorService)
        {
        }

        public virtual async Task<IEnumerable<IndexEntity>> GetLiteList()
        {
            return await _kokazContext.Set<T>().Select(c => new IndexEntity() { Id = c.Id, Name = c.Name }).ToListAsync();
        }
    }
}
