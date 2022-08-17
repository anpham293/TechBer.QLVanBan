using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Add_Column_Table_CayTieuChi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoSau",
                table: "TieuChiDanhGias",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhanNhomLevel1",
                table: "TieuChiDanhGias",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoSau",
                table: "TieuChiDanhGias");

            migrationBuilder.DropColumn(
                name: "PhanNhomLevel1",
                table: "TieuChiDanhGias");
        }
    }
}
