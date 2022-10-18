using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRUDNewsApi.Migrations
{
    public partial class AddUuidToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "Users",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Uuid",
                table: "Users",
                column: "Uuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Uuid",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "Users");
        }
    }
}
