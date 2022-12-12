using KokazGoodsTransfer.Models.TransferToBranchModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class TransferToOtherBranchTypeConfiguration : IEntityTypeConfiguration<TransferToOtherBranch>
    {
        public void Configure(EntityTypeBuilder<TransferToOtherBranch> builder)
        {
            builder.HasOne(c => c.SourceBranch)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);
            builder.HasOne(c => c.DestinationBranch)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(true);

        }
    }
}
