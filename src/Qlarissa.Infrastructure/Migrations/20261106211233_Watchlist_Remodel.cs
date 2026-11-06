using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Watchlist_Remodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchedSecurities_AspNetUsers_WatchedByUserId",
                table: "WatchedSecurities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WatchedSecurities",
                table: "WatchedSecurities");

            migrationBuilder.DropColumn(
                name: "WatchedByUserId",
                table: "WatchedSecurities");

            migrationBuilder.DropColumn(
                name: "IsPrimaryWatchlist",
                table: "WatchedSecurities");

            migrationBuilder.AddColumn<int>(
                name: "WatchlistId",
                table: "WatchedSecurities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WatchedSecurities",
                table: "WatchedSecurities",
                columns: new[] { "WatchlistId", "SecurityId" });

            migrationBuilder.CreateTable(
                name: "Watchlists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPinned = table.Column<bool>(type: "bit", nullable: false),
                    PinPosition = table.Column<int>(type: "int", nullable: false),
                    TrackedChangeDurationTradingDays = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Watchlists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Watchlists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_Id_UserId",
                table: "Watchlists",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Watchlists_UserId",
                table: "Watchlists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WatchedSecurities_Watchlists_WatchlistId",
                table: "WatchedSecurities",
                column: "WatchlistId",
                principalTable: "Watchlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WatchedSecurities_Watchlists_WatchlistId",
                table: "WatchedSecurities");

            migrationBuilder.DropTable(
                name: "Watchlists");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WatchedSecurities",
                table: "WatchedSecurities");

            migrationBuilder.DropColumn(
                name: "WatchlistId",
                table: "WatchedSecurities");

            migrationBuilder.AddColumn<string>(
                name: "WatchedByUserId",
                table: "WatchedSecurities",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrimaryWatchlist",
                table: "WatchedSecurities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WatchedSecurities",
                table: "WatchedSecurities",
                columns: new[] { "WatchedByUserId", "SecurityId" });

            migrationBuilder.AddForeignKey(
                name: "FK_WatchedSecurities_AspNetUsers_WatchedByUserId",
                table: "WatchedSecurities",
                column: "WatchedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
