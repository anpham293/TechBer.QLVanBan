using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class themcot_NguoiNopHoSoId_VanBanDuAn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "NguoiNopHoSoId",
                table: "VanBanDuAns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NguoiNopHoSoId",
                table: "VanBanDuAns");
        }
    }
}
