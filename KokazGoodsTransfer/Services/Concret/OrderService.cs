using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using System.Linq;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models.Static;
using System;

namespace KokazGoodsTransfer.Services.Concret
{
    public class OrderService : IOrderService
    {
        private readonly IUintOfWork _uintOfWork;
        private readonly INotificationService _notificationService;
        private static readonly Func<Order, bool> _finishOrderExpression = c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable
|| (c.OrderplacedId == (int)OrderplacedEnum.Delivered && (c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany || c.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered));
        public OrderService(IUintOfWork uintOfWork, INotificationService notificationService)
        {
            _uintOfWork = uintOfWork;
            _notificationService = notificationService; 
        }

        public async Task<GenaricErrorResponse<IEnumerable<Order>, string, IEnumerable<string>>> GetOrderToReciveForDelivredOrders(string code)
        {
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => c.Code == code);
            var lastOrderAdded = orders.OrderBy(c => c.Id).Last();
            if (!orders.Any())
            {
                return new GenaricErrorResponse<IEnumerable<Order>, string, IEnumerable<string>>("الشحنة غير موجودة");
            }
            ///التأكد من ان الشحنة ليست عند العميل او في المخزن
            {
                var orderInSotre = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Store);
                var orderWithClient = orders.Where(c => c.OrderplacedId == (int)OrderplacedEnum.Client);
                orders = orders.Except(orderInSotre.Union(orderWithClient));
            }
            {
                var finishOrders = orders.Where(_finishOrderExpression);
                orders = orders.Except(finishOrders);
            }
            {
                var orderInCompany = orders.Where(c => c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany).ToList();
                orders = orders.Except(orderInCompany);
            }
            if (!orders.Any())
            {
                var lastOrderPlacedAdded = lastOrderAdded.OrderplacedId;
                if (lastOrderAdded.OrderplacedId == (int)OrderplacedEnum.Store)
                    return new GenaricErrorResponse<IEnumerable<Order>, string, IEnumerable<string>>("الشحنة في المخزن");
                if (lastOrderAdded.OrderplacedId == (int)OrderplacedEnum.Client)
                    return new GenaricErrorResponse<IEnumerable<Order>, string, IEnumerable<string>>("الشحنة عند العميل");
                if (lastOrderAdded.OrderplacedId == (int)MoneyPalcedEnum.InsideCompany)
                    return new GenaricErrorResponse<IEnumerable<Order>, string, IEnumerable<string>>("الشحنة داخل الشركة");
            }

