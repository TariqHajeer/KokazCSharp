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
        public StatisticsController(KokazContext context, IMapper mapper, Logging logging, NotificationHub notificationHub) : base(context, mapper, logging)
        {
            this._notificationHub = notificationHub;
        }

        [HttpGet("MainStatics")]
        public async Task<IActionResult> MainStatics()
        {
            MainStaticsDto mainStatics = new MainStaticsDto()
            {
                TotalAgent = await this.Context.Users.Where(c => c.CanWorkAsAgent == true).CountAsync(),
                TotalClient = await this.Context.Clients.CountAsync(),
                TotalOrderInSotre = await this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store).CountAsync(),
                TotlaOrder = await this.Context.Orders.CountAsync(),
                TotalOrderInWay = await this.Context.Orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Way).CountAsync(),
                TotalOrderCountInProcess = await this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Processing).CountAsync(),
                TotalMoneyInComapny = 0
            };
            var totalEariningIncome = await this.Context.Incomes.SumAsync(c => c.Earining);
            var totalOutCome = await this.Context.OutComes.SumAsync(c => c.Amount);

            var clientOrder = this.Context.Orders;
            var orderInNigative = await (clientOrder.Where(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash || (c.OrderStateId != (int)OrderStateEnum.Finished && c.IsClientDiliverdMoney == true)).SumAsync(c => c.Cost - c.DeliveryCost)) * -1;
            var orderInPositve = await (clientOrder.Where(c => c.IsClientDiliverdMoney == false && c.OrderplacedId >= (int)OrderplacedEnum.Delivered && c.OrderplacedId < (int)OrderplacedEnum.Delayed && c.MoenyPlacedId != (int)MoneyPalcedEnum.WithAgent).SumAsync(c => c.Cost - c.AgentCost));

            var totalAccount = await this.Context.Receipts.Where(c => c.PrintId == null).SumAsync(c => c.Amount);

            var sumClientMone = totalAccount + orderInNigative + orderInPositve;

            var totalOrderEarinig = await this.Context.Orders.Where(c => c.OrderStateId == (int)OrderStateEnum.Finished && (c.MoenyPlacedId != (int)MoneyPalcedEnum.WithAgent && c.MoenyPlacedId != (int)MoneyPalcedEnum.OutSideCompany) && (c.OrderplacedId > (int)OrderplacedEnum.Way)).SumAsync(c => c.DeliveryCost - c.AgentCost);

            mainStatics.TotalMoneyInComapny += totalEariningIncome;
            mainStatics.TotalMoneyInComapny -= totalOutCome;
            mainStatics.TotalMoneyInComapny += sumClientMone;
            mainStatics.TotalMoneyInComapny += totalOrderEarinig;


            return Ok(mainStatics);
        }
        [HttpGet("Notification")]
        public async Task<IActionResult> GetAdminNotification()
        {
            var newOrdersCount = await this.Context.Orders
                .Where(c => c.IsSend == true && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .CountAsync();
            var newOrdersDontSendCount = await this.Context.Orders
                .Where(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client)
                .CountAsync();
            var orderRequestEditStateCount = await this.Context.ApproveAgentEditOrderRequests.Where(c => c.IsApprove == null).CountAsync();
            var newEditRquests = await this.Context.EditRequests.Where(c => c.Accept == null).CountAsync();
            var newPaymentRequetsCount = await this.Context.PaymentRequests
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

            var ShipmentTotal = this.Context.Orders.AsQueryable();
            var TotalIncome = this.Context.Incomes.AsQueryable();
            var TotalOutCome = this.Context.OutComes.AsQueryable();
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
                ShipmentTotal =await ShipmentTotal.SumAsync(c => c.DeliveryCost - c.AgentCost),
                TotalIncome =await TotalIncome.SumAsync(c => c.Earining),
                TotalOutCome =await TotalOutCome.SumAsync(c => c.Amount)
            };
            return Ok(aggregateDto);
        }
        [HttpGet("AgnetStatics")]
        public async  Task<IActionResult> AgnetStatics()
        {
            var agent =await this.Context.Users.Where(c => c.CanWorkAsAgent == true).ToListAsync();
            List<UserDto> userDtos = new List<UserDto>();
            foreach (var item in agent)
            {
                var user = mapper.Map<UserDto>(item);
                user.UserStatics = new UserStatics
                {
                    OrderInStore =await this.Context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Store).CountAsync(),
                    OrderInWay = await this.Context.Orders.Where(c => c.AgentId == item.Id && c.OrderplacedId == (int)OrderplacedEnum.Way).CountAsync()
                };
                userDtos.Add(user);
            }
            return Ok(userDtos);
        }

        [HttpGet("ClientBalance")]
        public async Task<IActionResult> ClientBalance()
        {
            var clients = await this.Context.Clients.ToListAsync();
            List<ClientBlanaceDto> clientBlanaceDtos = new List<ClientBlanaceDto>();
            foreach (var item in clients)
            {
                clientBlanaceDtos.Add(await GetClientBalanceById(item));
            }
            return Ok(clientBlanaceDtos);

        }
        private async Task<ClientBlanaceDto> GetClientBalanceById(Client item)
        {
            var totalAccountTask = await this.Context.Receipts.Where(c => c.ClientId == item.Id && c.PrintId == null).SumAsync(c => c.Amount);
            var clientOrder = await this.Context.Orders.Where(c => c.ClientId == item.Id).ToListAsync();
            var totalOrder = clientOrder.Sum(c => c.CalcClientBalanc());
            var totalAccount = totalAccountTask;
            return new ClientBlanaceDto()
            {
                ClientName = item.Name,
                Amount = totalAccount + totalOrder
            };
        }

    }
}