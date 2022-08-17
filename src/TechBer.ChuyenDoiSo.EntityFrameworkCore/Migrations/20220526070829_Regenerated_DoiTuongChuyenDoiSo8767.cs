using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_DoiTuongChuyenDoiSo8767 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "DoiTuongChuyenDoiSos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DoiTuongChuyenDoiSos_UserId",
                table: "DoiTuongChuyenDoiSos",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoiTuongChuyenDoiSos_AbpUsers_UserId",
                table: "DoiTuongChuyenDoiSos",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoiTuongChuyenDoiSos_AbpUsers_UserId",
                table: "DoiTuongChuyenDoiSos");

            migrationBuilder.DropIndex(
                name: "IX_DoiTuongChuyenDoiSos_UserId",
                table: "DoiTuongChuyenDoiSos");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DoiTuongChuyenDoiSos");
        }
    }
}
