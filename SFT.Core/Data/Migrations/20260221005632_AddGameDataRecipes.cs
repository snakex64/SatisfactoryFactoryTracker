using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SFT.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGameDataRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Resources",
                type: "text",
                nullable: false,
                defaultValue: "unknown");

            migrationBuilder.AddColumn<string>(
                name: "KeyName",
                table: "Resources",
                type: "text",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE "Resources"
                SET "KeyName" = lower(replace("Name", ' ', '-'))
                WHERE "KeyName" IS NULL;
                """);

            migrationBuilder.AlterColumn<string>(
                name: "KeyName",
                table: "Resources",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ProductionRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KeyName = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    CraftTimeSeconds = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionRecipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductionRecipeResources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductionRecipeId = table.Column<int>(type: "integer", nullable: false),
                    ResourceId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    IsInput = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionRecipeResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionRecipeResources_ProductionRecipes_ProductionRecip~",
                        column: x => x.ProductionRecipeId,
                        principalTable: "ProductionRecipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductionRecipeResources_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resources_KeyName",
                table: "Resources",
                column: "KeyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRecipeResources_ProductionRecipeId_ResourceId_IsI~",
                table: "ProductionRecipeResources",
                columns: new[] { "ProductionRecipeId", "ResourceId", "IsInput" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRecipeResources_ResourceId",
                table: "ProductionRecipeResources",
                column: "ResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionRecipes_KeyName",
                table: "ProductionRecipes",
                column: "KeyName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductionRecipeResources");

            migrationBuilder.DropTable(
                name: "ProductionRecipes");

            migrationBuilder.DropIndex(
                name: "IX_Resources_KeyName",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "KeyName",
                table: "Resources");
        }
    }
}
