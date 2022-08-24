using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class modify_fullaudited : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "QuyTrinhDuAns",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "QuyTrinhDuAns",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "QuyTrinhDuAns",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "QuyTrinhDuAns",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "QuyTrinhDuAns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "QuyTrinhDuAns",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "QuyTrinhDuAns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "QuyTrinhDuAns");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "QuyTrinhDuAns");
        }
    }
}
