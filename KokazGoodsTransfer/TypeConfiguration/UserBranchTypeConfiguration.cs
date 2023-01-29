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
            var ubl = new List<UserBranch>()
            {
                new UserBranch()
                {
                    UserId=1,
                    BranchId=1
                },
                new UserBranch()
                {
                    UserId=1,
                    BranchId=2
                },
                new UserBranch()
                {
                    UserId=1,
                    BranchId=3
                },
                new UserBranch()
                {
                    UserId=1,
                    BranchId=4
                }
            };
            builder.HasData(ubl);
        }
    }
}
