using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class themCot_DuAn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChuongId",
                table: "DuAns",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoaiKhoanId",
                table: "DuAns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaDVQHNS",
                table: "DuAns",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayBatDau",
                table: "DuAns",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayKetThuc",
                table: "DuAns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChuongId",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "LoaiKhoanId",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "MaDVQHNS",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "NgayBatDau",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "NgayKetThuc",
                table: "DuAns");
        }
    }
}
