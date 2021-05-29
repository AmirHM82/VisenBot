using Microsoft.EntityFrameworkCore.Migrations;

namespace TrimedBot.Migrations
{
    public partial class dsjdhskdksdkjskjdcnsnclasdjlka : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "Users");
        }
    }
}
