using AutoMapper;
using Quqaz.Web.Dtos.Countries;
using Quqaz.Web.Dtos.DiscountDtos;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Dtos.ReceiptDtos;
using Quqaz.Web.Dtos.Regions;
using Quqaz.Web.Dtos.Users;
using Quqaz.Web.Models;
using Quqaz.Web.Models.Infrastrcuter;
using Quqaz.Web.Models.Static;
using System.Linq;

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
