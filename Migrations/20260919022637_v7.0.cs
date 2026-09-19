using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Franqueada.API.Migrations
{
    /// <inheritdoc />
    public partial class v70 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PercentualRoyalty",
                table: "Franquias",
                type: "TEXT",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 5.0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentualRoyalty",
                table: "Franquias");
        }
    }
}
