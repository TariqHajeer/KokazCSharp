using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Groups;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class GroupService : IndexCURDService<Group, GroupDto, CreateGroupDto, UpdateGroupDto>, IGroupService
    {
        private readonly IRepository<Privilege> _privilegeRepository;
        public GroupService(IRepository<Group> repository, IRepository<Privilege> privilegeRepository, IMapper mapper, Logging logging, IHttpContextAccessorService httpContextAccessorService)
            : base(repository, mapper, logging, httpContextAccessorService)
        {
            _privilegeRepository = privilegeRepository;
        }
        public override async Task<ErrorRepsonse<GroupDto>> AddAsync(CreateGroupDto createDto)
        {
            createDto.Name = createDto.Name.Trim();
            var similar = await _repository.Any(c => c.Name == createDto.Name);
            var response = new ErrorRepsonse<GroupDto>();
            if (similar)
            {
                response.Errors.Add("Similar");
            }
            else
            if (createDto.PrivilegesId?.Any() != true)
            {
                response.Errors.Add("Privileges is required");
            }
            else
            {
                var group = _mapper.Map<Group>(createDto);
                await _repository.AddAsync(group);
                var dto = _mapper.Map<GroupDto>(group);
                response.Data = dto;
            }
            return response;
        }
        public override async Task<ErrorRepsonse<GroupDto>> Update(UpdateGroupDto updateDto)
        {
            var similar = await _repository.Any(c => c.Id != updateDto.Id && c.Name == updateDto.Name);
            var response = new ErrorRepsonse<GroupDto>();
            if (similar)
            {
                response.Errors.Add("Similar");
            }
            else
            {
                var group = await _repository.FirstOrDefualt(c => c.Id == updateDto.Id);

                if (group == null)
                {
                    response.NotFound = true;
                }
                else
                {
                    group.Name = updateDto.Name;
                    await _repository.LoadCollection(group, c => c.GroupPrivileges);
                    group.GroupPrivileges = group.GroupPrivileges.Where(c => updateDto.Privileges.Contains(c.PrivilegId)).ToList();
                    var newPrivlieges = updateDto.Privileges.Except(group.GroupPrivileges.Select(c => c.PrivilegId));
                    foreach (var item in newPrivlieges)
                    {
                        group.GroupPrivileges.Add(new GroupPrivilege() { GroupId = group.Id, PrivilegId = item });
                    }
                    await _repository.Update(group);
                    response.Data = _mapper.Map<GroupDto>(group);
                }
            }
            return response;
        }
        public override async Task<ErrorRepsonse<GroupDto>> Delete(int id)
        {
            var group = (await _repository.GetAsync(c => c.Id == id, c => c.GroupPrivileges)).FirstOrDefault();
            var response = new ErrorRepsonse<GroupDto>();
            if (group == null)
            {
                response.NotFound = true;
            }
            else
            {
                var count = await _privilegeRepository.Count();
                if (group.GroupPrivileges.Count() == count)
                {
                    var groups = await _repository.GetAll(c => c.GroupPrivileges);
                    if (groups.Any(c => c.GroupPrivileges.Count() != count))
                    {
                        response.CantDelete = true;
                    }
                    else
                    {
                        await _repository.Delete(group);
                        response.Data = _mapper.Map<GroupDto>(group);
                    }
                }
                else
                {
                    await _repository.Delete(group);
                    response.Data = _mapper.Map<GroupDto>(group);
                }
            }
            return response;
        }
        public async Task<IEnumerable<Privilege>> GetPrviligesByUserAndBranchId(int userId ,int branchId)
        {
            var groups = await _repository.GetByFilterInclue(c => c.UserGroups.Any(c => c.UserId == userId) && c.BranchId == branchId, new string[] { "GroupPrivileges.Privileg" });
            return groups.SelectMany(c => c.GroupPrivileges.Select(c => c.Privileg));
        }
        public async Task<IEnumerable<Privilege>> GetGroupsPrviligesByGroupsIds(IEnumerable<int> groupsIds, int branchId)
        {
            var groups = await _repository.GetByFilterInclue(c => c.BranchId == branchId && groupsIds.Contains(c.Id), new string[] { "GroupPrivileges.Privileg" });
            var privileges = groups.SelectMany(c => c.GroupPrivileges.Select(c => c.Privileg));
            privileges = privileges.GroupBy(c => c.Id).Select(c => c.First());
            return privileges;

        }
        public async Task<IEnumerable<PrivilegeDto>> GetPrivileges()
        {
            var priv = await _privilegeRepository.GetAll();
            return _mapper.Map<PrivilegeDto[]>(priv);
        }
    }
}
