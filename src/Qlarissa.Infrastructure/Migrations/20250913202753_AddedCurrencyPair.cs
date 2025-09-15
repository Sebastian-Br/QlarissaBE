using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCurrencyPair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_SecurityBase_AccountCurrencyId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityBase_SecurityBase_CurrencyId",
                table: "SecurityBase");

            migrationBuilder.AddColumn<string>(
                name: "ISIN",
                table: "SecurityBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastCompleteUpdateTime",
                table: "SecurityBase",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PriceLastUpdatedTime",
                table: "SecurityBase",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayCurrencyId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DividendPayout",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PayoutDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PayoutAmount = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    SecurityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendPayout", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DividendPayout_SecurityBase_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "SecurityBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DisplayCurrencyId",
                table: "AspNetUsers",
                column: "DisplayCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_DividendPayout_SecurityId",
                table: "DividendPayout",
                column: "SecurityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Currency_DisplayCurrencyId",
                table: "AspNetUsers",
                column: "DisplayCurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Currencies_AccountCurrencyId",
                table: "Portfolios",
                column: "AccountCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityBase_Currencies_CurrencyId",
                table: "SecurityBase",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Currency_DisplayCurrencyId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Currencies_AccountCurrencyId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_SecurityBase_Currencies_CurrencyId",
                table: "SecurityBase");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropTable(
                name: "DividendPayout");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DisplayCurrencyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ISIN",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "LastCompleteUpdateTime",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "PriceLastUpdatedTime",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "DisplayCurrencyId",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_SecurityBase_AccountCurrencyId",
                table: "Portfolios",
                column: "AccountCurrencyId",
                principalTable: "SecurityBase",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityBase_SecurityBase_CurrencyId",
                table: "SecurityBase",
                column: "CurrencyId",
                principalTable: "SecurityBase",
                principalColumn: "Id");
        }
    }
}
