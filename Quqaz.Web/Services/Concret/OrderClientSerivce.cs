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
using Quqaz.Web.Dtos.Statics;
using System.Text;
using Quqaz.Web.Helpers.Extensions;
using OfficeOpenXml;
using Quqaz.Web.Services.Helper;
using Google.Apis.Util;

namespace Quqaz.Web.Services.Concret
{
    public class OrderClientSerivce : IOrderClientSerivce
    {
        private readonly IRepository<Order> _repository;
        private readonly IRepository<Receipt> _receiptRepository;
        private readonly IRepository<OrderType> _orderTypeRepository;
        private readonly IRepository<Client> _clientRepository;
        private readonly NotificationHub _notificationHub;
        private readonly IHttpContextAccessorService _contextAccessorService;
        private readonly IUintOfWork _UintOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        public OrderClientSerivce(IRepository<Order> repository, NotificationHub notificationHub, IHttpContextAccessorService contextAccessorService, IMapper mapper, IRepository<OrderType> orderTypeRepository, IUintOfWork uintOfWork, IRepository<Client> clientRepository, IWebHostEnvironment environment, IRepository<Receipt> receiptRepository)
        {
            _repository = repository;
            _notificationHub = notificationHub;
            _contextAccessorService = contextAccessorService;
            _mapper = mapper;
            _orderTypeRepository = orderTypeRepository;
            _UintOfWork = uintOfWork;
            _clientRepository = clientRepository;
            _environment = environment;
            _receiptRepository = receiptRepository;
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
        private static async Task<ExcelWorksheet> GetFirstSheet(IFormFile file)
        {
            var workBook = await GetWorkbook(file);
            var sheet = workBook.Worksheets[0];
            return sheet;
        }
        private static async Task<ExcelWorkbook> GetWorkbook(IFormFile file)
        {
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
            ExcelPackage excel = new ExcelPackage(ms);
            return excel.Workbook;
        }
        private async Task<(List<OrderFromExcelDto> orderFromExcelDtos, List<string> errors)> GetOrderFromExcelFile(IFormFile file)
        {
            var sheet = await GetFirstSheet(file);
            List<string> errors = new List<string>();
            List<OrderFromExcelDto> orders = new List<OrderFromExcelDto>();
            for (var i = 2; i < sheet.Dimension.Columns; i++)
            {
                string errorMessageTemplate = "{0} في السطر رقم " + i;
                var code = sheet.Cells[i, OrderExcelIndexes.CodeIndex].Value?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(code))
                {
                    continue;
                }
                var recipientName = sheet.Cells[i, OrderExcelIndexes.RecipientNameIndex].Value?.ToString() ?? string.Empty;

                var countryName = sheet.Cells[i, OrderExcelIndexes.CountryIndex].Value?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(countryName))
                {
                    errors.Add(string.Format(errorMessageTemplate, "حقل المدينة فارغ"));
                    continue;
                }
                var cosAsString = sheet.Cells[i, OrderExcelIndexes.CostIndex].Value.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(cosAsString))
                {
                    errors.Add(string.Format(errorMessageTemplate, "حقل المبلغ فارغ "));
                    continue;
                }
                if (!decimal.TryParse(cosAsString, out decimal cost))
                {
                    errors.Add(string.Format(errorMessageTemplate, "حقل المبلغ لا يحتوي على رقم  "));
                }
                var phone = sheet.Cells[i, OrderExcelIndexes.PhoneIndex].Value?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(phone))
                {
                    errors.Add(string.Format(errorMessageTemplate, "حقل رقم الهاتف "));
                    continue;
                }
                if (phone.Length > 11)
                {
                    errors.Add(string.Format(errorMessageTemplate, "رقم الهاتف غير صحيح "));
                }
                var address = sheet.Cells[i, OrderExcelIndexes.AddressIndex].Value?.ToString() ?? string.Empty;
                var note = sheet.Cells[i, OrderExcelIndexes.ClientNoteIndex].Value?.ToString() ?? string.Empty;
                var orderType = sheet.Cells[i, OrderExcelIndexes.OrderTypeIndex].Value?.ToString() ?? string.Empty;
                int? orderCount = null;
                if (!string.IsNullOrEmpty(orderType))
                {
                    var orderCountAsString = sheet.Cells[i, OrderExcelIndexes.OrderTypeCountIndex].Value?.ToString() ?? string.Empty;
                    if (!string.IsNullOrEmpty(orderCountAsString))
                    {
                        if (int.TryParse(orderCountAsString, out int orderCountValue))
                        {
                            orderCount = orderCountValue;
                        }
                    }
                }
                var order = new OrderFromExcelDto()
                {
                    Code = code,
                    Country = countryName,
                    Cost = cost,
                    Phone = phone,
                    RecipientName = recipientName,
                    Address = address,
                    Note = note,
                    OrderType = orderType,
                    OrderTypeCount = orderCount

                };
                orders.Add(order);
            }
            return (orders, errors);
        }
        public async Task<bool> ImportFromExcel(IFormFile file, DateTime dateTime)
        {
            var sheet = await GetFirstSheet(file);

            var (excelOrder, errors) = await GetOrderFromExcelFile(file);

            var codes = excelOrder.Select(c => c.Code);
            var similarOrders = await _UintOfWork.Repository<Order>().Select(c => codes.Any(co => co == c.Code) && c.ClientId == _contextAccessorService.AuthoticateUserId(), c => c.Code);
            var similarCodesInExcelTable = await _UintOfWork.Repository<OrderFromExcel>().Select(filter: c => c.ClientId == _contextAccessorService.AuthoticateUserId() && codes.Contains(c.Code), c => c.Code, null);
            if (similarOrders.Any())
            {
                errors.Add($"الأكواد مكررة{string.Join(",", similarOrders)}");
            }
            if (similarCodesInExcelTable.Any())
            {
                errors.Add($"الأكواد {string.Join(",", similarCodesInExcelTable)} مكررة يجب مراجعة واجهة التصحيح");
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
            var typesNames = excelOrder.Select(c => c.OrderType).Distinct();
            var orderTypes = await _orderTypeRepository.GetAsync(c => typesNames.Contains(c.Name));
            var existsOrderTypeNames = orderTypes.Select(c => c.Name);
            var notExistsOrderType = typesNames.Except(existsOrderTypeNames);
            var newOrderTypes = notExistsOrderType.Select(c => new OrderType()
            {
                Name = c,
                BranchId = client.BranchId
            });
            await _UintOfWork.BegeinTransaction();
            try
            {
                await _UintOfWork.AddRange(newOrderTypes);

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
                            CreateDate = dateTime,
                            OrderType = item.OrderType,
                            OrderTypeCount = item.OrderTypeCount
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
                        var orderType = orderTypes.FirstOrDefault(c => c.Name == item.OrderType);
                        if (orderType == null)
                        {
                            var newOrderType = newOrderTypes.First(c => c.Name == item.OrderType);
                            order.OrderItems.Add(new OrderItem()
                            {
                                OrderTpye = newOrderType,
                                Count = item.OrderTypeCount ?? 1,

                            });
                        }
                        else
                        {

                            order.OrderItems.Add(new OrderItem()
                            {
                                OrderTpyeId = orderType.Id,
                                Count = item.OrderTypeCount ?? 1
                            });
                        }
                        orders.Add(order);
                    }
                }
                await _UintOfWork.AddRange(orderFromExcels);
                await _UintOfWork.AddRange(orders);
                await _UintOfWork.Commit();

                var newOrdersDontSendCount = await _UintOfWork.Repository<Order>().Count(c => c.IsSend == false && c.OrderPlace == OrderPlace.Client);
                AdminNotification adminNotification = new AdminNotification()
                {
                    NewOrdersDontSendCount = newOrdersDontSendCount
                };
                await _notificationHub.AdminNotifcation(adminNotification);
            }
            catch (Exception ex)
            {
                await _UintOfWork.Rollback();
            }
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
            var order = await _repository.FirstOrDefualt(c => c.Id == id, c => c.Country, c => c.TargetBranch, c => c.Agent.UserPhones);
            List<ClientTrackShipmentDto> tracking = new List<ClientTrackShipmentDto>();
            if (order.TargetBranchId.HasValue)
            {
                var inStroe = new ClientTrackShipmentDto()
                {
                    Number = 1,
                    Text = "في المخزن",
                };
                var inWayToOtherBranch = new ClientTrackShipmentDto()
                {
                    Number = 2,
                    Text = $"في الطريق إلى فرع {order.TargetBranch.Name}"
                };
                var inStoreInNexBranch = new ClientTrackShipmentDto()
                {
                    Number = 3,
                    Text = $"في مخزن فرع {order.TargetBranch.Name}"
                };
                var agent = new ClientTrackShipmentDto()
                {
                    Number = 4,
                    Text = "خرجت مع المندوب ",
                    ExtraText = order.Agent?.UserPhones.FirstOrDefault()?.Phone
                };
                var agentOrderStatus = new ClientTrackShipmentDto()
                {
                    Number = 5,
                    Text = "تم التسليم/ المبلغ مع المندوب "
                };
                var companyOrderStatus = new ClientTrackShipmentDto()
                {
                    Number = 6,
                    Text = "المبلغ داخل الشركة"
                };
                var finalOrderStatus = new ClientTrackShipmentDto()
                {
                    Number = 7,
                    Text = "تم التسديد"
                };
                tracking.Add(inStroe);
                tracking.Add(inWayToOtherBranch);
                tracking.Add(inStoreInNexBranch);
                tracking.Add(agent);
                tracking.Add(agentOrderStatus);
                tracking.Add(companyOrderStatus);
                tracking.Add(finalOrderStatus);
                if (order.OrderPlace == OrderPlace.Store)
                {
                    if (order.CurrentBranchId == order.BranchId)
                    {
                        inStroe.Checked = true;
                    }
                    else
                    {
                        inStoreInNexBranch.Checked = true;
                    }
                }
                else
                {
                    if (order.OrderPlace == OrderPlace.Way)
                    {
                        if (order.CurrentBranchId == order.BranchId)
                        {
                            inWayToOtherBranch.Checked = true;
                        }
                        else
                        {
                            agent.Checked = true;
                        }
                    }
                    else
                    {
                        if (order.OrderPlace == OrderPlace.Delayed)
                        {
                            inStoreInNexBranch.Checked = true;
                        }
                        else
                        {
                            if (order.OrderPlace == OrderPlace.CompletelyReturned)
                            {
                                agentOrderStatus.Text = "الشحنة مرتجعة مع المندوب ";
                                companyOrderStatus.Text = "الشحنة مرتجعة داخل الشركة";
                                finalOrderStatus.Text = "مرتجع تم التسليم";
                                if (order.MoneyPlace == MoneyPalce.WithAgent)
                                {
                                    agentOrderStatus.Checked = true;
                                }
                                else if (order.MoneyPlace == MoneyPalce.InsideCompany)
                                {
                                    companyOrderStatus.Checked = true;
                                }
                                else
                                {
                                    finalOrderStatus.Checked = true;
                                }
                            }
                            else
                            {
                                if (order.ShouldToPay() == 0)
                                {
                                    finalOrderStatus.Checked = true;
                                }
                                else
                                {
                                    agentOrderStatus.Text = "تم التسليم/ المبلغ مع المندوب/ هناك تغير في السعر";
                                    companyOrderStatus.Text = "المبلغ داخل الشركة/هناك تغير في السعر";
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var inStroe = new ClientTrackShipmentDto()
                {
                    Number = 1,
                    Text = "في المخزن",
                };
                var agent = new ClientTrackShipmentDto()
                {
                    Number = 2,
                    Text = "خرجت مع المندوب ",
                    ExtraText = order.Agent?.UserPhones.FirstOrDefault()?.Phone
                };
                var agentOrderStatus = new ClientTrackShipmentDto()
                {
                    Number = 3,
                    Text = "تم التسليم/ المبلغ مع المندوب "
                };
                var companyOrderStatus = new ClientTrackShipmentDto()
                {
                    Number = 4,
                    Text = "المبلغ داخل الشركة"
                };
                var finalOrderStatus = new ClientTrackShipmentDto()
                {
                    Number = 5,
                    Text = "تم التسديد"
                };
                tracking.Add(inStroe);
                tracking.Add(agent);
                tracking.Add(agentOrderStatus);
                tracking.Add(companyOrderStatus);
                tracking.Add(finalOrderStatus);
                if (order.OrderPlace == OrderPlace.Store)
                {
                    inStroe.Checked = true;
                }
                else
                {
                    if (order.OrderPlace == OrderPlace.Way)
                    {
                        agent.Checked = true;
                    }
                    else
                    {
                        if (order.OrderPlace == OrderPlace.Delayed)
                        {
                            inStroe.Checked = true;
                        }
                        else
                        {
                            if (order.OrderPlace == OrderPlace.CompletelyReturned)
                            {
                                agentOrderStatus.Text = "الشحنة مرتجعة مع المندوب ";
                                companyOrderStatus.Text = "الشحنة مرتجعة داخل الشركة";
                                finalOrderStatus.Text = "مرتجع تم التسليم";
                                if (order.MoneyPlace == MoneyPalce.WithAgent)
                                {
                                    agentOrderStatus.Checked = true;
                                }
                                else if (order.MoneyPlace == MoneyPalce.InsideCompany)
                                {
                                    companyOrderStatus.Checked = true;
                                }
                                else
                                {
                                    finalOrderStatus.Checked = true;
                                }
                            }
                            else
                            {
                                if (order.OrderPlace == OrderPlace.Delivered)
                                {
                                    if (!order.IsClientDiliverdMoney)
                                    {
                                        if (order.MoneyPlace == MoneyPalce.WithAgent)
                                        {
                                            agentOrderStatus.Checked = true;
                                        }
                                        else
                                        {
                                            companyOrderStatus.Checked = true;
                                        }
                                    }
                                    else
                                    {
                                        if (order.ShouldToPay() - order.ClientPaied == 0)
                                        {
                                            finalOrderStatus.Checked = true;
                                        }
                                        else
                                        {
                                            agentOrderStatus.Text = "تم التسليم/ المبلغ مع المندوب/ هناك تغير في السعر";
                                            companyOrderStatus.Text = "المبلغ داخل الشركة/هناك تغير في السعر";
                                            if (order.MoneyPlace == MoneyPalce.WithAgent)
                                            {
                                                agentOrderStatus.Checked = true;
                                            }
                                            else
                                            {
                                                companyOrderStatus.Checked = true;
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    if (order.OrderPlace == OrderPlace.PartialReturned)
                                    {
                                        agentOrderStatus.Text = "تم تسليمها مرتجع جزئي/ المبلغ مع المندوب";
                                        companyOrderStatus.Text = "المبلغ داخل الشرحة  مرتجع جزئي";
                                        if (!order.IsClientDiliverdMoney)
                                        {
                                            if (order.MoneyPlace == MoneyPalce.WithAgent)
                                            {
                                                agentOrderStatus.Checked = true;
                                            }
                                            else
                                            {
                                                companyOrderStatus.Checked = true;
                                            }
                                        }
                                        else
                                        {
                                            if (order.ShouldToPay() - order.ClientPaied == 0)
                                            {
                                                finalOrderStatus.Checked = true;
                                            }
                                            else
                                            {
                                                // agentOrderStatus.Text = "تم التسليم/ المبلغ مع المندوب/ هناك تغير في السعر";
                                                // companyOrderStatus.Text = "المبلغ داخل الشركة/هناك تغير في السعر";
                                                if (order.MoneyPlace == MoneyPalce.WithAgent)
                                                {
                                                    agentOrderStatus.Checked = true;
                                                }
                                                else
                                                {
                                                    companyOrderStatus.Checked = true;
                                                }
                                            }

                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            return tracking;
        }

        public async Task<List<AccountReportDto>> AccountReport(DateRangeFilter dateRangeFilter)
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
                row = row.Replace("{{note}}", order.Note?.ToString());
                row = row.Replace("{{clientNote}}", order.ClientNote?.ToString());
                rows.AppendLine(row);
            }
            var totalOrderRowPath = _environment.WebRootPath + "/HtmlTemplate/ClientTemplate/OrderToPayReport/TotalOrderRow.html";
            var totalOrderRowTemplate = await File.ReadAllTextAsync(totalOrderRowPath);
            totalOrderRowTemplate = totalOrderRowTemplate.Replace("{{totalCount}}", data.Count().ToString());
            totalOrderRowTemplate = totalOrderRowTemplate.Replace("{{totalCost}}", data.Sum(c => c.Cost).ToString());
            totalOrderRowTemplate = totalOrderRowTemplate.Replace("{{totalDeliveryCost}}", data.Sum(c => c.DeliveryCost).ToString());
            totalOrderRowTemplate = totalOrderRowTemplate.Replace("{{total}}", data.Sum(c => c.Cost - c.DeliveryCost).ToString());
            rows.AppendLine(totalOrderRowTemplate);


            var htmlPagePath = _environment.WebRootPath + "/HtmlTemplate/ClientTemplate/OrderToPayReport/OrderToPayRepor.html";
            var htmlPage = await File.ReadAllTextAsync(htmlPagePath);
            htmlPage = htmlPage.Replace("{{orderplacedName}}", string.Join('-', orderDontFinishFilter.OrderPlacedId.Select(c => c.GetDescription())));
            var tableTemplatePath = _environment.WebRootPath + "/HtmlTemplate/ClientTemplate/OrderToPayReport/OrderToPayReporTable.html";
            var tabelTemplate = await File.ReadAllTextAsync(tableTemplatePath);
            tabelTemplate = tabelTemplate.Replace("{{orderRows}}", rows.ToString());
            var receipts = await _receiptRepository.GetAsync(c => c.ClientId == clientId && c.ClientPaymentId == null);
            var receiptTempatePath = _environment.WebRootPath + "/HtmlTemplate/ClientTemplate/OrderToPayReport/ReceiptRow.html";
            var receiptTempate = await File.ReadAllTextAsync(receiptTempatePath);
            rows.Clear();
            counter = 1;
            foreach (var receipt in receipts)
            {
                var row = receiptTempate.Replace("{{incremental}}", (counter++).ToString());
                row = row.Replace("{{number}}", receipt.Id.ToString());
                row = row.Replace("{{date}}", receipt.Date.ToString("yyyy-mm-dd"));
                row = row.Replace("{{type}}", receipt.IsPay?"قبض":"صرف");
                row = row.Replace("{{cost}}", Math.Abs(receipt.Amount).ToString());
                row = row.Replace("{{about}}", receipt.About);
                row = row.Replace("{{note}}", receipt.Note);
                rows.Append(row);
            }
            tabelTemplate = tabelTemplate.Replace("{{receiptRows}}", rows.ToString());

            htmlPage = htmlPage.Replace("{{table}}", tabelTemplate);
            return htmlPage;

        }

        public async Task<ClientOrderReportDto> GetOrderStaticsReport(DateRangeFilter dateRangeFilter)
        {
            var predicate = PredicateBuilder.New<Order>(true);
            if (dateRangeFilter != null)
            {
                if (dateRangeFilter.Start.HasValue)
                {
                    predicate = predicate.And(c => c.Date.Value.Date > dateRangeFilter.Start.Value.Date);
                }
                if (dateRangeFilter.End.HasValue)
                {
                    predicate = predicate.And(c => c.Date.Value.Date < dateRangeFilter.End.Value.Date);
                }
            }
            var orderCount = await _repository.Count(predicate);
            var returndOrderPredicate = PredicateBuilder.New<Order>(predicate).And(c => c.OrderPlace == OrderPlace.CompletelyReturned && c.OrderState == OrderState.Finished);
            var returnOrderCount = await _repository.Count(returndOrderPredicate);
            var delviverOrderPredicate = PredicateBuilder.New<Order>(predicate).And(c => (c.OrderPlace == OrderPlace.PartialReturned || c.OrderPlace == OrderPlace.Delivered) && c.OrderState == OrderState.Finished);
            var delviverOrderCount = await _repository.Count(delviverOrderPredicate);
            var devlierdCountriesCount = await _repository.CountGroupBy(c => c.CountryId, delviverOrderPredicate);
            var requestedCountriesCount = await _repository.CountGroupBy(c => c.CountryId, predicate);
            var returnConutriesCount = await _repository.CountGroupBy(c => c.CountryId, returndOrderPredicate);

            var highestDeliveredCountryMapId = devlierdCountriesCount.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            var highestRequestedCountryMapId = requestedCountriesCount.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            var highestReturnedCountryMapId = returnConutriesCount.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            var deliveredOrderRatio = Convert.ToDecimal(returnOrderCount * 100) / Convert.ToDecimal(orderCount);
            var returnOrderRatio = Convert.ToDecimal(delviverOrderCount * 100) / Convert.ToDecimal(orderCount);
            return new ClientOrderReportDto()
            {
                DeliveredOrderRatio = deliveredOrderRatio,
                ReturnOrderRatio = returnOrderRatio,
                HighestDeliveredCountryMapId = highestDeliveredCountryMapId,
                HighestRequestedCountryMapId = highestRequestedCountryMapId,
                HighestReturnedCountryMapId = highestReturnedCountryMapId
            };
        }
    }
}