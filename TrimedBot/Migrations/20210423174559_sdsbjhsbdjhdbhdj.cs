using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrimedBot.Migrations
{
    public partial class sdsbjhsbdjhdbhdj : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddDate",
                table: "Medias",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddDate",
                table: "Medias");
        }
    }
}
