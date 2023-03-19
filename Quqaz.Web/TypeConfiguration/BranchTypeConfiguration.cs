using Quqaz.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Quqaz.Web.TypeConfiguration
{
    public class BranchTypeConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {

            builder.HasKey(x => x.Id);
            builder.Property(c => c.Name).IsUnicode(true);
            builder.Property(c => c.Name).IsRequired().IsUnicode(true).HasMaxLength(50);
            var branchesIds=new List<int>() { 4,8,3,5,7};
            var bl = new List<Branch>()
            {
                new Branch()
            {
                Id = 4,
                Name = "فرع الموصل",

            },
                    new Branch()
            {
                Id = 8,
                Name = "فرع اربيل",

            },
                    new Branch()
            {
                Id = 3,
                Name = "فرع سليمانية",

            },
               new Branch()
               {
                   Id=5,
                   Name="فرع بغداد",
               },
               new Branch()
               {
                   Id=7,
                   Name="فرع كروك",
               }
            };
            builder.HasData(bl);

        }
    }
}
