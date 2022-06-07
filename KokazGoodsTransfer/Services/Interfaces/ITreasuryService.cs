using KokazGoodsTransfer.Models;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.TreasuryDtos;
using KokazGoodsTransfer.Services.Helper;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.DAL.Helper;
using System.Linq.Expressions;
using System;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface ITreasuryService
    {
        Task<IEnumerable<TreasuryDto>> GetAll();
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
