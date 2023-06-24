using Microsoft.EntityFrameworkCore.Migrations;

namespace TrimedBot.DAL.Migrations
{
    public partial class asasasa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "TempMessages");

            migrationBuilder.AddColumn<byte>(
                name: "PostType",
                table: "ChannelPosts",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostType",
                table: "ChannelPosts");

            migrationBuilder.AddColumn<byte>(
                name: "Type",
                table: "TempMessages",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
