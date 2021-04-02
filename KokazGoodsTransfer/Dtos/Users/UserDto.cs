using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Common;
using KokazGoodsTransfer.Dtos.DepartmentDtos;
namespace KokazGoodsTransfer.Dtos.Users
{
    public class UserDto
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Experince { get; set; }
        public string Address { get; set; }
        public DateTime HireDate { get; set; }
        public string Note { get; set; }
        public bool CanWorkAsAgent { get; set; }
        public int? CountryId { get; set; }
        public decimal Salary { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int[] GroupsId { get; set; }
        public PhoneDto[] Phones { get; set; }
        public UserStatics UserStatics { get; set; }
    }
    public class UserStatics
    {
        public int OrderInStore { get; set; }
        public int OrderInWay { get; set; }
    }

}
