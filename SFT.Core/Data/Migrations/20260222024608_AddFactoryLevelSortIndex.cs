using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SFT.Core.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFactoryLevelSortIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortIndex",
                table: "FactoryLevels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            // Assign sequential 0-based SortIndex values to any pre-existing rows,
            // partitioned per factory and ordered by Id (insertion order).
            migrationBuilder.Sql(@"
                UPDATE ""FactoryLevels"" AS fl
                SET ""SortIndex"" = sub.rn
                FROM (
                    SELECT ""Id"",
                           ROW_NUMBER() OVER (PARTITION BY ""FactoryId"" ORDER BY ""Id"") - 1 AS rn
                    FROM ""FactoryLevels""
                ) AS sub
                WHERE fl.""Id"" = sub.""Id"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortIndex",
                table: "FactoryLevels");
        }
    }
}
