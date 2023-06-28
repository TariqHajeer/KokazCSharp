using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Models;
using Quqaz.Web.Models.Static;
using Quqaz.Web.Services.Interfaces;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.DAL.Infrastructure.Concret
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(KokazContext kokazContext, IHttpContextAccessorService httpContextAccessorService) : base(kokazContext, httpContextAccessorService)
        {
            Query = kokazContext.Set<Order>().AsQueryable();
            Query = Query.Where(c => c.BranchId == branchId || c.TargetBranchId == branchId || c.NextBranchId == branchId || c.CurrentBranchId == branchId);
        }
        public override Task Update(IEnumerable<Order> entites)
        {
            var updateDate = DateTime.UtcNow;
            entites.ForEach(c => { c.UpdatedBy = _httpContextAccessorService.AuthoticateUserName(); c.UpdatedDate = updateDate; });

            return base.Update(entites);
        }
        public async Task<PagingResualt<IEnumerable<Order>>> Get(PagingDto paging, OrderFilter filter, string[] propertySelectors = null)
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
            if (filter.MoneyPalced != null)
            {
                query = query.Where(c => c.MoneyPlace == filter.MoneyPalced);
            }
            if (filter.Orderplaced != null)
            {
                query = query.Where(c => c.OrderPlace == filter.Orderplaced);
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
            if (!string.IsNullOrEmpty(filter.CreatedBy?.Trim()))
            {
                query = query.Where(c => c.CreatedBy == filter.CreatedBy);
            }
            if (filter.CreatedDateRangeFilter != null)
            {
                if (filter.CreatedDateRangeFilter.Start != null)
                    query = query.Where(c => c.Date >= filter.CreatedDateRangeFilter.Start);
                if (filter.CreatedDateRangeFilter.End != null)
                    query = query.Where(c => c.Date <= filter.CreatedDateRangeFilter.End);

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
   .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<string>> GetCreatedByNames()
        {
            return await Query.Select(c => c.CreatedBy).Distinct().ToListAsync();
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
                .Where(c => c.IsSend == false && c.OrderPlace == OrderPlace.Client)
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
            if (orderFilter.MoneyPalced != null)
            {
                orderIQ = orderIQ.Where(c => c.MoneyPlace == (MoneyPalce)orderFilter.MoneyPalced);
            }
            if (orderFilter.Orderplaced != null)
            {
                orderIQ = orderIQ.Where(c => c.OrderPlace == orderFilter.Orderplaced);
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

        public async Task<PagingResualt<IEnumerable<Order>>> OrdersDontFinished(OrderDontFinishedFilter orderDontFinishedFilter, PagingDto pagingDto)
        {
            var predicate = PredicateBuilder.New<Order>(c => c.ClientId == orderDontFinishedFilter.ClientId && orderDontFinishedFilter.OrderPlacedId.Contains(c.OrderPlace) && c.AgentId != null);
            if (orderDontFinishedFilter.OrderPlacedId.Contains(OrderPlace.CompletelyReturned) || orderDontFinishedFilter.OrderPlacedId.Contains(OrderPlace.Unacceptable) || orderDontFinishedFilter.OrderPlacedId.Contains(OrderPlace.PartialReturned))
            {
                var orderFilterExcpt = orderDontFinishedFilter.OrderPlacedId.Except(new[] { OrderPlace.CompletelyReturned, OrderPlace.Unacceptable, OrderPlace.PartialReturned });
                predicate = PredicateBuilder.New<Order>(c => c.ClientId == orderDontFinishedFilter.ClientId && orderFilterExcpt.Contains(c.OrderPlace) && c.AgentId != null);
                var orderPlacePredicate = PredicateBuilder.New<Order>(false);
                if (orderDontFinishedFilter.OrderPlacedId.Contains(OrderPlace.CompletelyReturned))
                {
                    orderPlacePredicate = orderPlacePredicate.Or(c => c.OrderPlace == OrderPlace.CompletelyReturned && c.CurrentBranchId == _httpContextAccessorService.CurrentBranchId());
                }
                if (orderDontFinishedFilter.OrderPlacedId.Contains(OrderPlace.Unacceptable))
                {
                    orderPlacePredicate = orderPlacePredicate.Or(c => c.OrderPlace == OrderPlace.Unacceptable && c.CurrentBranchId == _httpContextAccessorService.CurrentBranchId());
                }
                if (orderDontFinishedFilter.OrderPlacedId.Contains(OrderPlace.PartialReturned))
                {
                    orderPlacePredicate = orderPlacePredicate.Or(c => c.OrderPlace == OrderPlace.PartialReturned && c.CurrentBranchId == _httpContextAccessorService.CurrentBranchId());
                }
                orderPlacePredicate = orderPlacePredicate.And(c => c.ClientId == orderDontFinishedFilter.ClientId && c.AgentId != null);
                predicate = predicate.Or(orderPlacePredicate);
                
            }
            if (orderDontFinishedFilter.ClientDoNotDeleviredMoney && !orderDontFinishedFilter.IsClientDeleviredMoney)
            {
                predicate = predicate.And(c => c.IsClientDiliverdMoney == false);
            }
            else if (!orderDontFinishedFilter.ClientDoNotDeleviredMoney && orderDontFinishedFilter.IsClientDeleviredMoney)
            {
                predicate = predicate.And(c => c.OrderState == OrderState.ShortageOfCash);
            }
            else if (orderDontFinishedFilter.ClientDoNotDeleviredMoney && orderDontFinishedFilter.IsClientDeleviredMoney)
            {
                predicate = predicate.And(c => c.OrderState == OrderState.ShortageOfCash || c.IsClientDiliverdMoney == false);
            }
            if (orderDontFinishedFilter.TableSelection != null)
            {
                if (orderDontFinishedFilter.TableSelection.IsSelectedAll)
                {
                    if (orderDontFinishedFilter.TableSelection.ExceptIds?.Any() == true)
                    {
                        predicate = predicate.And(c => !orderDontFinishedFilter.TableSelection.ExceptIds.Contains(c.Id));
                    }
                }
                else
                {
                    if (orderDontFinishedFilter.TableSelection.SelectedIds?.Any() == true)
                    {
                        predicate = predicate.And(c => orderDontFinishedFilter.TableSelection.SelectedIds.Contains(c.Id));
                    }

                }
            }
            var query = Query.Include(c=>c.Country).Where(predicate);
            var total = await query.CountAsync();
            var data = await query.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount).ToListAsync();
            return new PagingResualt<IEnumerable<Order>>()
            {
                Total = total,
                Data = data
            };
        }
    }
}
