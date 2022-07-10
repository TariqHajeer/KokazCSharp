using KokazGoodsTransfer.Dtos.BranchDtos;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IBranchService : ICRUDService<Branch,BranchDto,CreateBranchDto,UpdateBranchDto>
    {
    }
}
