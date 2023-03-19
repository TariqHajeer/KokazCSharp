using Quqaz.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;

namespace Quqaz.Web.TypeConfiguration
{
    public class GroupTypeConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            var id = 1;
            var branchesIds = new List<int>() { 4, 8, 3, 5, 7 };
            List<Group> groups = new List<Group>();
            foreach (var branch in branchesIds)
            {
                groups.Add(new Group()
                {
                    Id = id++,
                    Name = "مجموعة المدراء",
                    BranchId = branch
                });
            }
            builder.HasData(groups);
        }
    }
}
