using AtaRK.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.EF.EntityConfigurations
{
    public class DeviceGroupConfiguration : IEntityTypeConfiguration<DeviceGroup>
    {
        public void Configure(EntityTypeBuilder<DeviceGroup> builder)
        {
            builder.HasKey(i => i.Id).IsClustered(true);

            builder.Property(i => i.GroupName).IsRequired(true);
        }
    }
}
