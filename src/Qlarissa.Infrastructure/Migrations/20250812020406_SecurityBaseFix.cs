using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SecurityBaseFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyPrices_SecurityBase_SecurityId",
                table: "DailyPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityBase_SecurityBase_CurrencyId",
                table: "SecurityBase");

            migrationBuilder.AddColumn<string>(
                name: "InvestorRelationsURL",
                table: "SecurityBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPrices_SecurityBase_SecurityId",
                table: "DailyPrices",
                column: "SecurityId",
                principalTable: "SecurityBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityBase_SecurityBase_CurrencyId",
                table: "SecurityBase",
                column: "CurrencyId",
                principalTable: "SecurityBase",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyPrices_SecurityBase_SecurityId",
                table: "DailyPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityBase_SecurityBase_CurrencyId",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "InvestorRelationsURL",
                table: "SecurityBase");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyPrices_SecurityBase_SecurityId",
                table: "DailyPrices",
                column: "SecurityId",
                principalTable: "SecurityBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityBase_SecurityBase_CurrencyId",
                table: "SecurityBase",
                column: "CurrencyId",
                principalTable: "SecurityBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
