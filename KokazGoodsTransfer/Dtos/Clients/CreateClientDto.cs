using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KokazGoodsTransfer.Dtos.Clients
{
    public class CreateClientDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]  
        public string Password { get; set; }
        public int? Countryid { get; set; }
        public string Address { get; set; }
        public DateTime FirstDate { get; set; }
        public string Note { get; set; }
        
        public string[] Phones { get; set; }
        public string Mail { get; set; }
        public int UserId { get; set; } =1;

    }
}
