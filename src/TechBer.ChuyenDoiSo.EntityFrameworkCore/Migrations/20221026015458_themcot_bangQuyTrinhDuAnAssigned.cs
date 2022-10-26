using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class themcot_bangQuyTrinhDuAnAssigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "KeToanTiepNhanId",
                table: "QuyTrinhDuAnAssigneds",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDuyet",
                table: "QuyTrinhDuAnAssigneds",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayGui",
                table: "QuyTrinhDuAnAssigneds",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NguoiDuyetId",
                table: "QuyTrinhDuAnAssigneds",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NguoiGuiId",
                table: "QuyTrinhDuAnAssigneds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XuLyCuaLanhDao",
                table: "QuyTrinhDuAnAssigneds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
