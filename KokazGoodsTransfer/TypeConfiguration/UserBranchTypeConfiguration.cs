using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class UserBranchTypeConfiguration : IEntityTypeConfiguration<UserBranch>
    {
        public void Configure(EntityTypeBuilder<UserBranch> builder)
        {
            builder.HasKey(c => new { c.UserId, c.BranchId });
            var branchesIds = new List<int>() { 4, 8, 3, 5, 7 };
            var ubl = new List<UserBranch>();
            foreach (var item in branchesIds)
            {
                ubl.Add(new UserBranch()
                {
                    UserId = 1,
                    BranchId = item
                });
            }
            builder.HasData(ubl);
        }
    }
}
