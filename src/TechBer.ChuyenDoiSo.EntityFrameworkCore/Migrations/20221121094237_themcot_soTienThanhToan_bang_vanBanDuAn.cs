using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class themcot_soTienThanhToan_bang_vanBanDuAn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SoTienThanhToan",
                table: "VanBanDuAns",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoTienThanhToan",
                table: "VanBanDuAns");
        }
    }
}
