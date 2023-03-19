using Quqaz.Web.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.Groups
{
    public class UpdateGroupDto : IIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] Privileges { get; set; }
    }
}
