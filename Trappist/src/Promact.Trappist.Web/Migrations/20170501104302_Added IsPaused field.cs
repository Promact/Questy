using Microsoft.EntityFrameworkCore.Migrations;

namespace Promact.Trappist.Web.Migrations
{
    public partial class AddedIsPausedfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaused",
                table: "Test",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPaused",
                table: "Test");
        }
    }
}
