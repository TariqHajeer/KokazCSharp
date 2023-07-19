using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Currencies
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDelete { get; set; }
    }
}
