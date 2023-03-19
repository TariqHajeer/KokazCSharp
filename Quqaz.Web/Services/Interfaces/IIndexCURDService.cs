using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Models.Infrastrcuter;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IIndexCURDService<TEntity, TDTO, CreateDto, UpdateDto> : ICRUDService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class, IIndex where TDTO : class where CreateDto : class, INameEntity where UpdateDto : class, IIndex
    {
    }
}
