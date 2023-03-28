using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class XoaCot_HoSoGiay_Bang_VanBanDuAn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenNguoiGiaoHoSo",
                table: "VanBanDuAns",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianNhanHoSoGiay",
                table: "VanBanDuAns",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiNhanHoSoGiay",
                table: "VanBanDuAns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
