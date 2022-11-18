using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TechBer.ChuyenDoiSo.Migrations
{
    public partial class themcot_lastFileVanBanTime_bang_vanBanDuAn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastFileVanBanTime",
                table: "VanBanDuAns",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastFileVanBanTime",
                table: "VanBanDuAns");
        }
    }
}
