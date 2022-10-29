using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class TreasuryTypeConfiguration : IEntityTypeConfiguration<Treasury>
    {
        public void Configure(EntityTypeBuilder<Treasury> builder)
        {
            var t = new Treasury()
            {
                Id = 1,
                IsActive =true,
                CreateOnUtc= new System.DateTime(2020, 1, 1).ToUniversalTime(),
            };
            builder.HasData(t);
        }
    }
}
