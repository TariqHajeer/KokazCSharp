using System.Collections.Generic;

namespace Quqaz.Web.Dtos.BranchDtos
{
    public class BranchPricesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public List<BranchPriceDto> Prices { get; set; }
    }
}
