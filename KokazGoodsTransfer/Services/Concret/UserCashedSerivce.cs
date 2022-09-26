using AutoMapper;
using KokazGoodsTransfer.CustomException;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class UserCashedSerivce : CashService<User, UserDto, CreateUserDto, UpdateUserDto>, IUserCashedService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<UserPhone> _userPhoneRepository;
        private readonly IRepository<UserGroup> _userGroupRepositroy;
        private readonly ICountryCashedService _countryCashedService;
        public UserCashedSerivce(IRepository<User> repository, IMapper mapper, IMemoryCache cache, IRepository<Order> orderRepository, IRepository<UserPhone> userPhoneRepository, IRepository<UserGroup> userGroupRepositroy, Logging logging, IHttpContextAccessorService httpContextAccessorService, ICountryCashedService countryCashedService)
            : base(repository, mapper, cache, logging, httpContextAccessorService)
        {
            _orderRepository = orderRepository;
            _userPhoneRepository = userPhoneRepository;
            _userGroupRepositroy = userGroupRepositroy;
            _countryCashedService = countryCashedService;
        }
        public override async Task<ErrorRepsonse<UserDto>> Delete(int id)
        {
            var user = await _repository.GetById(id);
            if (user == null)
                return new ErrorRepsonse<UserDto>()
                {
                    Errors = new List<string>()
                    {
                        "Not.Found"
                    }
                };
            var canntDelete = new ErrorRepsonse<UserDto>(_mapper.Map<UserDto>(user));
            canntDelete.Errors.Add("Cannt.Delete");

            await _repository.LoadCollection(user, c => c.Incomes);
            if (user.Incomes.Any())
                return canntDelete;
            await _repository.LoadCollection(user, c => c.OutComes);
            if (user.OutComes.Any())
                return canntDelete;
            await _repository.LoadCollection(user, c => c.Clients);
            if (user.Clients.Any())
                return canntDelete;
            if (await _orderRepository.Any(c => c.AgentId == user.Id))
                return canntDelete;

            return await base.Delete(id);
        }

        public override async Task<IEnumerable<UserDto>> GetAll(params Expression<Func<User, object>>[] propertySelectors)
        {
            var list = await _repository.GetAsync(c => (c.CanWorkAsAgent == true && c.BranchId == _currentBranch) || (c.CanWorkAsAgent == false && c.Branches.Any(c => c.BranchId == _currentBranch)));

            var dtos = _mapper.Map<UserDto[]>(list).ToList();
            var agents = dtos.Where(c => c.CanWorkAsAgent == true).ToList();
            foreach (var agent in agents)
            {
                agent.UserStatics.OrderInStore = await _orderRepository.Count(c => c.AgentId == agent.Id && c.OrderplacedId == (int)OrderplacedEnum.Store);
                agent.UserStatics.OrderInWay = await _orderRepository.Count(c => c.AgentId == agent.Id && c.OrderplacedId == (int)OrderplacedEnum.Way);
            }
            return dtos;
        }
        public async Task<UserDto> AddAsync2(CreateUserDto createDto)
        {
            if (!string.IsNullOrEmpty(createDto.UserName) && await _repository.Any(c => c.UserName.ToLower() == createDto.UserName.ToLower()))
                throw new ConflictException("اسم المستخدم مكرر");
            if (await _repository.Any(c => c.Name.ToLower() == createDto.Name.ToLower()))
                throw new ConflictException("الأسم مكرر");
            var user = _mapper.Map<User>(createDto);
            if (createDto.CanWorkAsAgent)
            {
                var countries = await _countryCashedService.GetAsync(c => createDto.Countries.Contains(c.Id) && c.Branches.Any(), c => c.Branches);
                var branchesids = countries.SelectMany(c => c.BranchesIds).ToArray();
                if (branchesids.Except(_httpContextAccessorService.Branches()).Any())
                    throw new ConflictException("لا يمكنك  إضافة مدينة لمدنوب لديها فرع");
                user.BranchId = _currentBranch;
            }
            await _repository.AddAsync(user);
            if (user.CanWorkAsAgent)
                RemoveCash();
            return _mapper.Map<UserDto>(user);
        }
        public override async Task<IEnumerable<UserDto>> GetCashed()
        {
            if (!_cache.TryGetValue(cashName, out IEnumerable<UserDto> entites))
            {
                entites = await GetAsync(c => c.CanWorkAsAgent == true, c => c.AgentCountries.Select(c => c.Country), c => c.UserPhones);
                _cache.Set(cashName, entites);
            }
            return entites;
        }

        public override async Task<UserDto> GetById(int id)
        {
            var user = await _repository.FirstOrDefualt(c => c.Id == id, c => c.Branches, c => c.UserPhones, c => c.UserGroups, c => c.AgentCountries.Select(c => c.Country));
            var dto = _mapper.Map<UserDto>(user);
            if (dto.CanWorkAsAgent)
            {
                dto.UserStatics.OrderInStore = await _orderRepository.Count(c => c.AgentId == dto.Id && c.OrderplacedId == (int)OrderplacedEnum.Store);
                dto.UserStatics.OrderInWay = await _orderRepository.Count(c => c.AgentId == dto.Id && c.OrderplacedId == (int)OrderplacedEnum.Way);
            }
            return dto;
        }

        public virtual async Task DeletePhone(int id)
        {
            var userPhone = await _userPhoneRepository.GetById(id);
            var user = await _repository.GetById(userPhone.UserId);
            await _userPhoneRepository.Delete(userPhone);
            if (user.CanWorkAsAgent)
                RemoveCash();
        }

        public async Task DeleteGroup(int userId, int groupId)
        {
            var userGroup = await _userGroupRepositroy.FirstOrDefualt(c => c.UserId == userId && c.GroupId == groupId);
            if (userGroup != null)
                await _userGroupRepositroy.Delete(userGroup);
        }

        public async Task<PhoneDto> AddPhone(AddPhoneDto addPhoneDto)
        {
            var user = await _repository.GetById(addPhoneDto.objectId);
            if (user == null)
                return null;
            await _repository.LoadCollection(user, c => c.UserPhones);
            if (user.UserPhones.All(c => c.Phone == addPhoneDto.Phone))
                return null;
            var phone = new UserPhone()
            {
                UserId = user.Id,
                Phone = addPhoneDto.Phone
            };
            user.UserPhones.Add(phone);
            await _repository.Update(user);
            if (user.CanWorkAsAgent)
                RemoveCash();
            return _mapper.Map<PhoneDto>(phone);

        }

        public async Task AddToGroup(int userId, int groupId)
        {
            var user = await _repository.GetById(userId);
            user.UserGroups.Add(new UserGroup() { GroupId = groupId });
            await _repository.Update(user);
        }
        public override async Task<ErrorRepsonse<UserDto>> Update(UpdateUserDto updateDto)
        {
            var user = await _repository.GetById(updateDto.Id);
            await _repository.LoadCollection(user, c => c.AgentCountries);
            bool requeirdReacsh = user.CanWorkAsAgent == true || updateDto.CanWorkAsAgent != user.CanWorkAsAgent;
            var similerUserByname = await _repository.Any(c => c.Name.ToLower() == updateDto.Name.ToLower() && c.Id != updateDto.Id);
            if (similerUserByname)
            {
                return new ErrorRepsonse<UserDto>()
                {
                    Errors = new List<string>(){
                        "Employee.Exisit"
                    }
                };
            }
            updateDto.UserName = updateDto.UserName.Trim();
            if (!String.IsNullOrWhiteSpace(updateDto.UserName))
            {
                var similerUserByUserName = await _repository.Any(c => c.UserName.ToLower() == updateDto.UserName.ToLower() && c.Id != updateDto.Id);
                if (similerUserByUserName)
                {
                    return new ErrorRepsonse<UserDto>()
                    {
                        Errors = new List<string>(){
                        "Employee.Exisit"
                    }
                    };
                }
            }

            user.AgentCountries.Clear();
            _mapper.Map<UpdateUserDto, User>(updateDto, user);
            await _repository.Update(user);

            if (requeirdReacsh)
                RemoveCash();
            var response = new ErrorRepsonse<UserDto>(_mapper.Map<UserDto>(await GetById(user.Id)));
            return response;
        }
    }
}
