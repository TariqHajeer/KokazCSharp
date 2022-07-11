using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
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
        public OrderRepository(KokazContext kokazContext, IHttpContextAccessor httpContextAccessor) : base(kokazContext, httpContextAccessor)
        {
        }

        public async Task<PagingResualt<IEnumerable<Order>>> Get(Paging paging, OrderFilter filter, params Expression<Func<Order, object>>[] propertySelectors)
        {
            var query = _kokazContext.Orders.Include(c => c.Client)
                    .Include(c => c.Agent)
                    .Include(c => c.Region)
                    .Include(c => c.Country)
                    .Include(c => c.Orderplaced)
                    .Include(c => c.MoenyPlaced)
                    .Include(c => c.OrderClientPaymnets)
                        .ThenInclude(c => c.ClientPayment)
                    .Include(c => c.AgentOrderPrints)
                        .ThenInclude(c => c.AgentPrint)
               .AsQueryable();
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

            var totalTask = query.CountAsync();
            var dataTask = query.Skip((paging.Page - 1) * paging.RowCount).Take(paging.RowCount).ToListAsync();
            await Task.WhenAll(totalTask, dataTask);
            return new PagingResualt<IEnumerable<Order>>()
            {
                Total = await totalTask,
                Data = await dataTask,
            };
        }
    }
}
