using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class schema_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DailyPrices_SecurityId",
                table: "DailyPrices");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPrices_SecurityId_Date",
                table: "DailyPrices",
                columns: new[] { "SecurityId", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DailyPrices_SecurityId_Date",
                table: "DailyPrices");

            migrationBuilder.CreateIndex(
                name: "IX_DailyPrices_SecurityId",
                table: "DailyPrices",
                column: "SecurityId");
        }
    }
}
