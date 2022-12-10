﻿using KokazGoodsTransfer.Dtos.Clients;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.Countries;
using KokazGoodsTransfer.Dtos.Regions;
using KokazGoodsTransfer.Dtos.Users;
using System;
using System.Collections.Generic;

namespace KokazGoodsTransfer.Dtos.OrdersDtos
{
    public class OrderDto
    {

        public int Id { get; set; }
        public string Code { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Cost { get; set; }
        public decimal AgentCost { get; set; }
        public decimal? OldCost { get; set; }
        public string RecipientName { get; set; }
        public string RecipientPhones { get; set; }
        public string Address { get; set; }
        public string ClientNote { get; set; }
        public string CreatedBy { get; set; }
        public bool Seen { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DiliveryDate { get; set; }
        public string Note { get; set; }
        public bool IsClientDiliverdMoney { get; set; }
        public int? AgentPrintNumber { get; set; }
        public int? ClientPrintNumber { get; set; }
        public int OrderStateId { get; set; }
        public string CanResned { get; set; }
        public decimal? OldDeliveryCost { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsSend { get; set; }
        public ClientDto Client { get; set; }
        public CountryDto Country { get; set; }
        public CountryDto NextCountryDto { get; set; }
        public RegionDto Region { get; set; }
        public NameAndIdDto MonePlaced { get; set; }
        public NameAndIdDto Orderplaced { get; set; }
        public UserDto Agent { get; set; }
        public List<ResponseOrderItemDto> OrderItems { get; set; }
        public List<OrderLogDto> OrderLogs { get; set; }
        public List<PrintOrdersDto> AgentPrint { get; set; }
        public List<PrintOrdersDto> ClientPrint { get; set; }
        public List<CountryDto> Path { get; set; }
        public int? CurrentCountry { get; set; }
        public int? PrintedTimes { get; set; }
        public int AgentRequestStatus { get; set; }
        public string BranchName { get; set; }
        public List<ReceiptOfTheOrderStatusDetaliOrderDto> ReceiptOfTheOrderStatusDetalis { get; set; }
    }
}
