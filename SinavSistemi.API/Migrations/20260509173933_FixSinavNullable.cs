using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SinavSistemi.API.Migrations
{
    /// <inheritdoc />
    public partial class FixSinavNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "SinavSorular");

            migrationBuilder.AddColumn<int>(
                name: "SinavId",
                table: "Sorular",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_SinavId",
                table: "Sorular",
                column: "SinavId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sorular_Sinavlar_SinavId",
                table: "Sorular",
                column: "SinavId",
                principalTable: "Sinavlar",
                principalColumn: "SinavId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sorular_Sinavlar_SinavId",
                table: "Sorular");

            migrationBuilder.DropIndex(
                name: "IX_Sorular_SinavId",
                table: "Sorular");

            migrationBuilder.DropColumn(
                name: "SinavId",
                table: "Sorular");

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OgrenciId = table.Column<int>(type: "int", nullable: true),
                    Rol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sifre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
                });

            migrationBuilder.CreateTable(
                name: "SinavSorular",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SinavId = table.Column<int>(type: "int", nullable: false),
                    SoruId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinavSorular", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SinavSorular_Sinavlar_SinavId",
                        column: x => x.SinavId,
                        principalTable: "Sinavlar",
                        principalColumn: "SinavId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SinavSorular_Sorular_SoruId",
                        column: x => x.SoruId,
                        principalTable: "Sorular",
                        principalColumn: "SoruId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SinavSorular_SinavId",
                table: "SinavSorular",
                column: "SinavId");

            migrationBuilder.CreateIndex(
                name: "IX_SinavSorular_SoruId",
                table: "SinavSorular",
                column: "SoruId");
        }
    }
}
