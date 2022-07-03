using Microsoft.EntityFrameworkCore.Migrations;

namespace TrimedBot.DAL.Migrations
{
    public partial class ChannelPostChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelPosts",
                table: "ChannelPosts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ChannelPosts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelPosts",
                table: "ChannelPosts",
                column: "MessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelPosts",
                table: "ChannelPosts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ChannelPosts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelPosts",
                table: "ChannelPosts",
                column: "Id");
        }
    }
}
