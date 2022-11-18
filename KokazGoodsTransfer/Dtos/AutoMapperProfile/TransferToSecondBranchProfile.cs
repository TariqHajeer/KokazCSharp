using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersDtos.OrderWithBranchDto;
using KokazGoodsTransfer.Models.TransferToBranchModels;

namespace KokazGoodsTransfer.Dtos.AutoMapperProfile
{
    public class TransferToSecondBranchProfile : Profile
    {
        public TransferToSecondBranchProfile()
        {
            CreateMap<TransferToOtherBranch, TransferToSecondBranchReportDto>()
                .ForMember(c => c.DestinationBranch, opt => opt.MapFrom(src => src.DestinationBranch.Name));
        }
    }
}
