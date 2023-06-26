using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Added_BaoCaoVanBanDuAn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaoCaoVanBanDuAns",
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
                    TenantId = table.Column<int>(nullable: true),
                    NoiDungCongViec = table.Column<string>(nullable: true),
                    MoTaChiTiet = table.Column<string>(nullable: true),
                    FileBaoCao = table.Column<string>(nullable: true),
                    VanBanDuAnId = table.Column<int>(nullable: true),
                    UserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaoCaoVanBanDuAns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaoCaoVanBanDuAns_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaoCaoVanBanDuAns_VanBanDuAns_VanBanDuAnId",
                        column: x => x.VanBanDuAnId,
                        principalTable: "VanBanDuAns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoVanBanDuAns_TenantId",
                table: "BaoCaoVanBanDuAns",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoVanBanDuAns_UserId",
                table: "BaoCaoVanBanDuAns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaoCaoVanBanDuAns_VanBanDuAnId",
                table: "BaoCaoVanBanDuAns",
                column: "VanBanDuAnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaoCaoVanBanDuAns");
        }
    }
}
