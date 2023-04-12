using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class ThemCot_Bang_ThungHoSo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViTriLuuTru",
                table: "VanBanDuAns");

            migrationBuilder.AddColumn<int>(
                name: "ThungHoSoId",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QrString",
                table: "ThungHoSos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThungHoSoId",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "QrString",
                table: "ThungHoSos");

            migrationBuilder.AddColumn<string>(
                name: "ViTriLuuTru",
                table: "VanBanDuAns",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
