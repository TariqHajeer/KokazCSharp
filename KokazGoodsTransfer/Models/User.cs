using System;
using System.Collections.Generic;

#nullable disable

namespace KokazGoodsTransfer.Models
{
    public partial class User
    {
        public User()
        {
            AgentCountrs = new HashSet<AgentCountr>();
            Clients = new HashSet<Client>();
            Incomes = new HashSet<Income>();
            OrderLogs = new HashSet<OrderLog>();
            Orders = new HashSet<Order>();
            OutComes = new HashSet<OutCome>();
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

        public virtual ICollection<AgentCountr> AgentCountrs { get; set; }
        public virtual ICollection<Client> Clients { get; set; }
        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<OrderLog> OrderLogs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<OutCome> OutComes { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
        public virtual ICollection<UserPhone> UserPhones { get; set; }
    }
}
