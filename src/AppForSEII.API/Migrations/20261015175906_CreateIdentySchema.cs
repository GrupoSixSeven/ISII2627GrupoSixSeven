using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppForSEII.API.Migrations
{
    /// <inheritdoc />
    public partial class CreateIdentySchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PistasReservadas_Pistas_PistaIdPista",
                table: "PistasReservadas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PistasReservadas",
                table: "PistasReservadas");

            migrationBuilder.DropIndex(
                name: "IX_PistasReservadas_PistaIdPista",
                table: "PistasReservadas");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "PistasReservadas");

            migrationBuilder.DropColumn(
                name: "PistaIdPista",
                table: "PistasReservadas");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PistasReservadas",
                table: "PistasReservadas",
                columns: new[] { "IdPista", "IdReserva" });

            migrationBuilder.AddForeignKey(
                name: "FK_PistasReservadas_Pistas_IdPista",
                table: "PistasReservadas",
                column: "IdPista",
                principalTable: "Pistas",
                principalColumn: "IdPista",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PistasReservadas_Pistas_IdPista",
                table: "PistasReservadas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PistasReservadas",
                table: "PistasReservadas");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "PistasReservadas",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "PistaIdPista",
                table: "PistasReservadas",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PistasReservadas",
                table: "PistasReservadas",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_PistasReservadas_PistaIdPista",
                table: "PistasReservadas",
                column: "PistaIdPista");

            migrationBuilder.AddForeignKey(
                name: "FK_PistasReservadas_Pistas_PistaIdPista",
                table: "PistasReservadas",
                column: "PistaIdPista",
                principalTable: "Pistas",
                principalColumn: "IdPista");
        }
    }
}
