using AutoMapper;
using Quqaz.Web.Dtos.OrdersDtos.OrderWithBranchDto;
using Quqaz.Web.Models.TransferToBranchModels;

namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class TransferToSecondBranchProfile : Profile
    {
        public TransferToSecondBranchProfile()
        {
            CreateMap<TransferToOtherBranch, TransferToSecondBranchReportDto>()
                .ForMember(c => c.DestinationBranch, opt => opt.MapFrom(src => src.DestinationBranch.Name))
                .ForMember(c=>c.DriverName,opt=>opt.MapFrom(src=>src.Driver.Name));
            CreateMap<TransferToOtherBranchDetials, TransferToSecondBranchDetialsReportDto>();

        }
    }
}
