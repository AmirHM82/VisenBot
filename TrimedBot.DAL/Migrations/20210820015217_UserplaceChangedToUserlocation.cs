using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TrimedBot.DAL.Migrations
{
    public partial class UserplaceChangedToUserlocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserPlace",
                table: "Users",
                newName: "UserLocation");

            migrationBuilder.AddColumn<Guid>(
                name: "MediaId",
                table: "Tags",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_MediaId",
                table: "Tags",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Medias_MediaId",
                table: "Tags",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Medias_MediaId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_MediaId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "MediaId",
                table: "Tags");

            migrationBuilder.RenameColumn(
                name: "UserLocation",
                table: "Users",
                newName: "UserPlace");
        }
    }
}
