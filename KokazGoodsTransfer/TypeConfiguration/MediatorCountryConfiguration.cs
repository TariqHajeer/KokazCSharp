using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class MediatorCountryConfiguration : IEntityTypeConfiguration<MediatorCountry>
    {
        public void Configure(EntityTypeBuilder<MediatorCountry> builder)
        {
            builder.HasOne(c => c.FromCountry)
                .WithMany(c => c.FromCountries).HasForeignKey(c => c.FromCountryId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.MidCountry)
                .WithMany(c => c.MediatorCountries).HasForeignKey(c => c.MediatorCountryId).OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.ToCountry)
                .WithMany(c => c.ToCountries).HasForeignKey(c => c.ToCountryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasKey(c => new { c.FromCountryId, c.ToCountryId });
            int mid = 3;
            var data = new List<MediatorCountry>()
            {
                new MediatorCountry()
                {
                    FromCountryId = 1,
                    ToCountryId =2,
                    MediatorCountryId= mid,
                },
                new MediatorCountry()
                {
                    FromCountryId =2,
                    ToCountryId =1,
                    MediatorCountryId= mid,

                },
                new MediatorCountry()
                {
                    FromCountryId=1,
                    ToCountryId=4,
                    MediatorCountryId= mid,
                },
                new MediatorCountry()
                {
                    FromCountryId=4,
                    ToCountryId=1,
                    MediatorCountryId= mid,
                },
                new MediatorCountry()
                {
                    FromCountryId=1,
                    ToCountryId=5,
                    MediatorCountryId= mid,
                },
                new MediatorCountry()
                {
                    FromCountryId=2,
                    ToCountryId=5,
                    MediatorCountryId= mid,
                }
            };
            builder.HasData(data);

        }
    }
}
