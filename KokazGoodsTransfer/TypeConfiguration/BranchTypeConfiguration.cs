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

            var bl = new List<Branch>()
            {
                new Branch()
            {
                Id = 1,
                Name = "الفرع الرئيسي",
                CountryId = 1,

            },
                    new Branch()
            {
                Id = 2,
                Name = "الفرع الثاني",
                CountryId = 2,

            },
                    new Branch()
                    {
                        Id = 3,
                        Name="الفرع الثالث (وسيط)",
                        CountryId = 3,
                    },
                    new Branch() {
                        Id= 4,
                        Name="الفرع الرابع",
                        CountryId =4
                    }
            };
            builder.HasData(bl);
            builder.HasMany(c => c.FromBranches).WithOne(c => c.FromBranch).HasForeignKey(c => c.FromBranchId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.ToBranches).WithOne(c => c.ToBranch).HasForeignKey(c => c.ToBranchId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.MediatorBranches).WithOne(c => c.MidBranch).HasForeignKey(c => c.MediatorBranchId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
