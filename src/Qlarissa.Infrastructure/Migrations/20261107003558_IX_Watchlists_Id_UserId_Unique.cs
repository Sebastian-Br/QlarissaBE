using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IX_Watchlists_Id_UserId_Unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Watchlists_Id_UserId",
                table: "Watchlists");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_Id_UserId",
                table: "Watchlists",
                columns: new[] { "Id", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Watchlists_Id_UserId",
                table: "Watchlists");

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_Id_UserId",
                table: "Watchlists",
                columns: new[] { "Id", "UserId" });
        }
    }
}
