using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFT.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class MovePurityToMiningStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MiningStations_MineId_MinerMk_OverclockLevel",
                table: "MiningStations");

            migrationBuilder.DropColumn(
                name: "NodePurity",
                table: "Mines");

            migrationBuilder.AddColumn<int>(
                name: "NodePurity",
                table: "MiningStations",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_MiningStations_MineId_MinerMk_OverclockLevel_NodePurity",
                table: "MiningStations",
                columns: new[] { "MineId", "MinerMk", "OverclockLevel", "NodePurity" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MiningStations_MineId_MinerMk_OverclockLevel_NodePurity",
                table: "MiningStations");

            migrationBuilder.DropColumn(
                name: "NodePurity",
                table: "MiningStations");

            migrationBuilder.AddColumn<int>(
                name: "NodePurity",
                table: "Mines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MiningStations_MineId_MinerMk_OverclockLevel",
                table: "MiningStations",
                columns: new[] { "MineId", "MinerMk", "OverclockLevel" },
                unique: true);
        }
    }
}
