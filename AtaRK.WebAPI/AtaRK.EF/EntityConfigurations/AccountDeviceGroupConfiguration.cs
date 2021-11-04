using AtaRK.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.EF.EntityConfigurations
{
    public class AccountDeviceGroupConfiguration : IEntityTypeConfiguration<AccountDeviceGroup>
    {
        public void Configure(EntityTypeBuilder<AccountDeviceGroup> builder)
        {
            builder.HasKey(i => new { i.AccountId, i.GroupId });

            builder
                .HasOne(i => i.Account)
                .WithMany(i => i.Groups)
                .HasForeignKey(i => i.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(i => i.Group)
                .WithMany(i => i.Members)
                .HasForeignKey(i => i.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
