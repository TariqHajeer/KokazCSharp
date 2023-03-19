using Quqaz.Web.Dtos.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.EditRequestDtos
{
    public class EditRequestDto
    {
        public int Id { get; set; }
        public string OldName { get; set; }
        public string NewName { get; set; }
        public string OldUserName { get; set; }
        public string NewUserName { get; set; }
        public ClientDto Client { get; set; }
    }
}
