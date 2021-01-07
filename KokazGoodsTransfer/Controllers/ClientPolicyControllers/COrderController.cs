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

            int? regionId = null;
            if (CodeExist(createOrderFromClient.Code))
            {
                return Conflict();
            }
            if (createOrderFromClient.RegionId == null)
            {
                if (createOrderFromClient.RegioName != "")
                {
                    if (this.Context.Regions.Where(c => c.CountryId == createOrderFromClient.CountryId && c.Name.Equals(createOrderFromClient.RegioName, StringComparison.OrdinalIgnoreCase)).Any())
                    {
                        return Conflict();
                    }
                    var Region = new Region()
                    {
                        Name = createOrderFromClient.RegioName,
                        CountryId = createOrderFromClient.CountryId
                    };
                    this.Context.Add(Region);
                    regionId = Region.Id;
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
            order.MoenyPlacedId =(int) MoneyPalcedEnum.OutSideCompany;
            order.OrderplacedId = (int)OrderplacedEnum.Client;

            var orderItem = createOrderFromClient.OrderItem;
            foreach (var item in orderItem)
            {
                int orderTypeId;
                if (item.OrderTypeId == null)
                {
                    if (item.OrderTypeName == "")
                        return Conflict();
                    var similerOrderType = this.Context.OrderTypes.Where(c => c.Name.Equals(item.OrderTypeName,StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (similerOrderType == null)
                    {
                        var orderType = new OrderType()
                        {
                            Name = item.OrderTypeName,
                        };
                        this.Context.Add(orderType);
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
                order.OrderItems.Add(new OrderItem()
                {
                    OrderId = orderTypeId,
                    Count = item.Count
                });
            }
            this.Context.Add(order);
            this.Context.SaveChanges();
            return Ok();
        }
        [HttpGet("codeExist")]
        public IActionResult CheckCodeExist(string code)
        {
            return Ok(CodeExist(code));
        }
        bool CodeExist(string code)
        {
            if (this.Context.Orders.Where(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase)).Any())
            {
                return true;
            }
            return false;
        }
        [HttpGet]
        public IActionResult Get()
        {
            this.Context.Orders.ToList();
            return Ok();
        }
    }
}