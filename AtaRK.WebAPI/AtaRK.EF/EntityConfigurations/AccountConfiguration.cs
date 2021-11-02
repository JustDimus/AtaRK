using AtaRK.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.EF.EntityConfigurations
{
    internal class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(i => i.Id).IsClustered(true);

            builder
                .HasMany(i => i.Groups)
                .WithOne(i => i.Account)
                .HasForeignKey(i => i.AccountId);

            builder.HasIndex(i => i.Email).IsUnique(true);
        }
    }
}
