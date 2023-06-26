using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Added_TraoDoiVanBanDuAn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TraoDoiVanBanDuAns",
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
                    NgayGui = table.Column<DateTime>(nullable: false),
                    NoiDung = table.Column<string>(nullable: true),
                    UserId = table.Column<long>(nullable: true),
                    VanBanDuAnId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraoDoiVanBanDuAns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TraoDoiVanBanDuAns_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TraoDoiVanBanDuAns_VanBanDuAns_VanBanDuAnId",
                        column: x => x.VanBanDuAnId,
                        principalTable: "VanBanDuAns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TraoDoiVanBanDuAns_TenantId",
                table: "TraoDoiVanBanDuAns",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TraoDoiVanBanDuAns_UserId",
                table: "TraoDoiVanBanDuAns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TraoDoiVanBanDuAns_VanBanDuAnId",
                table: "TraoDoiVanBanDuAns",
                column: "VanBanDuAnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TraoDoiVanBanDuAns");
        }
    }
}
