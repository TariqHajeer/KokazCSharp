using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class GroupTypePrivileges : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            var id = 1;
            var group = new Group()
            {
                Id =id++ ,
                Name = "مجموعة المدراء",
                BranchId = 1,

            };
            builder.HasData(group);
            group = new Group()
            {
                Id = id++,
                Name = "مجموعة المدراء",
                BranchId = 2,
            };
            builder.HasData(group);
            group = new Group()
            {
                Id = id++,
                Name = "مجموعة المدراء",
                BranchId = 3,
            };
            builder.HasData(group);
            group = new Group()
            {
                Id = id++,
                Name = "مجموعة المدراء",
                BranchId = 4,
            };
            builder.HasData(group);
        }
    }
}
