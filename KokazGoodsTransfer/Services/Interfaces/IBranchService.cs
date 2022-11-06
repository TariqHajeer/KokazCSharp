using KokazGoodsTransfer.Dtos.BranchDtos;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IBranchService : ICRUDService<Branch,BranchDto,CreateBranchDto,UpdateBranchDto>
    {
        Task<IEnumerable<NameAndIdDto>> GetLite();
    }
}
