using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class modify_fullaudited_duan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "DuAns",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "CreatorUserId",
                table: "DuAns",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleterUserId",
                table: "DuAns",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "DuAns",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "DuAns",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "DuAns",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LastModifierUserId",
                table: "DuAns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "CreatorUserId",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "DeleterUserId",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "DuAns");

            migrationBuilder.DropColumn(
                name: "LastModifierUserId",
                table: "DuAns");
        }
    }
}
