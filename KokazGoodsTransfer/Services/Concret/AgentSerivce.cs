using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class AgentSerivce : CashService<User, UserDto, CreateUserDto, UpdateUserDto>, IAgentService
    {
        public AgentSerivce(IRepository<User> repository, IMapper mapper, IMemoryCache cache) : base(repository, mapper, cache)
        {
        }
        public override async Task<IEnumerable<UserDto>> GetCashed()
        {
            var name = typeof(User).FullName;
            if (!_cache.TryGetValue(name, out IEnumerable<UserDto> entites))
            {
                var list = await GetAsync(null, c => c.AgentCountrs.Select(c=>c.Country),c=>c.UserPhones);
                entites = _mapper.Map<UserDto[]>(list);
                _cache.Set(name, entites);
            }
            return entites;
        }

    }
}
