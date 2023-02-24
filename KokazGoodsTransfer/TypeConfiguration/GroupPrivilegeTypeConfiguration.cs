using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class GroupPrivilegeTypeConfiguration: IEntityTypeConfiguration<GroupPrivilege>
    {
        public void Configure(EntityTypeBuilder<GroupPrivilege> builder)
        {
            var gpl = new List<GroupPrivilege>();
            for (int i = 1; i <= 63; i++)
            {
                gpl.Add(new GroupPrivilege()
                {
                    GroupId = 1,
                    PrivilegId = i
                });
                gpl.Add(new GroupPrivilege()
                {
                    GroupId = 2,
                    PrivilegId = i
                });
                gpl.Add(new GroupPrivilege()
                {
                    GroupId = 3,
                    PrivilegId = i
                });
                gpl.Add(new GroupPrivilege()
                {
                    GroupId = 4,
                    PrivilegId = i
                });

            }
            builder.HasData(gpl);
        }
    }
}
