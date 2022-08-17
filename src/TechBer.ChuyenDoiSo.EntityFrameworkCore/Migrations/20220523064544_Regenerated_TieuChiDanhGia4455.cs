using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_TieuChiDanhGia4455 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvaluationMethod",
                table: "TieuChiDanhGias");

            migrationBuilder.DropColumn(
                name: "InformationDeclaration",
                table: "TieuChiDanhGias");

            migrationBuilder.DropColumn(
                name: "MaxPoint",
                table: "TieuChiDanhGias");

            migrationBuilder.DropColumn(
                name: "Notice",
                table: "TieuChiDanhGias");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "TieuChiDanhGias");

            migrationBuilder.AddColumn<int>(
                name: "DiemToiDa",
                table: "TieuChiDanhGias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "TieuChiDanhGias",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LoaiPhuLuc",
                table: "TieuChiDanhGias",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhuongThucDanhGia",
                table: "TieuChiDanhGias",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaiLieuGiaiTrinh",
                table: "TieuChiDanhGias",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiemToiDa",
                table: "TieuChiDanhGias");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "TieuChiDanhGias");

            migrationBuilder.DropColumn(
                name: "LoaiPhuLuc",
                table: "TieuChiDanhGias");

            migrationBuilder.DropColumn(
                name: "PhuongThucDanhGia",
                table: "TieuChiDanhGias");

            migrationBuilder.DropColumn(
                name: "TaiLieuGiaiTrinh",
                table: "TieuChiDanhGias");

            migrationBuilder.AddColumn<string>(
                name: "EvaluationMethod",
                table: "TieuChiDanhGias",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InformationDeclaration",
                table: "TieuChiDanhGias",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxPoint",
                table: "TieuChiDanhGias",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Notice",
                table: "TieuChiDanhGias",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "TieuChiDanhGias",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
