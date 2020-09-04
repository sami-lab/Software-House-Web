using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareHouseWeb.Data.Migrations
{
    public partial class MessingwithDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_At",
                table: "Reviews",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "created_At",
                table: "Reviews",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "Orders",
                type: "Date",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