            return new GenaricErrorResponse<IEnumerable<Order>, string, IEnumerable<string>>(orders);
        }
        public async Task<ErrorResponse<string, IEnumerable<string>>> ReceiptOfTheStatusOfTheDeliveredShipment(IEnumerable<ReceiptOfTheStatusOfTheDeliveredShipmentDto> receiptOfTheStatusOfTheDeliveredShipmentDtos)
        {
            var moneyPlacedes = await _uintOfWork.Repository<MoenyPlaced>().GetAll();
            var orderPlacedes = await _uintOfWork.Repository<OrderPlaced>().GetAll();
            var outSideCompny = moneyPlacedes.First(c => c.Id == (int)MoneyPalcedEnum.OutSideCompany).Name;
            var response = new ErrorResponse<string, IEnumerable<string>>();
            if (!receiptOfTheStatusOfTheDeliveredShipmentDtos.All(c => c.OrderplacedId == OrderplacedEnum.Way || c.OrderplacedId == OrderplacedEnum.Delivered || c.OrderplacedId == OrderplacedEnum.PartialReturned))
            {
                var wrongData = receiptOfTheStatusOfTheDeliveredShipmentDtos.Where(c => !(c.OrderplacedId == OrderplacedEnum.Way || c.OrderplacedId == OrderplacedEnum.Delivered || c.OrderplacedId == OrderplacedEnum.PartialReturned));
                var worngDataIds = wrongData.Select(c => c.Id);
                var worngOrders = await _uintOfWork.Repository<Order>().GetAsync(c => worngDataIds.Contains(c.Id));
                List<string> errors = new List<string>();
                foreach (var item in receiptOfTheStatusOfTheDeliveredShipmentDtos)
                {
                    string code = worngOrders.Where(c => c.Id == item.Id).FirstOrDefault()?.Code;
                    errors.Add($"لا يمكن وضع حالة الشحنة {OrderPlacedEnumToString(item.OrderplacedId)} للشحنة ذات الرقم : {code}");
                }
                response.Errors = errors;
                return response;
            }
            List<Notfication> notfications = new List<Notfication>();
            List<Notfication> addednotfications = new List<Notfication>();

            var ids = new HashSet<int>(receiptOfTheStatusOfTheDeliveredShipmentDtos.Select(c => c.Id));

            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => ids.Contains(c.Id));
            List<OrderLog> logs = new List<OrderLog>();
            foreach (var item in receiptOfTheStatusOfTheDeliveredShipmentDtos)
            {
                var order = orders.First(c => c.Id == item.Id);

                logs.Add(order);

                order.MoenyPlacedId = (int)item.MoenyPlacedId;
                order.OrderplacedId = (int)item.OrderplacedId;
                order.Note = item.Note;
                if (order.DeliveryCost != item.DeliveryCost)
                {
                    if (order.OldDeliveryCost == null)
                    {
                        order.OldDeliveryCost = order.DeliveryCost;
                    }
                }
                order.DeliveryCost = item.DeliveryCost;
                order.AgentCost = item.AgentCost;
                order.SystemNote = "ReceiptOfTheStatusOfTheDeliveredShipmentService";
                if (order.IsClientDiliverdMoney)
                {
                    switch (order.OrderplacedId)
                    {
                        case (int)OrderplacedEnum.Delivered:
                            {
                                if (decimal.Compare(order.Cost, item.Cost) != 0)
                                {
                                    if (order.OldCost == null)
                                        order.OldCost = order.Cost;
                                    order.Cost = item.Cost;
                                }
                                var payForClient = order.ShouldToPay();

                                if (decimal.Compare(payForClient, (order.ClientPaied ?? 0)) != 0)
                                {
                                    order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                                    if (order.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered)
                                    {
                                        order.MoenyPlacedId = (int)MoneyPalcedEnum.InsideCompany;
                                    }
                                }
                                else
                                {
                                    order.OrderStateId = (int)OrderStateEnum.Finished;
                                }
                            }
                            break;
                        case (int)OrderplacedEnum.PartialReturned:
                            {
                                if (order.OldCost == null)
                                    order.OldCost = order.Cost;
                                order.Cost = item.Cost;
                                order.OrderStateId = (int)OrderStateEnum.ShortageOfCash;
                            }
                            break;
                    }
                }
                else
                {
                    if (order.Cost != item.Cost)
                    {
                        if (order.OldCost == null)
                            order.OldCost = order.Cost;
                        order.Cost = item.Cost;
                    }
                }
                if (order.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany)
                {
                    order.ApproveAgentEditOrderRequests.Clear();
                }
                order.MoenyPlaced = moneyPlacedes.First(c => c.Id == order.MoenyPlacedId);
                order.Orderplaced = orderPlacedes.First(c => c.Id == order.OrderplacedId);
            }
            await _uintOfWork.BegeinTransaction();
            await _uintOfWork.UpdateRange(orders);
            await _uintOfWork.AddRange(logs);
            await _uintOfWork.UpdateRange(orders);
            await _notificationService.SendOrderReciveNotifcation(orders);
            await _uintOfWork.Commit();

            return response;
        }
        string OrderPlacedEnumToString(OrderplacedEnum orderplacedEnum)
        {
            return orderplacedEnum switch
            {
                OrderplacedEnum.Client => "عميل",
                OrderplacedEnum.Store => "مخزن",
                OrderplacedEnum.Way => "طريق",
                OrderplacedEnum.Delivered => "تم التسليم",
                OrderplacedEnum.CompletelyReturned => "مرتجع كلي",
                OrderplacedEnum.PartialReturned => "مرتجع جزئي",
                OrderplacedEnum.Unacceptable => "مرفوض",
                OrderplacedEnum.Delayed => "مؤجل",
                _ => "غير معلوم",
            };
        }
    }
}
