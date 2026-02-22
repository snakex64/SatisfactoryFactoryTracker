using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFT.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMinerMkAndNodePurity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MiningStations_MineId_OverclockLevel",
                table: "MiningStations");

            migrationBuilder.DropColumn(
                name: "OutputPerMinute",
                table: "Mines");

            migrationBuilder.AddColumn<int>(
                name: "MinerMk",
                table: "MiningStations",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "NodePurity",
                table: "Mines",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_MiningStations_MineId_MinerMk_OverclockLevel",
                table: "MiningStations",
                columns: new[] { "MineId", "MinerMk", "OverclockLevel" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MiningStations_MineId_MinerMk_OverclockLevel",
                table: "MiningStations");

            migrationBuilder.DropColumn(
                name: "MinerMk",
                table: "MiningStations");

            migrationBuilder.DropColumn(
                name: "NodePurity",
                table: "Mines");

            migrationBuilder.AddColumn<decimal>(
                name: "OutputPerMinute",
                table: "Mines",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_MiningStations_MineId_OverclockLevel",
                table: "MiningStations",
                columns: new[] { "MineId", "OverclockLevel" },
                unique: true);
        }
    }
}
