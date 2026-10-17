using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NexusTomorrow.Persistence.Migrations.Realm
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(SchemaConventions.CreateCaseInsensitiveCollation);

            migrationBuilder.EnsureSchema(
                name: "realm");

            migrationBuilder.CreateSequence(
                name: "character_id_seq",
                schema: "realm",
                incrementBy: 1000);

            migrationBuilder.CreateTable(
                name: "character",
                schema: "realm",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false),
                    account_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false, collation: "case_insensitive"),
                    creation_id = table.Column<long>(type: "bigint", nullable: false),
                    race = table.Column<byte>(type: "smallint", nullable: false),
                    sex = table.Column<byte>(type: "smallint", nullable: false),
                    @class = table.Column<byte>(name: "class", type: "smallint", nullable: false),
                    faction_id = table.Column<long>(type: "bigint", nullable: false),
                    path = table.Column<byte>(type: "smallint", nullable: false),
                    level = table.Column<long>(type: "bigint", nullable: false),
                    total_xp = table.Column<long>(type: "bigint", nullable: false),
                    world_id = table.Column<int>(type: "integer", nullable: false),
                    zone_id = table.Column<int>(type: "integer", nullable: false),
                    position_x = table.Column<float>(type: "real", nullable: false),
                    position_y = table.Column<float>(type: "real", nullable: false),
                    position_z = table.Column<float>(type: "real", nullable: false),
                    disabled = table.Column<bool>(type: "boolean", nullable: false),
                    requires_rename = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "character_bone",
                schema: "realm",
                columns: table => new
                {
                    character_id = table.Column<long>(type: "bigint", nullable: false),
                    bone_index = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_bone", x => new { x.character_id, x.bone_index });
                    table.ForeignKey(
                        name: "fk_character_bone_character_character_id",
                        column: x => x.character_id,
                        principalSchema: "realm",
                        principalTable: "character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "character_customization",
                schema: "realm",
                columns: table => new
                {
                    character_id = table.Column<long>(type: "bigint", nullable: false),
                    label = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_character_customization", x => new { x.character_id, x.label });
                    table.ForeignKey(
                        name: "fk_character_customization_character_character_id",
                        column: x => x.character_id,
                        principalSchema: "realm",
                        principalTable: "character",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_character_account_id",
                schema: "realm",
                table: "character",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_character_name_live",
                schema: "realm",
                table: "character",
                column: "name",
                unique: true,
                filter: "deleted_at IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "character_bone",
                schema: "realm");

            migrationBuilder.DropTable(
                name: "character_customization",
                schema: "realm");

            migrationBuilder.DropTable(
                name: "character",
                schema: "realm");

            migrationBuilder.DropSequence(
                name: "character_id_seq",
                schema: "realm");
        }
    }
}
