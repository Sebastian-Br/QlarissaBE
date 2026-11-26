using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveObsoleteBoundsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF EXISTS (SELECT 1 FROM [Bounds])
                BEGIN
                    ;THROW 51002, 'Bounds migration stopped: the legacy Bounds table contains rows that are no longer associated with graph settings. Export and reconcile those rows before applying this migration.', 1;
                END;
                """);

            migrationBuilder.DropTable(
                name: "Bounds");

            migrationBuilder.Sql(
                """
                IF EXISTS (
                    SELECT 1
                    FROM sys.indexes
                    WHERE name = N'IX_DisplaySettings_UserId_DeviceProfile'
                        AND object_id = OBJECT_ID(N'[dbo].[DisplaySettings]')
                )
                BEGIN
                    DROP INDEX [IX_DisplaySettings_UserId_DeviceProfile] ON [dbo].[DisplaySettings];
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    End = table.Column<double>(type: "float", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Start = table.Column<double>(type: "float", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bounds", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DisplaySettings_UserId_DeviceProfile",
                table: "DisplaySettings",
                columns: new[] { "UserId", "DeviceProfile" });
        }
    }
}
