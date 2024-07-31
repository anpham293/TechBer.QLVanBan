using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Added_DanhMucThuHoi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DanhMucThuHoies",
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
                    Stt = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    GhiChu = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    DuAnThuHoiId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhMucThuHoies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhMucThuHoies_DuAnThuHoies_DuAnThuHoiId",
                        column: x => x.DuAnThuHoiId,
                        principalTable: "DuAnThuHoies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhMucThuHoies_DuAnThuHoiId",
                table: "DanhMucThuHoies",
                column: "DuAnThuHoiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhMucThuHoies");
        }
    }
}
