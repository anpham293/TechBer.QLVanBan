using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_QuyTrinhDuAnAssigned7967 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "QuyTrinhDuAnAssigneds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuyTrinhDuAnAssigneds_ParentId",
                table: "QuyTrinhDuAnAssigneds",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuyTrinhDuAnAssigneds_QuyTrinhDuAnAssigneds_ParentId",
                table: "QuyTrinhDuAnAssigneds",
                column: "ParentId",
                principalTable: "QuyTrinhDuAnAssigneds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuyTrinhDuAnAssigneds_QuyTrinhDuAnAssigneds_ParentId",
                table: "QuyTrinhDuAnAssigneds");

            migrationBuilder.DropIndex(
                name: "IX_QuyTrinhDuAnAssigneds_ParentId",
                table: "QuyTrinhDuAnAssigneds");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "QuyTrinhDuAnAssigneds");
        }
    }
}
