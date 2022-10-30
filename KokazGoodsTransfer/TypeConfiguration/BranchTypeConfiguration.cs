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
            };
            builder.HasData(bl);

        }
    }
}
