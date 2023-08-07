using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Users
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Experince { get; set; }
        public string Address { get; set; }
        public DateTime HireDate { get; set; }
        public string Note { get; set; }
        public bool CanWorkAsAgent { get; set; }
        public int[] Countries { get; set; }
        public IEnumerable<int> BranchesIds { get; set; }
        public decimal Salary { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
