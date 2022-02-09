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
        public UserCashedSerivce(IRepository<User> repository, IMapper mapper, IMemoryCache cache, IRepository<Order> orderRepository) : base(repository, mapper, cache)
        {
            _orderRepository = orderRepository;
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
        public override Task<ErrorRepsonse<UserDto>> AddAsync(CreateUserDto createDto)
        {

            return base.AddAsync(createDto);
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

    }
}
