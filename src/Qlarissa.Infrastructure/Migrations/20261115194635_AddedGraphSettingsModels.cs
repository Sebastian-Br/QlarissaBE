using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedGraphSettingsModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Holding",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Bounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Start = table.Column<double>(type: "float", nullable: true),
                    End = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bounds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DividendPayoutGraphSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShowDividendYield = table.Column<bool>(type: "bit", nullable: false),
                    IsSynchronizedWithSecurityGraph = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GraphType = table.Column<int>(type: "int", nullable: false),
                    BoundsId = table.Column<int>(type: "int", nullable: false),
                    TickCount = table.Column<int>(type: "int", nullable: false),
                    ShowXAxisGridLines = table.Column<bool>(type: "bit", nullable: false),
                    ShowYAxisGridLines = table.Column<bool>(type: "bit", nullable: false),
                    LogarithmicScale = table.Column<bool>(type: "bit", nullable: false),
                    PriceCardHoverDistance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DividendPayoutGraphSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DividendPayoutGraphSettings_Bounds_BoundsId",
                        column: x => x.BoundsId,
                        principalTable: "Bounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SecurityGraphSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GraphType = table.Column<int>(type: "int", nullable: false),
                    BoundsId = table.Column<int>(type: "int", nullable: false),
                    TickCount = table.Column<int>(type: "int", nullable: false),
                    ShowXAxisGridLines = table.Column<bool>(type: "bit", nullable: false),
                    ShowYAxisGridLines = table.Column<bool>(type: "bit", nullable: false),
                    LogarithmicScale = table.Column<bool>(type: "bit", nullable: false),
                    PriceCardHoverDistance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityGraphSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecurityGraphSettings_Bounds_BoundsId",
                        column: x => x.BoundsId,
                        principalTable: "Bounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holding_UserId",
                table: "Holding",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DividendPayoutGraphSettings_BoundsId",
                table: "DividendPayoutGraphSettings",
                column: "BoundsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DividendPayoutGraphSettings_Id_UserId",
                table: "DividendPayoutGraphSettings",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_DividendPayoutGraphSettings_UserId",
                table: "DividendPayoutGraphSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DividendPayoutGraphSettings_UserId_GraphType",
                table: "DividendPayoutGraphSettings",
                columns: new[] { "UserId", "GraphType" });

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGraphSettings_BoundsId",
                table: "SecurityGraphSettings",
                column: "BoundsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGraphSettings_Id_UserId",
                table: "SecurityGraphSettings",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGraphSettings_UserId",
                table: "SecurityGraphSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGraphSettings_UserId_GraphType",
                table: "SecurityGraphSettings",
                columns: new[] { "UserId", "GraphType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DividendPayoutGraphSettings");

            migrationBuilder.DropTable(
                name: "SecurityGraphSettings");

            migrationBuilder.DropTable(
                name: "Bounds");

            migrationBuilder.DropIndex(
                name: "IX_Holding_UserId",
                table: "Holding");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Holding");
        }
    }
}
