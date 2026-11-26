using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DisplaySettingsAndGraphBounds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF EXISTS (SELECT 1 FROM [GraphSettingsTags])
                BEGIN
                    ;THROW 51000, 'Display settings migration stopped: GraphSettingsTags contains data. Export and reconcile those rows before applying this migration.', 1;
                END;
                IF EXISTS (SELECT 1 FROM [PlotSettingsTags])
                BEGIN
                    ;THROW 51001, 'Display settings migration stopped: PlotSettingsTags contains data. Export and reconcile those rows before applying this migration.', 1;
                END;
                """);

            migrationBuilder.DropTable(
                name: "GraphSettingsTags");

            migrationBuilder.DropTable(
                name: "PlotSettingsTags");

            migrationBuilder.AddColumn<double>(
                name: "Bounds_End",
                table: "GraphSettings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Bounds_EndDate",
                table: "GraphSettings",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Bounds_Start",
                table: "GraphSettings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "Bounds_StartDate",
                table: "GraphSettings",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DisplaySettings",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DeviceProfile = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisplaySettings", x => new { x.UserId, x.DeviceProfile, x.Key });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisplaySettings");

            migrationBuilder.DropColumn(
                name: "Bounds_End",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "Bounds_EndDate",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "Bounds_Start",
                table: "GraphSettings");

            migrationBuilder.DropColumn(
                name: "Bounds_StartDate",
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
    }
}
