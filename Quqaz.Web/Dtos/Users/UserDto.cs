using System;
using Quqaz.Web.Dtos.Common;
using Quqaz.Web.Dtos.Countries;
namespace Quqaz.Web.Dtos.Users
{
    public class UserDto
    {
        public UserDto()
        {
            this.UserStatics = new UserStatics();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Experince { get; set; }
        public string Address { get; set; }
        public DateTime HireDate { get; set; }
        public string Note { get; set; }
        public bool CanWorkAsAgent { get; set; }
        public NameAndIdDto[] Countries { get; set; }
        public decimal Salary { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int[] GroupsId { get; set; }
        public bool IsActive { get; set; }
        public int[] BranchesIds { get; set; }
        public PhoneDto[] Phones { get; set; }
        public UserStatics UserStatics { get; set; }

    }
    public class UserStatics
    {
        public int OrderInStore { get; set; }
        public int OrderInWay { get; set; }

    }

}
