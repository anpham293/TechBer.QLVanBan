using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Regenerated_ChiTietDanhGia7533 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChiTietDanhGias",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeleterUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    SoLieuKeKhai = table.Column<string>(nullable: true),
                    DiemTuDanhGia = table.Column<double>(nullable: true),
                    DiemHoiDongThamDinh = table.Column<double>(nullable: true),
                    DiemDatDuoc = table.Column<double>(nullable: true),
                    TieuChiDanhGiaId = table.Column<int>(nullable: false),
                    DoiTuongChuyenDoiSoId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDanhGias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietDanhGias_DoiTuongChuyenDoiSos_DoiTuongChuyenDoiSoId",
                        column: x => x.DoiTuongChuyenDoiSoId,
                        principalTable: "DoiTuongChuyenDoiSos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDanhGias_TieuChiDanhGias_TieuChiDanhGiaId",
                        column: x => x.TieuChiDanhGiaId,
                        principalTable: "TieuChiDanhGias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDanhGias_DoiTuongChuyenDoiSoId",
                table: "ChiTietDanhGias",
                column: "DoiTuongChuyenDoiSoId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDanhGias_TenantId",
                table: "ChiTietDanhGias",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDanhGias_TieuChiDanhGiaId",
                table: "ChiTietDanhGias",
                column: "TieuChiDanhGiaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietDanhGias");
        }
    }
}
