using Quqaz.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Quqaz.Web.TypeConfiguration
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
