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
            int id = 1;
            builder.HasMany(c => c.FromCountries).WithOne(c => c.FromCountry).HasForeignKey(c => c.FromCountryId);
            builder.HasMany(c => c.ToCountries).WithOne(c => c.ToCountry).HasForeignKey(c => c.ToCountryId);
            builder.HasMany(c => c.MediatorCountries).WithOne(c => c.MidCountry).HasForeignKey(c => c.MediatorCountryId);
            var countreis = new List<Country> {
                new Country()
                {
                    Id = id++,
                    Name = "مدينة1",
                    DeliveryCost = 10000,
                    IsMain = true,
                    Points = 15
                },
            new Country()
            {
                Id= id++,
                Name="مدينة2",
                DeliveryCost =20000,
                IsMain =false,
                Points=20
            },
            new Country()
            {
                Id= id++,
                Name="مدينة3 (وسيطة) ",
                DeliveryCost =20000,
                IsMain =false,
                Points=20
            }
            ,
            new Country()
            {
                Id= id ++,
                Name="مدينة 4",
                DeliveryCost =20000,
                IsMain =false,
                Points=20
            }
            ,
            new Country()
            {
                Id= id ++,
                Name="مدينة 5",
                DeliveryCost =20000,
                IsMain =false,
                Points=20
            }
            ,
            new Country()
            {
                Id= id ++,
                Name="مدينة 6",
                DeliveryCost =20000,
                IsMain =false,
                Points=20
            }
            };
            builder.HasData(countreis);
        }
    }
}
