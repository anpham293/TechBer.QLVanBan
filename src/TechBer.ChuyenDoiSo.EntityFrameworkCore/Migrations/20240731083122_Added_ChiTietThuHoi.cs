using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Added_ChiTietThuHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiTietThuHoies",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    Du1 = table.Column<decimal>(nullable: false),
                    Du2 = table.Column<decimal>(nullable: false),
                    Du3 = table.Column<decimal>(nullable: false),
                    Du4 = table.Column<decimal>(nullable: false),
                    Du5 = table.Column<decimal>(nullable: false),
                    Du6 = table.Column<decimal>(nullable: false),
                    Du7 = table.Column<decimal>(nullable: false),
                    Du8 = table.Column<decimal>(nullable: false),
                    Du9 = table.Column<decimal>(nullable: false),
                    Du10 = table.Column<decimal>(nullable: false),
                    Du11 = table.Column<decimal>(nullable: false),
                    Du12 = table.Column<decimal>(nullable: false),
                    Thu1 = table.Column<decimal>(nullable: false),
                    Thu2 = table.Column<decimal>(nullable: false),
                    Thu3 = table.Column<decimal>(nullable: false),
                    Thu4 = table.Column<decimal>(nullable: false),
                    Thu5 = table.Column<decimal>(nullable: false),
                    Thu6 = table.Column<decimal>(nullable: false),
                    Thu7 = table.Column<decimal>(nullable: false),
                    Thu8 = table.Column<DateTime>(nullable: false),
                    Thu9 = table.Column<decimal>(nullable: false),
                    Thu10 = table.Column<decimal>(nullable: false),
                    Thu11 = table.Column<decimal>(nullable: false),
                    Thu12 = table.Column<decimal>(nullable: false),
                    GhiChu = table.Column<string>(nullable: true),
                    DanhMucThuHoiId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietThuHoies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietThuHoies_DanhMucThuHoies_DanhMucThuHoiId",
                        column: x => x.DanhMucThuHoiId,
                        principalTable: "DanhMucThuHoies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietThuHoies_DanhMucThuHoiId",
                table: "ChiTietThuHoies",
                column: "DanhMucThuHoiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietThuHoies");
        }
    }
}
