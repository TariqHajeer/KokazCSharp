﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Users
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public int DepartmentId { get; set; }
        public string Experince { get; set; }
        public string Address { get; set; }
        public DateTime HireDate { get; set; }
        public string Note { get; set; }
        public bool CanWorkAsAgent { get; set; }
        public int[] Countries { get; set; }
        //public int? CountryId { get; set; }
        public decimal Salary { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }
}
