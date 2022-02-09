using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Users;
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
        public UserCashedSerivce(IRepository<User> repository, IMapper mapper, IMemoryCache cache, IRepository<Order> orderRepository, IRepository<UserPhone> userPhoneRepository, IRepository<UserGroup> userGroupRepositroy) : base(repository, mapper, cache)
        {
            _orderRepository = orderRepository;
            _userPhoneRepository = userPhoneRepository;
            _userGroupRepositroy = userGroupRepositroy;
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
        public override async Task<List<UserDto>> GetALl(params Expression<Func<User, object>>[] propertySelectors)
        {
            var dtos = await base.GetALl(propertySelectors);
            var agents = dtos.Where(c => c.CanWorkAsAgent == true).ToList();
            foreach (var agent in agents)
            {
                agent.UserStatics.OrderInStore = await _orderRepository.Count(c => c.AgentId == agent.Id && c.OrderplacedId == (int)OrderplacedEnum.Store);
                agent.UserStatics.OrderInWay = await _orderRepository.Count(c => c.AgentId == agent.Id && c.OrderplacedId == (int)OrderplacedEnum.Way);
            }
            return dtos;
        }
        public override async Task<ErrorRepsonse<UserDto>> AddAsync(CreateUserDto createDto)
        {
            var response = new ErrorRepsonse<UserDto>();
            if (await _repository.Any(c => c.UserName.ToLower() == createDto.UserName.ToLower()))
            {
                response.Errors.Add("UserName.Exisit");
                return response;
            }
            if (await _repository.Any(c => c.Name == createDto.Name))
            {
                response.Errors.Add("Name.Exisit");
                return response;
            }
            var user = _mapper.Map<User>(createDto);
            await _repository.AddAsync(user);
            response = new ErrorRepsonse<UserDto>(_mapper.Map<UserDto>(user));
            if (user.CanWorkAsAgent)
            {
                await RefreshCash();
            }
            return response;
        }
        public override async Task<IEnumerable<UserDto>> GetCashed()
        {
            var name = typeof(User).FullName;
            if (!_cache.TryGetValue(name, out IEnumerable<UserDto> entites))
            {
                var list = await GetAsync(null, c => c.AgentCountrs.Select(c => c.Country), c => c.UserPhones);
                entites = _mapper.Map<UserDto[]>(list);
                _cache.Set(name, entites);
            }
            return entites;
        }

        public async Task<UserDto> GetById(int id)
        {
            var user = await _repository.FirstOrDefualt(c => c.Id == id, c => c.UserPhones, c => c.UserGroups, c => c.AgentCountrs.Select(c => c.Country));
            var dto = _mapper.Map<UserDto>(user);
            if (dto.CanWorkAsAgent)
            {
                dto.UserStatics.OrderInStore = await _orderRepository.Count(c => c.AgentId == dto.Id && c.OrderplacedId == (int)OrderplacedEnum.Store);
                dto.UserStatics.OrderInWay = await _orderRepository.Count(c => c.AgentId == dto.Id && c.OrderplacedId == (int)OrderplacedEnum.Way);
            }
            return dto;
        }

        public async Task DeletePhone(int id)
        {
            var userPhone = await _userPhoneRepository.GetById(id);
            var user = await _repository.GetById(userPhone.UserId);
            await _userPhoneRepository.Delete(userPhone);
            if (user.CanWorkAsAgent)
                await RefreshCash();
        }

        public async Task DeleteGroup(int userId, int groupId)
        {
            var userGroup = await _userGroupRepositroy.FirstOrDefualt(c => c.UserId == userId && c.GroupId == groupId);
            if (userGroup != null)
                await _userGroupRepositroy.Delete(userGroup);
        }
    }
}
