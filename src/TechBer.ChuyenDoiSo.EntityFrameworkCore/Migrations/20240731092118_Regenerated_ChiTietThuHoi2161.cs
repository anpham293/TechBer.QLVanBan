using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_ChiTietThuHoi2161 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Thu8",
                table: "ChiTietThuHoies",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                table: "ChiTietThuHoies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ten",
                table: "ChiTietThuHoies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Thu8",
                table: "ChiTietThuHoies",
                type: "datetime(6)",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
