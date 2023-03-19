using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Users
{
    public class CreateUserDto
    {

        public string Name { get; set; }
        //public int DepartmentId { get; set; }
        public string Experince { get; set; }
        public string Address { get; set; }
        public DateTime HireDate { get; set; }
        public string Note { get; set; }
        public bool CanWorkAsAgent { get; set; }
        //public int? CountryId { get; set; }
        public int[] Countries { get; set; }
        public decimal? Salary { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int[] GroupsId { get; set; }
        public string[] Phones { get; set; }
        public int[] BranchesIds { get; set; }
    }
}
