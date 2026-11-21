using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Renamed_GraphType_DataSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowYAxisGridLines",
                table: "GraphSettings",
                newName: "ShowVerticalGridLines");

            migrationBuilder.RenameColumn(
                name: "ShowXAxisGridLines",
                table: "GraphSettings",
                newName: "ShowHorizontalGridLines");

            migrationBuilder.RenameColumn(
                name: "GraphType",
                table: "GraphSettings",
                newName: "DataSource");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowVerticalGridLines",
                table: "GraphSettings",
                newName: "ShowYAxisGridLines");

            migrationBuilder.RenameColumn(
                name: "ShowHorizontalGridLines",
                table: "GraphSettings",
                newName: "ShowXAxisGridLines");

            migrationBuilder.RenameColumn(
                name: "DataSource",
                table: "GraphSettings",
                newName: "GraphType");
        }
    }
}
