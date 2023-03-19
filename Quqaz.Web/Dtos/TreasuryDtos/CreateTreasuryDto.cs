using System;

namespace Quqaz.Web.Dtos.TreasuryDtos
{
    public class CreateTreasuryDto
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
