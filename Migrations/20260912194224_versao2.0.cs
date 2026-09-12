using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Franqueada.API.Migrations
{
    /// <inheritdoc />
    public partial class versao20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Perfil",
                table: "Usuários");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Usuários",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Usuários",
                type: "TEXT",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Produtos",
                type: "TEXT",
                nullable: false,
                defaultValue: "Desativado",
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldDefaultValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Perfis",
                type: "TEXT",
                maxLength: 45,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Usuários");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Perfis");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Usuários",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<string>(
                name: "Perfil",
                table: "Usuários",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Produtos",
                type: "INTEGER",
                nullable: false,
                defaultValue: 2,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldDefaultValue: "Desativado");
        }
    }
}
