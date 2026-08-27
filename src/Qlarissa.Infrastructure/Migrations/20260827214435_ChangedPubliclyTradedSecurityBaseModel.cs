using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPubliclyTradedSecurityBaseModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExchangeName",
                table: "SecurityBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExchangeShortName",
                table: "SecurityBase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecurityType",
                table: "SecurityBase",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeName",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "ExchangeShortName",
                table: "SecurityBase");

            migrationBuilder.DropColumn(
                name: "SecurityType",
                table: "SecurityBase");
        }
    }
}
