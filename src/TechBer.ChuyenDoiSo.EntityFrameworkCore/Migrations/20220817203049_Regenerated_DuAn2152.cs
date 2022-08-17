using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_DuAn2152 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoaiDuAnId",
                table: "DuAns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DuAns_LoaiDuAnId",
                table: "DuAns",
                column: "LoaiDuAnId");

            migrationBuilder.AddForeignKey(
                name: "FK_DuAns_LoaiDuAns_LoaiDuAnId",
                table: "DuAns",
                column: "LoaiDuAnId",
                principalTable: "LoaiDuAns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DuAns_LoaiDuAns_LoaiDuAnId",
                table: "DuAns");

            migrationBuilder.DropIndex(
                name: "IX_DuAns_LoaiDuAnId",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "LoaiDuAnId",
                table: "DuAns");
        }
    }
}
