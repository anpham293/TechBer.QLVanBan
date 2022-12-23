using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class xoacot_bang_quytrinhduanassigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeToanTiepNhanId",
                table: "QuyTrinhDuAnAssigneds");

            migrationBuilder.DropColumn(
                name: "NgayDuyet",
                table: "QuyTrinhDuAnAssigneds");

            migrationBuilder.DropColumn(
                name: "NgayGui",
                table: "QuyTrinhDuAnAssigneds");

            migrationBuilder.DropColumn(
                name: "NguoiDuyetId",
                table: "QuyTrinhDuAnAssigneds");

            migrationBuilder.DropColumn(
                name: "NguoiGuiId",
                table: "QuyTrinhDuAnAssigneds");

            migrationBuilder.DropColumn(
                name: "XuLyCuaLanhDao",
                table: "QuyTrinhDuAnAssigneds");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "KeToanTiepNhanId",
                table: "QuyTrinhDuAnAssigneds",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDuyet",
                table: "QuyTrinhDuAnAssigneds",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayGui",
                table: "QuyTrinhDuAnAssigneds",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NguoiDuyetId",
                table: "QuyTrinhDuAnAssigneds",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NguoiGuiId",
                table: "QuyTrinhDuAnAssigneds",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XuLyCuaLanhDao",
                table: "QuyTrinhDuAnAssigneds",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
