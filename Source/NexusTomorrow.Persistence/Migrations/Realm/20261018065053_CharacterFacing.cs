using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusTomorrow.Persistence.Migrations.Realm
{
    /// <inheritdoc />
    public partial class CharacterFacing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "facing",
                schema: "realm",
                table: "character",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "facing",
                schema: "realm",
                table: "character");
        }
    }
}
