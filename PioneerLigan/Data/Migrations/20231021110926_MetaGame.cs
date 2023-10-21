using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PioneerLigan.Data.Migrations
{
    /// <inheritdoc />
    public partial class MetaGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MetaGame",
                table: "LeagueEvent",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetaGame",
                table: "LeagueEvent");
        }
    }
}
