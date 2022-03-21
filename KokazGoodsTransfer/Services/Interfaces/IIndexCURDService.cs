using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Models.Infrastrcuter;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IIndexCURDService<TEntity, TDTO, CreateDto, UpdateDto> : ICRUDService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class, IIndex where TDTO : class where CreateDto : class, INameEntity where UpdateDto : class, IIndex
    {
    }
}
