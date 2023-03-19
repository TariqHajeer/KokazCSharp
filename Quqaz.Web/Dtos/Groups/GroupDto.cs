using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Groups
{
    public class GroupDto : IIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> PrivilegesId { get; set; }
        public string[] Users { get; set; }
    }
}
