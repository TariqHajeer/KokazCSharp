﻿using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Statics;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using KokazGoodsTransfer.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using LinqKit;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.ClientDtos;
using System.Linq.Expressions;
using System;

namespace KokazGoodsTransfer.Services.Concret
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
                TotalOrderInSotre = await _orderRepository.Count(c => c.OrderPlace == OrderPlace.Store),
                TotlaOrder = await _orderRepository.Count(),
                TotalOrderInWay = await _orderRepository.Count(c => c.OrderPlace == OrderPlace.Way),
                TotalOrderCountInProcess = await _orderRepository.Count(c => c.OrderStateId == (int)OrderStateEnum.Processing),
                TotalMoneyInComapny = 0
            };
            var totalEariningIncome = await _incomeRepository.Sum(c => c.Earining);
            var totalOutCome = await _outComeRepository.Sum(c => c.Amount);
            var orderInNigative = await _orderRepository.Sum(c => c.Cost - c.DeliveryCost, c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash || (c.OrderStateId != (int)OrderStateEnum.Finished && c.IsClientDiliverdMoney == true));
            orderInNigative *= -1;
            var orderInPositve = await _orderRepository.Sum(c => c.Cost - c.AgentCost, c => c.IsClientDiliverdMoney == false && c.OrderPlace >= OrderPlace.Delivered && c.OrderPlace < OrderPlace.Delayed && c.MoneyPlace != MoneyPalced.WithAgent);


            var totalAccount = await _receiptRepository.Sum(c => c.Amount, c => c.ClientPaymentId == null);

            var sumClientMone = totalAccount + orderInNigative + orderInPositve;

            var totalOrderEarinig = await _orderRepository.Sum(c => c.DeliveryCost - c.AgentCost, c => c.OrderStateId == (int)OrderStateEnum.Finished && (c.MoneyPlace != MoneyPalced.WithAgent && c.MoneyPlace != MoneyPalced.OutSideCompany) && (c.OrderPlace > OrderPlace.Way));

            mainStatics.TotalMoneyInComapny += totalEariningIncome;
            mainStatics.TotalMoneyInComapny -= totalOutCome;
            mainStatics.TotalMoneyInComapny += sumClientMone;
            mainStatics.TotalMoneyInComapny += totalOrderEarinig;
            return mainStatics;
        }

        public async Task<IEnumerable<UserDto>> AgnetStatics()
        {
            var agent = await _userRepository.Select(c => c.CanWorkAsAgent == true, c => new User() { Id = c.Id, Name = c.Name });
            var ordersInStore = await _orderRepository.CountGroupBy(c => c.AgentId, c => c.OrderPlace == OrderPlace.Store);
            var ordersInWay = await _orderRepository.CountGroupBy(c => c.AgentId, c => c.OrderPlace == OrderPlace.Way);
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
            orderPredicate = orderPredicate.And(c => c.OrderStateId == (int)OrderStateEnum.Finished && c.OrderPlace != OrderPlace.CompletelyReturned);
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

            var clients = await _clientRepository.Select(c => new { c.Id, c.Name });
            var totalAccount = await _receiptRepository.Sum(c => c.ClientId, h => h.Amount, c => c.ClientPaymentId != null);

            var paidOrders = await _orderRepository.Sum(c => c.ClientId, s => s.Cost - s.DeliveryCost - (s.ClientPaied ?? 0), c => c.IsClientDiliverdMoney && c.MoneyPlace != MoneyPalced.Delivered);
            var nonPaidOrders = await _orderRepository.Sum(c => c.ClientId, s => s.Cost - s.DeliveryCost, c => !c.IsClientDiliverdMoney && c.MoneyPlace == MoneyPalced.InsideCompany);

            List<ClientBlanaceDto> clientBlanaceDtos = new List<ClientBlanaceDto>();
            foreach (var item in clients)
            {
                var recipeTital = totalAccount.ContainsKey(item.Id) ? totalAccount[item.Id] : 0;
                var paidOrder = paidOrders.ContainsKey(item.Id) ? paidOrders[item.Id] : 0;
                var nonPaidOrder = nonPaidOrders.ContainsKey(item.Id) ? nonPaidOrders[item.Id] : 0;
                clientBlanaceDtos.Add(new ClientBlanaceDto()
                {
                    ClientName = item.Name,
                    Amount = recipeTital + paidOrder + nonPaidOrder
                });
            }
            return clientBlanaceDtos;
        }
        public async Task<StaticsDto> GetClientStatistic()
        {
            var pr = PredicateBuilder.New<Order>(c => c.ClientId == _httpContextAccessorService.AuthoticateUserId());
            async Task<int> Opr(Expression<Func<Order, bool>> filter = null)
            {
                if (filter != null)
                    return await _orderRepository.Count(pr.And(filter));
                return await _orderRepository.Count(pr);
            }
            var pric = PredicateBuilder.New<Order>(c => c.MoneyPlace == MoneyPalced.InsideCompany);
            var staticsDto = new StaticsDto()
            {
                TotalOrder = await Opr(),
                OrderDeliverdToClient = await Opr(c => c.MoneyPlace == MoneyPalced.WithAgent && (c.OrderPlace == OrderPlace.PartialReturned || c.OrderPlace == OrderPlace.Delivered)),
                OrderMoneyDelived = await Opr(c => c.IsClientDiliverdMoney == true && (c.OrderPlace == OrderPlace.Way || c.OrderPlace == OrderPlace.Delivered)),
                OrderInWat = await Opr(c => c.OrderPlace == OrderPlace.Way && c.IsClientDiliverdMoney != true),
                OrderInStore = await Opr(c => c.OrderPlace == OrderPlace.Store),
                OrderWithClient = await Opr(c => c.OrderPlace == OrderPlace.Client),
                OrderComplitlyReutrndDeliverd = await Opr(c => (c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.Unacceptable) && c.MoneyPlace == MoneyPalced.Delivered),
                OrderPartialReturned = await Opr(c => c.OrderPlace == OrderPlace.PartialReturned),
                DelayedOrder = await Opr(c => c.OrderPlace == OrderPlace.Delayed),
                OrderMoneyInCompany = await Opr(pric.And(c => c.OrderPlace == OrderPlace.Delivered || c.OrderPlace == OrderPlace.PartialReturned || c.OrderStateId == (int)OrderStateEnum.ShortageOfCash)),
                OrderComplitlyReutrndInCompany = await Opr(pric.And(c => c.OrderPlace == OrderPlace.CompletelyReturned || c.OrderPlace == OrderPlace.Unacceptable))

            };
            return staticsDto;
        }
    }
}
