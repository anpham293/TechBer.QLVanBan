using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_QuyTrinhDuAnAssigned6664 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DuAnId",
                table: "QuyTrinhDuAnAssigneds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuyTrinhDuAnAssigneds_DuAnId",
                table: "QuyTrinhDuAnAssigneds",
                column: "DuAnId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuyTrinhDuAnAssigneds_DuAns_DuAnId",
                table: "QuyTrinhDuAnAssigneds",
                column: "DuAnId",
                principalTable: "DuAns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuyTrinhDuAnAssigneds_DuAns_DuAnId",
                table: "QuyTrinhDuAnAssigneds");

            migrationBuilder.DropIndex(
                name: "IX_QuyTrinhDuAnAssigneds_DuAnId",
                table: "QuyTrinhDuAnAssigneds");

            migrationBuilder.DropColumn(
                name: "DuAnId",
                table: "QuyTrinhDuAnAssigneds");
        }
    }
}
