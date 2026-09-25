using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cinema2026.Repo.Migrations
{
    /// <inheritdoc />
    public partial class addScreenings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Gamle test-billetter pegede paa film-id. Nu peger billetter paa afspilninger,
            // saa de gamle ville pege forkert - derfor slettes de her.
            migrationBuilder.Sql("DELETE FROM Tickets");

            migrationBuilder.DropColumn(
                name: "HallId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Tickets",
                newName: "ScreeningId");

            migrationBuilder.CreateTable(
                name: "Screenings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    HallId = table.Column<int>(type: "int", nullable: false),
                    StartsAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Screenings");

            migrationBuilder.RenameColumn(
                name: "ScreeningId",
                table: "Tickets",
                newName: "MovieId");

            migrationBuilder.AddColumn<int>(
                name: "HallId",
                table: "Movies",
                type: "int",
                nullable: true);
        }
    }
}
