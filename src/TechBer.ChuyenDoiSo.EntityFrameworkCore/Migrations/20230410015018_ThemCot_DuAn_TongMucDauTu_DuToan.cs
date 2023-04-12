using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class ThemCot_DuAn_TongMucDauTu_DuToan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DuToan",
                table: "DuAns",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TongMucDauTu",
                table: "DuAns",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DuToan",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "TongMucDauTu",
                table: "DuAns");
        }
    }
}
