using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Models;
using System.Linq;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Models.Static;
using System.Linq.Expressions;
using System;

namespace KokazGoodsTransfer.Services.Concret
{
    public class OrderService : IOrderService
    {
        private readonly IUintOfWork _uintOfWork;
        private static readonly Func<Order, bool> _finishOrderExpression = c => c.OrderplacedId == (int)OrderplacedEnum.CompletelyReturned || c.OrderplacedId == (int)OrderplacedEnum.Unacceptable
|| (c.OrderplacedId == (int)OrderplacedEnum.Delivered && (c.MoenyPlacedId == (int)MoneyPalcedEnum.InsideCompany || c.MoenyPlacedId == (int)MoneyPalcedEnum.Delivered));
        public OrderService(IUintOfWork uintOfWork)
        {
            _uintOfWork = uintOfWork;
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

            var ids = new HashSet<int>(receiptOfTheStatusOfTheDeliveredShipmentDtos.Select(c => c.Id));
            var orders = await _uintOfWork.Repository<Order>().GetAsync(c => ids.Contains(c.Id));
            return response;
        }
        string OrderPlacedEnumToString(OrderplacedEnum orderplacedEnum)
        {
            switch (orderplacedEnum)
            {
                case OrderplacedEnum.Client: return "عميل";
                case OrderplacedEnum.Store: return "مخزن";
                case OrderplacedEnum.Way: return "طريق";
                case OrderplacedEnum.Delivered: return "تم التسليم";
                case OrderplacedEnum.CompletelyReturned: return "مرتجع كلي";
                case OrderplacedEnum.PartialReturned: return "مرتجع جزئي";
                case OrderplacedEnum.Unacceptable: return "مرفوض";
                case OrderplacedEnum.Delayed: return "مؤجل";

                default: return "غير معلوم";
            }
        }
    }
}
