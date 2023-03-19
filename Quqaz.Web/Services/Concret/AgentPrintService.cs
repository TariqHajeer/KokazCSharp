using AutoMapper;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.AgentDtos;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinqKit;
using Quqaz.Web.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq.Expressions;
using Quqaz.Web.Models.Static;
using Quqaz.Web.Dtos.NotifcationDtos;
using Quqaz.Web.HubsConfig;

namespace Quqaz.Web.Services.Concret
{
    public class AgentPrintService : IAgentPrintService
    {
        private readonly IRepository<AgentPrint> _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Order> _orderRepository;
        private readonly NotificationHub _notificationHub;
        protected int AuthoticateUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.Claims.ToList().Where(c => c.Type == "UserID").Single();
            return Convert.ToInt32(userIdClaim.Value);
        }
        public AgentPrintService(IRepository<AgentPrint> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IRepository<Order> orderRepository, NotificationHub notificationHub)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _orderRepository = orderRepository;
            _notificationHub = notificationHub;
        }

        public async Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetPrint(PagingDto pagingDto, PrintFilterDto printFilterDto)
        {
            var predicte = PredicateBuilder.New<AgentPrint>(true);
            if (printFilterDto.Date != null)
            {
                predicte = predicte.And(c => c.Date == printFilterDto.Date);
            }
            if (printFilterDto.Number != null)
            {
                predicte = predicte.And(c => c.Id == printFilterDto.Number);
            }
            predicte = predicte.And(c => c.AgentOrderPrints.Any(c => c.Order.AgentId == AuthoticateUserId()));
            var result = await _repository.GetAsync(pagingDto, predicte);
            return new PagingResualt<IEnumerable<PrintOrdersDto>>()
            {
                Data = _mapper.Map<IEnumerable<PrintOrdersDto>>(result.Data),
                Total = result.Total
            };
        }

        public async Task<PrintOrdersDto> GetPrintById(int printNumber)
        {
            var printed = await _repository.FirstOrDefualt(c => c.Id == printNumber, c => c.AgentPrintDetails);
            return _mapper.Map<PrintOrdersDto>(printed);
        }

        public async Task<int> Count(Expression<Func<AgentPrint, bool>> filter = null)
        {
            return await _repository.Count(filter);
        }

        public async Task<PagingResualt<IEnumerable<PrintOrdersDto>>> GetAgentPrint(PagingDto pagingDto, int? number, string agnetName)
        {
            var predicte = PredicateBuilder.New<AgentPrint>(true);
            if (number != null)
            {
                predicte = predicte.And(c => c.Id == number);
            }
            if (!String.IsNullOrWhiteSpace(agnetName))
            {
                predicte = predicte.And(c => c.DestinationName == agnetName);
            }
            var data = await _repository.GetAsync(pagingDto, predicte, null, c => c.OrderByDescending(x => x.Id)); ;
            return new PagingResualt<IEnumerable<PrintOrdersDto>>()
            {
                Total = data.Total,
                Data = _mapper.Map<IEnumerable<PrintOrdersDto>>(data.Data)
            };
        }

        public async Task<PrintOrdersDto> GetOrderByAgnetPrintNumber(int printNumber)
        {
            var printed = await _repository.FirstOrDefualt(c => c.Id == printNumber, c => c.AgentPrintDetails);
            return _mapper.Map<PrintOrdersDto>(printed);
        }

        public async Task SetOrderState(List<AgentOrderStateDto> agentOrderStateDtos)
        {
            var orders = await _orderRepository.GetAsync(c => agentOrderStateDtos.Select(c => c.Id).ToList().Contains(c.Id));
            agentOrderStateDtos.ForEach(aos =>
            {
                var order = orders.First(c => c.Id == aos.Id);
                order.NewCost = aos.Cost;
                order.NewOrderPlace = (OrderPlace)aos.OrderplacedId;
                order.AgentRequestStatus = (int)AgentRequestStatusEnum.Pending;
            });
            await _orderRepository.Update(orders);
            var count = await _orderRepository.Count(c => c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending);
            AdminNotification adminNotification = new AdminNotification()
            {

                OrderRequestEditStateCount = count,

            };
            await _notificationHub.AdminNotifcation(adminNotification);
        }
    }
}
