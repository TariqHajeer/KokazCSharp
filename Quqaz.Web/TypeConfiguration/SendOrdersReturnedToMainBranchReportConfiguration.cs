using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quqaz.Web.Models.SendOrdersReturnedToMainBranchModels;

namespace Quqaz.Web.TypeConfiguration
{
    public class SendOrdersReturnedToMainBranchReportConfiguration : IEntityTypeConfiguration<SendOrdersReturnedToMainBranchReport>
    {
        public void Configure(EntityTypeBuilder<SendOrdersReturnedToMainBranchReport> builder)
        {
            builder.HasOne(c => c.Branch)
                .WithMany().OnDelete(DeleteBehavior.Restrict);
        }
    }
}
