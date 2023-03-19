using Quqaz.Web.Dtos.BranchDtos;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IBranchService : ICRUDService<Branch,BranchDto,CreateBranchDto,UpdateBranchDto>
    {
        Task<IEnumerable<NameAndIdDto>> GetLite();
    }
}
