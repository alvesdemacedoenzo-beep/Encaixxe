using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Encaixxe.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Marcas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marcas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produtos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Material = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Medidas = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cor = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Preco = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    ImagemUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Destaque = table.Column<bool>(type: "bit", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    MarcaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produtos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Produtos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Produtos_Marcas_MarcaId",
                        column: x => x.MarcaId,
                        principalTable: "Marcas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Ativo", "Nome", "Slug" },
                values: new object[,]
                {
                    { 1, true, "Estantes", "estantes" },
                    { 2, true, "Mesas", "mesas" },
                    { 3, true, "Gaveteiros", "gaveteiros" },
                    { 4, true, "Organizadores", "organizadores" }
                });

            migrationBuilder.InsertData(
                table: "Marcas",
                columns: new[] { "Id", "Ativo", "Nome" },
                values: new object[] { 1, true, "Agraplast" });

            migrationBuilder.InsertData(
                table: "Produtos",
                columns: new[] { "Id", "Ativo", "CategoriaId", "Codigo", "Cor", "Descricao", "Destaque", "ImagemUrl", "MarcaId", "Material", "Medidas", "Nome", "Preco" },
                values: new object[,]
                {
                    { 1, true, 1, "AGR-001", "Variadas", "Produto Agraplast ideal para organização da casa.", true, null, 1, "Plástico", "Consultar", "Estante Organizadora", null },
                    { 2, true, 3, "AGR-002", "Variadas", "Gaveteiro prático para quartos, escritórios e áreas de serviço.", true, null, 1, "Plástico", "Consultar", "Gaveteiro Multiuso", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_CategoriaId",
                table: "Produtos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Produtos_MarcaId",
                table: "Produtos",
                column: "MarcaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produtos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Marcas");
        }
    }
}
