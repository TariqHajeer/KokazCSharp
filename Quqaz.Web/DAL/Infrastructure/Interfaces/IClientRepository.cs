using Quqaz.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quqaz.Web.DAL.Infrastructure.Interfaces
{
    public interface IClientRepository:IRepository<Client>
    {
        Task<List<Client>> GetClientsByBranchId(int branchId);
    }
}
