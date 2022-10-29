using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class GroupTypePrivileges : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            var group = new Group()
            {
                Id = 1,
                Name = "مجموعة المدراء",

            };
            builder.HasData(group);
        }
    }
}
