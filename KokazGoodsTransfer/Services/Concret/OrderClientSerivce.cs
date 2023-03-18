using AutoMapper;
using ExcelDataReader;
using KokazGoodsTransfer.CustomException;
using KokazGoodsTransfer.DAL.Helper;
using KokazGoodsTransfer.DAL.Infrastructure.Interfaces;
using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.NotifcationDtos;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.HubsConfig;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using KokazGoodsTransfer.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Services.Concret
{
    public class OrderClientSerivce : IOrderClientSerivce
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<OrderType> _orderTypeRepository;
        private readonly IRepository<MoenyPlaced> _moneyPlacedRepository;
        private readonly NotificationHub _notificationHub;
        private readonly IHttpContextAccessorService _contextAccessorService;
        private readonly IUintOfWork _UintOfWork;
        private readonly IMapper _mapper;
        public OrderClientSerivce(IRepository<Order> repository, NotificationHub notificationHub, IHttpContextAccessorService contextAccessorService, IMapper mapper, IRepository<OrderType> orderTypeRepository, IRepository<MoenyPlaced> moneyPlacedRepository, IUintOfWork uintOfWork)
        {
            _repository = repository;
            _notificationHub = notificationHub;
            _contextAccessorService = contextAccessorService;
            _mapper = mapper;
            _orderTypeRepository = orderTypeRepository;
            _moneyPlacedRepository = moneyPlacedRepository;
            _UintOfWork = uintOfWork;
        }

        public async Task<bool> CheckOrderTypesIdsExists(int[] ids)
        {
            ids = ids.Distinct().ToArray();
            var orderTypeIds = await _orderTypeRepository.Select(c => c.Id, c => ids.Contains(c.Id));
            if (orderTypeIds.Count() == ids.Length)
                return true;
            return false;
        }

        public Task<bool> CodeExist(string code)
        {
            return _repository.Any(c => c.Code == code && c.ClientId == _contextAccessorService.AuthoticateUserId());
        }

        public async Task CorrectOrderCountry(List<KeyValuePair<int, int>> pairs)
        {
            var ids = pairs.Select(c => c.Key).ToList();
            var cids = pairs.Select(c => c.Value).ToList();


            var ordersFromExcel = await _UintOfWork.Repository<OrderFromExcel>().GetAsync(c => ids.Contains(c.Id));
            var countries = await _UintOfWork.Repository<Country>().GetAsync(c => cids.Contains(c.Id), c => c.BranchToCountryDeliverryCosts);

            await _UintOfWork.BegeinTransaction();
            List<Order> orders = new List<Order>();
            foreach (var item in pairs)
            {
                var ofe = ordersFromExcel.FirstOrDefault(c => c.Id == item.Key);
                if (ofe == null)
                    continue;
                var country = countries.FirstOrDefault(c => c.Id == item.Value);

                var order = new Order()
                {
                    Code = ofe.Code,
                    CountryId = item.Value,
                    Address = ofe.Address,
                    RecipientName = ofe.RecipientName,
                    RecipientPhones = ofe.Phone,
                    ClientNote = ofe.Note,
                    Cost = ofe.Cost,
                    Date = ofe.CreateDate,
                    MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany,
                    OrderplacedId = (int)OrderplacedEnum.Client,
                    OrderStateId = (int)OrderStateEnum.Processing,
                    ClientId = _contextAccessorService.AuthoticateUserId(),
                    CreatedBy = _contextAccessorService.AuthoticateUserName(),
                    DeliveryCost = country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _contextAccessorService.CurrentBranchId()).DeliveryCost,
                    IsSend = false,
                };
                orders.Add(order);
            }
            await _UintOfWork.RemoveRange(ordersFromExcel);
            await _UintOfWork.AddRange(orders);
            await _UintOfWork.Commit();

            var newOrdersDontSendCount = await _UintOfWork.Repository<Order>().Count(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client);
            AdminNotification adminNotification = new AdminNotification()
            {
                NewOrdersDontSendCount = newOrdersDontSendCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
        }

        public async Task<List<string>> Validate(CreateOrderFromClient createOrderFromClient)
        {
            List<string> erros = new List<string>();
            if (await CodeExist(createOrderFromClient.Code))
            {
                erros.Add("الكود موجود مسبقاً");
            }
            if (createOrderFromClient.RecipientPhones.Length == 0)
            {
                erros.Add("رقم الهاتف مطلوب");
            }
            erros.AddRange(await OrderItemValidation(createOrderFromClient.OrderItem));
            return erros;
        }
        private async Task<HashSet<string>> OrderItemValidation(List<OrderItemDto> orderItemDtos)
        {
            var errors = new HashSet<string>();
            if (orderItemDtos?.Any() == false)
            {
                return errors;
            }
            if (orderItemDtos.Any(c => c.OrderTypeId == null && string.IsNullOrEmpty(c.OrderTypeName.Trim())))
            {
                errors.Add("يجب وضع اسم نوع الشحنة");
            }
            var orderTypesIds = orderItemDtos.Where(c => c.OrderTypeId != null).Select(c => c.OrderTypeId.Value).ToArray();
            if (await CheckOrderTypesIdsExists(orderTypesIds))
            {
                errors.Add("النوع غير موجود");
            }
            return errors;
        }
        public async Task<OrderResponseClientDto> Create(CreateOrderFromClient createOrderFromClient)
        {
            var validation = await Validate(createOrderFromClient);
            if (validation.Count > 0)
            {
                throw new ConflictException(validation);
            }
            var order = _mapper.Map<Order>(createOrderFromClient);
            var country = await _UintOfWork.Repository<Country>().FirstOrDefualt(c => c.Id == order.CountryId, c => c.BranchToCountryDeliverryCosts);
            order.DeliveryCost = country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _contextAccessorService.CurrentBranchId()).DeliveryCost;
            order.ClientId = _contextAccessorService.AuthoticateUserId();
            order.CreatedBy = _contextAccessorService.AuthoticateUserName();
            await _UintOfWork.BegeinTransaction();
            await _UintOfWork.Add(order);
            var orderItems = createOrderFromClient.OrderItem;
            if (orderItems?.Any() == true)
            {
                var orderTypesNames = orderItems.Where(c => c.OrderTypeId == null).Select(c => c.OrderTypeName).Distinct();
                var orderTypes = await _UintOfWork.Repository<OrderType>().GetAsync(c => orderTypesNames.Any(ot => ot == c.Name));

                foreach (var item in orderItems)
                {
                    int orderTypeId;
                    if (item.OrderTypeId.HasValue)
                        orderTypeId = item.OrderTypeId.Value;
                    else
                    {
                        var simi = orderTypes.FirstOrDefault(c => c.Name == item.OrderTypeName);
                        if (simi != null)
                            orderTypeId = simi.Id;
                        else
                        {
                            ///TODO : make it faster 
                            var orderType = new OrderType()
                            {
                                Name = item.OrderTypeName
                            };
                            await _UintOfWork.Add(orderType);
                            orderTypeId = orderType.Id;
                        }
                    }
                    await _UintOfWork.Add(new OrderItem()
                    {
                        Count = item.Count,
                        OrderId = order.Id,
                        OrderTpyeId = orderTypeId
                    });

                }
            }
            await _UintOfWork.Commit();
            var newOrdersDontSendCount = await _UintOfWork.Repository<Order>().Count(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client);
            AdminNotification adminNotification = new AdminNotification()
            {
                NewOrdersDontSendCount = newOrdersDontSendCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
            return _mapper.Map<OrderResponseClientDto>(order);
        }

        public async Task Delete(int id)
        {
            var order = await _repository.GetById(id);
            if (order.IsSend == true)
                throw new ConflictException("");
            await _repository.Delete(order);

        }

        public async Task<PagingResualt<IEnumerable<OrderDto>>> Get(PagingDto pagingDto, COrderFilter orderFilter)
        {
            var predicate = PredicateBuilder.New<Order>(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
            if (orderFilter.CountryId != null)
            {
                predicate = predicate.And(c => c.CountryId == orderFilter.CountryId);
            }
            if (orderFilter.Code != string.Empty && orderFilter.Code != null)
            {
                predicate = predicate.And(c => c.Code.StartsWith(orderFilter.Code));
            }

            if (orderFilter.RegionId != null)
            {
                predicate = predicate.And(c => c.RegionId == orderFilter.RegionId);
            }
            if (orderFilter.RecipientName != string.Empty && orderFilter.RecipientName != null)
            {
                predicate = predicate.And(c => c.RecipientName.StartsWith(orderFilter.RecipientName));
            }
            if (orderFilter.MonePlacedId != null)
            {
                predicate = predicate.And(c => c.MoenyPlacedId == orderFilter.MonePlacedId);
            }
            if (orderFilter.OrderplacedId != null)
            {
                predicate = predicate.And(c => c.OrderplacedId == orderFilter.OrderplacedId);
            }
            if (orderFilter.Phone != string.Empty && orderFilter.Phone != null)
            {
                predicate = predicate.And(c => c.RecipientPhones.Contains(orderFilter.Phone));
            }
            if (orderFilter.IsClientDiliverdMoney != null)
            {
                predicate = predicate.And(c => c.IsClientDiliverdMoney == orderFilter.IsClientDiliverdMoney);
            }
            if (orderFilter.ClientPrintNumber != null)
            {
                predicate = predicate.And(c => c.OrderClientPaymnets.Any(op => op.ClientPayment.Id == orderFilter.ClientPrintNumber));
            }

            var includes = new string[] { nameof(Order.Country), nameof(Order.Orderplaced), nameof(Order.MoenyPlaced), $"{nameof(Order.OrderItems)}.{nameof(OrderItem.OrderTpye)}", $"{nameof(Order.OrderClientPaymnets)}.{nameof(OrderClientPaymnet.ClientPayment)}" };

            var pagingResult = await _repository.GetAsync(pagingDto, predicate, includes);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResult.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResult.Data)
            };
        }

        public async Task<OrderDto> GetById(int id)
        {

            var inculdes = new string[] { nameof(Order.Agent), nameof(Order.Country), nameof(Order.Orderplaced), nameof(Order.MoenyPlaced), $"{nameof(Order.OrderItems)}.{nameof(OrderItem.OrderTpye)}", $"{nameof(Order.OrderClientPaymnets)}.{nameof(OrderClientPaymnet.ClientPayment)}" };
            var order = await _repository.FirstOrDefualt(c => c.Id == id, inculdes);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> NonSendOrder()
        {
            var orders = await _repository.GetAsync(c => c.IsSend == false && c.ClientId == _contextAccessorService.AuthoticateUserId(), c => c.Country, c => c.MoenyPlaced, c => c.Orderplaced);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<IEnumerable<PayForClientDto>> OrdersDontFinished(OrderDontFinishFilter orderDontFinishFilter)
        {
            var outSideCompany = await _moneyPlacedRepository.GetById((int)MoneyPalcedEnum.OutSideCompany);
            var predicate = PredicateBuilder.New<Order>(false);
            if (orderDontFinishFilter.ClientDoNotDeleviredMoney)
            {
                var pr1 = PredicateBuilder.New<Order>(true);
                pr1.And(c => c.IsClientDiliverdMoney == false);
                pr1.And(c => orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId));
                pr1.And(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
                predicate.Or(pr1);
            }
            if (orderDontFinishFilter.IsClientDeleviredMoney)
            {
                var pr2 = PredicateBuilder.New<Order>(true);
                pr2.And(c => c.OrderStateId == (int)OrderStateEnum.ShortageOfCash);
                pr2.And(c => orderDontFinishFilter.OrderPlacedId.Contains(c.OrderplacedId));
                pr2.And(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
                predicate.Or(pr2);
            }
            var includes = new string[] { nameof(Order.Region), nameof(Order.Country), nameof(Order.Orderplaced), nameof(Order.MoenyPlaced), nameof(Order.Agent), $"{nameof(Order.OrderClientPaymnets)}.{nameof(OrderClientPaymnet.ClientPayment)}", $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}" };
            var orders = await _repository.GetByFilterInclue(predicate, includes);
            orders.ForEach(o =>
            {
                if (o.MoenyPlacedId == (int)MoneyPalcedEnum.WithAgent)
                {
                    o.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                    o.MoenyPlaced = outSideCompany;
                }
            });
            return _mapper.Map<IEnumerable<PayForClientDto>>(orders);
        }

        public async Task<IEnumerable<OrderFromExcel>> OrdersNeedToRevision()
        {
            return await _UintOfWork.Repository<OrderFromExcel>().GetAsync(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
        }

        public async Task Send(int[] ids)
        {
            var orders = await _repository.GetAsync(c => ids.Contains(c.Id));
            foreach (var item in orders)
            {
                item.IsSend = true;
            }
            await _repository.Update(orders);
            var newOrdersCount = await _repository.Count(c => c.IsSend == true && c.OrderplacedId == (int)OrderplacedEnum.Client);
            var newOrdersDontSendCount = await _repository.Count(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client);
            var adminNotification = new AdminNotification()
            {
                NewOrdersCount = newOrdersCount,
                NewOrdersDontSendCount = newOrdersDontSendCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
        }

        private async Task<(List<OrderFromExcelDto> orderFromExcelDtos, HashSet<string> errors)> GetOrderFromExcelFile(IFormFile file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            HashSet<string> errors = new HashSet<string>();
            var excelOrder = new List<OrderFromExcelDto>();
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            using var reader = ExcelReaderFactory.CreateReader(stream);
            while (reader.Read())
            {
                var order = new OrderFromExcelDto();
                if (!reader.IsDBNull(0))
                {
                    order.Code = reader.GetValue(0).ToString();
                    if (excelOrder.Any(c => c.Code == order.Code))
                    {
                        errors.Add($"الكود {order.Code} مكرر");
                    }
                }
                else
                {
                    errors.Add("يجب ملئ الكود ");
                }
                if (!reader.IsDBNull(1))
                {
                    order.RecipientName = reader.GetValue(1).ToString();
                }
                if (!reader.IsDBNull(2))
                {
                    order.Country = reader.GetValue(2).ToString();
                }
                else
                {
                    errors.Add("يجب ملئ المحافظة");
                }
                if (!reader.IsDBNull(3))
                {
                    if (Decimal.TryParse(reader.GetValue(3).ToString(), out var d))
                    {
                        order.Cost = d;
                    }
                    else
                    {
                        errors.Add("كلفة الطلب ليست رقم");
                    }
                }
                else
                {
                    errors.Add("كلفة الطلب إجبارية");
                }
                if (!reader.IsDBNull(4))
                {
                    order.Address = reader.GetValue(4).ToString();
                }
                if (!reader.IsDBNull(5))
                {
                    order.Phone = reader.GetValue(5).ToString();
                    if (order.Phone.Length > 15)
                    {
                        errors.Add("رقم الهاتف لا يجب ان يكون اكبر من 15 رقم");
                    }
                }
                else
                {
                    errors.Add("رقم الهاتف إجباري");
                }
                if (!reader.IsDBNull(6))
                {
                    order.Note = reader.GetValue(6).ToString();
                }
                excelOrder.Add(order);
            }
            return (excelOrder, errors);
        }

        public async Task<bool> CreateFromExcel(IFormFile file, DateTime dateTime)
        {
            var (excelOrder, errors) = await GetOrderFromExcelFile(file);

            var codes = excelOrder.Select(c => c.Code);
            var similarOrders = await _UintOfWork.Repository<Order>().Select(c => c.Code, c => codes.Contains(c.Code) && c.ClientId == _contextAccessorService.AuthoticateUserId());
            var simialrCodeInExcelTable = await _UintOfWork.Repository<OrderFromExcel>().Select(c => c.Code, c => c.ClientId == _contextAccessorService.AuthoticateUserId() && codes.Contains(c.Code));
            if (similarOrders.Any())
            {
                errors.Add($"الأكواد مكررة{string.Join(",", similarOrders)}");
            }
            if (simialrCodeInExcelTable.Any())
            {
                errors.Add($"الأكواد {string.Join(",", simialrCodeInExcelTable)} مكررة يجب مراجعة واجهة التصحيح");
            }
            if (errors.Any())
            {
                throw new ConflictException(errors);
            }
            bool correct = false;


            var countriesName = excelOrder.Select(c => c.Country).Distinct().ToList();
            var countries = await _UintOfWork.Repository<Country>().GetAsync(c => countriesName.Contains(c.Name), c => c.BranchToCountryDeliverryCosts);
            var orderFromExcels = new List<OrderFromExcel>();
            var orders = new List<Order>();
            foreach (var item in excelOrder)
            {
                var country = countries.FirstOrDefault(c => c.Name == item.Country);
                if (country == null)
                {
                    var orderFromExcel = new OrderFromExcel()
                    {
                        Address = item.Address,
                        Code = item.Code,
                        Cost = item.Cost,
                        Country = item.Country,
                        Note = item.Note,
                        Phone = item.Phone,
                        RecipientName = item.RecipientName,
                        ClientId = _contextAccessorService.AuthoticateUserId(),
                        CreateDate = dateTime
                    };
                    orderFromExcels.Add(orderFromExcel);
                    correct = true;
                }
                else
                {
                    var order = new Order()
                    {
                        Code = item.Code,
                        CountryId = country.Id,
                        Address = item.Address,
                        RecipientName = item.RecipientName,
                        RecipientPhones = item.Phone,
                        ClientNote = item.Note,
                        Cost = item.Cost,
                        Date = dateTime,
                        MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany,
                        OrderplacedId = (int)OrderplacedEnum.Client,
                        OrderStateId = (int)OrderStateEnum.Processing,
                        ClientId = _contextAccessorService.AuthoticateUserId(),
                        CreatedBy = _contextAccessorService.AuthoticateUserName(),
                        DeliveryCost = country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _contextAccessorService.CurrentBranchId()).DeliveryCost,
                        IsSend = false,
                    };
                    orders.Add(order);
                }
            }
            await _UintOfWork.BegeinTransaction();
            await _UintOfWork.AddRange(orderFromExcels);
            await _UintOfWork.AddRange(orders);
            await _UintOfWork.Commit();

            var newOrdersDontSendCount = await _UintOfWork.Repository<Order>().Count(c => c.IsSend == false && c.OrderplacedId == (int)OrderplacedEnum.Client);
            AdminNotification adminNotification = new AdminNotification()
            {
                NewOrdersDontSendCount = newOrdersDontSendCount
            };
            await _notificationHub.AdminNotifcation(adminNotification);
            return correct;
        }

        public async Task Edit(EditOrder editOrder)
        {
            var order = await _UintOfWork.Repository<Order>().FirstOrDefualt(c => c.Id == editOrder.Id, c => c.OrderItems);
            order.Code = editOrder.Code;
            order.CountryId = editOrder.CountryId;
            order.Address = editOrder.Address;
            order.RecipientName = editOrder.RecipientName;
            order.ClientNote = editOrder.ClientNote;
            order.Cost = editOrder.Cost;
            order.Date = editOrder.Date;
            var country = await _UintOfWork.Repository<Country>().FirstOrDefualt(c => c.Id == editOrder.CountryId, c => c.BranchToCountryDeliverryCosts);
            order.DeliveryCost = country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _contextAccessorService.CurrentBranchId()).DeliveryCost;
            order.RecipientPhones = String.Join(',', editOrder.RecipientPhones);
            var erros = await OrderItemValidation(editOrder.OrderItem);
            if (erros.Any())
                throw new ConflictException(erros);


            order.OrderItems.Clear();
            var orderTypesNames = editOrder.OrderItem.Where(c => c.OrderTypeId == null).Select(c => c.OrderTypeName).Distinct();
            var orderTypes = await _UintOfWork.Repository<OrderType>().GetAsync(c => orderTypesNames.Any(ot => ot == c.Name));
            await _UintOfWork.BegeinTransaction();
            foreach (var item in editOrder.OrderItem)
            {

                int orderTypeId;
                if (item.OrderTypeId.HasValue)
                    orderTypeId = item.OrderTypeId.Value;
                else
                {
                    var simi = orderTypes.FirstOrDefault(c => c.Name == item.OrderTypeName);
                    if (simi != null)
                        orderTypeId = simi.Id;
                    else
                    {
                        ///TODO : make it faster 
                        var orderType = new OrderType()
                        {
                            Name = item.OrderTypeName
                        };
                        await _UintOfWork.Add(orderType);
                        orderTypeId = orderType.Id;
                    }
                }
                order.OrderItems.Add(new OrderItem()
                {
                    Count = item.Count,
                    OrderTpyeId = orderTypeId
                });
            }
            await _UintOfWork.Update(order);
            await _UintOfWork.Commit();
        }
    }
}