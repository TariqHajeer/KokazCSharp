using Quqaz.Web.DAL.Helper;
using Quqaz.Web.Dtos.AgentDtos;
using Quqaz.Web.Dtos.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using Quqaz.Web.Dtos.OrdersDtos;
using System.Linq.Expressions;
using System;
using Quqaz.Web.Models;

namespace Quqaz.Web.Services.Interfaces
{
    public interface IAgentPrintService
    {
        Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetPrint(PagingDto pagingDto, PrintFilterDto printFilterDto);
        Task<PrintOrdersDto> GetPrintById(int printNumber);
        Task<int> Count(Expression<Func<AgentPrint, bool>> filter = null);
        Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetAgentPrint(PagingDto pagingDto, int? number, string agnetName,string code);
        Task<PrintOrdersDto> GetOrderByAgnetPrintNumber(int printNumber);
        Task SetOrderState(List<AgentOrderStateDto> agentOrderStateDtos);


    }
}
