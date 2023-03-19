using AutoMapper;
using Quqaz.Web.Dtos.Groups;
using Quqaz.Web.Models;
using System.Collections.Generic;
using System.Linq;
namespace Quqaz.Web.Dtos.AutoMapperProfile
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDto>()
                .ForMember(c => c.PrivilegesId, opt => opt.MapFrom(src => src.GroupPrivileges.Select(c => c.PrivilegId)))
                .ForMember(c => c.Users, opt => opt.MapFrom(src => src.UserGroups.Select(c => c.User.Name).ToArray()));
            CreateMap<CreateGroupDto, Group>()
                .ForMember(c => c.GroupPrivileges, opt => opt.MapFrom((src, obj, i, context) =>
                {
                    var groupPrivileges = src.PrivilegesId.Select(c => new GroupPrivilege() { PrivilegId = c });
                    return groupPrivileges;
                }));
            CreateMap<Privilege, PrivilegeDto>();
        }
    }
}
