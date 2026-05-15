using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SinavSistemi.API.Migrations
{
    /// <inheritdoc />
    public partial class CleanFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SinavSorular_Sorular_SoruId",
                table: "SinavSorular");

            migrationBuilder.AddForeignKey(
                name: "FK_SinavSorular_Sorular_SoruId",
                table: "SinavSorular",
                column: "SoruId",
                principalTable: "Sorular",
                principalColumn: "SoruId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SinavSorular_Sorular_SoruId",
                table: "SinavSorular");

            migrationBuilder.AddForeignKey(
                name: "FK_SinavSorular_Sorular_SoruId",
                table: "SinavSorular",
                column: "SoruId",
                principalTable: "Sorular",
                principalColumn: "SoruId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
