using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApiBancoDeDadosETestes.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdministrador : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Admnistradores",
                columns: new[] { "Id", "Email", "Perfil", "Senha" },
                values: new object[] { 1, "Administrador@teste.com", "Admin", "123456" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Admnistradores",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
