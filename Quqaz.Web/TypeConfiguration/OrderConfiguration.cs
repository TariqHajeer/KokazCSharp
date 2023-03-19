using Quqaz.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Quqaz.Web.TypeConfiguration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(c => c.OrderPlace);
            builder.HasIndex(c => c.MoneyPlace);
            builder.Property(c => c.OrderState).IsRequired();
            builder.HasIndex(c => c.OrderState);

        }
    }
}
