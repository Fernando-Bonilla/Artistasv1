using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Artistas.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoModeloEspectaculo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Espectaculos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Horario = table.Column<TimeOnly>(type: "time", nullable: false),
                    ArtistaId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Espectaculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Espectaculos_Artistas_ArtistaId",
                        column: x => x.ArtistaId,
                        principalTable: "Artistas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Espectaculos_ArtistaId",
                table: "Espectaculos",
                column: "ArtistaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Espectaculos");
        }
    }
}
