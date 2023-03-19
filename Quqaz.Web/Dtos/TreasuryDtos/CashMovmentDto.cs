using System;

namespace Quqaz.Web.Dtos.TreasuryDtos
{
    public class CashMovmentDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string TreasuryUserName { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedBy { get; set; }
        public string Note { get; set; }
    }
}
