using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PioneerLigan.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedSpellingError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SuperArcheType",
                table: "Decks",
                newName: "SuperArchType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SuperArchType",
                table: "Decks",
                newName: "SuperArcheType");
        }
    }
}
