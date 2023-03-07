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
            builder.HasMany(c => c.FromCountries).WithOne(c => c.FromCountry).HasForeignKey(c => c.FromCountryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.ToCountries).WithOne(c => c.ToCountry).HasForeignKey(c => c.ToCountryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasMany(c => c.MediatorCountries).WithOne(c => c.MidCountry).HasForeignKey(c => c.MediatorCountryId).OnDelete(DeleteBehavior.Restrict);
            var countreis = new List<Country>();
            int id = 1;
            countreis.Add(new Country() { Id = id++, Name = "خارجي اربيل"});
            countreis.Add(new Country() { Id = id++, Name = "دهوك"});
            countreis.Add(new Country() { Id = id++, Name = "سليمانية"});
            countreis.Add(new Country() { Id = id++, Name = "موصل"});
            countreis.Add(new Country() { Id = id++, Name = "بغداد"});
            countreis.Add(new Country() { Id = id++, Name = "محافظات جنوبية"});
            countreis.Add(new Country() { Id = id++, Name = "كركوك"});
            countreis.Add(new Country() { Id = id++, Name = "اربيل"});
            countreis.Add(new Country() { Id = id++, Name = "خبات"});
            countreis.Add(new Country() { Id = id++, Name = "انبار"});
            countreis.Add(new Country() { Id = id++, Name = "تكريت"});
            countreis.Add(new Country() { Id = id++, Name = "ديالى"});
            countreis.Add(new Country() { Id = id++, Name = "واسط"});
            countreis.Add(new Country() { Id = id++, Name = "نجف"});
            countreis.Add(new Country() { Id = id++, Name = "كربلاء"});
            countreis.Add(new Country() { Id = id++, Name = "ديوانية"});
            countreis.Add(new Country() { Id = id++, Name = "مثنى"});
            countreis.Add(new Country() { Id = id++, Name = "صلاح الدين"});
            countreis.Add(new Country() { Id = id++, Name = "سامراء"});
            countreis.Add(new Country() { Id = id++, Name = "بصره"});
            countreis.Add(new Country() { Id = id++, Name = "عمارة"});
            countreis.Add(new Country() { Id = id++, Name = "ناصرية"});
            countreis.Add(new Country() { Id = id++, Name = "بابل"});
            countreis.Add(new Country() { Id = id++, Name = "كوت"});
            countreis.Add(new Country() { Id = id++, Name = "ذي قار"});
            countreis.Add(new Country() { Id = id++, Name = "ميسان"});
            countreis.Add(new Country() { Id = id++, Name = "سيروان"});
            countreis.Add(new Country() { Id = id++, Name = "مخمور"});
            countreis.Add(new Country() { Id = id++, Name = "مصيف"});
            countreis.Add(new Country() { Id = id++, Name = "سوران"});
            countreis.Add(new Country() { Id = id++, Name = "كوية"});
            countreis.Add(new Country() { Id = id++, Name = "شقلاوة"});
            countreis.Add(new Country() { Id = id++, Name = "ملا عمر"});
            countreis.Add(new Country() { Id = id++, Name = "بارازان"});
            countreis.Add(new Country() { Id = id++, Name = "سلافا ستي"});
            countreis.Add(new Country() { Id = id++, Name = "كوير"});
            countreis.Add(new Country() { Id = id++, Name = "برده رش"});
            countreis.Add(new Country() { Id = id++, Name = "ديانا"});
            countreis.Add(new Country() { Id = id++, Name = "خليفان"});
            countreis.Add(new Country() { Id = id++, Name = "راوندوز"});
            countreis.Add(new Country() { Id = id++, Name = "رانيا"});
            countreis.Add(new Country() { Id = id++, Name = "طق طق"});
            countreis.Add(new Country() { Id = id++, Name = "قلادزي"});
            countreis.Add(new Country() { Id = id++, Name = "كرخ محمد"});
            countreis.Add(new Country() { Id = id++, Name = "عمار بابل"});
            countreis.Add(new Country() { Id = id++, Name = "الرصافة"});
            countreis.Add(new Country() { Id = id++, Name = "كرخ 2"});
            countreis.Add(new Country() { Id = id++, Name = "قوقز فرع بغداد"});
            countreis.Add(new Country() { Id = id++, Name = "سماوة"});
            builder.HasData(countreis);
        }
    }
}
