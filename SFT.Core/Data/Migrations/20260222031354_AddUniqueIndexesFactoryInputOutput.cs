using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFT.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexesFactoryInputOutput : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FactoryOutputs_FactoryLevelId",
                table: "FactoryOutputs");

            migrationBuilder.DropIndex(
                name: "IX_FactoryInputs_FactoryLevelId",
                table: "FactoryInputs");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryOutputs_FactoryLevelId_ResourceId",
                table: "FactoryOutputs",
                columns: new[] { "FactoryLevelId", "ResourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FactoryInputs_FactoryLevelId_ResourceId",
                table: "FactoryInputs",
                columns: new[] { "FactoryLevelId", "ResourceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FactoryOutputs_FactoryLevelId_ResourceId",
                table: "FactoryOutputs");

            migrationBuilder.DropIndex(
                name: "IX_FactoryInputs_FactoryLevelId_ResourceId",
                table: "FactoryInputs");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryOutputs_FactoryLevelId",
                table: "FactoryOutputs",
                column: "FactoryLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_FactoryInputs_FactoryLevelId",
                table: "FactoryInputs",
                column: "FactoryLevelId");
        }
    }
}
