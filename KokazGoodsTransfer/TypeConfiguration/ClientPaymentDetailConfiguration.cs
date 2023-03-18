using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class ClientPaymentDetailConfiguration : IEntityTypeConfiguration<ClientPaymentDetail>
    {
        public void Configure(EntityTypeBuilder<ClientPaymentDetail> builder)
        {
            builder.HasIndex(c => c.MoneyPlace);
            builder.HasIndex(c => c.OrderPlace);
        }
    }
}
