using Microsoft.EntityFrameworkCore;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Models;
using Quqaz.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.DAL.Infrastructure.Concret
{
    public class ClientRepository : Repository<Client>, IClientRepository
    {
        public ClientRepository(KokazContext kokazContext, IHttpContextAccessorService httpContextAccessorService) : base(kokazContext, httpContextAccessorService)
        {
        }

        public Task<List<Client>> GetClientsByBranchId(int branchId)
        {
            return _kokazContext.Clients.Where(c => c.BranchId == branchId).ToListAsync();
        }
    }
}
