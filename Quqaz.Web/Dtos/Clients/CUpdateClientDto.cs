using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Clients
{
    public class CUpdateClientDto
    {
        public string Mail { get; set; }
        public string Password { get; set; }
        public string[] Phones { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
    }
}
