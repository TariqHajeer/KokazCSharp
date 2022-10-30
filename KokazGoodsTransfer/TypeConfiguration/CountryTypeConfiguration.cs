using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class CountryTypeConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            var countreis = new List<Country> {
                new Country()
                {
                    Id = 1,
                    Name = "مدينة1",
                    DeliveryCost = 10000,
                    IsMain = true,
                    Points = 15
                },
            new Country()
            {
                Id= 2,
                Name="مدينة2",
                DeliveryCost =20000,
                IsMain =false,
                Points=20
            }
            };
            builder.HasData(countreis);
        }
    }
}
