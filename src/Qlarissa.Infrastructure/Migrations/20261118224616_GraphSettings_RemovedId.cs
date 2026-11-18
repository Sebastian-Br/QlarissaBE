using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GraphSettings_RemovedId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityGraphSettings_Bounds_BoundsId",
                table: "SecurityGraphSettings");

            migrationBuilder.DropTable(
                name: "DividendPayoutGraphSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SecurityGraphSettings",
                table: "SecurityGraphSettings");

            migrationBuilder.DropIndex(
                name: "IX_SecurityGraphSettings_Id_UserId",
                table: "SecurityGraphSettings");

            migrationBuilder.DropIndex(
                name: "IX_SecurityGraphSettings_UserId_GraphType",
                table: "SecurityGraphSettings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SecurityGraphSettings");

            migrationBuilder.RenameTable(
                name: "SecurityGraphSettings",
                newName: "GraphSettings");

            migrationBuilder.RenameColumn(
                name: "PriceCardHoverDistance",
                table: "GraphSettings",
                newName: "ValueCardHoverDistance");

            migrationBuilder.RenameIndex(
                name: "IX_SecurityGraphSettings_UserId",
                table: "GraphSettings",
                newName: "IX_GraphSettings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_SecurityGraphSettings_BoundsId",
                table: "GraphSettings",
                newName: "IX_GraphSettings_BoundsId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "GraphSettings",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSynchronizedWithSecurityGraph",
                table: "GraphSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowDividendYield",
                table: "GraphSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GraphSettings",
                table: "GraphSettings",
                columns: new[] { "UserId", "GraphType" });

            migrationBuilder.AddForeignKey(
                name: "FK_GraphSettings_Bounds_BoundsId",
                table: "GraphSettings",
                column: "BoundsId",
                principalTable: "Bounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GraphSettings_Bounds_BoundsId",
                table: "GraphSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GraphSettings",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "IsSynchronizedWithSecurityGraph",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "ShowDividendYield",
                table: "GraphSettings");

            migrationBuilder.RenameTable(
                name: "GraphSettings",
                newName: "SecurityGraphSettings");

            migrationBuilder.RenameColumn(
                name: "ValueCardHoverDistance",
                table: "SecurityGraphSettings",
                newName: "PriceCardHoverDistance");

            migrationBuilder.RenameIndex(
                name: "IX_GraphSettings_UserId",
                table: "SecurityGraphSettings",
                newName: "IX_SecurityGraphSettings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GraphSettings_BoundsId",
                table: "SecurityGraphSettings",
                newName: "IX_SecurityGraphSettings_BoundsId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SecurityGraphSettings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SecurityGraphSettings",
                table: "SecurityGraphSettings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DividendPayoutGraphSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BoundsId = table.Column<int>(type: "int", nullable: false),
                    GraphType = table.Column<int>(type: "int", nullable: false),
                    IsSynchronizedWithSecurityGraph = table.Column<bool>(type: "bit", nullable: false),
                    LogarithmicScale = table.Column<bool>(type: "bit", nullable: false),
                    PriceCardHoverDistance = table.Column<int>(type: "int", nullable: false),
                    ShowDividendYield = table.Column<bool>(type: "bit", nullable: false),
                    ShowXAxisGridLines = table.Column<bool>(type: "bit", nullable: false),
                    ShowYAxisGridLines = table.Column<bool>(type: "bit", nullable: false),
                    TickCount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGraphSettings_Id_UserId",
                table: "SecurityGraphSettings",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_SecurityGraphSettings_UserId_GraphType",
                table: "SecurityGraphSettings",
                columns: new[] { "UserId", "GraphType" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityGraphSettings_Bounds_BoundsId",
                table: "SecurityGraphSettings",
                column: "BoundsId",
                principalTable: "Bounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
