using AutoMapper;
using ExcelDataReader;
using Quqaz.Web.CustomException;
using Quqaz.Web.DAL.Helper;
using Quqaz.Web.DAL.Infrastructure.Interfaces;
using Quqaz.Web.Dtos.Clients;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.NotifcationDtos;
using Quqaz.Web.Dtos.OrdersDtos;
using Quqaz.Web.HubsConfig;
using Quqaz.Web.Models;
using Quqaz.Web.Models.Static;
using Quqaz.Web.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel;
using Microsoft.AspNetCore.Razor.Language;
using System.Linq.Expressions;
using Quqaz.Web.Dtos.Statics;
using Microsoft.Win32.SafeHandles;
using System.Text;

namespace Quqaz.Web.Services.Concret
{
    public class OrderClientSerivce : IOrderClientSerivce
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<OrderType> _orderTypeRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly NotificationHub _notificationHub;
        private readonly IHttpContextAccessorService _contextAccessorService;
        private readonly IUintOfWork _UintOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public OrderClientSerivce(IRepository<Order> repository, NotificationHub notificationHub, IHttpContextAccessorService contextAccessorService, IMapper mapper, IRepository<OrderType> orderTypeRepository, IUintOfWork uintOfWork, IRepository<Client> clientRepository, IWebHostEnvironment environment)
        {
            _repository = repository;
            _notificationHub = notificationHub;
            _contextAccessorService = contextAccessorService;
            _mapper = mapper;
            _orderTypeRepository = orderTypeRepository;
            _UintOfWork = uintOfWork;
            _clientRepository = clientRepository;
            _environment = environment;
        }

