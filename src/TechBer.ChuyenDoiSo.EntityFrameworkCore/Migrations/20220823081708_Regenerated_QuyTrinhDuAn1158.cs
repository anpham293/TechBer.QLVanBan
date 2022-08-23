using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_QuyTrinhDuAn1158 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GhiChu",
                table: "QuyTrinhDuAns",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaQuyTrinh",
                table: "QuyTrinhDuAns",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "QuyTrinhDuAns",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuyTrinhDuAns_ParentId",
                table: "QuyTrinhDuAns",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuyTrinhDuAns_QuyTrinhDuAns_ParentId",
                table: "QuyTrinhDuAns",
                column: "ParentId",
                principalTable: "QuyTrinhDuAns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuyTrinhDuAns_QuyTrinhDuAns_ParentId",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropIndex(
                name: "IX_QuyTrinhDuAns_ParentId",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropColumn(
                name: "GhiChu",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropColumn(
                name: "MaQuyTrinh",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "QuyTrinhDuAns");
        }
    }
}
