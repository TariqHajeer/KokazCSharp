using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class ReceiptOfTheOrderStatusDetaliConfiguration : IEntityTypeConfiguration<ReceiptOfTheOrderStatusDetali>
    {
        public void Configure(EntityTypeBuilder<ReceiptOfTheOrderStatusDetali> builder)
        {
            builder.HasIndex(c => c.OrderState);
            builder.HasIndex(c => c.MoneyPalce);
            builder.HasIndex(c => c.OrderPlace);
        }
    }
}
