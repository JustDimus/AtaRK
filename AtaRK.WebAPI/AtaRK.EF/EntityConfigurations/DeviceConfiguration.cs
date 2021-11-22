using AtaRK.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.EF.EntityConfigurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder.HasKey(i => i.Id).IsClustered(true);

            builder
                .HasOne(i => i.Group)
                .WithMany(i => i.Devices)
                .HasForeignKey(i => i.GroupId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.Property(i => i.Type).IsRequired(true);

            builder.Property(i => i.DeviceName).IsRequired(false);
        }
    }
}
