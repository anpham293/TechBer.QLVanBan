using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Update_TieuChi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "TieuChiDanhGias");

            migrationBuilder.AlterColumn<double>(
                name: "DiemToiDa",
                table: "TieuChiDanhGias",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DiemToiDa",
                table: "TieuChiDanhGias",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "TieuChiDanhGias",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
