using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface ICashService<TEntity, TDTO, CreateDto, UpdateDto> : IService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class where TDTO : class where CreateDto : class where UpdateDto : class
    {
        Task<IEnumerable<TDTO>> GetCashed();
        void RemoveCash();
    }
}
