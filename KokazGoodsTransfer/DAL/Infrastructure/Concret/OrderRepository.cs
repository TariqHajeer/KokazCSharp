using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using KokazGoodsTransfer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.DAL.Infrastructure.Concret
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(KokazContext kokazContext, IHttpContextAccessorService httpContextAccessorService) : base(kokazContext, httpContextAccessorService)
        {
            Query = Query.Where(c => c.BranchId == branchId || c.SecondBranchId == branchId);
        }

        public async Task<PagingResualt<IEnumerable<Order>>> Get(Paging paging, OrderFilter filter, string[] propertySelectors = null)
        {
            var query = Query;
            if (propertySelectors.Any() == true)
            {
                foreach (var item in propertySelectors)
                {
                    query = query.Include(item);
                }
            }
            if (filter.CountryId != null)
            {
                query = query.Where(c => c.CountryId == filter.CountryId);
            }
            if (filter.Code != string.Empty && filter.Code != null)
            {
                query = query.Where(c => c.Code.StartsWith(filter.Code));
            }
            if (filter.ClientId != null)
            {
                query = query.Where(c => c.ClientId == filter.ClientId);
            }
            if (filter.RegionId != null)
            {
                query = query.Where(c => c.RegionId == filter.RegionId);
            }
            if (filter.RecipientName != string.Empty && filter.RecipientName != null)
            {
                query = query.Where(c => c.RecipientName.StartsWith(filter.RecipientName));
            }
            if (filter.MonePlacedId != null)
            {
                query = query.Where(c => c.MoenyPlacedId == filter.MonePlacedId);
            }
            if (filter.OrderplacedId != null)
            {
                query = query.Where(c => c.OrderplacedId == filter.OrderplacedId);
            }
            if (filter.Phone != string.Empty && filter.Phone != null)
            {
                query = query.Where(c => c.RecipientPhones.Contains(filter.Phone));
            }
            if (filter.AgentId != null)
            {
                query = query.Where(c => c.AgentId == filter.AgentId);
            }
            if (filter.IsClientDiliverdMoney != null)
            {
                query = query.Where(c => c.IsClientDiliverdMoney == filter.IsClientDiliverdMoney);
            }
            if (filter.ClientPrintNumber != null)
            {
                query = query.Where(c => c.OrderClientPaymnets.Any(op => op.ClientPayment.Id == filter.ClientPrintNumber));
            }
            if (filter.AgentPrintNumber != null)
            {
                query = query.Where(c => c.AgentOrderPrints.Any(c => c.AgentPrint.Id == filter.AgentPrintNumber));
            }
            if (filter.CreatedDate != null)
            {
                query = query.Where(c => c.Date == filter.CreatedDate);
            }
            if (filter.Note != "" && filter.Note != null)
            {
                query = query.Where(c => c.Note.Contains(filter.Note));
            }
            if (filter.AgentPrintStartDate != null)
            {
                ///TODO :
                ///chould check this query 
                query = query.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date >= filter.AgentPrintStartDate);

            }
            if (filter.AgentPrintEndDate != null)
            {
                ///TODO :
                ///chould check this query 
                query = query.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date <= filter.AgentPrintEndDate);
            }

            var total = await query.CountAsync();
            var data = paging == null ? await query.ToListAsync() : await query.Skip((paging.Page - 1) * paging.RowCount).Take(paging.RowCount).ToListAsync();
            return new PagingResualt<IEnumerable<Order>>()
            {
                Total = total,
                Data = data,
            };
        }
        
        public async Task<Order> GetByIdIncludeAllForEmployee(int id)
        {
            return await Query.Include(c => c.Client)
           .ThenInclude(c => c.ClientPhones)
           .Include(c => c.Client)
           .ThenInclude(c => c.Country)
       .Include(c => c.Agent)
           .ThenInclude(c => c.UserPhones)
       .Include(c => c.Region)
       .Include(c => c.Country)
       .Include(c => c.Orderplaced)
       .Include(c => c.MoenyPlaced)
       .Include(c => c.OrderItems)
           .ThenInclude(c => c.OrderTpye)
       .Include(c => c.OrderClientPaymnets)
       .ThenInclude(c => c.ClientPayment)
       .Include(c => c.OrderLogs)
       .Include(c => c.AgentOrderPrints)
            .ThenInclude(c => c.AgentPrint)
       .Include(c => c.ReceiptOfTheOrderStatusDetalis)
            .ThenInclude(c => c.ReceiptOfTheOrderStatus)
            .ThenInclude(c => c.Recvier)
   .FirstOrDefaultAsync(c => c.Id == Id);
        }

        public async Task<IEnumerable<Order>> OrderAtClient(OrderFilter orderFilter)
        {
            var orderIQ = Query
                    .Include(c => c.Client)
                    .Include(c => c.Country)
                    .Include(c => c.Client)
                .ThenInclude(c => c.ClientPhones)
                .Include(c => c.Client)
                .ThenInclude(c => c.Country)
                .Include(c => c.Region)
                .Include(c => c.Country)
                    .ThenInclude(c => c.AgentCountries)
                        .ThenInclude(c => c.Agent)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .Where(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client)
               .AsQueryable();
            if (orderFilter.CountryId != null)
            {
                orderIQ = orderIQ.Where(c => c.CountryId == orderFilter.CountryId);
            }
            if (orderFilter.Code != string.Empty && orderFilter.Code != null)
            {
                orderIQ = orderIQ.Where(c => c.Code.StartsWith(orderFilter.Code));
            }
            if (orderFilter.ClientId != null)
            {
                orderIQ = orderIQ.Where(c => c.ClientId == orderFilter.ClientId);
            }
            if (orderFilter.RegionId != null)
            {
                orderIQ = orderIQ.Where(c => c.RegionId == orderFilter.RegionId);
            }
            if (orderFilter.RecipientName != string.Empty && orderFilter.RecipientName != null)
            {
                orderIQ = orderIQ.Where(c => c.RecipientName.StartsWith(orderFilter.RecipientName));
            }
            if (orderFilter.MonePlacedId != null)
            {
                orderIQ = orderIQ.Where(c => c.MoenyPlacedId == orderFilter.MonePlacedId);
            }
            if (orderFilter.OrderplacedId != null)
            {
                orderIQ = orderIQ.Where(c => c.OrderplacedId == orderFilter.OrderplacedId);
            }
            if (orderFilter.Phone != string.Empty && orderFilter.Phone != null)
            {
                orderIQ = orderIQ.Where(c => c.RecipientPhones.Contains(orderFilter.Phone));
            }
            if (orderFilter.AgentId != null)
            {
                orderIQ = orderIQ.Where(c => c.AgentId == orderFilter.AgentId);
            }
            if (orderFilter.IsClientDiliverdMoney != null)
            {
                orderIQ = orderIQ.Where(c => c.IsClientDiliverdMoney == orderFilter.IsClientDiliverdMoney);
            }
            if (orderFilter.ClientPrintNumber != null)
            {
                orderIQ = orderIQ.Where(c => c.OrderClientPaymnets.Any(op => op.ClientPayment.Id == orderFilter.ClientPrintNumber));
            }
            if (orderFilter.AgentPrintNumber != null)
            {
                orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Any(c => c.AgentPrint.Id == orderFilter.AgentPrintNumber));
            }
            if (orderFilter.CreatedDate != null)
            {
                orderIQ = orderIQ.Where(c => c.Date == orderFilter.CreatedDate);
            }
            if (orderFilter.Note != "" && orderFilter.Note != null)
            {
                orderIQ = orderIQ.Where(c => c.Note.Contains(orderFilter.Note));
            }
            if (orderFilter.AgentPrintStartDate != null)
            {
                ///TODO :
                ///chould check this query 
                orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date >= orderFilter.AgentPrintStartDate);
            }
            if (orderFilter.AgentPrintEndDate != null)
            {
                ///TODO :
                ///chould check this query 
                orderIQ = orderIQ.Where(c => c.AgentOrderPrints.Select(c => c.AgentPrint).OrderBy(c => c.Id).LastOrDefault().Date <= orderFilter.AgentPrintEndDate);
            }
            return await orderIQ.ToListAsync();
        }

        public async Task<IEnumerable<Order>> OrdersDontFinished(OrderDontFinishedFilter orderDontFinishedFilter)
        {
            List<Order> orders = new List<Order>();
            if (orderDontFinishedFilter.ClientDoNotDeleviredMoney)
            {
                var list = await Query.Where(c => c.IsClientDiliverdMoney == false && c.ClientId == orderDontFinishedFilter.ClientId && orderDontFinishedFilter.OrderPlacedId.Contains(c.OrderplacedId))
                   .Include(c => c.Region)
                   .Include(c => c.Country)
                   .Include(c => c.MoenyPlaced)
                   .Include(c => c.Orderplaced)
                   .Include(c => c.Agent)
                   .Include(c => c.OrderClientPaymnets)
                   .ThenInclude(c => c.ClientPayment)
                   .Include(c => c.AgentOrderPrints)
                   .ThenInclude(c => c.AgentPrint)
                   .ToListAsync();
                if (list != null && list.Count() > 0)
                {
                    orders.AddRange(list);
                }
            }
            if (orderDontFinishedFilter.IsClientDeleviredMoney)
            {

                var list = await Query.Where(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash && c.ClientId == orderDontFinishedFilter.ClientId && orderDontFinishedFilter.OrderPlacedId.Contains(c.OrderplacedId))
               .Include(c => c.Region)
               .Include(c => c.Country)
               .Include(c => c.Orderplaced)
               .Include(c => c.MoenyPlaced)
               .Include(c => c.Agent)
               .Include(c => c.OrderClientPaymnets)
                   .ThenInclude(c => c.ClientPayment)
                   .Include(c => c.AgentOrderPrints)
                   .ThenInclude(c => c.AgentPrint)
               .ToListAsync();
                if (list != null && list.Count() > 0)
                {
                    orders.AddRange(list);
                }
            }
            return orders;
        }
    }
}
