using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Quqaz.Web.DAL.Infrastructure.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<PagingResualt<IEnumerable<Order>>> Get(PagingDto paging, OrderFilter filter, string[] propertySelectors = null);
        Task<PagingResualt<IEnumerable<Order>>> OrdersDontFinished(OrderDontFinishedFilter orderDontFinishedFilter, PagingDto pagingDto);
        Task<IEnumerable<Order>> OrderAtClient(OrderFilter orderFilter);
        Task<Order> GetByIdIncludeAllForEmployee(int id);
        Task<IEnumerable<string>> GetCreatedByNames();
        Task<(PagingResualt<IEnumerable<Order>> pagingResualt, decimal ordersCost, decimal deliveryCost)> OrdersDontFinished2(OrderDontFinishedFilter orderDontFinishedFilter, PagingDto pagingDto);

    }
}
