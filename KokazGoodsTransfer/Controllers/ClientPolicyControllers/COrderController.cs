using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.OrdersDtos;
using KokazGoodsTransfer.Models;
using KokazGoodsTransfer.Models.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KokazGoodsTransfer.Controllers.ClientPolicyControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class COrderController : AbstractClientPolicyController
    {
        public COrderController(KokazContext context, IMapper mapper) : base(context, mapper)
        {
        }
        private List<string> Validate(CreateOrderFromClient createOrderFromClient)
        {
            List<string> erros = new List<string>();
            if (CodeExist(createOrderFromClient.Code))
            {
                erros.Add("الكود موجود مسبقاً");
            }
            if (this.Context.Countries.Find(createOrderFromClient.CountryId) == null)
            {
                erros.Add("المدينة غير موجودة");
            }
            if (createOrderFromClient.RegionId != null)
            {
                var region = this.Context.Regions.Find(createOrderFromClient.RegionId);
                if (region == null || region.CountryId != createOrderFromClient.CountryId)
                {
                    erros.Add("المنطقة غير موجودة");
                }
            }
            if (createOrderFromClient.RecipientPhones.Length == 0)
            {
                erros.Add("رقم الهاتف مطلوب");
            }
            if (createOrderFromClient.OrderItem.Count > 0)
            {
                foreach (var item in createOrderFromClient.OrderItem)
                {
                    if (item.OrderTypeId != null)
                    {
                        var orderType = this.Context.OrderTypes.Find(item.OrderTypeId);
                        if (orderType == null)
                        {
                            erros.Add("النوع غير موجود");
                            break;
                        }
                    }
                }
            }
            return erros;
        }
        [HttpPost]
        public IActionResult Create(CreateOrderFromClient createOrderFromClient)
        {
            var dbTransacrion = this.Context.Database.BeginTransaction();
            try
            {
                var validate = this.Validate(createOrderFromClient);
                if (validate.Count != 0)
                {
                    return Conflict(new { messages = validate });
                }
                int? regionId = null;
                //if (CodeExist(createOrderFromClient.Code))
                //{
                //    return Conflict();
                //}
                if (createOrderFromClient.RegionId == null)
                {
                    if (createOrderFromClient.RegioName != "")
                    {
                        var similerRegion = this.Context.Regions.Where(c => c.CountryId == createOrderFromClient.CountryId && c.Name == createOrderFromClient.RegioName).FirstOrDefault();
                        if (similerRegion != null)
                        {
                            regionId = similerRegion.Id;
                        }
                        else
                        {
                            var region = new Region()
                            {
                                Name = createOrderFromClient.RegioName,
                                CountryId = createOrderFromClient.CountryId
                            };
                            this.Context.Add(region);
                            this.Context.SaveChanges();
                            regionId = region.Id;
                        }
                    }
                }
                else
                {
                    regionId = createOrderFromClient.RegionId;
                }

                var country = this.Context.Countries.Find(createOrderFromClient.CountryId);

                //this.Context.Entry(country).Collection(c => c.Users).Load();
                var order = mapper.Map<Order>(createOrderFromClient);
                order.ClientId = AuthoticateUserId();
                order.CreatedBy = AuthoticateUserName();
                order.RegionId = regionId;
                order.DeliveryCost = country.DeliveryCost;
                order.CreatedBy = AuthoticateUserName();
                order.MoenyPlacedId = (int)MoneyPalcedEnum.OutSideCompany;
                order.OrderplacedId = (int)OrderplacedEnum.Client;
                this.Context.Add(order);
                this.Context.SaveChanges();
                var orderItem = createOrderFromClient.OrderItem;
                foreach (var item in orderItem)
                {
                    int orderTypeId;
                    if (item.OrderTypeId == null)
                    {
                        if (item.OrderTypeName == "")
                            return Conflict();
                        var similerOrderType = this.Context.OrderTypes.Where(c => c.Name == item.OrderTypeName).FirstOrDefault();
                        if (similerOrderType == null)
                        {
                            var orderType = new OrderType()
                            {
                                Name = item.OrderTypeName,
                            };
                            this.Context.Add(orderType);
                            this.Context.SaveChanges();
                            orderTypeId = orderType.Id;

                        }
                        else
                        {
                            orderTypeId = similerOrderType.Id;
                        }
                    }
                    else
                    {
                        orderTypeId = (int)item.OrderTypeId;
                    }
                    this.Context.Add(new OrderItem()
                    {
                        OrderTpyeId = orderTypeId,
                        Count = item.Count,
                        OrderId = order.Id
                    });
                    this.Context.SaveChanges();
                }
                dbTransacrion.Commit();
                return Ok(mapper.Map<OrderResponseClientDto>(order));
            }

            catch (Exception ex)
            {
                dbTransacrion.Rollback();
                return BadRequest();

            }
        }
        [HttpGet("codeExist")]
        public IActionResult CheckCodeExist(string code)
        {
            return Ok(CodeExist(code));
        }
        bool CodeExist(string code)
        {
            if (this.Context.Orders.Where(c => c.Code == code).Any())
            {
                return true;
            }
            return false;
        }
        [HttpGet]
        
        public IActionResult Get([FromQuery] PagingDto pagingDto, [FromQuery]COrderFilter orderFilter)
        {
            var orderIQ = this.Context.Orders
                .Where(c => c.ClientId == AuthoticateUserId());
            if (orderFilter.CountryId != null)
            {
                orderIQ = orderIQ.Where(c => c.CountryId == orderFilter.CountryId);
            }
            if (orderFilter.Code != string.Empty && orderFilter.Code != null)
            {
                orderIQ = orderIQ.Where(c => c.Code.StartsWith(orderFilter.Code));
            }
            
            if (orderFilter.RegionId != null)
            {
                orderIQ = orderIQ.Where(c => c.RegionId == orderFilter.RegionId);
            }
            if (orderFilter.RecipientName != string.Empty && orderFilter.RecipientName != null)
            {
                orderIQ = orderIQ.Where(c => c.RecipientName.StartsWith(orderFilter.RecipientName));
            }
            if (orderFilter.MonePlacedId != null)
            {
                orderIQ = orderIQ.Where(c => c.MoenyPlacedId == orderFilter.MonePlacedId);
            }
            if (orderFilter.OrderplacedId != null)
            {
                orderIQ = orderIQ.Where(c => c.OrderplacedId == orderFilter.OrderplacedId);
            }
            if (orderFilter.Phone != string.Empty && orderFilter.Phone != null)
            {
                orderIQ = orderIQ.Where(c => c.RecipientPhones.Contains(orderFilter.Phone));
            }
            if (orderFilter.IsClientDiliverdMoney != null)
            {
                orderIQ = orderIQ.Where(c => c.IsClientDiliverdMoney == orderFilter.IsClientDiliverdMoney);
            }
            if (orderFilter.ClientPrintNumber != null)
            {
                orderIQ = orderIQ.Where(c => c.OrderPrints.Any(op => op.Print.PrintNmber == orderFilter.ClientPrintNumber && op.Print.Type == PrintType.Client));
            }
            var total = orderIQ.Count();
            var orders = orderIQ.Skip((pagingDto.Page - 1) * pagingDto.RowCount).Take(pagingDto.RowCount)
                .Include(c => c.Region)
                .Include(c => c.Country)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.OrderItems)
                    .ThenInclude(c => c.OrderTpye)
                .ToList();
            return Ok(new { data = mapper.Map<OrderDto[]>(orders), total });
        }
            
    }
}