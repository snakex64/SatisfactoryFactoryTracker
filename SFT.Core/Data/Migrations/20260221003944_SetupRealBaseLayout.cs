using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFT.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class SetupRealBaseLayout : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Factories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FactoryLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FactoryId = table.Column<int>(type: "integer", nullable: false),
                    Identifier = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoryLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactoryLevels_Factories_FactoryId",
                        column: x => x.FactoryId,
                        principalTable: "Factories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ResourceId = table.Column<int>(type: "integer", nullable: false),
                    OutputPerMinute = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mines_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactoryOutputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FactoryLevelId = table.Column<int>(type: "integer", nullable: false),
                    ResourceId = table.Column<int>(type: "integer", nullable: false),
                    AmountPerMinute = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoryOutputs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FactoryOutputs_FactoryLevels_FactoryLevelId",
                        column: x => x.FactoryLevelId,
                        principalTable: "FactoryLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactoryOutputs_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FactoryInputs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FactoryLevelId = table.Column<int>(type: "integer", nullable: false),
                    ResourceId = table.Column<int>(type: "integer", nullable: false),
                    AmountPerMinute = table.Column<decimal>(type: "numeric", nullable: false),
                    SourceMineId = table.Column<int>(type: "integer", nullable: true),
                    SourceFactoryLevelId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FactoryInputs", x => x.Id);
                    table.CheckConstraint("CK_FactoryInputs_Source", "(\"SourceMineId\" IS NULL) <> (\"SourceFactoryLevelId\" IS NULL)");
                    table.ForeignKey(
                        name: "FK_FactoryInputs_FactoryLevels_FactoryLevelId",
                        column: x => x.FactoryLevelId,
                        principalTable: "FactoryLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FactoryInputs_FactoryLevels_SourceFactoryLevelId",
                        column: x => x.SourceFactoryLevelId,
                        principalTable: "FactoryLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactoryInputs_Mines_SourceMineId",
                        column: x => x.SourceMineId,
                        principalTable: "Mines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FactoryInputs_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FactoryInputs_FactoryLevelId",
                table: "FactoryInputs",
                column: "FactoryLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryInputs_ResourceId",
                table: "FactoryInputs",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryInputs_SourceFactoryLevelId",
                table: "FactoryInputs",
                column: "SourceFactoryLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryInputs_SourceMineId",
                table: "FactoryInputs",
                column: "SourceMineId");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryLevels_FactoryId_Identifier",
                table: "FactoryLevels",
                columns: new[] { "FactoryId", "Identifier" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FactoryOutputs_FactoryLevelId",
                table: "FactoryOutputs",
                column: "FactoryLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryOutputs_ResourceId",
                table: "FactoryOutputs",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Mines_ResourceId",
                table: "Mines",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_Resources_Name",
                table: "Resources",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FactoryInputs");

            migrationBuilder.DropTable(
                name: "FactoryOutputs");

            migrationBuilder.DropTable(
                name: "Mines");

            migrationBuilder.DropTable(
                name: "FactoryLevels");

            migrationBuilder.DropTable(
                name: "Resources");

            migrationBuilder.DropTable(
                name: "Factories");
        }
    }
}
