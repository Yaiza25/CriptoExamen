using Microsoft.EntityFrameworkCore.Migrations;

namespace CriptoDB.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cartera",
                columns: table => new
                {
                    CarteraId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Exchange = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartera", x => x.CarteraId);
                });

            migrationBuilder.CreateTable(
                name: "Moneda",
                columns: table => new
                {
                    MonedaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Actual = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Maximo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moneda", x => x.MonedaId);
                });

            migrationBuilder.CreateTable(
                name: "Contrato",
                columns: table => new
                {
                    ContratoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarteraId = table.Column<int>(type: "int", nullable: false),
                    MonedaId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contrato", x => x.ContratoId);
                    table.ForeignKey(
                        name: "FK_Contrato_Cartera_CarteraId",
                        column: x => x.CarteraId,
                        principalTable: "Cartera",
                        principalColumn: "CarteraId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contrato_Moneda_MonedaId",
                        column: x => x.MonedaId,
                        principalTable: "Moneda",
                        principalColumn: "MonedaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contrato_CarteraId",
                table: "Contrato",
                column: "CarteraId");

            migrationBuilder.CreateIndex(
                name: "IX_Contrato_MonedaId",
                table: "Contrato",
                column: "MonedaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contrato");

            migrationBuilder.DropTable(
                name: "Cartera");

            migrationBuilder.DropTable(
                name: "Moneda");
        }
    }
}
