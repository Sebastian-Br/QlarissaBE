using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class replacedMarketCapitalizationColumnWithSharesOutstanding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MarketCapitalization",
                table: "SecurityBase",
                newName: "SharesOutstanding");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SharesOutstanding",
                table: "SecurityBase",
                newName: "MarketCapitalization");
        }
    }
}
