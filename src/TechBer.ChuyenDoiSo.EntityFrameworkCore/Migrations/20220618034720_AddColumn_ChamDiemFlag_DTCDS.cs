using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class AddColumn_ChamDiemFlag_DTCDS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChamDiemFlag",
                table: "DoiTuongChuyenDoiSos",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChamDiemFlag",
                table: "DoiTuongChuyenDoiSos");
        }
    }
}
