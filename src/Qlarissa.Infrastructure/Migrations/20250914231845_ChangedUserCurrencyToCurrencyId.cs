using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Qlarissa.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserCurrencyToCurrencyId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Currency_DisplayCurrencyId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DisplayCurrencyId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DisplayCurrencyId",
                table: "AspNetUsers",
                column: "DisplayCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Currency_DisplayCurrencyId",
                table: "AspNetUsers",
                column: "DisplayCurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
