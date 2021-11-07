using AtaRK.Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace AtaRK.EF.EntityConfigurations
{
    public class InviteConfiguration : IEntityTypeConfiguration<Invite>
    {
        public void Configure(EntityTypeBuilder<Invite> builder)
        {
            builder.HasKey(i => new { i.GroupId, i.InvitedId });

            builder
                .HasOne(i => i.Group)
                .WithMany(i => i.Invitations)
                .HasForeignKey(i => i.GroupId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder
                .HasOne(i => i.Creator)
                .WithMany(i => i.CreatedInvitations)
                .HasForeignKey(i => i.CreatorId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder
                .HasOne(i => i.Invited)
                .WithMany(i => i.Invitations)
                .HasForeignKey(i => i.InvitedId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
