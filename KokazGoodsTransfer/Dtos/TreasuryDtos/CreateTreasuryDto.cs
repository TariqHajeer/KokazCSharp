using System;

namespace KokazGoodsTransfer.Dtos.TreasuryDtos
{
    public class CreateTreasuryDto
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}
