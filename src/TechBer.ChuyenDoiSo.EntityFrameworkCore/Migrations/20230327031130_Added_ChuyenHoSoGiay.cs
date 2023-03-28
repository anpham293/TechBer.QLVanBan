using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Added_ChuyenHoSoGiay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChuyenHoSoGiaies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<int>(nullable: true),
                    NguoiChuyenId = table.Column<int>(nullable: false),
                    ThoiGianChuyen = table.Column<DateTime>(nullable: true),
                    SoLuong = table.Column<int>(nullable: false),
                    TrangThai = table.Column<int>(nullable: false),
                    ThoiGianNhan = table.Column<DateTime>(nullable: true),
                    VanBanDuAnId = table.Column<int>(nullable: true),
                    NguoiNhanId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChuyenHoSoGiaies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChuyenHoSoGiaies_AbpUsers_NguoiNhanId",
                        column: x => x.NguoiNhanId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChuyenHoSoGiaies_VanBanDuAns_VanBanDuAnId",
                        column: x => x.VanBanDuAnId,
                        principalTable: "VanBanDuAns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenHoSoGiaies_NguoiNhanId",
                table: "ChuyenHoSoGiaies",
                column: "NguoiNhanId");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenHoSoGiaies_TenantId",
                table: "ChuyenHoSoGiaies",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ChuyenHoSoGiaies_VanBanDuAnId",
                table: "ChuyenHoSoGiaies",
                column: "VanBanDuAnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChuyenHoSoGiaies");
        }
    }
}
