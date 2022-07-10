using AutoMapper;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models.Infrastrcuter;
using KokazGoodsTransfer.Services.Helper;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class IndexCURDService<TEntity, TDTO, CreateDto, UpdateDto> : CRUDService<TEntity, TDTO, CreateDto, UpdateDto>, IIndexCURDService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class, IIndex where TDTO : class where CreateDto : class, INameEntity where UpdateDto : class, IIndex
    {

        public IndexCURDService(IRepository<TEntity> repository, IMapper mapper, Logging logging, IHttpContextAccessor httpContextAccessor) : base(repository, mapper, logging, httpContextAccessor)
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
