using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quqaz.Web.Dtos.AgentDtos
{
    public class AgentOrderStateDto
    {
        public int Id { get; set; }
        public decimal Cost { get; set; }
        public int OrderplacedId { get; set; }
        public decimal AgentCost { get; set; }
    }
}
