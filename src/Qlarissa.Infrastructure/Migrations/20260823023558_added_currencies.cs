using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class added_currencies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "Name", "Symbol" },
                values: new object[,]
                {
                    { 1, "United States Dollar", "USD" },
                    { 2, "Euro", "EUR" },
                    { 3, "Japanese Yen", "JPY" },
                    { 4, "British Pound Sterling", "GBP" },
                    { 5, "Australian Dollar", "AUD" },
                    { 6, "Canadian Dollar", "CAD" },
                    { 7, "Swiss Franc", "CHF" },
                    { 8, "Chinese Yuan", "CNY" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
