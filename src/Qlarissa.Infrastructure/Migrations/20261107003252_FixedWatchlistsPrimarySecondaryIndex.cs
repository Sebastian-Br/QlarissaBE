using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixedWatchlistsPrimarySecondaryIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Watchlists_UserId",
                table: "Watchlists",
                newName: "IX_Watchlists_UserId_Secondary");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_UserId",
                table: "Watchlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_UserId_Primary",
                table: "Watchlists",
                column: "UserId",
                unique: true,
                filter: "[IsPrimary] = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Watchlists_UserId",
                table: "Watchlists");

            migrationBuilder.DropIndex(
                name: "IX_Watchlists_UserId_Primary",
                table: "Watchlists");

            migrationBuilder.RenameIndex(
                name: "IX_Watchlists_UserId_Secondary",
                table: "Watchlists",
                newName: "IX_Watchlists_UserId");
        }
    }
}
