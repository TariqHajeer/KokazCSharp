using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Dtos.Statics;
using KokazGoodsTransfer.Dtos.Users;
using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace KokazGoodsTransfer.Controllers.EmployeePolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : AbstractEmployeePolicyController
    {
        private readonly NotificationHub _notificationHub;
        public StatisticsController(KokazContext context, IMapper mapper, NotificationHub notificationHub) : base(context, mapper)
        {
            this._notificationHub = notificationHub;
        }

        [HttpGet("MainStatics")]
        public async Task<IActionResult> MainStatics()
        {
            MainStaticsDto mainStatics = new MainStaticsDto()
            {
                TotalAgent = await this._context.Users.Where(c => c.CanWorkAsAgent == true).CountAsync(),
                TotalClient = await this._context.Clients.CountAsync(),
                TotalOrderInSotre = await this._context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store).CountAsync(),
                TotlaOrder = await this._context.Orders.CountAsync(),
                TotalOrderInWay = await this._context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way).CountAsync(),
                TotalOrderCountInProcess = await this._context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Processing).CountAsync(),
                TotalMoneyInComapny = 0
            };
            var totalEariningIncome = await this._context.Incomes.SumAsync(c => c.Earining);
            var totalOutCome = await this._context.OutComes.SumAsync(c => c.Amount);

            var clientOrder = this._context.Orders;
            var orderInNigative = await (clientOrder.Where(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash || (c.OrderStateId != (int)OrderStateEnum.Finished && c.IsClientDiliverdMoney == true)).SumAsync(c => c.Cost - c.DeliveryCost)) * -1;
            var orderInPositve = await (clientOrder.Where(c => c.IsClientDiliverdMoney == false && c.OrderplacedId >= (int)OrderplacedEnum.Delivered && c.OrderplacedId < (int)OrderplacedEnum.Delayed && c.MoenyPlacedId != (int)MoneyPalcedEnum.WithAgent).SumAsync(c => c.Cost - c.AgentCost));

            var totalAccount = await this._context.Receipts.Where(c => c.ClientPaymentId == null).SumAsync(c => c.Amount);

            var sumClientMone = totalAccount + orderInNigative + orderInPositve;

            var totalOrderEarinig = await this._context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Finished && (c.MoenyPlacedId != (int)MoneyPalcedEnum.WithAgent && c.MoenyPlacedId != (int)MoneyPalcedEnum.OutSideCompany) && (c.OrderplacedId > (int)OrderplacedEnum.Way)).SumAsync(c => c.DeliveryCost - c.AgentCost);

            mainStatics.TotalMoneyInComapny += totalEariningIncome;
            mainStatics.TotalMoneyInComapny -= totalOutCome;
            mainStatics.TotalMoneyInComapny += sumClientMone;
            mainStatics.TotalMoneyInComapny += totalOrderEarinig;


            return Ok(mainStatics);
        }
        [HttpGet("Notification")]
        public async Task<IActionResult> GetAdminNotification()
        {
            var newOrdersCount = await this._context.Orders
                .Where(c => c.IsSend == true && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .CountAsync();
            var newOrdersDontSendCount = await this._context.Orders
                .Where(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .CountAsync();
            var orderRequestEditStateCount = await this._context.ApproveAgentEditOrderRequests.Where(c => c.IsApprove == null).CountAsync();
            var newEditRquests = await this._context.EditRequests.Where(c => c.Accept == null).CountAsync();
            var newPaymentRequetsCount = await this._context.PaymentRequests
                .Where(c => c.Accept == null).CountAsync();
            AdminNotification adminNotification = new AdminNotification()
            {
                NewOrdersCount = newOrdersCount,
                NewOrdersDontSendCount = newOrdersDontSendCount,
                OrderRequestEditStateCount = orderRequestEditStateCount,
                NewEditRquests = newEditRquests,
                NewPaymentRequetsCount = newPaymentRequetsCount
            };
            return Ok(adminNotification);
        }
        [HttpGet("GetAggregate")]
        public async Task<IActionResult> GetAggregate([FromQuery] DateFiter dateFiter)
        {

            var ShipmentTotal = this._context.Orders.AsQueryable();
            var TotalIncome = this._context.Incomes.AsQueryable();
            var TotalOutCome = this._context.OutComes.AsQueryable();
            if (dateFiter.FromDate != null)
            {
                ShipmentTotal = ShipmentTotal.Where(c => c.Date >= dateFiter.FromDate);
                TotalIncome = TotalIncome.Where(c => c.Date >= dateFiter.FromDate);
                TotalOutCome = TotalOutCome.Where(c => c.Date >= dateFiter.FromDate);
            }
            if (dateFiter.ToDate != null)
            {
                ShipmentTotal = ShipmentTotal.Where(c => c.Date <= dateFiter.ToDate);
                TotalIncome = TotalIncome.Where(c => c.Date <= dateFiter.ToDate);
                TotalOutCome = TotalOutCome.Where(c => c.Date <= dateFiter.ToDate);
            }
            ShipmentTotal = ShipmentTotal.Where(c => c.OrderStateId == (int)OrderStateEnum.Finished && c.OrderplacedId != (int)OrderplacedEnum.CompletelyReturned);
            AggregateDto aggregateDto = new AggregateDto()
            {
                ShipmentTotal = await ShipmentTotal.SumAsync(c => c.DeliveryCost - c.AgentCost),
                TotalIncome = await TotalIncome.SumAsync(c => c.Earining),
                TotalOutCome = await TotalOutCome.SumAsync(c => c.Amount)
            };
            return Ok(aggregateDto);
        }
        [HttpGet("AgnetStatics")]
        public async Task<IActionResult> AgnetStatics()
        {
            var agent = await this._context.Users.Where(c => c.CanWorkAsAgent == true).ToListAsync();
            List<UserDto> userDtos = new List<UserDto>();
            foreach (var item in agent)
            {
                var user = _mapper.Map<UserDto>(item);
                user.UserStatics = new UserStatics
                {
                    OrderInStore = await this._context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Store).CountAsync(),
                    OrderInWay = await this._context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Way).CountAsync()
                };
                userDtos.Add(user);
            }
            return Ok(userDtos);
        }

        [HttpGet("ClientBalance")]
        public async Task<IActionResult> ClientBalance()
        {
            var clients = await this._context.Clients.Select(c => new { c.Id, c.Name }).ToListAsync();
            var totalAccount = await this._context.Receipts.Where(c => c.ClientPaymentId == null).GroupBy(c => c.ClientId).Select(c => new { Id = c.Key, Sum = c.Sum(s => s.Amount) }).ToListAsync();

            var paidOrders = await this._context.Orders.Where(c => (c.IsClientDiliverdMoney && c.MoenyPlacedId != (int)MoneyPalcedEnum.Delivered)).GroupBy(c => c.ClientId).Select(c => new { c.Key, Sum = c.Sum(s => (s.ClientPaied ?? 0) * -1) }).ToListAsync();
            var nonPaidOrders = await this._context.Orders.Where(c => !c.IsClientDiliverdMoney && c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany).GroupBy(c => c.ClientId).Select(c => new { c.Key, Sum = c.Sum(s => s.Cost - s.DeliveryCost) }).ToListAsync();

            List<ClientBlanaceDto> clientBlanaceDtos = new List<ClientBlanaceDto>();
            foreach (var item in clients)
            {
                var recipeTital = totalAccount.FirstOrDefault(c => c.Id == item.Id)?.Sum ?? 0;
                var paidOrder = paidOrders.FirstOrDefault(c => c.Key == item.Id)?.Sum ?? 0;
                var nonPaidOrder = nonPaidOrders.FirstOrDefault(c => c.Key == item.Id)?.Sum ?? 0;
                clientBlanaceDtos.Add(new ClientBlanaceDto()
                {
                    ClientName = item.Name,
                    Amount = recipeTital + paidOrder + nonPaidOrder
                });
            }
            return Ok(clientBlanaceDtos);

        }

    }
}