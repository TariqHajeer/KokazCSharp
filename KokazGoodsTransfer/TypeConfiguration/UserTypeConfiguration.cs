﻿using KokazGoodsTransfer.Helpers;
using KokazGoodsTransfer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KokazGoodsTransfer.TypeConfiguration
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var u = new User()
            {
                Id = 1,
                Name = "admin",
                UserName = "admin",
                Password = MD5Hash.GetMd5Hash("admin"),
                HireDate = new System.DateTime(2022, 1, 1),
                CanWorkAsAgent =false,
                IsActive = true,
            };

            builder.HasData(u);
        }
    }
}
