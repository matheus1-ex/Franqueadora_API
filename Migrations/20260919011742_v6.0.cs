using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Franqueada.API.Migrations
{
    /// <inheritdoc />
    public partial class v60 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Produtos_ProdutoId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Unidades_UnidadeId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_Unidades_Franquias_FranquiaID",
                table: "Unidades");

            migrationBuilder.DropIndex(
                name: "IX_Estoques_ProdutoId_UnidadeId",
                table: "Estoques");

            migrationBuilder.DropColumn(
                name: "EstoqueMinimo",
                table: "Estoques");

            migrationBuilder.RenameColumn(
                name: "FranquiaID",
                table: "Unidades",
                newName: "FranquiaId");

            migrationBuilder.RenameIndex(
                name: "IX_Unidades_FranquiaID",
                table: "Unidades",
                newName: "IX_Unidades_FranquiaId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Franqueadoras",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estoques_ProdutoId",
                table: "Estoques",
                column: "ProdutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Produtos_ProdutoId",
                table: "Estoques",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id_Produto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Unidades_UnidadeId",
                table: "Estoques",
                column: "UnidadeId",
                principalTable: "Unidades",
                principalColumn: "Id_Und",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Unidades_Franquias_FranquiaId",
                table: "Unidades",
                column: "FranquiaId",
                principalTable: "Franquias",
                principalColumn: "Id_Franquia",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Produtos_ProdutoId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoques_Unidades_UnidadeId",
                table: "Estoques");

            migrationBuilder.DropForeignKey(
                name: "FK_Unidades_Franquias_FranquiaId",
                table: "Unidades");

            migrationBuilder.DropIndex(
                name: "IX_Estoques_ProdutoId",
                table: "Estoques");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Franqueadoras");

            migrationBuilder.RenameColumn(
                name: "FranquiaId",
                table: "Unidades",
                newName: "FranquiaID");

            migrationBuilder.RenameIndex(
                name: "IX_Unidades_FranquiaId",
                table: "Unidades",
                newName: "IX_Unidades_FranquiaID");

            migrationBuilder.AddColumn<int>(
                name: "EstoqueMinimo",
                table: "Estoques",
                type: "INTEGER",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.CreateIndex(
                name: "IX_Estoques_ProdutoId_UnidadeId",
                table: "Estoques",
                columns: new[] { "ProdutoId", "UnidadeId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Produtos_ProdutoId",
                table: "Estoques",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id_Produto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoques_Unidades_UnidadeId",
                table: "Estoques",
                column: "UnidadeId",
                principalTable: "Unidades",
                principalColumn: "Id_Und",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Unidades_Franquias_FranquiaID",
                table: "Unidades",
                column: "FranquiaID",
                principalTable: "Franquias",
                principalColumn: "Id_Franquia",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
