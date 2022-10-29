using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class OrderStateTypeConfiguration : IEntityTypeConfiguration<OrderState>
    {
        public void Configure(EntityTypeBuilder<OrderState> builder)
        {
            var osl = new List<OrderState>()
            {
                new OrderState()
                {
                    Id = 1,
                    State="قيد المعالجة"
                },
                new OrderState()
                {
                    Id = 2,
                    State="يحب اخذ النقود من العميل"
                },
                new OrderState()
                {
                    Id = 3,
                    State="منتهية"
                },
            };
            builder.HasData(osl);
        }
    }
}
