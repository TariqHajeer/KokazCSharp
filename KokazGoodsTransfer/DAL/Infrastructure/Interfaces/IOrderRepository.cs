using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Infrastructure.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<PagingResualt<IEnumerable<Order>>> Get(Paging paging, OrderFilter filter, string[] propertySelectors = null);
        Task<IEnumerable<Order>> OrdersDontFinished(OrderDontFinishedFilter orderDontFinishedFilter);
        Task<IEnumerable<Order>> OrderAtClient(OrderFilter orderFilter);
        Task<Order> GetByIdIncludeAllForEmployee(int id);


    }
}
