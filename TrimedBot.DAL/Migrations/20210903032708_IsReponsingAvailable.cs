using Microsoft.EntityFrameworkCore.Migrations;

namespace TrimedBot.DAL.Migrations
{
    public partial class IsReponsingAvailable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsResponsingAvailable",
                table: "Settings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsResponsingAvailable",
                table: "Settings");
        }
    }
}
