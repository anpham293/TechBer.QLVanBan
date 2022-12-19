using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class themcot_chuyenduyethoso_vanbanduan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "KeToanTiepNhanId",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDuyet",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayGui",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NguoiDuyetId",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NguoiGuiId",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrangThaiChuyenDuyetHoSo",
                table: "VanBanDuAns",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "XuLyCuaLanhDao",
                table: "VanBanDuAns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeToanTiepNhanId",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "NgayDuyet",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "NgayGui",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "NguoiDuyetId",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "NguoiGuiId",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "TrangThaiChuyenDuyetHoSo",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "XuLyCuaLanhDao",
                table: "VanBanDuAns");
        }
    }
}
