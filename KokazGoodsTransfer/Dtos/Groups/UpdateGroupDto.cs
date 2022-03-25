using KokazGoodsTransfer.Models.Infrastrcuter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Groups
{
    public class UpdateGroupDto : IIndex
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] Privileges { get; set; }
    }
}
