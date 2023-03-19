using Quqaz.Web.Dtos.Groups;
using Quqaz.Web.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IGroupService:IIndexCURDService<Group,GroupDto,CreateGroupDto,UpdateGroupDto>
    {
        Task<IEnumerable<PrivilegeDto>> GetPrivileges();
        Task<IEnumerable<Privilege>> GetGroupsPrviligesByGroupsIds(IEnumerable<int> groupsIds, int branchId);
        Task<IEnumerable<Privilege>> GetPrviligesByUserAndBranchId(int userId, int branchId);
    }
}
