using AutoMapper;
using Quqaz.Web.Models.Infrastrcuter;

namespace Quqaz.Web.Dtos.Common
{
    public class AutoMapperProfile : Profile
    {

        public AutoMapperProfile()
        {
            CreateMap<IIndex, NameAndIdDto>();
        }
    }
}
