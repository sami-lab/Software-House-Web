using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareHouseWeb.Data.Migrations
{
    public partial class ChangingDateFormat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_At",
                table: "Portfolio",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_At",
                table: "Portfolio",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
