using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.EF.EntityConfigurations
{
    public class DeviceConfigurationConfiguration : IEntityTypeConfiguration<AtaRK.Core.Models.Entities.DeviceConfiguration>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Entities.DeviceConfiguration> builder)
        {
            builder.HasKey(i => new { i.ConfigurationId, i.DeviceId });
        }
    }
}
