using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISIN",
                table: "SecurityBase");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "SecurityBase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ETFHoldingss",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ETFid = table.Column<int>(type: "int", nullable: false),
                    PortfolioId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETFHoldingss", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ETFHoldingss_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ETFHoldingss_SecurityBase_ETFid",
                        column: x => x.ETFid,
                        principalTable: "SecurityBase",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ETFHoldingss_ETFid",
                table: "ETFHoldingss",
                column: "ETFid");

            migrationBuilder.CreateIndex(
                name: "IX_ETFHoldingss_PortfolioId",
                table: "ETFHoldingss",
                column: "PortfolioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ETFHoldingss");

            migrationBuilder.AlterColumn<int>(
                name: "CurrencyId",
                table: "SecurityBase",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ISIN",
                table: "SecurityBase",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
