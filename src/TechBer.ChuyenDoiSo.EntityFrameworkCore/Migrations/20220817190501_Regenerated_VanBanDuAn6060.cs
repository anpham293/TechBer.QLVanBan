using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_VanBanDuAn6060 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuyTrinhDuAnId",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VanBanDuAns_QuyTrinhDuAnId",
                table: "VanBanDuAns",
                column: "QuyTrinhDuAnId");

            migrationBuilder.AddForeignKey(
                name: "FK_VanBanDuAns_QuyTrinhDuAns_QuyTrinhDuAnId",
                table: "VanBanDuAns",
                column: "QuyTrinhDuAnId",
                principalTable: "QuyTrinhDuAns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VanBanDuAns_QuyTrinhDuAns_QuyTrinhDuAnId",
                table: "VanBanDuAns");

            migrationBuilder.DropIndex(
                name: "IX_VanBanDuAns_QuyTrinhDuAnId",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "QuyTrinhDuAnId",
                table: "VanBanDuAns");
        }
    }
}
