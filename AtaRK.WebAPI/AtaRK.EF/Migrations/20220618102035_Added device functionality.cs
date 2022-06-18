using Microsoft.EntityFrameworkCore.Migrations;

namespace AtaRK.EF.Migrations
{
    public partial class Addeddevicefunctionality : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AdminOnlyAccess",
                table: "Device",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminOnlyAccess",
                table: "Device");
        }
    }
}
