using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AtaRK.EF.Migrations
{
    public partial class AddedConfigurationconfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Device_DeviceGroup_GroupId",
                table: "Device");

            migrationBuilder.DropTable(
                name: "DeviceConfiguration");

            migrationBuilder.AlterColumn<string>(
                name: "Setting",
                table: "Configuration",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "Configuration",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DeviceId",
                table: "Configuration",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Configuration_Device_DeviceId",
                table: "Configuration",
                column: "DeviceId",
                principalTable: "Device",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Device_DeviceGroup_GroupId",
                table: "Device",
                column: "GroupId",
                principalTable: "DeviceGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configuration_Device_DeviceId",
                table: "Configuration");

            migrationBuilder.DropForeignKey(
                name: "FK_Device_DeviceGroup_GroupId",
                table: "Device");

            migrationBuilder.DropIndex(
                name: "IX_Configuration_DeviceId",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Configuration");

            migrationBuilder.AlterColumn<int>(
                name: "Setting",
                table: "Configuration",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "DeviceConfiguration",
                columns: table => new
                {
                    ConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeviceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceConfiguration", x => new { x.ConfigurationId, x.DeviceId });
                    table.ForeignKey(
                        name: "FK_DeviceConfiguration_Configuration_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configuration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeviceConfiguration_Device_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Device",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeviceConfiguration_DeviceId",
                table: "DeviceConfiguration",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Device_DeviceGroup_GroupId",
                table: "Device",
                column: "GroupId",
                principalTable: "DeviceGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
