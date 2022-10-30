using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class MoneyPlacedTypeConfiguration : IEntityTypeConfiguration<MoenyPlaced>
    {
        public void Configure(EntityTypeBuilder<MoenyPlaced> builder)
        {
            var ml = new List<MoenyPlaced>()
            {
                new MoenyPlaced()
                {
                    Id =1,
                    Name ="خارج الشركة",
                    
                },
                new MoenyPlaced()
                {
                    Id =2,
                    Name ="مندوب",

                },
                new MoenyPlaced()
                {
                    Id =3,
                    Name ="داخل الشركة",

                },
                new MoenyPlaced()
                {
                    Id =4,
                    Name ="تم تسليمها",

                }
            };
            builder.HasData(ml);
        }
    }
}
