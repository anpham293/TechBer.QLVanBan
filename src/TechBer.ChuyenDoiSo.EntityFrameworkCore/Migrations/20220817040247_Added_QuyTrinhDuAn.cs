using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Added_QuyTrinhDuAn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuyTrinhDuAns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Descriptions = table.Column<string>(nullable: true),
                    LoaiDuAnId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyTrinhDuAns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuyTrinhDuAns_LoaiDuAns_LoaiDuAnId",
                        column: x => x.LoaiDuAnId,
                        principalTable: "LoaiDuAns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuyTrinhDuAns_LoaiDuAnId",
                table: "QuyTrinhDuAns",
                column: "LoaiDuAnId");

            migrationBuilder.CreateIndex(
                name: "IX_QuyTrinhDuAns_TenantId",
                table: "QuyTrinhDuAns",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuyTrinhDuAns");
        }
    }
}
