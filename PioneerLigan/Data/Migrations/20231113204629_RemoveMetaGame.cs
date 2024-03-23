using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PioneerLigan.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMetaGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Player",
                table: "Player");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeagueEvent",
                table: "LeagueEvent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_League",
                table: "League");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventResult",
                table: "EventResult");

            migrationBuilder.RenameTable(
                name: "Player",
                newName: "Players");

            migrationBuilder.RenameTable(
                name: "LeagueEvent",
                newName: "LeagueEvents");

            migrationBuilder.RenameTable(
                name: "League",
                newName: "Leagues");

            migrationBuilder.RenameTable(
                name: "EventResult",
                newName: "EventResults");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeagueEvents",
                table: "LeagueEvents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Leagues",
                table: "Leagues",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventResults",
                table: "EventResults",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DeckNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckNames", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckNames");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Leagues",
                table: "Leagues");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LeagueEvents",
                table: "LeagueEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventResults",
                table: "EventResults");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Player");

            migrationBuilder.RenameTable(
                name: "Leagues",
                newName: "League");

            migrationBuilder.RenameTable(
                name: "LeagueEvents",
                newName: "LeagueEvent");

            migrationBuilder.RenameTable(
                name: "EventResults",
                newName: "EventResult");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Player",
                table: "Player",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_League",
                table: "League",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LeagueEvent",
                table: "LeagueEvent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventResult",
                table: "EventResult",
                column: "Id");
        }
    }
}
