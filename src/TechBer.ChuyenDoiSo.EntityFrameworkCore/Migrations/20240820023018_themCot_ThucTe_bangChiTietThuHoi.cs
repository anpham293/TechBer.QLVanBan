using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class themCot_ThucTe_bangChiTietThuHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe1",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe10",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe11",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe12",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe2",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe3",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe4",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe5",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe6",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe7",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe8",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ThucTe9",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TongThuTe",
                table: "ChiTietThuHoies",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThucTe1",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe10",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe11",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe12",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe2",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe3",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe4",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe5",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe6",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe7",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe8",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "ThucTe9",
                table: "ChiTietThuHoies");

            migrationBuilder.DropColumn(
                name: "TongThuTe",
                table: "ChiTietThuHoies");
        }
    }
}
