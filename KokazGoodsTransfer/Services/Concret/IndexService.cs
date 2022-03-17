using AutoMapper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models.Infrastrcuter;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class IndexService<TEntity, TDTO, CreateDto, UpdateDto> : CRUDService<TEntity, TDTO, CreateDto, UpdateDto>, IIndexService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class, IIndex where TDTO : class where CreateDto : class, INameEntity where UpdateDto : class, IIndex
    {
        public IndexService(IRepository<TEntity> repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public override async Task<ErrorRepsonse<TDTO>> AddAsync(CreateDto createDto)
        {
            var similar = await _repository.Any(c => c.Name == createDto.Name);
            var response = new ErrorRepsonse<TDTO>();
            if (similar == true)
            {
                response.Errors.Add("Similar");
            }
            else
            {
                var entity = _mapper.Map<TEntity>(createDto);
                await _repository.AddAsync(entity);
                var dto = _mapper.Map<TDTO>(entity);
                response.Data = dto;
            }
            return response;
        }
        public override async Task<ErrorRepsonse<TDTO>> Update(UpdateDto updateDto)
        {
            var similar = await _repository.Any(c => c.Id != updateDto.Id && c.Name == updateDto.Name);
            var response = new ErrorRepsonse<TDTO>();
            if (similar == true)
            {
                response.Errors.Add("Similar");
            }
            else
            {
                var entity = await _repository.GetById(updateDto.Id);
                _mapper.Map(updateDto, entity);
                await _repository.Update(entity);
                response.Data = _mapper.Map<TDTO>(entity);
            }
            return response;
        }
    }
}
