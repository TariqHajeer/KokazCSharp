using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;
namespace KokazGoodsTransfer.TypeConfiguration
{
    public class BranchToCountryDeliverryCostConfiguration : IEntityTypeConfiguration<BranchToCountryDeliverryCost>
    {
        public void Configure(EntityTypeBuilder<BranchToCountryDeliverryCost> builder)
        {
            builder.HasKey(c => new { c.BranchId, c.CountryId });
            builder.Property(c => c.DeliveryCost).HasColumnType("decimal(6,0)");
            builder.HasOne(c => c.Branch)
                .WithMany(c => c.BranchToCountryDeliverryCosts)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.Country)
                .WithMany(c => c.BranchToCountryDeliverryCosts)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(c => c.BranchId);
            builder.HasIndex(c => c.CountryId);
            builder.Property(c => c.Points).HasColumnType("SMALLINT");
            var branchesIds = new int[] { 3, 4, 5, 7, 8 };
            var data = (new int[] { 3, 4, 5, 7, 8 }).SelectMany(b => Enumerable.Range(1, 49).Select(c => new BranchToCountryDeliverryCost()
            {
                BranchId = b,
                CountryId =c,
                DeliveryCost =5000,
                Points =20
            }));
            builder.HasData(data);
        }
    }
}
