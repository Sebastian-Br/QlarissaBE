using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MovedDeleteCascadeToHoldingEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoldingEvent_Holding_HoldingId",
                table: "HoldingEvent");

            migrationBuilder.AddForeignKey(
                name: "FK_HoldingEvent_Holding_HoldingId",
                table: "HoldingEvent",
                column: "HoldingId",
                principalTable: "Holding",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoldingEvent_Holding_HoldingId",
                table: "HoldingEvent");

            migrationBuilder.AddForeignKey(
                name: "FK_HoldingEvent_Holding_HoldingId",
                table: "HoldingEvent",
                column: "HoldingId",
                principalTable: "Holding",
                principalColumn: "Id");
        }
    }
}
