using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class AddColumn_Table_dtcds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TongDiemDatDuoc",
                table: "DoiTuongChuyenDoiSos",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TongDiemHoiDongThamDinh",
                table: "DoiTuongChuyenDoiSos",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TongDiemTuDanhGia",
                table: "DoiTuongChuyenDoiSos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongDiemDatDuoc",
                table: "DoiTuongChuyenDoiSos");

            migrationBuilder.DropColumn(
                name: "TongDiemHoiDongThamDinh",
                table: "DoiTuongChuyenDoiSos");

            migrationBuilder.DropColumn(
                name: "TongDiemTuDanhGia",
                table: "DoiTuongChuyenDoiSos");
        }
    }
}
