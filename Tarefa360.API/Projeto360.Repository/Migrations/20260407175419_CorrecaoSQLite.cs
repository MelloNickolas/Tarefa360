using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projeto360.Repository.Migrations
{
    public partial class CorrecaoSQLite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Projetos",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projetos", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 254, nullable: false),
                    Senha = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Ativo = table.Column<bool>(type: "INTEGER", nullable: false),
                    TipoUsuario = table.Column<int>(type: "INTEGER", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: true),
                    RefreshTokenExpiracao = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Historias",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ProjetoID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historias", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Historias_Projetos_ProjetoID",
                        column: x => x.ProjetoID,
                        principalTable: "Projetos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sprints",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DataInicio = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DataFim = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProjetoID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sprints", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Sprints_Projetos_ProjetoID",
                        column: x => x.ProjetoID,
                        principalTable: "Projetos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TwoFactorTokens",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UsuarioID = table.Column<int>(type: "INTEGER", nullable: false),
                    Codigo = table.Column<string>(type: "TEXT", maxLength: 6, nullable: false),
                    Expiracao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Utilizado = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwoFactorTokens", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TwoFactorTokens_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuariosEquipes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PapeisEquipe = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: false),
                    EquipeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosEquipes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UsuariosEquipes_Equipes_EquipeId",
                        column: x => x.EquipeId,
                        principalTable: "Equipes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsuariosEquipes_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tarefas",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Titulo = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Estimativa = table.Column<decimal>(type: "TEXT", nullable: true),
                    TipoTarefa = table.Column<int>(type: "INTEGER", nullable: false),
                    Concluido = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "GETDATE()"),
                    DataConclusao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProjetoID = table.Column<int>(type: "INTEGER", nullable: false),
                    HistoriaID = table.Column<int>(type: "INTEGER", nullable: false),
                    SprintID = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarefas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tarefas_Historias_HistoriaID",
                        column: x => x.HistoriaID,
                        principalTable: "Historias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tarefas_Projetos_ProjetoID",
                        column: x => x.ProjetoID,
                        principalTable: "Projetos",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tarefas_Sprints_SprintID",
                        column: x => x.SprintID,
                        principalTable: "Sprints",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tarefas_Usuarios_UsuarioID",
                        column: x => x.UsuarioID,
                        principalTable: "Usuarios",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "ID", "Ativo", "Email", "Nome", "RefreshToken", "RefreshTokenExpiracao", "Senha", "TipoUsuario" },
                values: new object[] { -1, true, "admin.tarefa360@gmail.com", "Administrador", null, null, "$2a$11$p2pVD8BJegQOpFHkZfX6QeAhqdmt94VQy326qIHRWSikWPX2SRdvC", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Historias_ProjetoID",
                table: "Historias",
                column: "ProjetoID");

            migrationBuilder.CreateIndex(
                name: "IX_Sprints_ProjetoID",
                table: "Sprints",
                column: "ProjetoID");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_HistoriaID",
                table: "Tarefas",
                column: "HistoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_ProjetoID",
                table: "Tarefas",
                column: "ProjetoID");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_SprintID",
                table: "Tarefas",
                column: "SprintID");

            migrationBuilder.CreateIndex(
                name: "IX_Tarefas_UsuarioID",
                table: "Tarefas",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_TwoFactorTokens_UsuarioID",
                table: "TwoFactorTokens",
                column: "UsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Email",
                table: "Usuarios",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEquipes_EquipeId",
                table: "UsuariosEquipes",
                column: "EquipeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosEquipes_UsuarioId",
                table: "UsuariosEquipes",
                column: "UsuarioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarefas");

            migrationBuilder.DropTable(
                name: "TwoFactorTokens");

            migrationBuilder.DropTable(
                name: "UsuariosEquipes");

            migrationBuilder.DropTable(
                name: "Historias");

            migrationBuilder.DropTable(
                name: "Sprints");

            migrationBuilder.DropTable(
                name: "Equipes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Projetos");
        }
    }
}
