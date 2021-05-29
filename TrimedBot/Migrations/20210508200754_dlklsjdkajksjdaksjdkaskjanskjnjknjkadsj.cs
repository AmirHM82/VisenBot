using Microsoft.EntityFrameworkCore.Migrations;

namespace TrimedBot.Migrations
{
    public partial class dlklsjdkajksjdaksjdkaskjanskjnjknjkadsj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinanceAds",
                table: "Settings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "FinanceAds",
                table: "Settings",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
