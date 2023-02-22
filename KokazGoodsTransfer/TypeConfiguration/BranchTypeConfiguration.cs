using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class BranchTypeConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {

            builder.HasKey(x => x.Id);
            builder.Property(c => c.Name).IsUnicode(true);
            builder.Property(c=>c.Name).IsRequired().IsUnicode(true).HasMaxLength(50);
            var bl = new List<Branch>()
            {
                new Branch()
            {
                Id = 1,
                Name = "الفرع الرئيسي",

            },
                    new Branch()
            {
                Id = 2,
                Name = "الفرع الثاني",

            },
                    new Branch()
            {
                Id = 3,
                Name = "الفرع الثالث الوسيط",

            },
               new Branch()
               {
                   Id=4,
                   Name="فرع 4",
               }
            };
            builder.HasData(bl);

        }
    }
}
