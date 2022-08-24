using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class Added_QuyTrinhDuAnAssigned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuyTrinhDuAnAssigneds",
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
                    TenantId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Descriptions = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: false),
                    SoVanBanQuyDinh = table.Column<int>(nullable: false),
                    MaQuyTrinh = table.Column<string>(maxLength: 255, nullable: true),
                    LoaiDuAnId = table.Column<int>(nullable: true),
                    QuyTrinhDuAnId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuyTrinhDuAnAssigneds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuyTrinhDuAnAssigneds_LoaiDuAns_LoaiDuAnId",
                        column: x => x.LoaiDuAnId,
                        principalTable: "LoaiDuAns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuyTrinhDuAnAssigneds_QuyTrinhDuAns_QuyTrinhDuAnId",
                        column: x => x.QuyTrinhDuAnId,
                        principalTable: "QuyTrinhDuAns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuyTrinhDuAnAssigneds_LoaiDuAnId",
                table: "QuyTrinhDuAnAssigneds",
                column: "LoaiDuAnId");

            migrationBuilder.CreateIndex(
                name: "IX_QuyTrinhDuAnAssigneds_QuyTrinhDuAnId",
                table: "QuyTrinhDuAnAssigneds",
                column: "QuyTrinhDuAnId");

            migrationBuilder.CreateIndex(
                name: "IX_QuyTrinhDuAnAssigneds_TenantId",
                table: "QuyTrinhDuAnAssigneds",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuyTrinhDuAnAssigneds");
        }
    }
}
