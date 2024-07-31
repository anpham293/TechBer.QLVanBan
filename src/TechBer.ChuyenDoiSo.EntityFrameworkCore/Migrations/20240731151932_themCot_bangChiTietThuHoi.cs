using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class themCot_bangChiTietThuHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TongDu",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TongThu",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongDu",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "TongThu",
                table: "ChiTietThuHoies");
        }
    }
}
