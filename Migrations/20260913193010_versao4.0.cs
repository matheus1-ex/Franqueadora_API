using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Franqueada.API.Migrations
{
    /// <inheritdoc />
    public partial class versao40 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Usuários");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Usuários",
                type: "TEXT",
                nullable: true);
        }
    }
}
