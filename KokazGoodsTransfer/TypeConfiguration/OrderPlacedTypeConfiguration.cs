using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class OrderPlacedTypeConfiguration: IEntityTypeConfiguration<OrderPlaced>
    {
        public void Configure(EntityTypeBuilder<OrderPlaced> builder)
        {
            var ol = new List<OrderPlaced>()
            {
                new OrderPlaced()
                {
                    Id=1,
                    Name ="عند العميل",
                },
                new OrderPlaced()
                {
                    Id=2,
                    Name ="في المخزن",
                },
                new OrderPlaced()
                {
                    Id=3,
                    Name ="في الطريق",
                },
                new OrderPlaced()
                {
                    Id=4,
                    Name ="تم التسليم",
                },
                new OrderPlaced()
                {
                    Id=5,
                    Name ="مرتجع كلي",
                },
                new OrderPlaced()
                {
                    Id=6,
                    Name ="مرتجع جزئي",
                },
                new OrderPlaced()
                {
                    Id=7,
                    Name ="مرفوض",
                },
                new OrderPlaced()
                {
                    Id=8,
                    Name ="مؤجل",
                },
            };
            builder.HasData(ol);
        }
    }
}
