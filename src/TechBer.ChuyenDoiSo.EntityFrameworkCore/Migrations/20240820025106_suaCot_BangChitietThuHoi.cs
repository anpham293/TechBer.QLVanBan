using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class suaCot_BangChitietThuHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongThuTe",
                table: "ChiTietThuHoies");

            migrationBuilder.AddColumn<decimal>(
                name: "TongThucTe",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongThucTe",
                table: "ChiTietThuHoies");

            migrationBuilder.AddColumn<decimal>(
                name: "TongThuTe",
                table: "ChiTietThuHoies",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
