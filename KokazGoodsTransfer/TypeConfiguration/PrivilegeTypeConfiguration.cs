using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class PrivilegeTypeConfiguration : IEntityTypeConfiguration<Privilege>
    {
        public void Configure(EntityTypeBuilder<Privilege> builder)
        {
            int id = 1;
            var pres=  new List<Privilege>()
            {
                new Privilege()
                {
                    Id = id++,
                    Name ="عرض المجموعات",
                    SysName ="ShowGroup",
                },
                new Privilege()
                {
                    Id =id++,
                    Name ="إضافة مجموعات",
                    SysName="AddGroup",
                },
                new Privilege()
                {
                    Id=id++,
                }
            };
            builder.HasData(pres);
            
        }
    }
}
