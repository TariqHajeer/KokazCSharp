using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.IncomeTypes
{
    public class IncomeTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; }
    }
}