        public async Task<bool> CheckOrderTypesIdsExists(int[] ids)
        {
            ids = ids.Distinct().ToArray();
            var orderTypeIds = await _orderTypeRepository.Select(filter: c => ids.Contains(c.Id), c => c.Id, null);
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

            var client = await _clientRepository.GetById(_contextAccessorService.AuthoticateUserId());
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
                    MoneyPlace = MoneyPalce.OutSideCompany,
                    OrderPlace = OrderPlace.Client,
                    OrderState = OrderState.Processing,
                    ClientId = _contextAccessorService.AuthoticateUserId(),
                    CreatedBy = _contextAccessorService.AuthoticateUserName(),
                    DeliveryCost = country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _contextAccessorService.CurrentBranchId()).DeliveryCost,
                    IsSend = false,
                    BranchId = client.BranchId,
                    CurrentBranchId = client.BranchId
                };
                orders.Add(order);
            }
            await _UintOfWork.RemoveRange(ordersFromExcel);
            await _UintOfWork.AddRange(orders);
            await _UintOfWork.Commit();

            var newOrdersDontSendCount = await _UintOfWork.Repository<Order>().Count(c => c.IsSend == false && c.OrderPlace == OrderPlace.Client);
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
            if (orderTypesIds.Any())
            {
                if (!(await CheckOrderTypesIdsExists(orderTypesIds)))
                {
                    errors.Add("النوع غير موجود");
                }
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
            var client = await _clientRepository.GetById(_contextAccessorService.AuthoticateUserId());
            var country = await _UintOfWork.Repository<Country>().FirstOrDefualt(c => c.Id == order.CountryId, c => c.BranchToCountryDeliverryCosts);
            order.DeliveryCost = country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _contextAccessorService.CurrentBranchId()).DeliveryCost;
            order.ClientId = _contextAccessorService.AuthoticateUserId();
            order.BranchId = client.BranchId;
            order.CurrentBranchId = client.BranchId;
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
            var newOrdersDontSendCount = await _UintOfWork.Repository<Order>().Count(c => c.IsSend == false && c.OrderPlace == OrderPlace.Client);
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
                predicate = predicate.And(c => c.MoneyPlace == orderFilter.MonePlacedId);
            }
            if (orderFilter.OrderplacedId != null)
            {
                predicate = predicate.And(c => c.OrderPlace == orderFilter.OrderplacedId);
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
            if (orderFilter.DateRange != null)
            {
                if (orderFilter.DateRange.FromDate.HasValue)
                {
                    predicate = predicate.And(c => c.Date >= orderFilter.DateRange.FromDate.Value);
                }
                if (orderFilter.DateRange.ToDate.HasValue)
                {
                    predicate = predicate.And(c => c.Date <= orderFilter.DateRange.ToDate.Value);
                }
            }

            var includes = new string[] { nameof(Order.Country), $"{nameof(Order.OrderItems)}.{nameof(OrderItem.OrderTpye)}", $"{nameof(Order.OrderClientPaymnets)}.{nameof(OrderClientPaymnet.ClientPayment)}" };

            var pagingResult = await _repository.GetAsync(paging: pagingDto, filter: predicate, propertySelectors: includes, orderBy: c => c.OrderByDescending(o => o.Date));
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Total = pagingResult.Total,
                Data = _mapper.Map<IEnumerable<OrderDto>>(pagingResult.Data)
            };
        }

        public async Task<OrderDto> GetById(int id)
        {

            var inculdes = new string[] { nameof(Order.Agent), nameof(Order.Country), $"{nameof(Order.OrderItems)}.{nameof(OrderItem.OrderTpye)}", $"{nameof(Order.OrderClientPaymnets)}.{nameof(OrderClientPaymnet.ClientPayment)}" };
            var order = await _repository.FirstOrDefualt(c => c.Id == id, inculdes);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<PagingResualt<IEnumerable<OrderDto>>> NonSendOrder(PagingDto pagingDto)
        {
            var orders = await _repository.GetAsync(paging: pagingDto, c => c.IsSend == false && c.ClientId == _contextAccessorService.AuthoticateUserId(), c => c.Country);
            return new PagingResualt<IEnumerable<OrderDto>>()
            {
                Data = _mapper.Map<IEnumerable<OrderDto>>(orders.Data),
                Total = orders.Total
            };
        }

        public async Task<PagingResualt<List<PayForClientDto>>> OrdersDontFinished(OrderDontFinishFilter orderDontFinishFilter, PagingDto paging)
        {
            var predicate = PredicateBuilder.New<Order>(false);
            if (orderDontFinishFilter.ClientDoNotDeleviredMoney)
            {
                var pr1 = PredicateBuilder.New<Order>(true);
                pr1.And(c => c.IsClientDiliverdMoney == false);
                pr1.And(c => orderDontFinishFilter.OrderPlacedId.Contains(c.OrderPlace));
                pr1.And(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
                predicate.Or(pr1);
            }
            if (orderDontFinishFilter.IsClientDeleviredMoney)
            {
                var pr2 = PredicateBuilder.New<Order>(true);
                pr2.And(c => c.OrderState == OrderState.ShortageOfCash);
                pr2.And(c => orderDontFinishFilter.OrderPlacedId.Contains(c.OrderPlace));
                pr2.And(c => c.ClientId == _contextAccessorService.AuthoticateUserId());
                predicate.Or(pr2);
            }
            var includes = new string[] { nameof(Order.Region), nameof(Order.Country), nameof(Order.Agent), $"{nameof(Order.OrderClientPaymnets)}.{nameof(OrderClientPaymnet.ClientPayment)}", $"{nameof(Order.AgentOrderPrints)}.{nameof(AgentOrderPrint.AgentPrint)}" };
            var orders = await _repository.GetByFilterInclue(paging, predicate, includes);
            orders.Data.ForEach(o =>
            {
                if (o.MoneyPlace == MoneyPalce.WithAgent)
                {
                    o.MoneyPlace = MoneyPalce.OutSideCompany;
                }
            });
            return new PagingResualt<List<PayForClientDto>>()
            {
                Total = orders.Total,
                Data = _mapper.Map<List<PayForClientDto>>(orders.Data)
            };
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
            var newOrdersCount = await _repository.Count(c => c.IsSend == true && c.OrderPlace == OrderPlace.Client);
            var newOrdersDontSendCount = await _repository.Count(c => c.IsSend == false && c.OrderPlace == OrderPlace.Client);
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
            var similarOrders = await _UintOfWork.Repository<Order>().Select(c => codes.Any(co => co == c.Code) && c.ClientId == _contextAccessorService.AuthoticateUserId(), c => c.Code);
            var simialrCodeInExcelTable = await _UintOfWork.Repository<OrderFromExcel>().Select(filter: c => c.ClientId == _contextAccessorService.AuthoticateUserId() && codes.Contains(c.Code), c => c.Code, null);
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
            var client = await _clientRepository.GetById(_contextAccessorService.AuthoticateUserId());
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
                        MoneyPlace = MoneyPalce.OutSideCompany,
                        OrderPlace = OrderPlace.Client,
                        OrderState = OrderState.Processing,
                        ClientId = _contextAccessorService.AuthoticateUserId(),
                        CreatedBy = _contextAccessorService.AuthoticateUserName(),
                        DeliveryCost = country.BranchToCountryDeliverryCosts.First(c => c.BranchId == _contextAccessorService.CurrentBranchId()).DeliveryCost,
                        IsSend = false,
                        BranchId = client.BranchId,
                        CurrentBranchId = client.BranchId
                    };
                    orders.Add(order);
                }
            }
            await _UintOfWork.BegeinTransaction();
            await _UintOfWork.AddRange(orderFromExcels);
            await _UintOfWork.AddRange(orders);
            await _UintOfWork.Commit();

            var newOrdersDontSendCount = await _UintOfWork.Repository<Order>().Count(c => c.IsSend == false && c.OrderPlace == OrderPlace.Client);
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

        public async Task<string> GetReceipt(int id)
        {
            var filePath = _environment.WebRootPath + "/HtmlTemplate/ClientTemplate/OrderReceipt.html";
            var readText = await File.ReadAllTextAsync(filePath);
            var order = await _repository.FirstOrDefualt(c => c.Id == id, c => c.Branch, c => c.Country,
            c => c.OrderItems.Select(o => o.OrderTpye),
            c => c.Client.ClientPhones);
            readText = readText.Replace("{{orderCode}}", order.Code);
            readText = readText.Replace("{{branchName}}", order.Branch.Name);
            readText = readText.Replace("{{clientFbName}}", order.Client.FacebookLinke);
            readText = readText.Replace("{{clientIgName}}", order.Client.IGLink);
            readText = readText.Replace("{{clientPhoneNumber}}", order.Client.ClientPhones.FirstOrDefault().Phone);
            readText = readText.Replace("{{orderDate}}", order.Date.Value.ToString("yyyy-mm-dd"));
            readText = readText.Replace("{{clientName}}", order.Client.Name);
            readText = readText.Replace("{{orderCost}}", (order.Cost + order.DeliveryCost).ToString());
            readText = readText.Replace("{{receiverName}}", order.RecipientName);
            readText = readText.Replace("{{receiverPhone}}", order.RecipientPhones);
            readText = readText.Replace("{{orderAddress}}", order.Country.Name + ":" + order.Address);
            var orderItem = order.OrderItems.FirstOrDefault();
            readText = readText.Replace("{{orderType}}", orderItem?.OrderTpye?.Name);
            readText = readText.Replace("{{orderTypeCount}}", orderItem?.Count.ToString());
            readText = readText.Replace("{{Note}}", order.Note);
            return readText;
        }

        public async Task<List<ClientTrackShipmentDto>> GetShipmentTracking(int id)
        {
            var order = await _repository.FirstOrDefualt(c => c.Id == id, c => c.Country, c => c.NextBranch, c => c.Agent.UserPhones);
            List<ClientTrackShipmentDto> tracking = new List<ClientTrackShipmentDto>();
            if (order.NextBranchId.HasValue)
            {
                var inStore = new ClientTrackShipmentDto()
                {
                    Number = 1,
                    Text = "في المخزن",
                    Checked = false
                };
                tracking.Add(inStore);
                if ((order.OrderPlace == OrderPlace.Store || order.OrderPlace == OrderPlace.Delayed) && order.CurrentBranchId == order.BranchId)
                {
                    inStore.Checked = true;
                }
                var goingToBranch = new ClientTrackShipmentDto()
                {
                    Number = 2,
                    Text = $"متوجعة إلى فرع ${order.NextBranch.Name}",
                    Checked = false
                };
                if (order.OrderPlace == OrderPlace.Way && order.CurrentBranchId == order.BranchId)
                {
                    goingToBranch.Checked = true;
                }
                tracking.Add(goingToBranch);
                var inNextBrach = new ClientTrackShipmentDto()
                {
                    Number = 3,
                    Text = $"وصلت إلى فرع {order.NextBranch.Name}",
                    Checked = false
                };
                if ((order.OrderPlace == OrderPlace.Store || order.OrderPlace == OrderPlace.Delayed) && order.CurrentBranchId == order.NextBranchId)
                {
                    inNextBrach.Checked = true;
                }
                tracking.Add(inNextBrach);
                var withAgent = new ClientTrackShipmentDto()
                {
                    Number = 4,
                    Text = "خرجت مع مندوب",
                    ExtraText = order.Agent?.UserPhones.FirstOrDefault()?.Phone,
                    Checked = false
                };
                if (order.OrderPlace == OrderPlace.Way && order.CurrentBranchId != order.BranchId)
                {
                    withAgent.Checked = true;
                }
                tracking.Add(withAgent);
                var waiting = new ClientTrackShipmentDto()
                {
                    Number = 5,
                    Text = "في إننزظار التسليم",
                    ExtraText = "الأربعاء ",
                    Checked = false
                };
                if (order.OrderPlace == OrderPlace.Delivered || order.OrderPlace == OrderPlace.CompletelyReturned || order.OrderPlace == OrderPlace.PartialReturned && order.MoneyPlace == MoneyPalce.WithAgent)
                {
                    waiting.Checked = true;
                }
                tracking.Add(waiting);
                var delivered = new ClientTrackShipmentDto()
                {
                    Number = 4,
                    Text = "تم التسليم",
                    Checked = tracking.All(c => !c.Checked)
                };
                tracking.Add(delivered);


            }
            else
            {
                var inStore = new ClientTrackShipmentDto()
                {
                    Number = 1,
                    Text = "في المخزن",
                    ExtraText = "الأربعاء ",
                    Checked = false
                };
                if (order.OrderPlace == OrderPlace.Store || order.OrderPlace == OrderPlace.Delayed)
                {
                    inStore.Checked = true;
                }
                tracking.Add(inStore);
                var agent = new ClientTrackShipmentDto()
                {
                    Number = 2,
                    Text = "خرجت مع مندوب",
                    ExtraText = order.Agent?.UserPhones.FirstOrDefault()?.Phone,
                    Checked = false
                };

                if (order.OrderPlace == OrderPlace.Way)
                {
                    agent.Checked = true;
                }
                tracking.Add(agent);
                var waiting = new ClientTrackShipmentDto()
                {
                    Number = 3,
                    Text = "في إننزظار التسليم",
                    Checked = false
                };
                if ((order.OrderPlace == OrderPlace.Delivered || order.OrderPlace == OrderPlace.CompletelyReturned || order.OrderPlace == OrderPlace.PartialReturned) && order.MoneyPlace == MoneyPalce.WithAgent)
                {
                    waiting.Checked = true;
                }
                tracking.Add(waiting);
                var delivered = new ClientTrackShipmentDto()
                {
                    Number = 4,
                    Text = "تم التسليم",
                    Checked = tracking.All(c => !c.Checked)
                };
                tracking.Add(delivered);

            }
            return tracking;
        }

        public async Task<List<AccountReportDto>> GetOrderStatics(DateRangeFilter dateRangeFilter)
        {
            var clientId = _contextAccessorService.AuthoticateUserId();
            var predicate = PredicateBuilder.New<Order>(c => c.ClientId == clientId);
            if (dateRangeFilter.Start.HasValue)
            {
                predicate = predicate.And(c => c.Date >= dateRangeFilter.Start);
            }
            if (dateRangeFilter.End.HasValue)
            {
                predicate = predicate.And(c => c.Date <= dateRangeFilter.End);
            }
            var totalOrder = await _repository.Count(predicate);
            var delivedOrderPredicate = predicate.And(c => c.OrderPlace == OrderPlace.Delivered || c.OrderPlace == OrderPlace.PartialReturned);
            var returndOrderCountPredicate = predicate.And(c => c.OrderPlace == OrderPlace.CompletelyReturned);
            var delivedOrderCount = await _repository.Count(delivedOrderPredicate);
            var returndOrderCount = await _repository.Count(returndOrderCountPredicate);

            return new List<AccountReportDto>
            {
                new AccountReportDto()
                {
                    Title = "عدد الطلبات الكلي",
                    Text = totalOrder.ToString(),
                },
                new AccountReportDto()
                {
                    Title = "عدد الطلبات الواصل",
                    Text = delivedOrderCount.ToString(),
                },
                new AccountReportDto()
                {
                    Title = "عدد الطلبات الراجع",
                    Text = returndOrderCount.ToString(),
                },
                new AccountReportDto()
                {
                    Title = "نسبة الواصل",
                    Text = totalOrder==0?0.ToString(): ((delivedOrderCount*100)/totalOrder).ToString(),
                },
                new AccountReportDto()
                {
                    Title = "نسبةالراجع",
                    Text = totalOrder==0?0.ToString():((returndOrderCount*100)/totalOrder).ToString(),
                },
            };
        }

        public async Task<List<AccountReportDto>> GetPhoneOrderStatusCount(string phone)
        {
            if (phone.Length < 11)
                return null;
            var returnOrderCount = await _repository.Count(c => c.RecipientPhones.Contains(phone) && c.OrderPlace == OrderPlace.CompletelyReturned);
            var delivedOrderCount = await _repository.Count(c => c.RecipientPhones.Contains(phone) && c.OrderPlace == OrderPlace.Delivered);
            var PartialReturnedOrderCount = await _repository.Count(c => c.RecipientPhones.Contains(phone) && c.OrderPlace == OrderPlace.PartialReturned);
            var list = new List<AccountReportDto>
            {
                new AccountReportDto()
                {
                    Text = $"{returnOrderCount}",
                    Title="مرتجع كلي"
                },
                new AccountReportDto()
                {
                    Text = $"{delivedOrderCount}",
                    Title="تم التسليم"
                },
                new AccountReportDto()
                {
                    Text = $"{PartialReturnedOrderCount}",
                    Title="مرتجع جزئي "
                },
            };
            return list;
        }

        public async Task<string> GetOrderDoseNotFinish(OrderDontFinishFilter orderDontFinishFilter)
        {
            var clientId = _contextAccessorService.AuthoticateUserId();
            var currentBranchId = _contextAccessorService.CurrentBranchId();
            var predicate = PredicateBuilder.New<Order>(c => c.ClientId == clientId && orderDontFinishFilter.OrderPlacedId.Contains(c.OrderPlace) && c.AgentId != null);
            if (orderDontFinishFilter.OrderPlacedId.Contains(OrderPlace.CompletelyReturned) || orderDontFinishFilter.OrderPlacedId.Contains(OrderPlace.Unacceptable) || orderDontFinishFilter.OrderPlacedId.Contains(OrderPlace.PartialReturned))
            {
                var orderFilterExcpt = orderDontFinishFilter.OrderPlacedId.Except(new[] { OrderPlace.CompletelyReturned, OrderPlace.Unacceptable, OrderPlace.PartialReturned });
                predicate = PredicateBuilder.New<Order>(c => c.ClientId == clientId && orderFilterExcpt.Contains(c.OrderPlace) && c.AgentId != null);
                var orderPlacePredicate = PredicateBuilder.New<Order>(false);
                if (orderDontFinishFilter.OrderPlacedId.Contains(OrderPlace.CompletelyReturned))
                {
                    orderPlacePredicate = orderPlacePredicate.Or(c => c.OrderPlace == OrderPlace.CompletelyReturned && c.CurrentBranchId == currentBranchId);
                }
                if (orderDontFinishFilter.OrderPlacedId.Contains(OrderPlace.Unacceptable))
                {
                    orderPlacePredicate = orderPlacePredicate.Or(c => c.OrderPlace == OrderPlace.Unacceptable && c.CurrentBranchId == currentBranchId);
                }
                if (orderDontFinishFilter.OrderPlacedId.Contains(OrderPlace.PartialReturned))
                {
                    orderPlacePredicate = orderPlacePredicate.Or(c => c.OrderPlace == OrderPlace.PartialReturned && c.CurrentBranchId == currentBranchId);
                }
                orderPlacePredicate = orderPlacePredicate.And(c => c.ClientId == clientId && c.AgentId != null);
                predicate = predicate.Or(orderPlacePredicate);

            }
            if (orderDontFinishFilter.ClientDoNotDeleviredMoney && !orderDontFinishFilter.IsClientDeleviredMoney)
            {
                predicate = predicate.And(c => c.IsClientDiliverdMoney == false);
            }
            else if (!orderDontFinishFilter.ClientDoNotDeleviredMoney && orderDontFinishFilter.IsClientDeleviredMoney)
            {
                predicate = predicate.And(c => c.OrderState == OrderState.ShortageOfCash);
            }
            else if (orderDontFinishFilter.ClientDoNotDeleviredMoney && orderDontFinishFilter.IsClientDeleviredMoney)
            {
                predicate = predicate.And(c => c.OrderState == OrderState.ShortageOfCash || c.IsClientDiliverdMoney == false);
            }
            var data = await _repository.GetAsync(filter: predicate);
            var rows = new StringBuilder();
            var rowTempaltePath = _environment.WebRootPath + "/HtmlTemplate/ClientTemplate/OrderToPayReport/OrderToPayRow.html";

            var rowTempalte = await File.ReadAllTextAsync(rowTempaltePath);
            int counter = 1;
            foreach (var order in data)
            {
                var row = rowTempalte.Replace("{{incremental}}", (counter++).ToString());
                row = row.Replace("{{code}}", order.Code);
                row = row.Replace("{{phone}}", order.RecipientPhones);
                row = row.Replace("{{cost}}", order.Cost.ToString());
                row = row.Replace("{{deliveryCost}}", order.DeliveryCost.ToString());
                row = row.Replace("{{total}}", (order.Cost - order.DeliveryCost).ToString());
                row = row.Replace("{{note}}", order.Note.ToString());
                row = row.Replace("{{clientNote}}", order.ClientNote.ToString());
                rows.AppendLine(row);
            }
            var htmlPagePath = _environment.WebRootPath + "/HtmlTemplate/ClientTemplate/OrderToPayReport/OrderToPayRepor.html";
            var htmlPage = await File.ReadAllTextAsync(rowTempaltePath);
            htmlPage = htmlPage.Replace("{{rows}}", rows.ToString());
            return htmlPage;

        }
    }
}