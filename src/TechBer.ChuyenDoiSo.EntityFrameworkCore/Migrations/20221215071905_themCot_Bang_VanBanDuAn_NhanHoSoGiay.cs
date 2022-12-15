using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class themCot_Bang_VanBanDuAn_NhanHoSoGiay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenNguoiGiaoHoSo",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianNhanHoSoGiay",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiNhanHoSoGiay",
                table: "VanBanDuAns",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenNguoiGiaoHoSo",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "ThoiGianNhanHoSoGiay",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "TrangThaiNhanHoSoGiay",
                table: "VanBanDuAns");
        }
    }
}
