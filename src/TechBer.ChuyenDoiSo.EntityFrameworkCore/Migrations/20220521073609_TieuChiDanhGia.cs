using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class TieuChiDanhGia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TieuChiDanhGias",
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
                    Name = table.Column<string>(nullable: false),
                    MaxPoint = table.Column<int>(nullable: false),
                    EvaluationMethod = table.Column<string>(nullable: true),
                    InformationDeclaration = table.Column<string>(nullable: true),
                    Notice = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TieuChiDanhGias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TieuChiDanhGias_TieuChiDanhGias_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TieuChiDanhGias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TieuChiDanhGias_ParentId",
                table: "TieuChiDanhGias",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TieuChiDanhGias_TenantId",
                table: "TieuChiDanhGias",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TieuChiDanhGias");
        }
    }
}
