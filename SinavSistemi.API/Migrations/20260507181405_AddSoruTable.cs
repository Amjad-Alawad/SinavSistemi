using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SinavSistemi.API.Migrations
{
    public partial class AddSoruTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    KullaniciId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KullaniciAdi = table.Column<string>(nullable: false),
                    Sifre = table.Column<string>(nullable: false),
                    Rol = table.Column<string>(nullable: false),
                    OgrenciId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.KullaniciId);
                });

            migrationBuilder.CreateTable(
                name: "Sorular",
                columns: table => new
                {
                    SoruId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DersId = table.Column<int>(nullable: false),
                    SoruMetni = table.Column<string>(nullable: false),
                    SecenekA = table.Column<string>(nullable: false),
                    SecenekB = table.Column<string>(nullable: false),
                    SecenekC = table.Column<string>(nullable: false),
                    SecenekD = table.Column<string>(nullable: false),
                    DogruCevap = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorular", x => x.SoruId);
                    table.ForeignKey(
                        name: "FK_Sorular_Dersler_DersId",
                        column: x => x.DersId,
                        principalTable: "Dersler",
                        principalColumn: "DersId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SinavSorular",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SinavId = table.Column<int>(nullable: false),
                    SoruId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SinavSorular", x => x.Id);

                    table.ForeignKey(
                        name: "FK_SinavSorular_Sinavlar_SinavId",
                        column: x => x.SinavId,
                        principalTable: "Sinavlar",
                        principalColumn: "SinavId",
                        onDelete: ReferentialAction.Cascade); // ✅ bu kalır

                    table.ForeignKey(
                        name: "FK_SinavSorular_Sorular_SoruId",
                        column: x => x.SoruId,
                        principalTable: "Sorular",
                        principalColumn: "SoruId",
                        onDelete: ReferentialAction.Restrict); // 🔥 BURASI KRİTİK
                });

            migrationBuilder.CreateIndex(
                name: "IX_SinavSorular_SinavId",
                table: "SinavSorular",
                column: "SinavId");

            migrationBuilder.CreateIndex(
                name: "IX_SinavSorular_SoruId",
                table: "SinavSorular",
                column: "SoruId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorular_DersId",
                table: "Sorular",
                column: "DersId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Kullanicilar");
            migrationBuilder.DropTable(name: "SinavSorular");
            migrationBuilder.DropTable(name: "Sorular");
        }
    }
}