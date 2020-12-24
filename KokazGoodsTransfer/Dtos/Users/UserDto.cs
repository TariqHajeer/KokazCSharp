using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KokazGoodsTransfer.Dtos.Department;
namespace KokazGoodsTransfer.Dtos.Users
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DepartmentDto Department { set; get; }
        public string UserName { get; set; }
        public bool CanWorkAsAgent { get; set; }
    }
}
