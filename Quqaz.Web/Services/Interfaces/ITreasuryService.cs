using Quqaz.Web.Models;
using System.Threading.Tasks;
using Quqaz.Web.Dtos.TreasuryDtos;
using Quqaz.Web.Services.Helper;
using System.Collections.Generic;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.DAL.Helper;
using System.Linq.Expressions;
using System;

namespace Quqaz.Web.Services.Interfaces
{
    public interface ITreasuryService
    {
        Task<IEnumerable<TreasuryDto>> GetAll();
        Task OrderCostChange(decimal oldCost, decimal newCost,string code);
        Task<ErrorRepsonse<TreasuryDto>> Create(CreateTreasuryDto createTreasuryDto);
        Task<TreasuryDto> GetById(int id);
        Task<PagingResualt<IEnumerable<TreasuryHistoryDto>>> GetTreasuryHistory(int id, PagingDto pagingDto);
        Task<ErrorRepsonse<TreasuryHistoryDto>> IncreaseAmount(int id, CreateCashMovmentDto createCashMovment);
        Task<ErrorRepsonse<TreasuryHistoryDto>> DecreaseAmount(int id, CreateCashMovmentDto createCashMovment);
        Task IncreaseAmountByOrderFromAgent(ReceiptOfTheOrderStatus receiptOfTheOrderStatus);
        Task DisActive(int id);
        Task Active(int id);
        Task<bool> Any(Expression<Func<Treasury,bool>> expression);
        Task<PagingResualt<IEnumerable<CashMovmentDto>>> GetCashMovment(PagingDto paging, int? treasueryId);
        Task<CashMovmentDto> GetCashMovmentById(int id);


    }
}
