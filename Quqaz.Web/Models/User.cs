using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Quqaz.Web.Models
{
    public partial class User: IMaybeHaveBranch, IIndex
    {
        public User()
        {
            AgentCountries = new HashSet<AgentCountry>();
            Clients = new HashSet<Client>();
            EditRequests = new HashSet<EditRequest>();
            Incomes = new HashSet<Income>();
            OrderLogs = new HashSet<OrderLog>();
            Orders = new HashSet<Order>();
            OutComes = new HashSet<OutCome>();
            ReceiptOfTheOrderStatusDetalis = new HashSet<ReceiptOfTheOrderStatusDetali>();
            ReceiptOfTheOrderStatuses = new HashSet<ReceiptOfTheOrderStatus>();
            UserGroups = new HashSet<UserGroup>();
            UserPhones = new HashSet<UserPhone>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Experince { get; set; }
        public string Adress { get; set; }
        public DateTime HireDate { get; set; }
        public string Note { get; set; }
        public bool CanWorkAsAgent { get; set; }
        public decimal? Salary { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public virtual Treasury Treasury { get; set; }
        public virtual ICollection<AgentCountry> AgentCountries { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<EditRequest> EditRequests { get; set; }
        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<OutCome> OutComes { get; set; }
        public virtual ICollection<ReceiptOfTheOrderStatusDetali> ReceiptOfTheOrderStatusDetalis { get; set; }
        public virtual ICollection<ReceiptOfTheOrderStatus> ReceiptOfTheOrderStatuses { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<UserPhone> UserPhones { get; set; }
        public virtual ICollection<UserBranch> Branches { get; set; }
        public int? BranchId { get ; set ; }
        public Branch Branch { get; set; }
    }
}
