using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Linq;

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
            int bagdadId = 5;
            int arbile = 8;
            int dhok = 7;
            var branchesIDs = new int[] { 4, 8, 3 };
            var countryNeedMid = new int[] { 10, 12, 13, 14, 18, 20, 23, 16, 26, 22, 15, 49 };
            var data = new List<MediatorCountry>();
            foreach (var item in branchesIDs)
            {
                data.AddRange(countryNeedMid.Select(c => new MediatorCountry()
                {
                    FromCountryId = item,
                    MediatorCountryId = bagdadId,
                    ToCountryId = c
                }));
            }
            for (int i = 1; i <= 49; i++)
            {
                if (i == dhok || i == arbile || branchesIDs.Contains(i) || i == bagdadId)
                    continue;
                data.Add(new MediatorCountry()
                {
                    FromCountryId = dhok,
                    MediatorCountryId = arbile,
                    ToCountryId = i
                });
            }

            builder.HasData(data);

        }
    }
}
