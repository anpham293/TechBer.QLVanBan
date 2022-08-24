using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class modify_fullaudited_vanban : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<long>(
                name: "QuyTrinhDuAnAssignedId",
                table: "VanBanDuAns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VanBanDuAns_QuyTrinhDuAnAssignedId",
                table: "VanBanDuAns",
                column: "QuyTrinhDuAnAssignedId");

            migrationBuilder.AddForeignKey(
                name: "FK_VanBanDuAns_QuyTrinhDuAnAssigneds_QuyTrinhDuAnAssignedId",
                table: "VanBanDuAns",
                column: "QuyTrinhDuAnAssignedId",
                principalTable: "QuyTrinhDuAnAssigneds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VanBanDuAns_QuyTrinhDuAnAssigneds_QuyTrinhDuAnAssignedId",
                table: "VanBanDuAns");

            migrationBuilder.DropIndex(
                name: "IX_VanBanDuAns_QuyTrinhDuAnAssignedId",
                table: "VanBanDuAns");

            migrationBuilder.DropColumn(
                name: "QuyTrinhDuAnAssignedId",
                table: "VanBanDuAns");

            migrationBuilder.AddColumn<int>(
                name: "QuyTrinhDuAnId",
                table: "VanBanDuAns",
                type: "int",
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
    }
}
