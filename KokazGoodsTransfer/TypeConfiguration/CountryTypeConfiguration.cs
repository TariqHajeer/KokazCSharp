using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class CountryTypeConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            var countreis = new List<Country>();
            int id = 1;
            countreis.Add(new Country() { Id = id++, Name = "خارجي اربيل", DeliveryCost = 5000, Points = 2, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "دهوك", DeliveryCost = 5000, Points = 2, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "سليمانية", DeliveryCost = 5000, Points = 2, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "موصل", DeliveryCost = 5000, Points = 2, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "بغداد", DeliveryCost = 6000, Points = 3, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "محافظات جنوبية", DeliveryCost = 8000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "كركوك", DeliveryCost = 5000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "اربيل", DeliveryCost = 4000, Points = 2, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "خبات", DeliveryCost = 5000, Points = 1, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "انبار", DeliveryCost = 8000, Points = 3, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "تكريت", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "ديالى", DeliveryCost = 8000, Points = 3, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "واسط", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "نجف", DeliveryCost = 8000, Points = 3, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "كربلاء", DeliveryCost = 8000, Points = 3, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "ديوانية", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "مثنى", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "صلاح الدين", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "سامراء", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "بصره", DeliveryCost = 8000, Points = 3, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "عمارة", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "ناصرية", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "بابل", DeliveryCost = 8000, Points = 3, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "كوت", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "ذي قار", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "ميسان", DeliveryCost = 8000, Points = 4, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "سيروان", DeliveryCost = 7000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "مخمور", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "مصيف", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "سوران", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "كوية", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "شقلاوة", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "ملا عمر", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "بارازان", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "سلافا ستي", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "كوير", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "برده رش", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "ديانا", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "خليفان", DeliveryCost = 5000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "راوندوز", DeliveryCost = 5000, Points = 1, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "رانيا", DeliveryCost = 5000, Points = 2, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "طق طق", DeliveryCost = 5000, Points = 2, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "قلادزي", DeliveryCost = 6000, Points = 1, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "كرخ محمد", DeliveryCost = 4000, Points = 1, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "عمار بابل", DeliveryCost = 8000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "الرصافة", DeliveryCost = 6000, Points = 2, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "كرخ 2", DeliveryCost = 6000, Points = 5, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "قوقز فرع بغداد", DeliveryCost = 6000, Points = 0, IsMain = false });
            countreis.Add(new Country() { Id = id++, Name = "سماوة", DeliveryCost = 8000, Points = 0, IsMain = false });
            builder.HasData(countreis);
        }
    }
}
