using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PioneerLigan.Data.Migrations
{
    /// <inheritdoc />
    public partial class MetaGameNewNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckNames");

            migrationBuilder.DropColumn(
                name: "MetaGame",
                table: "LeagueEvents");

            migrationBuilder.CreateTable(
                name: "MetaGames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaGames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MetaGames_LeagueEvents_LeagueEventId",
                        column: x => x.LeagueEventId,
                        principalTable: "LeagueEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuperArcheType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ColorAffiliation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetaGameId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decks_MetaGames_MetaGameId",
                        column: x => x.MetaGameId,
                        principalTable: "MetaGames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Decks_MetaGameId",
                table: "Decks",
                column: "MetaGameId");

            migrationBuilder.CreateIndex(
                name: "IX_MetaGames_LeagueEventId",
                table: "MetaGames",
                column: "LeagueEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "MetaGames");

            migrationBuilder.AddColumn<string>(
                name: "MetaGame",
                table: "LeagueEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
