using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Models.Infrastrcuter;
using Quqaz.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace Quqaz.Web.Services.Concret
{
    public class IndexService<TEntity> : IIndexService<TEntity> where TEntity : class, IIndex
    {
        private readonly IIndexRepository<TEntity> _indexRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cash;
        public IndexService(IIndexRepository<TEntity> indexRepository, IMapper mapper, IMemoryCache cash)
        {
            _indexRepository = indexRepository;
            _mapper = mapper;
            _cash = cash;

        }
        public async Task<IEnumerable<NameAndIdDto>> GetAllLite()
        {
            var className = typeof(TEntity).FullName;
            var cashName = className + "-" + typeof(IndexService<TEntity>).Name;
            if (!_cash.TryGetValue(cashName, out IEnumerable<NameAndIdDto> result) || !result.Any())
            {
                var list = await _indexRepository.GetLiteList();
                result = _mapper.Map<IEnumerable<NameAndIdDto>>(list);
                _cash.Set(cashName, result);
            }
            return result;
        }
    }
}
