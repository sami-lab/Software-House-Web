using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftwareHouseWeb.Data.Migrations
{
    public partial class UpdatingPromos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PromoCode",
                table: "Promos",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Ser_id",
                table: "Promos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Ser_Id",
                table: "Packages",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InitialPromoUsed",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Promos_Ser_id_PromoCode",
                table: "Promos",
                columns: new[] { "Ser_id", "PromoCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Promos_Services_Ser_id",
                table: "Promos",
                column: "Ser_id",
                principalTable: "Services",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promos_Services_Ser_id",
                table: "Promos");

            migrationBuilder.DropIndex(
                name: "IX_Promos_Ser_id_PromoCode",
                table: "Promos");

            migrationBuilder.DropColumn(
                name: "Ser_id",
                table: "Promos");

            migrationBuilder.DropColumn(
                name: "InitialPromoUsed",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PromoCode",
                table: "Promos",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "Ser_Id",
                table: "Packages",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
