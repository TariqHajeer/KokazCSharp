using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class GroupTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            var id = 1;
            var group = new Group()
            {
                Id = id++,
                Name = "مجموعة المدراء",

            };
            builder.HasData(group);
        }
    }
}
