using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplacedHoldingsModelsAndExtendedOtherEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ETFHoldings");

            migrationBuilder.DropTable(
                name: "StockHoldings");

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "SecurityBase",
                type: "float",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessSummary",
                table: "SecurityBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DividendRate",
                table: "SecurityBase",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DividendYield",
                table: "SecurityBase",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISIN",
                table: "SecurityBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MarketCapitalization",
                table: "SecurityBase",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "NetExpenseRatio",
                table: "SecurityBase",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RecommendationMean",
                table: "SecurityBase",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Stock_ISIN",
                table: "SecurityBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TargetMeanPrice",
                table: "SecurityBase",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "PayoutAmount",
                table: "DividendPayout",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<double>(
                name: "Open",
                table: "DailyPrices",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<double>(
                name: "Low",
                table: "DailyPrices",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<double>(
                name: "High",
                table: "DailyPrices",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<double>(
                name: "Close",
                table: "DailyPrices",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.AlterColumn<double>(
                name: "Average",
                table: "DailyPrices",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)",
                oldPrecision: 18,
                oldScale: 6);

            migrationBuilder.CreateTable(
                name: "Holding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    SecurityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holding_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Holding_SecurityBase_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "SecurityBase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Split",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SplitRatio = table.Column<double>(type: "float", nullable: false),
                    SecurityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Split", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Split_SecurityBase_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "SecurityBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HoldingEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoldingId = table.Column<int>(type: "int", nullable: false),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HoldingEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HoldingEvent_Holding_HoldingId",
                        column: x => x.HoldingId,
                        principalTable: "Holding",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holding_PortfolioId",
                table: "Holding",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Holding_SecurityId",
                table: "Holding",
                column: "SecurityId");

            migrationBuilder.CreateIndex(
                name: "IX_HoldingEvent_HoldingId",
                table: "HoldingEvent",
                column: "HoldingId");

            migrationBuilder.CreateIndex(
                name: "IX_Split_SecurityId",
                table: "Split",
                column: "SecurityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HoldingEvent");

            migrationBuilder.DropTable(
                name: "Split");

            migrationBuilder.DropTable(
                name: "Holding");

            migrationBuilder.DropColumn(
                name: "BusinessSummary",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "DividendRate",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "DividendYield",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "ISIN",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "MarketCapitalization",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "NetExpenseRatio",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "RecommendationMean",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "Stock_ISIN",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "TargetMeanPrice",
                table: "SecurityBase");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "SecurityBase",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "PayoutAmount",
                table: "DividendPayout",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Open",
                table: "DailyPrices",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Low",
                table: "DailyPrices",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "High",
                table: "DailyPrices",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Close",
                table: "DailyPrices",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Average",
                table: "DailyPrices",
                type: "decimal(18,6)",
                precision: 18,
                scale: 6,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateTable(
                name: "ETFHoldings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ETFid = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PortfolioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETFHoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ETFHoldings_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ETFHoldings_SecurityBase_ETFid",
                        column: x => x.ETFid,
                        principalTable: "SecurityBase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StockHoldings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PortfolioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockHoldings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockHoldings_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StockHoldings_SecurityBase_StockId",
                        column: x => x.StockId,
                        principalTable: "SecurityBase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ETFHoldings_ETFid",
                table: "ETFHoldings",
                column: "ETFid");

            migrationBuilder.CreateIndex(
                name: "IX_ETFHoldings_PortfolioId",
                table: "ETFHoldings",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHoldings_PortfolioId",
                table: "StockHoldings",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_StockHoldings_StockId",
                table: "StockHoldings",
                column: "StockId");
        }
    }
}
