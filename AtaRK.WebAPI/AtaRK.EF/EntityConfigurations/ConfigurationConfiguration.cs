using AtaRK.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.EF.EntityConfigurations
{
    public class ConfigurationConfiguration : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            builder.HasKey(i => i.Id);

            builder
                .HasOne(i => i.Device)
                .WithMany(i => i.Configurations)
                .HasForeignKey(i => i.DeviceId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
