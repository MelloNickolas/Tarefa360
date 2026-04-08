using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto360.Repository.Migrations
{
    public partial class AtualizarIDUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "ID",
                keyValue: -1);

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "ID", "Ativo", "Email", "Nome", "RefreshToken", "RefreshTokenExpiracao", "Senha", "TipoUsuario" },
                values: new object[] { 1, true, "admin.tarefa360@gmail.com", "Administrador", null, null, "$2a$11$3sdIJY8dQDuZIGPR7uMqIuhg.g9Npz/u9VAYvooyccmHN/s2nnDe2", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "ID", "Ativo", "Email", "Nome", "RefreshToken", "RefreshTokenExpiracao", "Senha", "TipoUsuario" },
                values: new object[] { -1, true, "admin.tarefa360@gmail.com", "Administrador", null, null, "$2a$11$p2pVD8BJegQOpFHkZfX6QeAhqdmt94VQy326qIHRWSikWPX2SRdvC", 1 });
        }
    }
}
