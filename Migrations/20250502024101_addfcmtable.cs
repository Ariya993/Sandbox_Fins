using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sandbox_Calc.Migrations
{
    /// <inheritdoc />
    public partial class addfcmtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Category",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "id_category",
                table: "Category",
                newName: "category");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Appuser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Appuser",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "APPUSER_FCM",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    token_fcm = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APPUSER_FCM", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APPUSER_FCM");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Appuser");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Appuser");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Category",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "category",
                table: "Category",
                newName: "id_category");
        }
    }
}
