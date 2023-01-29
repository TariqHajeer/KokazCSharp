using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class MediatorBranchTypeConfiguration : IEntityTypeConfiguration<MediatorBranch>
    {
        public void Configure(EntityTypeBuilder<MediatorBranch> builder)
        {
            builder.HasKey(c => new { c.FromBranchId, c.ToBranchId });
            List<MediatorBranch> list = new List<MediatorBranch>()
            {
                new MediatorBranch()
                {
                    FromBranchId = 1,
                    ToBranchId= 4,
                    MediatorBranchId= 3,
                },
                new MediatorBranch()
                {
                    FromBranchId = 2,
                    ToBranchId= 4,
                    MediatorBranchId= 3,
                },
                new MediatorBranch()
                {
                    FromBranchId = 4,
                    ToBranchId= 1,
                    MediatorBranchId= 3,
                },
                new MediatorBranch()
                {
                    FromBranchId = 4,
                    ToBranchId= 2,
                    MediatorBranchId= 3,
                }
            };
            builder.HasData(list);
            builder.HasOne(c => c.FromBranch).WithMany(c => c.FromBranches).HasPrincipalKey(c => c.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.ToBranch).WithMany(c => c.ToBranches).HasPrincipalKey(c => c.Id).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(c => c.MidBranch).WithMany(c => c.MediatorBranches).HasPrincipalKey(c => c.Id).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
