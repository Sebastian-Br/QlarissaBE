using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_WatchedSecurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WatchedSecurities",
                columns: table => new
                {
                    WatchedByUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SecurityId = table.Column<int>(type: "int", nullable: false),
                    IsPrimaryWatchlist = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedSecurities", x => new { x.WatchedByUserId, x.SecurityId });
                    table.ForeignKey(
                        name: "FK_WatchedSecurities_AspNetUsers_WatchedByUserId",
                        column: x => x.WatchedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchedSecurities_SecurityBase_SecurityId",
                        column: x => x.SecurityId,
                        principalTable: "SecurityBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchedSecurities_SecurityId",
                table: "WatchedSecurities",
                column: "SecurityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchedSecurities");
        }
    }
}
