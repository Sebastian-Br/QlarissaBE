using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class typo_etfholdingss : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ETFHoldingss_Portfolios_PortfolioId",
                table: "ETFHoldingss");

            migrationBuilder.DropForeignKey(
                name: "FK_ETFHoldingss_SecurityBase_ETFid",
                table: "ETFHoldingss");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ETFHoldingss",
                table: "ETFHoldingss");

            migrationBuilder.RenameTable(
                name: "ETFHoldingss",
                newName: "ETFHoldings");

            migrationBuilder.RenameIndex(
                name: "IX_ETFHoldingss_PortfolioId",
                table: "ETFHoldings",
                newName: "IX_ETFHoldings_PortfolioId");

            migrationBuilder.RenameIndex(
                name: "IX_ETFHoldingss_ETFid",
                table: "ETFHoldings",
                newName: "IX_ETFHoldings_ETFid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ETFHoldings",
                table: "ETFHoldings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ETFHoldings_Portfolios_PortfolioId",
                table: "ETFHoldings",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ETFHoldings_SecurityBase_ETFid",
                table: "ETFHoldings",
                column: "ETFid",
                principalTable: "SecurityBase",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ETFHoldings_Portfolios_PortfolioId",
                table: "ETFHoldings");

            migrationBuilder.DropForeignKey(
                name: "FK_ETFHoldings_SecurityBase_ETFid",
                table: "ETFHoldings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ETFHoldings",
                table: "ETFHoldings");

            migrationBuilder.RenameTable(
                name: "ETFHoldings",
                newName: "ETFHoldingss");

            migrationBuilder.RenameIndex(
                name: "IX_ETFHoldings_PortfolioId",
                table: "ETFHoldingss",
                newName: "IX_ETFHoldingss_PortfolioId");

            migrationBuilder.RenameIndex(
                name: "IX_ETFHoldings_ETFid",
                table: "ETFHoldingss",
                newName: "IX_ETFHoldingss_ETFid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ETFHoldingss",
                table: "ETFHoldingss",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ETFHoldingss_Portfolios_PortfolioId",
                table: "ETFHoldingss",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ETFHoldingss_SecurityBase_ETFid",
                table: "ETFHoldingss",
                column: "ETFid",
                principalTable: "SecurityBase",
                principalColumn: "Id");
        }
    }
}
