using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_LoaiDuAn8505 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrganizationUnitId",
                table: "LoaiDuAns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoaiDuAns_OrganizationUnitId",
                table: "LoaiDuAns",
                column: "OrganizationUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoaiDuAns_AbpOrganizationUnits_OrganizationUnitId",
                table: "LoaiDuAns",
                column: "OrganizationUnitId",
                principalTable: "AbpOrganizationUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoaiDuAns_AbpOrganizationUnits_OrganizationUnitId",
                table: "LoaiDuAns");

            migrationBuilder.DropIndex(
                name: "IX_LoaiDuAns_OrganizationUnitId",
                table: "LoaiDuAns");

            migrationBuilder.DropColumn(
                name: "OrganizationUnitId",
                table: "LoaiDuAns");
        }
    }
}
