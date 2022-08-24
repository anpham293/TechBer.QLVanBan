using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class modify_quytrinh_add_sovanban : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "QuyTrinhDuAns");

            migrationBuilder.AddColumn<int>(
                name: "SoVanBanQuyDinh",
                table: "QuyTrinhDuAns",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoVanBanQuyDinh",
                table: "QuyTrinhDuAns");

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "QuyTrinhDuAns",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
