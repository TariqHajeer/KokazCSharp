using Quqaz.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Quqaz.Web.TypeConfiguration
{
    public class OrderTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(c => c.CurrentBranch)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
        }
    }
}
