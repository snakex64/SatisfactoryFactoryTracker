using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFT.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexMineOutputs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MineOutputs_MineId",
                table: "MineOutputs");

            migrationBuilder.CreateIndex(
                name: "IX_MineOutputs_MineId_ResourceId",
                table: "MineOutputs",
                columns: new[] { "MineId", "ResourceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MineOutputs_MineId_ResourceId",
                table: "MineOutputs");

            migrationBuilder.CreateIndex(
                name: "IX_MineOutputs_MineId",
                table: "MineOutputs",
                column: "MineId");
        }
    }
}
