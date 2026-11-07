using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PrimarySecondaryWatchlists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Watchlists_UserId",
                table: "Watchlists");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimary",
                table: "Watchlists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSecondary",
                table: "Watchlists",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_UserId",
                table: "Watchlists",
                column: "UserId",
                unique: true,
                filter: "[IsSecondary] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Watchlists_UserId",
                table: "Watchlists");

            migrationBuilder.DropColumn(
                name: "IsPrimary",
                table: "Watchlists");

            migrationBuilder.DropColumn(
                name: "IsSecondary",
                table: "Watchlists");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_UserId",
                table: "Watchlists",
                column: "UserId");
        }
    }
}
