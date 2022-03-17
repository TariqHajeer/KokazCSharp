using KokazGoodsTransfer.Dtos.Groups;
using KokazGoodsTransfer.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IGroupService:IIndexService<Group,GroupDto,CreateGroupDto,UpdateGroupDto>
    {
        Task<IEnumerable<PrivilegeDto>> GetPrivileges();
    }
}
