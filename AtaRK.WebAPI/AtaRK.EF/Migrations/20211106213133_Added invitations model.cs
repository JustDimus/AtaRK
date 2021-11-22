using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AtaRK.EF.Migrations
{
    public partial class Addedinvitationsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invite",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvitedId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invite", x => new { x.GroupId, x.InvitedId });
                    table.ForeignKey(
                        name: "FK_Invite_Account_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invite_Account_InvitedId",
                        column: x => x.InvitedId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invite_DeviceGroup_GroupId",
                        column: x => x.GroupId,
                        principalTable: "DeviceGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invite_CreatorId",
                table: "Invite",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Invite_InvitedId",
                table: "Invite",
                column: "InvitedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invite");
        }
    }
}
