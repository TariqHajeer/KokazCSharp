using KokazGoodsTransfer.Models.Infrastrcuter;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IIndexService<TEntity, TDTO, CreateDto, UpdateDto>:ICRUDService<TEntity, TDTO, CreateDto, UpdateDto> where TEntity : class, IIndex where TDTO : class where CreateDto : class, INameEntity where UpdateDto : class,IIndex
    {
    }
}
