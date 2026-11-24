using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSettingsModelsAndTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GraphSettings_Bounds_BoundsId",
                table: "GraphSettings");

            migrationBuilder.DropIndex(
                name: "IX_GraphSettings_BoundsId",
                table: "GraphSettings");

            migrationBuilder.DropIndex(
                name: "IX_GraphSettings_UserId",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "BoundsId",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "LogarithmicScale",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "ShowDividendYield",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "ShowHorizontalGridLines",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "ShowVerticalGridLines",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "TickCount",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "ValueCardHoverDistance",
                table: "GraphSettings");

            migrationBuilder.CreateTable(
                name: "GraphSettingsTags",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GraphSetting = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GraphSettingsTags", x => new { x.UserId, x.GraphSetting, x.Key });
                    table.ForeignKey(
                        name: "FK_GraphSettingsTags_GraphSettings_UserId_GraphSetting",
                        columns: x => new { x.UserId, x.GraphSetting },
                        principalTable: "GraphSettings",
                        principalColumns: new[] { "UserId", "DataSource" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlotSettingsTags",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlotType = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlotSettingsTags", x => new { x.UserId, x.PlotType, x.Key });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GraphSettingsTags");

            migrationBuilder.DropTable(
                name: "PlotSettingsTags");

            migrationBuilder.AddColumn<int>(
                name: "BoundsId",
                table: "GraphSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "LogarithmicScale",
                table: "GraphSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowDividendYield",
                table: "GraphSettings",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowHorizontalGridLines",
                table: "GraphSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShowVerticalGridLines",
                table: "GraphSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "TickCount",
                table: "GraphSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ValueCardHoverDistance",
                table: "GraphSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GraphSettings_BoundsId",
                table: "GraphSettings",
                column: "BoundsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GraphSettings_UserId",
                table: "GraphSettings",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GraphSettings_Bounds_BoundsId",
                table: "GraphSettings",
                column: "BoundsId",
                principalTable: "Bounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
