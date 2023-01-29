using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class UserGroupTypeConfiguration : IEntityTypeConfiguration<UserGroup>
    {
        public void Configure(EntityTypeBuilder<UserGroup> builder)
        {
            builder.HasData(new UserGroup()
            {
                UserId = 1,
                GroupId = 1
            });
            builder.HasData(new UserGroup()
            {
                UserId = 1,
                GroupId = 2
            });
            builder.HasData(new UserGroup()
            {
                UserId = 1,
                GroupId = 3
            });
            builder.HasData(new UserGroup()
            {
                UserId = 1,
                GroupId = 4
            });
        }
    }
}
