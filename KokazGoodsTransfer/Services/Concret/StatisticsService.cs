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
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.Clients;

namespace KokazGoodsTransfer.Services.Concret
{
    public class StatisticsService : IStatisticsService
    {
        IRepository<User> _userRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRepository<Income> _incomeRepository;
        private readonly IRepository<OutCome> _outComeRepository;
        private readonly IRepository<Receipt> _receiptRepository;
        private readonly IRepository<EditRequest> _editRequestRepository;
        private readonly IRepository<PaymentRequest> _paymentRequestRepository;
        private readonly IMapper _mapper;


        public StatisticsService(IRepository<User> userRepository, IRepository<Client> clientRepository, IOrderRepository orderRepository, IRepository<Income> incomeRepository, IRepository<OutCome> outComeRepository, IRepository<Receipt> receiptRepository, IMapper mapper, IRepository<EditRequest> editRequestRepository, IRepository<PaymentRequest> paymentRequestRepository)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _orderRepository = orderRepository;
            _incomeRepository = incomeRepository;
            _outComeRepository = outComeRepository;
            _receiptRepository = receiptRepository;
            _mapper = mapper;
            _editRequestRepository = editRequestRepository;
            _paymentRequestRepository = paymentRequestRepository;
        }
        public async Task<MainStaticsDto> GetMainStatics()
        {
            MainStaticsDto mainStatics = new MainStaticsDto()
            {
                TotalAgent = await _userRepository.Count(c => c.CanWorkAsAgent == true),
                TotalClient = await _clientRepository.Count(),
                TotalOrderInSotre = await _orderRepository.Count(c => c.OrderplacedId == (int)OrderplacedEnum.Store),
                TotlaOrder = await _orderRepository.Count(),
                TotalOrderInWay = await _orderRepository.Count(c => c.OrderplacedId == (int)OrderplacedEnum.Way),
                TotalOrderCountInProcess = await _orderRepository.Count(c => c.OrderStateId == (int)OrderStateEnum.Processing),
                TotalMoneyInComapny = 0
            };
            var totalEariningIncome = await _incomeRepository.Sum(c => c.Earining);
            var totalOutCome = await _outComeRepository.Sum(c => c.Amount);
            var orderInNigative = await _orderRepository.Sum(c => c.Cost - c.DeliveryCost, c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash || (c.OrderStateId != (int)OrderStateEnum.Finished && c.IsClientDiliverdMoney == true));
            orderInNigative *= -1;
            var orderInPositve = await _orderRepository.Sum(c => c.Cost - c.AgentCost, c => c.IsClientDiliverdMoney == false && c.OrderplacedId >= (int)OrderplacedEnum.Delivered && c.OrderplacedId < (int)OrderplacedEnum.Delayed && c.MoenyPlacedId != (int)MoneyPalcedEnum.WithAgent);


            var totalAccount = await _receiptRepository.Sum(c => c.Amount, c => c.ClientPaymentId == null);

            var sumClientMone = totalAccount + orderInNigative + orderInPositve;

            var totalOrderEarinig = await _orderRepository.Sum(c => c.DeliveryCost - c.AgentCost, c => c.OrderStateId == (int)OrderStateEnum.Finished && (c.MoenyPlacedId != (int)MoneyPalcedEnum.WithAgent && c.MoenyPlacedId != (int)MoneyPalcedEnum.OutSideCompany) && (c.OrderplacedId > (int)OrderplacedEnum.Way));

            mainStatics.TotalMoneyInComapny += totalEariningIncome;
            mainStatics.TotalMoneyInComapny -= totalOutCome;
            mainStatics.TotalMoneyInComapny += sumClientMone;
            mainStatics.TotalMoneyInComapny += totalOrderEarinig;
            return mainStatics;
        }

        public async Task<IEnumerable<UserDto>> AgnetStatics()
        {
            //var agent = await this._context.Users.Where(c => c.CanWorkAsAgent == true).ToListAsync();
            //List<UserDto> userDtos = new List<UserDto>();
            //foreach (var item in agent)
            //{
            //    var user = _mapper.Map<UserDto>(item);
            //    user.UserStatics = new UserStatics
            //    {
            //        OrderInStore = await this._context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Store).CountAsync(),
            //        OrderInWay = await this._context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Way).CountAsync()
            //    };
            //    userDtos.Add(user);
            //}
            //return Ok(userDtos);
            var agent = await _userRepository.Select(c => c.CanWorkAsAgent == true, c => new User() { Id = c.Id, Name = c.Name });
            var ordersInStore = await _orderRepository.CountGroupBy(c => c.AgentId, c => c.OrderplacedId == (int)OrderplacedEnum.Store);
            var ordersInWay = await _orderRepository.CountGroupBy(c => c.AgentId, c => c.OrderplacedId == (int)OrderplacedEnum.Way);
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
            orderPredicate = orderPredicate.And(c => c.OrderStateId == (int)OrderStateEnum.Finished && c.OrderplacedId != (int)OrderplacedEnum.CompletelyReturned);
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

        public async Task<AdminNotification> GetAdminNotification()
        {
            var newOrdersCount = await _orderRepository.Count(c => c.IsSend == true && c.OrderplacedId == (int)OrderplacedEnum.Client);
            var newOrdersDontSendCount = await _orderRepository.Count(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client);
            var orderRequestEditStateCount = await _orderRepository.Count(c => c.AgentRequestStatus == (int)AgentRequestStatusEnum.Pending);
            var newEditRquests = await _editRequestRepository.Count(c => c.Accept == null);
            var newPaymentRequetsCount = await _paymentRequestRepository.Count(c => c.Accept == null);
            var adminNotification = new AdminNotification()
            {
                NewOrdersCount = newOrdersCount,
                NewOrdersDontSendCount = newOrdersDontSendCount,
                OrderRequestEditStateCount = orderRequestEditStateCount,
                NewEditRquests = newEditRquests,
                NewPaymentRequetsCount = newPaymentRequetsCount
            };
            return adminNotification;
        }

        public async Task<IEnumerable<ClientBlanaceDto>> GetClientBalance()
        {
            var clients = await _clientRepository.Select(c => new { c.Id, c.Name });
            var totalAccount = await _receiptRepository.Sum(c => c.ClientId, c => c.Amount, c => c.ClientPaymentId != null);

            var paidOrders = await _orderRepository.Sum(c => c.ClientId, s => (s.ClientPaied ?? 0) * -1, c => c.IsClientDiliverdMoney && c.MoenyPlacedId != (int)MoneyPalcedEnum.Delivered);
            var nonPaidOrders = await _orderRepository.Sum(c => c.ClientId, s => s.Cost - s.DeliveryCost, c => !c.IsClientDiliverdMoney && c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany);

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
    }
}
