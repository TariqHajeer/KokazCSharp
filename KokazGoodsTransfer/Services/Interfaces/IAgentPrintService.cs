using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.Dtos.AgentDtos;
using KokazGoodsTransfer.Dtos.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using System.Linq.Expressions;
using System;
using KokazGoodsTransfer.Models;

namespace KokazGoodsTransfer.Services.Interfaces
{
    public interface IAgentPrintService
    {
        Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetPrint(PagingDto pagingDto, PrintFilterDto printFilterDto);
        Task<PrintOrdersDto> GetPrintById(int printNumber);
        Task<int> Count(Expression<Func<AgentPrint, bool>> filter = null);
        Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetAgentPrint(PagingDto pagingDto, int? number, string agnetName);
        Task<PrintOrdersDto> GetOrderByAgnetPrintNumber(int printNumber);
        Task SetOrderState(List<AgentOrderStateDto> agentOrderStateDtos);


    }
}
