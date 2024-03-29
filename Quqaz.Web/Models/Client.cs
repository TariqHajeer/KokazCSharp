using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class Client : IHaveBranch, IIndex
    {
        public Client()
        {
            ClientPhones = new HashSet<ClientPhone>();
            DisAcceptOrders = new HashSet<DisAcceptOrder>();
            EditRequests = new HashSet<EditRequest>();
            Markets = new HashSet<Market>();
            Notfications = new HashSet<Notfication>();
            OrderFromExcels = new HashSet<OrderFromExcel>();
            OrderLogs = new HashSet<OrderLog>();
            Orders = new HashSet<Order>();
            PaymentRequests = new HashSet<PaymentRequest>();
            ReceiptOfTheOrderStatusDetalis = new HashSet<ReceiptOfTheOrderStatusDetali>();
            Receipts = new HashSet<Receipt>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CountryId { get; set; }
        public string Address { get; set; }
        public DateTime FirstDate { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public string Mail { get; set; }
        public int Points { get; set; }
        public int BranchId { get; set; }
        public string Photo { get; set; }
        public string FacebookLinke { get; set; }
        public string IGLink { get; set; }
        public virtual Country Country { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ClientPhone> ClientPhones { get; set; }
        public virtual ICollection<DisAcceptOrder> DisAcceptOrders { get; set; }
        public virtual ICollection<EditRequest> EditRequests { get; set; }
        public virtual ICollection<Market> Markets { get; set; }
        public virtual ICollection<Notfication> Notfications { get; set; }
        public virtual ICollection<OrderFromExcel> OrderFromExcels { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PaymentRequest> PaymentRequests { get; set; }
        public virtual ICollection<ReceiptOfTheOrderStatusDetali> ReceiptOfTheOrderStatusDetalis { get; set; }
        public virtual ICollection<Receipt> Receipts { get; set; }
        public virtual ICollection<FCMTokens> FCMTokens { get; set; }
        public Branch Branch { get; set; }
    }
}
