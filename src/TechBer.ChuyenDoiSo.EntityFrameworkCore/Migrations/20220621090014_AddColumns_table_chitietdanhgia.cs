using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class AddColumns_table_chitietdanhgia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHoiDongThamDinh",
                table: "ChiTietDanhGias",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTuDanhGia",
                table: "ChiTietDanhGias",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHoiDongThamDinh",
                table: "ChiTietDanhGias");

            migrationBuilder.DropColumn(
                name: "IsTuDanhGia",
                table: "ChiTietDanhGias");
        }
    }
}
