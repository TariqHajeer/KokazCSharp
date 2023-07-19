using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Groups
{
    public class CreateGroupDto : INameEntity
    {
        public string Name { set; get; }
        public int[] PrivilegesId { get; set; }
    }
}
