using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedColumnName_PriceHistoryLastDataPointDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastCompleteUpdateTime",
                table: "SecurityBase",
                newName: "PriceHistoryLastDataPointDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceHistoryLastDataPointDate",
                table: "SecurityBase",
                newName: "LastCompleteUpdateTime");
        }
    }
}
