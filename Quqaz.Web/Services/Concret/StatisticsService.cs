using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Statics;
using Quqaz.Web.Dtos.Users;
using Quqaz.Web.Models;
using Quqaz.Web.Models.Static;
using Quqaz.Web.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Quqaz.Web.Dtos.OrdersDtos;
using LinqKit;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.ClientDtos;
using System.Linq.Expressions;
using System;
using Quqaz.Web.Migrations;

namespace Quqaz.Web.Services.Concret
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<Income> _incomeRepository;
        private readonly IRepository<OutCome> _outComeRepository;
        private readonly IRepository<Receipt> _receiptRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessorService _httpContextAccessorService;


        public StatisticsService(IRepository<User> userRepository, IRepository<Client> clientRepository, IOrderRepository orderRepository, IRepository<Income> incomeRepository, IRepository<OutCome> outComeRepository, IRepository<Receipt> receiptRepository, IMapper mapper, IHttpContextAccessorService httpContextAccessorService)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _orderRepository = orderRepository;
            _incomeRepository = incomeRepository;
            _outComeRepository = outComeRepository;
            _receiptRepository = receiptRepository;
            _mapper = mapper;
            _httpContextAccessorService = httpContextAccessorService;
        }
        public async Task<MainStaticsDto> GetMainStatics()
        {
            MainStaticsDto mainStatics = new MainStaticsDto()
            {
                TotalAgent = await _userRepository.Count(c => c.CanWorkAsAgent == true),
                TotalClient = await _clientRepository.Count(),
                TotalOrderInSotre = await _orderRepository.Count(c => c.OrderPlace == OrderPlace.Store && c.CurrentBranchId == _httpContextAccessorService.CurrentBranchId()),
                TotlaOrder = await _orderRepository.Count(),
                TotalOrderInWay = await _orderRepository.Count(c => c.OrderPlace == OrderPlace.Way),
                TotalOrderCountInProcess = await _orderRepository.Count(c => c.OrderState == OrderState.Processing),
                TotalMoneyInComapny = 0
            };
            var orderInWayAndDelived = await _orderRepository.Sum(c => c.Cost - c.DeliveryCost, c => c.BranchId == _httpContextAccessorService.CurrentBranchId() && c.OrderState == OrderState.Processing && c.IsClientDiliverdMoney == false && c.MoneyPlace == MoneyPalce.InsideCompany);
            var totalAccount = await _receiptRepository.Sum(c => c.Amount, c => c.ClientPaymentId == null);
            mainStatics.TotalMoneyInComapny = orderInWayAndDelived + totalAccount;
            return mainStatics;
        }

        public async Task<IEnumerable<UserDto>> AgnetStatics()
        {
            var agent = await _userRepository.Select(c => c.CanWorkAsAgent == true, c => new User() { Id = c.Id, Name = c.Name });
            var ordersInStore = await _orderRepository.CountGroupBy(c => c.AgentId, c => c.OrderPlace == OrderPlace.Store);
            var ordersInWay = await _orderRepository.CountGroupBy(c => c.AgentId, c => c.AgentId != null && c.OrderPlace == OrderPlace.Way);
            var userDtos = new List<UserDto>();
            foreach (var item in agent)
            {
                var user = _mapper.Map<UserDto>(item);
                user.UserStatics = new UserStatics()
                {
                    OrderInStore = ordersInStore.ContainsKey(user.Id) ? ordersInStore[user.Id] : 0,
                    OrderInWay = ordersInWay.ContainsKey(user.Id) ? ordersInWay[user.Id] : 0,
                };
                userDtos.Add(user);
            }
            return userDtos;
        }

        public async Task<AggregateDto> GetAggregate(DateFiter dateFiter)
        {
            var incomePredicate = PredicateBuilder.New<Income>(true);
            var outComePredicate = PredicateBuilder.New<OutCome>(true);
            var orderPredicate = PredicateBuilder.New<Order>(true);
            orderPredicate = orderPredicate.And(c => c.OrderState == OrderState.Finished && c.OrderPlace != OrderPlace.CompletelyReturned);
            if (dateFiter.FromDate != null)
            {
                incomePredicate = incomePredicate.And(c => c.Date >= dateFiter.FromDate);
                outComePredicate = outComePredicate.And(c => c.Date >= dateFiter.FromDate);
                orderPredicate = orderPredicate.And(c => c.Date >= dateFiter.FromDate);
            }
            if (dateFiter.ToDate != null)
            {
                incomePredicate = incomePredicate.And(c => c.Date <= dateFiter.FromDate);
                outComePredicate = outComePredicate.And(c => c.Date <= dateFiter.FromDate);
                orderPredicate = orderPredicate.And(c => c.Date <= dateFiter.FromDate);
            }
            var aggregateDto = new AggregateDto()
            {
                ShipmentTotal = await _orderRepository.Sum(c => c.DeliveryCost - c.AgentCost, orderPredicate),
                TotalIncome = await _incomeRepository.Sum(c => c.Earining, incomePredicate),
                TotalOutCome = await _outComeRepository.Sum(c => c.Amount, outComePredicate)
            };
            return aggregateDto;
        }

        public async Task<IEnumerable<ClientBlanaceDto>> GetClientBalance()
        {

            var clients = await _clientRepository.Select(c => true, c => new { c.Id, c.Name });
            var totalAccount = await _receiptRepository.Sum(c => c.ClientId, h => h.Amount, c => c.ClientPaymentId == null);

            var paidOrdersWaitToRecive = await _orderRepository.Sum(c => c.ClientId, s => -(s.ClientPaied ?? 0), c => c.IsClientDiliverdMoney && c.MoneyPlace != MoneyPalce.Delivered && c.ClientPaied != null && c.OrderState != OrderState.ShortageOfCash);
            var paidOrdersHaveShortInCash = await _orderRepository.Sum(c => c.ClientId, s => (s.Cost - s.DeliveryCost) - s.ClientPaied ?? 0, c => c.IsClientDiliverdMoney && c.MoneyPlace != MoneyPalce.Delivered && c.ClientPaied != null && c.OrderState == OrderState.ShortageOfCash);
            var nonPaidOrders = await _orderRepository.Sum(c => c.ClientId, s => s.Cost - s.DeliveryCost, c => !c.IsClientDiliverdMoney && c.MoneyPlace == MoneyPalce.InsideCompany);

            List<ClientBlanaceDto> clientBlanaceDtos = new List<ClientBlanaceDto>();
            foreach (var item in clients)
            {
                var recipeTital = totalAccount.ContainsKey(item.Id) ? totalAccount[item.Id] : 0;
                var paidOrder = paidOrdersWaitToRecive.ContainsKey(item.Id) ? paidOrdersWaitToRecive[item.Id] : 0;
                var paidOrderHaveShortInCash = paidOrdersHaveShortInCash.ContainsKey(item.Id) ? paidOrdersHaveShortInCash[item.Id] : 0;
                var nonPaidOrder = nonPaidOrders.ContainsKey(item.Id) ? nonPaidOrders[item.Id] : 0;
                clientBlanaceDtos.Add(new ClientBlanaceDto()
                {
                    ClientName = item.Name,
                    Amount = recipeTital + paidOrder + nonPaidOrder + paidOrderHaveShortInCash
                });
            }
            return clientBlanaceDtos;
        }
        public async Task<StaticsDto> GetClientStatistic()
        {

            async Task<int> Opr(Expression<Func<Order, bool>> filter = null)
            {
                var pr = PredicateBuilder.New<Order>(c => c.ClientId == _httpContextAccessorService.AuthoticateUserId());
                if (filter != null)
                    return await _orderRepository.Count(pr.And(filter));
                return await _orderRepository.Count(pr);
            }
            var pric = PredicateBuilder.New<Order>();
            var staticsDto = new StaticsDto()
            {
                TotalOrder = await Opr(),
                OrderDeliverdToClient = await Opr(c => c.MoneyPlace == MoneyPalce.WithAgent && (c.OrderPlace == OrderPlace.PartialReturned || c.OrderPlace == OrderPlace.Delivered)),
                OrderMoneyDelived = await Opr(c => c.IsClientDiliverdMoney == true && (c.OrderPlace == OrderPlace.Way || c.OrderPlace == OrderPlace.Delivered)),
                OrderInWat = await Opr(c => c.OrderPlace == OrderPlace.Way && c.IsClientDiliverdMoney != true),
                OrderInStore = await Opr(c => c.OrderPlace == OrderPlace.Store),
                OrderWithClient = await Opr(c => c.OrderPlace == OrderPlace.Client),
                OrderComplitlyReutrndDeliverd = await Opr(c => (c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.Unacceptable) && c.MoneyPlace == MoneyPalce.Delivered),
                OrderPartialReturned = await Opr(c => c.OrderPlace == OrderPlace.PartialReturned),
                DelayedOrder = await Opr(c => c.OrderPlace == OrderPlace.Delayed),
                OrderMoneyInCompany = await Opr(c => c.MoneyPlace == MoneyPalce.InsideCompany && (c.OrderPlace == OrderPlace.Delivered || c.OrderPlace == OrderPlace.PartialReturned || c.OrderState == OrderState.ShortageOfCash)),
                OrderComplitlyReutrndInCompany = await Opr(pric.And(c => c.MoneyPlace == MoneyPalce.InsideCompany && (c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.Unacceptable)))

            };
            return staticsDto;
        }
    }
}
