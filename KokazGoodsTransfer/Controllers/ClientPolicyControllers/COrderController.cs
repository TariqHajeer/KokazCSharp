using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        [HttpPost]
        public IActionResult Create(CreateOrderFromClient createOrderFromClient)
        {
            var dbTransacrion = this.Context.Database.BeginTransaction();
            try
            {
                
                int? regionId = null;
                if (CodeExist(createOrderFromClient.Code))
                {
                    return Conflict();
                }
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
                return Ok(mapper.Map<OrderTypeResponseClientDto>(order));
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
        public IActionResult Get()
        {
            var orders = this.Context.Orders
                .Include(c => c.Region)
                .Include(c => c.Country)
                .Include(c => c.MoenyPlaced)
                .Include(c => c.Orderplaced)
                .Include(c => c.MoenyPlaced)
                .Where(c => c.ClientId == AuthoticateUserId())
                .ToList();
            return Ok(mapper.Map<OrderTypeResponseClientDto[]>(orders));
        }
    }
}